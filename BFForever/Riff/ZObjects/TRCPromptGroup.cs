using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class TRCPromptGroup : ZObject
    {
        public TRCPromptGroup(FString idx) : base(idx)
        {
            Entries = new List<TRCPromptGroupEntry>();
        }

        protected override void ImportData(AwesomeReader ar)
        {
            int count = ar.ReadInt32();
            ar.ReadInt32(); // Always 4
            
            for (int i = 0; i < count; i++)
            {
                TRCPromptGroupEntry entry = new TRCPromptGroupEntry();

                // 24 bytes
                entry.Title = ar.ReadInt64();
                entry.Description = ar.ReadInt64();
                int stringCount = ar.ReadInt32();

                // Jumps to packages/external paths entries
                long stringOffset = (ar.ReadInt32() - 4) + ar.BaseStream.Position;
                long previousPosition = ar.BaseStream.Position;

                for (int ii = 0; ii < stringCount; ii++)
                {
                    // Jumps to string offset
                    ar.BaseStream.Position = stringOffset;

                    // Reads string
                    entry.Options.Add(ar.ReadInt64());
                    stringOffset += 8;
                }

                // Returns to next entry
                ar.BaseStream.Position = previousPosition;

                // Adds to entries
                Entries.Add(entry);
            }
        }

        public List<TRCPromptGroupEntry> Entries { get; set; }
    }

    public class TRCPromptGroupEntry
    {
        public TRCPromptGroupEntry()
        {
            Options = new List<FString>();
        }

        public FString Title { get; set; }
        public FString Description { get; set; }

        public List<FString> Options { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Title, Description);
        }
    }
}
