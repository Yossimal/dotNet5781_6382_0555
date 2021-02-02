using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    class DoWorkSimulationData
    {
        public Action<TimeSpan> updateTime;
        public SimulationStopwatch stopwatch;

    }
}
