using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class MainViewModel : Conductor<object>
    {
        private static Dictionary<string, Type> pages = new Dictionary<string, Type>();
        private string _currentPage;
        private readonly Stack<string> _navigation=new Stack<string>();

        public bool CanBack
        {
            get => _navigation.Count != 0;
        }
        public MainViewModel()
        {
            InitializePages();
            LoadPage("Login");
        }
        
        private void InitializePages()
        {
            pages.Add("Login", typeof(LoginViewModel));
            pages.Add("Register", typeof(RegisterViewModel));
            pages.Add("ManagerHome", typeof(ManagerHomeViewModel));
        }
        public void LoadPage(string toLoad)
        {
            if (_currentPage != null) {
                _navigation.Push(_currentPage);
                NotifyOfPropertyChange(() => CanBack);
            }
            _currentPage = toLoad;
            object pageToLoad = Activator.CreateInstance(pages[toLoad], new object[1] { this });
            ActivateItem(pageToLoad);
        }

        public void Back() {
            _currentPage = null;//set the current page to null so the LoadPage wont set the last page to the current page
            LoadPage(_navigation.Pop());
            NotifyOfPropertyChange(() => CanBack);
        }
    }
}
