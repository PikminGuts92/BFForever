using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class UILocStrings : ZObject
    {
        public UILocStrings(FString idx) : base(idx)
        {
            Entries = new List<FString>();
        }

        protected override void ImportData(AwesomeReader ar)
        {
            int count = ar.ReadInt32();
            ar.ReadInt32(); // Always 4

            // Reads all fstring entries
            for (int i = 0; i < count; i++)
            {
                FString fstring = new FString(ar.ReadInt64());
                Entries.Add(fstring);
            }
        }

        public List<FString> Entries { get; set; }
    }
}
