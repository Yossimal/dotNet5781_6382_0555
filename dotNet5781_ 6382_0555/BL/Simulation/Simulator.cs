﻿using BL.Internal_Objects;
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
        /// <summary>
        /// Singelton instance
        /// </summary>
        private static Simulator _simulator;
        public static Simulator Instance => _simulator;
        private Simulator() { }
        static Simulator()
        {
            _simulator = new Simulator();
        }
        #endregion Singleton
        /// <summary>
        /// the simulation stopwatch
        /// </summary>
        private SimulationStopwatch stopwatch;
        /// <summary>
        /// is the simulation needs to stop
        /// </summary>
        internal volatile bool needToStop = false;
        /// <summary>
        /// the current simulation time
        /// </summary>
        internal TimeSpan CurrentTime => stopwatch.CurrentTime;
        /// <summary>
        /// Starts the simulation
        /// </summary>
        /// <param name="startTime">the time to start the simulation from</param>
        /// <param name="rate">the speed of the simulation 1=1sec</param>
        /// <param name="updateTime">action to be called when the time is updated</param>
        public void StartSimulation(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            //generate the simulation data
            stopwatch = new SimulationStopwatch(rate, startTime);
            BackgroundWorker simulationWorker = new BackgroundWorker();
            simulationWorker.WorkerReportsProgress = true;
            DoWorkSimulationData doWorkData = new DoWorkSimulationData
            {
                stopwatch = stopwatch,
                updateTime = updateTime
            };
            needToStop = false;
            simulationWorker.DoWork += DoWorkSimulation;
            simulationWorker.ProgressChanged += ProgressChangedSimulation;
            simulationWorker.RunWorkerCompleted += RunWorkerComplitedSimulation;
            //run the simulation
            simulationWorker.RunWorkerAsync(doWorkData);
        }
        /// <summary>
        /// stop the simulation (soft stop)
        /// </summary>
        public void StopSimulation()
        {
            needToStop = true;
        }
        #region the BGW event handlers
        /// <summary>
        ///  BGW Dowork
        /// </summary>
        private void DoWorkSimulation(object sender, DoWorkEventArgs args)
        {
            DoWorkSimulationData data = args.Argument as DoWorkSimulationData;
            BackgroundWorker worker = sender as BackgroundWorker;
            data.stopwatch.Restart();
            while (!needToStop)
            {
                Thread.Sleep(1000);
                ProgressChangedSimulationData dataToUpdate = new ProgressChangedSimulationData
                {
                    currentTime = data.stopwatch.CurrentTime,
                    updateTime = data.updateTime
                };
                worker.ReportProgress(0, dataToUpdate);
            }
            args.Result = new RunWorkerCompleatedSimulationData
            {
                stopwatch = data.stopwatch
            };
        }
        /// <summary>
        /// the BGW ProgressChanged
        /// </summary>
        private void ProgressChangedSimulation(object sender, ProgressChangedEventArgs args)
        {
            ProgressChangedSimulationData data = args.UserState as ProgressChangedSimulationData;
            data.updateTime(data.currentTime);
        }
        /// <summary>
        /// The BGW RunWorkerCompleted
        /// </summary>
        private void RunWorkerComplitedSimulation(object sender, RunWorkerCompletedEventArgs args)
        {
            RunWorkerCompleatedSimulationData data = args.Result as RunWorkerCompleatedSimulationData;
            data.stopwatch.Stop();
        }
        #endregion


    }
}
