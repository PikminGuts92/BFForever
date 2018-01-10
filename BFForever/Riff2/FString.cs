using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
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
            if (_key == 0) return;

            StringKey.UpdateValue(_key, value);
        }

        protected virtual bool IsValidValue(string value) => true;
        protected virtual string InvalidValueMessage() => "";
        protected virtual ulong CalculateHash(string value) => string.IsNullOrEmpty(value) ? 0 : _crc.Compute("#bFfStRiNg::" + value);

        public ulong Key => _key;
        public virtual string Value => StringKey.GetValue(_key, Localization.English);

        #region Overloaded Operators
        public static implicit operator ulong(FString f) => f.Key;
        public static implicit operator string(FString f) => f.Value;
        public static implicit operator FString(string s) => new FString(s);
        public static implicit operator FString(ulong key) => new FString(key);

        public static bool operator ==(FString a, FString b) => a.Key == b.Key;
        public static bool operator !=(FString a, FString b) => !(a == b);
        public static bool operator ==(FString a, ulong b) => a.Key == b;
        public static bool operator !=(FString a, ulong b) => !(a == b);
        public static bool operator ==(ulong a, FString b) => a == b.Key;
        public static bool operator !=(ulong a, FString b) => !(a == b);

        public static bool operator ==(FString a, string b) => a.Value == b;
        public static bool operator !=(FString a, string b) => !(a == b);

        public static bool operator ==(string a, FString b) => a == b.Value;
        public static bool operator !=(string a, FString b) => !(a == b);

        public override bool Equals(object obj) => (obj is FString) && ((FString)obj).Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
        #endregion

        public override string ToString() => Value ?? "0x" + Key.ToString("X16");
    }
}
