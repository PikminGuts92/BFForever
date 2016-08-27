using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class TimeEntry
    {
        private float _start;
        private float _end;

        /// <summary>
        /// Abstract constructor for time entry.
        /// </summary>
        public TimeEntry()
        {
            Start = 0.0f;
            End = 0.0f;
        }

        /// <summary>
        /// Gets or sets start time (ms)
        /// </summary>
        public float Start
        {
            get { return _start; }
            set
            {
                if (value >= 0.0f)
                    _start = value;
                else
                    _start = 0.0f;
            }
        }

        /// <summary>
        /// Gets or sets end time (ms)
        /// </summary>
        public float End
        {
            get { return _end; }
            set
            {
                if (value >= 0.0f && value >= _start)
                    _end = value;
                else
                    // End should never be less than start
                    _end = _start;
            }
        }
    }
}
