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

        public ZObject(HKey filePath, HKey directoryPath)
        {
            _filePath = filePath;
            _directoryPath = directoryPath;
        }

        internal abstract void ReadData(AwesomeReader ar);

        public HKey FilePath => _filePath;
        public HKey DirectoryPath => _directoryPath;
        //public abstract ReadOnlyHKey Type { get; }
    }
}
