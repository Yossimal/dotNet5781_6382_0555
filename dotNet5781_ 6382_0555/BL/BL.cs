using BL.BO;
using BL.Internal_Objects;
using BL.RestfulAPIModels;
using BL.Simulation;
using BL.Simulation.EventArgs;
using BL.Exceptions;
using DALAPI;
using DALAPI.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BL
{
    internal class BL : IBL
    {
        #region Singelton
        private static readonly BL _instance = new BL();
        public static BL Instance => _instance;
        private BL() { }
        #endregion Singelton

        #region Attributes
        IDAL dataAPI = DALFactory.API;//DL api instance
        const string MANAGER_CODE = "123!!!";//code for registering managers
        static readonly TimeSpan REFUEL_TIME = new TimeSpan(0, 2, 0, 0);//the duration of bus refuel
        static readonly TimeSpan CARE_TIME = new TimeSpan(1, 0, 0, 0);//the duration of bus care
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);//for checking the internet connection
        private const double AVERAGE_SPEED = 60;//the average bus speed
        #endregion Attributes
        #region Implementation
        public BOUser CheckUserName(BOUser user)
        {
            //all the users with the username and password
            IEnumerable<DAOUser> response = dataAPI.Where<DAOUser>(usr => user.UserName == usr.UserName && user.Password == usr.Password);
            if (response.Count() == 0)
            {
                throw new BadLoginDataException("Username or password is wrong");
            }
            BOUser ret = new BOUser
            {
                UserName = response.First().UserName,
                id = response.First().Id,
                IsManager = response.First().IsAdmin,
                Password = response.First().Password
            };
            return ret;
        }
        public int Register(BORegister register)
        {
            try
            {
                //check the register data
                if (register.User == null
                    || register.User.UserName == null
                    || register.User.UserName.Length <= 3
                    || register.User.UserName.Length >= 16)
                {
                    throw new BadUsernameException("User name length must be between 3 and 16 characters");
                }
                if (register.User.Password == null
                    || register.User.Password.Length <= 3
                    || register.User.Password.Length >= 16)
                {
                    throw new BadPasswordException("Password length must be between 3 and 16 characters");
                }
                DAOUser toAdd = new DAOUser
                {
                    UserName = register.User.UserName,
                    Password = register.User.Password,
                    IsAdmin = register.User.IsManager
                };

                if (register.User.IsManager && register.ManagerCode != MANAGER_CODE)
                {
                    throw new BadManagerCodeException("Bad manager code! Can't register");
                }
                if (dataAPI.Where<DAOUser>(user => user.UserName == toAdd.UserName).Count() > 0)
                {
                    throw new ItemAlreadyExistsException("User name already exists!");
                }
                //register the user
                return dataAPI.Add(toAdd);
            }
            catch (Exception ex)
            {
                bool isHandled = ex is ItemAlreadyExistsException
                              || ex is BadManagerCodeException
                              || ex is BadPasswordException
                              || ex is BadUsernameException;
                if (isHandled)
                {
                    throw ex;
                }
                throw new InvalidOperationException("There was problem to write the data", ex);
            }
        }
        public IEnumerable<BOBus> AllAvailableBuses()
        {
            //get all the available buses
            //Can be written in two lines with chaining methods LINQ but the course require the SQL style LINQ
            IEnumerable<BOBus> ret = from dataBus in dataAPI.All<DAOBus>()
                                     let bus = new BOBus(dataBus)
                                     where bus.Status == BusStatus.Ready || bus.TimeToReady < DateTime.Now
                                     orderby bus.LicenseNumber
                                     select bus;
            return RefreshBusesAvailability(ret);
        }
        public IEnumerable<BOBus> AllBuses()
        {
            //get all the buses
            var ret = dataAPI.All<DAOBus>()
                             .Select(bus => new BOBus(bus))
                             .OrderBy(bus => bus.LicenseNumber);
            return RefreshBusesAvailability(ret);
        }
        public BOBus RefuelBus(int licenseNumber)
        {
            try
            {
                //get the bus to refuel
                DAOBus toRefuel = dataAPI.GetById<DAOBus>(licenseNumber);
                if (toRefuel.Status != BusStatus.Ready)
                {
                    throw new BusNotAvailableException("The bus is not ready yet.");
                }
                toRefuel.Status = BusStatus.Refuel;
                toRefuel.TimeToReady = DateTime.Now + REFUEL_TIME;
                //refuel the bus
                dataAPI.Update(toRefuel);
                //return the bus on refueling
                return new BOBus(toRefuel);
            }
            catch (Exception ex)
            {
                if (ex is DALAPI.ItemNotFoundException)
                {
                    throw new ItemNotFoundException("Can't find the bus with that license number", ex);
                }
                throw ex;
            }
        }
        public BOBus CareBus(int licenseNumber)
        {
            //same as refuel
            try
            {
                DAOBus toCare = dataAPI.GetById<DAOBus>(licenseNumber);
                if (toCare.Status != BusStatus.Ready)
                {
                    throw new BusNotAvailableException("The bus is not ready yet.");
                }
                toCare.Status = BusStatus.InCare;
                toCare.TimeToReady = DateTime.Now + CARE_TIME;
                dataAPI.Update(toCare);
                return new BOBus(toCare);
            }
            catch (Exception ex)
            {
                if (ex is DALAPI.ItemNotFoundException)
                {
                    throw new ItemNotFoundException("Can't find the bus with that license number", ex);
                }
                throw ex;
            }
        }
        public TimeSpan? TimeToReady(int licenseNumber)
        {
            DAOBus busToTrack;
            try
            {
                //get the bus to check
                busToTrack = dataAPI.GetById<DAOBus>(licenseNumber);
            }
            catch (Exception ex)
            {
                if (ex is DALAPI.ItemNotFoundException)
                {
                    throw new ItemNotFoundException("Can't find the bus with that license number", ex);
                }
                throw ex;
            }
            //if the bus is already ready -> return null
            if (!busToTrack.TimeToReady.HasValue)
            {
                return null;
            }
            else if (busToTrack.TimeToReady < DateTime.Now)
            {
                busToTrack.TimeToReady = null;
                busToTrack.Status = BusStatus.Ready;
                dataAPI.Update(busToTrack);
                return null;
            }
            return busToTrack.TimeToReady - DateTime.Now;

        }
        public int AddBus(BOBus toAdd)
        {
            //set the las care date (if the last care data is null so it need to be the current date)
            DateTime lastCareDate = toAdd.LastCareDate;
            if (toAdd.LicenseDate == DateTime.Today)
            {
                lastCareDate = DateTime.Today;
            }
            DAOBus daoToAdd = new DAOBus
            {
                Id = toAdd.LicenseNumber,
                LicenseDate = toAdd.LicenseDate,
                FuelRemain = BOBus.MAX_FUEL,
                IsDeleted = false,
                MileageCounter = toAdd.MileageCounter,
                Status = BusStatus.Ready,
                TimeToReady = null,
                LastCareDate = lastCareDate
            };
            try
            {
                //add the bus
                return dataAPI.Add(daoToAdd);
            }
            catch (Exception ex)
            {
                if (ex is DALAPI.ItemAlreadyExistsException)
                {
                    throw new ItemAlreadyExistsException("There is already a bus with that license number.", ex);
                }
                throw ex;
            }
        }
        public bool DeleteBus(int licenseNumber)
        {
            DAOBus toRemove = new DAOBus
            {
                Id = licenseNumber
            };
            return dataAPI.Remove(toRemove);
        }
        public BOBus GetBus(int licenseNumber)
        {
            DAOBus toReturn = dataAPI.GetById<DAOBus>(licenseNumber);
            //check if the bus ready (to set it as ready)
            if (toReturn.TimeToReady.HasValue && toReturn.TimeToReady < DateTime.Now)
            {
                toReturn.Status = BusStatus.Ready;
                toReturn.TimeToReady = null;
                dataAPI.Update(toReturn);
            }
            if (toReturn == null)
            {
                throw new ItemNotFoundException("Can't find the bus with the given license number");
            }
            return new BOBus
            {
                LicenseDate = toReturn.LicenseDate,
                FuelRemain = toReturn.FuelRemain,
                LastCareDate = toReturn.LastCareDate,
                LicenseNumber = toReturn.LicenseNumber,
                MileageCounter = toReturn.MileageCounter,
                Status = toReturn.Status,
                TimeToReady = toReturn.TimeToReady
            };
        }
        public IEnumerable<BOStation> AllStations()
        {
            return dataAPI.All<DAOStation>().Select(station =>
            {//convert all the DAO stations to BO stations
                return new BOStation
                {
                    Code = station.Code,
                    Name = station.Name
                };
            });
        }
        public BOStation GetStation(int code)
        {
            return new BOStation(dataAPI.GetById<DAOStation>(code));
        }
        public bool DeleteStation(int code)
        {
            if (AllLinesInStation(code).Count() != 0) {
                throw new StationInUseException($"the station {code} is in use");
            }
            DAOStation toRemove = new DAOStation
            {
                Id = code
            };
            return dataAPI.Remove(toRemove);
        }
        public int AddStation(BOAddStation toAdd)
        {

            DAOStation daoToAdd = new DAOStation
            {
                Id = toAdd.Station.Code,
                Name = toAdd.Station.Name,
                Longitude = toAdd.Longitude,
                Latitude = toAdd.Latitude
            };
            try
            {
                return dataAPI.Add(daoToAdd);
            }
            catch (Exception ex)
            {
                if (ex is DALAPI.ItemAlreadyExistsException)
                {
                    throw new ItemAlreadyExistsException("There is already a station with that code", ex);
                }
                throw ex;
            }
        }
        public BOLine GetLine(int id)
        {
            DAOLine daoLine = dataAPI.GetById<DAOLine>(id);
            if (daoLine == null)
            {
                throw new ItemNotFoundException("Can't find a line with that id.");
            }
            BOLine ret = new BOLine(daoLine);
            ret.Path = AllLineStations(ret);//get all the stations in the line ordered by their index
            return ret;
        }
        public IEnumerable<BOLine> GetAllLines(int lineNumber)
        {
            return dataAPI.Where<DAOLine>(line => line.Code == lineNumber)
                 .Select(line =>
                 {
                     BOLine ret = new BOLine(line);
                     ret.Path = AllLineStations(ret);//get all the stations in the line ordered by their index
                     return ret;
                 });
        }
        public IEnumerable<BOLine> GetAllLines()
        {
            return dataAPI.All<DAOLine>()
                .Select(line =>
                {
                    BOLine ret = new BOLine(line);
                    ret.Path = AllLineStations(ret);//get all the stations in the line ordered by their index
                    return ret;
                }).OrderBy(line=>line.LineNumber);
        }
        public bool RemoveLine(int id)
        {
            DAOLine toRemove = new DAOLine { Id = id };
            return dataAPI.Remove(toRemove);
        }
        public async Task<int> AddLine(BOLine toAdd)
        {
            List<BOStation> stations = toAdd.Path.ToList();
            await AddAllStationsToDatabase(stations);//add the adjacent stations data to the database (if there is no DAOAdjacentStation -> create it using HERE API)

            DAOLine daoToAdd = new DAOLine
            {
                Area = toAdd.EnumArea,
                Code = toAdd.LineNumber,
                FirstStationId = toAdd.Path.First().Code,
                LastStationId = toAdd.Path.Last().Code,
            };
            int toAddId = 0;//the new line id
            try
            {
                toAddId = dataAPI.Add(daoToAdd);
            }
            catch (Exception ex)
            {
                //currently there is no suspect exceptions from the data API.
                throw ex;
            }
            //add all the lines stations to the database
            for (int i = 0; i < stations.Count; i++)
            {
                DAOLineStation lineStation = new DAOLineStation
                {
                    Index = i,
                    LineId = toAddId,
                    StationId = stations[i].Code,
                    NextStationId = (i == stations.Count - 1 ? -1 : stations[i + 1].Code),
                    PrevStationId = (i == 0 ? -1 : stations[i - 1].Code)
                };
                try
                {
                    dataAPI.Add(lineStation);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return toAddId;
        }
        public async Task<BOLine> AddStationToLine(int stationCode, int lineId, int index = -1)
        {
            DAOStation toAddData;//the station to add
            //try to get the station from the database
            try
            {
                toAddData = dataAPI.GetById<DAOStation>(stationCode);
            }
            catch (Exception ex)
            {
                if (ex is DALAPI.ItemNotFoundException)
                {
                    throw new ItemNotFoundException("The station not exists in the database", ex);
                }
                throw ex;
            }
            int pathLength = GetLinePathLength(lineId);//get the current length of the path
            if (index >= pathLength)//if we got an index out of the path range
            {
                throw new IndexOutOfRangeException("The station index is out of range");
            }
            DAOLineStation toAdd = new DAOLineStation
            {
                StationId = toAddData.Id,
                LineId = lineId
            };
            //when the method comes with no index -> add the station to the end
            if (index <= -1)
            {
                index = pathLength;
                toAdd.NextStationId = -1;
                //the previous station -> the last station in the path
                DAOLineStation prev = dataAPI.Where<DAOLineStation>(s => s.LineId == lineId && s.Index == pathLength - 1).First();
                //set the previous station data
                toAdd.PrevStationId = prev.StationId;
                toAdd.NextStationId = -1;
                toAdd.Index = pathLength;
                prev.NextStationId = toAdd.StationId;
                //update the previous station in the database
                dataAPI.Update(prev);
                //add the station to the database
                dataAPI.Add(toAdd);
                //update the distance and time between that station and the previous station
                await UpdateNearStations(toAdd.PrevStationId, toAdd.StationId);
            }
            //In the case that we want to add the station as first station we need to have other implementation 
            else if (index == 0)
            {
                //get all the stations in the line path
                List<DAOLineStation> stationsToUpdate = dataAPI.Where<DAOLineStation>(s => s.LineId == lineId).OrderBy(s => s.Index).ToList();
                //Set the data of the station that we want to add
                toAdd.NextStationId = stationsToUpdate[0].StationId;
                toAdd.Index = 0;
                toAdd.PrevStationId = -1;
                //push all the stations one step forward
                foreach (DAOLineStation stat in stationsToUpdate)
                {
                    stat.Index++;
                    dataAPI.Update(stat);
                }
                //add the station to the database
                dataAPI.Add(toAdd);
                //set the adjacent stations data
                await UpdateNearStations(toAdd.StationId, toAdd.NextStationId);
            }
            //If the method has called with index>0 -> update all the indexes of the stations in the line before the index
            else
            {
                //Get all the stations from the index to add and after
                List<DAOLineStation> stationsToUpdate = dataAPI.Where<DAOLineStation>(s => s.LineId == lineId && s.Index >= index).OrderBy(s => s.Index).ToList();
                DAOLineStation nextStation = Min(stationsToUpdate, ls => ls.Index);
                //set the data of the station that we want to add to the line
                toAdd.Index = stationsToUpdate[0].Index;
                toAdd.NextStationId = stationsToUpdate[0].StationId;
                //set the previos station of the station that after the statuion that we added to the station that we've added
                stationsToUpdate[0].PrevStationId = toAdd.StationId;
                //increament the index of all the stations above the station that we've added
                for (int i = stationsToUpdate.Count - 1; i >= 0; i--)
                {
                    stationsToUpdate[i].Index++;
                    dataAPI.Update(stationsToUpdate[i]);
                }
                //Update the next station of the previos station to be the id of the station that we want to add
                DAOLineStation prevStation = dataAPI.Where<DAOLineStation>(station => station.Index == index - 1).First();
                prevStation.NextStationId = toAdd.StationId;
                toAdd.PrevStationId = prevStation.StationId;
                dataAPI.Update(prevStation);
                //handle the distances
                await UpdateNearStations(toAdd.StationId, toAdd.NextStationId);
                await UpdateNearStations(toAdd.PrevStationId, toAdd.StationId);
                dataAPI.Add(toAdd);
            }

            //return the new line
            DAOLine daoRet = dataAPI.GetById<DAOLine>(lineId);
            BOLine ret = new BOLine(daoRet);
            ret.Path = AllLineStations(ret);
            return ret;
        }
        public async Task<BOLine> RemoveStationFromLine(int lineId, int stationCode)
        {
            int pathLength = GetLinePathLength(lineId);//the length of the path
            DAOLineStation stationToRemove = dataAPI.Where<DAOLineStation>(ls=>ls.StationId==stationCode).FirstOrDefault();//the station that we want to remove
            int index = stationToRemove.Index;//the index of the station to remove
            if (index >= pathLength)//if the index is out of range ->throw exception
            {
                throw new IndexOutOfRangeException("The index is higher then the path length");
            }
            //all the stations that we need to update due to the remove
            List<DAOLineStation> stationsToUpdate = dataAPI.Where<DAOLineStation>(s => s.Index > index).OrderBy(s => s.Index).ToList();

            //update the previos station
            if (index != 0)
            {
                DAOLineStation prev = dataAPI.Where<DAOLineStation>(s => s.Index == index - 1).First();
                prev.NextStationId = stationsToUpdate[0].StationId;
                stationsToUpdate[0].PrevStationId = prev.StationId;
                await UpdateNearStations(prev.StationId, prev.NextStationId);
                dataAPI.Update(prev);
            }
            else
            {
                stationsToUpdate[0].PrevStationId = -1;
            }
            //decreament the index of all the stations in the database
            foreach (DAOLineStation station in stationsToUpdate)
            {
                station.Index--;
                dataAPI.Update(station);
            }
            //remove the station
            dataAPI.Remove(stationToRemove);
            return GetLine(lineId);
        }
        public bool IsInternetAvailable()
        {
            int description;
            //check if the internet available
            return InternetGetConnectedState(out description, 0);
        }
        public IEnumerable<string> GetAllAreas()
        {
            return new string[5] { "General", "North", "South", "Center", "Jerusalem" };
        }

        public BOLineStation GetLineStationFromStationAndLine(int lineId, int stationId, out BOStation next, out BOStation prev, bool getFullLine = false)
        {
            //data about the line and the station and the line station
            DAOStation station = dataAPI.GetById<DAOStation>(stationId);
            DAOLineStation lineStation = dataAPI.Where<DAOLineStation>(s => s.StationId == stationId && s.LineId == lineId).First();
            DAOLine line = dataAPI.GetById<DAOLine>(lineId);
            BOLine lineToRet = null;//the line that holds that station
            //set the next and prev to null so if the station is at the start or at the end we wont need to handle the null problems
            next = null;
            prev = null;
            if (getFullLine)//if we want the full line -> set the full line in the path
            {
                lineToRet = GetLine(lineId);
            }
            else
            {
                lineToRet = new BOLine
                {
                    EnumArea = line.Area,
                    Id = line.Id,
                    LineNumber = line.Code,
                    Path = null
                };
            }
            //For the last station we need another impemitation
            if (lineStation.NextStationId == -1)
            {
                if (lineStation == null || station == null || line == null)
                {
                    throw new ItemNotFoundException("Cant find the line staation in the database.");
                }
                prev = GetStation(lineStation.PrevStationId);

                return new BOLineStation
                {
                    Code = station.Code,
                    Line = lineToRet,
                    Name = station.Name,
                    DistanceFromNext = 0,
                    TimeFromNext = new TimeSpan(0)
                };
            }
            //if it not the last station -> we need to get also the distance from the next station
            DAOAdjacentStations nextStationData = dataAPI.Where<DAOAdjacentStations>(adjacentStations => (adjacentStations.FromStationId == station.Id) && (adjacentStations.ToStationId == lineStation.NextStationId)).First();
            //if this is the first station we dont want to update the prev station
            if (lineStation.Index != 0)
            {
                prev = GetStation(lineStation.PrevStationId);//set the previous station
            }
            if (lineStation == null || station == null || line == null || nextStationData == null)
            {
                throw new ItemNotFoundException("Cant find the line staation in the database.");
            }
            next = GetStation(lineStation.NextStationId);//set the next station
            return new BOLineStation
            {
                Code = station.Code,
                Name = station.Name,
                DistanceFromNext = nextStationData.Distance,
                TimeFromNext = nextStationData.Time,
                Line = lineToRet
            };
        }
        public void UpdateNearStations(int fromCode, int toCode, double distance = -1, TimeSpan? time = null)
        {
            //gets the adjacent stations that we need to update
            IEnumerable<DAOAdjacentStations> stationQuery = dataAPI.Where<DAOAdjacentStations>(s => s.FromStationId == fromCode && s.ToStationId == toCode);
            if (stationQuery.Count() == 0)
            {
                throw new ItemNotFoundException("Cant find the path between those stations.");
            }
            DAOAdjacentStations toUpdate = stationQuery.First();
            //check if we need to do something
            if (distance < 0 && !time.HasValue)
            {
                return;
            }
            //check if need to update the distance
            if (distance > 0)
            {
                toUpdate.Distance = distance;
            }
            //check if need to update the time
            if (time.HasValue)
            {
                toUpdate.Time = time.Value;
            }
            dataAPI.Update(toUpdate);
        }
        public void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            //starts the simulator
            Simulator.Instance.StartSimulation(startTime, rate, updateTime);
        }
        public void StopSimulator()
        {
            //stop the simulator
            Simulator.Instance.StopSimulation();
        }
        public void OnLineUpdate(EventHandler<LineDriveEventArgs> handler)
        {
            //add the evnet
            DrivesManager.Instance.onLineUpdate += handler;
        }
        public void OnLineFinish(EventHandler<LineDriveEventArgs> handler)
        {
            //add the event
            DrivesManager.Instance.onLineFinish += handler;
        }
        public void SetStationToTrack(int stationId)
        {
            //set the station
            DrivesManager.Instance.SetStationPanel(stationId);
        }
        public IEnumerable<BOYellowSign> AllLinesInStation(int stationId)
        {
            
            return dataAPI.Where<DAOLineStation>(s => s.StationId == stationId)//all the line stations in that station
                          .Select(s => dataAPI.GetById<DAOLine>(s.LineId))//get all the lines in that station
                          .Select(l => new BOYellowSign { LineNumber = l.Code, LastStationName = GetLastStation(l.Id).Name });//convert to YellowSign
        }
        #endregion Implementation
        #region private and internal methods
        /// <summary>
        /// get all the line stations of a line
        /// </summary>
        /// <param name="line">the line</param>
        /// <returns>list of the line's line stations</returns>
        internal List<BOStation> AllLineStations(BOLine line)
        {
            //get all the line stations of this line as dao
            var query1 = dataAPI.Where<DAOLineStation>(station => station.LineId == line.Id);
            //join those line stations with the basic stations data
            var query2 = query1.Join(dataAPI.All<DAOStation>(),
                           s => s.StationId,
                           s => s.Id,
                           (lineStation, station) => new { station = new BOStation(station), nextStationId = lineStation.NextStationId, index = lineStation.Index });
            //join to that data the adjacent stations data
            var query3 = query2.Join(dataAPI.All<DAOAdjacentStations>(),
                                s => s.station.Code,
                                s => s.FromStationId,
                                (stationData, adjacentStation) => new { stationData = stationData, adjacentStationData = adjacentStation });
            //build the BO line stations
            var query4 = query3.Where(data => data.stationData.nextStationId == data.adjacentStationData.ToStationId)
                                       .Select(data => new
                                       {
                                           station = new BOLineStation
                                           {
                                               Code = data.stationData.station.Code,
                                               Name = data.stationData.station.Name,
                                               Line = line,
                                               DistanceFromNext = data.adjacentStationData.Distance,
                                               TimeFromNext = data.adjacentStationData.Time
                                           },
                                           data.stationData.index
                                       }).OrderBy(station => station.index)
                                        .Select(station => (station.station as BOStation))
                                        .ToList();
            //the data to return
            var ret = query4;
            ////We need to add the last station (the LINQ dont cover it
            var lastStationQuery = dataAPI.Where<DAOLineStation>(s => s.LineId == line.Id);
            int lastStationIndex = lastStationQuery.Max(x => x.Index);
            DAOLineStation lastLineStation = dataAPI.Where<DAOLineStation>(s => s.LineId == line.Id && s.Index == lastStationIndex).First();
            DAOStation lastStation = dataAPI.Where<DAOStation>(s => s.Code == lastLineStation.StationId).First();
            BOStation lastStationBO = new BOStation
            {
                Code = lastStation.Id,
                Name = lastStation.Name
            };
            ret.Add(lastStationBO);
            return ret;
        }
        /// <summary>
        /// Get the distance between two stations
        /// </summary>
        /// <param name="from">the source station</param>
        /// <param name="to">the destination station</param>
        /// <returns>the distance between those stations</returns>
        private async Task<double> GetDistanceBetweenStations(DAOStation from, DAOStation to)
        {
            //generate new Distance instance for handling the RESTAPI request
            Distance dist = new Distance();
            dist.FromLat = from.Latitude;
            dist.FromLon = from.Longitude;
            dist.ToLon = to.Longitude;
            dist.ToLat = to.Latitude;
            //open new request
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(dist.RequestURL))
            {
                string JSONText = await response.Content.ReadAsStringAsync();
                return dist.GetDistance(JSONText);
            }
        }
        /// <summary>
        /// add all the stations of a line to the database (check that all the stations will have adjacent stations data)
        /// </summary>
        /// <param name="stations">all the stations to set with their order</param>
        private async Task AddAllStationsToDatabase(List<BOStation> stations)
        {
            //We need to work with two adjacent stations so we need to work with basic for loop
            for (int i = 0; i < stations.Count - 1; i++)
            {
                IEnumerable<DAOAdjacentStations> checkEmpty = dataAPI.Where<DAOAdjacentStations>(a => stations[i].Code == a.FromStationId && stations[i + 1].Code == a.ToStationId);
                //If there is no data for the distance between those stations =>retruve it from the here api and add it to the database
                if (checkEmpty.Count() == 0)
                {
                    //We need to get the distance between the stations
                    DAOAdjacentStations connectCurrentStations = new DAOAdjacentStations
                    {
                        FromStationId = stations[i].Code,
                        ToStationId = stations[i + 1].Code
                    };
                    DAOStation from, to;
                    try
                    {
                        //We need to get the stations from the data so we can access top theyr longitude and latitude
                        from = dataAPI.GetById<DAOStation>(stations[i].Code);
                        to = dataAPI.GetById<DAOStation>(stations[i + 1].Code);
                    }
                    catch (Exception ex)
                    {
                        if (ex is ItemNotFoundException)
                        {
                            throw new InnerItemNotFoundException("Cant find one or more from the inner stations in the bus line station", ex);
                        }
                        throw ex;
                    }
                    try
                    {
                        //Connect to here api and get the distance
                        connectCurrentStations.Distance = await GetDistanceBetweenStations(from, to);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    connectCurrentStations.Time = TimeSpan.FromHours(connectCurrentStations.Distance / AVERAGE_SPEED);
                    try
                    {
                        //Add the connectCurrentStations to the database
                        dataAPI.Add(connectCurrentStations);
                    }
                    catch (Exception ex)
                    {
                        //we not suspect for exceptions in that case
                        throw ex;
                    }
                }
            }
        }
        /// <summary>
        /// checks if there is data about the time and distance between two stations and if not ->set them using HERE REST API
        /// </summary>
        /// <param name="fromId">the source station id</param>
        /// <param name="toId">the destination station id</param>
        private async Task UpdateNearStations(int fromId, int toId)
        {
            //Check if the stations data exists in the database
            if (dataAPI.Where<DAOAdjacentStations>(s => s.FromStationId == fromId && s.ToStationId == toId).Count() != 0)
            {
                return;
            }
            //get the station data
            DAOStation from = dataAPI.GetById<DAOStation>(fromId);
            DAOStation to = dataAPI.GetById<DAOStation>(toId);
            double distance = 0;
            try
            {
                //get the distance between those stations
                distance = await GetDistanceBetweenStations(from, to);
            }
            catch (Exception ex)
            {
                //currently no suspect exceptions
                throw ex;
            }
            //calculate the travel duration according to the distance and the average speed
            TimeSpan time = TimeSpan.FromHours(distance / AVERAGE_SPEED);
            //generate and add DAOAdjacentStations to the database
            DAOAdjacentStations toAdd = new DAOAdjacentStations
            {
                Distance = distance,
                Time = time,
                FromStationId = fromId,
                ToStationId = toId
            };
            dataAPI.Add(toAdd);

        }
        /// <summary>
        /// get the length of line path
        /// </summary>
        /// <param name="lineId">the line id</param>
        /// <returns>the line path length</returns>
        private int GetLinePathLength(int lineId)
        {
            //the amount of the line stations in that line
            return dataAPI.Where<DAOLineStation>(station => station.LineId == lineId).Count();
        }
        /// <summary>
        /// returns the minimum in a list according to given hash function
        /// </summary>
        /// <typeparam name="T">what we want to compare</typeparam>
        /// <param name="lst">the list</param>
        /// <param name="comparer">the hash function</param>
        /// <returns>the minimum value</returns>
        private T Min<T>(List<T> lst, Func<T, int> comparer)
        {
            int minVal = lst.Min(t => comparer(t));
            return lst.First(t => comparer(t) == minVal);
        }
        /// <summary>
        /// update all the buses availability enum according to their TimeToReady
        /// </summary>
        /// <param name="toRefresh">the list of the buses to refresh</param>
        /// <returns>the refreshed list</returns>
        private IEnumerable<BOBus> RefreshBusesAvailability(IEnumerable<BOBus> toRefresh)
        {
            foreach (BOBus bus in toRefresh)
            {
                if (bus.TimeToReady <= DateTime.Now && bus.Status != BusStatus.Ready)
                {
                    bus.TimeToReady = null;
                    bus.Status = BusStatus.Ready;
                    DAOBus toUpdate = dataAPI.GetById<DAOBus>(bus.LicenseNumber);
                    toUpdate.TimeToReady = bus.TimeToReady;
                    toUpdate.Status = bus.Status;
                    dataAPI.Update(toUpdate);
                }
            }
            return toRefresh;
        }
        /// <summary>
        /// get the last station in a line
        /// </summary>
        /// <param name="lineId">the line id</param>
        /// <returns>the last station in the given line</returns>
        internal DAOStation GetLastStation(int lineId)
        {
            return dataAPI.Where<DAOLineStation>(s => s.LineId == lineId && s.NextStationId == -1)
                          .Select(ls => dataAPI.GetById<DAOStation>(ls.StationId))
                          .First();
        }

        #endregion
    }
}
