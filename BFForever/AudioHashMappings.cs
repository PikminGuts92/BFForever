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

        public bool ShouldSerializePreview() => Preview.Input != 0 && Preview.Output != 0;
        public bool ShouldSerializeBacking() => Backing.Input != 0 && Backing.Output != 0;
        public bool ShouldSerializeBass() => Bass.Input != 0 && Bass.Output != 0;
        public bool ShouldSerializeDrums() => Drums.Input != 0 && Drums.Output != 0;
        public bool ShouldSerializeLeadGuitar() => LeadGuitar.Input != 0 && LeadGuitar.Output != 0;
        public bool ShouldSerializeRhythmGuitar() => RhythmGuitar.Input != 0 && RhythmGuitar.Output != 0;
        public bool ShouldSerializeVox() => Vox.Input != 0 && Vox.Output != 0;

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

        public static AudioHashMappings Import(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<AudioHashMappings>(json);
        }
    }
}
