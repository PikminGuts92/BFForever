using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class TextEventEntry : TimeEntry
    {
        public TextEventEntry()
        {

        }

        /// <summary>
        /// Gets or sets event name (ex: "Intro", "Verse 1", etc.)
        /// </summary>
        public FString EventName { get; set; }
    }
}
