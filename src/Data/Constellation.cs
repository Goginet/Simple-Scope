using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Data
{
    public struct Vector<T>
    {
        public T PointA { get; set; }
        public T PointB { get; set; }

        public Vector(T pointA, T pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }
    }

    public class Constellation : SpaceObject, IEnumerable<Star>
    {
        private ObservableCollection<Star> _stars;
        private ObservableCollection<Vector<Star>> _vectors;

        public Constellation() {
            _stars = new ObservableCollection<Star>();
            _vectors = new ObservableCollection<Vector<Star>>();
        }

        public Constellation(string name): this() {
            Name = name;
        }

        public void AddVector(Star starA, Star starB) {
            _stars.Add(starA);
            _stars.Add(starB);
            _vectors.Add(new Vector<Star>(starA, starB));
        }

        public IEnumerator<Star> GetEnumerator() {
            return _stars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _stars.GetEnumerator();
        }

        public override string ToString() {
            return Name;
        }

        public override Point3D Position {
            get {
                if (_stars.Count == 0) {
                    return base.Position;
                }
                return _stars.ElementAt(0).Position;
            }
            set { base.Position = value; }
        }

        public ObservableCollection<Star> Stars {
            get { return _stars; }
        }

        public ObservableCollection<Vector<Star>> Vectors
        {
            get { return _vectors; }
        }

        public override SpaceObject GetCopy()
        {
            Constellation copy = new Constellation();
            copy.Name = Name;
            copy.Position = Position;
            copy.Universe = Universe;
            foreach(Star star in Stars)
            {
                copy.Stars.Add(star);
            }
            foreach (Vector<Star> vector in Vectors)
            {
                copy.Vectors.Add(vector);
            }
            return copy;
        }
    }
}
