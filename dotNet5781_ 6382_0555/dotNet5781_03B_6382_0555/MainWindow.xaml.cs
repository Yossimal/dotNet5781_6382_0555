using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_03B_6382_0555.EventsObjects;

namespace dotNet5781_03B_6382_0555
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, Bus> buses = new Dictionary<int, Bus>();
        public MainWindow()
        {
            InitializeComponent();
            BusesListBox.DataContext = buses;

        }

        private void AddBusClick(object sender, RoutedEventArgs e)
        {
            AddBus addBus = new AddBus();
            addBus.PressAdd += new EventHandler<AddBusEventArgs>(AddBus_PressedAdd);
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
                    RefreshList();
                }
            }
            else
            {
                buses.Add(toAdd.LicenseNumber, toAdd);
                MessageBox.Show("Bus added successfuly", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshList();
            }

        }

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            Button busButton = sender as Button;
            KeyValuePair<int, Bus> buttonBusPair = (KeyValuePair<int, Bus>)busButton.DataContext;
            Bus currentBus = buttonBusPair.Value;
            ListBoxItem myListBoxItem = GetItemInList(busButton, BusesListBox);
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            ProgressBar busProgressBar = (ProgressBar)myDataTemplate.FindName("ProgressBar", myContentPresenter);
            busProgressBar.Visibility = Visibility.Visible;
            currentBus.Refuel();
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            RefuelBGWData bgwData = new RefuelBGWData(currentBus, busButton,busProgressBar);
            backgroundWorker.RunWorkerAsync(argument: bgwData);
            backgroundWorker.ProgressChanged += OnRefuling;
            backgroundWorker.RunWorkerCompleted += FinishRefuling;
            backgroundWorker.DoWork += TrackRefuling;
            backgroundWorker.WorkerReportsProgress = true;
            busButton.Background = Brushes.Red;
        }

        private void FinishRefuling(object sender, RunWorkerCompletedEventArgs e)
        {
            RefuelBGWData data = (RefuelBGWData)e.Result;
            data.Button.Background = Brushes.DarkGray;
        }

        private void OnRefuling(object sender, ProgressChangedEventArgs e)
        {
            RefuelBGWData dataToTrack=(RefuelBGWData)e.UserState;
            int percent = e.ProgressPercentage;
            dataToTrack.ProgressBar.Value = percent;
        }

        private void TrackRefuling(object sender, DoWorkEventArgs e)
        {
            RefuelBGWData dataToTrack = (RefuelBGWData)e.Argument;
            BackgroundWorker worker = (BackgroundWorker) sender;
            DateTime startTime=DateTime.Now;
            TimeSpan? allTime = dataToTrack.Bus.TimeToReady - startTime;
            while (DateTime.Now < dataToTrack.Bus.TimeToReady)
            {
                Thread.Sleep(1000);
                TimeSpan? timeRemain = dataToTrack.Bus.TimeToReady - DateTime.Now;
                int percent = (timeRemain.Value.Seconds / allTime.Value.Seconds)*100;
                worker.ReportProgress(100-percent,dataToTrack);
            }

            e.Result = dataToTrack;
        }

        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            Button temp = sender as Button;
            KeyValuePair<int, Bus> buttonBusPair = (KeyValuePair<int, Bus>)temp.DataContext;
            MessageBox.Show($"Driving: {buttonBusPair.Value.LicenseNumber}");
        }
        private void RefreshList()
        {
            BusesListBox.DataContext = null;
            BusesListBox.DataContext = buses;
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        public ListBoxItem GetItemInList(Control control, ListBox list)
        {
            object context = control.DataContext;
            for(int i=0;i<list.Items.Count;i++)
            {
                ListBoxItem item = (ListBoxItem)(list.ItemContainerGenerator.ContainerFromIndex(i));
                if (context == item.DataContext)
                {
                    return item;
                }
            }

            return null;
        }
       

    }

}

