using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Vox : ZObject
    {
        public Vox(FString idx) : base(idx)
        {
            Entries = new List<VoxEntry>();
        }
        
        public List<VoxEntry> Entries { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 8
            ar.ReadInt32(); // Size of each VoxEntry (32 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads vox entry (32 bytes)
                VoxEntry entry = new VoxEntry();

                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                ar.ReadInt16(); // 2 zero'd bytes

                entry.Pitch = (sbyte)ar.ReadByte();
                entry.SecondaryPitch = (sbyte)ar.ReadByte();
                ar.ReadInt32(); // Always 0

                entry.Lyric = ar.ReadInt64();
                entry.NoteType = ar.ReadInt32();
                ar.ReadInt32(); // Always 0

                Entries.Add(entry);
            }
        }
    }

    public class VoxEntry : TimeEntry
    {
        private sbyte _pitch; // Same as MIDI specs
        private sbyte _pitch2; // Seconday pitch?
        private int _type;

        /// <summary>
        /// Constructor for Vox Entry
        /// </summary>
        public VoxEntry()
        {
            _pitch = 0;
            _pitch2 = 0;
            _type = 1;
        }

        /// <summary>
        /// Gets or sets lyric
        /// </summary>
        public FString Lyric { get; set; }

        /// <summary>
        /// Gets or sets note pitch
        /// </summary>
        public sbyte Pitch
        {
            get { return _pitch; }
            set
            {
                if (value >= 0)
                    _pitch = value;
                else
                    _pitch = 0;
            }
        }

        /// <summary>
        /// Gets or sets secondary note pitch
        /// </summary>
        public sbyte SecondaryPitch
        {
            get { return _pitch2; }
            set
            {
                if (value >= 0)
                    _pitch2 = value;
                else
                    _pitch2 = 0;
            }
        }

        /// <summary>
        /// Gets or sets note type (1 = regular, 2 = non-pitched, 3 = extended)
        /// </summary>
        public int NoteType
        {
            get { return _type; }
            set
            {
                switch (value)
                {
                    case 1: // Regular
                    case 2: // Non-Pitched
                    case 3: // Extended
                        _type = value;
                        break;
                    default:
                        _type = 1;
                        break;
                }
            }
        }
    }
}
