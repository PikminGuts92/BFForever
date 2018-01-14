using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    // Hierarchy Key - Globally unique
    public class HKey : FString
    {
        public HKey(ulong key) : base(key) { }
        public HKey(string value) : base(value) { }

        internal HKey Extend(string extension)
        {
            if (!IsValidValue(extension))
                throw new Exception(InvalidValueMessage());

            ulong newKey = HKey.GetHash(extension, _key);

            if (StringKey.ContainsStringKey(_key))
            {
                // Adds new value
                string newValue = StringKey.GetValue(_key, Localization.English) + extension;
                StringKey.UpdateValue(newKey, newValue);
                return new HKey(newKey);
            }
            
            return new HKey(newKey);
        }

        internal HKey GetParentDirectory() => string.IsNullOrEmpty(Value) ? "" : GetParentDirectory(Value);

        private string GetParentDirectory(string path)
        {
            int idx = path.LastIndexOf('.');
            if (idx < 0) return "";

            return path.Substring(0, idx);
        }

        internal HKey GetLastText() => string.IsNullOrEmpty(Value) ? "" : GetLastText(Value);

        private string GetLastText(string path)
        {
            int idx = path.LastIndexOf('.');
            if (idx < 0) return "";

            return path.Substring(idx + 1);
        }

        // TODO: Change this to regex expression
        protected override bool IsValidValue(string value) => value == null || !value.Any(x => !(char.IsLetterOrDigit(x) || x == '.' || x == '@' || x == '_' || x == '!'));
        protected override string InvalidValueMessage() => "Invalid HKey: May only contain alphanumerics or the symbols ('.', '_', '@', '!')";
        protected override ulong CalculateHash(string value) => string.IsNullOrEmpty(value) ? 0 : _crc.Compute(value, true);

        #region Overloaded Operators
        public static implicit operator ulong(HKey h) => (h != default(HKey)) ? h.Key : 0;
        public static implicit operator string(HKey h) => h?.Value;
        public static implicit operator HKey(string s) => new HKey(s);
        public static implicit operator HKey(ulong key) => new HKey(key);

        public static bool operator ==(HKey a, HKey b)
        {
            if ((object)a == null && (object)b == null)
                return true;
            else if ((object)a == null || (object)b == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(HKey a, HKey b) => !(a == b);

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => Key.GetHashCode();
        #endregion

        private static ulong GetHash(string value) => _crc.Compute(value, true);
        private static ulong GetHash(string value, ulong initial)
        {
            ulong origInit = _crc.Initial, result;

            _crc.Initial = initial ^ _crc.Final;
            result = _crc.Compute(value, true);

            _crc.Initial = origInit;
            return result;
        }
    }
}
