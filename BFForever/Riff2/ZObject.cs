using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * ZOBJECT HEADER (32 bytes)
 * =========================
 *  HKEY - File Path
 *  HKEY - Directory Path
 *  HKEY - Type
 * INT64 - Always 0
 */

namespace BFForever.Riff2
{
    public abstract class ZObject
    {
        protected readonly HKey _filePath;
        protected readonly HKey _directoryPath;
        protected readonly HKey _type;

        public ZObject(HKey filePath, HKey directoryPath, HKey type)
        {
            _filePath = filePath;
            _directoryPath = directoryPath;
            _type = type;
        }

        internal abstract void ReadData(AwesomeReader ar, FEnvironment env);

        public HKey FilePath => _filePath;
        public HKey DirectoryPath => _directoryPath;
        public HKey Type => _type;
    }
}
