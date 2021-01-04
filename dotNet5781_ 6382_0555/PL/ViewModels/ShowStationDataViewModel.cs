using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class ShowStationDataViewModel:Screen
    {
        private MainViewModel _mainViewModel;
        private StationModel _stationToShow;
        public ShowStationDataViewModel(MainViewModel mainViewModel, StationModel stationToShow) {
            _mainViewModel = mainViewModel;
            _stationToShow = stationToShow;
        }

        public StationModel StationToShow {
            get => _stationToShow;
            set {
                _stationToShow = value;
                NotifyOfPropertyChange(() => StationToShow);
            }
        }
    }
}
