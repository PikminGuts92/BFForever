using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Measure : ZObject
    {
        public Measure(FString idx) : base(idx)
        {
            Entries = new List<MeasureEntry>();
        }

        public List<MeasureEntry> Entries { get; set; }

        public override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 4
            ar.ReadInt32(); // Size of each TimeEntry (12 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads entry (12 bytes)
                MeasureEntry entry = new MeasureEntry();

                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                entry.Beat = ar.ReadSingle();

                Entries.Add(entry);
            }
        }
    }

    public class MeasureEntry : TimeEntry
    {
        private float _beat;

        public MeasureEntry()
        {
            _beat = 1.0f;
        }

        /// <summary>
        /// Gets or sets beat (1.0 = up, 2.0 = down)
        /// </summary>
        public float Beat
        {
            get
            {
                return _beat;
            }
            set
            {
                if (value == 1.0f)
                    _beat = value;
                else if (value == 2.0f)
                    _beat = value;
            }
        }
    }
}
