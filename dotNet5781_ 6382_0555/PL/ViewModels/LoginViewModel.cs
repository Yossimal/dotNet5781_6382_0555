using BL.BO;
using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using System.Windows;
using System.Security;
using System.Net;

namespace PL.ViewModels
{
    class LoginViewModel : Screen
    {
        IBL logic = BLFactory.API;
        private UserModel _user = new UserModel();
        private MainViewModel _mainViewModel;

        public LoginViewModel(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
        }

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
            }
        }

        public string UserName
        {
            get { return _user.UserName; }
            set
            {
                _user.UserName = value;
                NotifyOfPropertyChange(() => User);
                NotifyOfPropertyChange(() => UserName);
            }
        }
        public string Password
        {
            get => _user.Password;
            set
            {
                _user.Password = value;
                NotifyOfPropertyChange(() => User);
                NotifyOfPropertyChange(() => Password);
            }
        }

        //public bool CanClearText(string userName, string password)
        //{
        //    return !(String.IsNullOrWhiteSpace(userName) && String.IsNullOrWhiteSpace(password));
        //}

        //public void ClearText(string userName, string password)
        //{
        //    UserName = "";
        //    Password = "";
        //}

        public void Login(string userName, string password)
        {
            BOUser logicUser = new BOUser
            {
                UserName = userName,
                Password = password

            };
            try
            {
                logicUser = logic.CheckUserName(logicUser);
                NavigationHandler(new UserModel(logicUser));
            }
            catch (Exception ex)
            {
                if (ex is BadLoginDataException)
                {
                    MessageBox.Show("User name or password worng", "Can't login", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public bool CanLogin(string userName, string password)
        {
            return !(String.IsNullOrWhiteSpace(userName)
                || String.IsNullOrWhiteSpace(password));
        }

        public void Register()
        {
            _mainViewModel.LoadPage("Register");
        }

        private void NavigationHandler(UserModel user)
        {
            if (user.IsManager)
            {
                _mainViewModel.LoadPage("ManagerHome");
            }
            else
            {
                MessageBox.Show("Coming Soon", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
