using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    class Location
    {
        private double latitude;
        private double longitude;

        public Location(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public override string ToString() { return $"{latitude}°N {longitude}°E"; }
        public static Location GetRandomLocation()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            return new Location(rand.NextDouble() * 180 - 90, rand.NextDouble() * 360 - 180);
        }
        public static Location GetRandomLocation(double minLat, double maxLat, double minLong, double maxLong)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            double latitude = rand.NextDouble() * (maxLat - minLat) + minLat;
            double longitude = rand.NextDouble() * (maxLong - minLong) + minLong;
            return new Location(latitude, longitude);
        }

    }
}
