using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/* 
 * CELT HEADER (40 bytes)
 * ======================
 * BYTE[4] - "BFAD"
 *   INT16 - Version (Always 2)
 *   INT16 - Encryption Flag
 *            0: Non-encrypted
 *            1: Encrypted
 *   INT32 - Total Samples
 *   INT32 - Bitrate
 *   INT16 - Uncompressed Frame Size? (Always 960)
 *   INT16 - Unknown (Always 312)
 *   INT16 - Sample Rate (Always 48000)
 *   INT16 - Unknown (Always 1)
 *   INT32 - Audio Header Block Offset
 *   INT32 - Audio Header Block Size
 *   INT32 - Encoded Audio Block Offset
 *   INT32 - Encoded Audio Block Size
 *  
 *  The following blocks are encrypted...
 *  
 *  AUDIO HEADER BLOCK
 *  ==================
 *  
 *  ENCODED AUDIO BLOCK
 *  ===================
 */
namespace BFForever.Audio
{
    public class Celt
    {
        private const int MAGIC = 0x44414642; // "BFAD"
        private const int MAGIC_R = 0x42464144;

        public uint Version { get; set; } = 2;
        public bool Encrypted { get; set; } = false;
        public uint TotalSamples { get; set; }
        public uint Bitrate { get; set; } = 96000;

        public ushort FrameSize { get; set; } = 960;
        public ushort Unknown1 { get; set; } = 312;
        public ushort SampleRate { get; set; } = 48000;
        public ushort Unknown2 { get; set; } = 1;

        public uint AudioHeaderOffset { get; set; }
        public uint AudioHeaderSize { get; set; }

        public uint AudioBlockOffset { get; set; }
        public uint AudioBlockSize { get; set; }

        public byte[] AudioHeader { get; set; }
        public byte[] AudioBlock { get; set; }

        public bool BigEndian { get; set; } = false;

        public static Celt FromFile(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                return FromStream(fs);
            }
        }

        public static Celt FromStream(Stream stream)
        {
            Celt celt = new Celt();

            using (AwesomeReader ar = new AwesomeReader(stream, false))
            {
                // Checks for "BFAD" magic
                switch(ar.ReadInt32())
                {
                    case MAGIC:
                        ar.BigEndian = false;
                        break;
                    case MAGIC_R:
                        ar.BigEndian = true;
                        break;
                    default:
                        throw new Exception("Invalid magic. Expected \"BFAD\"");
                }

                celt.BigEndian = ar.BigEndian; // Sets endianess

                // Parses header information
                celt.Version = ar.ReadUInt16();
                celt.Encrypted = Convert.ToBoolean(ar.ReadInt16());
                celt.TotalSamples = ar.ReadUInt32();
                celt.Bitrate = ar.ReadUInt32();

                celt.FrameSize = ar.ReadUInt16();
                celt.Unknown1 = ar.ReadUInt16();
                celt.SampleRate = ar.ReadUInt16();
                celt.Unknown2 = ar.ReadUInt16();

                celt.AudioHeaderOffset = ar.ReadUInt32();
                celt.AudioHeaderSize = ar.ReadUInt32();
                celt.AudioBlockOffset = ar.ReadUInt32();
                celt.AudioBlockSize = ar.ReadUInt32();
                celt.FixOffsets(); // Only useful for audio extracted from RAM, harmless
                
                uint headerSize = celt.AudioBlockOffset - celt.AudioHeaderOffset; // Multiple of 4
                uint blockSize = celt.AudioBlockSize;
                
                if ((headerSize + blockSize) % 16 != 0)
                    blockSize += 16 - ((headerSize + blockSize) % 16);

                celt.AudioHeader = ar.ReadBytes((int)headerSize);
                celt.AudioBlock = ar.ReadBytes((int)blockSize);
            }

            return celt;
        }

        private void FixOffsets()
        {
            if (AudioBlockOffset <= 40)
                return;

            AudioBlockOffset = (AudioBlockOffset - AudioHeaderOffset) + 40;
            AudioHeaderOffset = 40;
        }

        public void Export(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream fs = File.OpenWrite(path))
            {
                WriteToStream(fs);
            }
        }

        private void WriteToStream(Stream stream)
        {
            using (AwesomeWriter aw = new AwesomeWriter(stream))
            {
                aw.BigEndian = BigEndian;
                aw.Write((int)MAGIC);
                aw.Write((ushort)Version);
                aw.Write(Convert.ToUInt16(Encrypted));
                aw.Write((uint)TotalSamples);
                aw.Write((uint)Bitrate);

                aw.Write((ushort)FrameSize);
                aw.Write((ushort)Unknown1);
                aw.Write((ushort)SampleRate);
                aw.Write((ushort)Unknown2);

                aw.Write((uint)AudioHeaderOffset);
                aw.Write((uint)AudioHeaderSize);
                aw.Write((uint)AudioBlockOffset);
                aw.Write((uint)AudioBlockSize);

                aw.Write(AudioHeader);
                aw.Write(AudioBlock);
            }
        }
    }
}
