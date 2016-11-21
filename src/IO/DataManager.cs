using Simple_Scope.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Scope.IO
{
    public class DataManager
    {
        private DataFile _dataFile;
        private Universe _data;
        private IFormatter formatter;

        public DataManager(DataFile dataFile)
        {
            _data = new Universe();
            _dataFile = dataFile;
            switch (_dataFile.Type) {
                case DataFile.FileType.Binary:
                    formatter = new MyBinaryFormatter();
                    break;
                case DataFile.FileType.Text:
                    formatter = new MyFormatter();
                    break;
                case DataFile.FileType.BinaryModify:
                    formatter = new BinaryFormatter();
                    break;
            }
        }

        public void LoadDataIn()
        {
            if (!File.Exists(_dataFile.DataFilePath)) {
                MessageBox.Show("File Not Found!");
                return;
            }
            Universe data = new Universe();
            using (Stream stream = new FileStream(_dataFile.DataFilePath, FileMode.Open)) {
                if (_dataFile.Compression) {
                    using (Stream decompress = new GZipStream(stream, CompressionMode.Decompress)) {
                        data = formatter.Deserialize(decompress) as Universe;       
                    }
                }  else {
                    data = formatter.Deserialize(stream) as Universe;
                }
            }
            if (data == null) {
                MessageBox.Show("Parse Error!");
            } else {
                _data = data;
            }
        }

        public void LoadDataOut()
        {
            using (Stream stream = new FileStream(_dataFile.DataFilePath, FileMode.Create)) {
                if (_dataFile.Compression) {
                    using (Stream compress = new GZipStream(stream, CompressionMode.Compress)) {
                        formatter.Serialize(compress, _data);
                    }
                } else {
                    formatter.Serialize(stream, _data);
                }
            }
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
    }
}
