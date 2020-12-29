using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOStation
    {
        const bool IsRunningId = false;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        public int Code
        {
            get => this.Id;
            set => this.Id = value;
        }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
