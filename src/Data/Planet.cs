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
        private double _orbitRadius;
        private double _weight;
        private double _axisPeriod;
        private double _orbitPeriod;
        private Star _parent;

        public Planet(): base() { }

        public override SpaceObject GetCopy() {
            Planet copy = new Planet();
            copy.Name = Name;
            copy.Position = Position;
            copy.Parent = Parent;
            copy.Radius = Radius;
            copy.OrbitRadius = OrbitRadius;
            copy.AxisPeriod = AxisPeriod;
            copy.OrbitPeriod = OrbitPeriod;
            copy.Weight = Weight;
            copy.Universe = Universe;
            return copy;
        }

        public override void Destroy() {
            if (_parent != null) {
                _parent.Children.Remove(this);
            }
        }

        public override void Save() {
            if (_parent != null) {
                _parent.Children.Add(this);
            }
        }

        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public double OrbitRadius
        {
            get { return _orbitRadius; }
            set { _orbitRadius = value; }
        }

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public double AxisPeriod
        {
            get { return _axisPeriod; }
            set { _axisPeriod = value; }
        }

        public double OrbitPeriod
        {
            get { return _orbitPeriod; }
            set { _orbitPeriod = value; }
        }

        public SpaceObject Parent {
            get { return _parent; }
            set { _parent = value as Star; }
        }

        public string ParentBuffer { get; set; }
    }
}
