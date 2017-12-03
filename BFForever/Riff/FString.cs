using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BFForever.Riff
{
    internal class FStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(FString);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }

    // Object level keys
    [JsonConverter(typeof(FStringConverter))]
    public class FString
    {
        private long _globalKey;
        
        /// <summary>
        /// Creates fused string
        /// </summary>
        /// <param name="key">Key</param>
        public FString(long key)
        {
            StringKey _sk = StringKey.FindCreate(key);
            StringKey.AddString(_sk); // Adds globally
            _globalKey = _sk.Key;
        }

        /// <summary>
        /// Creates fused string
        /// </summary>
        /// <param name="s">String</param>
        public FString(string s)
        {
            StringKey _sk = StringKey.FindCreate(s);
            StringKey.AddString(_sk); // Adds globally
            _globalKey = _sk.Key;
        }

        #region Overloaded Operators
        /// <summary>
        /// Returns key value of fused string
        /// </summary>
        /// <param name="f">Fused string</param>
        public static implicit operator long(FString f)
        {
            return f.Key;
        }

        /// <summary>
        /// Returns English string of fused string
        /// </summary>
        /// <param name="f">Fused string</param>
        public static implicit operator string(FString f)
        {
            return f.Value;
        }

        public static implicit operator FString(string s)
        {
            return new FString(s);
        }

        public static implicit operator FString(long key)
        {
            return new FString(key);
        }

        public static bool operator ==(FString a, FString b)
        {
            return a._globalKey == b._globalKey;
        }

        public static bool operator !=(FString a, FString b)
        {
            return !(a == b);
        }

        public static bool operator ==(FString a, long b)
        {
            return a._globalKey == b;
        }

        public static bool operator !=(FString a, long b)
        {
            return !(a == b);
        }

        public static bool operator ==(long a, FString b)
        {
            return a == b._globalKey;
        }

        public static bool operator !=(long a, FString b)
        {
            return !(a == b);
        }

        public static bool operator ==(FString a, string b)
        {
            return a.Value == b;
        }

        public static bool operator !=(FString a, string b)
        {
            return !(a == b);
        }

        public static bool operator ==(string a, FString b)
        {
            return a == b.Value;
        }

        public static bool operator !=(string a, FString b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return ((FString)obj)._globalKey == _globalKey;
        }

        public override int GetHashCode()
        {
            return _globalKey.GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            StringKey sk = StringKey.Find(_globalKey);
            if (sk == null) return "???";

            // Base directory in string tables do not contain English strings for whatever reason
            string text = (sk.GetValue() == null) ? sk.GetValue(Language.Japanese) : sk.GetValue();
            return (text == null) ? "???" : text;
        }

        /// <summary>
        /// Gets string key
        /// </summary>
        public long Key { get { return _globalKey; } }

        /// <summary>
        /// Gets or sets string value (English)
        /// </summary>
        public string Value
        {
            get
            {
                return StringKey.Find(_globalKey)?.GetValue();
            }
            set
            {
                if (value == null) return;
                StringKey sk = StringKey.FindCreate(value);
                StringKey.AddString(sk); // Adds globally
                _globalKey = sk.Key;
            }
        }
    }
}
