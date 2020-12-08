using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dotNet5781_03B_6382_0555.EventsObjects
{
    class RefuelBGWData
    {
        public Bus Bus { get; set; }
        public Button Button { get; set; }
        public ProgressBar ProgressBar { get; set; }
        public TextBlock ProgressTextBlock { get; set; }
        public TimeSpan? RemainingTime { get; set; }
        public  Image NoFuelImage { get; set; }

        public RefuelBGWData()
        {
        }

       
    }
}
