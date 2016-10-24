using SkyVisual;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using SkyVisual.DrawingObjects;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Simple_Scope.Data {
    public class Star: SpaceObject, IEnumerable<Planet>, ISkyDrawingObject
    {
        private ObservableCollection<Planet> _planets;
        private Constellation _parent;
        private string _saveParentName;
        private double _weight;
        private double _luminosity;
        private SkyDrawingObject _skyDrawing;

        public Star(): base() {
            _planets = new ObservableCollection<Planet>();
        }

        public Star(string name, Point3D position): base(name, position) {
            _planets = new ObservableCollection<Planet>();
        }

        public override SpaceObject GetCopy()
        {
            Star copy = new Star(Name, Position);
            foreach(Planet planet in Planets)
            {
                copy.Planets.Add(planet);
            }
            copy.Parent = Parent;
            copy.Weight = Weight;
            copy.Luminosity = Luminosity;
            copy.Universe = Universe;
            return copy;
        }

        public ObservableCollection<Planet> Planets {
            get { return _planets; }
        }

        public double Luminosity
        {
            get { return _luminosity; }
            set { _luminosity = value; }
        }

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public override Universe Universe {
            set {
                base.Universe = value;
                Parent = _saveParentName;
            }
        }

        public string Parent {
            set {
                if (Universe == null) {
                    _saveParentName = value;
                } else {
                    Constellation parent = (Universe.GetObjectByName(value) as Constellation);
                    if (_parent != null) {
                        _parent.Stars.Remove(this);
                    }
                    _parent = parent;
                    if (_parent != null) {
                        _parent.Stars.Add(this);
                    }
                }
            }
            get { return _parent.Name; }
        }

        public override void Destroy() {
            base.Destroy();
            _parent.Stars.Remove(this);
        }

        public IEnumerator<Planet> GetEnumerator() {
            return ((IEnumerable<Planet>)_planets).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<Planet>)_planets).GetEnumerator();
        }

        public SkyDrawingObject SkyDrawingObject
        {
            get
            {
                if(_skyDrawing == null) {
                    SkyDrawingPoint skyDrawing = new SkyDrawingPoint();
                    skyDrawing.Position = Position;
                    skyDrawing.Brush = new SolidColorBrush(Colors.White);
                    _skyDrawing = skyDrawing;
                }
                return _skyDrawing;
            }
        }
    }
}
