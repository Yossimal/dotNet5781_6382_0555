using BL.BO;
using BL.Internal_Objects;
using BL.RestfulAPIModels;
using BL.Simulation;
using BL.Simulation.EventArgs;
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
        IDAL dataAPI = DALFactory.API;
        const string MANAGER_CODE = "123!!!";
        static readonly TimeSpan REFUEL_TIME = new TimeSpan(0, 2, 0, 0);
        static readonly TimeSpan CARE_TIME = new TimeSpan(1, 0, 0, 0);
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        private const double AVERAGE_SPEED = 60;
        #endregion Attributes
        #region Implementation
        public BOUser CheckUserName(BOUser user)
        {
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
            var ret = dataAPI.All<DAOBus>()
                             .Select(bus => new BOBus(bus))
                             .OrderBy(bus => bus.LicenseNumber);
            return RefreshBusesAvailability(ret);
        }
        public BOBus RefuelBus(int licenseNumber)
        {
            try
            {
                DAOBus toRefuel = dataAPI.GetById<DAOBus>(licenseNumber);
                if (toRefuel.Status != BusStatus.Ready)
                {
                    throw new BusNotAvailableException("The bus is not ready yet.");
                }
                toRefuel.Status = BusStatus.Refuel;
                toRefuel.TimeToReady = DateTime.Now + REFUEL_TIME;
                dataAPI.Update(toRefuel);
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
            {
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
            ret.Path = AllLineStations(ret);
            return ret;
        }
        public IEnumerable<BOLine> GetAllLines(int lineNumber)
        {
            return dataAPI.Where<DAOLine>(line => line.Code == lineNumber)
                 .Select(line =>
                 {
                     BOLine ret = new BOLine(line);
                     ret.Path = AllLineStations(ret);
                     return ret;
                 });
        }
        public IEnumerable<BOLine> GetAllLines()
        {
            return dataAPI.All<DAOLine>()
                .Select(line =>
                {
                    BOLine ret = new BOLine(line);
                    ret.Path = AllLineStations(ret);
                    return ret;
                });
        }
        public bool RemoveLine(int id)
        {
            DAOLine toRemove = new DAOLine { Id = id };
            return dataAPI.Remove(toRemove);
        }
        public async Task<int> AddLine(BOLine toAdd)
        {
            List<BOStation> stations = toAdd.Path.ToList();
            await AddAllStationsToDatabase(stations);

            DAOLine daoToAdd = new DAOLine
            {
                Area = toAdd.EnumArea,
                Code = toAdd.LineNumber,
                FirstStationId = toAdd.Path.First().Code,
                LastStationId = toAdd.Path.Last().Code,
            };
            int toAddId = 0;
            try
            {
                toAddId = dataAPI.Add(daoToAdd);
            }
            catch (Exception ex)
            {
                //currently there is no suspect exceptions from the data API.
                throw ex;
            }
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
            DAOStation toAddData;
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
            int pathLength = GetLinePathLength(lineId);
            if (index >= pathLength)
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
                DAOLineStation prev = dataAPI.Where<DAOLineStation>(s => s.LineId == lineId && s.Index == pathLength - 1).First();
                toAdd.PrevStationId = prev.StationId;
                toAdd.NextStationId = -1;
                toAdd.Index = pathLength;
                prev.NextStationId = toAdd.StationId;
                dataAPI.Update(prev);
                dataAPI.Add(toAdd);
                await UpdateNearStations(toAdd.PrevStationId, toAdd.StationId);
            }
            //In the case that we want to add the station as first station we need to have other implementation 
            else if (index == 0)
            {
                List<DAOLineStation> stationsToUpdate = dataAPI.Where<DAOLineStation>(s => s.LineId == lineId).OrderBy(s => s.Index).ToList();
                //Set the data of the station that we want to add
                toAdd.NextStationId = stationsToUpdate[0].StationId;
                toAdd.Index = 0;
                toAdd.PrevStationId = -1;
                foreach (DAOLineStation stat in stationsToUpdate)
                {
                    stat.Index++;
                    dataAPI.Update(stat);
                }
                dataAPI.Add(toAdd);
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
            int pathLength = GetLinePathLength(lineId);
            DAOLineStation stationToRemove = dataAPI.GetById<DAOLineStation>(stationCode);
            int index = stationToRemove.Index;
            if (index >= pathLength)
            {
                throw new IndexOutOfRangeException("The index is higher then the path length");
            }
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
            return GetLine(lineId);
        }
        public bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
        public IEnumerable<string> GetAllAreas()
        {
            return new string[5] { "General", "North", "South", "Center", "Jerusalem" };
        }

        public BOLineStation GetLineStationFromStationAndLine(int lineId, int stationId, out BOStation next, out BOStation prev, bool getFullLine = false)
        {
            DAOStation station = dataAPI.GetById<DAOStation>(stationId);
            DAOLineStation lineStation = dataAPI.Where<DAOLineStation>(s => s.StationId == stationId && s.LineId == lineId).First();
            DAOLine line = dataAPI.GetById<DAOLine>(lineId);
            BOLine lineToRet = null;
            next = null;
            prev = null;
            if (getFullLine)
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
                prev = GetStation(lineStation.PrevStationId);
            }
            if (lineStation == null || station == null || line == null || nextStationData == null)
            {
                throw new ItemNotFoundException("Cant find the line staation in the database.");
            }
            next = GetStation(lineStation.NextStationId);
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
            Simulator.Instance.StartSimulation(startTime, rate, updateTime);
        }
        public void StopSimulator()
        {
            Simulator.Instance.StopSimulation();
        }
        public void OnLineUpdate(EventHandler<LineDriveEventArgs> handler)
        {
            DrivesManager.Instance.onLineUpdate += handler;
        }
        public void OnLineFinish(EventHandler<LineDriveEventArgs> handler)
        {
            DrivesManager.Instance.onLineFinish += handler;
        }
        public void SetStationToTrack(int stationId)
        {
            DrivesManager.Instance.SetStationPanel(stationId);
        }
        public IEnumerable<BOYellowSign> AllLinesInStation(int stationId)
        {
            return dataAPI.Where<DAOLineStation>(s => s.StationId == stationId)
                          .Select(s => dataAPI.GetById<DAOLine>(s.LineId))
                          .Select(l => new BOYellowSign { LineNumber = l.Code, LastStationName = GetLastStation(l.Id).Name });
        }
        #endregion Implementation
        #region private and internal methods
        internal List<BOStation> AllLineStations(BOLine line)
        {

            var part1 = dataAPI.Where<DAOLineStation>(station => station.LineId == line.Id);
            var part2 = part1.Join(dataAPI.All<DAOStation>(),
                           s => s.StationId,
                           s => s.Id,
                           (lineStation, station) => new { station = new BOStation(station), nextStationId = lineStation.NextStationId, index = lineStation.Index });
            var part3 = part2.Join(dataAPI.All<DAOAdjacentStations>(),
                                s => s.station.Code,
                                s => s.FromStationId,
                                (stationData, adjacentStation) => new { stationData = stationData, adjacentStationData = adjacentStation });
            var part4 = part3.Where(data => data.stationData.nextStationId == data.adjacentStationData.ToStationId)
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
            var ret = part4;
            ////We need to add the last station
            var query = dataAPI.Where<DAOLineStation>(s => s.LineId == line.Id);
            int lastStationIndex = query.Max(x => x.Index);
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
        private async Task<double> GetDistanceBetweenStations(DAOStation from, DAOStation to)
        {
            Distance dist = new Distance();
            dist.FromLat = from.Latitude;
            dist.FromLon = from.Longitude;
            dist.ToLon = to.Longitude;
            dist.ToLat = to.Latitude;
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(dist.RequestURL))
            {
                string JSONText = await response.Content.ReadAsStringAsync();
                return dist.GetDistance(JSONText);
            }
        }
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
        private async Task UpdateNearStations(int fromId, int toId)
        {
            //Check if the stations data exists in the database
            if (dataAPI.Where<DAOAdjacentStations>(s => s.FromStationId == fromId && s.ToStationId == toId).Count() != 0)
            {
                return;
            }
            DAOStation from = dataAPI.GetById<DAOStation>(fromId);
            DAOStation to = dataAPI.GetById<DAOStation>(toId);
            double distance = 0;
            try
            {
                distance = await GetDistanceBetweenStations(from, to);
            }
            catch (Exception ex)
            {
                //currently no suspect exceptions
                throw ex;
            }
            TimeSpan time = TimeSpan.FromHours(distance / AVERAGE_SPEED);
            DAOAdjacentStations toAdd = new DAOAdjacentStations
            {
                Distance = distance,
                Time = time,
                FromStationId = fromId,
                ToStationId = toId
            };
            dataAPI.Add(toAdd);

        }
        private int GetLinePathLength(int lineId)
        {
            return dataAPI.Where<DAOLineStation>(station => station.LineId == lineId).Count();
        }
        private T Min<T>(List<T> lst, Func<T, int> comperer)
        {
            int minVal = lst.Min(t => comperer(t));
            return lst.First(t => comperer(t) == minVal);
        }

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
        internal DAOStation GetLastStation(int lineId)
        {
            return dataAPI.Where<DAOLineStation>(s => s.LineId == lineId && s.NextStationId == -1)
                          .Select(ls => dataAPI.GetById<DAOStation>(ls.StationId))
                          .First();
        }

        #endregion
    }
}
