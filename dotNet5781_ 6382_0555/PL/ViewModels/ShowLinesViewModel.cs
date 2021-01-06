using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class ShowLinesViewModel:Screen
    {
        #region private fuilds
        private MainViewModel _mainViewModel;
        private LineModel _line;
        #endregion

        #region constructors
        public ShowLinesViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        #endregion
        #region properties
        public LineModel Line {
            get => _line;
            set {
                _line = value;
                NotifyOfPropertyChange(() => Line);
            }
        }
        #endregion
    }
}
