using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Video : ZObject
    {
        public Video(FString idx) : base(idx)
        {

        }

        public FString InternalPath { get; set; }

        public override void ImportData(AwesomeReader ar)
        {
            // Reads video path
            ar.BaseStream.Position += 8;
            InternalPath = ar.ReadInt64();
        }
    }
}
