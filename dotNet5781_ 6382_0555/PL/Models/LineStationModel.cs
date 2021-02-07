using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class LineStationModel
    {
        /// <summary>
        /// the travel time duration from the next station
        /// </summary>
        private TimeSpan _timeFromNext;
        /// <summary>
        /// the distance from the next station
        /// </summary>
        private double _distanceFromNext;

        /// <summary>
        /// the current station
        /// </summary>
        public StationModel Station { get; set; }
        /// <summary>
        /// the next station
        /// </summary>
        public StationModel Next { get; set; }
        /// <summary>
        /// the previos station
        /// </summary>
        public StationModel Prev { get; set; }
        /// <summary>
        /// fullprop for the distance from the next station
        /// </summary>
        public double DistanceFromNext
        {
            get => Math.Round(_distanceFromNext, 3);
            set => _distanceFromNext = value;
        }
        /// <summary>
        /// full prop for the travel time from the next station
        /// </summary>
        public TimeSpan TimeFromNext
        {
            get
            {
                return new TimeSpan(_timeFromNext.Hours, _timeFromNext.Minutes, 0);
            }
            set => _timeFromNext = value;
        }

    }
}
