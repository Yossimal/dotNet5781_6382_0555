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
    /// <summary>
    /// handling a single drive
    /// </summary>
    internal class LineDriveHandler
    {
        #region private fileds
        private static Random _rand = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// the simulator
        /// </summary>
        private Simulator _simulator = Simulator.Instance;
        /// <summary>
        /// are we need to stop that specific drive
        /// </summary>
        private volatile bool _needToStop;
        /// <summary>
        /// the start time of that drive
        /// </summary>
        private TimeSpan _startTime;
        /// <summary>
        /// the line that driving
        /// </summary>
        private BOLine _line;
        /// <summary>
        /// the station that we tracking
        /// </summary>
        private BOStation _stationToTrack;
        //the path of the line
        private List<BOLineStation> _linePath;
        /// <summary>
        /// backgroundworker for that drive
        /// </summary>
        private BackgroundWorker _driveBackgroundWorker;
        /// <summary>
        /// const numbers for caculating delays of the arrival time
        /// </summary>
        private const float MINIMUM_COEFFICIENT = 0.9f;
        private const float MAXIMUM_COEFFICIENT = 2f;
        #endregion
        #region public methods
        /// <summary>
        /// initialize the drive handler
        /// </summary>
        /// <param name="lineToDrive">the line that need to drive</param>
        /// <param name="stationToTrack">the station that we want to track</param>
        /// <param name="startTime">the starting time</param>
        public LineDriveHandler(BOLine lineToDrive, BOStation stationToTrack, TimeSpan startTime)
        {

            this._startTime = startTime;
            this._stationToTrack = stationToTrack;
            this._needToStop = false;
            this._line = lineToDrive;
            this._linePath = GetLinePath().ToList();
            //initialize the BGW
            this._driveBackgroundWorker = new BackgroundWorker();
            this._driveBackgroundWorker.DoWork += DoWork;
            this._driveBackgroundWorker.ProgressChanged += ProgressChanged;
            this._driveBackgroundWorker.WorkerReportsProgress = true;
            this._driveBackgroundWorker.RunWorkerCompleted += RunWorkerCompleted;

        }
        /// <summary>
        /// run the drive
        /// </summary>
        public void StartDrive()
        {
            _driveBackgroundWorker.RunWorkerAsync();
        }
        /// <summary>
        /// stop the drive
        /// </summary>
        public void StopDrive()
        {
            _needToStop = true;
        }
        #endregion
        #region BGW event handlers
        /// <summary>
        /// the main BGW DoWork method
        /// </summary>
        private void DoWork(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //while the bus didnt arrived => report the new arrival time each 1sec
            while (!_needToStop && CalculateArrivalTime(_simulator.CurrentTime) > TimeSpan.Zero)
            {
                BOLineTiming timing = new BOLineTiming();
                timing.ArrivalTime = CalculateArrivalTime(_simulator.CurrentTime);
                timing.LastStationName = BL.Instance.GetLastStation(_line.Id).Name;
                timing.LineNumber = _line.LineNumber;
                timing.StartTime = _startTime;
                timing.LineNumber = _line.LineNumber;
                timing.LineId = _line.Id;
                timing.TrackStationId = _stationToTrack.Code;
                worker.ReportProgress(0, timing);//report the progress so we can send the data to the event and invoke it outside of the BGW thread
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
            args.Result = result;
        }
        /// <summary>
        /// the BGW progress changed
        /// </summary>
        private void ProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            BOLineTiming timing = args.UserState as BOLineTiming;
            DrivesManager.Instance.LineUpdate(timing);//invoke the event of the progress changed
        }
        /// <summary>
        /// the BGW RunWorkerCompleted
        /// </summary>
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            BOLineTiming result = args.Result as BOLineTiming;
            DrivesManager.Instance.LineFinish(result);//invoke the event of the finish drive
        }
        #endregion
        #region private methods
        /// <summary>
        /// calculating the arrival time from a given time
        /// </summary>
        /// <param name="currentTime">the time now</param>
        /// <returns></returns>
        private TimeSpan CalculateArrivalTime(TimeSpan currentTime)
        {
            return GetTime() - (currentTime - _startTime);//calculate the arruval time based on the start time
        }
        private BOStation CalculateCurrentStation(TimeSpan currentTime)
        {
            //calculate the current station from the bus last station and the bus current time
            return null;
        }
        /// <summary>
        /// get all the lines in the bus path
        /// </summary>
        /// <returns>all the relevan stations for the current drive</returns>
        private IEnumerable<BOLineStation> GetLinePath()
        {
            return _line.Path.Select(station => BL.Instance.GetLineStationFromStationAndLine(_line.Id, station.Code, out _, out _))
                             .Select(station =>
                             {
                                 station.TimeFromNext = GetRandomDelayTime(station.TimeFromNext);//randomize the timeso we can simulate a delay
                                 return station;
                             });
        }
        /// <summary>
        /// get a random time based on the delay calculation  consts
        /// </summary>
        /// <param name="timeFromNext">the time to randomize (in the project is used for the time from the next station)</param>
        /// <returns>random time based on the delay ratio</returns>
        private TimeSpan GetRandomDelayTime(TimeSpan timeFromNext)
        {
            double coefficient = _rand.NextDouble() * (MAXIMUM_COEFFICIENT - MINIMUM_COEFFICIENT) + MINIMUM_COEFFICIENT;
            return TimeSpan.FromSeconds(timeFromNext.TotalSeconds * coefficient);
        }
        /// <summary>
        /// Get the distance from the destination
        /// </summary>
        /// <returns>the distance from the destination</returns>
        private double GetDistance()
        {
            double ret = 0;
            for (int i = 0; i < _linePath.Count; i++)
            {
                if (_linePath[i].Code == _stationToTrack.Code)
                {
                    break;
                }
                ret += _linePath[i].DistanceFromNext;
            }
            return ret;
        }
        /// <summary>
        /// Get the arrival time to the destination
        /// </summary>
        /// <returns>the remaining time from the destination</returns>
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
    #endregion
}
