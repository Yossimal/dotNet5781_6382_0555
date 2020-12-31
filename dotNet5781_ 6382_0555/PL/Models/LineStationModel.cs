using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class LineStationModel
    {
        public int Id { get;set; }
        StationModel Station { get; set; }
        LineStationModel Next { get; set; }
        LineStationModel Prev { get; set; }
        double distanceFromPrev { get; set; }
        TimeSpan TimeFromPrev { get; set; }
    }
}
