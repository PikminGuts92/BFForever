using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public enum Language : long
    {
        English = 5412069155413958780L,
        Japanese = -4916594395764136780L,
        German = 8434362063832740322L,
        Italian = 4181558474080832064L,
        Spanish = -1868168087102288302L,
        French = 6388165613802289312L
    }

    public class StringTable : Chunk
    {
        public StringTable(FString idx) : base(idx)
        {
            TableLanguage = Language.English; // Speak 'murican
            Strings = new List<FString>();
        }

        public FString BaseDirectory { get; set; }
        public Language TableLanguage { get; set; }
        public List<FString> Strings { get; set; }

        public override void ImportData(AwesomeReader ar)
        {
            BaseDirectory = ar.ReadInt64(); // songs.Halestorm.LoveBites
            TableLanguage = (Language)ar.ReadInt64();
            ar.BaseStream.Position += 8; // Skips zeros

            Dictionary<long, string> dic = new Dictionary<long, string>();
            int count = ar.ReadInt32();
            ar.BaseStream.Position += 12; // Skips to entries
            long difference = ar.BaseStream.Position + (count * 16);

            long[] key = new long[count];
            long[] offset = new long[count];

            for (int i = 0; i < count; i++)
            {
                key[i] = ar.ReadInt64();
                offset[i] = ar.ReadInt32();
                ar.ReadInt32(); // Should be zero
            }

            for (int i = 0; i < count; i++)
            {
                ar.BaseStream.Position = offset[i] + difference;
                string text = ar.ReadNullString();

                if (!dic.ContainsKey(key[i]))
                {
                    dic.Add(key[i], text);
                    continue;
                }

                // Evidently the same string with different casings will have the same key (only for directory paths?)
                if (string.Compare(dic[key[i]], text, true) == 0)
                {
                    dic.Remove(key[i]);
                    dic.Add(key[i], text);
                }
                else
                {
                    Console.WriteLine("STRING ERROR: {0} != {1}", dic[key[i]], text);
                    //subDictionary.Add(key[i], text);
                }
            }

            Strings = new List<FString>();

            foreach (var d in dic)
            {
                // Finds/adds key string globally
                StringKey sk = StringKey.FindCreate(d.Key);
                sk.SetValue(d.Value, TableLanguage);

                // Adds string to string table collection (And adds globally)
                StringKey.AddString(sk);
                Strings.Add(sk.Key);
            }
        }
    }
}
