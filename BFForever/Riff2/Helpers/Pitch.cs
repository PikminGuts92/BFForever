using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BFForever.Riff2
{
    public struct Pitch
    {
        private byte _value;
        private static readonly string[] _pitchesFlat = new string[] { "C", "D♭", "D", "E♭", "E", "F", "G♭", "G", "A♭", "A", "B♭", "B" };

        public Pitch(int value)
        {
            _value = 0;
            Value = value; // Ensures value remains in range
        }

        private Pitch(string s)
        {
            // TODO: Actually finish this (Stick to just flats)
            _value = 0;
            // Regex regex = new Regex(@"[A-F][b#]?[0-9]+", RegexOptions.IgnoreCase);
            Regex regex = new Regex(@"([c-gC-G][b#]?[0]) | ([a-gA-G][b#]?([1-9]|([1][0])))");
            if (!regex.IsMatch(s)) return;
        }

        public int Value
        {
            get => _value;
            set => _value = (byte)(Math.Abs(value) % 0x80); // 0-127
        }

        public string Name => _pitchesFlat[_value % 12] + ((_value / 12) - 1).ToString(); // C-1 -> G9

        #region Overloaded Operators
        public static implicit operator int(Pitch p) => p._value;
        public static implicit operator string(Pitch p) => p.Name;
        public static implicit operator Pitch(int i) => new Pitch(i);
        //public static implicit operator Pitch(string s) => new Pitch(s);
        
        #endregion

        public override bool Equals(object obj) => (obj is Pitch && ((Pitch)obj)._value == _value);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => Name;
    }
}
