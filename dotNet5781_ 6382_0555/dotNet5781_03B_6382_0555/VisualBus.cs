using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dotNet5781_03B_6382_0555
{
    class VisualBus : Bus
    {
        public Visibility NoFuelIconVisibility { get; set; }
        public Visibility NeedRepairIconVisibility { get; set; }
        public Visibility NeedToSleepVisibility { get; set; }

        public VisualBus(int licenseNumber, DateTime startWorkingDate, int mileage) : base(licenseNumber, startWorkingDate, mileage)
        {
        }

        public VisualBus(Bus toCopy) : base(toCopy)
        {
            this.RefreshVisualization();
        }

        public override void Refuel()
        {
            this.NoFuelIconVisibility = Visibility.Hidden;
            base.Refuel();
        }

        public override void TakeCare()
        {
            this.NeedRepairIconVisibility = Visibility.Hidden;
            base.TakeCare();
        }

        public override bool Drive(int distance)
        {
            bool ret = base.Drive(distance);
            this.RefreshVisualization();
            this.NeedToSleepVisibility =TimeSpan.FromHours(this.DrivingTimeToday.TotalHours+1)<Bus.MaxDriveTimeInDay ? Visibility.Hidden : Visibility.Visible;
            return ret;
        }

        public void RefreshVisualization()
        {
            this.NoFuelIconVisibility = (Bus.MaxKmAfterRefueling - this.KmAfterRefueling < 10)
                ? Visibility.Visible
                : Visibility.Hidden;
            this.NeedRepairIconVisibility = this.IsDanger ? Visibility.Visible : Visibility.Hidden;
            this.NeedToSleepVisibility =CanDriveToday ? Visibility.Hidden : Visibility.Visible;

        }
        
    }
}
