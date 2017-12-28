using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class FString
    {
        protected long _stringKey;

        public FString(long key)
        {
            _stringKey = key;
        }

        public long Key => _stringKey;
        public virtual string Value
        {
            get => FEnvironment.GetStringValue(_stringKey);
            set
            {
                // TODO: Implement this
                throw new NotImplementedException();
            }
        }

        public override string ToString() => Value;
    }
}
