using System;
using System.Collections.Generic;
using System.Text;


namespace DALAPI.DAO
{
    /// <summary>
    /// Line entity - represents basic data of line
    /// line represents by code number, first and last stations, and area
    /// </summary>
    public class DAOLine
    {
        #region BackgroundData
        //The Code cant be the id because there us many lines with the same number
        public bool IsRunningId => true;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        #endregion
        #region RealData
        public int Code { get; set; }
        public int FirstStationId { get; set; }
        public int LastStationId { get; set; }
        public Area Area { get;set; }

        #endregion

    }
}
