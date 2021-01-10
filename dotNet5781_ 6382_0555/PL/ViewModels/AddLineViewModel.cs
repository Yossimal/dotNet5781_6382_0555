using BL;
using Caliburn.Micro;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    class AddLineViewModel : Screen
    {
        private MainViewModel _mainViewModel;
        private LineModel _lineToAdd;
        private BindableCollection<string> _areas;
        private IBL logic = BLFactory.API;
        private BindableCollection<StationModel> _path;

        public AddLineViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Areas = new BindableCollection<string>(logic.GetAllAreas());
            _lineToAdd = new LineModel {
                Area = Areas[0],
                Code = 0,
                Stations = new BindableCollection<StationModel>()
            };
            Path = new BindableCollection<StationModel>();
        }

        public LineModel LineToAdd
        {
            get => _lineToAdd;
            set
            {
                _lineToAdd = value;
                NotifyOfPropertyChange(() => LineToAdd);
                NotifyOfPropertyChange(() => LineNumber);
            }
        }
        public string LineNumber
        {
            get => _lineToAdd.Code.ToString();
            set
            {
                if (int.TryParse(value, out _))
                {
                    _lineToAdd.Code = int.Parse(value);
                    NotifyOfPropertyChange(() => LineNumber);
                    NotifyOfPropertyChange(() => LineToAdd);
                    NotifyOfPropertyChange(() => SelectedArea);
                }
            }
        }
        public string SelectedArea
        {
            get => _lineToAdd.Area;
            set
            {
                if (Areas.Contains(value))
                {
                    _lineToAdd.Area = value;
                    NotifyOfPropertyChange(() => SelectedArea);
                    NotifyOfPropertyChange(() => LineToAdd);
                }
            }
        }
        public BindableCollection<string> Areas
        {
            get => _areas;
            set
            {
                _areas = value;
                NotifyOfPropertyChange(() => Areas);
            }
        }
        public BindableCollection<StationModel> Path
        {
            get => _path;
            set
            {
                _path = value;
                NotifyOfPropertyChange(() => Path);
            }
        }
       
        public bool CanClearText(string lineNumber) {
            return  int.Parse(lineNumber)<=0;    
        }
        public void ClearText(string lineNumber) {
            SelectedArea = Areas[0];
            lineNumber = "0";
            Path = new BindableCollection<StationModel>();
        }
        #region private methods
        static bool IsNullEmptyOrWhiteSpace(string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
        }
        #endregion
    }
}
