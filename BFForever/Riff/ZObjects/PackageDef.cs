using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class PackageDef : ZObject
    {
        public PackageDef(FString idx) : base(idx)
        {
            Version = 1100024;
            PackageName = "DLC0024";
            Entries = new List<string>();
        }

        public int Version { get; set; }
        public string PackageName { get; set; }
        public List<string> Entries { get; set; }

        public override void ImportData(AwesomeReader ar)
        {
            Version = ar.ReadInt32(); // 1100024

            long nextString = ar.BaseStream.Position + 256;
            PackageName = ar.ReadNullString(); // "DLC0024"

            ar.BaseStream.Position = nextString;
            int count = ar.ReadInt32(); // # of strings

            // Offset - Always 4
            int offset = ar.ReadInt32();
            nextString = ar.BaseStream.Position + (offset - 4);

            Entries = new List<string>();

            for (int i = 0; i < count; i++)
            {
                ar.BaseStream.Position = nextString;

                // Reads string
                Entries.Add(ar.ReadNullString());
                nextString += 256;
            }
        }
    }
}
