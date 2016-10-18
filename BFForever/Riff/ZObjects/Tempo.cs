using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Tempo : ZObject
    {
        public Tempo(FString idx) : base(idx)
        {
            Entries = new List<TempoEntry>();
        }

        public List<TempoEntry> Entries { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 1
            ar.ReadInt32(); // Size of each TimeEntry (12 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads entry (12 bytes)
                TempoEntry entry = new TempoEntry();

                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                entry.BPM = ar.ReadSingle();

                Entries.Add(entry);
            }
        }
    }

    public class TempoEntry : TimeEntry
    {
        private float _bpm;

        public TempoEntry()
        {
            _bpm = 120.0f;
        }

        /// <summary>
        /// Gets or sets bpm
        /// </summary>
        public float BPM
        {
            get
            {
                return _bpm;
            }
            set
            {
                if (value >= 0.0f)
                    _bpm = value;
            }
        }
    }
}
