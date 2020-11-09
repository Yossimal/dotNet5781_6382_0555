using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotNet5781_02__6382_0555
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stationsData = File.ReadAllLines(@".\..\..\..\Stations.dat");
            List<BusStation> stations = ReadData(stationsData);
            Console.WriteLine("Hello World!");
        }
        private static List<BusStation> ReadData(string[] arr)
        {
            List<BusStation> ret = new List<BusStation>();
            foreach (string str in arr)
            {
                if (ret.Where(p => p.Code == int.Parse(str.Split(' ')[1])).Count() != 0)
                {
                    throw new ArgumentException("The station code must be unique");
                }
                ret.Add(new BusStation(int.Parse(str.Split(' ')[1]), str.Split(' ')[0]));
            }
            return ret;
        }
        private static LineBusCollection GenerateLines(List<BusStation> stations)
        {
            LineBusCollection ret = new LineBusCollection();
            for (int i = 1; i <= 10; i++)
            {
                BusLine line = new BusLine(i, Area.General);
                for (int j = 0; j < stations.Count; j++)
                {
                    if (j % i == 0 || j % (i + 1) == 0)
                    {
                        Random rand = new Random(DateTime.Now.Millisecond);
                        LineBusStation station = new LineBusStation(stations[j].Code,
                            stations[j].Location, stations[j].Address, rand.Next(1, 51),
                            new TimeSpan(rand.Next(0, 2), rand.Next(0, 60), rand.Next(1, 60)));
                        line.Add(station);
                    }
                }
                ret.Add(line);
            }
            return ret;
        }

    }
}

