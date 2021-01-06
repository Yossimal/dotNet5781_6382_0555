using DALAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALXML
{
    class DALXML : IDAL
    {
        private Dictionary<Type, string> _files;
        private string _runningIds;

        public DALXML(string runningIdsPath)
        {
            _files = new Dictionary<Type, string>();
            _runningIds = runningIdsPath;
        }

        public int Add(object toAdd)
        {
            throw new NotImplementedException();
        }

        public void AddCollection(IEnumerable<object> toAdd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DAOType> All<DAOType>() where DAOType : class, new()
        {
            throw new NotImplementedException();
        }

        public DAOType GetById<DAOType>(int id) where DAOType : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Remove(object toRemove)
        {
            throw new NotImplementedException();
        }

        public void Update(object toUpdate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
