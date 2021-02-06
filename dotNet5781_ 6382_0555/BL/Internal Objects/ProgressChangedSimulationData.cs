using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    /// <summary>
    /// data for the progress changed event handler of the simulation
    /// </summary>
    class ProgressChangedSimulationData
    {
        /// <summary>
        /// the current time
        /// </summary>
        public TimeSpan currentTime;
        /// <summary>
        /// action to invoke
        /// </summary>
        public Action<TimeSpan> updateTime;
    }
}
