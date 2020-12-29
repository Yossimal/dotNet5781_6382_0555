using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOLineStation
    {
        const bool IsRunningId = true;
        public bool IsDeleted { get; set; }
        public int Id;
        public int LineId { get; set; }
        public int StationId { get; set; }
        public int Index { get; set; }
        public int NextStationId { get; set; }
        public int PrevStationId { get; set; }
    }
}
