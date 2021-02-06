using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    /// <summary>
    /// a station with location paramater
    /// </summary>
    class Station
    {
        /// <summary>
        /// the station
        /// </summary>
        internal BOStation PublicData { get; set; }
        /// <summary>
        /// the station location
        /// </summary>
        internal Location Location { get; set; }

    }
}
