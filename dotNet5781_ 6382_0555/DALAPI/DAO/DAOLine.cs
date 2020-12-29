using System;
using System.Collections.Generic;
using System.Text;


namespace DALAPI.DAO
{
    public class DAOLine
    {
        const bool IsRunningId = true;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        public int Code { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public Area area { get;set; }

    }
}
