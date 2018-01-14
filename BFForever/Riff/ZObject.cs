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

namespace BFForever.Riff
{
    public abstract class ZObject
    {
        protected HKey _filePath;
        protected HKey _directoryPath;

        public ZObject(HKey filePath, HKey directoryPath)
        {
            _filePath = filePath;
            _directoryPath = directoryPath;
        }

        internal List<FString> GetAllStrings()
        {
            List<FString> strings = new List<FString>();

            // Only writes type, just like real BF data
            if (!(this is StringTable)) strings.Add(Type);

            AddMemberStrings(strings);
            return strings.Distinct().ToList();
        }

        protected abstract void AddMemberStrings(List<FString> strings);

        internal abstract void ReadData(AwesomeReader ar);

        internal void WriteData(AwesomeWriter aw)
        {
            aw.Write((ulong)_filePath);
            aw.Write((ulong)_directoryPath);
            aw.Write((ulong)Type);
            aw.BaseStream.Position += 8;

            WriteObjectData(aw);
        }

        protected abstract void WriteObjectData(AwesomeWriter aw);

        public HKey FilePath
        {
            get => _filePath ?? new HKey(null);
            set => _filePath = value;
        }

        public HKey DirectoryPath
        {
            get => _directoryPath ?? new HKey(null);
            set => _directoryPath = value;
        }

        public abstract HKey Type { get; } 
    }
}
