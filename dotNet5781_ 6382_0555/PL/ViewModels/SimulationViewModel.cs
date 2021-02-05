using BL;
using BL.BO;
using BL.Simulation.EventArgs;
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
    class SimulationViewModel : Screen
    {
        private MainViewModel _mainViewModel;
        private TimeSpan _currentTime;
        private int _rate;
        private TimeSpan _startTime;
        private IBL logic = BLFactory.API;
        private bool _isSimulationRunning = false;
        private BindableCollection<StationModel> _stations;
        private BindableCollection<LineDriveModel> _drives;
        private StationModel _selectedStation;
        private BindableCollection<YellowSignModel> _yellowSign;


        public SimulationViewModel(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            this._currentTime = TimeSpan.Zero;
            this._startTime = TimeSpan.Zero;
            this.logic.OnLineUpdate(OnLineUpdate);
            this.logic.OnLineFinish(OnLineFinish);
            this.Stations = new BindableCollection<StationModel>(logic.AllStations().Select(s => new StationModel { Code = s.Code.ToString(), Name = s.Name }));
            this.Drives = new BindableCollection<LineDriveModel>();
        }
        public string CurrentTime
        {
            get
            {
                return $"{_currentTime.Hours}:{_currentTime.Minutes}:{_currentTime.Seconds}";
            }
        }
        public int Rate
        {
            get => this._rate;
            set
            {
                this._rate = value;
                NotifyOfPropertyChange(() => Rate);
            }
        }
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                NotifyOfPropertyChange(() => StartTime);
            }
        }
        public int StartSeconds
        {
            get => _startTime.Seconds;
            set
            {
                _startTime = new TimeSpan(_startTime.Hours, _startTime.Minutes, value);
                NotifyOfPropertyChange(() => StartSeconds);
                NotifyOfPropertyChange(() => StartTime);
            }
        }
        public int StartMinuts
        {
            get => _startTime.Minutes;
            set
            {

                _startTime = new TimeSpan(_startTime.Hours, value, _startTime.Seconds);
                NotifyOfPropertyChange(() => StartMinuts);
                NotifyOfPropertyChange(() => StartTime);
            }
        }
        public int StartHours
        {
            get => _startTime.Hours;
            set
            {

                _startTime = new TimeSpan(value, _startTime.Minutes, _startTime.Seconds);
                NotifyOfPropertyChange(() => StartHours);
                NotifyOfPropertyChange(() => StartTime);
            }
        }
        public string SimulationToggleButtonText
        {
            get
            {
                if (_isSimulationRunning)
                {
                    return "Stop the simulation";
                }
                else
                {
                    return "Start the simulation";
                }
            }
        }
        public BindableCollection<StationModel> Stations
        {
            get => _stations;
            set
            {
                _stations = value;
                NotifyOfPropertyChange(() => Stations);
            }
        }
        public BindableCollection<LineDriveModel> Drives
        {
            get => _drives;
            set
            {
                this._drives = value;
                NotifyOfPropertyChange(() => Drives);
            }
        }
        public StationModel SelectedStation
        {
            get => _selectedStation;
            set
            {
                _selectedStation = value;
                NotifyOfPropertyChange(() => SelectedStation);
                NotifyOfPropertyChange(() => CurrentStation);
            }
        }
        public string CurrentStation => SelectedStation != null ? SelectedStation.Name : "Choose station from the list";
        public Visibility SimulationDataVisibility
        {
            get => _isSimulationRunning ? Visibility.Visible : Visibility.Hidden;
        }
        public BindableCollection<YellowSignModel> YellowSign
        {
            get => _yellowSign;
            set
            {
                _yellowSign = value;
                NotifyOfPropertyChange(() => YellowSign);
            }
        }
        public void SimulationButtonClick()
        {
            if (_isSimulationRunning)
            {
                StopSimulator();
            }
            else
            {
                RunSimulation();
            }
            _isSimulationRunning = !_isSimulationRunning;
            NotifyOfPropertyChange(() => SimulationToggleButtonText);
            NotifyOfPropertyChange(() => SimulationDataVisibility);
        }

        public void SetStation()
        {
            if (SelectedStation == null)
            {
                return;
            }
            logic.SetStationToTrack(int.Parse(SelectedStation.Code));
            Drives = new BindableCollection<LineDriveModel>();
            IEnumerable<YellowSignModel> dataForYellowSign = logic.AllLinesInStation(int.Parse(SelectedStation.Code))
                                                                  .Select(ys => new YellowSignModel() { LastStationName = ys.LastStationName, LineNumber = ys.LineNumber });
            YellowSign = new BindableCollection<YellowSignModel>(dataForYellowSign);
        }
        private void RunSimulation()
        {
            logic.StartSimulator(StartTime, Rate, OnTimeChange);
        }
        private void StopSimulator()
        {
            logic.StopSimulator();
        }
        private void OnTimeChange(TimeSpan span)
        {
            _currentTime = span;
            NotifyOfPropertyChange(() => CurrentTime);
        }
        private void OnLineUpdate(object sender, LineDriveEventArgs args)
        {

            BOLineTiming timing = args.lineTiming;
            if (SelectedStation.Code != timing.TrackStationId.ToString())
            {
                return;
            }
            try
            {
                LineDriveModel drive = Drives.FirstOrDefault(d => timing.LineId == d.LineId);
                if (drive == null)
                {
                    drive = new LineDriveModel()
                    {
                        LineId = timing.LineId,
                        LineNumber = timing.LineNumber,
                        NearestArrivalTime = timing.ArrivalTime,
                        LastStationName = timing.LastStationName
                    };

                    Drives.Add(drive);
                }
                else if (drive.NearestArrivalTime > timing.ArrivalTime)
                {
                    drive.NearestArrivalTime = timing.ArrivalTime;
                }
                Drives = new BindableCollection<LineDriveModel>(Drives.OrderBy(d => d.NearestArrivalTime));
            }
            catch { }
        }
        private void OnLineFinish(object sender, LineDriveEventArgs args)
        {

            LineDriveModel toRemove = Drives.FirstOrDefault(d => d.LineId == args.lineTiming.LineId);
            if (toRemove == null)
            {
                return;
            }
            try
            {
                Drives.Remove(toRemove);
            }
            catch { }
        }
    }
}
