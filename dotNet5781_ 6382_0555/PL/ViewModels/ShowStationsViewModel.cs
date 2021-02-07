using BL;
using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class ShowStationsViewModel : Screen
    {
        #region private fields
        MainViewModel _mainViewModel;
        StationModel _selectedStation;
        BindableCollection<StationModel> _stations;
        IBL logic = BLFactory.API;
        #endregion

        public ShowStationsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Stations = new BindableCollection<StationModel>(logic.AllStations()
                                                                 .Select(station => new StationModel(station)));

        }
        #region properties for Caliburn.Micro
        public StationModel SelectedStation
        {
            get => _selectedStation;
            set
            {
                _selectedStation = value;
                NotifyOfPropertyChange(() => SelectedStation);
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
        #endregion
        #region events
        public void ShowStationData()
        {
            if (SelectedStation != null)
            {
                _mainViewModel.LoadPage("ShowStationData", SelectedStation);
            }
        }
        public void AddStation()
        {
            _mainViewModel.LoadPage("AddStation");
        }
        #endregion
    }
}
