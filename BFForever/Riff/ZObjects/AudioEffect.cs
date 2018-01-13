using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * AudioEffect ZObject
 * ===================
 * INT32 - Constant (7)
 * INT32 - Event Size (16)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff
{
    public class AudioEffect : ZObject
    {
        public AudioEffect(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<AudioEffectEntry>();
        }

        protected override void AddMemberStrings(List<FString> strings) => strings.AddRange(Events.Select(x => x.EffectPath));

        internal override void ReadData(AwesomeReader ar)
        {
            Events.Clear();
            ar.BaseStream.Position += 8; // Skips constants

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            for (int i = 0; i < count; i++)
            {
                AudioEffectEntry ev = new AudioEffectEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ev.EffectPath = ar.ReadUInt64();

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)7);
            aw.Write((int)16);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach(AudioEffectEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((ulong)ev.EffectPath);
            }
        }

        public override HKey Type => Global.ZOBJ_AudioEffect;

        public List<AudioEffectEntry> Events { get; set; }
    }

    public class AudioEffectEntry : TimeEvent
    {
        public HKey EffectPath { get; set; }
    }
}
