using SkyVisual.DrawingObjects;
using SkyVisual.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SkyVisual.ViewPorts
{

    public class SphereViewPort : SkyViewPort
    {
        public double Radius { get; set; } = 0;
        public int Meridians { get; set; } = 360;
        public int Parallels { get; set; } = 180;
        public bool Reverse { get; set; } = false;
        public int FieldWidth { get; set; } = 100;
        public int FieldHight { get; set; } = 100;
        public Brush Background { get; set; }
        
        private Viewport3D view3d = new Viewport3D();

        public SphereViewPort() : base() { }

        public SphereViewPort(double radius) : this()
        {
            Radius = radius;
        }

        public Camera Camera
        {
            get
            {
                return view3d.Camera;
            }
            set
            {
                view3d.Camera = value;
            }
        }

        public override Projection Projection
        {
            get { return base.Projection; }
            set
            {
                if (value is SphereProjection)
                {
                    base.Projection = value;
                }
                else
                {
                    // TODO: throw exeption!!!
                }
            }
        }

        public override Visual GetPort(SkyDrawingObject[] objects)
        {
            return BuildSphere(objects);
        }

        private Visual BuildSphere(SkyDrawingObject[] objects)
        {
            if(Projection == null)
            {
                return null;
            }

            rebuild();

            SphereMesh mesh = new SphereMesh(Radius, Meridians, Parallels);
            mesh.FromSphereToPlace += (Projection as SphereProjection).FromSphereToPlace;
            mesh.Reverse = Reverse;

            DrawingBrush brush = new DrawingBrush(DrawField(objects));
            DiffuseMaterial material = new DiffuseMaterial(brush);

            GeometryModel3D model = new GeometryModel3D(mesh.Mesh, material);
            ModelUIElement3D element = new ModelUIElement3D();
            element.Model = model;

            CreateLight();
            
            view3d.Children.Add(element);

            return view3d;
        }

        private void rebuild()
        {
            Camera oldCam = view3d.Camera;
            view3d = new Viewport3D();
            view3d.Camera = oldCam;
        }

        private void CreateLight()
        {
            AmbientLight light = new AmbientLight(Colors.White);
            ModelVisual3D model = new ModelVisual3D();
            model.Content = light;
            view3d.Children.Add(model);
        }

        private Drawing DrawField(SkyDrawingObject[] objects)
        {
            DrawingGroup field = new DrawingGroup();

            field.Children.Add(DrawBackground());

            foreach (SkyDrawingObject obj in objects)
            {
                field.Children.Add(obj.Draw(Projection));
            }

            return field;
        }

        private Drawing DrawBackground()
        {
            Pen pen = new Pen(Background, 1);
            RectangleGeometry geometry = new RectangleGeometry(new Rect(0, 0, Radius * 4, Radius * 2));

            return new GeometryDrawing(Background, pen, geometry);
        }
    }
}
