using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    public class LineBusStation : BusStation
    {
        private float distanceFromPre;
        private TimeSpan timeFromPre;

        public float DistanceFromPre { get => distanceFromPre; set => distanceFromPre = value; }
        public TimeSpan TimeFromPre { get => timeFromPre; set => timeFromPre = value; }

        /// <summary>
        /// Generate new LineBusStation
        /// </summary>
        /// <param name="code">The station code</param>
        /// <param name="location">The station location</param>
        /// <param name="address">The station address</param>
        /// <param name="distanceFromPre">The distance from the previos station</param>
        /// <param name="timeFromPre">The travel time from the previos station</param>
        public LineBusStation(int code, Location location, string address, float distanceFromPre,
            TimeSpan timeFromPre) : base(code, location, address)
        {
            this.distanceFromPre = distanceFromPre;
            this.timeFromPre = timeFromPre;
        }
        /// <summary>
        /// Generate new LineBusStation
        /// </summary>
        /// <param name="code">The station code</param>
        /// <param name="address">The station address</param>
        /// <param name="distanceFromPre">The distance from the previos station</param>
        /// <param name="timeFromPre">The travel duration from the previos station</param>
        public LineBusStation(int code, string address, float distanceFromPre,
     TimeSpan timeFromPre) : base(code, address)
        {
            this.distanceFromPre = distanceFromPre;
            this.timeFromPre = timeFromPre;
        }

    }
}
