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
        const string MANAGER_CODE = "123!!!";
        #endregion Attributes
        #region Implementation
        public int CheckUserName(BOUser user)
        {
            IEnumerable<DAOUser> response = dataAPI.Where<DAOUser>(usr => user.UserName == usr.UserName && user.Password == usr.Password);
            if (response.Count() == 0)
            {
                return -1;
            }
            return response.First().Id;
        }

        public int Register(BORegister register)
        {
            try
            {
                if (register.User.UserName.Length<=3|| register.User.UserName.Length>=16)
                {
                    throw new InvalidOperationException("User name length must be between 3 and 16 characters");
                }
                if (register.User.Password.Length <= 3 || register.User.Password.Length >= 16)
                {
                    throw new InvalidOperationException("Password length must be between 3 and 16 characters");
                }
                DAOUser toAdd = new DAOUser
                {
                    UserName = register.User.UserName,
                    Password = register.User.Password,
                    IsAdmin = register.User.IsManager
                };

                if (register.User.IsManager && register.ManagerCode != MANAGER_CODE)
                {
                    throw new InvalidOperationException("Bad manager code! Can't register");
                }
                if (dataAPI.Where<DAOUser>(user => user.UserName == toAdd.UserName).Count() > 0)
                {
                    throw new InvalidOperationException("User name already exists!");
                }
                return dataAPI.Add(toAdd);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("There was problem to write the data",ex);
            }
        }
        #endregion Implementation

    }
}
