using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class Catalog2 : ZObject
    {
        public Catalog2(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }

        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            throw new NotImplementedException();
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            throw new NotImplementedException();
        }

        protected override HKey Type => Hashes.ZOBJ_Catalog2;
    }
}
