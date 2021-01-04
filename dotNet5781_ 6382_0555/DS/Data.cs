using System;
using System.Collections.Generic;
using DALAPI.DAO;

namespace DS
{
    public class Data
    {
        public static readonly Dictionary<Type, List<object>> Database = new Dictionary<Type, List<object>>();
        public static readonly Dictionary<Type, int> RunningId = new Dictionary<Type, int>();

        static Data()
        {
            InitializeUsers();
            InitializeBuses();
        }

        private static void InitializeBuses()
        {
            Type busType = typeof(DAOBus);
            Database.Add(busType,new List<object>());
            Add(busType, new DAOBus
            {
                Id = 12345678,
                FuelRemain = 32.4,
                LicenseDate = new DateTime(2018, 10, 3),
                IsDeleted = false,
                MileageCounter = 158,
                Status = DALAPI.BusStatus.Ready,
                LastCareDate=new DateTime(2019,10,3)
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
            Database.Add(userType, new List<object>());
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
                UserName="b",
                Password="b",
                IsAdmin=true
            });
        }
        private static void InitializeStations() {
            Type stationType = typeof(DAOStation);
            Database.Add(stationType, new List<object>());

        }
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
        private static void Add(Type type, object toAdd) {
            if (!Database.ContainsKey(type))
            {
                Database.Add(type, new List<object>());
            }
            Database[type].Add(toAdd);
        }

    }
}
