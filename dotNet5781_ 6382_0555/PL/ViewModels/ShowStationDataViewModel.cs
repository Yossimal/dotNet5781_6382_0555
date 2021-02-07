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
        #region private fields
        private MainViewModel _mainViewModel;
        private StationModel _stationToShow;
        private IBL logic = BLFactory.API;
        #endregion

        public ShowStationDataViewModel(MainViewModel mainViewModel, StationModel stationToShow)
        {
            _mainViewModel = mainViewModel;
            _stationToShow = stationToShow;
        }
        #region properties for Caliburn.Micro
        public StationModel StationToShow
        {
            get => _stationToShow;
            set
            {
                _stationToShow = value;
                NotifyOfPropertyChange(() => StationToShow);
            }
        }
        #endregion
        #region events
        public void DeleteStation()
        {
            try
            {
                if (logic.DeleteStation(int.Parse(StationToShow.Code)))
                {
                    MessageBox.Show("The station has been deleted successfuly!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _mainViewModel.LoadPageNoBack("ShowStations");
                }
            }
            catch (Exception ex)
            {
                if (ex is BL.Exceptions.StationInUseException)
                {
                    MessageBox.Show("The station is in use.\nthat station from the paths of the lines that useing it before.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion
    }
}
