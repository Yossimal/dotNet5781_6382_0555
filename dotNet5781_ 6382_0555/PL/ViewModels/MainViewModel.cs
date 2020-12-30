using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class MainViewModel:Conductor<object>
    {
        Dictionary<string, Type> pages = new Dictionary<string, Type>();

        public MainViewModel() {
            InitializePages();
            LoadPage("Login");
        }
        private void InitializePages()
        {
            pages.Add("Login", typeof(LoginViewModel));
        }
        public void LoadPage(string toLoad) {
            object pageToLoad = Activator.CreateInstance(pages[toLoad]);
            ActivateItem(pageToLoad);
        }
    }
}
