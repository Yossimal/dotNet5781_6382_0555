using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Maps.MapControl.WPF;

namespace dotNet5781_03B_6382_0555
{
    /// <summary>
    /// Interaction logic for DriveOptions.xaml
    /// </summary>
    public partial class DriveOptions : Window
    {
        public event EventHandler<DoDriveEventArgs> PressedEnter;
        private Button senderButton;
        public DriveOptions(Button sender)
        {
            InitializeComponent();
            this.KeyDown += CallPressedEnter;
            this.senderButton = sender;


        }

        private void CallPressedEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int distance;
                if (int.TryParse(DistanceTextBox.Text.Replace(" ",""), out distance))
                {
                    PressedEnter(this, new DoDriveEventArgs(distance,senderButton));
                    Close();
                }
                else
                {
                    MessageBox.Show("The distance must be a number", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            
        }
    }
}
