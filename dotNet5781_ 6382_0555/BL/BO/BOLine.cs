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
        /// <summary>
        /// the line area
        /// </summary>
        private Area _area;
        #endregion
        #region properties
        /// <summary>
        /// the line id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the line number
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// the line path
        /// </summary>
        public IEnumerable<BOStation> Path { get; set; }
        /// <summary>
        /// the line area as string (the area is DL enum so the BL area as enum is internal)
        /// </summary>
        public string Area { 
            get=> _area.ToString();
            set {
                if (Enum.TryParse<Area>(value, out _))
                {
                    _area = (Area)Enum.Parse(typeof(Area), value);
                }
            }
        }
        /// <summary>
        /// the line area
        /// </summary>
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
