using BL.Internal_Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Simulation
{
    class Simulator
    {
        #region Singleton
        private static Simulator _simulator;
        public static Simulator Instance => _simulator;
        private Simulator() { }
        static Simulator() {
            _simulator = new Simulator();
        }
        #endregion Singleton
        private volatile bool _needToStop = false;
        public void StartSimulation(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            SimulationStopwatch stopwatch = new SimulationStopwatch(rate, startTime);
            BackgroundWorker simulationWorker = new BackgroundWorker();
            simulationWorker.WorkerReportsProgress = true;
            DoWorkSimulationData doWorkData = new DoWorkSimulationData
            {
                stopwatch = stopwatch,
                updateTime = updateTime
            };
            _needToStop = false;
            simulationWorker.DoWork += DoWorkSimulation;
            simulationWorker.ProgressChanged += ProgressChangedSimulation;
            simulationWorker.RunWorkerCompleted += RunWorkerComplitedSimulation;

            simulationWorker.RunWorkerAsync(doWorkData);
        }
        public void StopSimulation()
        {
            _needToStop = true;
        }

        private void DoWorkSimulation(object sender, DoWorkEventArgs args)
        {
            DoWorkSimulationData data = args.Argument as DoWorkSimulationData;
            BackgroundWorker worker = sender as BackgroundWorker;
            data.stopwatch.Restart();
            while (!_needToStop)
            {
                Thread.Sleep(1000);
                ProgressChangedSimulationData dataToUpdate = new ProgressChangedSimulationData
                {
                    currentTime = data.stopwatch.CurrentTime,
                    updateTime = data.updateTime
                };
                worker.ReportProgress(0, dataToUpdate);
            }
            args.Result = new RunWorkerCompleatedSimulationData {
                stopwatch = data.stopwatch
            };
        }
        private void ProgressChangedSimulation(object sender, ProgressChangedEventArgs args)
        {
            ProgressChangedSimulationData data = args.UserState as ProgressChangedSimulationData;
            data.updateTime(data.currentTime);
        }
        private void RunWorkerComplitedSimulation(object sender, RunWorkerCompletedEventArgs args)
        {
            RunWorkerCompleatedSimulationData data = args.Result as RunWorkerCompleatedSimulationData;
            data.stopwatch.Stop();
        }


    }
}
