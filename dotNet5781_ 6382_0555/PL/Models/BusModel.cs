using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class BusModel
    {
        public string LicenseNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string AvailabilityMessage { get; set; }
        public double FuelRemain { get; set; }
        public double MilageCounter { get; set; }
        //In PL the status is string because we need to show it without using logic
        public string BusStatus { get; set; }
        public DateTime LicenseDate { get; set; }
    }
}
