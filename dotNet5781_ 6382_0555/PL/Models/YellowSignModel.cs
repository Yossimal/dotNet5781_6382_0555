using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class YellowSignModel
    {
        /// <summary>
        /// the line number
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// the name of the last station in line
        /// </summary>
        public string LastStationName { get; set; }
    }
}
