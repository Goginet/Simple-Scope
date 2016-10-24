using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SkyVisual.Projections
{
    public class CylindricalProjection : SphereProjection
    {
        public CylindricalProjection() : base() { }

        public CylindricalProjection(double radius) : base(radius) {}

        public override Point FromSphereToPlace(Point point)
        {
            double x = point.X * Radius;
            double y = Radius + Radius * Math.Cos(point.Y);
            return new Point(x, y);
        }
    }
}
