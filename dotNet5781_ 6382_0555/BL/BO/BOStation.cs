using DALAPI.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class BOStation
    {
        /// <summary>
        /// the station code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// the station name
        /// </summary>
        public string Name { get; set; }
        internal BOStation(DAOStation station)
        {
            this.Code = station.Code;
            this.Name = station.Name;
        }
        public BOStation() { }
    }
}
