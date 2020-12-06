using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Button temp = sender as Button;
            KeyValuePair<int, Bus> buttonBusPair = (KeyValuePair<int, Bus>)temp.DataContext;
            MessageBox.Show($"Refueling: {buttonBusPair.Value.LicenseNumber}");
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
    }
}
