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
    class BusModel
    {
        public string LicenseNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string AvailabilityMessage { get; set; }
        public double FuelRemain { get; set; }
        public double MilageCounter { get; set; }
        public DateTime NextCareDate { get; set; }
        //In PL the status is string because we need to show it without using logic
        public string BusStatus { get; set; }
        public DateTime LicenseDate { get; set; }
        public DateTime LastCareDate { get; set; }
        public BusModel() { }
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
        public BOBus ToBO() {
            return new BOBus
            {
                FuelRemain=this.FuelRemain,
                LicenseDate=this.LicenseDate,
                LicenseNumber= ReFormatLicense(this.LicenseNumber),
                MileageCounter=this.MilageCounter,
                LastCareDate=this.LastCareDate
            };
        }
        public static Visibility ControlsVisibility { get; set; }
        public static string FormatLicense(int license)
        {
            string asString = license.ToString();
            if (asString.Length == 8)
            {
                return asString.Substring(0, 3) + '-' + asString.Substring(3, 2) + '-' + asString.Substring(5);
            }
            return asString.Substring(0, 2) + '-' + asString.Substring(2, 3) + '-' + asString.Substring(5);
        }
        public static int ReFormatLicense(string license) {
            return int.Parse(license.Replace("-", ""));
        }
    }
}
