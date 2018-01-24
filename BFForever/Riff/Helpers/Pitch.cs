using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BFForever.Riff
{
    public class PitchConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Pitch);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;

            return objectType == typeof(Pitch) ? new Pitch(value) : new Pitch();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            serializer.Serialize(writer, ((Pitch)value).Name);
        }
    }

    [JsonConverter(typeof(PitchConverter))]
    public struct Pitch
    {
        private byte _value;
        private static readonly string[] _pitchesFlat = new string[] { "C", "D♭", "D", "E♭", "E", "F", "G♭", "G", "A♭", "A", "B♭", "B" };
        private static readonly string[] _pitchesSharp = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        private static readonly Regex regex = new Regex(@"^[A-G][b♭#]?\-?[0-9][0-9]?$");
        private static readonly Regex regexNumber = new Regex(@"\-?[0-9]+");

        public Pitch(int value)
        {
            // Ensures value remains in range
            _value = GetByteValue(value);
        }

        public Pitch(string s)
        {
            // Default value (C-1)
            _value = 0;

             // Must be uppercase
            if (!regex.IsMatch(s)) return;

            Match matchNumber = regexNumber.Match(s);
            int number = int.Parse(matchNumber.Value);

            string pitchName = s.Substring(0, matchNumber.Index).Replace('b', '♭');

            int pitchIdx = 0;
            string[] pitches = pitchName.Contains('♭') ? _pitchesFlat : _pitchesSharp;
            
            // Corrects non-existent pitches
            switch (pitchName)
            {
                case "B#":
                    pitchName = "C";
                    break;
                case "Cb":
                    pitchName = "B";
                    break;
                case "E#":
                    pitchName = "F";
                    break;
                case "F♭":
                    pitchName = "E";
                    break;
            }
            
            // Gets pitch index
            foreach(string pitch in pitches)
            {
                if (pitch == pitchName) break;
                pitchIdx++;
            }

            int value = 12 + (number * 12) + pitchIdx;
            _value = GetByteValue(value); // Sanitizes value
        }

        private byte GetByteValue(int value) => (byte)(Math.Abs(value) % 0x80);

        public int Value => _value;

        public string Name => _pitchesFlat[_value % 12] + ((_value / 12) - 1).ToString(); // C-1 -> G9

        #region Overloaded Operators
        public static implicit operator int(Pitch p) => p._value;
        public static implicit operator string(Pitch p) => p.Name;
        public static implicit operator Pitch(int i) => new Pitch(i);
        public static implicit operator Pitch(string s) => new Pitch(s);
        #endregion

        public override bool Equals(object obj) => (obj is Pitch && ((Pitch)obj)._value == _value);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => Name;
    }
}
