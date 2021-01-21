using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class BOLineTiming
    {
        public int LineId { get; set; }
        public int LineNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string LastStationName { get; set; }
        internal int LastStationId { get; set; }
    }
}
