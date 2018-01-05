using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BFForever.Riff2
{
    public class RiffFile
    {
        private const int MAGIC_RIFF = 0x46464952; // "RIFF"
        private const int MAGIC_RIFF_R = 0x52494646;
        private const int MAGIC_INDX = 0x58444E49;
        private const int MAGIC_STBL = 0x6C625453;
        private const int MAGIC_ZOBJ = 0x4A424F5A;

        private readonly List<ZObject> _objects;

        public RiffFile()
        {
            BigEndian = true;
            _objects = new List<ZObject>();
        }

        public static RiffFile FromFile(string input)
        {
            if (!File.Exists(input)) return null; // Returns if file doesn't exist

            using (FileStream fs = File.OpenRead(input))
            {
                return FromStream(fs);
            }
        }

        private static RiffFile FromStream(Stream stream)
        {
            RiffFile riff = new RiffFile();
            AwesomeReader ar = new AwesomeReader(stream);

            // Checks for "RIFF" magic.
            switch (ar.ReadInt32())
            {
                case MAGIC_RIFF:
                    ar.BigEndian = false;
                    break;
                case MAGIC_RIFF_R:
                    ar.BigEndian = true;
                    break;
                default:
                    throw new Exception("Invalid magic. Expected \"RIFF\"");
            }

            riff.BigEndian = ar.BigEndian; // Sets endianess
            ar.BaseStream.Position += 4; // Skips total size

            int chunkType; uint size;
            GetChunkInfo(ar, out chunkType, out size);

            if (chunkType != MAGIC_INDX)
                throw new Exception("First chunk was not an Index!");

            Index index = new Index(ar);

            foreach(IndexEntry entry in index.Entries)
            {
                ar.BaseStream.Position = entry.Offset; // Jumps to offset
                GetChunkInfo(ar, out chunkType, out size);

                if (chunkType != MAGIC_STBL && chunkType != MAGIC_ZOBJ) continue;
                
                // Reads header info
                HKey filePath = new HKey(ar.ReadInt64());
                HKey directoryPath = new HKey(ar.ReadInt64());
                HKey type = new HKey(ar.ReadInt64());
                ar.BaseStream.Position += 8;

                ZObject obj;

                if (chunkType == MAGIC_STBL)
                {
                    // Gets localization
                    if (!StringTable.IsValidLocalization(type)) continue;

                    // Loads string table
                    obj = new StringTable(filePath, directoryPath, StringTable.GetLocalization(type));
                    obj.ReadData(ar);
                }
                else if (chunkType == MAGIC_ZOBJ)
                {
                    // TODO: Update this to not use switch statement
                    if (type.Value == null) continue;

                    switch (type.Value.ToLower())
                    {
                        case "packagedef":
                            obj = new PackageDef(filePath, directoryPath);
                            break;
                        case "index2":
                            obj = new Index2(filePath, directoryPath);
                            break;
                        case "catalog2":
                            obj = new Catalog2(filePath, directoryPath);
                            break;
                        // Instrument related
                        case "audio":
                            obj = new Audio(filePath, directoryPath);
                            break;
                        case "audioeffect":
                            obj = new AudioEffect(filePath, directoryPath);
                            break;
                        case "chord":
                            obj = new Chord(filePath, directoryPath);
                            break;
                        case "event":
                            obj = new Event(filePath, directoryPath);
                            break;
                        case "instrument":
                            obj = new Instrument(filePath, directoryPath);
                            break;
                        case "measure":
                            obj = new Measure(filePath, directoryPath);
                            break;
                        case "section":
                            obj = new Section(filePath, directoryPath);
                            break;
                        case "song":
                            obj = new Song(filePath, directoryPath);
                            break;
                        case "tab":
                            obj = new Tab(filePath, directoryPath);
                            break;
                        case "tempo":
                            obj = new Tempo(filePath, directoryPath);
                            break;
                        case "timesignature":
                            obj = new TimeSignature(filePath, directoryPath);
                            break;
                        case "texture":
                            obj = new Texture(filePath, directoryPath);
                            break;
                        case "video":
                            obj = new Video(filePath, directoryPath);
                            break;
                        case "vox":
                            obj = new Vox(filePath, directoryPath);
                            break;
                        case "voxpushphrase":
                            obj = new VoxPushPhrase(filePath, directoryPath);
                            break;
                        default:
                            continue;
                    }

                    // Loads zobject
                    obj.ReadData(ar);
                }
                else
                    continue;

                // Adds object
                riff._objects.Add(obj);
            }

            return riff;
        }

        public void WriteToFile(string output)
        {
            using (FileStream fs = File.Create(output))
            {
                WriteToStream(fs);
            }
        }

        private void WriteToStream(Stream stream)
        {
            AwesomeWriter aw = new AwesomeWriter(stream, BigEndian);
            long startOffset = aw.BaseStream.Position;
            long offset = startOffset + 24 + (_objects.Count * 16);

            var chunks = _objects.Select(x => new
            {
                Path = x.FilePath,
                Offset = offset,
                Data = CreateChunk(x, BigEndian, ref offset),
                IsStringTable = (x is StringTable)
            }).ToArray();

            aw.Write((int)MAGIC_RIFF);
            aw.Write((uint)(offset - (startOffset + 8)));

            Index index = new Index()
            {
                Entries = new List<IndexEntry>(chunks.Select(x => new IndexEntry()
                {
                    FilePath = x.Path,
                    Offset = (uint)x.Offset}
                ))
            };

            // Writes index chunk
            aw.Write((int)MAGIC_INDX);
            aw.Write((int)(8 + (index.Entries.Count * 16)));
            index.WriteData(aw);

            // Writes other zobjects and string tables
            foreach (var chunk in chunks)
            {
                aw.Write((int)(chunk.IsStringTable ? MAGIC_STBL : MAGIC_ZOBJ));
                aw.Write((int)chunk.Data.Length);
                aw.Write(chunk.Data);
            }
        }

        private byte[] CreateChunk(ZObject obj, bool bigEndian, ref long offset)
        {
            using (AwesomeWriter aw = new AwesomeWriter(new MemoryStream(), bigEndian))
            {
                obj.WriteData(aw);

                // Ensures chunk size is always divisible by 4
                if (aw.BaseStream.Position % 4 != 0)
                    aw.Write(new byte[4 - (aw.BaseStream.Position % 4)]);

                offset += aw.BaseStream.Position + 8; // 8 = Chunk Magic + Size
                return ((MemoryStream)aw.BaseStream).ToArray();
            }
        }

        private static void GetChunkInfo(AwesomeReader ar, out int type, out uint size)
        {
            type = ar.ReadInt32();
            size = ar.ReadUInt32();
        }

        public bool BigEndian { get; set; }
        public List<ZObject> Objects => _objects;
    }
}
