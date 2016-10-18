using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Audio : ZObject
    {
        public Audio(FString idx) : base(idx)
        {
            
        }

        public FString ExternalPath { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            // Reads audio path
            ar.BaseStream.Position += 8;
            ExternalPath = ar.ReadInt64();
            ar.BaseStream.Position += 8;
        }
    }
}
