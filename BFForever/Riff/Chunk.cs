using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public abstract class Chunk
    {
        public Chunk(FString idx)
        {
            IndexKey = idx;
        }

        public static Chunk FromStream(AwesomeReader ar)
        {
            int chunkType = ar.ReadInt32(); // INDX or STbl or ZOBJ
            int chunkSize = ar.ReadInt32();
            
            long idx = ar.ReadInt64(); // Index key

            Chunk chunk;
            switch (chunkType)
            {
                case Constant.INDX:
                    chunk = null;
                    break;
                case Constant.STbl:
                    chunk = new StringTable(idx);
                    break;
                case Constant.ZOBJ:
                    FString directory = ar.ReadInt64();
                    FString type = ar.ReadInt64();
                    ar.BaseStream.Position += 8; // Skips zeros
                    
                    switch(type.Value.ToLower())
                    {
                        case "index2":
                            chunk = new Index2(idx);
                            break;
                        default:
                            chunk = null;
                            break;
                    }
                    chunk.ImportData(ar);

                    break;
                default:
                    return null;
            }

            return chunk;
        }

        public abstract void ImportData(AwesomeReader ar);

        public FString IndexKey { get; set; }
    }
}
