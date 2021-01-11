using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOAdjacentStations
    {

        /// <summary>
        /// The station that the bus will come from
        /// We need to get the distance and the time for both sides because 
        /// sometimes the road in one side is  different from the other side.
        /// </summary>
        #region BackgroundData
        public bool IsRunningId => true;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        #endregion BackgroundData
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
    }
}
