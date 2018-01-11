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

        // TODO: Change this to regex expression
        protected override bool IsValidValue(string value) => value == null || !value.Any(x => !(char.IsLetterOrDigit(x) || x == '.' || x == '@' || x == '_' || x == '!'));
        protected override string InvalidValueMessage() => "Invalid HKey: May only contain alphanumerics or the symbols ('.', '_', '@', '!')";
        protected override ulong CalculateHash(string value) => string.IsNullOrEmpty(value) ? 0 : _crc.Compute(value, true);

        #region Overloaded Operators
        public static implicit operator ulong(HKey h) => h.Key;
        public static implicit operator string(HKey h) => h.Value;
        public static implicit operator HKey(string s) => new HKey(s);
        public static implicit operator HKey(ulong key) => new HKey(key);

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
