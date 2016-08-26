using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    // Object level keys
    public class FString
    {
        private StringKey _sk;
        
        /// <summary>
        /// Creates fused string
        /// </summary>
        /// <param name="key">Key</param>
        public FString(long key)
        {
            _sk = StringKey.FindCreate(key);
            StringKey.AddString(_sk); // Adds globally
        }

        /// <summary>
        /// Creates fused string
        /// </summary>
        /// <param name="s">String</param>
        public FString(string s)
        {
            _sk = StringKey.FindCreate(s);
            StringKey.AddString(_sk); // Adds globally
        }

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

        /// <summary>
        /// Gets string key
        /// </summary>
        public long Key { get { return _sk.Key; } }

        /// <summary>
        /// Gets or sets string value
        /// </summary>
        public string Value
        {
            get
            {
                return _sk.GetValue();
            }
            set
            {
                if (value == null) return;
                _sk = StringKey.FindCreate(value);
                StringKey.AddString(_sk); // Adds globally
            }
        }
    }
}
