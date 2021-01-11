using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    /// <summary>
    /// Station entity - represents basic data of a station
    /// station represents by code, name, longitude and latitude
    /// </summary>
    public class DAOStation
    {
        #region BackgroundData
        public bool IsRunningId => false;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        #endregion
        #region RealData
        public int Code
        {
            get => this.Id;
            set => this.Id = value;
        }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        #endregion
    }
}
