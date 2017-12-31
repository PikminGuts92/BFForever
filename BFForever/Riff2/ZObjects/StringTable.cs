using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class StringTable : ZObject
    {
        private class LocalizationPair
        {
            private LocalizationPair(long key, Localization loc)
            {
                Key = key;
                EnumValue = loc;
            }

            public long Key { get; }
            public Localization EnumValue { get; }

            public static readonly LocalizationPair[] Localizations = new LocalizationPair[]
            {
                new LocalizationPair(Hashes.STbl_English, Localization.English),
                new LocalizationPair(Hashes.STbl_Japanese, Localization.Japanese),
                new LocalizationPair(Hashes.STbl_German, Localization.German),
                new LocalizationPair(Hashes.STbl_Italian, Localization.Italian),
                new LocalizationPair(Hashes.STbl_Spanish, Localization.Spanish),
                new LocalizationPair(Hashes.STbl_French, Localization.French)
            };
        }

        private readonly Localization _localization;
        Dictionary<long, string> _strings = new Dictionary<long, string>();

        public StringTable(HKey filePath, HKey directoryPath, Localization localization = Localization.English) : base(filePath, directoryPath)
        {
            _localization = Localization;
            _strings = new Dictionary<long, string>();
        }

        internal static bool IsValidLocalization(HKey key) => LocalizationPair.Localizations.Count(x => x.Key == key) != 0;
        internal static Localization GetLocalization(HKey key) => LocalizationPair.Localizations.FirstOrDefault(x => x.Key == key).EnumValue;
        internal static HKey GetHKey(Localization loc) => LocalizationPair.Localizations.FirstOrDefault(x => x.EnumValue == loc).Key;

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
            //throw new NotImplementedException();
        }

        protected override HKey Type => GetHKey(_localization);

        public Localization Localization => _localization;

        public Dictionary<long, string> Strings => _strings;
    }
}
