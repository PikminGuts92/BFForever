using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Concentus;
using Concentus.Common;
using Concentus.Enums;
using Concentus.Structs;
using NAudio;
using NAudio.Wave;

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
 *   INT32 - Reckoning Offset
 *   INT32 - Reckoning Size
 *   INT32 - Packet Stream Offset
 *   INT32 - Packet Stream Size
 *  
 *  The following blocks are encrypted...
 *  
 *  RECKONING BLOCK
 *  ==================
 *  Packet counts which determine how stream is to be decoded from audio block. Counts are encoded as either 7-bit or 15-bit integers.
 *  The first bit used as a switch for 2 byte mode. Big endian byte order.
 *  The first count is # of empty packets to skip (empty frames), next is # of packets to read from audio block, (continue alternating)
 *  
 *  PACKET STREAM BLOCK
 *  ===================
 *  OPUS packet stream. Each packet begins with a size encoded as 12-bits in big endian byte order (The first 4 bits can be ignored).
 *  TOC byte seems to always be 0xFC -> CELT-mode (Fullband @ 20 ms), stereo, 1 frame
 */
namespace BFForever.Audio
{
    public class Celt
    {
        private const int MAGIC = 0x44414642; // "BFAD"
        private const int MAGIC_R = 0x42464144;

        public uint Channels { get; set; } = 2;
        public bool Encrypted { get; set; } = false;
        public uint TotalSamples { get; set; }
        public uint Bitrate { get; set; } = 96000;

        public ushort FrameSize { get; set; } = 960;
        public ushort Unknown1 { get; set; } = 312;
        public ushort SampleRate { get; set; } = 48000;
        public ushort Unknown2 { get; set; } = 1;

        public uint ReckoningOffset { get; set; } // Thing that counts
        public uint ReckoningSize { get; set; }

        public uint PacketStreamOffset { get; set; }
        public uint PacketStreamSize { get; set; }

        public byte[] Reckoning { get; set; }
        public byte[] PacketStream { get; set; }

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
                celt.Channels = ar.ReadUInt16();
                celt.Encrypted = Convert.ToBoolean(ar.ReadInt16());
                celt.TotalSamples = ar.ReadUInt32();
                celt.Bitrate = ar.ReadUInt32();

                celt.FrameSize = ar.ReadUInt16();
                celt.Unknown1 = ar.ReadUInt16();
                celt.SampleRate = ar.ReadUInt16();
                celt.Unknown2 = ar.ReadUInt16();

                celt.ReckoningOffset = ar.ReadUInt32();
                celt.ReckoningSize = ar.ReadUInt32();
                celt.PacketStreamOffset = ar.ReadUInt32();
                celt.PacketStreamSize = ar.ReadUInt32();
                celt.FixOffsets(); // Only useful for audio extracted from RAM, harmless
                
                uint reckonSize = celt.PacketStreamOffset - celt.ReckoningOffset; // Multiple of 4
                uint streamSize = celt.PacketStreamSize;
                
                if ((reckonSize + streamSize) % 16 != 0)
                    streamSize += 16 - ((reckonSize + streamSize) % 16);

                celt.Reckoning = ar.ReadBytes((int)reckonSize);
                celt.PacketStream = ar.ReadBytes((int)streamSize);
            }

            return celt;
        }

        private void FixOffsets()
        {
            if (PacketStreamOffset <= 40)
                return;
            
            ReckoningOffset = 40;
            PacketStreamOffset = ReckoningOffset + ReckoningSize;

            if (PacketStreamOffset % 4 != 0)
                PacketStreamOffset += 4 - (PacketStreamOffset % 4);
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
                aw.Write((ushort)Channels);
                aw.Write(Convert.ToUInt16(Encrypted));
                aw.Write((uint)TotalSamples);
                aw.Write((uint)Bitrate);

                aw.Write((ushort)FrameSize);
                aw.Write((ushort)Unknown1);
                aw.Write((ushort)SampleRate);
                aw.Write((ushort)Unknown2);

                aw.Write((uint)ReckoningOffset);
                aw.Write((uint)ReckoningSize);
                aw.Write((uint)PacketStreamOffset);
                aw.Write((uint)PacketStreamSize);

                aw.Write(Reckoning);
                aw.Write(PacketStream);
            }
        }

        public void WriteToWavFile(string outputPath)
        {
            if (Encrypted) throw new Exception("Audio stream is encrypted! Cannot proceed!");

            bool skipMode = true;
            int audioOffset = 0, reckonOffset = 0;
            OpusDecoder decoder = OpusDecoder.Create(SampleRate, (int)Channels);

            using (WaveFileWriter writer = new WaveFileWriter(outputPath, new WaveFormat(SampleRate, 16, (int)Channels))) // 16-bit PCM
            {
                while (reckonOffset < ReckoningSize)
                {
                    int numPackets = Reckoning[reckonOffset];

                    // Checks if first bit is 1
                    if ((numPackets & 0x80) == 0x80)
                        numPackets = ((numPackets ^ 0x80) << 8) | Reckoning[++reckonOffset];
                    
                    if (skipMode)
                    {
                        short[] outputShorts = new short[numPackets * FrameSize * Channels];
                        writer.WriteSamples(outputShorts, 0, outputShorts.Length);
                    }
                    else
                    {
                        int packetOffset = 0;
                        short[] outputShorts = new short[FrameSize * Channels];

                        // Decoding loop
                        while (packetOffset++ < numPackets && audioOffset < PacketStreamSize)
                        {
                            int packetSize = ((PacketStream[audioOffset++] & 0x0F) << 8) | PacketStream[audioOffset++]; // 12-bit encoding

                            // Decodes OPUS packet
                            decoder.Decode(PacketStream, audioOffset, packetSize, outputShorts, 0, FrameSize);

                            // Writes frame
                            writer.WriteSamples(outputShorts, 0, outputShorts.Length);
                            audioOffset += packetSize;
                        }
                    }

                    reckonOffset++;
                    skipMode = !skipMode;
                }
            }
        }
    }
}
