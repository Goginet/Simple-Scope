﻿using SkyVisual.DrawingObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Simple_Scope.Data
{
    public abstract class SpaceObject
    {
        public virtual Universe Universe { get; set; }
        public virtual string Name { get; set; }
        private Point3D _position;
        
        public SpaceObject() { }

        public SpaceObject(string name, Point3D position) {
            Name = name;
            _position = position;
        }

        public Sky CreateSky(Vector3D normal) {
            return new Sky(_position, normal);
        }

        public virtual Point3D Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual double X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public virtual double Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        public virtual double Z
        {
            get { return _position.Z; }
            set { _position.Z = value; }
        }
        
        public virtual void Destroy() {
            Universe = null;
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual SpaceObject GetCopy() {
            return this.MemberwiseClone() as SpaceObject;
        }
    }
}