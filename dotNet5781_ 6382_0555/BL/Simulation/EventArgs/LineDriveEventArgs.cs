using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Simulation.EventArgs
{
    /// <summary>
    /// Object for handling the results of specific line simulation
    /// </summary>
    public class LineDriveEventArgs:System.EventArgs
    {
        /// <summary>
        /// data of specific bus in route
        /// </summary>
        public BOLineTiming lineTiming { get; set; }
    }
}
