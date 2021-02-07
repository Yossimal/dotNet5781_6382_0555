using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace PL.Models
{
    class LineModel
    {
        /// <summary>
        /// the line Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the line code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// all the stations in the line
        /// </summary>
        public BindableCollection<StationModel> Stations { get; set; }
        //In PL the area is string because we need to show it without using logic
        /// <summary>
        /// the line area
        /// </summary>
        public string Area { get; set; }
    }
}
