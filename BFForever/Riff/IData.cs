using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public interface IData
    {
        /// <summary>
        /// Returns every object in data structure (not including header data)
        /// </summary>
        /// <returns>Objects</returns>
        IReadOnlyCollection<object> GetObjects();
    }
}
