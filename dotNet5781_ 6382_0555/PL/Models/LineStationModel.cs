using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class LineStationModel
    {
        private TimeSpan _timeFromNext;
        private double _distanceFromNext;

        public StationModel Station { get; set; }
        public StationModel Next { get; set; }
        public StationModel Prev { get; set; }
        public double DistanceFromNext
        {
            get => Math.Round(_distanceFromNext, 3);
            set => _distanceFromNext = value;
        }
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
