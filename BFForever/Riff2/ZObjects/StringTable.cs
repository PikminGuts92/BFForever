using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class StringTable : ZObject
    {
        private readonly Localization _localization;
        Dictionary<long, string> _strings = new Dictionary<long, string>();

        public StringTable(HKey filePath, HKey directoryPath, Localization localization = Localization.English) : base(filePath, directoryPath)
        {
            _localization = Localization;
            _strings = new Dictionary<long, string>();
        }

        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            _strings.Clear();

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 12; // Skips to entries
            long difference = ar.BaseStream.Position + (count * 16);

            long[] key = new long[count];
            long[] offset = new long[count];

            for (int i = 0; i < count; i++)
            {
                key[i] = ar.ReadInt64();
                offset[i] = ar.ReadInt32();
                ar.BaseStream.Position += 4; // Should be zero
            }

            for (int i = 0; i < count; i++)
            {
                ar.BaseStream.Position = offset[i] + difference;
                string text = ar.ReadNullString();

                if (!_strings.ContainsKey(key[i]))
                {
                    _strings.Add(key[i], text);
                    continue;
                }

                // Evidently the same string with different casings will have the same key (only for directory paths?)
                if (string.Compare(_strings[key[i]], text, true) == 0)
                {
                    _strings.Remove(key[i]);
                    _strings.Add(key[i], text);
                }
                else
                {
                    throw new Exception($"STRING ERROR: {_strings[key[i]]} != {text}");
                }
            }

            foreach (var d in _strings)
            {
                // Finds/adds key string globally
                StringKey sk = FEnvironment.FindCreate(d.Key);
                sk[_localization] = d.Value;
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            throw new NotImplementedException();
        }
        
        protected override long TypeKey {
            get
            {
                switch(_localization)
                {
                    default:
                    case Localization.English:
                        return Hashes.STbl_English;
                    case Localization.Japanese:
                        return Hashes.STbl_Japanese;
                    case Localization.German:
                        return Hashes.STbl_German;
                    case Localization.Italian:
                        return Hashes.STbl_Italian;
                    case Localization.Spanish:
                        return Hashes.STbl_Spanish;
                    case Localization.French:
                        return Hashes.STbl_French;
                }
            }
        }

        public Localization Localization => _localization;

        public Dictionary<long, string> Strings => _strings;
    }
}
