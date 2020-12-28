using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOTrip:DAOBasic
    {
        public int UserId { get; set; }
        public int LineId { get; set; }
        public int InStationId { get; set; }
        public TimeSpan InAt { get; set; }
        public int OutStationId { get; set; }
        public TimeSpan OutTime { get; set; }
    }
}