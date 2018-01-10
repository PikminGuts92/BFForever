using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Tab ZObject
 * ===========
 * INT32 - Constant (11)
 * INT32 - Event Size (64)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 * 
 * Tab Event (64 bytes)
 * ====================
 * FLOAT - Start Position
 * FLOAT - End Position
 * INT32 - String Number (1-6)
 * INT32 - Fret Number (0-24?)
 * INT32 - Finger Number (0-5)
 * INT32 - Note Type
 * INT32 - Bend Type
 * FLOAT - Bend Strength (Increments of 0.5?)
 * INT32 - Vibrato Type
 * INT32 - Always 0?
 * FLOAT - Always 63.0?
 * FLOAT - Always 63.0?
 * FLOAT - Unknown (Usually 0.0, sometimes -240.0)
 * FLOAT - Unknown (Usually 0.0, sometimes -114.0 or 126.0)
 *  BOOL - Extended Note
 *  BOOL - Palm Mute
 *  BOOL - Tremelo
 *  BOOL - Always 0?
 * INT32 - Bass Type
 */

namespace BFForever.Riff2
{
    public class Tab : ZObject
    {
        public Tab(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<TabEntry>();
        }

        protected override void AddMemberStrings(List<FString> strings) { }

        internal override void ReadData(AwesomeReader ar)
        {
            Events.Clear();
            ar.BaseStream.Position += 8; // Skips constants

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            for (int i = 0; i < count; i++)
            {
                // Reads tab entry (64 bytes)
                TabEntry entry = new TabEntry();
                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();

                entry.StringNumber = ar.ReadInt32();
                entry.FretNumber = ar.ReadInt32();
                entry.Finger = (TabFinger)ar.ReadInt32();

                entry.NoteType = (TabNoteType)ar.ReadInt32();
                entry.BendType = (TabBendType)ar.ReadInt32();
                entry.BendStrength = ar.ReadSingle();
                entry.VibratoType = (VibratoType)ar.ReadInt32();

                ar.BaseStream.Position += 4; // Always 0?
                entry.Unknown1 = ar.ReadSingle();
                entry.Unknown2 = ar.ReadSingle();
                entry.Unknown3 = ar.ReadSingle();
                entry.Unknown4 = ar.ReadSingle();

                entry.ExtendedNote = ar.ReadBoolean();
                entry.PalmMute = ar.ReadBoolean();
                entry.Tremelo = ar.ReadBoolean();
                ar.BaseStream.Position += 1; // Always 0

                entry.BassType = (TabBassType)ar.ReadInt32();
                Events.Add(entry);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)11);
            aw.Write((int)64);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (TabEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);

                aw.Write((int)ev.StringNumber);
                aw.Write((int)ev.FretNumber);
                aw.Write((int)ev.Finger);

                aw.Write((int)ev.NoteType);
                aw.Write((int)ev.BendType);
                aw.Write((float)ev.BendStrength);
                aw.Write((int)ev.VibratoType);

                aw.BaseStream.Position += 4; // Always 0
                aw.Write((float)ev.Unknown1);
                aw.Write((float)ev.Unknown2);
                aw.Write((float)ev.Unknown3);
                aw.Write((float)ev.Unknown4);

                aw.Write((bool)ev.ExtendedNote);
                aw.Write((bool)ev.PalmMute);
                aw.Write((bool)ev.Tremelo);
                aw.BaseStream.Position += 1; // Always 0

                aw.Write((int)ev.BassType);
            }
        }

        public override HKey Type => Global.ZOBJ_Tab;

        public List<TabEntry> Events { get; set; }
    }

    public class TabEntry : TimeEvent
    {
        public int StringNumber { get; set; }
        public int FretNumber { get; set; }
        public TabFinger Finger { get; set; }
        public TabNoteType NoteType { get; set; }
        public TabBendType BendType { get; set; }
        public float BendStrength { get; set; }
        public VibratoType VibratoType { get; set; }
        public float Unknown1 { get; set; } = 63.0f;
        public float Unknown2 { get; set; } = 63.0f;
        public float Unknown3 { get; set; } = 0.0f;
        public float Unknown4 { get; set; } = 0.0f;
        public bool ExtendedNote { get; set; }
        public bool PalmMute { get; set; }
        public bool Tremelo { get; set; }
        public TabBassType BassType { get; set; }
    }
}
