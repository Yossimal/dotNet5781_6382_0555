using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DALAPI;
using DALAPI.DAO;
using DALObject;
using DS;

namespace DAL
{

    sealed public class DALObject : IDAL
    {
        #region singelton
        /// <summary>
        /// singleton implementation
        /// </summary>
        private static readonly DALObject _instance = new DALObject();
        public static DALObject Instance => _instance;
        private DALObject() { }
        #endregion

        #region Implementation 
        public int Add(object toAdd)
        {
            //The type of the object that we want to add
            Type toAddType = toAdd.GetType();
            //Get the list that we want to add the object to
            List<object> lst = GetTypeList(toAdd);
            //Set the running id logic
            if (toAdd.IsRunningId())
            {
                if (!Data.RunningId.ContainsKey(toAddType))
                {
                    Data.RunningId.Add(toAddType, 0);
                }
                toAdd.SetId(Data.RunningId[toAddType]);
                Data.RunningId[toAddType]++;
            }
            //else check if the id is unique
            else if (lst.Exists(d => d.GetId() == toAdd.GetId() && !d.IsDeleted()))
            {
                throw new ItemAlreadyExistsException($"There is already instance with id {toAdd.GetId()} in the list with Type {toAdd.GetType().Name}");
            }
            else if (lst.Exists(d => d.GetId() == toAdd.GetId() && d.IsDeleted()))
            {
                this.Update(toAdd);
                return toAdd.GetId();
            }
            lst.Add(toAdd.Clone());
            //return the id of the object that we've added
            return toAdd.GetId();
        }

        public void AddCollection(IEnumerable<object> toAdd)
        {
            foreach (object obj in toAdd)
            {
                Add(obj);
            }
        }

        public bool Remove(object toRemove)
        {
            Type type = toRemove.GetType();
            //trying to remove object that not exists in the database don't need to throw an exception.
            if (!Data.Database.ContainsKey(type))
            {
                return false;
            }

            List<object> removeFromLst = GetTypeList(toRemove);
            if (!removeFromLst.Exists(d => (d.GetId() == toRemove.GetId() && !d.IsDeleted())))
            {
                return false;
            }

            int index = removeFromLst.FindIndex(d => (d.GetId() == toRemove.GetId() && !d.IsDeleted()));
            removeFromLst[index].Delete();//delete the object using the IsDeleted property
            return true;

        }

        public void Update(object toUpdate)
        {
            Type toUpdateType = toUpdate.GetType();
            //If there is no object with the same type throw an exception
            if (!Data.Database.ContainsKey(toUpdateType))
            {
                throw new ItemNotFoundException($"The list with the type {toUpdateType.Name} do not exists");
            }
            List<object> updateList = GetTypeList(toUpdate);
            //If we cant find the object id in the database
            if (!updateList.Exists(d => d.GetId() == toUpdate.GetId()))
            {
                throw new ItemNotFoundException("Can't find an object with the same id as the given object id");
            }
            int index = updateList.FindIndex(d => (d.GetId() == toUpdate.GetId() && !d.IsDeleted()));
            updateList[index] = toUpdate.Clone();
        }

        public DAOType GetById<DAOType>(int id) where DAOType : class, new()
        {
            Type retrieveType = typeof(DAOType);
            if (!Data.Database.ContainsKey(retrieveType))
            {
                return null;
            }
            //get the object by his id
            object ret = Data.Database[retrieveType].FirstOrDefault(d => (d.GetId() == id&&!d.IsDeleted()));
            if (ret == null)
            {
                return null;
            }
            return (ret as DAOType).Clone();
        }

        public IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType : class, new()
        {
            Type retType = typeof(DAOType);
            bool test = Data.Database.ContainsKey(retType);
            if (!test)
            {
                return new List<DAOType>();
            }

            var temp = Data.Database[retType].Where(o =>
            {
                DAOType dat = o as DAOType;
                return condition(dat) && !o.IsDeleted();
            });
            return temp.Select(o => (o as DAOType).Clone());//clone all the list

        }

        public IEnumerable<DAOType> All<DAOType>() where DAOType : class, new()
        {
            Type retType = typeof(DAOType);
            if (!Data.Database.ContainsKey(retType))
            {
                return new List<DAOType>();
            }

            return Where<DAOType>(d => true);
        }
        #endregion Implementation
        #region PrivateMethods
        /// <summary>
        /// get the list from ds with the given object type
        /// </summary>
        /// <param name="getBy"></param>
        /// <returns></returns>
        private List<object> GetTypeList(object getBy)
        {

            Type type = getBy.GetType();//get the object type
            //if the list not exists in the dictionary ->create it
            if (!Data.Database.ContainsKey(type))
            {
                Data.Database.Add(type, new List<object>());
            }
            //return the list
            return Data.Database[type];
        }
        #endregion PrivateMethods

    }
}
