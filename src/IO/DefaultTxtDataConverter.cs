using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Scope.Data;

namespace Simple_Scope.IO
{
    public class DefaultTxtDataConverter : DataConverter
    {
        public static readonly char[] separator = { ',' };
        public static readonly int StarPropertiesCount = 7;
        public static readonly int PlanetPropertiesCount = 10;
        public static readonly int ConstellationPropertiesCount = 4;

        public SpaceObject ConvertFromString(Universe universe, string str)
        {
            SpaceObject obj = null;

            string[] fields = str.Split(separator);

            if (fields[0] == "Star" && fields.Length == StarPropertiesCount)
            {
                int i = 0;
                double x = 0, y = 0, z = 0, luminosity = 0, weight = 0;

                Star readStar = new Star();
                readStar.Name = fields[++i];
                Double.TryParse(fields[++i], out x);
                Double.TryParse(fields[++i], out y);
                Double.TryParse(fields[++i], out z);
                Double.TryParse(fields[++i], out luminosity);
                readStar.X = x;
                readStar.Y = y;
                readStar.Z = z;
                readStar.Luminosity = luminosity;
                readStar.Parent = universe.GetObjectByName(fields[++i]) as Constellation;

                obj = readStar;
            }
            else if (fields[0] == "Planet" && fields.Length == PlanetPropertiesCount)
            {
                int i = 0;
                double x = 0, y = 0, z = 0, orbitPeriod = 0, axisPeriod = 0,
                    orbitRadius = 0, radius = 0, weight = 0;

                Planet readPlanet = new Planet();
                readPlanet.Name = fields[++i];
                Double.TryParse(fields[++i], out x);
                Double.TryParse(fields[++i], out y);
                Double.TryParse(fields[++i], out z);
                Double.TryParse(fields[++i], out orbitPeriod);
                Double.TryParse(fields[++i], out axisPeriod);
                Double.TryParse(fields[++i], out orbitRadius);
                Double.TryParse(fields[++i], out radius);
                Double.TryParse(fields[++i], out weight);
                readPlanet.X = x;
                readPlanet.Y = y;
                readPlanet.Z = z;
                readPlanet.OrbitPeriod = orbitPeriod;
                readPlanet.AxisPeriod = axisPeriod;
                readPlanet.OrbitRadius = orbitRadius;
                readPlanet.Radius = radius;
                readPlanet.Weight = weight;
                readPlanet.Parent = universe.GetObjectByName(fields[++i]) as Star;

                obj = readPlanet;
            }
            else if (fields[0] == "Constellation" && fields.Length == ConstellationPropertiesCount)
            {
                int i = 0;
                double x = 0, y = 0, z = 0;

                Constellation readConstellation = new Constellation();
                readConstellation.Name = fields[++i];
                Double.TryParse(fields[++i], out x);
                Double.TryParse(fields[++i], out y);
                Double.TryParse(fields[++i], out z);
                readConstellation.X = x;
                readConstellation.Y = y;
                readConstellation.Z = z;

                obj = readConstellation;
            }

            return obj;
        }

        public string ConvertToString(SpaceObject obj)
        {
            string str = "";
            if (obj is Star)
            {
                Star star = obj as Star;
                const string type = "Star";
                str = type + " " +
                    star.Name + " " +
                    star.Position.X + " " +
                    star.Position.Y + " " +
                    star.Position.Z + " " +
                    star.Luminosity + " " +
                    star.Parent.ToString() + " ";
            }
            else if (obj is Planet)
            {
                Planet planet = obj as Planet;
                const string type = "Planet";
                str = type + " " +
                    planet.Name + " " +
                    planet.Position.X + " " +
                    planet.Position.Y + " " +
                    planet.Position.Z + " " +
                    planet.OrbitPeriod + " " +
                    planet.AxisPeriod + " " +
                    planet.OrbitRadius + " " +
                    planet.Radius + " " +
                    planet.Weight + " " +
                    planet.Parent.ToString() + " ";
            }
            else if (obj is Constellation)
            {
                Constellation constellation = obj as Constellation;
                const string type = "Constellation";
                str = type + " " +
                    constellation.Name + " " +
                    constellation.Position.X + " " +
                    constellation.Position.Y + " " +
                    constellation.Position.Z + " ";
            }
            return str;
        }
    }
}
