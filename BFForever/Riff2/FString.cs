using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class FString
    {
        private long _stringKey;
        private readonly FEnvironment _environment;

        public FString(long key, FEnvironment env)
        {
            _stringKey = key;
            _environment = env;
        }

        public long Key => _stringKey;
        public string Value
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
