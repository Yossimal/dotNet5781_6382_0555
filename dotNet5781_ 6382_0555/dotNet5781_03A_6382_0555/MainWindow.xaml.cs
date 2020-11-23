/* Yosef Malka 208090555
 * Aaron Kremer 034706382 */

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
using dotNet5781_02__6382_0555;

namespace dotNet5781_03A_6382_0555
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusLineCollection lines = new BusLineCollection();
        private BusLine currentDisplayBusLine;
        private List<BusStation> stations;

        Random rand = new Random(DateTime.Now.Millisecond);
        public MainWindow()
        {
            InitializeComponent();
            FirstInitialization();
        }
        public void FirstInitialization()
        {
            byte[] stationsDataBytes = ProgramResources.Stations;
            string[] stationData = System.Text.Encoding.UTF8.GetString(stationsDataBytes).Split('\n');
            stations = Program.ReadData(stationData);
            for (int i = 0; i < 10; i++)
            {
                lines.Add(GenerateBusLine());
            }
            cbBusLines.ItemsSource = lines;
            cbBusLines.DisplayMemberPath = "LineNum";
            cbBusLines.SelectedIndex = 0;
        }
        /// <summary>
        /// The function generates a bus line
        /// </summary>
        /// <param name="stations"> stations of line </param> 
        /// <returns> return random bus line </returns>
        private BusLine GenerateBusLine()
        {
            int temp = rand.Next(1, 1000);
            while (lines.IsExists(temp))
            {
                temp = rand.Next(1, 1000);
            }
            BusLine line = new BusLine(temp, Program.GetRandomArea());
            for (int i = 0; i < 8; i++)
            {
                temp = rand.Next(0, stations.Count);
                if (line.IsExists(stations[temp].Code))
                {
                    i--;
                    continue;
                }
                LineBusStation station = new LineBusStation(stations[temp].Code,
                            stations[temp].Location, stations[temp].Address, rand.Next(1, 51),
                            new TimeSpan(rand.Next(0, 2), rand.Next(0, 60), rand.Next(1, 60)));
                line.Add(station);
            }
            return line;
        }
        /// <summary>
        /// Upload Area to text box and setting list box with line params
        /// </summary>
        /// <param name="index"> index of a line in lines </param>
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = lines[index, 0];
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine;
            AreaTextBox.Text = currentDisplayBusLine.Area.ToString();
        }
        /// <summary>
        /// choose bus line number from combo box
        /// </summary>
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).LineNum);
        }
        /// <summary>
        /// Remove station from bus line, and refreshing list box
        /// </summary>
        private void RemoveStation_Click(object sender, RoutedEventArgs e)
        {
            if (lbBusLineStations.SelectedItem == null)
            {
                MessageBox.Show("You need to choose item from the list to remove","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            MessageBoxResult results = MessageBox.Show("Are you sure that you want to remove that station?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (results != MessageBoxResult.Yes) {
                return;
            }
            currentDisplayBusLine.Remove((lbBusLineStations.SelectedItem as LineBusStation).Code);
            lbBusLineStations.DataContext = null;
            lbBusLineStations.DataContext = currentDisplayBusLine;
            MessageBox.Show("The station has successfully removed", "Success",MessageBoxButton.OK,MessageBoxImage.Information);
        }
        /// <summary>
        /// Add station to bus line, and refreshing list box
        /// </summary>    
        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            int rand = this.rand.Next(0, stations.Count);
            while (currentDisplayBusLine.IsExists(stations[rand].Code))
            {
                rand = this.rand.Next(0, stations.Count);
            }
            LineBusStation station = new LineBusStation(stations[rand].Code,
                        stations[rand].Location, stations[rand].Address, this.rand.Next(1, 51),
                        new TimeSpan(this.rand.Next(0, 2), this.rand.Next(0, 60), this.rand.Next(1, 60)));
            currentDisplayBusLine.Add(station);
            MessageBox.Show("The station has been added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            lbBusLineStations.DataContext = null;
            lbBusLineStations.DataContext = currentDisplayBusLine;
        }
    }
}


