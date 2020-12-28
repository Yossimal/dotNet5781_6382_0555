using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOLineStation:DAOBasic
    {
        public int LineId { get; set; }
        public int StationId { get; set; }
        public int Index { get; set; }
        public int NextStationId { get; set; }
        public int PrevStationId { get; set; }
    }
}
