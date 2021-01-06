using DALAPI;
using DALAPI.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class BOLine
    {
        #region private fuilds
        private Area _area;
        private IDAL dataAPI = DALFactory.API;
        #endregion
        #region properties
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public List<BOStation> Path { get; set; }
        public string Area => _area.ToString();
        #endregion
        #region constructors
        public BOLine() { }
        internal BOLine(DAOLine line)
        {
            this.Id = line.Id;
            this.LineNumber = line.Code;
            this._area = line.Area;
            this.Path = null;
        }
        #endregion


    }
}
