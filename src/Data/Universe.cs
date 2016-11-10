using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Data {
    public class Universe: ICollection<SpaceObject>, INotifyCollectionChanged {
        public Point3D Position { get; set; }
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private List<SpaceObject> _objects;

        public Universe() {
            _objects = new List<SpaceObject>();
        }

        public SpaceObject GetObjectByName(string name) {
            foreach(SpaceObject obj in _objects) {
                if (obj.Name == name) {
                    return obj;
                }
            }
            return null;
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
            item.Save();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear() {
            foreach (SpaceObject obj in _objects) {
                obj.Destroy();
            }
            ((ICollection<SpaceObject>)_objects).Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(SpaceObject item) {
            return ((ICollection<SpaceObject>)_objects).Contains(item);
        }

        public void CopyTo(SpaceObject[] array, int arrayIndex) {
            ((ICollection<SpaceObject>)_objects).CopyTo(array, arrayIndex);
        }

        public bool Remove(SpaceObject item) {
            bool ok = ((ICollection<SpaceObject>)_objects).Remove(item);
            if (ok) {
                item.Destroy();
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            return ok;
        }

        public IEnumerator<SpaceObject> GetEnumerator() {
            return ((ICollection<SpaceObject>)_objects).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((ICollection<SpaceObject>)_objects).GetEnumerator();
        }
    }
}
