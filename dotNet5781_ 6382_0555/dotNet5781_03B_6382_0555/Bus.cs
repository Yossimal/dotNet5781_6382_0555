using System;

namespace dotNet5781_03B_6382_0555
{
    enum status { }
    public class Bus
    {


        /// <summary>
        /// Building bus from license nuber and date
        /// </summary>
        /// <param name="licenseNumber">The license number of the bus</param>
        /// <param name="startWorkingDate">The date of the bus first ride</param>
        public Bus(int licenseNumber, DateTime startWorkingDate,int mileage)
        {
            LicenseNumber = licenseNumber;
            StartWorkingDate = startWorkingDate;
            MileageAfterCare = 0;
            KmAfterRefueling = 0;
            LastCareDate = DateTime.Now;
            MileageCounter = mileage;
            TimeToReady = null;
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="toCopy">The bus to copy</param>
        public Bus (Bus toCopy) {
            LicenseNumber = toCopy.LicenseNumber;
            StartWorkingDate = toCopy.StartWorkingDate;
            MileageAfterCare = toCopy.MileageAfterCare;
            KmAfterRefueling = toCopy.KmAfterRefueling;
            LastCareDate = toCopy.LastCareDate;
            MileageCounter = toCopy.MileageCounter;
            TimeToReady = toCopy.TimeToReady;
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

                TimeSpan span = DateTime.Now-LastCareDate; 

                int yearsAfterCare = (zeroTime + span).Year - 1;
                return MileageAfterCare > MaxKmBeforeCare && yearsAfterCare >= 1;
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

        /// <summary>
        /// refuling the bus
        /// </summary>
        public void Refuel()
        {
            KmAfterRefueling = 0;
            this.TimeToReady=DateTime.Now+TimeToRefuel;
        }
        /// <summary>
        /// Taking care of the bus
        /// </summary>
        public void TakeCare()
        {
            MileageAfterCare = 0;
            LastCareDate = DateTime.Now;
        }
        /// <summary>
        /// Check if the bus can drive the given distance
        /// </summary>
        /// <param name="distance">The distance that the bus want to drive</param>
        /// <returns>Can he drive that distance</returns>
        public bool CanDrive(int distance)
        {
            return !IsDanger && distance < (MaxKmAfterRefueling - KmAfterRefueling)&&TimeToReady<DateTime.Now;
        }
        /// <summary>
        /// Driving a given distance
        /// </summary>
        /// <param name="distance">The distance that the bus want to drive</param>
        /// <returns>true if he could drive, else false</returns>
        public bool Drive(int distance)
        {
            if (!CanDrive(distance))
            {
                return false;
            }
            KmAfterRefueling += distance;
            MileageCounter += distance;
            MileageAfterCare += distance;
            return true;
        }

        public TimeSpan TimeToRefuel {get=>new TimeSpan(0,0,5); }

    }
}
