using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public abstract class ZObject : Chunk
    {
        public ZObject(FString idx) : base(idx)
        {

        }

        public FString Directory { get; set; }

        public override void ImportData(AwesomeReader ar)
        {
            // Must override this!
        }
    }
}
