using BL;
using BL.BO;
using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    class AddLineViewModel : Screen
    {
        #region private fuilds
        private MainViewModel _mainViewModel;
        private LineModel _lineToAdd;
        private BindableCollection<string> _areas;
        private IBL logic = BLFactory.API;
        private BindableCollection<StationModel> _path;
        private StationModel _selectedStation;
        private BindableCollection<StationModel> _stations;
        #endregion
        #region constructors
        public AddLineViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            Areas = new BindableCollection<string>(logic.GetAllAreas());
            _lineToAdd = new LineModel
            {
                Area = Areas[0],
                Code = 0,
                Stations = new BindableCollection<StationModel>()
            };
            Path = new BindableCollection<StationModel>();
            IEnumerable<StationModel> allStations = logic.AllStations().Select(station => new StationModel { Code = station.Code.ToString(), Name = station.Name });
            Stations = new BindableCollection<StationModel>(allStations);
            if (Stations.Count > 0)
            {
                SelectedStation = Stations[0];
            }
        }
        #endregion
        #region properties
        #region data properties
        public bool CanAddLine
        {
            get
            {
                return int.Parse(LineNumber) > 0 && Path.Count > 1;
            }
        }
        public LineModel LineToAdd
        {
            get => _lineToAdd;
            set
            {
                _lineToAdd = value;
                NotifyOfPropertyChange(() => LineToAdd);
                NotifyOfPropertyChange(() => LineNumber);
            }
        }
        public string LineNumber
        {
            get => _lineToAdd.Code.ToString();
            set
            {
                if (int.TryParse(value, out _))
                {
                    _lineToAdd.Code = int.Parse(value);
                    NotifyOfPropertyChange(() => LineNumber);
                    NotifyOfPropertyChange(() => LineToAdd);
                    NotifyOfPropertyChange(() => SelectedArea);
                    NotifyOfPropertyChange(() => CanAddLine);
                    NotifyOfPropertyChange(() => CanClearText);
                }
            }
        }
        public string SelectedArea
        {
            get => _lineToAdd.Area;
            set
            {
                if (Areas.Contains(value))
                {
                    _lineToAdd.Area = value;
                    NotifyOfPropertyChange(() => SelectedArea);
                    NotifyOfPropertyChange(() => LineToAdd);
                    NotifyOfPropertyChange(() => CanAddLine);
                    NotifyOfPropertyChange(() => CanClearText);
                }
            }
        }
        public BindableCollection<StationModel> Stations
        {
            get => _stations;
            set
            {
                _stations = value;
                NotifyOfPropertyChange(() => Stations);
            }
        }
        public BindableCollection<string> Areas
        {
            get => _areas;
            set
            {
                _areas = value;
                NotifyOfPropertyChange(() => Areas);
            }
        }
        public BindableCollection<StationModel> Path
        {
            get => _path;
            set
            {
                _path = value;
                NotifyOfPropertyChange(() => Path);
                NotifyOfPropertyChange(() => CanAddLine);
            }
        }
        public StationModel SelectedStation
        {
            get => _selectedStation;
            set
            {
                _selectedStation = value;
                NotifyOfPropertyChange(() => SelectedStation);
                NotifyOfPropertyChange(() => CanAddStation);
            }
        }
        #endregion
        #region events properties
        public bool CanAddStation
        {
            get => !Path.Contains(SelectedStation);
        }
        public bool CanClearText
        {
            get
            {
                return int.Parse(LineNumber) > 0
                    || Path.Count > 0
                    || SelectedArea != Areas[0];
            }
        }

        public void ClearText(string lineNumber)
        {
            SelectedArea = Areas[0];
            lineNumber = "0";
            Path = new BindableCollection<StationModel>();
            SelectedStation = Stations[0];
        }
        #endregion
        #endregion
        #region events
        public void AddStation()
        {
            Path.Add(SelectedStation);
            NotifyOfPropertyChange(() => Path);
            NotifyOfPropertyChange(() => CanAddStation);
            NotifyOfPropertyChange(() => CanAddLine);
            NotifyOfPropertyChange(() => CanClearText);
        }
        public void RemoveStation(string stationToRemoveCode)
        {
            StationModel toRemove = Path.Where(station => station.Code == stationToRemoveCode).First();
            Path.Remove(toRemove);
        }
        public async void AddLine()
        {
            if (!logic.IsInternetAvailable())
            {
                MessageBox.Show("You mus have an internet connection for that action.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BOLine lineToAdd = new BOLine
            {
                Area = SelectedArea,
                LineNumber = int.Parse(LineNumber),
                Path = Path.Select(station => new BOStation { Code = int.Parse(station.Code), Name = station.Name })
            };
            try
            {
                int lineId = await logic.AddLine(lineToAdd);
                MessageBox.Show("The line has been added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainViewModel.LoadPage("ShowLines");
            }
            catch
            {
                MessageBox.Show("Unexpected error was occured while trying to add the line.\n Try to check your internet connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        #region private methods
        [Obsolete]
        static bool IsNullEmptyOrWhiteSpace(string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
        }
        #endregion
    }
}
