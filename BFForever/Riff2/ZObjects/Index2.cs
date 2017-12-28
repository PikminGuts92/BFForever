using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class Index2 : ZObject
    {
        public Index2(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }
        
        internal override void ReadData(AwesomeReader ar)
        {
            throw new NotImplementedException();
        }
    }
}
