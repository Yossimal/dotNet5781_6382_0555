using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Models
{
    class RegisterModel
    {
        /// <summary>
        /// the user data
        /// </summary>
        public UserModel User { get; set; }
        /// <summary>
        /// password for registering managers
        /// </summary>
        public string ManagerCode { get; set; }
    }
}

