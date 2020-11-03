//Aharon Kremer 034706382 & Yosef Malka 208090555

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;

namespace dotNet5781_01__6382_0555
{
    enum Actions { Exit = 'e', InsertBus = 'i', ChooseBusForRide = 'c', RefuelOrCare = 'r', ShowMilageFromCare = 's' }
    class Program
    {
        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>();
            Actions act;
            do
            {
                // object temp;
                Console.WriteLine("Choose your action (e,i,c,r,s)");
                if (!GetAction(out act))
                {
                    Console.Error.WriteLine("ERROR");
                    act = Actions.InsertBus;
                    continue;
                }
                // act = (Actions)temp;

                switch (act)
                {
                    case Actions.InsertBus:
                        InsertBus(buses);
                        break;
                    case Actions.ChooseBusForRide:
                        ChooseBus(buses);
                        break;
                    case Actions.RefuelOrCare:
                        RefuelOrCare(buses);
                        break;
                    case Actions.ShowMilageFromCare:
                        PrintBusesList(buses);
                        break;
                    case Actions.Exit:
                        break;
                }
            } while (act != Actions.Exit);

        }
        /// <summary>
        /// Inserting a bus to the buses list
        /// </summary>
        /// <param name="buses">The buses list</param>
        private static void InsertBus(List<Bus> buses)
        {
            DateTime dt;
            int license;
            Console.WriteLine("Input date(format dd/mm/yyyy):");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.Error.WriteLine("ERROR");
                return;
            }

            Console.WriteLine("Input license number:");
            if (!int.TryParse(Console.ReadLine(), out license))
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
            if (!IsUnique(buses,license)) {
                Console.Error.WriteLine("The bus alredy exists.");
                return;
            }

            if (dt.Year < 2018 && license < Math.Pow(10, 7) && license >= Math.Pow(10, 6) ||
                dt.Year>=2018&& license < Math.Pow(10, 8) && license >= Math.Pow(10, 7))
            {
                buses.Add(new Bus(license, dt));
            }
            else
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
        }
        /// <summary>
        /// Choosing a bus for a drivr and executing the drive
        /// </summary>
        /// <param name="buses">The buses list</param>
        private static void ChooseBus(List<Bus> buses)
        {
            int licenseNum;
            Console.WriteLine("Insert license number:");
            if (!int.TryParse(Console.ReadLine(), out licenseNum) || !CheckLicenseNumValid(licenseNum))
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
            Bus bus = null;
            foreach (Bus b in buses)
            {
                if (b.LicenseNumber == licenseNum)
                {
                    bus = b;
                }
            }
            if (bus != null)
            {
                Random rand = new Random(DateTime.Now.Millisecond);
                int distance = rand.Next(1, 120);
                if (bus.CanDrive(distance))
                {
                    bus.Drive(distance);
                }
                else
                {
                    Console.Error.WriteLine("The bus can't drive!");
                }
            }
            else
            {
                Console.Error.WriteLine("Bus not found!");
            }
        }
        /// <summary>
        /// Refuling or taking care of a bus
        /// </summary>
        /// <param name="buses">The buses list</param>
        private static void RefuelOrCare(List<Bus> buses)
        {
            int licenseNum;
            Console.WriteLine("Insert license number:");
            if (!int.TryParse(Console.ReadLine(), out licenseNum) || !CheckLicenseNumValid(licenseNum))
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
            Bus bus = null;
            foreach (Bus b in buses)
            {
                if (b.LicenseNumber == licenseNum)
                {
                    bus = b;
                }
            }
            if (bus == null)
            {
                Console.Error.WriteLine("Bus not found!");
                return;
            }
            char ch;
            Console.WriteLine("Refuel or Care (r/c):");
            if (!char.TryParse(Console.ReadLine().ToLower(), out ch))
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
            if (ch == 'r')
            {
                bus.Refuel();
            }
            else if (ch == 'c')
            {
                bus.TakeCare();
            }
            else
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
        }
        /// <summary>
        /// printing the buses list and how many the've drived after the last care
        /// </summary>
        /// <param name="buses">The buses list</param>
        private static void PrintBusesList(List<Bus> buses)
        {
            Console.WriteLine($"{"License number".PadRight(16,' ')}|{"Milage".PadRight(16,' ')}");
            foreach (Bus b in buses)
            {
                Console.WriteLine($"{FormatLicense(b.LicenseNumber).PadRight(16,' ')}|{b.MileageAfterCare.ToString().PadRight(16,' ')}");
            }
        }
        /// <summary>
        /// Checking if the given license number is valid
        /// </summary>
        /// <param name="licenseNum">Te license number to check</param>
        /// <returns></returns>
        private static bool CheckLicenseNumValid(int licenseNum)
        {
            return licenseNum >= Math.Pow(10, 6) && licenseNum < Math.Pow(10, 8);
        }
        /// <summary>
        /// Reading Action enum from the user by character
        /// </summary>
        /// <param name="act">The results will be here</param>
        /// <returns>Did the user inserted enum character?</returns>
        private static bool GetAction(out Actions act)
        {
            char action;
            if (!char.TryParse(Console.ReadLine().ToLower(), out action))
            {
                act = Actions.InsertBus;
                return false;
            }

            if (!Enum.IsDefined(typeof(Actions), (int)action))
            {
                act = Actions.InsertBus;
                return false;
            }
            act = (Actions)action;
            return true;
        }
        /// <summary>
        /// Checking if the given license number exists in the given bus list
        /// </summary>
        /// <param name="buses">the bus list</param>
        /// <param name="licenseNumber">the number to check</param>
        /// <returns>false if the number exists, else true</returns>
        private static bool IsUnique(List<Bus> buses, int licenseNumber) {
            foreach (Bus b in buses) {
                if (b.LicenseNumber == licenseNumber) {
                    return false;
                }
            }
            return true;

        }
        /// <summary>
        /// firmatting the license number to a license number format
        /// </summary>
        /// <param name="license">The unformatted license number</param>
        /// <returns>The formatted license number</returns>
        private static string FormatLicense(int license)
        {
            string asString = license.ToString();
            if (asString.Length == 8)
            {
                return asString.Substring(0, 3) + '-' + asString.Substring(3, 2) + '-' + asString.Substring(5);
            }
            return asString.Substring(0, 2) + '-' + asString.Substring(2, 3) + '-' + asString.Substring(5);
        }
    }
}


