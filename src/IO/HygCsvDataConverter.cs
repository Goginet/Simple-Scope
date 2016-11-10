using Simple_Scope.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Scope.IO
{
    public class HygCsvDataConverter : DataConverter
    {
        public enum StarColumns {
            ID = 0,
            HIP = 1,
            HD = 2,
            HR = 3,
            GL = 4,
            BF = 5,
            Proper = 6,
            X = 17,
            Y = 18,
            Z = 19,
            Con = 29,
            Lum = 33,
        }
        public enum PlanetColumns {
            Name = 0,
            X = 1,
            Y = 2,
            Z = 3,
            Radius = 4,
            OrbitRadius = 5,
            AxisPeriod = 6,
            OrbitPeriod = 7,
            Weight = 8,
            Parent = 8,
        }

        public enum ConstellationColumns {
            Name = 0,
        }

        public static readonly char[] separator = { ',' };
        public static readonly int StarPropertiesCount = 37;
        public static readonly int PlanetPropertiesCount = 10;
        public static readonly int ConstellationPropertiesCount = 1;

        public SpaceObject ConvertFromString(Universe universe, string str)
        {
            SpaceObject obj = null;

            string[] fields = str.Split(separator);

            if (fields.Length == StarPropertiesCount)
            {
                StarHyg readStar = new StarHyg();

                int hip, hd, hr;
                Int32.TryParse(fields[(int)StarColumns.HIP], out hip);
                Int32.TryParse(fields[(int)StarColumns.HD], out hd);
                Int32.TryParse(fields[(int)StarColumns.HR], out hr);

                double luminosity, x, y, z;
                Double.TryParse(fields[(int)StarColumns.Lum], out luminosity);
                Double.TryParse(fields[(int)StarColumns.X], out x);
                Double.TryParse(fields[(int)StarColumns.Y], out y);
                Double.TryParse(fields[(int)StarColumns.Z], out z);

                readStar.Gl = fields[(int)StarColumns.GL];
                readStar.Bf = fields[(int)StarColumns.BF];
                readStar.Proper = fields[(int)StarColumns.Proper];
                readStar.ParentBuffer = fields[(int)StarColumns.Con];
                readStar.Hip = hip;
                readStar.Hd = hd;
                readStar.Hr = hr;
                readStar.X = x;
                readStar.Y = y;
                readStar.Z = z;
                readStar.Luminosity = luminosity;

                obj = readStar;
            } else if (fields.Length == PlanetPropertiesCount) {
                Planet planet = new Planet();
                
                double x, y, z, radius, orbitRadius, axisPeriod, orbitPeriod, weight;
                Double.TryParse(fields[(int)PlanetColumns.X], out x);
                Double.TryParse(fields[(int)PlanetColumns.Y], out y);
                Double.TryParse(fields[(int)PlanetColumns.Z], out z);
                Double.TryParse(fields[(int)PlanetColumns.Radius], out radius);
                Double.TryParse(fields[(int)PlanetColumns.OrbitRadius], out orbitRadius);
                Double.TryParse(fields[(int)PlanetColumns.AxisPeriod], out axisPeriod);
                Double.TryParse(fields[(int)PlanetColumns.OrbitPeriod], out orbitPeriod);
                Double.TryParse(fields[(int)PlanetColumns.Weight], out weight);

                planet.Name = fields[(int)PlanetColumns.Name];
                planet.X = x;
                planet.Y = y;
                planet.Z = z;
                planet.Radius = radius;
                planet.OrbitRadius = orbitRadius;
                planet.AxisPeriod = axisPeriod;
                planet.OrbitPeriod = orbitPeriod;
                planet.Weight = weight;
                planet.ParentBuffer = fields[(int)PlanetColumns.Parent];
                obj = planet;
            } else if (fields.Length == ConstellationPropertiesCount) {
                Constellation consteltaion = new Constellation();
                consteltaion.Name = fields[(int)ConstellationColumns.Name];
                obj = consteltaion;
            }

            return obj;
        }

        public string ConvertToString(SpaceObject obj)
        {
            string str = "";
            if (obj is Star)
            {
                StarHyg star = star = obj as StarHyg;
                if (!(obj is StarHyg)) {
                    star = new StarHyg();
                    Star starObj = obj as Star;
                    star.Proper = starObj.Name;
                    star.Position = starObj.Position;
                    star.Luminosity = starObj.Luminosity;
                    star.Parent = starObj.Parent;
                }
                str = "" + separator[0] +
                    star.Hip.ToString() + separator[0] +
                    star.Hd.ToString() + separator[0] +
                    star.Hr.ToString() + separator[0] +
                    star.Gl.ToString() + separator[0] +
                    star.Bf.ToString() + separator[0] +
                    star.Proper.ToString() + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    star.X.ToString() + separator[0] +
                    star.Y.ToString() + separator[0] +
                    star.Z.ToString() + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    ((star.Parent != null) ? star.Parent.Name:"") + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    star.Luminosity.ToString() + separator[0] +
                    "" + separator[0] +
                    "" + separator[0] +
                    "";
            } else if (obj is Planet) {
                Planet planet = obj as Planet;
                str = planet.Name.ToString() + separator[0] +
                    planet.X.ToString() + separator[0] +
                    planet.Y.ToString() + separator[0] +
                    planet.Z.ToString() + separator[0] +
                    planet.Radius.ToString() + separator[0] +
                    planet.OrbitRadius.ToString() + separator[0] +
                    planet.AxisPeriod.ToString() + separator[0] +
                    planet.OrbitPeriod.ToString() + separator[0] +
                    planet.Weight.ToString() + separator[0] +
                    ((planet.Parent != null) ? planet.Parent.Name : "");
            } else if (obj is Constellation) {
                Constellation constellation = obj as Constellation;
                str = constellation.Name.ToString();
            }

            return str;
        }
    }
}

