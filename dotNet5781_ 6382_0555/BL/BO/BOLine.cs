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
        public IEnumerable<BOStation> Path { get; set; }
        public string Area { 
            get=> _area.ToString();
            set {
                if (Enum.TryParse<Area>(value, out _))
                {
                    _area = (Area)Enum.Parse(typeof(Area), value);
                }
            }
        }
        internal Area EnumArea {
            get => _area;
            set => _area = value;
        }
        
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
