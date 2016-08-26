using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BFForever.Riff
{
    public class RiffFile
    {
        /// <summary>
        /// Constructor for Riff file
        /// </summary>
        public RiffFile()
        {
            BigEndian = true;
            Objects = new List<ZObject>();
        }

        /// <summary>
        /// Imports objects from riff file.
        /// </summary>
        /// <param name="input">Riff file path</param>
        public void Import(string input)
        {
            if (!File.Exists(input)) return; // Returns if file doesn't exist

            using (FileStream fs = File.OpenRead(input))
            {
                // Imports objects from file
                //ParseRiff(fs);
            }
        }

        public bool BigEndian { get; set; }

        public List<ZObject> Objects { get; set; }
        
    }
}
