using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BFForever.Riff
{
    public class FStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(FString);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;

            if (objectType == typeof(HKey))
                return new HKey(value);
            else if (objectType == typeof(FString))
                return new FString(value);
            else
                return new HKey("");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            serializer.Serialize(writer, ((FString)value).Value);
        }
    }

    [JsonConverter(typeof(FStringConverter))]
    public class FString
    {
        protected static readonly CRC64 _crc = new CRC64();
        protected ulong _key;

        public FString(ulong key)
        {
            _key = key;
        }
        
        public FString(string value)
        {
            if (!IsValidValue(value)) throw new Exception();
            _key = CalculateHash(value);

            if (_key == 0)
            {
                // Adds null if not already present
                if (!StringKey.ContainsStringKey(0)) StringKey.UpdateValue(0, "");
                return;
            }

            StringKey.UpdateValue(_key, value);
        }

        protected virtual bool IsValidValue(string value) => true;
        protected virtual string InvalidValueMessage() => "";
        protected virtual ulong CalculateHash(string value) => string.IsNullOrEmpty(value) ? 0 : _crc.Compute("#bFfStRiNg::" + value);

        public ulong Key => _key;
        public virtual string Value => StringKey.GetValue(_key, Localization.English);
        
        #region Overloaded Operators
        public static implicit operator ulong(FString f) => (f != default(FString)) ? f.Key : 0;
        public static implicit operator string(FString f) => f?.Value;
        public static implicit operator FString(string s) => new FString(s);
        public static implicit operator FString(ulong key) => new FString(key);
        
        public static bool operator ==(FString a, FString b)
        {
            if ((object)a == null && (object)b == null)
                return true;
            else if ((object)a == null || (object)b == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(FString a, FString b) => !(a == b);

        public override bool Equals(object obj) => (obj is FString) && ((FString)obj).Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
        #endregion

        public override string ToString() => Value ?? "0x" + Key.ToString("X16");
    }
}
