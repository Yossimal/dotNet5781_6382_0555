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
using System.Windows.Shapes;

namespace dotNet5781_03B_6382_0555
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : Window
    {
        public event EventHandler<AddBusEventArgs> PressAdd;
        public AddBus()
        {
            InitializeComponent();
            DisableInputs();
        }

        private void IsUsed_Check(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check.IsChecked == true)
            {
                StartWorkingDayInput.IsEnabled = true;
                MilageTextBox.IsEnabled = true;
            }
            else
            {
                DisableInputs();
            }
        }
        private void DisableInputs()
        {
            StartWorkingDayInput.IsEnabled = false;
            MilageTextBox.IsEnabled = false;
            StartWorkingDayInput.SelectedDate = DateTime.Now;
            MilageTextBox.Text = "0";
        }

        private void SaveAdd_Click(object sender, RoutedEventArgs e)
        {
            Bus userBus;
            if (!GenerateBus(out userBus)) { return; }
            PressAdd(this, new AddBusEventArgs(userBus));
        }

        private void SaveExit_Click(object sender, RoutedEventArgs e)
        {
            Bus userBus;
            if (!GenerateBus(out userBus)) { return; }
            PressAdd(this, new AddBusEventArgs(userBus));
            Close();
        }
        private bool GenerateBus(out Bus bus)
        {
            string licenseNumberStr = LicenseNumberTextBox.Text.Replace("-", "");
            int licenseNumber;
            if (!int.TryParse(licenseNumberStr, out licenseNumber))
            {
                MessageBox.Show("The license number can include numbers and - only", "Invalid license number", MessageBoxButton.OK, MessageBoxImage.Error);
                bus = null;
                return false;

            }
            DateTime dt = StartWorkingDayInput.SelectedDate.GetValueOrDefault(DateTime.Now);
            if (dt > DateTime.Now) {
                MessageBox.Show("You can't choose a date from the future", "Invalid date", MessageBoxButton.OK, MessageBoxImage.Error);
                bus = null;
                return false;

            }
            if (dt.Year < 2018 && (licenseNumber >= Math.Pow(10, 7) || licenseNumber < Math.Pow(10, 6)) ||
                dt.Year >= 2018 && (licenseNumber >= Math.Pow(10, 8) || licenseNumber < Math.Pow(10, 7)))
            {
                MessageBox.Show("The license number must include 7 digits for bus that started working before 2018 and 8 digits for bus that started working from 2018", "Invalid license number", MessageBoxButton.OK, MessageBoxImage.Error);
                bus = null;
                return false;
            }
            int mileage;
            if (!int.TryParse(MilageTextBox.Text, out mileage))
            {
                MessageBox.Show("The milage must be a number", "Invalid milage", MessageBoxButton.OK, MessageBoxImage.Error);
                bus = null;
                return false;
            }
            if (mileage < 0)
            {
                MessageBox.Show("The milage must be positive number", "Invalid milage", MessageBoxButton.OK, MessageBoxImage.Error);
                bus = null;
                return false;
            }
            bus = new Bus(licenseNumber, dt, mileage);
            return true;
        }

        private void DiscardAndExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
