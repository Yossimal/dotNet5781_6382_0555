using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    /// <summary>
    /// data for the simulation do work event
    /// </summary>
    class DoWorkSimulationData
    {
        /// <summary>
        /// the cation to activate when updating the timer
        /// </summary>
        public Action<TimeSpan> updateTime;
        /// <summary>
        /// the stopwatch
        /// </summary>
        public SimulationStopwatch stopwatch;

    }
}
