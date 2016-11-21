using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Data {
    [Serializable]
    public class Universe: ICollection<SpaceObject>, INotifyCollectionChanged {
        public Point3D Position { get; set; }
        private ObservableCollection<SpaceObject> _objects;
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public Universe() {
            _objects = new ObservableCollection<SpaceObject>();
            _objects.CollectionChanged += Chenge;
        }

        [OnDeserialized]
        internal void Init(StreamingContext context) {
            if (_objects != null) {
                _objects.CollectionChanged += Chenge;
            }
        }

        public SpaceObject GetObjectByName(string name) {
            foreach(SpaceObject obj in _objects) {
                if (obj.Name == name) {
                    return obj;
                }
            }
            return null;
        }

        public void InsertObjects(IEnumerable<SpaceObject> objects)
        {
            _objects = new ObservableCollection<SpaceObject>(objects);
            _objects.CollectionChanged += Chenge;
        }

        public void Chenge(object sender, NotifyCollectionChangedEventArgs e) {
            CollectionChanged?.Invoke(sender, e);
            if (e.Action == NotifyCollectionChangedAction.Add) {
                foreach (SpaceObject obj in e.NewItems) {
                    obj.Save();
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove) {
                foreach (SpaceObject obj in e.OldItems) {
                    obj.Destroy();
                }
            }
        }

        public IEnumerator<SpaceObject> GetEnumerator() {
            return ((IEnumerable<SpaceObject>)_objects).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<SpaceObject>)_objects).GetEnumerator();
        }

        public int Count {
            get {
                return ((ICollection<SpaceObject>)_objects).Count;
            }
        }

        public bool IsReadOnly {
            get {
                return ((ICollection<SpaceObject>)_objects).IsReadOnly;
            }
        }

        public void Add(SpaceObject item) {
            ((ICollection<SpaceObject>)_objects).Add(item);
        }

        public void Clear() {
            ((ICollection<SpaceObject>)_objects).Clear();
        }

        public bool Contains(SpaceObject item) {
            return ((ICollection<SpaceObject>)_objects).Contains(item);
        }

        public void CopyTo(SpaceObject[] array, int arrayIndex) {
            ((ICollection<SpaceObject>)_objects).CopyTo(array, arrayIndex);
        }

        public bool Remove(SpaceObject item) {
            return ((ICollection<SpaceObject>)_objects).Remove(item);
        }
    }
}
