using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// thrown trying to use unavailable bus
    /// </summary>
    public class BusNotAvailableException : InvalidOperationException
    {
        public BusNotAvailableException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
