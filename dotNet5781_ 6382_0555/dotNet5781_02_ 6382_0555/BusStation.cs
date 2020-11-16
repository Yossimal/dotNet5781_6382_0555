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

        /// <summary>
        /// Generate new bus station
        /// </summary>
        /// <param name="code">The satation code</param>
        /// <param name="location">The station location</param>
        /// <param name="address">The station address</param>
        public BusStation(int code, Location location, string address)
        {
            if (code>=Math.Pow(10,6))
            {
                throw new ArgumentException("Code has to many digits");
            }
            this.code = code;
            this.location = location;
            this.address = address;
        }
        /// <summary>
        /// Generate new bus station with a random location in Israel
        /// </summary>
        /// <param name="code">The station code</param>
        /// <param name="address">The station address</param>
        public BusStation(int code, string address)
        {
            if (code >= Math.Pow(10, 6))
            {
                throw new ArgumentException("Code has to many digits");
            }
            this.code = code;
            this.location = Location.GetRandomLocation(31,33.3,34.3,35.5);
            this.address = address;
        }
        /// <summary>
        /// Generate new bus station
        /// </summary>
        /// <param name="code">The station code</param>
        /// <param name="location">The station location</param>
        public BusStation(int code,Location location)
        {
            this.location = location;
            this.code = code;
            this.address = "no-address";
        }
        /// <summary>
        /// Generate new bus station
        /// </summary>
        /// <param name="code">The station code</param>
        public BusStation(int code) {
            this.location = Location.GetRandomLocation(31, 33.3, 34.3, 35.5);
            this.code = code;
            this.address = "no-address";
        }

        public int Code { get => code; }

        internal Location Location => location;

        public string Address { get => address; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>string that present the station data</returns>
        public override string ToString() { return $"Bus Station Code: {code}, {location}"; }

    }
}
