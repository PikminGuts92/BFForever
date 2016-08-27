using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Chord : ZObject
    {
        public Chord(FString idx) : base(idx)
        {
            Entries = new List<TextEventEntry>();
        }

        public List<TextEventEntry> Entries { get; set; }

        public override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 6
            ar.ReadInt32(); // Size of each TimeEntry (24 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads entry (24 bytes)
                TextEventEntry entry = new TextEventEntry();

                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                entry.EventName = ar.ReadInt64();
                ar.ReadInt64(); // Always 0

                Entries.Add(entry);
            }
        }
    }
}
