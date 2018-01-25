using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * VoxSpread ZObject
 * =================
 * INT32 - Constant (2)
 * INT32 - Event Size (12)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff
{
    // It's really just the same thing as regular spread
    public class VoxSpread : Spread
    {
        public VoxSpread(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<SpreadEntry>();
        }
        
        public override HKey Type => Global.ZOBJ_VoxSpread;
    }
}