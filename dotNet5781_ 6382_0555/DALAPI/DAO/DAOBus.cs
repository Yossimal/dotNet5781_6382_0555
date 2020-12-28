using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOBus:DAOBasic
    {
        public override bool IsRunningId => true;

        public int LicenseNumber
        {
            get => this.Id;
            set => this.Id = value;
        }
        public DateTime LicenseDate { get; set; }
        public double MileageCounter { get; set; }
        public double FuelRemain { get; set; }
        public BusStatus Status { get; set; }
    }
}
