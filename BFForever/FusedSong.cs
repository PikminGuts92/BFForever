﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using BFForever.Riff;

namespace BFForever
{
    public class FusedSong
    {
        public string Identifier { get; set; } = "";
        public string Author { get; set; } = "";

        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public string Album { get; set; } = "";
        public string Description { get; set; } = "";

        public float SongLength { get; set; }
        public float GuitarIntensity { get; set; }
        public float BassIntensity { get; set; }
        public float VoxIntensity { get; set; }

        public int Year { get; set; } = DateTime.Now.Year;

        // Tunings
        public InstrumentTuning LeadGuitarTuning { get; set; } = InstrumentTuning.Guitar_EStandard;
        public InstrumentTuning RhythmGuitarTuning { get; set; } = InstrumentTuning.Guitar_EStandard;
        public InstrumentTuning BassTuning { get; set; } = InstrumentTuning.Bass_EStandard;

        // External Files
        public string TabPath { get; set; } = "";
        public string TexturePath { get; set; } = "";
        public string VideoPath { get; set; } = "";

        public AudioPaths AudioPaths { get; set; } = new AudioPaths();

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

        public static FusedSong Import(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<FusedSong>(json);
        }
    }
}
