using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotNet5781_02__6382_0555
{
    class Program
    {
        public static Random rand = new Random(DateTime.Now.Millisecond);

        private const ConsoleColor ChosenTextColor = ConsoleColor.Red;
        private const ConsoleColor ChosenTextBGC = ConsoleColor.Gray;
        private const string StationsDataPath = @".\..\..\..\Stations.dat";
        static void Main(string[] args)
        {
            string[] stationsData = File.ReadAllLines(args.Length==0?StationsDataPath:args[0]);
            List<BusStation> stations = ReadData(stationsData);
            BusLineCollection lines = GenerateLines(stations);
            List<string> options = Actions.AllActions;
            int index = 0;
            while (true)
            {
                try
                {
                    string str = MenuHandler(options, ref index);
                    Console.Clear();
                    switch (str)
                    {
                        case Actions.AddLine:
                            AddBusLine(lines);
                            break;
                        case Actions.AddStation:
                            AddStation(lines, stations);
                            break;
                        case Actions.DelLine:
                            DelBusLine(lines);
                            break;
                        case Actions.DelStation:
                            DelLineStation(lines);
                            break;
                        case Actions.SearchStationLines:
                            SearchStationLines(lines);
                            break;
                        case Actions.SearchPathBetweenStations:
                            SearchPath(lines);
                            break;
                        case Actions.PrintAllLines:
                            PrintLines(lines);
                            break;
                        case Actions.PrintAllSationsLines:
                            PrintStationsLines(lines, stations);
                            break;
                        case Actions.Exit:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine("Press enter to continue");
                    Console.ReadLine();
                }
            }
        }

        private static void AddBusLine(BusLineCollection lines)
        {
            int lineNum = ReadLineNumber();
            Area area;
            Console.WriteLine("Enter Area (g,n,s,c,j)");
            if (!GetArea(out area))
            {
                Console.Error.WriteLine("Wrong digit input");
                return;
            }
            lines.Add(new BusLine(lineNum, area));

        }
        private static void AddStation(BusLineCollection lines, List<BusStation> stations)
        {
            int code;
            Console.WriteLine("Enter station code");
            if (!int.TryParse(Console.ReadLine(), out code))
            {
                Console.Error.WriteLine("Number is required");
                return;
            }
            if (!stations.Exists(station => station.Code == code))
            {
                Console.Error.WriteLine("Code is not exists");
                return;
            }
            int lineNum = ReadLineNumber();
            if (!lines.IsExists(lineNum))
            {
                Console.Error.WriteLine("Line is not exists");
                return;
            }
            BusStation retStation = stations.Find(station => station.Code == code);
            Random rand = new Random(DateTime.Now.Millisecond);
            lines[lineNum, 0].Add(new LineBusStation(retStation.Code,
                            retStation.Location, retStation.Address, rand.Next(1, 51),
                            new TimeSpan(rand.Next(0, 2), rand.Next(0, 60), rand.Next(1, 60))));
        }
        private static void DelBusLine(BusLineCollection lines)
        {
            int lineNum = ReadLineNumber();
            lines.Remove(lines[lineNum, 0]);
        }
        private static void DelLineStation(BusLineCollection lines)
        {
            int code = ReadStationCode();
            int lineNum = ReadLineNumber();
            if (lines[lineNum].Length >= 2)
            {
                char userChoice = '\0';
                while (userChoice != 'n' && userChoice != 'y')
                {
                    Console.WriteLine("There is more than one line. Delete all (y/n)?");
                    userChoice = (char)Console.Read();
                    Console.WriteLine();
                }
                if (userChoice == 'y')
                {
                    foreach (BusLine line in lines[lineNum])
                    {
                        line.Remove(code);
                    }
                    return;
                }
            }
            lines[lineNum, 0].Remove(code);
        }
        private static void SearchStationLines(BusLineCollection lines)
        {
            int code = ReadStationCode();
            List<BusLine> linesToPrint = lines.GetStationLines(code);
            foreach (BusLine line in linesToPrint)
            {
                Console.Write($"{ line.LineNum }, ");
            }
            Console.WriteLine();
        }
        private static void SearchPath(BusLineCollection lines)
        {
            Console.WriteLine("Input source station:");
            int srcCode = ReadStationCode();
            Console.WriteLine("Input destination station:");
            int destCode = ReadStationCode();
            if (srcCode == destCode)
            {
                throw new InvalidOperationException("Expected two different stations");
            }
            List<BusLine> linesToPrint = new List<BusLine>();
            foreach (BusLine line in lines)
            {
                if (line.IsExists(srcCode) && line.IsExists(destCode) && line.IsBefore(srcCode, destCode))
                {
                    linesToPrint.Add(line.SubPath(srcCode, destCode));
                }
            }
            linesToPrint.Sort();
            linesToPrint.ForEach(line => Console.WriteLine(line));
        }
        private static void PrintLines(BusLineCollection lines)
        {
            foreach (BusLine line in lines)
            {
                Console.WriteLine(line);
                Console.WriteLine();
            }
        }
        private static void PrintStationsLines(BusLineCollection lines, List<BusStation> stations)
        {
            foreach (BusStation station in stations)
            {
                Console.Write($"- { station.Code }:".PadRight(10));
                List<BusLine> lineToPrint = lines.GetStationLines(station.Code);
                lineToPrint.ForEach(line => Console.Write($"{line.LineNum}, "));
                Console.WriteLine();
            }
        }
        private static bool GetArea(out Area act)
        {
            char action;
            if (!char.TryParse(Console.ReadLine().ToLower(), out action))
            {
                act = Area.General;
                return false;
            }

            if (!Enum.IsDefined(typeof(Area), (int)action))
            {
                act = Area.General;
                return false;
            }
            act = (Area)action;
            return true;
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
        private static BusLineCollection GenerateLines(List<BusStation> stations)
        {
            BusLineCollection ret = new BusLineCollection();
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
        private static string MenuHandler(List<string> items, ref int index)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = ChosenTextBGC;
                    Console.ForegroundColor = ChosenTextColor;

                    Console.WriteLine(items[i]);
                }
                else
                {
                    Console.WriteLine(items[i]);
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo ckey = Console.ReadKey();

            if (ckey.Key == ConsoleKey.DownArrow)
            {
                if (index == items.Count - 1)
                {
                    index = 0; //Remove the comment to return to the topmost item in the list
                }
                else { index++; }
            }
            else if (ckey.Key == ConsoleKey.UpArrow)
            {
                if (index <= 0)
                {
                    index = items.Count - 1; //Remove the comment to return to the item in the bottom of the list
                }
                else { index--; }
            }
            else if (ckey.Key == ConsoleKey.Enter)
            {
                return items[index];
            }
            else
            {
                return "";
            }

            Console.Clear();
            return "";
        }
        private static int ReadLineNumber()
        {
            int lineNum;
            Console.WriteLine("Enter line number");
            if (!int.TryParse(Console.ReadLine(), out lineNum))
            {
                throw new InvalidOperationException("Number is requierd");
            }
            if (lineNum < 0 || lineNum >= Math.Pow(10, 3))
            {
                throw new InvalidOperationException("Number is invalid");
            }
            return lineNum;
        }
        private static int ReadStationCode()
        {
            int stationCode;
            Console.WriteLine("Enter station code");
            if (!int.TryParse(Console.ReadLine(), out stationCode))
            {
                throw new InvalidOperationException("Number is requierd");
            }
            if (stationCode < 0 || stationCode >= Math.Pow(10, 6))
            {
                throw new InvalidOperationException("Number is invalid");
            }
            return stationCode;
        }

    }
}

