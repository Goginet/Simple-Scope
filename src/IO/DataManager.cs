using Simple_Scope.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Scope.IO
{
    public class DataManager
    {
        private DataFile _dataFile;
        private Universe _data;

        public DataManager(DataFile dataFile)
        {
            _data = new Universe();
            _dataFile = dataFile;
        }

        public void LoadDataIn()
        {
            _data = new Universe();
            IEnumerable<SpaceObject> objects = readSpaceObjectsFromTextFile(_dataFile, _data);
            foreach (SpaceObject obj in objects) {
                _data.Add(obj);
            }
        }

        public void LoadDataOut()
        {
            writeSpaceObjectsToTextFile(_data, _dataFile, true);
        }

        public Universe Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public DataFile DataFile
        {
            get { return _dataFile; }
        }

        private static IEnumerable<SpaceObject> readSpaceObjectsFromTextFile(DataFile file, Universe universe)
        {
            List<SpaceObject> objects = new List<SpaceObject>();
            if (File.Exists(file.DataFilePath))
            {
                StreamReader reader = new StreamReader(file.DataFilePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    SpaceObject readObject = file.Converter.ConvertFromString(universe, line);
                    if (readObject != null)
                    {
                        readObject.Universe = universe;
                        objects.Add(readObject);
                    }
                }
                reader.Close();
            }

            return objects;
        }

        private static void writeSpaceObjectsToTextFile(IEnumerable<SpaceObject> data, DataFile file, bool reWrite)
        {
            StreamWriter writer = null;
            try {
                writer = new StreamWriter(new FileStream(file.DataFilePath, reWrite ? FileMode.Create : FileMode.OpenOrCreate));
            } catch (UnauthorizedAccessException e) {
                MessageBox.Show("Permission denied!");
            } catch (DirectoryNotFoundException e) {
                MessageBox.Show("Directory Not Found!");
            }
            if (writer == null) {
                return;
            }
            foreach (SpaceObject obj in data)
            {
                string line = file.Converter.ConvertToString(obj);
                writer.WriteLine(line);
            }
            writer.Close();
        }
    }
}
