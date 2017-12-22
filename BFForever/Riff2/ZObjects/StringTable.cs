using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class StringTable : ZObject
    {
        public StringTable(HKey filePath, HKey directoryPath, HKey type) : base(filePath, directoryPath, type)
        {
        }

        internal override void ReadData(AwesomeReader ar, FEnvironment env)
        {
            throw new NotImplementedException();
        }
    }
}
