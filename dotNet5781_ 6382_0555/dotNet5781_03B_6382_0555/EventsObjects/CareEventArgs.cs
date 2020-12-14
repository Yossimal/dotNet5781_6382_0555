using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dotNet5781_03B_6382_0555.EventsObjects
{
    /// <summary>
    /// Object for handling the Care event in the BusData window
    /// </summary>
    public class CareEventArgs:EventArgs
    {
        public Bus Bus { get; set; }
        public  Control ItemControl { get; set; }

        public CareEventArgs(Bus bus,Control itemControl)
        {
            this.Bus = bus;
            this.ItemControl = itemControl;
        }
    }
}
