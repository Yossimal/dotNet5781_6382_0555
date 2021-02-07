using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace PL.Models
{
    public class UserModel
    {
        /// <summary>
        /// the user id
        /// </summary>
        public readonly int Id;
        /// <summary>
        /// the user name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// the user password
        /// </summary>
        public string Password { get; set; }
        public bool IsManager { get; set; }

        public UserModel(BOUser user)
        {
            this.UserName = user.UserName;
            this.Password = user.Password;
            this.IsManager = user.IsManager;
            this.Id = user.Id;
        }
        public UserModel() { }
    }
}


