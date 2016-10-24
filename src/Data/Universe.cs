using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Scope.Data {
    public class Universe: IEnumerable<SpaceObject>, INotifyCollectionChanged {
        private ObservableCollection<SpaceObject> _objects;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public Universe() {
            _objects = new ObservableCollection<SpaceObject>();
            _objects.CollectionChanged += Chenge;
        }

        public SpaceObject GetObjectByName(string name) {
            foreach(SpaceObject obj in _objects) {
                if (obj.Name == name) {
                    return obj;
                }
            }
            return null;
        }

        public IEnumerator<SpaceObject> GetEnumerator() {
            return ((IEnumerable<SpaceObject>)_objects).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<SpaceObject>)_objects).GetEnumerator();
        }

        public ObservableCollection<SpaceObject> Objects {
            get { return _objects; }
        }

        private void Chenge(object sender, NotifyCollectionChangedEventArgs e) {
            CollectionChanged?.Invoke(sender, e);
            if (e.Action == NotifyCollectionChangedAction.Add) {
                foreach(SpaceObject obj in e.NewItems) {
                    obj.Universe = this;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove) {
                foreach (SpaceObject obj in e.OldItems) {
                    obj.Destroy();
                }
            }
        }
    }
}
