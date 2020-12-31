using DALAPI;
using DALAPI.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class BOBus
    {
        public int LicenseNumber {get;set;}
        public DateTime LicenseDate { get; set; }
        public double MileageCounter { get; set; }
        public double FuelRemain { get; set; }
        public BusStatus Status { get; set; }

        public BOBus() { }
        public BOBus(DAOBus bus) {
            LicenseDate = bus.LicenseDate;
            LicenseNumber = bus.LicenseNumber;
            MileageCounter = bus.MileageCounter;
            FuelRemain = bus.FuelRemain;
            Status = bus.Status;
        }
    }
}
