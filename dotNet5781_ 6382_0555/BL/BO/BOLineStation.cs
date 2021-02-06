using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class BOLineStation:BOStation
    {
        /// <summary>
        /// the line of that LineStation
        /// </summary>
        public BOLine Line { get; set; }
        /// <summary>
        /// the distance from the next station
        /// </summary>
        public double DistanceFromNext { get; set; }
        /// <summary>
        /// the travel time from the next station
        /// </summary>
        public TimeSpan TimeFromNext { get; set; }
    }
}
