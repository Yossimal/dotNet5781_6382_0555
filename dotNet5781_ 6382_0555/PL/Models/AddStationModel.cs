using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class AddStationModel
    {
        private string _code;
        private string _longitude;
        private string _latitude;

        public AddStationModel() {
            _code = "";
            _longitude = "";
            _latitude = "";
            Name = "";
        }
        public string Code {
            get=>_code.ToString();
            set {               
                if (int.TryParse(value+"0", out _)||value=="") {
                    _code = value;
                }
            }
        }
        public string Name { get; set; }
        public string Longitude { get=>_longitude;
            set {
                if (double.TryParse(value + "0", out _) || value == "") {
                    _longitude = value;
                }
            } 
        }
        public string Latitude { 
            get=>_latitude;
            set {
                if (double.TryParse(value + "0", out _) || value == "") {
                    _latitude = value;
                }
            }
        }


    }
}
