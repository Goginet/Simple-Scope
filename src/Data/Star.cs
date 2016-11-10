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

    

    public class Star : SpaceObject, IEnumerable<Planet>, ISkyDrawingObject {
        private ObservableCollection<Planet> _planets;
        private Constellation _parent;
        private string _saveParentName;
        private double _apparentMagnitude;
        private double _absoluteMagnitude;

        private SkyDrawingObject _skyDrawing;

        private double _luminosity;

        public Star() : base() {
            _planets = new ObservableCollection<Planet>();
        }

        public override SpaceObject GetCopy() {
            Star copy = new Star();
            copy.Name = Name;
            copy.Position = Position;
            foreach (Planet child in Children) {
                copy.Children.Add(child);
            }
            copy.Parent = Parent;
            copy._saveParentName = _saveParentName;
            copy.Luminosity = Luminosity;
            copy.Universe = Universe;
            return copy;
        }

        public override void Destroy() {
            foreach (Planet child in Children) {
                child.Parent = null;
            }
            if (_parent != null) {
                _parent.Children.Remove(this);
            }
        }

        public override void Save() {
            foreach (Planet child in Children) {
                child.Parent = this;
            }
            if (_parent != null) {
                _parent.Children.Add(this);
            }
        }

        public Constellation Parent {
            set { _parent = value; }
            get { return _parent; }
        }

        public ICollection<Planet> Children {
            get { return _planets; }
        }

        public virtual SkyDrawingObject SkyDrawingObject {
            get {
                if (_skyDrawing == null) {
                    double x = ApparentMagnitude;
                    double R = (-2 * x) + 12;
                    SkyDrawingPoint skyDrawing = new SkyDrawingPoint();
                    skyDrawing.Position = Position;
                    skyDrawing.Radius = R;
                    skyDrawing.Brush = new SolidColorBrush(Colors.White);
                    _skyDrawing = skyDrawing;
                }
                return _skyDrawing;
            }
        }

        public IEnumerator<Planet> GetEnumerator() {
            return ((IEnumerable<Planet>)_planets).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<Planet>)_planets).GetEnumerator();
        }

        public double Luminosity {
            get { return _luminosity; }
            set { _luminosity = value; }
        }

        public double ApparentMagnitude {
            get {
                double M = AbsoluteMagnitude;
                double d = Distance;
                double d0 = 32.616;
                double m = M + 5 * Math.Log10(d / d0);
                return m;
            }
        }

        public double AbsoluteMagnitude {
            get {
                double M0 = 4.83;
                double M = M0 - (Math.Log10(Luminosity) / 0.4);
                return M;
            }
        }
        
        public double Distance {
            get {
                double X0 = Universe.Position.X;
                double Y0 = Universe.Position.Y;
                double Z0 = Universe.Position.Z;
                return Math.Sqrt((Math.Pow(X - X0, 2) + Math.Pow(Y - Y0, 2) + Math.Pow(Z - Z0, 2))) * 3.26156;
            }
        }

        public string ParentBuffer { get; set; }
    }
}
