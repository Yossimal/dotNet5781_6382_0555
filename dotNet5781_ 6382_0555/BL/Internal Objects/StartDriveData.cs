using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    /// <summary>
    /// the drive data
    /// </summary>
    internal class StartDriveData
    {
        /// <summary>
        /// the start drive time
        /// </summary>
        internal TimeSpan StartTime { get; set; }
        /// <summary>
        /// the line id
        /// </summary>
        internal int LineId { get; set; }

    }
}
