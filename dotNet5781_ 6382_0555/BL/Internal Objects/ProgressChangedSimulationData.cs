using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    class ProgressChangedSimulationData
    {
        public TimeSpan currentTime;
        public Action<TimeSpan> updateTime;
    }
}
