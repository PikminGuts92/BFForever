using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class TimeSignature : ZObject
    {
        public TimeSignature(FString idx) : base(idx)
        {
            Entries = new List<TimeSignatureEntry>();
        }

        public List<TimeSignatureEntry> Entries { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 0
            ar.ReadInt32(); // Size of each TimeEntry (16 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads entry (16 bytes)
                TimeSignatureEntry entry = new TimeSignatureEntry();

                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                entry.Beat = ar.ReadInt32();
                entry.Measure = ar.ReadInt32();

                Entries.Add(entry);
            }
        }
    }

    public class TimeSignatureEntry : TimeEntry
    {

        public TimeSignatureEntry()
        {
            // 4/4 time signature
            Beat = 4;
            Measure = 4;
        }
        
        /// <summary>
        /// Gets or sets beat
        /// </summary>
        public int Beat { get; set; }

        /// <summary>
        /// Gets or sets measure
        /// </summary>
        public int Measure { get; set; }
    }
}
