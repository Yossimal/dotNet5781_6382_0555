using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00__6382_0555
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome0555();

            Console.ReadKey();
        }

        private static void Welcome0555()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine("{0} , welcome to my first console application", name);
        }

        static partial void Welcome6382();
    }
}

