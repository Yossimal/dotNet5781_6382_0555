using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOBusOnTrip:DAOBasic
    {
        public int BusId { get; set; }
        public int LineId { get; set; }
        public TimeSpan PlannedTakeOf { get; set; }
        public TimeSpan ActualTakeOf { get; set; }
        public int PrevStation { get; set; }
        public TimeSpan PrevStationAt { get; set; }
        public TimeSpan NextStationAt { get; set; }
    }
}
