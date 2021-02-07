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
    class AddBusViewModel : Screen
    {
        /// <summary>
        /// the bus data
        /// </summary>
        private BusModel _busToAdd;
        /// <summary>
        /// instance of the main view model
        /// </summary>
        private MainViewModel _mainViewModel;
        /// <summary>
        /// instance of IBL
        /// </summary>
        private IBL logic = BLFactory.API;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mainViewModel">the program main vew model</param>
        public AddBusViewModel(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            this.BusToAdd = new BusModel
            {
                LicenseDate = DateTime.Today
            };
        }
        #region properties for the Caliburn.Micro Events
        public DateTime LastCareDate
        {
            get => _busToAdd.LastCareDate;
            set
            {
                _busToAdd.LastCareDate = value;
                NotifyOfPropertyChange(() => LastCareDate);
                NotifyOfPropertyChange(() => BusToAdd);

            }
        }
        public BusModel BusToAdd
        {
            get => _busToAdd;
            set
            {
                _busToAdd = value;
                NotifyOfPropertyChange(() => BusToAdd);
            }
        }
        public string LicenseNumber
        {
            get => _busToAdd.LicenseNumber;
            set
            {
                int temp;
                //Condition to ensure that only digits will be written
                if (int.TryParse(value, out temp) || String.IsNullOrEmpty(value))
                {
                    value = temp.ToString();
                    _busToAdd.LicenseNumber = value;
                }
                NotifyOfPropertyChange(() => LicenseNumber);
                NotifyOfPropertyChange(() => BusToAdd);
            }
        }
        public DateTime LicenseDate
        {
            get => _busToAdd.LicenseDate;
            set
            {
                _busToAdd.LicenseDate = value;
                NotifyOfPropertyChange(() => LicenseDate);
                NotifyOfPropertyChange(() => BusToAdd);
                NotifyOfPropertyChange(() => LastCareDateVisibility);
            }
        }
        public string MilageCounter
        {
            get => _busToAdd.MilageCounter.ToString();
            set
            {
                double temp;
                if (double.TryParse(value, out temp) || String.IsNullOrEmpty(value))
                {
                    _busToAdd.MilageCounter = temp;
                }
                NotifyOfPropertyChange(() => MilageCounter);
                NotifyOfPropertyChange(() => BusToAdd);
            }
        }
        public Visibility LastCareDateVisibility
        {
            get
            {
                return LicenseDate == DateTime.Today ?
                    Visibility.Hidden :
                    Visibility.Visible;
            }
        }
        #endregion
        #region Events for Calimburn.Micro
        public void ClearText(string licenseNumber, string milageCounter)
        {
            LicenseNumber = "";
            MilageCounter = "";
        }
        public bool CanClearText(string licenseNumber, string milageCounter)
        {
            return !String.IsNullOrEmpty(milageCounter)
                || !String.IsNullOrWhiteSpace(milageCounter)
                || !String.IsNullOrEmpty(licenseNumber)
                || !String.IsNullOrWhiteSpace(licenseNumber);
        }
        public void AddBus(string licenseNumber, string milageCounter)
        {
            try
            {
                logic.AddBus(BusToAdd.ToBO());
                _mainViewModel.LoadPage("ShowBuses");
            }
            catch (Exception ex)
            {
                if (ex is ItemAlreadyExistsException)
                {
                    MessageBox.Show("There is already a bus with that license number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("An unexpected error was occured while trying to add the bus.\nIf that problem shows again contact us.");
                }
            }
        }
        public bool CanAddBus(string licenseNumber, string milageCounter)
        {
            return !String.IsNullOrEmpty(milageCounter)
               && !String.IsNullOrWhiteSpace(milageCounter)
               && !String.IsNullOrEmpty(licenseNumber)
               && !String.IsNullOrWhiteSpace(licenseNumber);
        }
        #endregion
    }
}
