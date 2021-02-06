using BL.BO;
using BL.Simulation.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        #region User managment methods
        /// <summary>
        /// Check if the given username and password exists in the database
        /// </summary>
        /// <param name="user">the user data</param>
        /// <returns>if the user exists => the user data with the id, else =>throw an exception </returns>
        /// <exception cref="BadLoginDataException">the username or password are incorrect</exception>
        BOUser CheckUserName(BOUser user);
        /// <summary>
        /// register user
        /// </summary>
        /// <param name="register">the user to register</param>
        /// <returns>the user id in the database</returns>
        /// <exception cref="BadManagerCodeException">the given manager code is incorrect</exception>
        /// <exception cref="BadPasswordException">the given password is illigal</exception>
        /// <exception cref="BadUsernameException">the given username is illigal</exception>
        /// <exception cref="ItemAlreadyExistsException">thre is already an user with that username</exception>
        int Register(BORegister register);
        #endregion
        #region bus managment methods
        /// <summary>
        /// get all the available buses
        /// </summary>
        /// <returns>all the available buses from the IDAL</returns>
        IEnumerable<BOBus> AllAvailableBuses();
        /// <summary>
        /// get all the buses
        /// </summary>
        /// <returns>all the buses from the IDAL</returns>
        IEnumerable<BOBus> AllBuses();
        /// <summary>
        /// refuel a bus
        /// </summary>
        /// <param name="licenseNumber">the bus license  number</param>
        /// <returns>the bus that we've sended to refuel after the refuel method</returns>
        /// <exception cref="ItemNotFoundException">the given bus dont exists in the database</exception>
        BOBus RefuelBus(int licenseNumber);
        /// <summary>
        /// care a bus
        /// </summary>
        /// <param name="licenseNumber">the bus license  number</param>
        /// <returns>the bus that we've sended to care after the care method</returns>
        /// <exception cref="ItemNotFoundException">the given bus dont exists in the database</exception>
        BOBus CareBus(int licenseNumber);
        /// <summary>
        /// add a bus to the database
        /// </summary>
        /// <param name="toAdd">the bus to add</param>
        /// <returns>the bus id</returns>
        /// <exception cref="ItemAlreadyExistsException">when there is already a bus tith that license number in the database</exception>
        int AddBus(BOBus toAdd);
        /// <summary>
        /// remove bus from the database
        /// </summary>
        /// <param name="licenseNumber">the bus license number</param>
        /// <returns>did the bus has been removed</returns>
        bool DeleteBus(int licenseNumber);
        /// <summary>
        /// get a bus from the database
        /// </summary>
        /// <param name="licenseNumber">the bus license number</param>
        /// <returns>the bus with the given license number</returns>
        /// <exception cref="ItemNotFoundException">cant find the bus in the database</exception>
        BOBus GetBus(int licenseNumber);
        #endregion
        #region stations managment methods
        /// <summary>
        /// get all the stations from the database
        /// </summary>
        /// <returns>all the stations</returns>
        IEnumerable<BOStation> AllStations();
        /// <summary>
        /// get a station data with specific code
        /// </summary>
        /// <param name="code">the station code</param>
        /// <returns>the station data</returns>
        BOStation GetStation(int code);
        /// <summary>
        /// remove a station
        /// </summary>
        /// <param name="code">the station code</param>
        /// <returns>true, if the station has been removed</returns>
        /// <exception cref="InnerItemNotFoundException">cant find the station in the database</exception>
        bool DeleteStation(int code);
        /// <summary>
        /// add a station to the database
        /// </summary>
        /// <param name="toAdd">the station to add</param>
        /// <returns>the station id</returns>
        ///<exception cref="ItemAlreadyExistsException">there is already a station with that code in the database</exception>
        int AddStation(BOAddStation toAdd);
        /// <summary>
        /// update the data of two near stations
        /// </summary>
        /// <param name="fromCode">the first station code</param>
        /// <param name="toCode">the second station code</param>
        /// <param name="distance">the distance between those stations</param>
        /// <param name="time">the travel time between thode stations</param>
        void UpdateNearStations(int fromCode, int toCode, double distance = -1, TimeSpan? time = null);
        #endregion
        #region LinesManagment methods
        /// <summary>
        /// get a line with his id
        /// </summary>
        /// <param name="id">the line id (not line number)</param>
        /// <returns>the requested line</returns>
        /// <exception cref="ItemNotFoundException">the line not exists in the database</exception>
        BOLine GetLine(int id);
        /// <summary>
        /// get all the lines with a given number
        /// </summary>
        /// <param name="lineNumber">the lines number</param>
        /// <returns>all the lines with the given number</returns>
        IEnumerable<BOLine> GetAllLines(int lineNumber);
        /// <summary>
        /// get all the lines in the database
        /// </summary>
        /// <returns>all the lines in the database</returns>
        IEnumerable<BOLine> GetAllLines();
        /// <summary>
        /// remove a line from the database
        /// </summary>
        /// <param name="id">the line to remove id</param>
        /// <returns>true, if the line has been removed</returns>
        bool RemoveLine(int id);
        /// <summary>
        /// add line to the database
        /// </summary>
        /// <param name="toAdd">the line to add</param>
        /// <returns>the line id</returns>
        Task<int> AddLine(BOLine toAdd);
        /// <summary>
        /// add station to line
        /// </summary>
        /// <param name="stationCode">the station code</param>
        /// <param name="lineId">the line id</param>
        /// <param name="index">where to add the station in the line (by index, starts from 0)</param>
        /// <returns>the new line</returns>
        Task<BOLine> AddStationToLine(int stationCode, int lineId, int index = -1);
        /// <summary>
        /// remove station from line
        /// </summary>
        /// <param name="lineId">the id of the line that we wnat to remove the sattion from</param>
        /// <param name="stationCode">the station to remove code</param>
        /// <returns>the new line</returns>
        Task<BOLine> RemoveStationFromLine(int lineId, int stationCode);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllAreas();
        BOLineStation GetLineStationFromStationAndLine(int lineId, int stationId, out BOStation next, out BOStation prev, bool fullLine = false);
        IEnumerable<BOYellowSign> AllLinesInStation(int stationId);
        #endregion
        #region Simulation methods
        void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime);
        void StopSimulator();
        void OnLineUpdate(EventHandler<LineDriveEventArgs> handler);
        void OnLineFinish(EventHandler<LineDriveEventArgs> handler);
        void SetStationToTrack(int stationId);
        #endregion
        #region other methods
        bool IsInternetAvailable();
        #endregion
    }
}
