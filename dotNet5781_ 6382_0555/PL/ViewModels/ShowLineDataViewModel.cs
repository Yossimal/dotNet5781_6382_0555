using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;
using BL.BO;
using BL;
using System.Windows;

namespace PL.ViewModels
{
    class ShowLineDataViewModel : Screen
    {
        #region private fields
        private LineModel _line;
        private BindableCollection<StationModel> _allStations;
        private StationModel _selectedStationToAdd;
        private StationModel _selectedStationToAddBefore;
        private MainViewModel _mainViewModel;
        private StationModel _selectedStation;
        private IBL logic = BLFactory.API;
        #endregion
        #region constructors
        public ShowLineDataViewModel(MainViewModel mainViewModel, LineModel line)
        {
            _mainViewModel = mainViewModel;
            Line = line;
            LinePath = _line.Stations;
            IEnumerable<StationModel> allStations = logic.AllStations().Select(station => new StationModel
            {
                Code = station.Code.ToString(),
                Name = station.Name
            });
            AllStations = new BindableCollection<StationModel>(allStations);
            SelectedStationToAdd = AllStations[0];
            SelectedStationToAddBefore = LinePath[0];
        }
        #endregion
        #region properties for Caliburn.Micro
        #region data properties
        public LineModel Line
        {
            get => _line;
            set
            {
                _line = value;
                SelectedStationToAdd = _line.Stations[0];
                SelectedStationToAddBefore = _line.Stations[0];
                NotifyOfPropertyChange(() => Line);
                NotifyOfPropertyChange(() => LinePath);
                NotifyOfPropertyChange(() => PathCombobox);
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
            get => Line.Stations;
            set
            {
                Line.Stations = value;
                NotifyOfPropertyChange(() => LinePath);
                NotifyOfPropertyChange(() => PathCombobox);
                NotifyOfPropertyChange(() => Line);
            }
        }
        public BindableCollection<StationModel> AllStations
        {
            get => _allStations;
            set
            {
                _allStations = value;
                NotifyOfPropertyChange(() => AllStations);
            }
        }
        public BindableCollection<StationModel> PathCombobox
        {
            get => Line.Stations;
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
        public StationModel SelectedStationToAdd
        {
            get => _selectedStationToAdd;
            set
            {
                _selectedStationToAdd = value;
                NotifyOfPropertyChange(() => SelectedStationToAdd);
                NotifyOfPropertyChange(() => CanAddStationAfter);
                NotifyOfPropertyChange(() => CanAddStationInEnd);
            }
        }
        public StationModel SelectedStationToAddBefore
        {
            get => _selectedStationToAddBefore;
            set
            {
                _selectedStationToAddBefore = value;
                NotifyOfPropertyChange(() => SelectedStationToAddBefore);
                NotifyOfPropertyChange(() => CanAddStationAfter);
            }
        }
        #endregion
        #region event properties
        public bool CanAddStationAfter
        {
            get
            {
                if (SelectedStationToAdd == null)
                {
                    return false;
                }
                return CanAddStationInEnd
                    && !LinePath.Any(station => station.Code == SelectedStationToAdd.Code);
            }
        }
        public bool CanAddStationInEnd
        {
            get
            {
                return !LinePath.Any(s=>s.Code==SelectedStationToAdd.Code);
            }
        }
        #endregion
        #endregion
        #region events
        public async void AddStationAfter()
        {
            try
            {
                int index = LinePath.IndexOf(SelectedStationToAddBefore);
                BOLine newLine = await logic.AddStationToLine(int.Parse(SelectedStationToAdd.Code), Line.Id, index);
                LineModel newLineModel = new LineModel
                {
                    Code = newLine.LineNumber,
                    Id = Line.Id,
                    Area = Line.Area
                };
                IEnumerable<StationModel> newPath = newLine.Path.Select(station => new StationModel
                {
                    Code = station.Code.ToString(),
                    Name = station.Name
                });
                newLineModel.Stations = new BindableCollection<StationModel>(newPath);
                Line = newLineModel;
                MessageBox.Show("The station was added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error was occures while trying to add the station.\nIf thet problem appear again, contact us.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void AddStationInEnd()
        {
            try
            {
                BOLine newLine = await logic.AddStationToLine(int.Parse(SelectedStationToAdd.Code), Line.Id);
                LineModel newLineModel = new LineModel
                {
                    Code = newLine.LineNumber,
                    Id = Line.Id,
                    Area = Line.Area
                };
                IEnumerable<StationModel> newPath = newLine.Path.Select(station => new StationModel
                {
                    Code = station.Code.ToString(),
                    Name = station.Name
                });
                newLineModel.Stations = new BindableCollection<StationModel>(newPath);
                Line = newLineModel;
                MessageBox.Show("The station was added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error was occures while trying to add the station.\nIf thet problem appear again, contact us.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        public async Task RemoveStation(string code) {
            BOLine afterRemove=await logic.RemoveStationFromLine(Line.Id, int.Parse(code));
            Line = new LineModel()
            {
                Area = afterRemove.Area,
                Code = afterRemove.LineNumber,
                Id = afterRemove.Id,
                Stations = new BindableCollection<StationModel>(afterRemove.Path.Select(s => new StationModel(s)))
            };
            MessageBox.Show("The Station has been removed succesfuly", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion


    }
}
