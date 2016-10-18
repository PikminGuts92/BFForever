using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Event : ZObject
    {
        public Event(FString idx) : base(idx)
        {
            Entries = new List<EventEntry>();
        }

        public List<EventEntry> Entries { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 13
            ar.ReadInt32(); // Size of each TimeEntry (24 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads entry (24 bytes)
                EventEntry entry = new EventEntry();

                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                entry.EventName = ar.ReadInt64();
                ar.ReadInt32(); // Always 0
                entry.EventIndex = ar.ReadInt32();

                Entries.Add(entry);
            }
        }
    }

    public class EventEntry : TextEventEntry
    {
        /// <summary>
        /// EventEntry constructor
        /// <para>Valid event names: "AudioStart", "AudioEnd", "SongEnd", and "Phrase"</para>
        /// </summary>
        public EventEntry()
        {
            EventIndex = 0;
        }

        /// <summary>
        /// Gets or sets event index (Only used for 'Phrase' events)
        /// </summary>
        public int EventIndex { get; set; }
    }
}
