using DALAPI;
using DALAPI.DAO;
using System;

namespace BL.BO
{
    public class BOBus
    {
        public const double FUEL_TO_WARNING = 20;
        public static readonly TimeSpan TIME_BEFORE_CARE_TO_WARNING = new TimeSpan(10, 0, 0);
        public const double MAX_FUEL = 500d;

        public int LicenseNumber { get; set; }
        public DateTime LicenseDate { get; set; }
        public double MileageCounter { get; set; }
        public double FuelRemain { get; set; }
        public BusStatus Status { get; set; }
        public DateTime LastCareDate { get; set; }
        public DateTime? TimeToReady { get; set; }
        public bool IsAvailable => this.Status == BusStatus.Ready;
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
        public string StatusStr => this.Status.ToString();
        public DateTime NextCareDate =>
            new DateTime(LastCareDate.Year + 1, LastCareDate.Month,
                LastCareDate.Day);
        public BOBus() { }
        public BOBus(DAOBus bus)
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
