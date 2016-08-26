using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    // Global strings
    public class StringKey
    {
        private readonly long _key;
        private string _english, _japanese, _german, _italian, _spanish, _french;
        private static Dictionary<long, StringKey> _strings = new Dictionary<long, StringKey>();
        private static long _lastNumber = 1210976091438049082; // Used as key assignment - Highest value found in BF

        /// <summary>
        /// Creates string key object
        /// </summary>
        /// <param name="key">Unique Key</param>
        private StringKey(long key)
        {
            _key = key; // Only set once!
        }

        public void SetValue(string s, Language lan = Language.English)
        {
            switch (lan)
            {
                case Language.Japanese:
                    _japanese = s;
                    break;
                case Language.German:
                    _german = s;
                    break;
                case Language.Italian:
                    _italian = s;
                    break;
                case Language.Spanish:
                    _spanish = s;
                    break;
                case Language.French:
                    _french = s;
                    break;
                default: // Language.English
                    _english = s;
                    break;
            }
        }

        public string GetValue(Language lan = Language.English)
        {
            switch (lan)
            {
                case Language.Japanese:
                    return _japanese;
                case Language.German:
                    return _german;
                case Language.Italian:
                    return _italian;
                case Language.Spanish:
                    return _spanish;
                case Language.French:
                    return _french;
                default: // Language.English
                    return _english;
            }
        }

        public static void AddString(StringKey sk)
        {
            if (Contains(sk.Key))
            {
                // String key exists globally
                StringKey newString = _strings[sk.Key] + sk; // Merges strings

                // Adds new string to global collection
                _strings.Remove(sk.Key);
                _strings.Add(newString.Key, newString);
            }
            else
            {
                // String key doesn't exist globally
                // - Adds to global collection
                _strings.Add(sk.Key, sk);
            }
        }

        public static void RemoveString(StringKey sk)
        {
            if (Contains(sk.Key))
                _strings.Remove(sk.Key);
        }

        /// <summary>
        /// Finds or creates string key object (Matching key)
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>String Key</returns>
        public static StringKey FindCreate(long key)
        {
            // Found string key
            if (Contains(key))
                return _strings[key];

            // Didn't find string key - Creates one
            return new StringKey(UniqueKey());
        }

        /// <summary>
        /// Finds or creates string key object (Matching string)
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>String Key</returns>
        public static StringKey FindCreate(string s)
        {
            StringKey sk = new StringKey(UniqueKey());
            sk._english = sk._japanese = sk._german = sk._italian = sk._spanish = sk._french = s;

            return sk;
        }

        public static bool Contains(long key)
        {
            return _strings.ContainsKey(key);
        }

        private static long UniqueKey()
        {
            while (Contains(_lastNumber))
                _lastNumber++;

            return _lastNumber;
        }

        public static StringKey operator+(StringKey a, StringKey b)
        {
            StringKey c = new StringKey(a.Key);

            // Copies values from a to b
            c._english = a._english;
            c._japanese = a._japanese;
            c._german = a._german;
            c._italian = a._italian;
            c._spanish = a._spanish;
            c._french = a._french;

            if (a.Key == b.Key)
            {
                // Overwrites old strings so long as the new one isn't null
                if (b._english != null) c._english = b._english;
                if (b._japanese != null) c._japanese = b._japanese;
                if (b._german != null) c._german = b._german;
                if (b._italian != null) c._italian = b._italian;
                if (b._spanish != null) c._spanish = b._spanish;
                if (b._french != null) c._french = b._french;
            }

            return c;
        }

        /// <summary>
        /// Gets string key
        /// </summary>
        public long Key { get { return _key; } }
    }
}

