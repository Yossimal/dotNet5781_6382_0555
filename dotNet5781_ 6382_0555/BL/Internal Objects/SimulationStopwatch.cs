using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Internal_Objects
{
    class SimulationStopwatch
    {
        private TimeSpan _startTime;
        private Stopwatch _stopwatch;
        private int _rate = 1;
        public SimulationStopwatch(int rate,TimeSpan startTime)
        {
            this._startTime = startTime;
            this._stopwatch = new Stopwatch();
            this._rate = rate;
        }
        public TimeSpan Elapsed => TimeSpan.FromMilliseconds(_stopwatch.Elapsed.TotalMilliseconds * _rate);
        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds * _rate;
        public long ElapsedTicks => _stopwatch.ElapsedTicks * _rate;
        public bool IsRunning => _stopwatch.IsRunning;
        public TimeSpan CurrentTime => TimeSpan.FromMilliseconds(_startTime.TotalMilliseconds + this.ElapsedMilliseconds);
        public void Start()
        {
            _stopwatch.Start();
        }
        public void Restart() {
            _stopwatch.Restart();
        }
        public void Stop()
        {
            _stopwatch.Stop();
        }
        
    }
}
