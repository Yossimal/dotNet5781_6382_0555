using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOLineTrip:DAOBasic
    {
        public int LineId { get; set; }
        public TimeSpan StartAt { get; set; }
        public TimeSpan? Frequency { get; set; }
        public TimeSpan? FinishAt { get; set; }
    }
}
