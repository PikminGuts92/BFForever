using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Description:
 *  The zobject chunk is an internal file system entry. 
 * 
 * ZOBJECT CHUNK
 * =============
 *  HKEY - File Path
 *  HKEY - Directory Path
 *  HKEY - Type
 * INT64 - Always 0
 * BYTES - ZObject Data
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

        internal int Size() => CalculateSize() + 32;
        protected abstract int CalculateSize();
        internal abstract void ReadData(AwesomeReader ar);

        internal void WriteData(AwesomeWriter aw)
        {
            aw.Write((long)_filePath);
            aw.Write((long)_directoryPath);
            aw.Write((long)TypeKey);
            aw.BaseStream.Position += 8;

            WriteObjectData(aw);
        }

        protected abstract void WriteObjectData(AwesomeWriter aw);

        public HKey FilePath => _filePath;
        public HKey DirectoryPath => _directoryPath;
        protected abstract long TypeKey { get; } 
    }
}
