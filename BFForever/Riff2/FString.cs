using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class FString
    {
        protected static readonly CRC64 _global = new CRC64();
        protected ulong _key;

        public FString(ulong key)
        {
            _key = key;
        }
        
        public FString(string s)
        {
            ulong hash = _global.Compute("#bFfStRiNg::" + s);
            _key = hash;
            //_globalKey = _lastNumber++;
            
            if (StringKey.ContainsStringKey(_key)) return; // ?

            //StringKey sk = StringKey.FromString(s, _globalKey);
            //StringKey.AddStringKey(sk);
        }

        public ulong Key => _key;
        public virtual string Value => StringKey.GetStringValue(_key);

        #region Overloaded Operators
        public static implicit operator ulong(FString f) => f.Key;
        public static implicit operator string(FString f) => f.Value;
        public static implicit operator FString(string s) => new FString(s);
        public static implicit operator FString(ulong key) => new FString(key);
        //public static implicit operator FString(HKey h) => new FString(h.Value);

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

        public override string ToString() => Value;
    }
}
