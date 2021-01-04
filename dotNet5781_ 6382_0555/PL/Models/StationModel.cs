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
        public string Code { get; set; }
        public string Name { get; set; }

        public StationModel() { }
        public StationModel(BOStation station) {
            this.Name = station.Name;
            this.Code = station.Code.ToString();
        }
    }
}
