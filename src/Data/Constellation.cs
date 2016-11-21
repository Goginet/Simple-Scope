using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Data {
    [Serializable]
    public class Constellation : SpaceObject, IEnumerable<Star> {
        private ObservableCollection<Star> _stars = new ObservableCollection<Star>();

        public Constellation() {
        }

        public override SpaceObject GetCopy() {
            Constellation copy = new Constellation();
            copy.Name = Name;
            copy.Position = Position;
            copy.Universe = Universe;
            foreach (Star star in Children) {
                copy.Children.Add(star);
            }
            return copy;
        }

        public override void Destroy() {
            foreach (Star child in Children) {
                if (child != null) {
                    child.Parent = null;
                }
            }
        }

        public override void Save() {
            foreach (Star child in Children) {
                if (child != null) {
                    child.Parent = this;
                }
            }
        }

        public void AddVector(Star starA, Star starB) {
            _stars.Add(starA);
            _stars.Add(starB);
        }

        public IEnumerator<Star> GetEnumerator() {
            return _stars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _stars.GetEnumerator();
        }

        public ICollection<Star> Children {
            get { return _stars; }
        }
    }
}
