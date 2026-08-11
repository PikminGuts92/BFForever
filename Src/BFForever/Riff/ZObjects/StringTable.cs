using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/* 
 * Description:
 *  The string table lists contains all strings for a given zobject directory. Each are seperated by localization.
 *  
 *  Note: Entries are sorted by hash key values
 * 
 * StringTable ZObject
 * ===================
 * INT32 - Count of Entries
 * INT32 - Entries Offset (Always 12)
 * INT32 - Size of String Blob
 * INT32 - String Blob Offset
 * Entries[]
 * StringBlob
 * 
 * Entry (16 bytes)
 * ================
 *  SKEY - String Key
 * INT32 - Offset in String Table
 * INT32 - Always 0
 */

namespace BFForever.Riff
{
    public class StringTable : ZObject
    {
        private class LocalizationPair
        {
            private LocalizationPair(ulong key, Localization loc)
            {
                HashValue = key;
                EnumValue = loc;
            }

            public ulong HashValue { get; }
            public Localization EnumValue { get; }

            public static readonly LocalizationPair[] Localizations = new LocalizationPair[]
            {
                new LocalizationPair(Global.STbl_English, Localization.English),
                new LocalizationPair(Global.STbl_Japanese, Localization.Japanese),
                new LocalizationPair(Global.STbl_German, Localization.German),
                new LocalizationPair(Global.STbl_Italian, Localization.Italian),
                new LocalizationPair(Global.STbl_Spanish, Localization.Spanish),
                new LocalizationPair(Global.STbl_French, Localization.French)
            };
        }

        private readonly Localization _localization;
        Dictionary<ulong, string> _strings = new Dictionary<ulong, string>();

        public StringTable(HKey filePath, HKey directoryPath, Localization localization = Localization.English) : base(filePath, directoryPath)
        {
            _localization = localization;
            _strings = new Dictionary<ulong, string>();
        }

        internal static bool IsValidLocalization(HKey key) => LocalizationPair.Localizations.Count(x => x.HashValue == key.Key) != 0;
        internal static Localization GetLocalization(HKey key) => LocalizationPair.Localizations.FirstOrDefault(x => x.HashValue == key).EnumValue;
        internal static HKey GetHKey(Localization loc) => LocalizationPair.Localizations.FirstOrDefault(x => x.EnumValue == loc).HashValue;

        protected override void AddMemberStrings(List<FString> strings)
        {
            // It's not in English tables for whatever reason
            // if (Localization != Localization.English)
            //    strings.Add(DirectoryPath);
        }

        internal override void ReadData(AwesomeReader ar)
        {
            _strings.Clear();

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 12; // Skips to entries
            long stringTableOffset = ar.BaseStream.Position + (count * 16);

            ulong[] key = new ulong[count];
            long[] offset = new long[count];

            for (int i = 0; i < count; i++)
            {
                key[i] = ar.ReadUInt64();
                offset[i] = ar.ReadInt32();
                ar.BaseStream.Position += 4; // Should be zero
            }

            for (int i = 0; i < count; i++)
            {
                ar.BaseStream.Position = offset[i] + stringTableOffset;
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
                // Updates global string value
                StringKey.UpdateValue(d.Key, d.Value, _localization);
            }

            // English localization is missing directory path entry
            if (Localization == Localization.Japanese)
                StringKey.UpdateValue(DirectoryPath.Key, _strings[DirectoryPath.Key], Localization.English);
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            // Sorts by key value
            _strings = _strings.OrderBy(x => x.Key).ToDictionary(key => key.Key, value => value.Value);

            Dictionary<ulong, int> offsets;
            byte[] blob = CreateBlob(out offsets);

            aw.Write((int)_strings.Count);
            aw.Write((int)12);
            aw.Write((int)blob.Length);
            aw.Write((int)(_strings.Count * 16) + 4);

            foreach(var entry in _strings)
            {
                aw.Write((long)entry.Key);
                aw.Write((int)offsets[entry.Key]);
                aw.BaseStream.Position += 4;
            }

            aw.Write(blob);
        }

        private byte[] CreateBlob(out Dictionary<ulong, int> offsets)
        {
            offsets = new Dictionary<ulong, int>();
            byte[] nullByte = { 0x00 };

            using (MemoryStream ms = new MemoryStream())
            {
                foreach (var s in _strings)
                {
                    offsets.Add(s.Key, (int)ms.Position);

                    byte[] data = Encoding.UTF8.GetBytes(s.Value);
                    ms.Write(data, 0, data.Length);
                    ms.Write(nullByte, 0, nullByte.Length);
                }

                return ms.ToArray();
            }
        }

        public override HKey Type => GetHKey(_localization);
        public Localization Localization => _localization;
        public Dictionary<ulong, string> Strings => _strings;
    }
}
