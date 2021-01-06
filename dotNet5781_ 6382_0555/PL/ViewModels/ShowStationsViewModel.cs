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
        MainViewModel _mainViewModel;
        StationModel _selectedStation;
        BindableCollection<StationModel> _stations;
        IBL logic = BLFactory.API;
        public ShowStationsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Stations = new BindableCollection<StationModel>(logic.AllStations()
                                                                 .Select(station=>new StationModel(station)));

        }
        public StationModel SelectedStation {
            get => _selectedStation;
            set {
                _selectedStation = value;
                NotifyOfPropertyChange(() => SelectedStation);
            }
        }
        public BindableCollection<StationModel> Stations {
            get => _stations;
            set {
                _stations = value;
                NotifyOfPropertyChange(() => Stations);
            }
        }
        public void ShowStationData() {
            if(SelectedStation != null){
                _mainViewModel.LoadPage("ShowStationData", SelectedStation);
            }
        }
        public void AddStation() {
            _mainViewModel.LoadPage("AddStation");
        }
    }
}
