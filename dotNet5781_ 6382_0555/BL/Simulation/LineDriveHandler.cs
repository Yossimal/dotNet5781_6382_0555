using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BL.BO;
using DALAPI.DAO;
using DALAPI;
using System.ComponentModel;

namespace BL.Simulation
{
    internal class LineDriveHandler
    {
        //public event EventHandler<>
        private static Random _rand = new Random(DateTime.Now.Millisecond);
        private Simulator _simulator = Simulator.Instance;
        private volatile bool _needToStop;
        private TimeSpan _startTime;
        private BOLine _line;
        private BOStation _stationToTrack;
        private List<BOLineStation> _linePath;
        private BackgroundWorker _driveBackgroundWorker;
        private const float MINIMUM_COEFFICIENT = 0.9f;
        private const float MAXIMUM_COEFFICIENT = 2f;


        public LineDriveHandler(BOLine lineToDrive, BOStation stationToTrack,TimeSpan startTime)
        {
            this._startTime = startTime;
            this._stationToTrack = stationToTrack;
            this._needToStop = false;
            this._line = lineToDrive;
            this._linePath = GetLinePath().ToList();
            this._driveBackgroundWorker = new BackgroundWorker();
            this._driveBackgroundWorker.DoWork += DoWork;
            this._driveBackgroundWorker.ProgressChanged += ProgressChanged;
            this._driveBackgroundWorker.WorkerReportsProgress = true;
            this._driveBackgroundWorker.RunWorkerCompleted += RunWorkerCompleted;

        }
        public void StartDrive()
        {
            _driveBackgroundWorker.RunWorkerAsync();
        }
        public void StopDrive()
        {
            _needToStop = true;
        }
        private void DoWork(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (!_needToStop && CalculateArrivalTime(_simulator.CurrentTime)>TimeSpan.Zero)
            {
                BOLineTiming timing = new BOLineTiming();
                timing.ArrivalTime = CalculateArrivalTime(_simulator.CurrentTime);
                timing.LastStationName = BL.Instance.GetLastStation(_line.Id).Name;
                timing.LineNumber = _line.LineNumber;
                timing.StartTime = _startTime;
                timing.LineNumber = _line.LineNumber;
                timing.LineId = _line.Id;
                timing.TrackStationId = _stationToTrack.Code;
                worker.ReportProgress(0, timing);
                Thread.Sleep(1000);
            }
            //set the finish report
            BOLineTiming result = new BOLineTiming();
            result.ArrivalTime = TimeSpan.Zero;
            result.LastStationName = BL.Instance.GetLastStation(_line.Id).Name;
            result.LineNumber = _line.LineNumber;
            result.StartTime = _startTime;
            result.LineNumber = _line.LineNumber;
            result.LineId = _line.Id;
            result.TrackStationId = _stationToTrack.Code;
            args.Result= result;
        }
        private void ProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            BOLineTiming timing = args.UserState as BOLineTiming;
            DrivesManager.Instance.LineUpdate(timing);
        }
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            BOLineTiming result = args.Result as BOLineTiming;
            DrivesManager.Instance.LineFinish(result);
        }
        private TimeSpan CalculateArrivalTime(TimeSpan currentTime)
        {
            return GetTime()- (currentTime-_startTime);
        }
        private BOStation CalculateCurrentStation(TimeSpan currentTime)
        {
            //calculate the current station from the bus last station and the bus current time
            return null;
        }

        private IEnumerable<BOLineStation> GetLinePath()
        {
            return _line.Path.Select(station => BL.Instance.GetLineStationFromStationAndLine(_line.Id, station.Code, out _, out _))
                             .Select(station =>
                             {
                                 station.TimeFromNext = RandomizeTime(station.TimeFromNext);
                                 return station;
                             });
        }

        private TimeSpan RandomizeTime(TimeSpan timeFromNext)
        {
            double coefficient = _rand.NextDouble() * (MAXIMUM_COEFFICIENT - MINIMUM_COEFFICIENT) + MINIMUM_COEFFICIENT;
            return TimeSpan.FromSeconds(timeFromNext.TotalSeconds * coefficient);
        }
        private double GetDistance() {
            double ret = 0;
            for(int i = 0; i < _linePath.Count; i++)
            {
                if (_linePath[i].Code == _stationToTrack.Code) {
                    break;
                }
                ret += _linePath[i].DistanceFromNext;
            }
            return ret;
        }
        private TimeSpan GetTime()
        {
            TimeSpan ret = TimeSpan.Zero;
            for (int i = 0; i < _linePath.Count; i++)
            {
                if (_linePath[i].Code == _stationToTrack.Code)
                {
                    break;
                }
                ret += _linePath[i].TimeFromNext;
            }
            return ret;
        }
    }
}
