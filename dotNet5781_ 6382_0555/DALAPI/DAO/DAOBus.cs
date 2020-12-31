using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOBus
    {
        #region BackgroundData
        public bool IsRunningId => true;
        public bool IsDeleted { get; set; }
        public int Id;
        #endregion
        #region realData
        public int LicenseNumber
        {
            get => this.Id;
            set => this.Id = value;
        }
        public DateTime LicenseDate { get; set; }
        public double MileageCounter { get; set; }
        public double FuelRemain { get; set; }
        public BusStatus Status { get; set; }
        #endregion
    }
}
