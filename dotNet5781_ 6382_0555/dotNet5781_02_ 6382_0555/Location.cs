using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    /// <summary>
    /// location in earth that described by latitude and longtude
    /// </summary>
    class Location
    {
        private double latitude;
        private double longitude;
        /// <summary>
        /// Generate Location
        /// </summary>
        /// <param name="latitude">The location latitude</param>
        /// <param name="longitude">The location longtude</param>
        public Location(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A string that describbes the location</returns>
        public override string ToString() { return $"{latitude}°N {longitude}°E"; }
        /// <summary>
        /// Random location in all earth
        /// </summary>
        /// <returns>Random location</returns>
        public static Location GetRandomLocation()
        {
            return new Location(Program.rand.NextDouble() * 180 - 90, Program.rand.NextDouble() * 360 - 180);
        }
        /// <summary>
        /// Random location bounded in specific squere
        /// </summary>
        /// <param name="minLat">The minimum latitude point</param>
        /// <param name="maxLat">The maximum latitude point</param>
        /// <param name="minLong">The minimum longtude point</param>
        /// <param name="maxLong">The maximum longtude point</param>
        /// <exception cref="InvalidOperationException">One of the parameters is out of range when latitude can be in [-90,90] and longtude can be in [-180,180]</exception>
        /// <returns></returns>
        public static Location GetRandomLocation(double minLat, double maxLat, double minLong, double maxLong)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            if (minLat < -90 || minLong < -180 || maxLat > 90 || maxLong > 180) {
                throw new InvalidOperationException("One of the parameters is out of range");
            }
             double latitude = rand.NextDouble() * (maxLat - minLat) + minLat;
            double longitude = rand.NextDouble() * (maxLong - minLong) + minLong;
            return new Location(latitude, longitude);
        }

    }
}
