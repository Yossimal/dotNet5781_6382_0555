using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class ShowLineStationViewModel : Screen
    {
        private MainViewModel _mainViewModel;
        private LineModel _line;
        private LineStationModel _station;

        public ShowLineStationViewModel(MainViewModel mainViewModel, LineStationModel station, LineModel line)
        {
            _mainViewModel = mainViewModel;
            Station = station;
            Line = line;
        }

        public LineStationModel Station
        {
            get => _station;
            set
            {
                _station = value;
                NotifyOfPropertyChange(() => Station);
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
    }
}
