using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Vox ZObject
 * ===========
 * INT32 - Constant (8)
 * INT32 - Event Size (32)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff
{
    public class Vox : ZObject
    {
        public Vox(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<VoxEntry>();
        }

        protected override void AddMemberStrings(List<FString> strings) => strings.AddRange(Events.Select(x => x.Lyric));

        internal override void ReadData(AwesomeReader ar)
        {
            Events.Clear();
            ar.BaseStream.Position += 8; // Skips constants

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            for (int i = 0; i < count; i++)
            {
                // Reads vox entry (32 bytes)
                VoxEntry ev = new VoxEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                
                ev.Pitch = ar.ReadInt24() & 0xFF;
                ev.PitchAlt = ar.ReadByte();
                ar.BaseStream.Position += 4; // Always 0

                ev.Lyric = ar.ReadUInt64();
                ev.NoteType = (VoxNote)ar.ReadInt32();
                ar.BaseStream.Position += 4; // Always 0

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)8);
            aw.Write((int)32);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (VoxEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);

                aw.Write((int)(ev.Pitch << 8 | ev.PitchAlt));
                aw.BaseStream.Position += 4; // Always 0

                aw.Write((ulong)ev.Lyric);
                aw.Write((int)ev.NoteType);
                aw.Write((int)0); // Always 0
            }
        }
        
        public override HKey Type => Global.ZOBJ_Vox;

        public List<VoxEntry> Events { get; set; }
    }

    public class VoxEntry : TimeEvent
    {
        public FString Lyric { get; set; }
        public Pitch Pitch { get; set; }
        public Pitch PitchAlt { get; set; } // Not sure what this is
        public VoxNote NoteType { get; set; }
    }

    public enum VoxNote : int
    {
        Regular = 1,
        NonPitched = 2,
        Extended = 3
    }
}
