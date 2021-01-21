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
        public BOLineTiming LineTiming { get; set; }
        //public event EventHandler<>
        private static Random _rand = new Random(DateTime.Now.Millisecond);
        private IDAL _dataAPI = DALFactory.API;
        private volatile bool _needToStop;
        private BOLine _line;
        private List<BOLineStation> _linePath;
        private BackgroundWorker _driveBackgroundWorker;
        private Action<BOLineTiming> _onProgressAction;
        private Action _finishAction;
        private const float MINIMUM_COEFFICIENT = 0.9f;
        private const float MAXIMUM_COEFFICIENT = 2f;


        public LineDriveHandler(BOLineTiming lineTiming,Action<BOLineTiming> onTrack,Action onFinish)
        {
            this.LineTiming = lineTiming;
            this._needToStop = false;
            this._line = BLFactory.API.GetLine(lineTiming.LineId);
            this._linePath = GetLinePath().ToList();
            this._driveBackgroundWorker = new BackgroundWorker();
            this._driveBackgroundWorker.DoWork += DoWork;
            this._driveBackgroundWorker.ProgressChanged += ProgressChanged;
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
        private  void DoWork(object sender, DoWorkEventArgs args) {
            while (!_needToStop)
            {
                BOLineTiming timing = new BOLineTiming();
                //TODO : calculate the time and report progress
                Thread.Sleep(1000);
            }
        }
        private void ProgressChanged(object sender, ProgressChangedEventArgs args) {
            BOLineTiming timing = args.UserState as BOLineTiming;
            _onProgressAction(timing);
        }
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args) {
            _finishAction();
        }

        private TimeSpan CalculateArrivalTime(TimeSpan currentTime) {
            //TODO : calculate the time using the current time and the bus starting time
            return TimeSpan.Zero;
        }
        private BOStation CalculateCurrentStation(TimeSpan currentTime)
        {
            //calculate the current station from the bus last station and the bus current time
            return null;
        }

        private IEnumerable<BOLineStation> GetLinePath()
        {
            return _line.Path.Select(station => BL.Instance.GetLineStationFromStationAndLine(_line.Id, station.Code, out _, out _))
                             .Select(station=> {
                                 station.TimeFromNext = RandomizeTime(station.TimeFromNext);
                                 return station;
                             });
        }

        private TimeSpan RandomizeTime(TimeSpan timeFromNext)
        {
            double coefficient = _rand.NextDouble() * (MAXIMUM_COEFFICIENT - MINIMUM_COEFFICIENT) + MINIMUM_COEFFICIENT;
            return TimeSpan.FromSeconds(timeFromNext.TotalSeconds * coefficient);
        }
    }
}
