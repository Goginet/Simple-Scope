using SkyVisual.DrawingObjects;
using SkyVisual.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SkyVisual.ViewPorts
{
    public abstract class SkyViewPort
    {
        public virtual Projection Projection { get; set; }
        public abstract Visual GetPort(SkyDrawingObject[] objects);
    }
}
