using SkyVisual.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SkyVisual.DrawingObjects
{
    public class SkyDrawingPoint : SkyDrawingObject
    {
        public double Radius { get; set; } = 15;
        public Point3D Position { get; set; }
        public SolidColorBrush Brush { get; set; }

        public SkyDrawingPoint() : base() { }

        public SkyDrawingPoint(double radius, Point3D position) : base()
        {
            Position = position;
            Radius = radius;
        }

        public override Drawing Draw(Projection projection)
        {
            double radiusX = Radius, radiusY = Radius;
            if (projection is SphereProjection)
            {
                radiusX = (projection as SphereProjection).GetLengthX(Position, Radius);
                radiusY = (projection as SphereProjection).GetLengthY(Position, Radius);
            }
            EllipseGeometry geometry = new EllipseGeometry(projection.Get2D(Position), radiusX, radiusY);
            return new GeometryDrawing(Brush, null, geometry);
        }
    }
}
