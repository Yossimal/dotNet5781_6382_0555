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
        private string _title="";
        private object _lastScreen;
        public string Title
        {
            get => _title;
            set {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }
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
            pages.Add("ShowBuses", typeof(ShowBusesViewModel));
            pages.Add("ShowStations", typeof(ShowStationsViewModel));
            pages.Add("ShowLines", typeof(ShowLinesViewModel));
            pages.Add("AddBus", typeof(AddBusViewModel));
            pages.Add("ShowBusData", typeof(ShowBusDataViewModel));
        }
        public void LoadPage(string toLoad,params object[] parameters)
        {
            if (_currentPage != null) {
                _navigation.Push(_currentPage);
                NotifyOfPropertyChange(() => CanBack);
            }
            _currentPage = toLoad;
            List<object> constructorParams = new List<object>();
            constructorParams.Add(this);
            constructorParams.AddRange(parameters);
            object pageToLoad = Activator.CreateInstance(pages[toLoad], constructorParams.ToArray());
            if (_lastScreen != null) {
                DeactivateItem(_lastScreen, true);
            }
            _lastScreen = pageToLoad;
            ActivateItem(pageToLoad);
            Title = _currentPage;
        }
        public void Back() {
            _currentPage = null;//set the current page to null so the LoadPage wont set the last page to the current page
            LoadPage(_navigation.Pop());
            NotifyOfPropertyChange(() => CanBack);
        }
    }
}
