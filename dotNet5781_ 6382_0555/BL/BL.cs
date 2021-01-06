using BL.BO;
using DALAPI;
using DALAPI.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

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
                throw new InvalidOperationException("There was problem to write the data", ex);
            }
        }
        public IEnumerable<BOBus> AllAvelibleBuses()
        {
            //Can be written in two lines with chaining methods LINQ but the course require the SQL style LINQ
            IEnumerable<BOBus> ret = from dataBus in dataAPI.All<DAOBus>()
                                     let bus = new BOBus(dataBus)
                                     where bus.Status == BusStatus.Ready
                                     select bus;
            return ret;
        }
        public IEnumerable<BOBus> AllBuses()
        {
            return dataAPI.All<DAOBus>()
                .Select(bus => new BOBus(bus));
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
                Longitude=toAdd.Longitude,
                Latitude=toAdd.Latitude
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
        #endregion Implementation
    }
}
