// Aharon Kremer 034706382 & Yosef Malka 208090555
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace dotNet5781_02__6382_0555
{
    /// <summary>
    /// The client class
    /// </summary>
    class Program
    {
        public static Random rand = new Random(DateTime.Now.Millisecond);
        private const ConsoleColor ChosenTextColor = ConsoleColor.Red;
        private const ConsoleColor ChosenTextBGC = ConsoleColor.Gray;
        private static string stationsDataPath = @".\..\..\..\Stations.dat";
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="args">
        /// cmd arguments 
        /// [0] -> optional: path for the stations data file
        /// </param>
        static void Main(string[] args)
        {
            stationsDataPath = args.Length >= 1 ? args[0] : stationsDataPath;
            string[] stationsData = File.ReadAllLines(args.Length==0?stationsDataPath:args[0]);
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
        /// <summary>
        /// rading from the end-user bus line and adding it to the given collection
        /// </summary>
        /// <param name="lines">The ollection that we want to add the line to</param>
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
        /// <summary>
        /// Get a station number from the user (can choose from the given list) and add it to an existing bus line
        /// </summary>
        /// <param name="lines">The collection of the bus lines that he can add the station to</param>
        /// <param name="stations">The list of the stations that the user can add</param>
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
        /// <summary>
        /// Get a line number from the end-user and removing it from the collection
        /// </summary>
        /// <param name="lines">The bus lines collection</param>
        private static void DelBusLine(BusLineCollection lines)
        {
            int lineNum = ReadLineNumber();
            lines.Remove(lines[lineNum, 0]);
        }
        /// <summary>
        /// Get a station number from the end-user and a bus line station and removing the station from the line path
        /// </summary>
        /// <param name="lines">The collection of the bus linesd that the user can choose from</param>
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
        /// <summary>
        /// Get a station code from the user and printing all the numbers of the lines from the given collection that have that station in the path
        /// </summary>
        /// <param name="lines">The lines collection</param>
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
        /// <summary>
        /// Gets two station codes from the end-user and printing all the buses that moving from the first station to the second one in their path
        /// </summary>
        /// <param name="lines">The collection of the lines for search path</param>
        /// <exception cref="InvalidOperationException">The user inserted the same code in the first and the second station</exception>
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
        /// <summary>
        /// Print all the lines and their pathes from a given collection
        /// </summary>
        /// <param name="lines">The line collection to print</param>
        private static void PrintLines(BusLineCollection lines)
        {
            foreach (BusLine line in lines)
            {
                Console.WriteLine(line);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// print all the stations from a given list and for each staion print all the line numbers of the lines that have that station in their path
        /// </summary>
        /// <param name="lines">The lines collection</param>
        /// <param name="stations">The stations list to print</param>
        private static void PrintStationsLines(BusLineCollection lines, List<BusStation> stations)
        {
            foreach (BusStation station in stations)
            {
                Console.WriteLine($"{ station }:");
                List<BusLine> lineToPrint = lines.GetStationLines(station.Code);
                Console.Write("lines:\t");
                lineToPrint.ForEach(line => Console.Write($"{line.LineNum} "));
                Console.WriteLine("\n");
                
            }
        }
        /// <summary>
        /// Read an area from the end-user
        /// </summary>
        /// <param name="area">The area that we have readed will be here</param>
        /// <returns>true if the user inserted legal area and false for illigal area</returns>
        private static bool GetArea(out Area area)
        {
            char action;
            if (!char.TryParse(Console.ReadLine().ToLower(), out action))
            {
                area = Area.General;
                return false;
            }

            if (!Enum.IsDefined(typeof(Area), (int)action))
            {
                area = Area.General;
                return false;
            }
            area = (Area)action;
            return true;
        }
        /// <summary>
        /// Read all the bus stations from formatted array
        /// </summary>
        /// <param name="arr">stations array that each argument contains a station in the format #address #code</param>
        /// <returns>A deserialized list from the array</returns>
        private static List<BusStation> ReadData(string[] arr)
        {
            List<BusStation> ret = new List<BusStation>();
            foreach (string str in arr)
            {
                Thread.Sleep(1);
                if (ret.Where(p => p.Code == int.Parse(str.Split(' ')[1])).Count() != 0)
                {
                    throw new ArgumentException("The station code must be unique");
                }
                ret.Add(new BusStation(int.Parse(str.Split(' ')[1]), str.Split(' ')[0]));
            }
            return ret;
        }
        /// <summary>
        /// Gets 10 lines that have all the exercise requirments
        /// </summary>
        /// <param name="stations">The stations list</param>
        /// <returns>BusLineCollection that contains all the generated lines</returns>
        private static BusLineCollection GenerateLines(List<BusStation> stations)
        {
            BusLineCollection ret = new BusLineCollection();
            for (int i = 1; i <= 10; i++)
            {
                BusLine line = new BusLine(i, GetRandomArea());
                for (int j = 0; j < stations.Count; j++)
                {
                    if ((j %  10)+1== i || (j % 10)+1 == (i + 1))//each station will get the line of his mod with max lines and the next line
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
        /// <summary>
        /// Handeling the multichoice menue.
        /// Each user actions clearing the console.
        /// </summary>
        /// <param name="items">The oprions that the user can choose</param>
        /// <param name="index">The current index of the user choice</param>
        /// <returns>The user choice or "" if the user didn't pressed the Enter key</returns>
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
        /// <summary>
        /// Reading a bus line number from the user
        /// </summary>
        /// <returns>The line number that the user inserted</returns>
        /// <exception cref="InvalidOperationException">The end-user didn't inserted number OR the number that the user inserted is out of the line range (the range is [0,999])</exception>
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
        /// <summary>
        /// Read a bus station code from the end-user
        /// </summary>
        /// <returns>The code that the user inserted</returns>
        /// <exception cref="InvalidOperationException">The end-user didn't inserted a number OR the number that the end-user inserted is out of the code range (the range is [0,999999]</exception>
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
        /// <summary>
        /// Get a random area
        /// </summary>
        /// <returns>The random area</returns>
        private static Area GetRandomArea() {
            Array values = typeof(Area).GetEnumValues();
            return (Area)values.GetValue(rand.Next(values.Length - 1));
        } 

    }
}

