using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    /// <summary>
    /// for adding new station
    /// </summary>
    class AddStationModel
    {
        private string _code;
        private string _longitude;
        private string _latitude;
        /// <summary>
        /// constructor (set all teh strings to "")
        /// </summary>
        public AddStationModel()
        {
            _code = "";
            _longitude = "";
            _latitude = "";
            Name = "";
        }
        /// <summary>
        /// the station code
        /// </summary>
        public string Code
        {
            get => _code.ToString();
            set
            {
                if (int.TryParse(value + "0", out _) || value == "")
                {
                    _code = value;
                }
            }
        }
        /// <summary>
        /// the station name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// the station longitude
        /// </summary>
        public string Longitude
        {
            get => _longitude;
            set
            {
                if (double.TryParse(value + "0", out _) || value == "")
                {
                    _longitude = value;
                }
            }
        }
        /// <summary>
        /// the station latitude
        /// </summary>
        public string Latitude
        {
            get => _latitude;
            set
            {
                if (double.TryParse(value + "0", out _) || value == "")
                {
                    _latitude = value;
                }
            }
        }


    }
}
