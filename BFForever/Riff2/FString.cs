using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class FString
    {
        protected long _globalKey;

        public FString(long key)
        {
            _globalKey = key;
        }

        public FString(string s)
        {
            throw new NotImplementedException();
        }

        public long Key => _globalKey;
        public virtual string Value
        {
            get => FEnvironment.GetStringValue(_globalKey);
            set
            {
                // TODO: Implement this
                throw new NotImplementedException();
            }
        }

        #region Overloaded Operators
        public static implicit operator long(FString f)
        {
            return f.Key;
        }
        
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
            if (obj is FString) return false;
            return ((FString)obj)._globalKey == _globalKey;
        }

        public override int GetHashCode()
        {
            return _globalKey.GetHashCode();
        }

        #endregion

        public override string ToString() => Value;
    }
}
