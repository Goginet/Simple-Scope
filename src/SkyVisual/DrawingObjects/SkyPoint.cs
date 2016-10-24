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
        public double Radius { get; set; } = 0.2;
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
            // TODO: throws exeptions!!!!!!!!!!!!!!!!
            EllipseGeometry geometry = new EllipseGeometry(projection.Get2D(Position), Radius, Radius);
            return new GeometryDrawing(Brush, null, geometry);
        }
    }
}
