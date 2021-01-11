using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class LineStationModel
    {
        public StationModel Station { get; set; }
        public StationModel Next { get; set; }
        public StationModel Prev { get; set; }
        public double DistanceFromNext { get; set; }
        public TimeSpan TimeFromNext { get; set; }
    }
}
