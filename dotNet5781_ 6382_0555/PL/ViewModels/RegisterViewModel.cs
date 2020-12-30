using BL;
using BL.BO;
using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    class RegisterViewModel : Screen
    {
        private IBL logic = BLFactory.API;
        private MainViewModel mainViewModel;
        private UserModel _user = new UserModel();
        private Visibility _managerCodeVisibility = Visibility.Hidden;
        private string _managerCode;

        public RegisterViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }


        public string UserName
        {
            get => User.UserName;
            set
            {
                User.UserName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => User);
            }
        }
        public UserModel User
        {
            get { return _user; }
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => IsManager);
            }
        }
        public string Password
        {
            get => User.Password;
            set
            {
                User.Password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => User);
            }
        }
        public bool IsManager
        {
            get => User.IsManager;
            set
            {
                User.IsManager = value;
                _managerCodeVisibility = User.IsManager ? Visibility.Visible : Visibility.Hidden;
                NotifyOfPropertyChange(() => IsManager);
                NotifyOfPropertyChange(() => User);
                NotifyOfPropertyChange(() => ManagerCodeVisibility);
            }
        }
        public Visibility ManagerCodeVisibility
        {
            get => _managerCodeVisibility;
        }
        public string ManagerCode
        {
            get => _managerCode;
            set
            {
                _managerCode = value;
                NotifyOfPropertyChange(ManagerCode);
            }
        }
        public void Register()
        {
            BORegister register = new BORegister
            {
                User = new BOUser
                {
                    UserName = User.UserName,
                    Password = User.Password,
                    IsManager = User.IsManager
                },
                ManagerCode = ManagerCode
            };
            try
            {
                int id = logic.Register(register);
                if (id != -1)
                {
                    MessageBox.Show($"Success register\n id is {id}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
