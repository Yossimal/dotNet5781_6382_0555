using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// thrown when the user name is illigal
    /// </summary>
    public class BadUsernameException : InvalidOperationException
    {
        public BadUsernameException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
