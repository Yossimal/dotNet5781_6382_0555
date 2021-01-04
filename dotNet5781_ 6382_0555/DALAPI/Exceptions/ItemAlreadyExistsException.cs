using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALAPI
{
    public class ItemAlreadyExistsException:InvalidOperationException
    {
        public ItemAlreadyExistsException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
