using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Simulation.EventArgs
{
    public class LineDriveEventArgs:System.EventArgs
    {
        public BOLineTiming lineTiming { get; set; }
    }
}
