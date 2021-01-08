using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class InnerItemNotFoundException : ItemNotFoundException
    {
        public InnerItemNotFoundException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
