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
        public int Code { get; set; }
        public string Name { get; set; }
        public BOStation(DAOStation station) { }
        public BOStation() { }
    }
}
