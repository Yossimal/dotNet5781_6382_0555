using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    /// <summary>
    /// data for adding station to the database
    /// </summary>
    public class BOAddStation
    {
        /// <summary>
        /// the station to add
        /// </summary>
        public BOStation Station { get; set; }
        /// <summary>
        /// coordinate variables for the station
        /// </summary>
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
