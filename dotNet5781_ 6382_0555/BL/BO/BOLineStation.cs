using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class BOLineStation:BOStation
    {
        public BOLine Line { get; set; }
        public double DistanceFromNext { get; set; }
        public TimeSpan TimeFromNext { get; set; }
    }
}
