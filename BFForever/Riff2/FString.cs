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
        protected readonly FEnvironment _environment;

        public FString(long key, FEnvironment env)
        {
            _stringKey = key;
            _environment = env;
        }

        public long Key => _stringKey;
        public virtual string Value
        {
            get => _environment.GetStringValue(_stringKey);
            set
            {
                // TODO: Implement this
                throw new NotImplementedException();
            }
        }

        public override string ToString() => Value;
    }
}
