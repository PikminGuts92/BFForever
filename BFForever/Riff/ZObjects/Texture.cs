using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Texture : ZObject
    {
        public Texture(FString idx) : base(idx)
        {

        }

        public FString InternalPath { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            // Reads texture path
            ar.BaseStream.Position += 8;
            InternalPath = ar.ReadInt64();
        }
    }
}
