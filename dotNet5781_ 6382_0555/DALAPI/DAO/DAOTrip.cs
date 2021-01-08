using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOTrip
    {
        #region background data
        internal bool IsRunningId => true;
        internal bool IsDeleted { get; set; }
        public int Id;
        #endregion
        #region real data
        public int UserId { get; set; }
        public int LineId { get; set; }
        public int InStationId { get; set; }
        public TimeSpan InAt { get; set; }
        public int OutStationId { get; set; }
        public TimeSpan OutTime { get; set; }
        #endregion
    }
}