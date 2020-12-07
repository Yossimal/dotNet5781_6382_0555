using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using dotNet5781_03B_6382_0555.EventsObjects;

namespace dotNet5781_03B_6382_0555.Windows
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        private Bus busToPresent;
        private Control itemControl;
        public event EventHandler<RefuelEventArgs> PressRefuel;
        public event EventHandler<CareEventArgs> PressCare;

        public BusData(Control selectedItem)
        {
            InitializeComponent();
            this.busToPresent = ((KeyValuePair<int, Bus>)selectedItem.DataContext).Value;
            this.itemControl = selectedItem;
            InitializeBusData();
        }

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


        }

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

        private void TimerTick(object sender, ProgressChangedEventArgs e)
        {
            this.TimeToReadyTextBlock.Text = Tools.FormatTimeSpan(Tools.RealFromSimulationTime((TimeSpan)e.UserState));
        }

        private void TimerFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            this.TimeToReadyTextBlock.Text = "The bus is ready to drive";
        }

        private void SendToRefuel(object sender, RoutedEventArgs e)
        {
            PressRefuel(this, new RefuelEventArgs(busToPresent, itemControl));
        }

        private void SendToCare(object sender, RoutedEventArgs e)
        {
            PressCare(this, new CareEventArgs(busToPresent, itemControl));
        }
    }
}
