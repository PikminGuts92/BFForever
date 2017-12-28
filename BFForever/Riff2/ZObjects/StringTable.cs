using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class StringTable : ZObject
    {
        private readonly Localization _localization;

        public StringTable(HKey filePath, HKey directoryPath, Localization localization = Localization.English) : base(filePath, directoryPath)
        {
            _localization = Localization;
        }
        
        internal override void ReadData(AwesomeReader ar)
        {
            throw new NotImplementedException();
        }

        public Localization Localization => _localization;
    }
}
