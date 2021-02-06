using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    /// <summary>
    /// Object that telling when we need to send a single drive
    /// </summary>
    class BusSendData
    {
        /// <summary>
        /// the id of the line that we want to send
        /// </summary>
        public int LineId { get; set; }
        /// <summary>
        /// the time to send that line
        /// </summary>
        public TimeSpan SendTime { get; set; }
    }
}
