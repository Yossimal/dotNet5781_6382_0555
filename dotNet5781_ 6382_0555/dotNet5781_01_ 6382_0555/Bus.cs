using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_01__6382_0555
{
    class Bus
    {
        private int licenseNumber;
        private DateTime startWorkingDate;
        private int mileageAfterCare;
        private int mileageCounter;
        private int kmAfterRefueling;
        private DateTime lastCareDate;

        /// <summary>
        /// Building bus from license nuber and date
        /// </summary>
        /// <param name="licenseNumber">The license number of the bus</param>
        /// <param name="startWorkingDate">The date of the bus first ride</param>
        public Bus(int licenseNumber, DateTime startWorkingDate)
        {
            this.licenseNumber = licenseNumber;
            this.startWorkingDate = startWorkingDate;
            this.mileageAfterCare = 0;
            this.kmAfterRefueling = 0;
            this.lastCareDate = DateTime.Now;
            Random rand = new Random(DateTime.Now.Millisecond);
            this.mileageCounter = rand.Next(0, 20000);
        }
        
        /// <summary>
        /// The Maximum km that the bus can drive before another care
        /// </summary>
        public static int MaxKmBeforeCare { get => 20000; }
        /// <summary>
        /// the maximum km thaat the bus can drive befor it need to reful
        /// </summary>
        public static int MaxKmAfterRefueling { get => 1200; }
        /// <summary>
        /// Is the bus declered as danger
        /// </summary>
        public bool IsDanger
        {
            get
            {
                DateTime zeroTime = new DateTime(1, 1, 1);

                TimeSpan span = DateTime.Now-lastCareDate; 

                int yearsAfterCare = (zeroTime + span).Year - 1;
                return MileageAfterCare > MaxKmBeforeCare && yearsAfterCare >= 1;
            }
        }
        /// <summary>
        /// The bus license number
        /// </summary>
        public int LicenseNumber { get => licenseNumber; }
        /// <summary>
        /// The bus first riding date
        /// </summary>
        public DateTime StartWorkingDate { get => startWorkingDate; }
        /// <summary>
        /// How mant km the bus drives after the last care
        /// </summary>
        public int MileageAfterCare { get => mileageAfterCare; }
        /// <summary>
        /// Counting all the km that the bus drived ever
        /// </summary>
        public int MileageCounter { get => mileageCounter; }
        /// <summary>
        /// how many km the bus drived after the last refuling
        /// </summary>
        public int KmAfterRefueling { get => kmAfterRefueling; }
        /// <summary>
        /// The date of the lst care of the bus
        /// </summary>
        public DateTime LastCareDate { get => lastCareDate; }
        /// <summary>
        /// refuling the bus
        /// </summary>
        public void Refuel() { kmAfterRefueling = 0; }
        /// <summary>
        /// Taking care of the bus
        /// </summary>
        public void TakeCare()
        {
            mileageAfterCare = 0;
            lastCareDate = DateTime.Now;
        }
        /// <summary>
        /// Check if the bus can drive the given distance
        /// </summary>
        /// <param name="distance">The distance that the bus want to drive</param>
        /// <returns>Can he drive that distance</returns>
        public bool CanDrive(int distance)
        {
            return !this.IsDanger && distance < (MaxKmAfterRefueling - kmAfterRefueling);
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
            kmAfterRefueling += distance;
            mileageCounter += distance;
            mileageAfterCare += distance;
            return true;
        }

    }
}
