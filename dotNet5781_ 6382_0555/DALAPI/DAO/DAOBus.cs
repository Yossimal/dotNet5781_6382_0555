using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    /// <summary>
    /// Bus entity - represents basic data of a bus
    /// </summary>
    public class DAOBus
    {
        #region BackgroundData
        public bool IsRunningId => false;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
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
        public DateTime LastCareDate { get; set; }
        public DateTime? TimeToReady { get; set; }
        #endregion
    }
}
