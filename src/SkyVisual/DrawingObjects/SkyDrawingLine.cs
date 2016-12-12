using SkyVisual.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SkyVisual.DrawingObjects
{
    public class SkyDrawingLine : SkyDrawingObject
    {
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public double Thickness { get; set; }
        public SolidColorBrush Brush { get; set; }

        public SkyDrawingLine() : base() { }

        public override Drawing Draw(Projection projection)
        {
            // TODO: throws exeptions!!!!!!!!!!!!!!!!
            Pen pen = new Pen(Brush, Thickness);
            return new GeometryDrawing(Brush, pen, new LineGeometry(projection.Get2D(StartPoint), projection.Get2D(EndPoint)));
        }
    }
}
