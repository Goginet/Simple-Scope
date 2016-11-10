using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SkyVisual.Projections
{
    public abstract class SphereProjection : Projection
    {
        public double Radius { get; set; }

        public SphereProjection(): base() { }

        public SphereProjection(double radius) : this()
        {
            Radius = radius;
        }

        public sealed override Point Get2D(Point3D point)
        {
            double R = Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Z, 2));
            double R2 = Math.Sqrt(Math.Pow(R, 2) + Math.Pow(point.Y, 2));                                        
            double alpha = Math.Acos(point.Z / R);

            if (0 > point.X)
            {
                alpha = Math.PI + alpha;
            }
            
            double delta = Math.Acos(point.Y / R2);

            return FromSphereToPlace(new Point(alpha, delta));
        }

        public double GetLengthX(Point3D point, double length)
        {
            double R = Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Z, 2));
            double R2 = Math.Sqrt(Math.Pow(R, 2) + Math.Pow(point.Y, 2));

            double delta = Math.Acos(point.Y / R2);

            double r = Radius * Math.Sin(delta);
            
            double a = length / ( 2 * r);

            double tan = Math.Tan(a);

            return Radius * Math.Tan(a) * 2;
        }

        public double GetLengthY(Point3D point, double length)
        {
            double R = Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Z, 2));
            double R2 = Math.Sqrt(Math.Pow(R, 2) + Math.Pow(point.Y, 2));

            double delta = Math.Asin(point.Y / R2);

            double a = length / (2 * Radius);

            double L = Radius * Math.Tan(a) * 2;

            return L * Math.Cos(delta);
        }

        public abstract Point FromSphereToPlace(Point point);
    }
}
