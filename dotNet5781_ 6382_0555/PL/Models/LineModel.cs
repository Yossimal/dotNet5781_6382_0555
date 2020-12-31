using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class LineModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public LineStationModel FirstStation { get; set; }
        public LineStationModel LastStation { get; set; }
        //In PL the status is string because we need to show it without using logic
        public string Area { get; set; }
    }
}
