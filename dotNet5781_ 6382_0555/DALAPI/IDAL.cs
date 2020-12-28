using System;
using System.Collections.Generic;
using System.Text;
using DALAPI.DAO;

namespace DALAPI
{
    public interface IDAL
    {
        void Add(DAOBasic toAdd);
        void AddCollection(IEnumerable<DAOBasic> toAdd);
        bool Remove(DAOBasic toRemove);
        void Update(DAOBasic toUpdate);
        DAOBasic GetById<DAOType>(int id) where DAOType:DAOBasic;
        IEnumerable<DAOBasic> Where<DAOType>(Func<DAOBasic, bool> condition) where DAOType:DAOBasic;
        IEnumerable<DAOBasic> All<DAOType>() where DAOType:DAOBasic;

    }
}
