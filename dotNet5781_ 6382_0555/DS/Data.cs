using System;
using System.Collections.Generic;
using DALAPI.DAO;
using DAL;
using System.Linq;

namespace DS
{
    public class Data
    {
        public static readonly Dictionary<Type, List<object>> Database = new Dictionary<Type, List<object>>();
        public static readonly Dictionary<Type, int> RunningId = new Dictionary<Type, int>();

        private static DAL.DALXML dalxml = DAL.DALXML.Instance;
        static Data()
        {
            InitializeData();
        }

        private static void InitializeData()
        {
            //initialize the users list
            Database.Add(typeof(DAOUser), new List<object>());
            Database[typeof(DAOUser)].AddRange(dalxml.All<DAOUser>().Select(dao => dao as object));
            //initialize the buses list
            Database.Add(typeof(DAOBus), new List<object>());
            Database[typeof(DAOBus)].AddRange(dalxml.All<DAOBus>().Select(dao => dao as object));
            //initialize the lines list
            Database.Add(typeof(DAOLine), new List<object>());
            Database[typeof(DAOLine)].AddRange(dalxml.All<DAOLine>().Select(dao => dao as object));
            //initialize the line stations list
            Database.Add(typeof(DAOLineStation), new List<object>());
            Database[typeof(DAOLineStation)].AddRange(dalxml.All<DAOLineStation>().Select(dao => dao as object));
            //initialize the line trip list
            Database.Add(typeof(DAOLineTrip), new List<object>());
            Database[typeof(DAOLineTrip)].AddRange(dalxml.All<DAOLineTrip>().Select(dao => dao as object));
            //initialize the stations list
            Database.Add(typeof(DAOStation), new List<object>());
            Database[typeof(DAOStation)].AddRange(dalxml.All<DAOStation>().Select(dao => dao as object));
            //initialize the adjacent stations list
            Database.Add(typeof(DAOAdjacentStations), new List<object>());
            Database[typeof(DAOAdjacentStations)].AddRange(dalxml.All<DAOAdjacentStations>().Select(dao => dao as object));
        }
        /// <summary>
        /// Data were written automaically (usind ts) and added 
        /// </summary>
        #region initializers
        private static void InitializeBuses()
        {
            Type busType = typeof(DAOBus);
            Add(busType, new DAOBus
            {
                Id = 12345678,
                FuelRemain = 32.4,
                LicenseDate = new DateTime(2018, 10, 3),
                IsDeleted = false,
                MileageCounter = 158,
                Status = DALAPI.BusStatus.Ready,
                LastCareDate = new DateTime(2019, 10, 3)
            });
            Add(busType, new DAOBus
            {
                Id = 74584125,
                FuelRemain = 25,
                LicenseDate = new DateTime(2019, 1, 25),
                IsDeleted = false,
                MileageCounter = 853,
                Status = DALAPI.BusStatus.Ready,
                LastCareDate = new DateTime(2020, 10, 3)
            });
            Add(busType, new DAOBus
            {
                Id = 7451258,
                FuelRemain = 32.4,
                LicenseDate = new DateTime(2010, 10, 3),
                IsDeleted = false,
                MileageCounter = 25,
                Status = DALAPI.BusStatus.Ready,
                LastCareDate = new DateTime(2018, 10, 3)
            });
            Add(busType, new DAOBus
            {
                Id = 95845621,
                FuelRemain = 32.4,
                LicenseDate = new DateTime(2020, 10, 3),
                IsDeleted = false,
                MileageCounter = 158,
                Status = DALAPI.BusStatus.Ready,
                LastCareDate = new DateTime(2020, 10, 5)
            });

        }
        private static void InitializeUsers()
        {
            Type userType = typeof(DAOUser);
            RunningId.Add(userType, 0);
            AddWithRunningId(userType, new DAOUser
            {
                UserName = "test",
                Password = "123",
                IsAdmin = false
            });
            AddWithRunningId(userType, new DAOUser
            {
                UserName = "a",
                Password = "1",
                IsAdmin = true
            });
            AddWithRunningId(userType, new DAOUser
            {
                UserName = "b",
                Password = "b",
                IsAdmin = true
            });
        }
        private static void InitializeStations()
        {
            Type stationType = typeof(DAOStation);
            Add(stationType, new DAOStation
            {
                Id = 111111,
                Longitude = 33.52,
                Latitude = 42.758,
                Name = "Station 1"
            });
            Add(stationType, new DAOStation
            {
                Id = 444444,
                Longitude = 31.52,
                Latitude = 42.758,
                Name = "Station 4"
            });
            Add(stationType, new DAOStation
            {
                Id = 222222,
                Longitude = 33.52547,
                Latitude = 42.7584125,
                Name = "Station 2"
            });
            Add(stationType, new DAOStation
            {
                Id = 333333,
                Longitude = 38.5454672,
                Latitude = 37.7582534,
                Name = "Station 3"
            });
            Add(stationType, new DAOStation
            {
                Id = 555555,
                Longitude = 31.52,
                Latitude = 42.758,
                Name = "Station 5"
            });
            Add(stationType, new DAOStation
            {
                Id = 666666,
                Longitude = 31.52,
                Latitude = 42.758,
                Name = "Station 6"
            });
            Add(stationType, new DAOStation
            {
                Id = 777777,
                Longitude = 31.52,
                Latitude = 42.758,
                Name = "Station 7"
            });
            Add(stationType, new DAOStation
            {
                Id = 888888,
                Longitude = 31.52,
                Latitude = 42.758,
                Name = "Station 8"
            });
            Add(stationType, new DAOStation
            {
                Id = 999999,
                Longitude = 31.52,
                Latitude = 42.758,
                Name = "Station 9"
            });
        }
        private static void InitializeLines()
        {
            Type lineType = typeof(DAOLine);
            AddWithRunningId(lineType, new DAOLine
            {
                Area = DALAPI.Area.General,
                Code = 15,
                FirstStationId = 111111,
                LastStationId = 999999
            });
            AddWithRunningId(lineType, new DAOLine
            {
                Area = DALAPI.Area.General,
                Code = 15,
                FirstStationId = 999999,
                LastStationId = 111111,
            });
            AddWithRunningId(lineType, new DAOLine
            {
                Area = DALAPI.Area.General,
                Code = 18,
                FirstStationId = 444444,
                LastStationId = 666666
            });
            AddWithRunningId(lineType, new DAOLine
            {
                Area = DALAPI.Area.General,
                Code = 26,
                FirstStationId = 222222,
                LastStationId = 777777
            });
        }
        private static void InitializeLineStations()
        {
            Type lineStationType = typeof(DAOLineStation);
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 0,
                LineId = 0,
                NextStationId = 222222,
                StationId = 111111,
                PrevStationId = -1
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 1,
                LineId = 0,
                NextStationId = 555555,
                StationId = 222222,
                PrevStationId = 111111
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 2,
                LineId = 0,
                NextStationId = 999999,
                StationId = 555555,
                PrevStationId = 222222
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 3,
                LineId = 0,
                NextStationId = -1,
                StationId = 999999,
                PrevStationId = 555555
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 0,
                LineId = 1,
                NextStationId = 333333,
                StationId = 999999,
                PrevStationId = -1
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 1,
                LineId = 1,
                NextStationId = 111111,
                StationId = 333333,
                PrevStationId = 999999
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 2,
                LineId = 1,
                NextStationId = -1,
                StationId = 999999,
                PrevStationId = 333333
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 0,
                LineId = 2,
                NextStationId = 555555,
                StationId = 444444,
                PrevStationId = -1
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 1,
                LineId = 2,
                NextStationId = 666666,
                StationId = 555555,
                PrevStationId = 444444
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 2,
                LineId = 2,
                NextStationId = -1,
                StationId = 666666,
                PrevStationId = 555555
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 0,
                LineId = 3,
                NextStationId = 888888,
                StationId = 222222,
                PrevStationId = -1
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 1,
                LineId = 3,
                NextStationId = 777777,
                StationId = 888888,
                PrevStationId = 222222
            });
            AddWithRunningId(lineStationType, new DAOLineStation
            {
                Index = 2,
                LineId = 3,
                NextStationId = -1,
                StationId = 777777,
                PrevStationId = 888888
            });
        }
        //Auto generated code
        private static void InitializeAdjucentStations()
        {
            Type adjacentStationsType = typeof(DAOAdjacentStations);
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 222222,
                Distance = 26.736635759484173,
                Time = TimeSpan.FromHours(1.659685203035954)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 333333,
                Distance = 38.46798768036785,
                Time = TimeSpan.FromHours(0.28672959033213385)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 444444,
                Distance = 37.59782315663424,
                Time = TimeSpan.FromHours(0.6836116545275693)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 555555,
                Distance = 33.02845719520455,
                Time = TimeSpan.FromHours(1.0103454615085612)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 666666,
                Distance = 38.6398058702937,
                Time = TimeSpan.FromHours(1.8303542448346144)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 777777,
                Distance = 34.32496009472905,
                Time = TimeSpan.FromHours(0.8045499609111448)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 888888,
                Distance = 25.04436882416193,
                Time = TimeSpan.FromHours(1.9407589907514184)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 111111,
                FromStationId = 999999,
                Distance = 34.198503519857184,
                Time = TimeSpan.FromHours(0.8839294253644574)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 111111,
                Distance = 24.03341508792208,
                Time = TimeSpan.FromHours(0.8061719458025673)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 333333,
                Distance = 31.971492736288507,
                Time = TimeSpan.FromHours(0.9105954164826486)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 444444,
                Distance = 24.173547712573114,
                Time = TimeSpan.FromHours(0.3593157646429659)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 555555,
                Distance = 34.091309528924654,
                Time = TimeSpan.FromHours(1.82838395688801)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 666666,
                Distance = 36.19026807099278,
                Time = TimeSpan.FromHours(1.4206966086253852)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 777777,
                Distance = 25.093300673833458,
                Time = TimeSpan.FromHours(0.8397327520907985)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 888888,
                Distance = 31.826015828019045,
                Time = TimeSpan.FromHours(1.6260067286703372)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 222222,
                FromStationId = 999999,
                Distance = 30.3891226036311,
                Time = TimeSpan.FromHours(1.5407933582239257)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 111111,
                Distance = 30.49849208599655,
                Time = TimeSpan.FromHours(1.3680429359209585)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 222222,
                Distance = 36.38909326609901,
                Time = TimeSpan.FromHours(1.059196102363242)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 444444,
                Distance = 28.183984190280512,
                Time = TimeSpan.FromHours(1.2489940350760598)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 555555,
                Distance = 24.854700004886723,
                Time = TimeSpan.FromHours(1.7758935605000579)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 666666,
                Distance = 28.718559011904187,
                Time = TimeSpan.FromHours(0.6288259339920157)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 777777,
                Distance = 34.60604408351492,
                Time = TimeSpan.FromHours(0.9707857437726155)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 888888,
                Distance = 27.76211906402363,
                Time = TimeSpan.FromHours(1.2484634036783837)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 333333,
                FromStationId = 999999,
                Distance = 27.189809477436032,
                Time = TimeSpan.FromHours(1.8883587838341196)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 111111,
                Distance = 39.58705505990561,
                Time = TimeSpan.FromHours(0.4537951622397872)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 222222,
                Distance = 31.23133496774287,
                Time = TimeSpan.FromHours(0.28607909447032714)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 333333,
                Distance = 30.947251467856635,
                Time = TimeSpan.FromHours(0.554559642513641)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 555555,
                Distance = 34.435439172256146,
                Time = TimeSpan.FromHours(0.51121642274494)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 666666,
                Distance = 20.862058694966407,
                Time = TimeSpan.FromHours(1.0336671084823863)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 777777,
                Distance = 31.34687405681668,
                Time = TimeSpan.FromHours(1.3733877490909296)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 888888,
                Distance = 27.448390503783497,
                Time = TimeSpan.FromHours(0.6605938071309612)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 444444,
                FromStationId = 999999,
                Distance = 32.77218518475786,
                Time = TimeSpan.FromHours(1.7914350753702337)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 111111,
                Distance = 35.04856412641642,
                Time = TimeSpan.FromHours(1.593644625152059)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 222222,
                Distance = 33.43134803006191,
                Time = TimeSpan.FromHours(1.7929106244739939)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 333333,
                Distance = 28.40731176529467,
                Time = TimeSpan.FromHours(1.5219797579249539)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 444444,
                Distance = 26.473695088777554,
                Time = TimeSpan.FromHours(0.4815441367640916)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 666666,
                Distance = 32.9426142525998,
                Time = TimeSpan.FromHours(1.9554453868728972)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 777777,
                Distance = 38.842295829009885,
                Time = TimeSpan.FromHours(1.3853277356080764)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 888888,
                Distance = 33.44124784425546,
                Time = TimeSpan.FromHours(0.7892232434805571)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 555555,
                FromStationId = 999999,
                Distance = 26.70167366074649,
                Time = TimeSpan.FromHours(1.4440237147927626)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 111111,
                Distance = 22.667107051472474,
                Time = TimeSpan.FromHours(1.377895137635535)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 222222,
                Distance = 21.57443658918371,
                Time = TimeSpan.FromHours(1.0492167270343307)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 333333,
                Distance = 29.88256948647503,
                Time = TimeSpan.FromHours(1.6097765266877881)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 444444,
                Distance = 35.10611407126761,
                Time = TimeSpan.FromHours(1.7407344202371096)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 555555,
                Distance = 37.12045372811584,
                Time = TimeSpan.FromHours(0.7054960376488699)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 777777,
                Distance = 37.342441689515056,
                Time = TimeSpan.FromHours(1.0418233002405575)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 888888,
                Distance = 24.09036545175544,
                Time = TimeSpan.FromHours(1.1001926395181179)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 666666,
                FromStationId = 999999,
                Distance = 30.799439534586778,
                Time = TimeSpan.FromHours(0.39990720235903465)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 111111,
                Distance = 38.61246459567688,
                Time = TimeSpan.FromHours(0.6696667482703502)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 222222,
                Distance = 25.97292393930221,
                Time = TimeSpan.FromHours(0.9464725860522722)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 333333,
                Distance = 35.94880560861269,
                Time = TimeSpan.FromHours(0.6885649918634942)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 444444,
                Distance = 32.27430839343022,
                Time = TimeSpan.FromHours(1.9386529840856108)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 555555,
                Distance = 25.980365638037476,
                Time = TimeSpan.FromHours(1.715832276582519)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 666666,
                Distance = 35.841394233172466,
                Time = TimeSpan.FromHours(0.6916429786610092)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 888888,
                Distance = 30.05188717985128,
                Time = TimeSpan.FromHours(1.8905564753749817)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 777777,
                FromStationId = 999999,
                Distance = 29.574027157263206,
                Time = TimeSpan.FromHours(1.5576898496233178)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 111111,
                Distance = 33.61783286401551,
                Time = TimeSpan.FromHours(0.7734260847312544)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 222222,
                Distance = 34.498522936759336,
                Time = TimeSpan.FromHours(1.8176102279478117)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 333333,
                Distance = 35.390616910460764,
                Time = TimeSpan.FromHours(1.5293396145082256)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 444444,
                Distance = 23.486088117904906,
                Time = TimeSpan.FromHours(0.3056667226672057)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 555555,
                Distance = 26.60495153697535,
                Time = TimeSpan.FromHours(1.3150957329164665)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 666666,
                Distance = 32.296918253216646,
                Time = TimeSpan.FromHours(0.29598511467415833)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 777777,
                Distance = 34.59087172534659,
                Time = TimeSpan.FromHours(0.3202143872538064)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 888888,
                FromStationId = 999999,
                Distance = 20.25358834206168,
                Time = TimeSpan.FromHours(1.112557368297555)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 111111,
                Distance = 35.85709137001999,
                Time = TimeSpan.FromHours(1.8753436618418216)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 222222,
                Distance = 38.98370227078816,
                Time = TimeSpan.FromHours(0.3303557991780428)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 333333,
                Distance = 24.14871653007653,
                Time = TimeSpan.FromHours(1.9023688437404738)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 444444,
                Distance = 31.202178675629163,
                Time = TimeSpan.FromHours(1.995132650781269)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 555555,
                Distance = 25.97896155589776,
                Time = TimeSpan.FromHours(1.6946990402669517)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 666666,
                Distance = 24.07358806782466,
                Time = TimeSpan.FromHours(1.8030977515311204)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 777777,
                Distance = 35.89252021831736,
                Time = TimeSpan.FromHours(1.8823462492402088)
            });
            AddWithRunningId(adjacentStationsType, new DAOAdjacentStations
            {
                ToStationId = 999999,
                FromStationId = 888888,
                Distance = 35.299999490375214,
                Time = TimeSpan.FromHours(0.6708095283822244)
            });
        }
        #endregion
        private static void AddWithRunningId(Type type, object toAdd)
        {
            if (!Database.ContainsKey(type))
            {
                Database.Add(type, new List<object>());
            }
            if (!RunningId.ContainsKey(type))
            {
                RunningId.Add(type, 0);
            }
            toAdd.SetId(RunningId[type]);
            RunningId[type]++;
            Database[type].Add(toAdd);
        }
        private static void Add(Type type, object toAdd)
        {
            if (!Database.ContainsKey(type))
            {
                Database.Add(type, new List<object>());
            }
            Database[type].Add(toAdd);
        }

    }
}
