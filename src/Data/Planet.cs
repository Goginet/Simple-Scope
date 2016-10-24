using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Data
{
    public class Planet: SpaceObject
    {
        private double _radius;
        private string _saveParentName;
        private double _orbitRadius;
        private double _weight;
        private double _axisPeriod;
        private double _orbitPeriod;
        private Star _parent;

        public Planet(): base() { }

        public Planet(string name, Point3D position): base(name, position) {
        }

        public override Universe Universe {
            set {
                base.Universe = value;
                Parent = _saveParentName;
            }
        }

        public double Radius {
            get { return _radius; }
            set { _radius = value; }
        }

        public double OrbitRadius {
            get { return _orbitRadius; }
            set { _orbitRadius = value; }
        }

        public double Weight {
            get { return _weight; }
            set { _weight = value; }
        }

        public double AxisPeriod {
            get { return _axisPeriod; }
            set { _axisPeriod = value; }
        }

        public double OrbitPeriod {
            get { return _orbitPeriod; }
            set { _orbitPeriod = value; }
        }
        

        public string Parent {
            set {
                if (Universe == null) {
                    _saveParentName = value;
                }
                else {
                    Star parent = (Universe.GetObjectByName(value) as Star);
                    if (_parent != null) {
                        _parent.Planets.Remove(this);
                    }
                    _parent = parent;
                    if (_parent != null) {
                        _parent.Planets.Add(this);
                    }
                }
            }
            get {
                if (_parent == null) {
                    return "";
                }
                return _parent.Name;
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_parent != null) {
                _parent.Planets.Remove(this);
            }
        }

        public override SpaceObject GetCopy()
        {
            Planet copy = new Planet();
            copy._parent = _parent;
            copy.Name = Name;
            copy.Position = Position;
            copy.Radius = Radius;
            copy.OrbitRadius = OrbitRadius;
            copy.AxisPeriod = AxisPeriod;
            copy.OrbitPeriod = OrbitPeriod;
            copy.Weight = Weight;
            copy._parent = _parent;
            return copy;
        }
    }
}
