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
        public int Id { get; set; }
        public int Code { get; set; }
        BindableCollection<StationModel> Stations { get; set; }
        //In PL the area is string because we need to show it without using logic
        public string Area { get; set; }
    }
}
