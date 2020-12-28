using System;
using System.Collections.Generic;
using System.Text;


namespace DALAPI.DAO
{
    public class DAOLine:DAOBasic
    {
        public int Code { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public Area area { get;set; }

    }
}
