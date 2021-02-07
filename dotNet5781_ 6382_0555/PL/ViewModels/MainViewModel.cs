using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    /// <summary>
    /// Pages management class
    /// 'Main' uses Caliburn.Micro to activate and deactivate pages
    /// 'Main' inherits from 'Conductor', Caliburn.Micro's class
    /// </summary>
    class MainViewModel : Conductor<object>
    {
        /// <summary>
        /// all the pages in the program
        /// </summary>
        private static Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        /// <summary>
        /// thee current open page name
        /// </summary>
        private string _currentPageName;
        /// <summary>
        /// the last opened pages (for the back button)
        /// </summary>
        private readonly Stack<string> _navigation = new Stack<string>();
        /// <summary>
        /// the title of the program (the page name)
        /// </summary>
        private string _title = "";
        /// <summary>
        /// the last opened screen
        /// </summary>
        private object _lastScreen;
        /// <summary>
        /// set the title to the opn page title
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }
        /// <summary>
        /// check if we can go back
        /// </summary>
        public bool CanBack
        {
            get => _navigation.Count != 0;
        }
        /// <summary>
        /// Constructor
        /// Initialize pages and loads "Login" page
        /// </summary>
        public MainViewModel()
        {
            InitializePages();
            LoadPage("Login");
        }
        /// <summary>
        /// initialize all the pages in the program
        /// </summary>
        private void InitializePages()
        {
            _pages.Add("AddBus", typeof(AddBusViewModel));
            _pages.Add("AddLine", typeof(AddLineViewModel));
            _pages.Add("AddStation", typeof(AddStationViewModel));
            _pages.Add("Login", typeof(LoginViewModel));
            _pages.Add("ManagerHome", typeof(ManagerHomeViewModel));
            _pages.Add("Register", typeof(RegisterViewModel));
            _pages.Add("Simulation", typeof(SimulationViewModel));
            _pages.Add("ShowBusData", typeof(ShowBusDataViewModel));
            _pages.Add("ShowBuses", typeof(ShowBusesViewModel));
            _pages.Add("ShowLineData", typeof(ShowLineDataViewModel));
            _pages.Add("ShowLineStationData", typeof(ShowLineStationViewModel));
            _pages.Add("ShowLines", typeof(ShowLinesViewModel));
            _pages.Add("ShowStations", typeof(ShowStationsViewModel));
            _pages.Add("ShowStationData", typeof(ShowStationDataViewModel));
            
        }

        /// <summary>
        /// Loads linked page
        /// 'Linked' means that there are pages before this page
        /// that need to be handle after this page
        /// </summary>
        /// <param name="toLoad">th page name</param>
        /// <param name="parameters">prameters for the page constructor</param>
        /// <seealso cref="LoadPageNoBack(string, object[])"/>
        public void LoadPage(string toLoad, params object[] parameters)
        {
            //if there is current page -> push it to the stack
            if (_currentPageName != null)
            {
                _navigation.Push(_currentPageName);
                NotifyOfPropertyChange(() => CanBack);
            }
            //set the page to load to be the current page
            _currentPageName = toLoad;
            //generate the page and load it
            List<object> constructorParams = new List<object>();
            constructorParams.Add(this);
            constructorParams.AddRange(parameters);
            object pageToLoad = Activator.CreateInstance(_pages[toLoad], constructorParams.ToArray());
            //close the last screen
            if (_lastScreen != null)
            {
                DeactivateItem(_lastScreen, true);
            }
            //set the new page to be the last screen
            _lastScreen = pageToLoad;
            ActivateItem(pageToLoad);
            Title = _currentPageName;
        }
        /// <summary>
        /// Loads a single page,
        /// 'Single' mean's that all pages before this page finished their targets
        /// and closed
        /// </summary>
        /// <param name="toLoad">the page to load</param>
        /// <param name="parameters">constructo parameters</param>
        /// <seealso cref="LoadPage(string, object[])"/>
        public void LoadPageNoBack(string toLoad, params object[] parameters)
        {
            _currentPageName = toLoad;
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
            Title = _currentPageName;
        }
        /// <summary>
        /// go back one page
        /// </summary>
        public void Back()
        {
            _currentPageName = null;//set the current page to null so the LoadPage wont set the last page to the current page
            LoadPage(_navigation.Pop());
            NotifyOfPropertyChange(() => CanBack);
        }
    }
}
