using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Debug; // For debug output

namespace BFForever.Riff
{
    public class Tab : ZObject
    {
        public Tab(FString idx) : base(idx)
        {
            Entries = new List<TabEntry>();
        }

        public List<TabEntry> Entries { get; set; }

        public override void ImportData(AwesomeReader ar)
        {
            ar.ReadInt32(); // Always 11
            ar.ReadInt32(); // Size of each VoxEntry (64 bytes)

            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads vox entry (64 bytes)
                TabEntry entry = new TabEntry();
                
                // 0 - 19 bytes
                entry.Start = ar.ReadSingle();
                entry.End = ar.ReadSingle();
                entry.StringNumber = ar.ReadInt32(); // String (1-6)
                entry.FretNumber = ar.ReadInt32(); // Fret (0-22?)
                entry.NoteColor = ar.ReadInt32(); // Finger (0-6)

                // 20 - 39
                entry.SpecialOperation = ar.ReadInt32();
                entry.BendType = ar.ReadInt32();
                entry.BendStrength = ar.ReadSingle();
                entry.Vibrato = Convert.ToBoolean(ar.ReadInt32());
                ar.ReadInt32(); // Should be zero

                // 40 - 59
                ar.ReadInt32(); // Should be 63.0 for both.
                ar.ReadInt32();
                ar.ReadSingle(); // Observed: 0.0, -240.0
                ar.ReadSingle(); // Observed: 0.0, -114.0, 126.0

                entry.Extended = ar.ReadBoolean();
                entry.PalmMute = ar.ReadBoolean();
                entry.Tremelo = ar.ReadBoolean();
                ar.ReadByte(); // Not sure.

                // 60 - 63
                ar.ReadBytes(3);
                entry.PopSlap = ar.ReadByte();

                Entries.Add(entry);
            }
        }
    }

    public class TabEntry : TimeEntry
    {
        private int _stringNumber;
        private int _fretNumber;
        private int _noteColor; // For fingering
        private int _special;
        private int _bendType;
        private int _popSlap;

        public TabEntry()
        {
            _stringNumber = 1; // High e
            _fretNumber = 0;
            _noteColor = 0; // Purple
            _special = 0; // Regular note

            _bendType = 0; // Regular note
            BendStrength = 0.0f;
            Vibrato = false;

            Extended = false;
            PalmMute = false;
            Tremelo = false;

            _popSlap = 0;
        }

        /// <summary>
        /// Gets or sets string number
        /// <para>1: e</para>
        /// <para>2: B</para>
        /// <para>3: G</para>
        /// <para>4: D</para>
        /// <para>5: A</para>
        /// <para>6: E</para>
        /// </summary>
        public int StringNumber
        {
            get
            {
                return _stringNumber;
            }
            set
            {
                if (value >= 1 && value <= 6)
                    _stringNumber = value;
                else
                    // For debugging
                    WriteLine($"TAB @ {this.Start} : String Number '{value}' is not valid!");
            }
        }

        /// <summary>
        /// Gets or sets fret number (0-22)
        /// </summary>
        public int FretNumber
        {
            get
            {
                return _fretNumber;
            }
            set
            {
                if (value >= 0 && value <= 22)
                    _fretNumber = value;
                else
                    // For debugging
                    WriteLine($"TAB @ {this.Start} : Fret Number '{value}' is not valid!");
            }
        }

        /// <summary>
        /// Gets or sets note color (0-6)
        /// <para>0: Purple (Open)</para>
        /// <para>1: Green</para>
        /// <para>2: Red</para>
        /// <para>3: Yellow</para>
        /// <para>4: Blue</para>
        /// <para>5: Orange</para>
        /// <para>6: Black (Tap)</para>
        /// </summary>
        public int NoteColor
        {
            get
            {
                return _noteColor;
            }
            set
            {
                if (value >= 0 && value <= 6)
                    _noteColor = value;
                else
                    // For debugging
                    WriteLine($"TAB @ {this.Start} : Note Color '{value}' is not valid!");
            }
        }

        /// <summary>
        /// Gets or sets special note operation
        /// <para>0: Regular</para>
        /// <para>1: Left-Hand Mute</para>
        /// <para>2: Slide to next note (Determined by Bend Type)</para>
        /// <para>3: Upward slide to nothing</para>
        /// <para>4: Downward slide to nothing</para>
        /// <para>9: Trill</para>
        /// <para>10: Natural Harmonic</para>
        /// <para>11: Pinch Harmonic</para>
        /// </summary>
        public int SpecialOperation // Special note operation
        {
            get
            {
                return _special;
            }
            set
            {
                switch (value)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 9:
                    case 10:
                    case 11:
                        _special = value;
                        break;
                    default:
                        // For debugging
                        WriteLine($"TAB @ {this.Start} : Special operation '{value}' is not valid!");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets special note value
        /// <para>0: Regular</para>
        /// <para>1: Start from original note and bend up by 'bend strength' over duration</para>
        /// <para>2: Pre-bend from Bend Strength, bend down for duration</para>
        /// <para>3: Pre-bend from Bend Strength, hold bend</para>
        /// <para>4: Pre-bend from Bend Strength, hold bend, is a hold-over note (i.e. This note is connected to the one before it)</para>
        /// </summary>
        public int BendType
        {
            get
            {
                return _bendType;
            }
            set
            {
                switch (value)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        _bendType = value;
                        break;
                    default:
                        // For debugging
                        WriteLine($"TAB @ {this.Start} : Bend type '{value}' is not valid!");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets bend strength (increments of 0.5)
        /// </summary>
        public float BendStrength { get; set; }

        /// <summary>
        /// Gets or sets vibrato
        /// </summary>
        public bool Vibrato { get; set; }

        /// <summary>
        /// Gets or sets extended
        /// </summary>
        public bool Extended { get; set; }

        /// <summary>
        /// Gets or sets palm mute
        /// </summary>
        public bool PalmMute { get; set; }

        /// <summary>
        ///  Gets or sets tremelo
        /// </summary>
        public bool Tremelo { get; set; }

        /// <summary>
        /// Gets or sets pop/slap
        /// <para>0: Regular</para>
        /// <para>1: Slap</para>
        /// <para>2: Pop</para>
        /// </summary>
        public int PopSlap
        {
            get
            {
                return _popSlap;
            }
            set
            {
                switch(value)
                {
                    case 0:
                    case 1:
                    case 2:
                        _popSlap = value;
                        break;
                    default:
                        // For debugging
                        WriteLine($"TAB @ {this.Start} : Pop slap '{value}' is not valid!");
                        break;
                }
            }
        }
    }
}
