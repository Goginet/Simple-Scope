using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SkyVisual.Projections
{
    public abstract class Projection
    {
        public abstract Point Get2D(Point3D point);
    }
}
