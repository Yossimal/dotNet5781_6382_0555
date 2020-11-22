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
            List<BusStation> stations = Program.ReadData(stationData);
            BusLineCollection lines = new BusLineCollection();

            for (int i = 0; i < 10; i++)
            {
                lines.Add(GenerateBusLine(lines, stations));
            }
            cbBusLines.ItemsSource = lines;
            cbBusLines.DisplayMemberPath = "LineNum";
            cbBusLines.SelectedIndex = 0;
        }
        private BusLine GenerateBusLine(BusLineCollection lines, List<BusStation> stations)
        {
            int temp = rand.Next(0, 1000);
            while (lines.IsExists(temp))
            {
                temp = rand.Next(0, 1000);
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
    }
}


