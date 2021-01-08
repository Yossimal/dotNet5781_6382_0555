using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOLineStation
    {
        #region Backgroun Data
        internal bool IsRunningId => true;
        internal bool IsDeleted { get; set; }
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
