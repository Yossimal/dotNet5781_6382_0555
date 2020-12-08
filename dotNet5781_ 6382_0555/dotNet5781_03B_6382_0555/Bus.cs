using System;

namespace dotNet5781_03B_6382_0555
{
    public class Bus
    {
        private DateTime lastDrive;

        /// <summary>
        /// Building bus from license nuber and date
        /// </summary>
        /// <param name="licenseNumber">The license number of the bus</param>
        /// <param name="startWorkingDate">The date of the bus first ride</param>
        public Bus(int licenseNumber, DateTime startWorkingDate, int mileage)
        {
            this.lastDrive = DateTime.Today;
            LicenseNumber = licenseNumber;
            StartWorkingDate = startWorkingDate;
            MileageAfterCare = 0;
            KmAfterRefueling = 0;
            LastCareDate = DateTime.Now;
            MileageCounter = mileage;
            TimeToReady = null;
            DrivingTimeToday = new TimeSpan(0, 0, 0);
            this.Status = Status.Ready;
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toCopy">The bus to copy</param>
        public Bus(Bus toCopy)
        {
            LicenseNumber = toCopy.LicenseNumber;
            StartWorkingDate = toCopy.StartWorkingDate;
            MileageAfterCare = toCopy.MileageAfterCare;
            KmAfterRefueling = toCopy.KmAfterRefueling;
            LastCareDate = toCopy.LastCareDate;
            MileageCounter = toCopy.MileageCounter;
            TimeToReady = toCopy.TimeToReady;
            DrivingTimeToday = toCopy.DrivingTimeToday;
            lastDrive = toCopy.lastDrive;
        }

        public DateTime? TimeToReady { get; set; }
        /// <summary>
        /// The Maximum km that the bus can drive before another care
        /// </summary>
        public static int MaxKmBeforeCare { get => 20000; }
        /// <summary>
        /// the maximum km that the bus can drive befor it need to reful
        /// </summary>
        public static int MaxKmAfterRefueling { get => 1200; }
        /// <summary>
        /// Is the bus declared as danger
        /// </summary>
        public bool IsDanger
        {
            get
            {
                DateTime zeroTime = new DateTime(1, 1, 1);

                TimeSpan span = DateTime.Now - LastCareDate;

                int careCheckData = (int)(span.TotalDays - 366);
                return MileageAfterCare > MaxKmBeforeCare || careCheckData > 0;
            }
        }
        /// <summary>
        /// The bus license number
        /// </summary>
        public int LicenseNumber { get; }
        /// <summary>
        /// The bus first riding date
        /// </summary>
        public DateTime StartWorkingDate { get; }
        /// <summary>
        /// How mant km the bus drives after the last care
        /// </summary>
        public int MileageAfterCare { get; private set; }
        /// <summary>
        /// Counting all the km that the bus drived ever
        /// </summary>
        public int MileageCounter { get; private set; }
        /// <summary>
        /// how many km the bus drived after the last refuling
        /// </summary>
        public int KmAfterRefueling { get; private set; }
        /// <summary>
        /// The date of the lst care of the bus
        /// </summary>
        public DateTime LastCareDate { get; private set; }
        public static TimeSpan MaxDriveTimeInDay { get => new TimeSpan(0, 12, 0, 0); }
        public TimeSpan DrivingTimeToday { get; set; }
        public static double AverageBusSpeed => 35;

        public bool CanDriveToday
        {
            get
            {
                return this.DrivingTimeToday < Bus.MaxDriveTimeInDay 
                       && this.lastDrive.Day == DateTime.Today.Day;
            }
        }

        /// <summary>
        /// Refueling the bus
        /// </summary>
        public virtual void Refuel()
        {
            KmAfterRefueling = 0;
            this.Status = Status.Refuel;
            this.TimeToReady = DateTime.Now + Tools.SimulationTime(TimeToRefuel);
        }
        /// <summary>
        /// Taking care of the bus
        /// </summary>
        public virtual void TakeCare()
        {
            MileageAfterCare = 0;
            this.Status = Status.InCare;
            this.TimeToReady = DateTime.Now + Tools.SimulationTime(this.TimeToCare);
            LastCareDate = DateTime.Now;
        }
        /// <summary>
        /// Check if the bus can drive the given distance
        /// </summary>
        /// <param name="distance">The distance that the bus want to drive</param>
        /// <returns>Can he drive that distance</returns>
        public bool CanDrive(int distance)
        {
            return !IsDanger 
                   && MileageAfterCare + distance < MaxKmBeforeCare 
                   && distance < (MaxKmAfterRefueling - KmAfterRefueling) 
                   && this.Status == Status.Ready 
                   && (DrivingTimeToday+AverageTime(distance)) < MaxDriveTimeInDay;
        }
        /// <summary>
        /// Driving a given distance
        /// </summary>
        /// <param name="distance">The distance that the bus want to drive</param>
        /// <returns>true if he could drive, else false</returns>
        public virtual bool Drive(int distance)
        {
            if (!CanDrive(distance))
            {
                return false;
            }

            this.Status = Status.Drive;
            int speed = Tools.RandomInt(20, 50);
            TimeSpan drivingTime = TimeSpan.FromHours((double)distance / (double)speed);
            if (DateTime.Today.Day != lastDrive.Day)
            {
                this.DrivingTimeToday=new TimeSpan(0);
            }

            this.DrivingTimeToday += drivingTime;
            TimeToReady = DateTime.Now + Tools.SimulationTime(drivingTime);
            KmAfterRefueling += distance;
            MileageCounter += distance;
            MileageAfterCare += distance;
            return true;
        }

        public TimeSpan TimeToRefuel { get => new TimeSpan(2, 0, 0); }
        public Status Status { get; set; }
        public TimeSpan TimeToCare { get => new TimeSpan(1, 0, 0, 0); }
        public static string FormatLicense(int license)
        {
            string asString = license.ToString();
            if (asString.Length == 8)
            {
                return asString.Substring(0, 3) + '-' + asString.Substring(3, 2) + '-' + asString.Substring(5);
            }
            return asString.Substring(0, 2) + '-' + asString.Substring(2, 3) + '-' + asString.Substring(5);
        }

        public static TimeSpan AverageTime(double distance)
        {
            return TimeSpan.FromHours(distance / AverageBusSpeed);
        }

        public static Bus[] InitializeBuses()
        {
            Bus[] ret = new Bus[10];
            ret[0] = new Bus(Tools.RandomInt(10000000, 10000050), new DateTime(2020, Tools.RandomInt(1, 5), Tools.RandomInt(1, 15)), MaxKmBeforeCare - 1);
            ret[0].MileageAfterCare = MaxKmBeforeCare - 1;
            ret[1] = new Bus(Tools.RandomInt(10000051, 10000100), new DateTime(2018, Tools.RandomInt(1, 5), Tools.RandomInt(1, 15)), MaxKmBeforeCare - 1000);
            ret[1].LastCareDate = new DateTime(2018, Tools.RandomInt(6, 8), Tools.RandomInt(15, 25));
            ret[2] = new Bus(Tools.RandomInt(10000101, 10000150), new DateTime(Tools.RandomInt(2018, 2020), Tools.RandomInt(1, 5), Tools.RandomInt(1, 15)), MaxKmBeforeCare - 2000);
            ret[2].KmAfterRefueling = MaxKmAfterRefueling - 1;
            for (int i = 3; i < ret.Length; i++)
            {
                ret[i] = new Bus(Tools.RandomInt(10000000 * (i - 1), 10000000 * i), new DateTime(Tools.RandomInt(2018, 2020), Tools.RandomInt(1, 5), Tools.RandomInt(1, 15)), MaxKmBeforeCare - Tools.RandomInt(0, MaxKmBeforeCare));
            }
            return ret;
        }


    }
}
