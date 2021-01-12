using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;
using BL.BO;
using BL;

namespace PL.ViewModels
{
    class ShowLineDataViewModel : Screen
    {
        private LineModel _line;
        private BindableCollection<StationModel> _linePath;
        private MainViewModel _mainViewModel;
        private StationModel _selectedStation;
        private IBL logic = BLFactory.API;

        public ShowLineDataViewModel(MainViewModel mainViewModel, LineModel line)
        {
            _mainViewModel = mainViewModel;
            Line = line;
            LinePath = _line.Stations;

        }

        public LineModel Line
        {
            get => _line;
            set
            {
                _line = value;
                NotifyOfPropertyChange(() => Line);
                NotifyOfPropertyChange(() => LineNumber);
            }
        }
        public string LineNumber
        {
            get => _line.Code.ToString();
            set
            {
                if (int.TryParse(value, out _))
                {
                    _line.Code = int.Parse(value);
                    NotifyOfPropertyChange(() => Line);
                    NotifyOfPropertyChange(() => Line.Code);
                }
            }
        }
        public string Area
        {
            get => _line.Area;
        }
        public BindableCollection<StationModel> LinePath
        {
            get => _linePath;
            set
            {
                _linePath = value;
                NotifyOfPropertyChange(() => LinePath);
            }
        }
        public StationModel SelectedStation
        {
            get => _selectedStation;
            set
            {
                _selectedStation = value;
                NotifyOfPropertyChange(() => SelectedStation);
            }
        }
        public void ShowLineStationData()
        {
            if (SelectedStation == null)
            {
                return;
            }
            BOStation next, prev;
            BOLineStation boLineStaiton = logic.GetLineStationFromStationAndLine(Line.Id, int.Parse(SelectedStation.Code), out next, out prev);

            LineStationModel toSend = new LineStationModel
            {
                Next = next == null ? new StationModel() : new StationModel(next),
                Prev = prev == null ? new StationModel() : new StationModel(prev),
                Station = new StationModel(boLineStaiton),
                DistanceFromNext = boLineStaiton.DistanceFromNext,
                TimeFromNext = boLineStaiton.TimeFromNext
            };

            _mainViewModel.LoadPageNoBack("ShowLineStationData", toSend, Line);

        }



    }
}
