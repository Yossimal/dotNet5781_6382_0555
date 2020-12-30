using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALAPI;
using DALAPI.DAO;

namespace BL
{
    internal class BL : IBL
    {
        #region Singelton
        private static readonly BL _instance = new BL();
        public static BL Instance => _instance;
        private BL() { }
        #endregion Singelton

        #region Attributes
        IDAL dataAPI = DALFactory.API;
        #endregion Attributes
        #region Implementation
        public int CheckUserName(BOUser user)
        {
            IEnumerable<DAOUser> response = dataAPI.Where<DAOUser>(usr => user.UserName == usr.UserName && user.Password == usr.Password);
            if (response.Count() == 0) {
                return -1;
            }
            return response.First().Id;
        }
        #endregion Implementation

    }
}
