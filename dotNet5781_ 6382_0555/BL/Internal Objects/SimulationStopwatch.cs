using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    /// <summary>
    /// Improved stopwatch for the simulaation
    /// </summary>
    class SimulationStopwatch
    {
        /// <summary>
        /// the starting time of the stopwatch
        /// </summary>
        private TimeSpan _startTime;
        /// <summary>
        /// a basic stopwatch that will run
        /// </summary>
        private Stopwatch _stopwatch;
        /// <summary>
        /// the rate of the stopwatch
        /// </summary>
        private int _rate = 1;
        /// <summary>
        /// generate new stopwatch
        /// </summary>
        /// <param name="rate">the stopwatch rate</param>
        /// <param name="startTime">the stopwatch start time</param>
        public SimulationStopwatch(int rate,TimeSpan startTime)
        {
            this._startTime = startTime;
            this._stopwatch = new Stopwatch();
            this._rate = rate;
        }
        /// <summary>
        /// get the seconds time calculated with the rate
        /// </summary>
        public TimeSpan Elapsed => TimeSpan.FromMilliseconds(_stopwatch.Elapsed.TotalMilliseconds * _rate);
        /// <summary>
        /// get the elapsed milliseconds calculated with the rate
        /// </summary>
        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds * _rate;
        /// <summary>
        /// get the elapsed ticks calculated with the rate
        /// </summary>
        public long ElapsedTicks => _stopwatch.ElapsedTicks * _rate;
        /// <summary>
        /// check is the stopwatch running
        /// </summary>
        public bool IsRunning => _stopwatch.IsRunning;
        /// <summary>
        /// the curent stopwatch time (calculated with the start time and the rate)
        /// </summary>
        public TimeSpan CurrentTime => TimeSpan.FromMilliseconds(_startTime.TotalMilliseconds + this.ElapsedMilliseconds);
        /// <summary>
        /// start the stopwatch
        /// </summary>
        public void Start()
        {
            _stopwatch.Start();
        }
        /// <summary>
        /// restart the stopwatch
        /// </summary>
        public void Restart() {
            _stopwatch.Restart();
        }
        /// <summary>
        /// stop the stopwatch
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
        }
        
        
    }
}
