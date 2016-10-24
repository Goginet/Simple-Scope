using Simple_Scope.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Data
{
    public class Sky {
        private Point3D _position;
        private Vector3D _normal;

        public Sky() { }

        public Sky(Point3D position, Vector3D normal) {
            _position = position;
            _normal = normal;
        }

        public Point3D Position {
            get { return Position; }
            set { _position = value; }
        }

        public Vector3D Normal {
            get { return _normal; }
            set { _normal = value; }
        }
    }
}
