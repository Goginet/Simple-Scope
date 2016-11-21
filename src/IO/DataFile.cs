using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Scope.IO
{
    public struct DataFile
    {
        public enum FileType {
            Binary,
            BinaryModify,
            Text
        }

        public string DataFilePath { get; set; }
        public FileType Type { get; set; }
        public bool Compression { get; set; }

        public DataFile(string dataFilePath, FileType type, bool compression)
        {
            DataFilePath = dataFilePath;
            Type = type;
            Compression = compression;
        }
    }
}
