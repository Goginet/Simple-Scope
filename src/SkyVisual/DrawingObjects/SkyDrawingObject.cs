using SkyVisual.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SkyVisual.DrawingObjects
{
    public abstract class SkyDrawingObject
    {
        public abstract Drawing Draw(Projection projection);
    }
}
