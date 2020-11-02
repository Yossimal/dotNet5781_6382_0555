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

        public static int MaxKmBeforeCare { get => 20000; }
        public static int MaxKmAfterRefueling { get => 1200; }
        public bool IsDanger
        {
            get
            {
                DateTime zeroTime = new DateTime(1, 1, 1);

                TimeSpan span = lastCareDate - DateTime.Now;

                int yearsAfterCare = (zeroTime + span).Year - 1;
                return MileageAfterCare > MaxKmBeforeCare && yearsAfterCare >= 1;
            }
        }

        public int LicenseNumber { get => licenseNumber; }
        public DateTime StartWorkingDate { get => startWorkingDate; }
        public int MileageAfterCare { get => mileageAfterCare; }
        public int MileageCounter { get => mileageCounter; }
        public int KmAfterRefueling { get => kmAfterRefueling; }
        public DateTime LastCareDate { get => lastCareDate; }
        public void Refuel() { kmAfterRefueling = 0; }
        public void TakeCare()
        {
            mileageAfterCare = 0;
            lastCareDate = DateTime.Now;
        }
        public bool CanDrive(int distance)
        {
            return !this.IsDanger && distance > (MaxKmAfterRefueling - kmAfterRefueling);
        }
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
