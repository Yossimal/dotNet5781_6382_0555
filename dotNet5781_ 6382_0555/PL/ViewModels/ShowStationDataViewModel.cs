using BL;
using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    class ShowStationDataViewModel : Screen
    {
        private MainViewModel _mainViewModel;
        private StationModel _stationToShow;
        private IBL logic = BLFactory.API;
        public ShowStationDataViewModel(MainViewModel mainViewModel, StationModel stationToShow)
        {
            _mainViewModel = mainViewModel;
            _stationToShow = stationToShow;
        }

        public StationModel StationToShow
        {
            get => _stationToShow;
            set
            {
                _stationToShow = value;
                NotifyOfPropertyChange(() => StationToShow);
            }
        }

        public void DeleteStation()
        {
            if (logic.DeleteStation(int.Parse(StationToShow.Code)))
            {
                MessageBox.Show("The station has been deleted successfuly!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainViewModel.LoadPageNoBack("ShowStations");
            }
        }
    }
}
