using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    // Hierarchy Key - Globally unique
    public class HKey : FString
    {
        public HKey(ulong key) : base(key)
        {

        }

        public HKey(string s) : base(GetHash(s))
        {
            //throw new NotImplementedException();
        }

        #region Overloaded Operators
        public static implicit operator ulong(HKey h) => h.Key;
        public static implicit operator string(HKey h) => h.Value;
        public static implicit operator HKey(string s) => new HKey(s);
        public static implicit operator HKey(ulong key) => new HKey(key);
        //public static implicit operator HKey(FString f) => new HKey(f.Value, f.Key);

        public static bool operator ==(HKey a, HKey b) => a.Key == b.Key;
        public static bool operator !=(HKey a, HKey b) => !(a == b);
        public static bool operator ==(HKey a, ulong b) => a.Key == b;
        public static bool operator !=(HKey a, ulong b) => !(a == b);
        public static bool operator ==(ulong a, HKey b) => a == b.Key;
        public static bool operator !=(ulong a, HKey b) => !(a == b);

        public static bool operator ==(HKey a, string b) => a.Value == b;
        public static bool operator !=(HKey a, string b) => !(a == b);
        public static bool operator ==(string a, HKey b) => a == b.Value;

        public static bool operator !=(string a, HKey b) => !(a == b);

        public override bool Equals(object obj) => (obj is HKey) && ((HKey)obj).Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
        #endregion

        internal static ulong GetHash(string value) => _global.Compute(value, true);
        internal static ulong GetHash(string value, long initial)
        {
            ulong origInit = _global.Initial, result;
            _global.Initial = (ulong)initial;
            
            result = _global.Compute(value, true);
            _global.Initial = origInit;
            return result;
        }
    }
}
