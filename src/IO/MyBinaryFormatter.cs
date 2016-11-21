using Simple_Scope.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Scope.IO {
    class MyBinaryFormatter: IFormatter {

        public readonly byte EndFlag = 2;
        public readonly byte ContinueFlag = 3;

        public SerializationBinder Binder {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public StreamingContext Context {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public ISurrogateSelector SurrogateSelector {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }
        
        public object Deserialize(Stream serializationStream) {
            Universe universe = new Universe();
            try {
                using (BinaryReader reader = new BinaryReader(serializationStream)) {
                    while (reader.PeekChar() != -1) {
                        Constellation constellation = new Constellation();
                        constellation.Universe = universe;
                        constellation.Name = reader.ReadString();
                        while (reader.ReadByte() == ContinueFlag) {
                            Star star = new Star();
                            star.Universe = universe;
                            star.Name = reader.ReadString();
                            star.X = reader.ReadDouble();
                            star.Y = reader.ReadDouble();
                            star.Z = reader.ReadDouble();
                            star.Luminosity = reader.ReadDouble();
                            while (reader.ReadByte() == ContinueFlag) {
                                Planet planet = new Planet();
                                planet.Universe = universe;
                                planet.Name = reader.ReadString();
                                planet.X = reader.ReadDouble();
                                planet.Y = reader.ReadDouble();
                                planet.Z = reader.ReadDouble();
                                planet.Radius = reader.ReadDouble();
                                planet.OrbitRadius = reader.ReadDouble();
                                planet.AxisPeriod = reader.ReadDouble();
                                planet.OrbitPeriod = reader.ReadDouble();
                                planet.Parent = star;
                                universe.Add(planet);
                            }
                            star.Parent = constellation;
                            universe.Add(star);
                        }
                        universe.Add(constellation);
                    }
                }
            } catch (Exception e) {
                return null;
            }
            return universe;
        }

        public void Serialize(Stream serializationStream, object graph) {
            Universe universe = graph as Universe;
            if (universe == null) {
                return;
            }
            using (BinaryWriter writer = new BinaryWriter(serializationStream)) {

                foreach (SpaceObject obj in universe) {
                    if (obj is Constellation) {
                        Constellation constellation = obj as Constellation;
                        writer.Write(constellation.Name);
                        foreach (Star star in constellation.Children) {
                            writer.Write(ContinueFlag);
                            writer.Write(star.Name);
                            writer.Write(star.X);
                            writer.Write(star.Y);
                            writer.Write(star.Z);
                            writer.Write(star.Luminosity);
                            foreach(Planet planet in star.Children) {
                                writer.Write(ContinueFlag);
                                writer.Write(planet.Name);
                                writer.Write(planet.X);
                                writer.Write(planet.Y);
                                writer.Write(planet.Z);
                                writer.Write(planet.Radius);
                                writer.Write(planet.OrbitRadius);
                                writer.Write(planet.AxisPeriod);
                                writer.Write(planet.OrbitPeriod);
                            }
                            writer.Write(EndFlag);
                        }
                        writer.Write(EndFlag);
                    }
                }
            }
        }

    }
}
