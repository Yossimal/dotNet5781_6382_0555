using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    /// <summary>
    /// Holding data for line in drive
    /// </summary>
    public class BOLineTiming
    {
        /// <summary>
        /// the line id
        /// </summary>
        public int LineId { get; set; }
        /// <summary>
        /// the line number
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// the line start time
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// the time for arrival
        /// </summary>
        public TimeSpan ArrivalTime { get; set; }
        /// <summary>
        /// the name of the last station in the path
        /// </summary>
        public string LastStationName { get; set; }
        /// <summary>
        /// the id of the station that we want to track
        /// </summary>
        public int TrackStationId { get; set; }
    }
}
