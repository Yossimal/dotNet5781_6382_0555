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
    //public class DALObjectOld : IDAL
    //{
    //    #region singelton

    //    private static readonly DALObject instance;
    //    public static DALObject Instance => instance;
    //    private DALObject() { }
    //    #endregion

    //    #region global
    //    public void Add(DAOBasic toAdd)
    //    {
    //        List<DAOBasic> lst = GetTypeList(toAdd);
    //        if (lst.Exists(d => d.Id == toAdd.Id))
    //        {
    //            throw new InvalidOperationException($"There is already instance with id {toAdd.Id} in the list with Type {toAdd.GetType().Name}");
    //        }
    //        lst.Add(toAdd.Clone());
    //    }

    //    public void AddCollection(IEnumerable<DAOBasic> toAdd)
    //    {
    //        if (!toAdd.Any())
    //        {
    //            return;//There is no exception in adding an empty list
    //        }
    //        List<DAOBasic> lst = GetTypeList(toAdd.First());
    //        //check that all the id's in the given list are not exists in the id's of the data list
    //        if (lst.Any(x => toAdd.Clone().Any(d => d.Id == x.Id)))
    //        {
    //            throw new InvalidOperationException($"Can't add one or more of the objects in the given list because they have the same Id as other object in the list with Type {toAdd.First().GetType().Name}");
    //        }
    //        //check that there is no duplicate id in the given list
    //        if (toAdd.Count(d => toAdd.Any(d2 => d.Id == d2.Id)) > 1)
    //        {
    //            throw new InvalidOperationException($"The given collection of type {toAdd.First().GetType()} have duplicate Id's ");
    //        }
    //        lst.AddRange(toAdd.Clone());
    //    }

    //    public bool Remove(DAOBasic toRemove)
    //    {

    //        List<DAOBasic> lst = GetTypeList(toRemove);
    //        //Removing nothing dont need to throw an exception.
    //        if (!lst.Exists(d=>d.Id==toRemove.Id))
    //        {
    //            return false;
    //        }
    //        toRemove.IsDeleted = true;
    //        this.Update(toRemove);
    //        return true;
    //    }

    //    public void Update(DAOBasic toUpdate)
    //    {
    //        List<DAOBasic> lst = GetTypeList(toUpdate);
    //        int index = lst.FindIndex(d => d.Id == toUpdate.Id);
    //        if (index == -1)
    //        {
    //            throw new InvalidOperationException($"Cant find the object {toUpdate} in the list with the type {toUpdate.GetType().Name}");
    //        }

    //        lst[index] = toUpdate;
    //    }

    //    public DAOBasic GetById<DAOType>(int id) where DAOType : DAOBasic
    //    {
    //        List<DAOBasic> lst = Data.data[typeof(DAOType)];
    //        if (lst.Any(d => d.Id == id))
    //        {
    //            return lst.Find(d => d.Id == id);
    //        }
    //        return null;
    //    }


    //    public IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType:class
    //    {

    //        return Data.data[typeof(DAOType)].Where(condition).Select(x=>(DAOType)x);
    //    }

    //    public IEnumerable<object> All<DAOType>() 
    //    {
    //        return Data.data[typeof(DAOType)];
    //    }
    //    #endregion

    //    #region privateMethods

    //    private List<object> GetTypeList(object getBy)
    //    {
    //        Type type = getBy.GetType();
    //        return Data.data[type];
    //    }



    //    #endregion privateMethods

    //}

    public class DALObject : IDAL
    {
        #region singelton

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
                throw new InvalidOperationException($"There is already instance with id {toAdd.GetId()} in the list with Type {toAdd.GetType().Name}");
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
            removeFromLst[index].Delete();
            return true;

        }

        public void Update(object toUpdate)
        {
            Type toUpdateType = toUpdate.GetType();
            //If there is no object with the same type throw an exception
            if (!Data.Database.ContainsKey(toUpdateType))
            {
                throw new InvalidOperationException($"The list with the type {toUpdateType.Name} do not exists");
            }
            List<object> updateList = GetTypeList(toUpdate);
            //If we cant find the object id in the database
            if (!updateList.Exists(d => d.GetId() == toUpdate.GetId()))
            {
                throw new InvalidOperationException("Can't find an object with the same id as the given object id");
            }
            int index = updateList.FindIndex(d => (d.GetId() == toUpdate.GetId() && !d.IsDeleted()));
            updateList[index] = toUpdate;
        }

        public DAOType GetById<DAOType>(int id) where DAOType : class, new()
        {
            Type retrieveType = typeof(DAOType);
            if (Data.Database.ContainsKey(retrieveType))
            {
                return null;
            }

            object ret = Data.Database[retrieveType].FirstOrDefault(d => d.GetId() == id);
            return (ret as DAOType).Clone();
        }

        public IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType : class, new()
        {
            Type retType = typeof(DAOType);
            if (!Data.Database.ContainsKey(retType))
            {
                return new List<DAOType>();
            }

            return Data.Database[retType].Where(o =>
            {
                DAOType dat = o as DAOType;
                return condition(dat);
            }).Select(o => (o as DAOType).Clone());

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
        private List<object> GetTypeList(object getBy)
        {

            Type type = getBy.GetType();
            if (!Data.Database.ContainsKey(type))
            {
                Data.Database.Add(type, new List<object>());
            }
            return Data.Database[type];
        }
        #endregion PrivateMethods

    }
}
