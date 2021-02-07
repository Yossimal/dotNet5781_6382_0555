using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class StationModel
    {
        /// <summary>
        /// the station code (primary key)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// the station name
        /// </summary>
        public string Name { get; set; }

        public StationModel() { }
        /// <summary>
        /// generate station model from BOStation
        /// </summary>
        /// <param name="station"></param>
        public StationModel(BOStation station) {
            this.Name = station.Name;
            this.Code = station.Code.ToString();
        }
    }
}
