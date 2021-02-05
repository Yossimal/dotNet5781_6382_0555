using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class LineDriveModel
    {
        public int LineId { get; set; }
        public int LineNumber { get; set; }
        public TimeSpan NearestArrivalTime {get;set;}
        public string NearestArrivalTimeAsString { get => $"{NearestArrivalTime.Hours}:{NearestArrivalTime.Minutes}:{NearestArrivalTime.Seconds}"; }
        public string LastStationName { get; set; }
    }
}
