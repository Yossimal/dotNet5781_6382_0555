using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    /// <summary>
    /// Line's Station - represents a station in line
    /// any line has an ID
    /// </summary>
    public class DAOLineStation
    {
        #region Backgroun Data
        public bool IsRunningId => true;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        #endregion
        #region real data
        public int LineId { get; set; }
        public int StationId { get; set; }
        public int Index { get; set; }
        public int NextStationId { get; set; }
        public int PrevStationId { get; set; }
        #endregion
    }
}
