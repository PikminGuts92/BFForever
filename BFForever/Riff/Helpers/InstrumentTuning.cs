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
    public struct InstrumentTuning
    {
        public FString Name { get; set; }

        // Highest -> lowest pitch
        public Pitch String1 { get; set; }
        public Pitch String2 { get; set; }
        public Pitch String3 { get; set; }
        public Pitch String4 { get; set; }
        public Pitch String5 { get; set; }
        public Pitch String6 { get; set; }
        public Pitch String7 { get; set; }
        public Pitch String8 { get; set; }       

        // I'm not exactly sure what this is but to be safe we'll preserve it
        public Pitch String1Alt { get; set; }
        public Pitch String2Alt { get; set; }
        public Pitch String3Alt { get; set; }
        public Pitch String4Alt { get; set; }
        public Pitch String5Alt { get; set; }
        public Pitch String6Alt { get; set; }
        public Pitch String7Alt { get; set; }
        public Pitch String8Alt { get; set; }        

        internal static InstrumentTuning ReadData(AwesomeReader ar)
        {
            InstrumentTuning tuning = new InstrumentTuning();

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

        internal static void WriteData(AwesomeWriter aw, InstrumentTuning tuning)
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

        // Available Tunings
        public static InstrumentTuning Guitar_EStandard => new InstrumentTuning()
        {
            Name = Global.Tuning_Guitar_EStandard,
            String1 = 64, // E4
            String2 = 59, // B3
            String3 = 55, // G3
            String4 = 50, // D3
            String5 = 45, // A2
            String6 = 40, // E2
            String7 = 0,
            String8 = 0,

            String1Alt = 64, // E4
            String2Alt = 48, // C3
            String3Alt = 48, // C3
            String4Alt = 48, // C3
            String5Alt = 32, // Ab1
            String6Alt = 32, // Ab1
            String7Alt = 0,
            String8Alt = 0
        };

        public static InstrumentTuning Guitar_DropD => new InstrumentTuning()
        {
            Name = Global.Tuning_Guitar_DropD,
            String1 = 64, // E4
            String2 = 59, // B3
            String3 = 55, // G3
            String4 = 50, // D3
            String5 = 45, // A2
            String6 = 38, // D2
            String7 = 0,
            String8 = 0,

            String1Alt = 64, // E4
            String2Alt = 48, // C3
            String3Alt = 48, // C3
            String4Alt = 48, // C3
            String5Alt = 32, // Ab1
            String6Alt = 32, // Ab1
            String7Alt = 0,
            String8Alt = 0
        };

        public static InstrumentTuning Guitar_EbStandard => new InstrumentTuning()
        {
            Name = Global.Tuning_Guitar_EbStandard,
            String1 = 63, // Eb4
            String2 = 58, // Bb3
            String3 = 54, // Gb3
            String4 = 49, // Db3
            String5 = 44, // Ab2
            String6 = 39, // Eb2
            String7 = 0,
            String8 = 0,

            String1Alt = 66, // Gb4
            String2Alt = 50, // D3
            String3Alt = 50, // D3
            String4Alt = 50, // D3
            String5Alt = 34, // Bb1
            String6Alt = 34, // Bb1
            String7Alt = 0,
            String8Alt = 0
        };

        // Available Tunings
        public static InstrumentTuning Bass_EStandard => new InstrumentTuning()
        {
            Name = Global.Tuning_Bass_EStandard,
            String1 = 43, // G2
            String2 = 38, // D3
            String3 = 33, // A1
            String4 = 28, // E1
            String5 = 0,
            String6 = 0,
            String7 = 0,
            String8 = 0,
            
            String1Alt = 32, // Ab1
            String2Alt = 32, // Ab1
            String3Alt = 16, // E0
            String4Alt = 16, // E0
            String5Alt = 0,
            String6Alt = 0,
            String7Alt = 0,
            String8Alt = 0
        };
    }
}
