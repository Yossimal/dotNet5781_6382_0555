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
                object temp;
                Console.WriteLine("Choose your action (e,i,c,r,s)");
                if (!Enum.TryParse(typeof(Actions),Console.ReadLine().ToLower(), out temp))
                {
                    Console.Error.WriteLine("ERROR");
                    act = Actions.InsertBus;
                    continue;
                }
                act = (Actions)temp;
 
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
        private static void InsertBus(List<Bus> buses)
        {
            DateTime dt;
            int license;
            Console.WriteLine("Input date:");
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

            if (dt.Year < 2018 && license < Math.Pow(10, 7) && license >= Math.Pow(10, 6) ||
                license < Math.Pow(10, 8) && license >= Math.Pow(10, 7))
            {
                buses.Add(new Bus(license, dt));
            }
            else
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
        }
        private static void ChooseBus(List<Bus> buses)
        {
            int licenseNum;
            Console.WriteLine("Insert license number:");
            if (!int.TryParse(Console.ReadLine(), out licenseNum) || !CheckLicenseNumValid(licenseNum))
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
            Bus bus=null;
            foreach (Bus b in buses) {
                if (b.LicenseNumber == licenseNum)
                {
                    bus = b;
                }            
            }
            if (bus!=null)
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
            if (!char.TryParse(Console.ReadLine().ToLower(),out ch))
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
            if (ch=='r')
            {
                bus.Refuel();
            }
            else if (ch=='c')
            {
                bus.TakeCare();
            }
            else
            {
                Console.Error.WriteLine("ERROR");
                return;
            }
        }
        private static void PrintBusesList(List<Bus> buses)
        {
            Console.WriteLine("License number\t|\tMilage");
            foreach (Bus b in buses)
            {
                Console.WriteLine($"{b.LicenseNumber}\t|\t{b.MileageAfterCare}");
            }
        }
        private static bool CheckLicenseNumValid (int licenseNum)
        {
            return licenseNum >= Math.Pow(10, 6) && licenseNum < Math.Pow(10, 8);
        }
    }
}


