﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// thrown when the login data is incorrect
    /// </summary>
    public class BadLoginDataException:InvalidOperationException
    {
        public BadLoginDataException(string message, Exception innerException=null) : base(message, innerException) { }
    }
}
