using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    class BusStation
    {
        protected int code;
        protected readonly Location location;
        protected string address;

        public BusStation(int code, Location location, string address)
        {
            this.code = code;
            this.location = location;
            this.address = address;
        }
        public BusStation(int code, string address)
        {
            this.code = code;
            this.location = Location.GetRandomLocation(31,33.3,34.3,35.5);
            this.address = address;
        }

        public int Code { get => code; }

        internal Location Location => location;

        public string Address { get => address; }
        public override string ToString() { return $"Bus Station Code: {code}, {location}"; }

    }
}
