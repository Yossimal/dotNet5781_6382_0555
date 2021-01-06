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
        private static Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        private string _currentPage;
        private readonly Stack<string> _navigation = new Stack<string>();
        private string _title = "";
        private object _lastScreen;
        public string Title
        {
            get => _title;
            set
            {
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
            _pages.Add("AddBus", typeof(AddBusViewModel));
            _pages.Add("AddStation", typeof(AddStationViewModel));
            _pages.Add("Login", typeof(LoginViewModel));
            _pages.Add("ManagerHome", typeof(ManagerHomeViewModel));
            _pages.Add("Register", typeof(RegisterViewModel));
            _pages.Add("ShowBusData", typeof(ShowBusDataViewModel));
            _pages.Add("ShowBuses", typeof(ShowBusesViewModel));
            _pages.Add("ShowLines", typeof(ShowLinesViewModel));
            _pages.Add("ShowStations", typeof(ShowStationsViewModel));
            _pages.Add("ShowStationData", typeof(ShowStationDataViewModel));
        }
        public void LoadPage(string toLoad, params object[] parameters)
        {
            if (_currentPage != null)
            {
                _navigation.Push(_currentPage);
                NotifyOfPropertyChange(() => CanBack);
            }
            _currentPage = toLoad;
            List<object> constructorParams = new List<object>();
            constructorParams.Add(this);
            constructorParams.AddRange(parameters);
            object pageToLoad = Activator.CreateInstance(_pages[toLoad], constructorParams.ToArray());
            if (_lastScreen != null)
            {
                DeactivateItem(_lastScreen, true);
            }
            _lastScreen = pageToLoad;
            ActivateItem(pageToLoad);
            Title = _currentPage;
        }
        public void LoadPageNoBack(string toLoad, params object[] parameters)
        {
            _currentPage = toLoad;
            List<object> constructorParams = new List<object>();
            constructorParams.Add(this);
            constructorParams.AddRange(parameters);
            object pageToLoad = Activator.CreateInstance(_pages[toLoad], constructorParams.ToArray());
            if (_lastScreen != null)
            {
                DeactivateItem(_lastScreen, true);
            }
            _lastScreen = pageToLoad;
            ActivateItem(pageToLoad);
            Title = _currentPage;
        }
        public void Back()
        {
            _currentPage = null;//set the current page to null so the LoadPage wont set the last page to the current page
            LoadPage(_navigation.Pop());
            NotifyOfPropertyChange(() => CanBack);
        }
    }
}
