using BL.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DALAPI.DAO;
using DALAPI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Simulation
{
    class DrivesManager
    {
        private Simulator simulator = Simulator.Instance;
        private IDAL dataAPI = DALFactory.API;
        private List<BOBus> busesInStation;
        void SetStationPanel(int station, Action<BOLineTiming> updateBus)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += DoWorkDriveManager;
            worker.ProgressChanged += ProgressChangedDriveManager;
            worker.RunWorkerCompleted += RunWorkerComplitedDriveManager;
        }

        void DoWorkDriveManager(object sender, DoWorkEventArgs args)
        {
            while (!simulator.needToStop)
            {

            }
        }
        void ProgressChangedDriveManager(object sender, ProgressChangedEventArgs args)
        {

        }
        void RunWorkerComplitedDriveManager(object sender, RunWorkerCompletedEventArgs args)
        {

        }
        void ReadBusesInStation(int stationId) { 
            List<DAOLineStation> lineStations = dataAPI.Where<DAOLineStation>(stationId=)
        }
    }
}
