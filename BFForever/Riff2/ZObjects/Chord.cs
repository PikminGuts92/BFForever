using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Chord ZObject
 * =============
 * INT32 - Constant (6)
 * INT32 - Event Size (24)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff2
{
    public class Chord : ZObject
    {
        public Chord(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<TextEvent>();
        }

        protected override void AddMemberStrings(List<FString> strings) => strings.AddRange(Events.Select(x => x.EventName));

        internal override void ReadData(AwesomeReader ar)
        {
            Events.Clear();
            ar.BaseStream.Position += 8; // Skips constants

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            for (int i = 0; i < count; i++)
            {
                TextEvent ev = new TextEvent();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ev.EventName = ar.ReadUInt64();
                ar.BaseStream.Position += 8; // Always 0

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)6);
            aw.Write((int)24);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (TextEvent ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((ulong)ev.EventName);
                aw.Write((ulong)0);
            }
        }

        public override HKey Type => Global.ZOBJ_Chord;

        public List<TextEvent> Events { get; set; }
    }
}
