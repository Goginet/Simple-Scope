using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Windows
{
    public class SphericalCamera: INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private Point _directionInGrad;
        private Point _mosePosition;
        private bool _cameraFix = true;
        private double _cameraSpeed;
        private Vector3D _direction;

        public SphericalCamera() { }

        public void Unfix(Point newPosition) {
            _mosePosition = newPosition;
            _cameraFix = false;
        }

        public void Fix() {
            _cameraFix = true;
        }

        public void Move(Point newPosition) {
            if (!_cameraFix) {
                double deltaX = (_mosePosition.X - newPosition.X) / _cameraSpeed;
                double deltaY = (_mosePosition.Y - newPosition.Y) / _cameraSpeed;
                _mosePosition = newPosition;

                _directionInGrad.X -= deltaX;
                _directionInGrad.Y += deltaY;

                if (_directionInGrad.X > 360 || _directionInGrad.X < 0) {
                    _directionInGrad.X = Math.Abs(_directionInGrad.X - 360);
                }
                if (_directionInGrad.Y > 360 || _directionInGrad.Y < 0) {
                    _directionInGrad.Y = Math.Abs(_directionInGrad.Y - 360);
                }

                _direction = GetDirection(_directionInGrad);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Direction"));
            }
        }

        public Point DirectionInGrad {
            get { return _directionInGrad; }
            set {
                _directionInGrad = value;
                _direction = GetDirection(_directionInGrad);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Direction"));
            }
        }

        public Vector3D Direction {
            get { return _direction; }
            set {
                _direction = value;
                _directionInGrad = GetDirectionInGrad(_direction);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Direction"));
            }
        }

        public double CameraSpeed {
            get { return _cameraSpeed; }
            set { _cameraSpeed = value; }
        }

        private Vector3D GetDirection(Point point) {
            double x = Math.Sin(toRad(point.Y)) * Math.Sin(toRad(point.X));
            double y = Math.Cos(toRad(point.Y));
            double z = Math.Sin(toRad(point.Y)) * Math.Cos(toRad(point.X));

            return new Vector3D(x, y, z);
        }

        private Point GetDirectionInGrad(Vector3D point) {
            double r = Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Z, 2));
            double R = Math.Sqrt(Math.Pow(point.Y, 2) + Math.Pow(r, 2));
            double y = Math.Acos(point.Y / R);
            double x = Math.Acos(point.Z / r);
            if (point.X < 0) {
                x = 2 * Math.PI - x;
            }
            return new Point(toGrad(x), toGrad(y));
        }

        private double toRad(double grad) {
            return (grad / 180) * Math.PI;
        }

        private double toGrad(double rad) {
            return (rad / Math.PI) * 180;
        }
    }
}
