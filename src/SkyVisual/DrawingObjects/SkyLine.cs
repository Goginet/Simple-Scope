using SkyVisual.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SkyVisual.DrawingObjects
{
    public class SkyLine : SkyDrawingObject
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public double Thickness { get; set; }
        public SolidColorBrush Brush { get; set; }

        public SkyLine() : base() { }

        public override Drawing Draw(Projection projection)
        {
            // TODO: throws exeptions!!!!!!!!!!!!!!!!
            Pen pen = new Pen(Brush, Thickness);
            return new GeometryDrawing(Brush, pen, new LineGeometry(StartPoint, EndPoint));
        }
    }
}
