using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Windows
{
    public class SphericalCamera
    {
        public double Scale { get; set; }
        public IInputElement RelativeTo { get; set; }

        private Point directionInGrad = new Point(0, 90);
        private Point mosePosition;
        private bool cameraFix = true;
        private double cameraSpeed;
        private const double cameraStep = 1000;

        public SphericalCamera() { }

        public SphericalCamera(double cameraSpeed, double scale, IInputElement relativeTo)
        {
            Scale = scale;
            CameraSpeed = cameraSpeed;
            RelativeTo = relativeTo;
        }

        public void Unfix(object sender, MouseButtonEventArgs e)
        {
            mosePosition = e.GetPosition(RelativeTo);
            cameraFix = false;
        }

        public void Fix(object sender, MouseButtonEventArgs e)
        {
            cameraFix = true;
        }

        public void Move(object sender, MouseEventArgs e)
        {
            if (!cameraFix)
            {
                Point newPosition = e.GetPosition(RelativeTo);

                double deltaX = (mosePosition.X - newPosition.X) / cameraSpeed;
                double deltaY = (mosePosition.Y - newPosition.Y) / cameraSpeed;
                mosePosition = newPosition;

                directionInGrad.X -= deltaX;
                directionInGrad.Y += deltaY;

                if (directionInGrad.X > 360 || directionInGrad.X < 0)
                {
                    directionInGrad.X = Math.Abs(directionInGrad.X - 360);
                }
                if (directionInGrad.Y > 360 || directionInGrad.Y < 0)
                {
                    directionInGrad.Y = Math.Abs(directionInGrad.Y - 360);
                }
            }
        }

        public Vector3D Direction
        {
            get { return GetDirection(directionInGrad); }
        }

        public Point DirectionInGrad
        {
            get { return directionInGrad; }
            set { directionInGrad = value; }
        }

        public double CameraSpeed
        {
            get { return (cameraStep * Scale) / cameraSpeed; }
            set { cameraSpeed = (cameraStep * Scale) / value; }
        }

        private Vector3D GetDirection(Point point)
        {
            double x = Scale * Math.Sin(toRad(point.Y)) * Math.Sin(toRad(point.X));
            double y = Scale * Math.Cos(toRad(point.Y));
            double z = Scale * Math.Sin(toRad(point.Y)) * Math.Cos(toRad(point.X));

            return new Vector3D(x, y, z);
        }

        private double toRad(double grad)
        {
            return (grad / 180) * Math.PI;
        }
    }
}
