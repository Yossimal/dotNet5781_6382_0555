using dotNet5781_03B_6382_0555.EventsObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace dotNet5781_03B_6382_0555.Windows
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        /// <summary>
        /// the bus that we need to present
        /// </summary>
        private Bus busToPresent;
        /// <summary>
        /// A control that holds that bus in the main-window
        /// </summary>
        private Control itemControl;
        /// <summary>
        /// Occur when the user pressing the refuel button
        /// </summary>
        public event EventHandler<RefuelEventArgs> PressRefuel;
        /// <summary>
        /// Occur when the user pressing the care button
        /// </summary>
        public event EventHandler<CareEventArgs> PressCare;

        public BusData(Control selectedItem)
        {
            InitializeComponent();
            this.busToPresent = ((KeyValuePair<int, VisualBus>)selectedItem.DataContext).Value;
            this.itemControl = selectedItem;
            InitializeBusData();
        }
        /// <summary>
        /// Set all the data textboxes
        /// </summary>
        private void InitializeBusData()
        {
            AvailabilityStateTextBlock.Text = busToPresent.Status.ToString();
            int leftFuel = Bus.MaxKmAfterRefueling - busToPresent.KmAfterRefueling;
            if (leftFuel <= 0)
            {
                FuelStatusTextBlock.Text = "No fuel";
            }

            FuelStatusTextBlock.Text = leftFuel + "KM left";
            LastCareDateTextBlock.Text = busToPresent.LastCareDate.ToShortDateString();
            LicenseNumberTextBlock.Text = Bus.FormatLicense(busToPresent.LicenseNumber);
            NextCareDateTextBlock.Text = busToPresent.LastCareDate.AddYears(1).ToShortDateString();
            StartWorkingDateTextBlock.Text = busToPresent.StartWorkingDate.ToShortDateString();
            if (busToPresent.TimeToReady != null)
            {
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.RunWorkerAsync(argument: busToPresent);
                backgroundWorker.DoWork += RunningTimer;
                backgroundWorker.ProgressChanged += TimerTick;
                backgroundWorker.RunWorkerCompleted += TimerFinished;
                backgroundWorker.WorkerReportsProgress = true;
            }
            else
            {
                this.TimeToReadyTextBlock.Text = "The bus is ready to drive";
            }

            this.KMToCareTextBlock.Text = (Bus.MaxKmBeforeCare - busToPresent.MileageAfterCare).ToString();
            this.DrivingTimeTodayTextBlock.Text = Tools.FormatTimeSpan(busToPresent.DrivingTimeToday);

        }
        /// <summary>
        /// Set the time-left timer BGW (count the time and call ReportProcess
        /// </summary>
        private void RunningTimer(object sender, DoWorkEventArgs e)
        {
            Bus bus = (Bus)e.Argument;
            BackgroundWorker worker = (BackgroundWorker)sender;
            DateTime startTime = DateTime.Now;

            TimeSpan? allTime = bus.TimeToReady - startTime;
            TimeSpan? remainingTime = allTime;
            while (DateTime.Now < bus.TimeToReady && bus.TimeToReady != null)
            {
                Thread.Sleep(1000);
                remainingTime = bus.TimeToReady - DateTime.Now;
                if (remainingTime.HasValue)
                {
                    int percent = (int) ((remainingTime.Value.TotalSeconds / allTime.Value.TotalSeconds) * 100);
                    worker.ReportProgress(100 - percent, remainingTime);
                }
            }

            e.Result = bus;
        }
        /// <summary>
        /// Set the timeToReady textbox 
        /// </summary>
        private void TimerTick(object sender, ProgressChangedEventArgs e)
        {
            this.TimeToReadyTextBlock.Text = Tools.FormatTimeSpan(Tools.RealFromSimulationTime((TimeSpan)e.UserState));
        }
        /// <summary>
        /// Occur when the timeToReady timer has finished
        /// </summary>
        private void TimerFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            this.TimeToReadyTextBlock.Text = "The bus is ready to drive";
        }
        /// <summary>
        /// Send the bus to refuel (launch event)
        /// </summary>
        private void SendToRefuel(object sender, RoutedEventArgs e)
        {
            PressRefuel(this, new RefuelEventArgs(busToPresent, itemControl));
        }
        /// <summary>
        /// Send the bus to a care (launch event)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToCare(object sender, RoutedEventArgs e)
        {
            PressCare(this, new CareEventArgs(busToPresent, itemControl));
        }
    }
}
