using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class ManagerHomeViewModel:Screen
    {
        MainViewModel _mainViewModel;

        public ManagerHomeViewModel(MainViewModel mainViewModel) {
            _mainViewModel = mainViewModel;
        }
    }
}
