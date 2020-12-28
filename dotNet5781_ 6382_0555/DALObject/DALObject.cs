using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DALAPI;
using DALAPI.DAO;
using DS;

namespace DAL
{
    public class DALObject : IDAL
    {
        #region singelton

        private static readonly DALObject instance;
        public static DALObject Instance => instance;
        private DALObject() { }
        #endregion

        #region global
        public void Add(DAOBasic toAdd)
        {
            List<DAOBasic> lst = GetTypeList(toAdd);
            if (lst.Exists(d => d.Id == toAdd.Id))
            {
                throw new InvalidOperationException($"There is already instance with id {toAdd.Id} in the list with Type {toAdd.GetType().Name}");
            }
            lst.Add(toAdd.Clone());
        }

        public void AddCollection(IEnumerable<DAOBasic> toAdd)
        {
            if (!toAdd.Any())
            {
                return;//There is no exception in adding an empty list
            }
            List<DAOBasic> lst = GetTypeList(toAdd.First());
            if (lst.Any(x => toAdd.Clone().Any(d => d.Id == x.Id)))
            {
                throw new InvalidOperationException($"Can't add one or more of the objects in the given list because they have the same Id as other object in the list with Type {toAdd.First().GetType().Name}");
            }
            lst.AddRange(toAdd.Clone());
        }

        public bool Remove(DAOBasic toRemove)
        {
            List<DAOBasic> lst = GetTypeList(toRemove);
            return lst.Remove(toRemove);
        }

        public void Update(DAOBasic toUpdate)
        {
            List<DAOBasic> lst = GetTypeList(toUpdate);
            int index = lst.FindIndex(d => d.Id == toUpdate.Id);
            if (index == -1)
            {
                throw new InvalidOperationException($"Cant find the object {toUpdate} in the list with the type {toUpdate.GetType().Name}");
            }

            lst[index] = toUpdate;
        }

        public DAOBasic GetById<DAOType>(int id) where DAOType : DAOBasic
        {
            List<DAOBasic> lst = Data.data[typeof(DAOType)];
            if (lst.Any(d => d.Id == id))
            {
                return lst.Find(d => d.Id == id);
            }
            return null;
        }


        public IEnumerable<DAOBasic> Where<DAOType>(Func<DAOBasic, bool> condition) where DAOType : DAOBasic
        {
            return Data.data[typeof(DAOType)].Where(condition);
        }

        public IEnumerable<DAOBasic> All<DAOType>() where DAOType : DAOBasic
        {
            return Data.data[typeof(DAOType)];
        }
        #endregion

        #region privateMethods

        private List<DAOBasic> GetTypeList(DAOBasic getBy)
        {
            Type type = getBy.GetType();
            return Data.data[type];
        }
        #endregion privateMethods

    }
}
