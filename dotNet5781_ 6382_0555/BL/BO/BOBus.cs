using DALAPI;
using DALAPI.DAO;
using System;

namespace BL.BO
{
    /// <summary>
    /// presents a bus
    /// </summary>
    public class BOBus
    {
        internal const int MAX_FUEL= 2000;
        /// <summary>
        /// the bus license number
        /// </summary>
        public int LicenseNumber { get; set; }
        /// <summary>
        /// the bus license date
        /// </summary>
        public DateTime LicenseDate { get; set; }
        /// <summary>
        /// the bus current milage
        /// </summary>
        public double MileageCounter { get; set; }
        /// <summary>
        /// the bus remaining fuel
        /// </summary>
        public double FuelRemain { get; set; }
        /// <summary>
        /// the bus status
        /// </summary>
        public BusStatus Status { get; set; }
        /// <summary>
        /// the bus last care date
        /// </summary>
        public DateTime LastCareDate { get; set; }
        /// <summary>
        /// the time for the bus to get available
        /// </summary>
        public DateTime? TimeToReady { get; set; }
        /// <summary>
        /// is the bus available
        /// </summary>
        public bool IsAvailable => this.Status == BusStatus.Ready;
        /// <summary>
        /// the availability message (BusStatus may not be used if not needed)
        /// </summary>
        public string AvailabilityMassage
        {
            get
            {
                switch (this.Status)
                {
                    case BusStatus.Drive:
                        return "The bus is in a drive.";
                        //break;
                    case BusStatus.InCare:
                        return "The bus is currently in care.";
                        //break;
                    case BusStatus.Refuel:
                        return "The bus currently refueling.";
                        //break;
                    case BusStatus.Ready:
                        return "The bus is ready to drive";
                        //break;
                    default:
                        return "no-message";
                }
                //return null;
            }
        }
        /// <summary>
        /// the bus status 
        /// </summary>
        public string StatusStr => this.Status.ToString();
        /// <summary>
        /// the next care date
        /// </summary>
        public DateTime NextCareDate =>
            new DateTime(LastCareDate.Year + 1, LastCareDate.Month,
                LastCareDate.Day);
        public BOBus() { }
        internal BOBus(DAOBus bus)
        {
            LicenseDate = bus.LicenseDate;
            LicenseNumber = bus.LicenseNumber;
            MileageCounter = bus.MileageCounter;
            FuelRemain = bus.FuelRemain;
            Status = bus.Status;
            TimeToReady = bus.TimeToReady;
            LastCareDate = bus.LastCareDate;
        }
    }
}
