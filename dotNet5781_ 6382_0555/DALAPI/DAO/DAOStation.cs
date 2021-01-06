using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
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
