using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// thrown when cant get an inner item in the database
    /// </summary>
    public class InnerItemNotFoundException : ItemNotFoundException
    {
        public InnerItemNotFoundException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
