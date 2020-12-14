using dotNet5781_03B_6382_0555.EventsObjects;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dotNet5781_03B_6382_0555
{
    /// <summary>
    /// Interaction logic for DriveOptions.xaml
    /// </summary>
    public partial class DriveOptions : Window
    {
        /// <summary>
        /// Occur when the user presses the Enter key
        /// </summary>
        public event EventHandler<DoDriveEventArgs> PressedEnter;
        /// <summary>
        /// The button that open that window (the drive button)
        /// </summary>
        private Button senderButton;
        public DriveOptions(Button sender)
        {
            InitializeComponent();
            this.KeyDown += CallPressedEnter;
            this.senderButton = sender;


        }
        /// <summary>
        /// When the user presses Enter
        /// </summary>
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
        /// <summary>
        /// Prevent the user to insert an non-digits characters
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            
        }
    }
}
