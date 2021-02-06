using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// thrown when the manager code is incorrect
    /// </summary>
    public class BadManagerCodeException:InvalidOperationException
    {
        public BadManagerCodeException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
