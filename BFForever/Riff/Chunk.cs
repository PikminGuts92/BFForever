using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            
            long idx = 0; // Index key

            Chunk chunk;
            switch (chunkType)
            {
                case Constant.INDX:
                    chunk = new Index();
                    chunk.ImportData(ar);

                    break;
                case Constant.STbl:
                    idx = ar.ReadInt64(); // Reads idx key
                    chunk = new StringTable(idx);
                    chunk.ImportData(ar);

                    break;
                case Constant.ZOBJ:
                    idx = ar.ReadInt64(); // Reads idx key
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
                        case Constant.RIFF_Event:
                            chunk = new Event(idx);
                            break;
                        case Constant.RIFF_Video:
                            chunk = new Video(idx);
                            break;
                        case Constant.RIFF_Texture:
                            chunk = new Texture(idx);
                            break;
                        case Constant.RIFF_Tab:
                            chunk = new Tab(idx);
                            break;
                        case Constant.RIFF_UILocStrings:
                            chunk = new UILocStrings(idx);
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

        protected abstract void ImportData(AwesomeReader ar);

        protected virtual byte[] ToBytes(bool bigEndian = true)
        {
            if (!(this is IData))
                // Needed for the GetObjects method
                return null;

            using (AwesomeWriter aw = new AwesomeWriter(new MemoryStream()))
            {
                aw.BigEndian = bigEndian; // Sets endianness
                aw.Write((long)IndexKey); // Index key

                if (this is ZObject) WriteObjectData(aw);
                //else if (this is STbl) WriteTableData(aw, man);

                // Returns chunk data
                return ((MemoryStream)aw.BaseStream).ToArray();
            }
        }

        private int GetObjectSize(IEnumerable<string> objects)
        {
            int size = 0;

            // Gets length of all objects put together
            foreach (object obj in objects)
            {
                if (obj is sbyte || obj is byte) size++;
                else if (obj is short || obj is ushort) size += 2;
                else if (obj is int || obj is uint || obj is float) size += 4;
                else if (obj is long || obj is ulong || obj is string || obj is IEnumerable<string>) size += 8;
                else if (obj is IEnumerable<object>) size += ((IEnumerable<object>)obj).Count();
                else
                    // Leaving this here for now
                    throw new NotImplementedException();
            }

            return size;
        }

        private void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((long)((ZObject)this).Directory.Key); // Directory key
            //aw.Write((long)((ZObject)this).); // 64-bit type key

            /*
             * Implement a switch statement here!
             */

            aw.Write((long)0); // Zero'd data

            // Read only collection of all data in chunk
            IReadOnlyCollection<object> objects = ((IData)this).GetObjects();

            int objSize = 0;

            // Gets length of all objects put together
            foreach (object obj in objects)
            {
                if (obj is sbyte || obj is byte) objSize++;
                else if (obj is short || obj is ushort) objSize += 2;
                else if (obj is int || obj is uint || obj is float) objSize += 4;
                else if (obj is long || obj is ulong || obj is string || obj is IEnumerable<string>) objSize += 8;
            }

            // Bytes to be written at the end
            List<byte[]> data = new List<byte[]>();

            // Sets next data offset
            int nextDataOffset = (int)aw.BaseStream.Position + objSize;

            foreach (object obj in objects)
            {
                // Writes integers
                if (obj is sbyte || obj is byte) aw.Write((sbyte)obj);
                else if (obj is short || obj is ushort) aw.Write((short)obj);
                else if (obj is int || obj is uint) aw.Write((int)obj);
                else if (obj is long || obj is ulong) aw.Write((long)obj);

                // Writes float
                else if (obj is float) aw.Write((float)obj);

                // Writes fused string
                else if (obj is FString) aw.Write((long)((FString)obj).Key);

                else if (obj is IEnumerable<FString>)
                {
                    IEnumerable<FString> strings = obj as IEnumerable<FString>;
                    aw.Write(strings.Count()); // Writes number of objects in collection

                    int relativeOffset = nextDataOffset - (int)aw.BaseStream.Position;
                    aw.Write(relativeOffset); // Writes relative offset

                    using (MemoryStream ms = new MemoryStream())
                    {
                        foreach (string s in strings)
                        {
                            // Gets byte data for string key
                            byte[] key = BitConverter.GetBytes((long)((FString)obj).Key);
                            if (aw.BigEndian) Array.Reverse(key); // Reverses endianness

                            // Writes data to memory
                            ms.Write(key, 0, key.Length);
                        }

                        // Adds to data list
                        data.Add(ms.ToArray());
                    }

                    // Updates next data offset
                    nextDataOffset += data[data.Count - 1].Length;
                }

                else if (obj is IEnumerable<object>)
                {
                    IEnumerable<object> subObj = obj as IEnumerable<object>;
                    aw.Write(objects.Count()); // Writes number of objects in collection

                    int relativeOffset = nextDataOffset - (int)aw.BaseStream.Position;
                    aw.Write(relativeOffset); // Writes relative offset

                    using (MemoryStream ms = new MemoryStream())
                    {
                        foreach (object o in subObj)
                        {
                            byte[] oBytes;

                            // Writes integers
                            if (o is sbyte || o is byte) oBytes = new byte[] { (byte)o };
                            else if (o is short || o is ushort) oBytes = BitConverter.GetBytes((short)o);
                            else if (o is int || o is uint) oBytes = BitConverter.GetBytes((int)o);
                            else if (o is long || o is ulong) oBytes = oBytes = BitConverter.GetBytes((long)o);

                            // Writes float
                            else if (o is float) oBytes = BitConverter.GetBytes((float)o);

                            // Writes fused string
                            else if (o is FString) oBytes = BitConverter.GetBytes((long)((FString)o).Key);

                            // Writes byte array
                            else if (o is byte[]) oBytes = o as byte[];

                            else throw new NotImplementedException();

                            if (!(o is string) && !(o is byte[]) && aw.BigEndian)
                                Array.Reverse(oBytes); // Reverses endianness

                            // Writes data to memory
                            ms.Write(oBytes, 0, oBytes.Length);
                        }

                        // Adds to data list
                        data.Add(ms.ToArray());
                    }

                    // Updates next data offset
                    nextDataOffset += data[data.Count - 1].Length;
                }
            }

            // Finally writes all data at the end
            foreach (byte[] d in data) aw.Write(d);
        }

        private void WriteTableData(AwesomeWriter aw)
        {

        }

        public FString IndexKey { get; set; }
    }
}
