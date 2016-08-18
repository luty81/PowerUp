using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp
{
    public class Interval
    {
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public Interval(DateTime start, DateTime? end = null)
        {
            Start = start;
            End = end;
        }
    }
}
