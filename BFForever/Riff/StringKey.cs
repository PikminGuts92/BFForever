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

        
        public static StringKey Find(long key)
        {
            return _strings[key];
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
            if (Exists(sk.Key))
            {
                // String key exists globally
                _strings[sk.Key].MergeWith(sk); // Merges strings
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
            if (Exists(sk.Key))
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
            if (Exists(key))
                return _strings[key];

            // Didn't find string key - Creates one
            return new StringKey(key);
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

        public static bool Exists(long key)
        {
            return _strings.ContainsKey(key);
        }

        private static long UniqueKey()
        {
            while (Exists(_lastNumber))
                _lastNumber++;

            return _lastNumber;
        }

        private void MergeWith(StringKey b)
        {
            if (this.Key != b.Key) return;

            // Overwrites old strings so long as the new one isn't null
            if (b._english != null) this._english = b._english;
            if (b._japanese != null) this._japanese = b._japanese;
            if (b._german != null) this._german = b._german;
            if (b._italian != null) this._italian = b._italian;
            if (b._spanish != null) this._spanish = b._spanish;
            if (b._french != null) this._french = b._french;
        }

        /// <summary>
        /// Gets string key
        /// </summary>
        public long Key { get { return _key; } }
    }
}

