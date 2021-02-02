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
        private BusModel _busToAdd;
        private MainViewModel _mainViewModel;
        private IBL logic = BLFactory.API;

        public AddBusViewModel(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            this.BusToAdd = new BusModel
            {
                LicenseDate = DateTime.Today
            };
        }
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
    }
}
