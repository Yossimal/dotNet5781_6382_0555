using System;
using System.Collections.Generic;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOStation:DAOBasic
    {
        public override bool IsRunningId => false;

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
