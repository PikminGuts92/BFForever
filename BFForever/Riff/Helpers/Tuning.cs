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

namespace BFForever.Riff
{
    public class Tuning
    {
        public FString Name { get; set; } = "E Standard";

        // Highest -> lowest pitch
        public Pitch String1 { get; set; } = 64; // E4
        public Pitch String2 { get; set; } = 59; // B3
        public Pitch String3 { get; set; } = 55; // G3
        public Pitch String4 { get; set; } = 50; // D3
        public Pitch String5 { get; set; } = 45; // A2
        public Pitch String6 { get; set; } = 40; // E2
        public Pitch String7 { get; set; } = 0;
        public Pitch String8 { get; set; } = 0;

        // I'm not exactly sure what this is but to be safe we'll preserve it
        public Pitch String1Alt { get; set; } = 64; // E4
        public Pitch String2Alt { get; set; } = 48; // C3
        public Pitch String3Alt { get; set; } = 48; // C3
        public Pitch String4Alt { get; set; } = 48; // C3
        public Pitch String5Alt { get; set; } = 32; // Ab1
        public Pitch String6Alt { get; set; } = 32; // Ab1
        public Pitch String7Alt { get; set; } = 0;
        public Pitch String8Alt { get; set; } = 0;

        internal static Tuning ReadData(AwesomeReader ar)
        {
            Tuning tuning = new Tuning();

            // 40 bytes
            tuning.Name = ar.ReadUInt64();

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
            aw.Write((ulong)tuning.Name);
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
