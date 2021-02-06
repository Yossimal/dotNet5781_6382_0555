using BL.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DALAPI.DAO;
using DALAPI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Simulation.EventArgs;
using BL.Internal_Objects;
using System.Threading;

namespace BL.Simulation
{
    /// <summary>
    /// Sending all the drives owhen a station is selected
    /// </summary>
    class DrivesManager
    {
        #region singleton
        /// <summary>
        /// Singelton instance
        /// </summary>
        private static DrivesManager _instance = null;
        public static DrivesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DrivesManager();
                }
                return _instance;
            }
        }
        private DrivesManager() { }
        #endregion
        #region private parameters
        /// <summary>
        /// Instance of the simulator
        /// </summary>
        private Simulator simulator = Simulator.Instance;
        /// <summary>
        /// DAL instance
        /// </summary>
        private IDAL dataAPI = DALFactory.API;
        /// <summary>
        /// all the lines that have that station in their path
        /// </summary>
        private List<BOLine> linesInStation = new List<BOLine>();
        /// <summary>
        /// all the drives that currently running
        /// this field is needed for clear all the drives when reselecting station
        /// </summary>
        private List<LineDriveHandler> runningHandlers;
        /// <summary>
        /// The station that we want to track
        /// </summary>
        private BOStation stationToTrack = null;
        /// <summary>
        /// is the manager working now? (true when a station is selected and not need to switch stations )
        /// </summary>
        private bool working = false;
        #endregion
        #region internals
        /// <summary>
        /// Event that calls when there is an update of the line
        /// </summary>
        internal EventHandler<LineDriveEventArgs> onLineUpdate;
        /// <summary>
        /// Event that calls when a line has finished his route
        /// </summary>
        internal EventHandler<LineDriveEventArgs> onLineFinish;

        /// <summary>
        /// Activate the manager with a given station
        /// </summary>
        /// <param name="station">the station that we want to simulate</param>
        internal void SetStationPanel(int station)
        {
            //if hte manager working => close the manager and stop all the old drives
            if (working){
                working = false;
                while (runningHandlers.Count != 0) {
                    Thread.Sleep(1);
                }
                StopDrives();
            }
            //start working on the new station simulation
            working = true;
            stationToTrack = BLFactory.API.GetStation(station);
            Thread managerThread = new Thread(DriveSender);
            managerThread.IsBackground = true;//Set the thread to background so if the method will be terminated
            ReadLinesInStation(station);
            managerThread.Start();//Start sending all the drives
        }
        #region events invokes
        /// <summary>
        /// invoke the update event
        /// </summary>
        /// <param name="lineTiming">line timing to send in the event args</param>
        internal void LineUpdate(BOLineTiming lineTiming)
        {
            LineDriveEventArgs args = new LineDriveEventArgs()
            {
                lineTiming = lineTiming
            };
            onLineUpdate.Invoke(this, args);
        }
        /// <summary>
        /// invoke the finish event
        /// </summary>
        /// <param name="lineTiming">line timing to send in the event args</param>
        internal void LineFinish(BOLineTiming lineTiming)
        {
            LineDriveEventArgs args = new LineDriveEventArgs()
            {
                lineTiming = lineTiming
            };
            onLineFinish.Invoke(this, args);

        }
        #endregion
        #endregion
        #region private methods
        /// <summary>
        /// starting all the given drives
        /// </summary>
        /// <param name="allSendingTimes">the drives that we need to start</param>
        void StartDrives(List<BusSendData> allSendingTimes)
        {
            List<BusSendData> sentLines = new List<BusSendData>();//save all the lines that was sent at that cicle in the list so we can remove them after
            //start sending the lines
            foreach (BusSendData sendData in allSendingTimes)
            {
                //if we need to send that current drive
                if (simulator.CurrentTime > sendData.SendTime)
                {
                    sentLines.Add(sendData);
                    BOLine lineToSend = linesInStation.FirstOrDefault(l => l.Id == sendData.LineId);//get the line that we want to send
                    if (lineToSend == null) {//sometimes the lines are removed from other places 
                        continue;
                    }
                    //send that drive
                    LineDriveHandler driveHandler = new LineDriveHandler(lineToSend, stationToTrack, sendData.SendTime);
                    driveHandler.StartDrive();
                }
            }
            //remove all the sended drives from the list
            foreach (BusSendData toRemove in sentLines)
            {
                allSendingTimes.Remove(toRemove);
            }
        }
        /// <summary>
        /// stop all the active drives
        /// </summary>
        void StopDrives()
        {
            //stop all the drive handlers
            foreach (LineDriveHandler handler in runningHandlers)
            {
                handler.StopDrive();
               
            }
            //clear the running handlers list
            runningHandlers = new List<LineDriveHandler>();
        }
        /// <summary>
        /// Sending all the lines to drive
        /// </summary>
        void DriveSender()
        {
            runningHandlers = new List<LineDriveHandler>();
            //Run until the simulator is terminated
            while (!simulator.needToStop&&working)
            {
                //save the time of the start of the simulator
                TimeSpan startTime = simulator.CurrentTime;
                //get all the send times
                List<DAOLineTrip> allTrips = dataAPI.Where<DAOLineTrip>(trip => linesInStation.Any(line => line.Id == trip.LineId))
                                                    .ToList();
                List<BusSendData> allSendingTimes = new List<BusSendData>();//all the times that we sending buses (startTime+k*frequency<endTime)
                //Make work list for today
                foreach (DAOLineTrip trip in allTrips)
                {
                    //if the frequency is null => send the line once
                    if (!trip.Frequency.HasValue)
                    {
                        if (trip.StartAt < startTime)
                        {
                            allSendingTimes.Add(new BusSendData() { LineId = trip.LineId, SendTime = trip.StartAt });
                        }
                    }
                    //else => send the line in each time that he need to be sended
                    else
                    {
                        //run from start time to end time with frequency steps
                        for (TimeSpan i = trip.StartAt; i <= trip.FinishAt.Value; i += trip.Frequency.Value)
                        {
                            if (i > startTime)
                            {
                                allSendingTimes.Add(new BusSendData() { LineId = trip.LineId, SendTime = i });
                            }
                        }
                    }
                }
                //after all the send data is ready => start sending the buses
                while (!simulator.needToStop && working && allSendingTimes.Count != 0)
                {
                    StartDrives(allSendingTimes);
                    Thread.Sleep(1000);
                }
            }
            //if the simulatoer need to stop => stop the manager
            if (simulator.needToStop)
            {
                StopDrives();
            }
        }
        /// <summary>
        /// Read all the lines in a given station (full read)
        /// </summary>
        /// <param name="stationId">the station id</param>
        void ReadLinesInStation(int stationId)
        {
            //the given station
            DAOStation currentStation = dataAPI.GetById<DAOStation>(stationId);
            //all the ids of the lines that have this station in their path
            IEnumerable<int> linesId = dataAPI.Where<DAOLineStation>(station => stationId == station.StationId)//get all the line stations of the given station
                                              .Select(station => station.LineId);//get the line id from the LineStation
            //get all the relevant line stations grouped by their line id
            var allLineStations = dataAPI.Where<DAOLineStation>(station => linesId.Contains(station.LineId))//get all the line stations
                                         .GroupBy(station => station.LineId);//group the line stations by their line id
            List<BOLine> lines = new List<BOLine>();//the lines list
            //read all the groups, set them to a BOLine and add them to the lines list
            foreach (var igroup in allLineStations)
            {
                //read the line
                DAOLine line = dataAPI.GetById<DAOLine>(igroup.Key);
                BOLine toAdd = new BOLine(line);
                //read all the line stations and sort them with the index in the path
                List<DAOLineStation> lineStations = igroup.OrderBy(lineStation => lineStation.Index)
                                                          .ToList();
                //Get BOLine station from each DAOLineStation (holds also the station data and the distance from the next station)
                List<BOLineStation> toAddPath = igroup.Select(dao =>
                {
                    DAOAdjacentStations currentAndNext;
                    //handle the distance calculations
                    if (dao.Index != lineStations.Count - 1)
                    {
                        currentAndNext = dataAPI.Where<DAOAdjacentStations>(ads => ads.FromStationId == dao.StationId && ads.ToStationId == lineStations[dao.Index + 1].StationId)
                                                                  .First();
                    }
                    else
                    {
                        currentAndNext = new DAOAdjacentStations()
                        {
                            Distance = 0,
                            Time = TimeSpan.Zero
                        };
                    }
                    //finish the distance calculations
                    //generate the BOLineStation  for the select
                    BOLineStation ret = new BOLineStation()
                    {
                        Code = dao.StationId,
                        Line = toAdd,
                        Name = currentStation.Name,
                        DistanceFromNext = currentAndNext.Distance,
                        TimeFromNext = currentAndNext.Time
                    };
                    return ret;
                }
                ).ToList();
                toAdd.Path = toAddPath;
                lines.Add(toAdd);
            }
            linesInStation = lines;//set the linesInStation list to lines
        }
        #endregion
      
    }
}
