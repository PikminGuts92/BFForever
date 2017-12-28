using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class PackageDef : ZObject
    {
        public PackageDef(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }

        internal override void ReadData(AwesomeReader ar)
        {
            throw new NotImplementedException();
        }
    }
}
