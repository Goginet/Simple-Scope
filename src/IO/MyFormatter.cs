using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Scope.IO {
    public class ObjectsBufferSave {

        private Queue<object> _objects;
        private HashSet<int> _refs;

        public ObjectsBufferSave() {
            _objects = new Queue<object>();
            _refs = new HashSet<int>();
        }

        public void AddObject(object obj) {
            if (!_refs.Contains(obj.GetHashCode())) {
                _refs.Add(obj.GetHashCode());
                _objects.Enqueue(obj);
            }
        }

        public object PopObject() {
            if (_objects.Count != 0) {
                return _objects.Dequeue();
            }
            return null;
        }
    }

    public class ObjectsBufferLoad {
        public int RootRef { get; set; }
        public List<Link> Links { get; set; }
        public Dictionary<int, object> Objects { get; set; }

        public ObjectsBufferLoad() {
            Objects = new Dictionary<int, object>();
            Links = new List<Link>();
        }

        public object RootObject {
            get {
                return Objects[RootRef];
            }
        }
    }

    public class Link {
        public object Obj { get; set; }
        public int RefIndex { get; set; }
    }

    public class LinkToField : Link {
        public FieldInfo Field { get; set; }
    }

    public class LinkToArrayEl : Link {
        public int[] Indexes { get; set; }
    }

    public class JsonOutputObject {
        public int RefIndex { get; set; }
        public object Object { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }

    public struct JsonArrayElement {
        public int[] Indexes { get; set; }
        public object Value { get; set; }
    }

    public class JsonFormatter : IFormatter {
        private const string fieldType = "type";
        private const string fieldRef = "ref";
        private const string fieldRoot = "root";
        private const string fieldBase = "base";
        private const string fieldArrayElements = "objects";
        private const string fieldArrayElIndex = "index";
        private const string fieldArrayElValue = "value";
        private const string fieldArraySize = "size";
        public const string refOperatorStr = "&ref_";

        public SerializationBinder Binder {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public StreamingContext Context {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public ISurrogateSelector SurrogateSelector {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public void Serialize(Stream serializationStream, object graph) {
            using (StreamWriter sw = new StreamWriter(serializationStream)) {
                ObjectsBufferSave objectsBuffer = new ObjectsBufferSave();
                objectsBuffer.AddObject(graph);
                using (JsonTextWriter writer = new JsonTextWriter(sw)) {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartObject();
                    writer.WritePropertyName(fieldRoot);
                    writer.WriteValue(graph.GetHashCode());
                    writer.WritePropertyName(fieldArrayElements);
                    writer.WriteStartArray();

                    object obj;
                    while ((obj = objectsBuffer.PopObject()) != null) {
                        Type objType = obj.GetType();
                        WriteObject(obj, false, objType, writer, objectsBuffer);
                    }

                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }
            }
        }

        public object Deserialize(Stream serializationStream) {
            ObjectsBufferLoad objectsBuffer = new ObjectsBufferLoad();
            JsonTextReader reader = new JsonTextReader(new StreamReader(serializationStream));

            while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                if (reader.TokenType == JsonToken.PropertyName) {
                    if (reader.Value.Equals(fieldRoot) && reader.Read()) {
                        objectsBuffer.RootRef = ToRefIndex(reader.Value);
                    } else if (reader.Value.Equals(fieldArrayElements) && reader.Read()) {
                        while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                            if (reader.TokenType == JsonToken.StartObject) {
                                JsonOutputObject outputObj = ReadJsonObject(reader);
                                objectsBuffer.Objects.Add(outputObj.RefIndex, outputObj.Object);
                                foreach (Link link in outputObj.Links) {
                                    objectsBuffer.Links.Add(link);
                                }
                            }
                        }
                    }
                }
            }

            foreach (Link link in objectsBuffer.Links) {
                object value;
                if (link is LinkToField) {
                    LinkToField linkToField = link as LinkToField;
                    if (objectsBuffer.Objects.TryGetValue(linkToField.RefIndex, out value)) {
                        try {
                            linkToField.Field.SetValue(link.Obj, value);
                        }
                        catch (Exception e) {

                        }
                    }
                } else if (link is LinkToArrayEl) {
                    LinkToArrayEl linkToArrayEl = link as LinkToArrayEl;
                    Array arr = linkToArrayEl.Obj as Array;
                    if (objectsBuffer.Objects.TryGetValue(link.RefIndex, out value)) {
                        try {
                            arr.SetValue(value, linkToArrayEl.Indexes);
                        } catch (Exception e) {

                        }
                    }
                }
            }

            return objectsBuffer.RootObject;
        }

        private void WriteObject(object obj, bool isBase, Type type, JsonTextWriter writer, ObjectsBufferSave objectsBuffer) {
            writer.WriteStartObject();

            writer.WritePropertyName(fieldType);
            writer.WriteValue(type.AssemblyQualifiedName);

            if (!type.IsValueType && !isBase) {
                writer.WritePropertyName(fieldRef);
                writer.WriteValue(obj.GetHashCode());
            }

            if (type.IsArray) {
                Array arr = (Array)obj;
                writer.WritePropertyName(fieldArraySize);
                if (arr.Rank > 1) {
                    writer.WriteStartArray();
                    for (int i = 0; i < arr.Rank; i++) {
                        writer.WriteValue(arr.GetLength(i));
                    }
                    writer.WriteEndArray();
                } else {
                    writer.WriteValue(arr.Length);
                }
                writer.WritePropertyName(fieldArrayElements);
                int[] indexes = new int[arr.Rank];
                WriteArray(writer, objectsBuffer, (Array)obj, arr.Rank - 1, indexes);
            }
            
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields) {
                if (field.GetCustomAttribute(typeof(NonSerializedAttribute)) == null) {
                    object value = field.GetValue(obj);
                    string name = field.Name;
                    Type fieldType = field.FieldType;

                    writer.WritePropertyName(name);

                    if (IsValueType(fieldType)) {
                        if (IsPrimitive(fieldType)) {
                            writer.WriteValue(value);
                        } else {
                            WriteObject(value, false, fieldType, writer, objectsBuffer);
                        }
                    } else {
                        if(value != null) {
                            int refIndex = value.GetHashCode();
                            writer.WriteValue(refOperatorStr + refIndex.ToString());
                            objectsBuffer.AddObject(value);
                        } else {
                            writer.WriteValue(refOperatorStr + "null");
                        }
                    }
                }
            }

            Type baseType = type.BaseType;
            if (baseType != null) {
                if (Attribute.GetCustomAttribute(baseType, typeof(SerializableAttribute)) != null) {
                    writer.WritePropertyName(fieldBase);
                    WriteObject(obj, true, baseType, writer, objectsBuffer);
                }
            }

            writer.WriteEndObject();
        }

        private void WriteArray(JsonTextWriter writer, ObjectsBufferSave objectsBuffer, Array arr, int dimension, int[] indexes) {
            writer.WriteStartArray();
            for (int i = 0; i < arr.GetLength(dimension); i++) {
                indexes[dimension] = i;
                if (dimension == 0) {
                    WriteArrayEl(writer, objectsBuffer, indexes, arr.GetValue(indexes));
                } else {
                    WriteArray(writer, objectsBuffer, arr, dimension - 1, indexes);
                }
            }
            writer.WriteEndArray();
        }

        private void WriteArrayEl(JsonTextWriter writer, ObjectsBufferSave objectsBuffer, int[] indexes, object value) {
            if (value != null) {
                writer.WriteStartObject();
                writer.WritePropertyName(fieldArrayElIndex);
                if (indexes.Length > 1) {
                    writer.WriteStartArray();
                    foreach (int index in indexes) {
                        writer.WriteValue(index);
                    }
                    writer.WriteEndArray();
                } else {
                    writer.WriteValue(indexes[0]);
                }
                writer.WritePropertyName(fieldArrayElValue);
                Type type = value.GetType();
                if (IsValueType(type)) {
                    if (IsPrimitive(type)) {
                        writer.WriteValue(value);
                    } else {
                        WriteObject(value, false, type, writer, objectsBuffer);
                    }
                } else {
                    int refIndex = value.GetHashCode();
                    writer.WriteValue(refOperatorStr + refIndex.ToString());
                    objectsBuffer.AddObject(value);
                }
                writer.WriteEndObject();
            }
        }

        private JsonOutputObject ReadJsonObject(JsonTextReader reader) {
            JsonOutputObject outputJsonObject = new JsonOutputObject();

            Dictionary<string, object> fields = Read(reader, ref outputJsonObject);
            
            string n = fields[fieldType].ToString();
            Type objType = Type.GetType(n);

            if (objType.IsArray) {
                CreateArray(fields, ref outputJsonObject);
            } else {
                CreateObject(fields, ref outputJsonObject);
            }

            return outputJsonObject;
        }

        private Dictionary<string, object> Read(JsonTextReader reader, ref JsonOutputObject outputJsonObject) {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            string key = null;
            while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                if (reader.TokenType == JsonToken.PropertyName) {
                    key = reader.Value.ToString();
                    if (key == fieldArraySize && reader.Read()) {
                        List<int> list = new List<int>();
                        if (reader.TokenType == JsonToken.StartArray) {
                            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                                list.Add((int)(long)reader.Value);
                            }
                        } else {
                            list.Add((int)(long)reader.Value);
                        }
                        
                        fields.Add(key, list);
                    }
                } else if (reader.TokenType == JsonToken.StartObject) {
                    if (key == fieldBase) {
                        fields.Add(key, Read(reader, ref outputJsonObject));
                    } else {
                        JsonOutputObject newObj = ReadJsonObject(reader);
                        outputJsonObject.Links.AddRange(newObj.Links);
                        fields.Add(key, newObj.Object);
                    }
                } else if (reader.TokenType == JsonToken.StartArray) {
                    List<JsonArrayElement> list = new List<JsonArrayElement>();
                    ReadArray(reader, list, ref outputJsonObject);
                    fields.Add(key, list);
                } else if (reader.Value != null) {
                    fields.Add(key, reader.Value);
                }
            }

            return fields;
        }

        private void ReadArray(JsonTextReader reader, List<JsonArrayElement> arr, ref JsonOutputObject outputJsonObject) {
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                if (reader.TokenType == JsonToken.StartObject) {
                    JsonArrayElement el = new JsonArrayElement();
                    while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                        if (reader.TokenType == JsonToken.PropertyName) {
                            if (reader.Value.Equals(fieldArrayElIndex) && reader.Read()) {
                                List<int> indexesList = new List<int>();
                                if (reader.TokenType == JsonToken.StartArray) {
                                    while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                                        indexesList.Add((int)(long)reader.Value);
                                    }
                                } else {
                                    indexesList.Add((int)(long)reader.Value);
                                }
                                el.Indexes = indexesList.ToArray();
                            } else if (reader.Value.Equals(fieldArrayElValue) && reader.Read()) {
                                if (reader.TokenType == JsonToken.StartObject) {
                                    JsonOutputObject newObj = ReadJsonObject(reader);
                                    outputJsonObject.Links.AddRange(newObj.Links);
                                    el.Value = newObj.Object;
                                } else {
                                    el.Value = reader.Value;
                                }
                            }
                        }
                    }
                    arr.Add(el);
                } else if (reader.TokenType == JsonToken.StartArray) {
                    ReadArray(reader, arr, ref outputJsonObject);
                }
            }
        }

        private void CreateObject(Dictionary<string, object> fields, ref JsonOutputObject outputJsonObject) {
            if (!fields.ContainsKey(fieldType)) {
                return;
            }

            if (fields.ContainsKey(fieldRef)) {
                outputJsonObject.RefIndex = ToRefIndex(fields[fieldRef]);
            } else {
                outputJsonObject.RefIndex = 0;
            }

            Type objType = Type.GetType(fields[fieldType].ToString());
            if (objType == null) {
                return;
            }

            object obj = Activator.CreateInstance(objType);

            foreach (string fieldName in fields.Keys) {
                if (!IsSpecialField(fieldName)) {
                    object value = fields[fieldName];
                    FieldInfo field = objType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (IsRef(value)) {
                        LinkToField newLink = new LinkToField();
                        newLink.Obj = obj;
                        newLink.Field = field;
                        newLink.RefIndex = ParseRef(value.ToString());
                        outputJsonObject.Links.Add(newLink);
                    } else {
                        field.SetValue(obj, ConvertValue(value, field.FieldType));
                    }
                }
            }

            if (fields.ContainsKey(fieldBase)) {
                ReadBase(obj, fields[fieldBase] as Dictionary<string, object>, ref outputJsonObject);
            }

            outputJsonObject.Object = obj;
        }

        private void ReadBase(object obj, Dictionary<string, object> fields, ref JsonOutputObject outputJsonObject) {
            Type objType = Type.GetType(fields[fieldType].ToString());

            foreach (string fieldName in fields.Keys) {
                if (!IsSpecialField(fieldName)) {
                    object value = fields[fieldName];
                    FieldInfo field = objType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (IsRef(value)) {
                        LinkToField newLink = new LinkToField();
                        newLink.Obj = obj;
                        newLink.Field = field;
                        newLink.RefIndex = ParseRef(value.ToString());
                        outputJsonObject.Links.Add(newLink);
                    } else {
                        field.SetValue(obj, ConvertValue(value, field.FieldType));
                    }
                }
            }
        }

        private void CreateArray(Dictionary<string, object> fields, ref JsonOutputObject outputJsonObject) {
            Type objType = Type.GetType(fields[fieldType].ToString());
            if (objType == null) {
                return;
            }

            if (fields.ContainsKey(fieldRef)) {
                outputJsonObject.RefIndex = ToRefIndex(fields[fieldRef]);
            } else {
                outputJsonObject.RefIndex = 0;
            }

            List<int> Length = fields[fieldArraySize] as List<int>;
            List<JsonArrayElement> values = fields[fieldArrayElements] as List<JsonArrayElement>;

            Type[] sizeTypes = new Type[Length.Count];
            object[] sizes = new object[Length.Count];
            for (int i = 0; i < sizeTypes.Length; i++) {
                sizeTypes[i] = typeof(Int32);
                sizes[i] = Length[i];
            }

            ConstructorInfo constructor = objType.GetConstructor(sizeTypes);
            Array arr = constructor.Invoke(sizes) as Array;
            foreach (JsonArrayElement value in values) {
                if (IsRef(value.Value)) {
                    LinkToArrayEl newLink = new LinkToArrayEl();
                    newLink.Obj = arr;
                    newLink.Indexes = value.Indexes;
                    newLink.RefIndex = ParseRef(value.Value.ToString());
                    outputJsonObject.Links.Add(newLink);
                } else {
                    arr.SetValue(value.Value, value.Indexes);
                }
            }

            outputJsonObject.Object = arr;
        }

        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////

        private bool IsValueType(Type type) {
            if (type.IsValueType || type == typeof(string)) {
                return true;
            }
            return false;
        }

        private bool IsPrimitive(Type type) {
            if (type.IsPrimitive || type == typeof(string)) {
                return true;
            }
            return false;
        }

        private object ConvertValue(object value, Type fieldType) {
            object newValue;

            if (fieldType == typeof(UInt64)) {
                newValue = (UInt64)(long)value;
            } else if (fieldType == typeof(UInt32)) {
                newValue = (UInt32)(long)value;
            } else if (fieldType == typeof(UInt16)) {
                newValue = (UInt16)(long)value;
            } else if (fieldType == typeof(Int64)) {
                newValue = (Int64)(long)value;
            } else if (fieldType == typeof(Int32)) {
                newValue = (Int32)(long)value;
            } else if (fieldType == typeof(Int16)) {
                newValue = (Int16)(long)value;
            } else {
                newValue = value;
            }

            return newValue;
        }

        private int ToRefIndex(object obj) {
            return (int)(long)obj;
        }

        private int ParseRef(string refStr) {
            string refIndexStr = refStr.Remove(0, refOperatorStr.Length);
            int refIndex = 0;
            if (Int32.TryParse(refIndexStr, out refIndex)) {
                return refIndex;
            }
            return 0;
        }

        private bool IsRef(object obj) {
            string str = obj as string;
            if (str != null) {
                return str.Contains(refOperatorStr);
            }
            return false;
        }

        private bool IsSpecialField(string fieldName) {
            if (fieldName == fieldType) {
                return true;
            } else if (fieldName == fieldRef) {
                return true;
            } else if (fieldName == fieldBase) {
                return true;
            }
            return false;
        }

    }
}