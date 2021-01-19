﻿using BL;
using Caliburn.Micro;
using PL.Models;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace PL.ViewModels
{
    class ShowBusDataViewModel : Screen
    {
        private MainViewModel _mainViewModel;
        private BusModel _busToShow;
        private IBL logic = BLFactory.API;
        private int LicenseNumber => BusModel.ReFormatLicense(BusToShow.LicenseNumber);
        public ShowBusDataViewModel(MainViewModel mainViewModel, BusModel busToShow)
        {
            _mainViewModel = mainViewModel;
            _busToShow = busToShow;
        }
        public BusModel BusToShow
        {
            get => _busToShow;
            set
            {
                _busToShow = value;
                NotifyOfPropertyChange(() => BusToShow);
            }
        }
        public void DeleteBus()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to remove that bus?\n You can't undo this proccess.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (logic.DeleteBus(LicenseNumber))
                {
                    MessageBox.Show("The bus has been removed successfuly", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _mainViewModel.LoadPageNoBack("ShowBuses");
                }
                else
                {
                    MessageBox.Show("Something went wrong with the proccess.\n If that problem comes more times contact us", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _mainViewModel.LoadPage("ShowBuses");
                }
            }
        }
        public void Refuel()
        {
            try
            {
                logic.RefuelBus(LicenseNumber);
            }
            catch (Exception ex)
            {
                if (ex is BusNotAvailableException)
                {
                    MessageBox.Show("The bus is not ready yet.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    throw ex;
                }
            }
            RefreshData();
        }
        public void Care()
        {
            try
            {
                logic.CareBus(LicenseNumber);
            }
            catch (Exception ex)
            {
                if (ex is BusNotAvailableException)
                {
                    MessageBox.Show("The bus is not ready yet.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    throw ex;
                }
            }
            RefreshData();
        }
        public void RefreshData()
        {
            BusToShow = new BusModel(logic.GetBus(LicenseNumber));
        }
        public void UpdateDataTrack(object sender, DoWorkEventArgs args)
        {
            Thread.Sleep(10000);
        }
        public void TrackRefresh(object sender, ProgressChangedEventArgs args)
        {
            RefreshData();
        }
    }
}
