using Simple_Scope.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simple_Scope.IO {
    public class MyFormatter : IFormatter {
        public const int ConstellationFieldsCount = 2;
        public const int StarFieldsCount = 6;
        public const int PlanetFieldsCount = 8;
        public readonly char[] ConstellationSeparator = { ',' };
        public readonly char[] StarSeparator = { '$' };
        public readonly char[] PlanetSeparator = { '@' };
        public readonly char[] StarArraySeparator = { '&' };
        public readonly char[] PlanetArraySeparator = { ';' };

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

        public object Deserialize(Stream serializationStream) {
            Universe universe = new Universe();
            StreamReader reader = new StreamReader(serializationStream);
            HygCsvDataConverter converter = new HygCsvDataConverter();
            string line;
            while((line = reader.ReadLine()) != null) {
                string[] constellationFields = line.Split(ConstellationSeparator);
                if (constellationFields.Length != ConstellationFieldsCount) {
                    return null;
                }
                int i = 0;
                Constellation constellation = new Constellation();
                constellation.Universe = universe;
                constellation.Name = constellationFields[i++];
                string starsStr = constellationFields[i++];
                string[] stars = starsStr.Split(StarArraySeparator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string starStr in stars) {
                    string[] starFields = starStr.Split(StarSeparator);
                    if (starFields.Length != StarFieldsCount) {
                        return null;
                    }
                    Star star = new Star();
                    star.Universe = universe;
                    int j = 0;
                    double x, y, z, luminosity;
                    star.Name = starFields[j++];
                    Double.TryParse(starFields[j++], out x);
                    Double.TryParse(starFields[j++], out y);
                    Double.TryParse(starFields[j++], out z);
                    Double.TryParse(starFields[j++], out luminosity);
                    star.X = x;
                    star.Y = y;
                    star.Z = z;
                    star.Luminosity = luminosity;
                    string planetsStr = starFields[j++];
                    string[] planets = planetsStr.Split(PlanetArraySeparator, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string planetStr in planets) {
                        string[] planetFields = planetStr.Split(PlanetSeparator);
                        if (planetFields.Length != PlanetFieldsCount) {
                            return null;
                        }
                        Planet planet = new Planet();
                        planet.Universe = universe;
                        int k = 0;
                        double xP, yP, zP, radius, orbitRadius, axisPeriod, orbitPeriod;
                        planet.Name = planetFields[k++];
                        Double.TryParse(planetFields[k++], out xP);
                        Double.TryParse(planetFields[k++], out yP);
                        Double.TryParse(planetFields[k++], out zP);
                        Double.TryParse(planetFields[k++], out radius);
                        Double.TryParse(planetFields[k++], out orbitRadius);
                        Double.TryParse(planetFields[k++], out axisPeriod);
                        Double.TryParse(planetFields[k++], out orbitPeriod);
                        planet.X = xP;
                        planet.Y = yP;
                        planet.Z = zP;
                        planet.Radius = radius;
                        planet.OrbitRadius = orbitRadius;
                        planet.AxisPeriod = axisPeriod;
                        planet.OrbitPeriod = orbitPeriod;
                        planet.Parent = star;
                        universe.Add(planet);
                    }
                    star.Parent = constellation;
                    universe.Add(star);
                }
                universe.Add(constellation);
            }
            return universe;
        }

        public void Serialize(Stream serializationStream, object graph) {
            Universe universe = graph as Universe;
            if (universe == null) {
                return;
            }
            using (StreamWriter writer = new StreamWriter(serializationStream)) {
                foreach (SpaceObject obj in universe) {
                    if (obj is Constellation) {
                        Constellation constellation = obj as Constellation;
                        string str = "";
                        str += constellation.Name + ",";
                        foreach (Star star in constellation.Children) {
                            str += star.Name + "$";
                            str += star.X + "$";
                            str += star.Y + "$";
                            str += star.Z + "$";
                            str += star.Luminosity + "$";
                            foreach (Planet planet in star.Children) {
                                str += planet.Name + "@";
                                str += planet.X + "@";
                                str += planet.Y + "@";
                                str += planet.Z + "@";
                                str += planet.Radius + "@";
                                str += planet.OrbitRadius + "@";
                                str += planet.AxisPeriod + "@";
                                str += planet.OrbitPeriod + ";";
                            }
                            str += "&";
                        }
                        writer.WriteLine(str);
                    }
                }
            }
        }

    }
}


//public void Serialize(Stream serializationStream, object graph) {
//    Type type = graph.GetType();
//    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//    foreach (FieldInfo info in fields) {
//        if (info.GetCustomAttribute(typeof(NonSerializedAttribute)) == null) {
//            if (info.FieldType is IEnumerable) {
//                IEnumerable arr = info.GetValue(graph) as IEnumerable;
//                foreach (object obj in arr) {
//                    WriteObject(serializationStream, obj);
//                }
//            } else {
//                WriteObject(serializationStream, info.GetValue(graph));
//            }
//        }
//    }
//}

//private void WriteObject(Stream stream, object obj) {
//    FieldInfo info = obj.GetType();
//    char[] value = (info.GetValue(obj).ToString() + ";").ToArray();
//    byte[] value2 = value.Select(c => (byte)c).ToArray();
//    stream.Write(value2, 0, value.Length);
//}