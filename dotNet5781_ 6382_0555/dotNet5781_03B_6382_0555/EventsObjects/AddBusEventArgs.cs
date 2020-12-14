using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_6382_0555
{
    /// <summary>
    /// Item for handling the AddBus event in the AddBus window
    /// </summary>
    public class AddBusEventArgs : EventArgs
    {
        public Bus Bus { get; }
        public AddBusEventArgs(Bus bus)
        {
            this.Bus = bus;
        }
    }
}
