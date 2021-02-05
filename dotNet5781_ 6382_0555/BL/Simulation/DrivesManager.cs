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
    class DrivesManager
    {
        #region singleton
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

        private Simulator simulator = Simulator.Instance;
        private IDAL dataAPI = DALFactory.API;
        private List<BOLine> linesInStation = new List<BOLine>();
        private List<LineDriveHandler> runningHandlers;
        private BOStation stationToTrack = null;
        private bool working = false;

        internal EventHandler<LineDriveEventArgs> onLineUpdate;
        internal EventHandler<LineDriveEventArgs> onLineFinish;

        internal void SetStationPanel(int station)
        {
            if (working){
                working = false;
                while (runningHandlers.Count != 0) {
                    Thread.Sleep(1);
                }
                StopDrives();
            }
            working = true;
            stationToTrack = BLFactory.API.GetStation(station);
            Thread managerThread = new Thread(DriveSender);
            managerThread.IsBackground = true;
            ReadLinesInStation(station);
            managerThread.Start();
        }
        void StartDrives(List<BusSendData> allSendingTimes)
        {
            List<BusSendData> sentLines = new List<BusSendData>();//save all the lines that was sent at that cicle in the list so we can remove them after
            foreach (BusSendData sendData in allSendingTimes)
            {
                if (simulator.CurrentTime > sendData.SendTime)
                {
                    sentLines.Add(sendData);
                    BOLine lineToSend = linesInStation.FirstOrDefault(l => l.Id == sendData.LineId);
                    if (lineToSend == null) {
                        continue;
                    }
                    LineDriveHandler driveHandler = new LineDriveHandler(lineToSend, stationToTrack, sendData.SendTime);
                    driveHandler.StartDrive();
                }
            }
            foreach (BusSendData toRemove in sentLines)
            {
                allSendingTimes.Remove(toRemove);
            }
        }
        void StopDrives()
        {
            foreach (LineDriveHandler handler in runningHandlers)
            {
                handler.StopDrive();
               
            }
            runningHandlers = new List<LineDriveHandler>();
        }
        void DriveSender()
        {
            runningHandlers = new List<LineDriveHandler>();
            while (!simulator.needToStop&&working)
            {
                TimeSpan startTime = simulator.CurrentTime;
                //get all the send times
                List<DAOLineTrip> allTrips = dataAPI.Where<DAOLineTrip>(trip => linesInStation.Any(line => line.Id == trip.LineId))
                                                    .ToList();
                List<BusSendData> allSendingTimes = new List<BusSendData>();
                //Make work list for today
                foreach (DAOLineTrip trip in allTrips)
                {
                    //if the frequency is null => send the libe once
                    if (!trip.Frequency.HasValue)
                    {
                        if (trip.StartAt < startTime)
                        {
                            allSendingTimes.Add(new BusSendData() { LineId = trip.LineId, SendTime = trip.StartAt });
                        }
                    }
                    else
                    {
                        for (TimeSpan i = trip.StartAt; i <= trip.FinishAt.Value; i += trip.Frequency.Value)
                        {
                            if (i > startTime)
                            {
                                allSendingTimes.Add(new BusSendData() { LineId = trip.LineId, SendTime = i });
                            }
                        }
                    }
                }
                while (!simulator.needToStop && working && allSendingTimes.Count != 0)
                {
                    StartDrives(allSendingTimes);
                    Thread.Sleep(1000);
                }
            }
            if (simulator.needToStop)
            {
                StopDrives();
            }
        }
        void ReadLinesInStation(int stationId)
        {
            DAOStation currentStation = dataAPI.GetById<DAOStation>(stationId);
            IEnumerable<int> linesId = dataAPI.Where<DAOLineStation>(station => stationId == station.StationId)
                                              .Select(station => station.LineId);
            var allLineStations = dataAPI.Where<DAOLineStation>(station => linesId.Contains(station.LineId))
                                         .GroupBy(station => station.LineId);
            List<BOLine> lines = new List<BOLine>();
            foreach (var igroup in allLineStations)
            {
                DAOLine line = dataAPI.GetById<DAOLine>(igroup.Key);
                BOLine toAdd = new BOLine(line);
                List<DAOLineStation> lineStations = igroup.OrderBy(lineStation => lineStation.Index)
                                                              .ToList();
                
                List<BOLineStation> toAddPath = igroup.Select(dao =>
                {
                    DAOAdjacentStations currentAndNext;
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
            linesInStation = lines;
        }
        internal void LineUpdate(BOLineTiming lineTiming)
        {
            LineDriveEventArgs args = new LineDriveEventArgs()
            {
                lineTiming = lineTiming
            };
            onLineUpdate.Invoke(this, args);
        }
        internal void LineFinish(BOLineTiming lineTiming)
        {
            LineDriveEventArgs args = new LineDriveEventArgs()
            {
                lineTiming = lineTiming
            };
            onLineFinish.Invoke(this, args);

        }
    }
}
