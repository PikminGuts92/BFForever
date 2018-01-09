using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    // Globabl strings
    internal class StringKey
    {
        private static Dictionary<ulong, StringKey> _globalStrings = new Dictionary<ulong, StringKey>();
        private readonly ulong _key;
        private readonly Dictionary<Localization, string> _values;

        public StringKey(ulong key)
        {
            _key = key;

            // Adds localized strings
            _values = new Dictionary<Localization, string>();
            _values.Add(Localization.English, "");
            _values.Add(Localization.Japanese, "");
            _values.Add(Localization.German, "");
            _values.Add(Localization.Italian, "");
            _values.Add(Localization.Spanish, "");
            _values.Add(Localization.French, "");
        }
        
        public string GetValue(Localization loc) => _values[loc];
        public void SetValue(string value, Localization loc) => _values[loc] = value;

        public string this[Localization loc]
        {
            get => _values[loc];
            set => _values[loc] = value;
        }

        public ulong Key => _key;

        public static StringKey operator +(StringKey a, StringKey b)
        {
            if (a._key != b._key)
                throw new Exception("Strings must have matching keys to be merged!");

            StringKey c = new StringKey(a._key);

            var aNonEmpty = a._values.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x);
            var bNonEmpty = b._values.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x);

            foreach (var pair in aNonEmpty)
                c._values[pair.Key] = pair.Value;

            foreach (var pair in bNonEmpty)
                c._values[pair.Key] = pair.Value;

            return c;
        }

        // String management
        internal static string GetStringValue(ulong key) => _globalStrings.ContainsKey(key) ? _globalStrings[key][Localization.English] : null;

        internal static StringKey FindCreate(ulong key)
        {
            if (_globalStrings.ContainsKey(key)) return _globalStrings[key];

            StringKey sk = new StringKey(key);
            _globalStrings.Add(key, sk);
            return sk;
        }

        internal static void AddStringKey(StringKey sk) => _globalStrings.Add(sk.Key, sk);
        internal static bool ContainsStringKey(ulong key) => _globalStrings.ContainsKey(key);
    }
}
