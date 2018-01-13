using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    // Global strings
    internal class StringKey
    {
        private static Dictionary<ulong, StringKey> _globalStrings = new Dictionary<ulong, StringKey>();
        private readonly ulong _key;
        private readonly Dictionary<Localization, string> _values;

        private StringKey(ulong key) : this(key, "")
        {

        }

        private StringKey(ulong key, string value)
        {
            _key = key;

            // Adds localized strings
            _values = new Dictionary<Localization, string>
            {
                { Localization.English, value },
                { Localization.Japanese, value },
                { Localization.German, value },
                { Localization.Italian, value },
                { Localization.Spanish, value },
                { Localization.French, value }
            };
        }

        private void UpdateAllValues(string value)
        {
            var keys = _values.Keys.ToList();

            foreach(var k in keys)
                _values[k] = value;
        }
        
        public string this[Localization loc]
        {
            get => _values[loc];
            set => _values[loc] = value;
        }

        public ulong Key => _key;

        // String management
        internal static string GetValue(ulong key, Localization localization) => _globalStrings.ContainsKey(key) ? _globalStrings[key][localization] : null;

        internal static bool UpdateValue(ulong key, string value)
        {
            // Updates all values
            if (!_globalStrings.ContainsKey(key))
                _globalStrings.Add(key, new StringKey(key, value));

            StringKey sk = _globalStrings[key];
            sk.UpdateAllValues(value);
            return true;
        }

        internal static bool UpdateValue(ulong key, string value, Localization localization)
        {
            // Updates only specified localization
            if (!_globalStrings.ContainsKey(key))
                _globalStrings.Add(key, new StringKey(key));

            _globalStrings[key][localization] = value;
            return true;
        }
        
        internal static bool ContainsStringKey(ulong key) => _globalStrings.ContainsKey(key);
    }
}
