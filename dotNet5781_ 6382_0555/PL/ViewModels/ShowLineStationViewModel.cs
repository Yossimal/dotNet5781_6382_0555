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
    class ShowLineStationViewModel : Screen
    {
        #region private fields
        private MainViewModel _mainViewModel;
        private IBL logic = BLFactory.API;
        private LineModel _line;
        private LineStationModel _stationToShow;
        private bool _isUpdateMode;
        private string _distanceUpdate;
        private string _hourTimeUpdate;
        private string _minTimeUpdate;
        #endregion

        public ShowLineStationViewModel(MainViewModel mainViewModel, LineStationModel station, LineModel line)
        {
            _mainViewModel = mainViewModel;
            StationToShow = station;
            IsUpdateMode = false;
            Line = line;
            MinTimeUpdate = "0";
            HourTimeUpdate = "0";
        }
        #region properties for Caliburn.Micro
        public LineStationModel StationToShow
        {
            get => _stationToShow;
            set
            {
                _stationToShow = value;
                NotifyOfPropertyChange(() => StationToShow);
            }
        }
        public LineModel Line
        {
            get => _line;
            set
            {
                _line = value;
                NotifyOfPropertyChange(() => Line);
            }

        }
        public string ToggleUpdateModeText
        {
            get
            {
                if (IsUpdateMode)
                {
                    return "Disable Update Mode.";
                }
                return "Activate Update Mode.";
            }
        }
        public bool IsUpdateMode
        {
            get => _isUpdateMode;
            set
            {
                _isUpdateMode = value;
                NotifyOfPropertyChange(() => IsUpdateMode);
                NotifyOfPropertyChange(() => ToggleUpdateModeText);
                NotifyOfPropertyChange(() => UpdateModeVisibility);
                NotifyOfPropertyChange(() => ToggleUpdateModeSpan);
                NotifyOfPropertyChange(() => ViewModeVisibility);
            }
        }
        public Visibility UpdateModeVisibility
        {
            get
            {
                return IsUpdateMode ? Visibility.Visible : Visibility.Hidden;
            }
        }
        public int ToggleUpdateModeSpan
        {
            get => IsUpdateMode ? 1 : 2;
        }
        public Visibility ViewModeVisibility
        {
            get
            {
                return IsUpdateMode ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public string DistanceUpdate
        {
            get => _distanceUpdate;
            set
            {
                if (double.TryParse(value, out _))
                {
                    _distanceUpdate = value;
                    NotifyOfPropertyChange(() => CanUpdateChanges);
                }
            }
        }
        public bool CanUpdateChanges
        {
            get
            {
                return int.Parse(MinTimeUpdate) != 0 || int.Parse(HourTimeUpdate) != 0;
            }
        }
        public string TimeUpdate
        {
            get => $"{_minTimeUpdate}:{_hourTimeUpdate}";
            set
            {
                if (value.Contains(":"))
                {
                    string[] timeData = value.Split(':');
                    int min, hour;
                    //We dont want to update only one side of the value so we need to do check for both hour and min again
                    if (int.TryParse(timeData[0], out hour) && int.TryParse(timeData[1], out min))
                    {

                        if (min >= 0 && min < 60 && hour < 24 && hour >= 0)
                        {
                            HourTimeUpdate = timeData[0];
                            MinTimeUpdate = timeData[1];
                        }
                        NotifyOfPropertyChange(() => CanUpdateChanges);
                    }
                }
            }
        }
        public string MinTimeUpdate
        {
            get => _minTimeUpdate;
            set
            {
                int min;
                if (int.TryParse(value, out min))
                {
                    if (min >= 0 && min < 60)
                    {
                        _minTimeUpdate = value;
                        NotifyOfPropertyChange(() => MinTimeUpdate);
                        NotifyOfPropertyChange(() => TimeUpdate);
                        NotifyOfPropertyChange(() => CanUpdateChanges);
                    }
                }
            }
        }
        public string HourTimeUpdate
        {
            get => _hourTimeUpdate;
            set
            {
                int hour;
                if (int.TryParse(value, out hour))
                {
                    if (hour < 24 && hour >= 0)
                    {
                        _hourTimeUpdate = value;
                        NotifyOfPropertyChange(() => HourTimeUpdate);
                        NotifyOfPropertyChange(() => TimeUpdate);
                        NotifyOfPropertyChange(() => CanUpdateChanges);
                    }
                }
            }
        }
        #endregion
        #region events
        public void ToggleUpdateMode()
        {
            IsUpdateMode = !IsUpdateMode;
        }
        public void UpdateChanges()
        {
            StationToShow.DistanceFromNext = double.Parse(DistanceUpdate);
            StationToShow.TimeFromNext = new TimeSpan(int.Parse(HourTimeUpdate), int.Parse(MinTimeUpdate), 0);
            logic.UpdateNearStations(int.Parse(StationToShow.Station.Code), int.Parse(StationToShow.Next.Code), StationToShow.DistanceFromNext, StationToShow.TimeFromNext);
            StationToShow = StationToShow;//Notify of all the changes
            MessageBox.Show("The Station data has been updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
