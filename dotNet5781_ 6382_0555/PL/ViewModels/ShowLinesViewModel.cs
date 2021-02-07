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
    class ShowLinesViewModel : Screen
    {
        #region private fuilds
        private MainViewModel _mainViewModel;
        private LineModel _selectedLine;
        private BindableCollection<LineModel> _lines;
        IBL logic = BLFactory.API;
        #endregion

        #region constructors
        public ShowLinesViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            IEnumerable<LineModel> allLines = logic.GetAllLines().Select(line => new LineModel
                                                                                    {
                                                                                        Id = line.Id,
                                                                                        Area = line.Area,
                                                                                        Code = line.LineNumber,
                                                                                        Stations = new BindableCollection<StationModel>(line.Path.Select(station => new StationModel(station)))
                                                                                    });
            Lines = new BindableCollection<LineModel>(allLines);
        }
        #endregion
        #region properties for Caliburn.Micro
        public LineModel SelectedLine
        {
            get => _selectedLine;
            set
            {
                _selectedLine = value;
                NotifyOfPropertyChange(() => SelectedLine);
            }
        }
        public BindableCollection<LineModel> Lines
        {
            get => _lines;
            set
            {
                _lines = value;
                NotifyOfPropertyChange(() => Lines);
            }
        }
        #endregion
        #region events
        public void AddLine()
        {
            _mainViewModel.LoadPage("AddLine");
        }
        public void ShowLineData()
        {
            if (SelectedLine == null)
            {
                return;
            }
            _mainViewModel.LoadPage("ShowLineData", SelectedLine);
        }

        #endregion
    }
}
