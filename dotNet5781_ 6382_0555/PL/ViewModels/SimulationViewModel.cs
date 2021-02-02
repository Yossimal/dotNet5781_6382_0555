using BL;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public SimulationViewModel(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            this._currentTime = TimeSpan.Zero;
            this._startTime = TimeSpan.Zero;
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
    }
}
