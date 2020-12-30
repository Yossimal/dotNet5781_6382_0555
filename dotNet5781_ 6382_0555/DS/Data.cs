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

    }
}
