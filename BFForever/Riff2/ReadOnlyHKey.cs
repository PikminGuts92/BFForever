using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class ReadOnlyHKey : FString
    {
        public ReadOnlyHKey(long key) : base(key)
        {

        }
        
        public override string Value
        {
            get => FEnvironment.GetStringValue(_stringKey);
        }
    }
}
