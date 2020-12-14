using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dotNet5781_03B_6382_0555.EventsObjects
{
    /// <summary>
    /// Object for handling the Drive event
    /// </summary>
    public class DoDriveEventArgs:EventArgs
    {
        public int Distance { get; set; }
        public Button ClickedButton { get; set; }

        public DoDriveEventArgs(int distance,Button clickedButton)
        {
            this.Distance = distance;
            this.ClickedButton = clickedButton;

        }
    }
}
