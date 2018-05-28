using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BFForever
{
    public class AudioHashMappings
    {
        public HashMapping Preview { get; set; }
        public HashMapping Backing { get; set; }
        public HashMapping Bass { get; set; }
        public HashMapping Drums { get; set; }
        public HashMapping LeadGuitar { get; set; }
        public HashMapping RhythmGuitar { get; set; }
        public HashMapping Vox { get; set; }

        public void Export(string path)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(this, settings);
            File.WriteAllText(path, json);
        }

        public static HashMapping Import(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<HashMapping>(json);
        }
    }
}
