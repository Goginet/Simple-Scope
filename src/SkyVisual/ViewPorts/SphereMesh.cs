using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SkyVisual.ViewPorts
{
    public delegate Point FromSphereToPlace(Point point);

    public class SphereMesh
    {
        public double Radius { get; set; } = 0;
        public int Meridians { get; set; } = 360;
        public int Parallels { get; set; } = 180;
        public Point3D Centre { get; set; } = new Point3D(0, 0, 0);
        public bool Reverse { get; set; } = false;

        public FromSphereToPlace FromSphereToPlace { get; set; }

        public SphereMesh() { }

        public SphereMesh(double radius) : this()
        {
            Radius = radius;
        }

        public SphereMesh(double radius, int meridians, int parallels) : this(radius)
        {
            Meridians = meridians;
            Parallels = parallels;
        }

        public SphereMesh(double radius, int meridians, int parallels, Point3D centre) : this(radius, meridians, parallels)
        {
            Centre = centre;
        }

        public MeshGeometry3D Mesh
        {
            get { return BuildSphereMesh(); }
        }

        public MeshGeometry3D BuildSphereMesh()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            Point[,] points = new Point[Parallels + 1, Meridians + 1];

            for (int i = 0; i <= Parallels; i++)
            {
                for (int j = 0; j <= Meridians; j++)
                {
                    double delta = i * (180 / Parallels);
                    double alpha = j * (360 / Meridians);
                    points[i, j] = new Point(toRad(alpha), toRad(delta));
                }
            }

            for (int i = 0; i < Parallels; i++)
            {
                for (int j = 0; j < Meridians; j++)
                {
                    Point A = points[i, j];
                    Point B = points[i, j + 1];
                    Point C = points[i + 1, j + 1];
                    Point D = points[i + 1, j];
                    drawSquare(mesh, A, B, C, D);
                }
            }

            return mesh;
        }

        private void drawSquare(MeshGeometry3D mesh, Point A, Point B, Point C, Point D)
        {
            drawTriangle(mesh, A, B, C);
            drawTriangle(mesh, A, C, D);
        }

        private void drawTriangle(MeshGeometry3D mesh, Point A, Point B, Point C)
        {
            if (Reverse)
            {
                mesh.Positions.Add(getPoint3D(A));
                mesh.Positions.Add(getPoint3D(B));
                mesh.Positions.Add(getPoint3D(C));
                mesh.TextureCoordinates.Add(FromSphereToPlace(A));
                mesh.TextureCoordinates.Add(FromSphereToPlace(B));
                mesh.TextureCoordinates.Add(FromSphereToPlace(C));
            }
            else
            {
                mesh.Positions.Add(getPoint3D(C));
                mesh.Positions.Add(getPoint3D(B));
                mesh.Positions.Add(getPoint3D(A));
                mesh.TextureCoordinates.Add(FromSphereToPlace(C));
                mesh.TextureCoordinates.Add(FromSphereToPlace(B));
                mesh.TextureCoordinates.Add(FromSphereToPlace(A));
            }

            mesh.TriangleIndices.Add(mesh.TriangleIndices.Count);
            mesh.TriangleIndices.Add(mesh.TriangleIndices.Count);
            mesh.TriangleIndices.Add(mesh.TriangleIndices.Count);

        }

        private Point3D getPoint3D(Point point)
        {
            double x = Radius * Math.Sin(point.Y) * Math.Sin(point.X);
            double y = Radius * Math.Cos(point.Y);
            double z = Radius * Math.Sin(point.Y) * Math.Cos(point.X);

            return new Point3D(x, y, z);
        }

        private double toRad(double grad)
        {
            return (grad * Math.PI) / 180;
        }
    }
}
