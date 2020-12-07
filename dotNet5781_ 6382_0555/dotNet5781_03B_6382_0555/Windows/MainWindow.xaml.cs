using dotNet5781_03B_6382_0555.EventsObjects;
using dotNet5781_03B_6382_0555.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace dotNet5781_03B_6382_0555
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableDictionary<int, Bus> buses = new ObservableDictionary<int, Bus>();
        private Brush buttonDefaultColorBrush;
        public MainWindow()
        {
            InitializeComponent();
            BusesListBox.DataContext = buses;
            foreach (Bus initializedBus in Bus.InitializeBuses())
            {
                buses[initializedBus.LicenseNumber] = initializedBus;
                
            }
        }

        private void AddBusClick(object sender, RoutedEventArgs e)
        {
            AddBus addBus = new AddBus();
            addBus.PressAdd += AddBus_PressedAdd;
            addBus.ShowDialog();

        }
        private void AddBus_PressedAdd(object sender, AddBusEventArgs e)
        {
            if (e.Bus == null) { return; }
            Bus toAdd = new Bus(e.Bus);
            if (buses.ContainsKey(toAdd.LicenseNumber))
            {
                MessageBoxResult result;
                result = MessageBox.Show("Bus is already exists!\nDo you want to replace it?", "Caution", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    buses[toAdd.LicenseNumber] = toAdd;
                }
            }
            else
            {
                buses.Add(toAdd.LicenseNumber, toAdd);
                MessageBox.Show("Bus added successfuly", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            Button busButton = sender as Button;
            SetRefuel(busButton);
        }

        private void SetRefuel(Button busButton)
        {
            KeyValuePair<int, Bus> buttonBusPair = (KeyValuePair<int, Bus>)busButton.DataContext;
            Bus currentBus = buttonBusPair.Value;
            if (currentBus.TimeToReady != null)
            {
                MessageBox.Show("The bus is not available yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RefuelBGWData bgwData = new RefuelBGWData();
            HandleGraphicRefueling(busButton, bgwData);
            HandleLogicRefuling(currentBus, bgwData);
        }

        private void HandleGraphicRefueling(Button busButton, RefuelBGWData bgwData)
        {
            ListBoxItem myListBoxItem = Tools.GetItemInList(busButton, BusesListBox);
            ContentPresenter myContentPresenter = Tools.FindVisualChild<ContentPresenter>(myListBoxItem);
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            ProgressBar busProgressBar = (ProgressBar)myDataTemplate.FindName("ProgressBar", myContentPresenter);
            TextBlock busProgressBarTextBlock = (TextBlock)myDataTemplate.FindName("ProgressText", myContentPresenter);
            busProgressBar.Visibility = Visibility.Visible;
            busProgressBarTextBlock.Visibility = Visibility.Visible;
            if (buttonDefaultColorBrush == null)
            {
                buttonDefaultColorBrush = busButton.Background;
            }

            busButton.Background = Brushes.Red;
            bgwData.ProgressBar = busProgressBar;
            bgwData.ProgressTextBlock = busProgressBarTextBlock;
            bgwData.Button = busButton;
        }

        private void HandleLogicRefuling(Bus currentBus, RefuelBGWData bgwData)
        {
            currentBus.Refuel();
            bgwData.Bus = currentBus;
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunWorkerAsync(argument: bgwData);
            backgroundWorker.ProgressChanged += OnRefueling;
            backgroundWorker.RunWorkerCompleted += FinishRefueling;
            backgroundWorker.DoWork += TrackRefueling;
            backgroundWorker.WorkerReportsProgress = true;
        }

        private void FinishRefueling(object sender, RunWorkerCompletedEventArgs e)
        {
            RefuelBGWData data = (RefuelBGWData)e.Result;
            if (data == null)
            {
                return;
            }

            data.Bus.Status = Status.Ready;
            data.Bus.TimeToReady = null;
            data.Button.Background = buttonDefaultColorBrush;
            data.ProgressBar.Visibility = Visibility.Hidden;
            data.ProgressTextBlock.Visibility = Visibility.Hidden;
        }

        private void OnRefueling(object sender, ProgressChangedEventArgs e)
        {
            RefuelBGWData dataToTrack = (RefuelBGWData)e.UserState;
            int percent = e.ProgressPercentage;
            dataToTrack.ProgressBar.Visibility = Visibility.Visible;
            dataToTrack.ProgressTextBlock.Visibility = Visibility.Visible;
            dataToTrack.ProgressBar.Value = percent;
            if (dataToTrack.RemainingTime != null)
            {
                dataToTrack.ProgressTextBlock.Text = Tools.FormatTimeSpan(Tools.RealFromSimulationTime(dataToTrack.RemainingTime.Value));
            }
        }

        private void TrackRefueling(object sender, DoWorkEventArgs e)
        {
            RefuelBGWData dataToTrack = (RefuelBGWData)e.Argument;
            BackgroundWorker worker = (BackgroundWorker)sender;
            DateTime startTime = DateTime.Now;
            TimeSpan? allTime = dataToTrack.Bus.TimeToReady - startTime;
            while (DateTime.Now < dataToTrack.Bus.TimeToReady && dataToTrack.Bus.TimeToReady != null)
            {
                Thread.Sleep(1000);
                dataToTrack.RemainingTime = dataToTrack.Bus.TimeToReady - DateTime.Now;
                int percent = (int)((dataToTrack.RemainingTime.Value.TotalSeconds / allTime.Value.TotalSeconds) * 100);
                worker.ReportProgress(100 - percent, dataToTrack);
            }

            e.Result = dataToTrack;
        }

        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            Button driveButton = sender as Button;
            DriveOptions driveOptions = new DriveOptions(driveButton);
            driveOptions.PressedEnter += new EventHandler<DoDriveEventArgs>(HandleDriveEvent);
            driveOptions.Show();
        }

        private void HandleDriveEvent(object sender, DoDriveEventArgs e)
        {
            Button busButton = e.ClickedButton;
            Bus busToDrive = ((KeyValuePair<int, Bus>)busButton.DataContext).Value;
            BackgroundWorker driveBGW = new BackgroundWorker();
            if (busToDrive.TimeToReady != null)
            {
                MessageBox.Show("The bus is not available yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DrivingBGWData bgwData = new DrivingBGWData();
            HandleGraphicDriving(busButton, bgwData);
            HandleLogicDriving(busToDrive, bgwData, e.Distance);
        }

        private void HandleGraphicDriving(Button busButton, DrivingBGWData bgwData)
        {
            ListBoxItem myListBoxItem = Tools.GetItemInList(busButton, BusesListBox);
            ContentPresenter myContentPresenter = Tools.FindVisualChild<ContentPresenter>(myListBoxItem);
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            ProgressBar busProgressBar = (ProgressBar)myDataTemplate.FindName("ProgressBar", myContentPresenter);
            TextBlock busProgressBarTextBlock = (TextBlock)myDataTemplate.FindName("ProgressText", myContentPresenter);
            busProgressBar.Visibility = Visibility.Visible;
            busProgressBarTextBlock.Visibility = Visibility.Visible;
            if (buttonDefaultColorBrush == null)
            {
                buttonDefaultColorBrush = busButton.Background;
            }

            busButton.Background = Brushes.Red;
            bgwData.ProgressBar = busProgressBar;
            bgwData.ProgressTextBlock = busProgressBarTextBlock;
            bgwData.Button = busButton;
        }

        private void HandleLogicDriving(Bus busToDrive, DrivingBGWData bgwData, int distanceToDrive)
        {
            bgwData.Distance = distanceToDrive;
            bgwData.Bus = busToDrive;
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunWorkerAsync(argument: bgwData);
            backgroundWorker.ProgressChanged += OnDriving;
            backgroundWorker.RunWorkerCompleted += FinishDriving;
            backgroundWorker.DoWork += TrackDriving;
            backgroundWorker.WorkerReportsProgress = true;
        }

        private void OnDriving(object sender, ProgressChangedEventArgs e)
        {
            DrivingBGWData dataToTrack = (DrivingBGWData)e.UserState;
            int percent = e.ProgressPercentage;
            dataToTrack.ProgressBar.Visibility = Visibility.Visible;
            dataToTrack.ProgressTextBlock.Visibility = Visibility.Visible;
            dataToTrack.ProgressBar.Value = percent;
            if (dataToTrack.RemainingTime != null)
            {
                dataToTrack.ProgressTextBlock.Text = Tools.FormatTimeSpan(Tools.RealFromSimulationTime(dataToTrack.RemainingTime.Value));
            }
        }

        private void TrackDriving(object sender, DoWorkEventArgs e)
        {

            DrivingBGWData dataToTrack = (DrivingBGWData)e.Argument;
            if (!dataToTrack.Bus.CanDrive(dataToTrack.Distance))
            {
                MessageBox.Show("The bus is not ready to drive.\n check if the bus need a care or need to be refueled.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = dataToTrack;
                return;
            }
            dataToTrack.Bus.Drive(dataToTrack.Distance);

            BackgroundWorker worker = (BackgroundWorker)sender;
            DateTime startTime = DateTime.Now;
            TimeSpan? allTime = dataToTrack.Bus.TimeToReady - startTime;
            while (DateTime.Now < dataToTrack.Bus.TimeToReady && dataToTrack.Bus.TimeToReady != null)
            {
                Thread.Sleep(1000);
                dataToTrack.RemainingTime = dataToTrack.Bus.TimeToReady - DateTime.Now;
                int percent = (int)((dataToTrack.RemainingTime.Value.TotalSeconds / allTime.Value.TotalSeconds) * 100);
                worker.ReportProgress(100 - percent, dataToTrack);
            }

            e.Result = dataToTrack;
        }

        private void FinishDriving(object sender, RunWorkerCompletedEventArgs e)
        {
            DrivingBGWData data = (DrivingBGWData)e.Result;
            if (data == null)
            {
                return;
            }

            data.Bus.Status = Status.Ready;
            data.Bus.TimeToReady = null;
            data.Button.Background = buttonDefaultColorBrush;
            data.ProgressBar.Visibility = Visibility.Hidden;
            data.ProgressTextBlock.Visibility = Visibility.Hidden;
        }

        private void OnItemSelected(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem selectedItem = (BusesListBox.ItemContainerGenerator.ContainerFromIndex(BusesListBox.SelectedIndex)) as ListBoxItem;
            BusData busData = new BusData(selectedItem);
            busData.PressRefuel += BusData_OnPressRefuel;
            busData.PressCare += BusData_OnPressCare;
            busData.Show();
        }

        private void BusData_OnPressRefuel(object sender, RefuelEventArgs e)
        {
            ListBoxItem item = Tools.GetItemInList(e.ItemControl, BusesListBox);
            ContentPresenter contentPresenter = Tools.FindVisualChild<ContentPresenter>(item);
            DataTemplate myDataTemplate = contentPresenter.ContentTemplate;
            Button refuelButton = (Button)myDataTemplate.FindName("RefuelButton", contentPresenter);
            SetRefuel(refuelButton);
        }

        private void BusData_OnPressCare(object sender, CareEventArgs e)
        {
            CareBGWData bgwData = new CareBGWData();
            Bus currentBus = e.Bus;
            if (currentBus.TimeToReady != null)
            {
                MessageBox.Show("The bus is not available yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            HandleGraphicCaring(e.ItemControl, bgwData);
            HandleLogicCaring(currentBus, bgwData);
        }

        private void HandleGraphicCaring(Control busControl, CareBGWData bgwData)
        {
            ListBoxItem myListBoxItem = Tools.GetItemInList(busControl, BusesListBox);
            ContentPresenter myContentPresenter = Tools.FindVisualChild<ContentPresenter>(myListBoxItem);
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            ProgressBar busProgressBar = (ProgressBar)myDataTemplate.FindName("ProgressBar", myContentPresenter);
            TextBlock busProgressBarTextBlock = (TextBlock)myDataTemplate.FindName("ProgressText", myContentPresenter);
            busProgressBar.Visibility = Visibility.Visible;
            busProgressBarTextBlock.Visibility = Visibility.Visible;
            bgwData.ProgressBar = busProgressBar;
            bgwData.ProgressTextBlock = busProgressBarTextBlock;
        }

        private void HandleLogicCaring(Bus currentBus, CareBGWData bgwData)
        {
            currentBus.TakeCare();
            bgwData.Bus = currentBus;
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunWorkerAsync(argument: bgwData);
            backgroundWorker.ProgressChanged += OnCaring;
            backgroundWorker.RunWorkerCompleted += FinishCaring;
            backgroundWorker.DoWork += TrackCaring;
            backgroundWorker.WorkerReportsProgress = true;
        }

        private void FinishCaring(object sender, RunWorkerCompletedEventArgs e)
        {
            CareBGWData data = (CareBGWData)e.Result;
            if (data == null)
            {
                return;
            }

            data.Bus.Status = Status.Ready;
            data.Bus.TimeToReady = null;
            data.ProgressBar.Visibility = Visibility.Hidden;
            data.ProgressTextBlock.Visibility = Visibility.Hidden;
        }

        private void OnCaring(object sender, ProgressChangedEventArgs e)
        {
            CareBGWData dataToTrack = (CareBGWData)e.UserState;
            int percent = e.ProgressPercentage;
            dataToTrack.ProgressBar.Visibility = Visibility.Visible;
            dataToTrack.ProgressTextBlock.Visibility = Visibility.Visible;
            dataToTrack.ProgressBar.Value = percent;
            if (dataToTrack.RemainingTime != null)
            {
                dataToTrack.ProgressTextBlock.Text = Tools.FormatTimeSpan(Tools.RealFromSimulationTime(dataToTrack.RemainingTime.Value));
            }
        }

        private void TrackCaring(object sender, DoWorkEventArgs e)
        {
            CareBGWData dataToTrack = (CareBGWData)e.Argument;
            BackgroundWorker worker = (BackgroundWorker)sender;
            DateTime startTime = DateTime.Now;
            TimeSpan? allTime = dataToTrack.Bus.TimeToReady - startTime;
            while (DateTime.Now < dataToTrack.Bus.TimeToReady && dataToTrack.Bus.TimeToReady != null)
            {
                Thread.Sleep(1000);
                dataToTrack.RemainingTime = dataToTrack.Bus.TimeToReady - DateTime.Now;
                int percent = (int)((dataToTrack.RemainingTime.Value.TotalSeconds / allTime.Value.TotalSeconds) * 100);
                worker.ReportProgress(100 - percent, dataToTrack);
            }

            e.Result = dataToTrack;
        }
    }

}

