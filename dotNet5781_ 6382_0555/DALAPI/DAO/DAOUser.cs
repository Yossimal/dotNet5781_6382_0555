using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DALAPI.DAO
{
    public class DAOUser
    {
        public bool IsRunningId => true;
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// The user name for login.
        /// We don't want the user name to be the primary key because we want to give him the option to change it in the future
        /// </summary>
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }


    }
}
