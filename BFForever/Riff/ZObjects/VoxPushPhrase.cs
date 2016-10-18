using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class VoxPushPhrase : ZObject
    {
        public VoxPushPhrase(FString idx) : base(idx)
        {
            Entries = new List<TimeEntry>();
        }

        public List<TimeEntry> Entries { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 10
            ar.ReadInt32(); // Size of each TimeEntry (8 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads voxpushphrase entry (8 bytes)
                TimeEntry entry = new TimeEntry();

                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                
                Entries.Add(entry);
            }
        }
    }
}
