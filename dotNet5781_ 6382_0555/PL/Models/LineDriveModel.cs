using System;

namespace PL.Models
{
    class LineDriveModel
    {
        /// <summary>
        /// the line Id
        /// </summary>
        public int LineId { get; set; }
        /// <summary>
        /// the line number
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// the arrival time of the line
        /// </summary>
        public TimeSpan NearestArrivalTime {get;set;}
        /// <summary>
        /// the arrival time of the line for display
        /// </summary>
        public string NearestArrivalTimeAsString { get => $"{NearestArrivalTime.Hours}:{NearestArrivalTime.Minutes}:{NearestArrivalTime.Seconds}"; }
        /// <summary>
        /// the name of the last station in the line
        /// </summary>
        public string LastStationName { get; set; }
    }
}
