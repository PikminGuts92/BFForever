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
                    chunk.ImportData(ar);

                    break;
                case Constant.ZOBJ:
                    FString directory = ar.ReadInt64();
                    FString type = ar.ReadInt64();
                    ar.BaseStream.Position += 8; // Skips zeros
                    
                    switch(type.Key)
                    {
                        case Constant.RIFF_Index2:
                            chunk = new Index2(idx);
                            break;
                        case Constant.RIFF_PackageDef:
                            chunk = new PackageDef(idx);
                            break;
                        case Constant.RIFF_Catalog2:
                            chunk = new Catalog2(idx);
                            break;
                        case Constant.RIFF_Song:
                            chunk = new Song(idx);
                            break;
                        case Constant.RIFF_Audio:
                            chunk = new Audio(idx);
                            break;
                        case Constant.RIFF_Instrument:
                            chunk = new Instrument(idx);
                            break;
                        case Constant.RIFF_Vox:
                            chunk = new Vox(idx);
                            break;
                        case Constant.RIFF_VoxPushPhrase:
                            chunk = new VoxPushPhrase(idx);
                            break;
                        case Constant.RIFF_Tempo:
                            chunk = new Tempo(idx);
                            break;
                        case Constant.RIFF_Measure:
                            chunk = new Measure(idx);
                            break;
                        case Constant.RIFF_TimeSignature:
                            chunk = new TimeSignature(idx);
                            break;
                        case Constant.RIFF_Section:
                            chunk = new Section(idx);
                            break;
                        case Constant.RIFF_Chord:
                            chunk = new Chord(idx);
                            break;
                        case Constant.RIFF_AudioEffect:
                            chunk = new AudioEffect(idx);
                            break;
                        default:
                            return null;
                    }
                    ((ZObject)chunk).Directory = directory;
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
