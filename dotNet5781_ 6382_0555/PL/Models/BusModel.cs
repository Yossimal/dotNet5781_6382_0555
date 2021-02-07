using BL.BO;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.Models
{
    /// <summary>
    /// data of bus
    /// </summary>
    class BusModel
    {
        /// <summary>
        /// license number (primary key)
        /// </summary>
        public string LicenseNumber { get; set; }
        /// <summary>
        /// is the bus available
        /// </summary>
        public bool IsAvailable { get; set; }
        /// <summary>
        /// the message to send on the availability fuild
        /// </summary>
        public string AvailabilityMessage { get; set; }
        /// <summary>
        /// the remaining fuel of the bus
        /// </summary>
        public double FuelRemain { get; set; }
        /// <summary>
        /// total milage
        /// </summary>
        public double MilageCounter { get; set; }
        /// <summary>
        /// the bus next care date
        /// </summary>
        public DateTime NextCareDate { get; set; }
        //In PL the status is string because we need to show it without using logic
        /// <summary>
        /// the bus status
        /// </summary>
        public string BusStatus { get; set; }
        /// <summary>
        /// the bus license date
        /// </summary>
        public DateTime LicenseDate { get; set; }
        /// <summary>
        /// last care date of the bus
        /// </summary>
        public DateTime LastCareDate { get; set; }
        public BusModel() { }
        /// <summary>
        /// construct BusModel from BOBus
        /// </summary>
        /// <param name="bus"></param>
        public BusModel(BOBus bus)
        {
            this.AvailabilityMessage = bus.AvailabilityMassage;
            this.BusStatus = bus.StatusStr;
            this.FuelRemain = bus.FuelRemain;
            this.IsAvailable = bus.IsAvailable;
            this.LicenseDate = bus.LicenseDate;
            this.LicenseNumber = FormatLicense(bus.LicenseNumber);
            this.MilageCounter = bus.MileageCounter;
            this.LastCareDate = bus.LastCareDate;
            this.NextCareDate = bus.NextCareDate;
        }
        /// <summary>
        /// convert BusModel to BOBus
        /// </summary>
        /// <returns>the BOBus</returns>
        public BOBus ToBO()
        {
            return new BOBus
            {
                FuelRemain = this.FuelRemain,
                LicenseDate = this.LicenseDate,
                LicenseNumber = ReFormatLicense(this.LicenseNumber),
                MileageCounter = this.MilageCounter,
                LastCareDate = this.LastCareDate
            };
        }
        public static Visibility ControlsVisibility { get; set; }

        /// <summary>
        /// Checks licens's size and return it with an appropriate format
        /// Validy had been done in BO
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public static string FormatLicense(int license)
        {
            string asString = license.ToString();
            if (asString.Length == 8)
            {
                return asString.Substring(0, 3) + '-' + asString.Substring(3, 2) + '-' + asString.Substring(5);
            }
            return asString.Substring(0, 2) + '-' + asString.Substring(2, 3) + '-' + asString.Substring(5);
        }
        /// <summary>
        /// ReFormat PL licens in order to return it to BO
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public static int ReFormatLicense(string license)
        {
            return int.Parse(license.Replace("-", ""));
        }
    }
}
