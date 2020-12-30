using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsManager { get; set; }
    }
}
