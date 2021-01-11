using System;
using System.Collections.Generic;
using System.Text;
using DALAPI.DAO;

namespace DALAPI
{
    /// <summary>
    /// Data Access Layer interface
    /// </summary>
    public interface IDAL
    {
        int Add(object toAdd);
        void AddCollection(IEnumerable<object> toAdd);
        bool Remove(object toRemove);
        void Update(object toUpdate);
        DAOType GetById<DAOType>(int id) where DAOType:class,new();
        IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType:class,new();
        IEnumerable<DAOType> All<DAOType>() where DAOType:class,new();

    }
}
