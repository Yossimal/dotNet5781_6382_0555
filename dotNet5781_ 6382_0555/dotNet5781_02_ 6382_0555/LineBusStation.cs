using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    class LineBusStation : BusStation
    {
        private float distanceFromPre;
        private TimeSpan timeFromPre;

        public float DistanceFromPre { get => distanceFromPre; set => distanceFromPre = value; }
        public TimeSpan TimeFromPre { get => timeFromPre; set => timeFromPre = value; }

        public LineBusStation(int code, Location location, string address, float distanceFromPre,
            TimeSpan timeFromPre) : base(code, location, address)
        {
            this.distanceFromPre = distanceFromPre;
            this.timeFromPre = timeFromPre;
        }
        public LineBusStation(int code, string address, float distanceFromPre,
     TimeSpan timeFromPre) : base(code, address)
        {
            this.distanceFromPre = distanceFromPre;
            this.timeFromPre = timeFromPre;
        }

    }
}
