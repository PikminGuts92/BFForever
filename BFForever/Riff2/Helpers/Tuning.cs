using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Tuning (40 bytes)
 * =================
 *  SKEY - Tuning Name
 * INT32 - String 1
 *   [0] - Always 0
 *   [1] - Always 0
 *   [2] - Pitch
 *   [3] - Alternate Pitch?
 * INT32 - String 2 \
 * INT32 - String 3  |
 * INT32 - String 4  |
 * INT32 - String 5  | Same as first string data
 * INT32 - String 6  |
 * INT32 - String 7  |
 * INT32 - String 8 /
 */

namespace BFForever.Riff2
{
    public class Tuning
    {
        public FString Name { get; set; }
        public Pitch String1 { get; set; } // Highest pitch
        public Pitch String2 { get; set; }
        public Pitch String3 { get; set; }
        public Pitch String4 { get; set; }
        public Pitch String5 { get; set; }
        public Pitch String6 { get; set; }
        public Pitch String7 { get; set; }
        public Pitch String8 { get; set; }

        // I'm not exactly sure what this is but to be safe we'll preserve it
        public Pitch String1Alt { get; set; } // Highest pitch
        public Pitch String2Alt { get; set; }
        public Pitch String3Alt { get; set; }
        public Pitch String4Alt { get; set; }
        public Pitch String5Alt { get; set; }
        public Pitch String6Alt { get; set; }
        public Pitch String7Alt { get; set; }
        public Pitch String8Alt { get; set; }

        internal static Tuning ReadData(AwesomeReader ar)
        {
            Tuning tuning = new Tuning();

            // 40 bytes
            tuning.Name = ar.ReadInt64();

            tuning.String1 = ar.ReadInt24() & 0xFF;
            tuning.String1Alt = ar.ReadByte();

            tuning.String2 = ar.ReadInt24() & 0xFF;
            tuning.String2Alt = ar.ReadByte();

            tuning.String3 = ar.ReadInt24() & 0xFF;
            tuning.String3Alt = ar.ReadByte();

            tuning.String4 = ar.ReadInt24() & 0xFF;
            tuning.String4Alt = ar.ReadByte();

            tuning.String5 = ar.ReadInt24() & 0xFF;
            tuning.String5Alt = ar.ReadByte();

            tuning.String6 = ar.ReadInt24() & 0xFF;
            tuning.String6Alt = ar.ReadByte();

            tuning.String7 = ar.ReadInt24() & 0xFF;
            tuning.String7Alt = ar.ReadByte();

            tuning.String8 = ar.ReadInt24() & 0xFF;
            tuning.String8Alt = ar.ReadByte();

            return tuning;
        }

        internal static void WriteData(AwesomeWriter aw, Tuning tuning)
        {
            // 40 bytes
            aw.Write((long)tuning.Name);
            aw.Write((int)(tuning.String1 << 8 | tuning.String1Alt));
            aw.Write((int)(tuning.String2 << 8 | tuning.String2Alt));
            aw.Write((int)(tuning.String3 << 8 | tuning.String3Alt));
            aw.Write((int)(tuning.String4 << 8 | tuning.String4Alt));
            aw.Write((int)(tuning.String5 << 8 | tuning.String5Alt));
            aw.Write((int)(tuning.String6 << 8 | tuning.String6Alt));
            aw.Write((int)(tuning.String7 << 8 | tuning.String7Alt));
            aw.Write((int)(tuning.String8 << 8 | tuning.String8Alt));
        }
    }
}
