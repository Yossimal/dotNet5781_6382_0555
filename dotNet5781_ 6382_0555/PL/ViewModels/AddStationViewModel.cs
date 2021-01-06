using BL;
using BL.BO;
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
    class AddStationViewModel : Screen
    {
        private MainViewModel _mainViewModel;
        private AddStationModel _stationToAdd;
        private IBL logic = BLFactory.API;

        public AddStationViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _stationToAdd = new AddStationModel();
        }

        public AddStationModel StationToAdd
        {
            get => _stationToAdd;
            set
            {
                _stationToAdd = value;
                NotifyOfPropertyChange(() => StationToAdd);
                NotifyOfPropertyChange(() => Longitude);
                NotifyOfPropertyChange(() => Latitude);
                NotifyOfPropertyChange(() => Code);
                NotifyOfPropertyChange(() => Name);
            }
        }
        public string Longitude
        {
            get => StationToAdd.Longitude;
            set
            {
                StationToAdd.Longitude = value;
                NotifyOfPropertyChange(() => StationToAdd);
                NotifyOfPropertyChange(() => Longitude);
            }
        }
        public string Latitude
        {
            get => StationToAdd.Latitude;
            set
            {
                StationToAdd.Latitude = value;
                NotifyOfPropertyChange(() => StationToAdd);
                NotifyOfPropertyChange(() => Latitude);
            }
        }
        public string Name
        {
            get => StationToAdd.Name;
            set
            {
                StationToAdd.Name = value;
                NotifyOfPropertyChange(() => StationToAdd);
                NotifyOfPropertyChange(() => Name);
            }
        }
        public string Code
        {
            get => StationToAdd.Code;
            set
            {
                StationToAdd.Code = value;
                NotifyOfPropertyChange(() => StationToAdd);
                NotifyOfPropertyChange(() => Code);
            }
        }
        public void AddStation(string code, string name, string longitude, string latitude)
        {
            BOAddStation stationToAdd = new BOAddStation
            {
                Station = new BOStation
                {
                    Name = this.Name,
                    Code = int.Parse(this.Code),
                },
                Longitude = double.Parse(this.Longitude),
                Latitude = double.Parse(this.Latitude)
            };
            try
            {
                int addCode = logic.AddStation(stationToAdd);
                MessageBox.Show("The Station has been added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainViewModel.LoadPage("ShowStations");
            }
            catch (Exception ex)
            {
                if (ex is ItemAlreadyExistsException)
                {
                    MessageBox.Show("There is already a station with that code.\nTry cahnging the code number.");
                }
                else
                {
                    MessageBox.Show("An unexpected error was occured while trying to add the bus.\nIf that problem shows again contact us.");
                }
            }
        }
        public bool CanAddStation(string code, string name, string longitude, string latitude)
        {
            return !IsNullEmptyOrWhiteSpace(code)
                && !IsNullEmptyOrWhiteSpace(name)
                && !IsNullEmptyOrWhiteSpace(longitude)
                && !IsNullEmptyOrWhiteSpace(latitude);
        }
        public void ClearText(string code, string name, string longitude, string latitude)
        {
            StationToAdd = new AddStationModel();
        }
        public bool CanClearText(string code, string name, string longitude, string latitude)
        {
            return !IsNullEmptyOrWhiteSpace(code)
                || !IsNullEmptyOrWhiteSpace(name)
                || !IsNullEmptyOrWhiteSpace(longitude)
                || !IsNullEmptyOrWhiteSpace(latitude);
        }

        static bool IsNullEmptyOrWhiteSpace(string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
        }

    }
}
