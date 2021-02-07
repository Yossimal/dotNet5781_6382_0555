using BL;
using Caliburn.Micro;
using PL.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace PL.ViewModels
{
    class ShowBusesViewModel : Conductor<object>
    {
        #region private fields
        MainViewModel _mainViewModel;
        private BindableCollection<BusModel> _buses;
        private BusModel _selectedBus = new BusModel();
        private bool _showAll = false;
        IBL logic = BLFactory.API;
        #endregion
        public ShowBusesViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            SetBusesList();
            _selectedBus = Buses.Count == 0 ? null : Buses[0];
            BackgroundWorker autoUpdateWorker = new BackgroundWorker();
            autoUpdateWorker.DoWork += TrackBuses;
            autoUpdateWorker.WorkerReportsProgress = true;
            autoUpdateWorker.ProgressChanged += MakeUpdateToBuses;
            autoUpdateWorker.RunWorkerAsync();
        }
        #region properties for Caliburn.Micro
        public string ShowAllButtonText
        {
            get => _showAll ? "Show available" : "Show all buses";
        }
        public BindableCollection<BusModel> Buses
        {
            get => _buses;
            set
            {
                _buses = value;
                NotifyOfPropertyChange(() => Buses);
            }
        }

        public BusModel SelectedBus
        {
            get => _selectedBus;
            set
            {
                _selectedBus = value;
                NotifyOfPropertyChange(() => SelectedBus);
            }
        }
        #endregion
        #region events
        public void ToggleAllBuses()
        {
            _showAll = !_showAll;
            NotifyOfPropertyChange(() => ShowAllButtonText);
            BusModel.ControlsVisibility = _showAll ? Visibility.Hidden : Visibility.Visible;
            SetBusesList();
        }
        public void TrackBuses(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                Thread.Sleep(5000);
                worker.ReportProgress(0);
            }
        }
        public void MakeUpdateToBuses(object sender, ProgressChangedEventArgs args)
        {
            SetBusesList();
        }
        private void SetBusesList()
        {
            if (_showAll)
            {
                this.Buses =
                    new BindableCollection<BusModel>(logic.AllBuses()
                                                          .Select(bus => new BusModel(bus)));
            }
            else
            {
                this.Buses =
                    new BindableCollection<BusModel>(logic.AllAvailableBuses()
                                                          .Select(bus => new BusModel(bus)));
            }
        }

        public void RefuelBus(string licenseToRefuel)
        {
            logic.RefuelBus(BusModel.ReFormatLicense(licenseToRefuel));
            SetBusesList();
        }
        public void CareBus(string licenseToCare)
        {
            logic.CareBus(BusModel.ReFormatLicense(licenseToCare));
            SetBusesList();
        }
        public void AddBus()
        {
            _mainViewModel.LoadPageNoBack("AddBus");
        }
        public void ShowBusData()
        {
            if (SelectedBus != null)
            {
                ActivateItem(new ShowBusDataViewModel(_mainViewModel, SelectedBus));
            }
        }
        #endregion
    }
}
