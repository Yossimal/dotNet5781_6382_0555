using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// thrown when trying to add item that already exists to the database (with the same primary key)
    /// </summary>
    public class ItemAlreadyExistsException : InvalidOperationException
    {
        public ItemAlreadyExistsException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
