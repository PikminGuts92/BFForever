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

            int chunkType = GetChunkType(ar);

            if (chunkType != MAGIC_INDX)
                throw new Exception("First chunk was not an Index!");

            Index index = new Index(ar);

            foreach(IndexEntry entry in index.Entries)
            {
                ar.BaseStream.Position = entry.Offset; // Jumps to offset
                chunkType = GetChunkType(ar);

                if (chunkType != MAGIC_STBL && chunkType != MAGIC_ZOBJ) continue;
                
                // Reads header info
                HKey filePath = new HKey(ar.ReadUInt64());
                HKey directoryPath = new HKey(ar.ReadUInt64());
                HKey type = new HKey(ar.ReadUInt64());
                ar.BaseStream.Position += 8;
                
                if (chunkType == MAGIC_STBL)
                {
                    // Gets localization
                    if (!StringTable.IsValidLocalization(type)) continue;

                    // Loads string table
                    StringTable table = new StringTable(filePath, directoryPath, StringTable.GetLocalization(type));
                    table.ReadData(ar);
                }
                else if (chunkType == MAGIC_ZOBJ)
                {
                    if (!Global.ZObjectTypes.ContainsKey(type)) continue; // Unsupported type
                    ZObject obj = Activator.CreateInstance(Global.ZObjectTypes[type], new object[] { filePath, directoryPath }) as ZObject;
                    
                    // Loads zobject
                    obj.ReadData(ar);
                    riff._objects.Add(obj);
                }
                else
                     // Unknown chunk
                    continue;
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

            List<ZObject> objects = _objects.Where(x => x is ZObject).ToList();
            objects.AddRange(CreateStringTables(_objects));

            var chunks = objects.Select(x => new
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

        private List<StringTable> CreateStringTables(List<ZObject> zobjects)
        {
            List<StringTable> tables = new List<StringTable>();
            var groups = zobjects.Where(x => !(x is StringTable)).GroupBy(x => x.DirectoryPath);

            foreach (var group in groups)
            {
                if (group.Key.Key != 0)
                {
                    // Creates shared string tables
                    foreach (HKey local in Global.StringTableLocalizationsOnDisc)
                    {
                        List<FString> strings = group.SelectMany(x => x.GetAllStrings()).ToList();

                        HKey localGroupDirectory = group.Key.Extend("." + local);
                        StringTable table = new StringTable(localGroupDirectory, group.Key.GetParentDirectory(), StringTable.GetLocalization(local));

                        foreach (FString str in strings)
                            table.Strings.Add(str.Key, StringKey.GetValue(str.Key, table.Localization));

                        tables.Add(table);
                    }

                    continue;
                }

                // Creates string table for singular zobject (Uses file path)
                foreach (HKey local in Global.StringTableLocalizationsOnDisc)
                {
                    foreach (ZObject obj in group)
                    {
                        List<FString> strings = obj.GetAllStrings();

                        HKey objectDirectory = obj.FilePath.Extend("." + local);
                        StringTable table = new StringTable(objectDirectory, obj.FilePath.GetParentDirectory(), StringTable.GetLocalization(local));

                        foreach (FString str in strings)
                            table.Strings.Add(str.Key, StringKey.GetValue(str.Key, table.Localization));

                        tables.Add(table);
                    }
                }
            }

            return tables;
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

        private static int GetChunkType(AwesomeReader ar)
        {
            int type = ar.ReadInt32();
            ar.BaseStream.Position += 4; // Skips size

            return type;
        }

        public bool BigEndian { get; set; }
        public List<ZObject> Objects => _objects;
    }
}
