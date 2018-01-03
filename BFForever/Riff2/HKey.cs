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
        public HKey(long key) : base(key)
        {

        }

        public HKey(string s) : base(s)
        {
            throw new NotImplementedException();
        }

        #region Overloaded Operators
        public static implicit operator long(HKey h)
        {
            return h.Key;
        }
        
        public static implicit operator string(HKey h)
        {
            return h.Value;
        }

        public static implicit operator HKey(string s)
        {
            return new HKey(s);
        }

        public static implicit operator HKey(long key)
        {
            return new HKey(key);
        }

        public static bool operator ==(HKey a, HKey b)
        {
            return a._globalKey == b._globalKey;
        }

        public static bool operator !=(HKey a, HKey b)
        {
            return !(a == b);
        }

        public static bool operator ==(HKey a, long b)
        {
            return a._globalKey == b;
        }

        public static bool operator !=(HKey a, long b)
        {
            return !(a == b);
        }

        public static bool operator ==(long a, HKey b)
        {
            return a == b._globalKey;
        }

        public static bool operator !=(long a, HKey b)
        {
            return !(a == b);
        }

        public static bool operator ==(HKey a, string b)
        {
            return a.Value == b;
        }

        public static bool operator !=(HKey a, string b)
        {
            return !(a == b);
        }

        public static bool operator ==(string a, HKey b)
        {
            return a == b.Value;
        }

        public static bool operator !=(string a, HKey b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        internal static long GetHash(string value) => (long)_global.Compute(value, true);
        internal static long GetHash(string value, long initial)
        {
            ulong origInit = _global.Initial, result;
            _global.Initial = (ulong)initial;
            
            result = _global.Compute(value, true);
            _global.Initial = origInit;
            return (long)result;
        }
    }
}
