﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BadPasswordException:InvalidOperationException
    {
        public BadPasswordException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
