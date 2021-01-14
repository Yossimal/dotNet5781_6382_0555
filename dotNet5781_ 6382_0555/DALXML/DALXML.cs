using DALAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DALXML
{
    class DALXML : IDAL
    {
        private Dictionary<Type, string> _files;
        private string _runningIds;
        private string _saveDirectory;

        public DALXML(string runningIdsPath,string saveDirectory)
        {
            _files = new Dictionary<Type, string>();
            _runningIds = runningIdsPath;
            _saveDirectory = saveDirectory;
            if (!Directory.Exists(saveDirectory)) {
                Directory.CreateDirectory(saveDirectory);
            }
        }

        public int Add(object toAdd)
        {
            Type toAddType = toAdd.GetType();
            if (!_files.ContainsKey(toAddType))
            {
                File.Create(_saveDirectory + "\\" + toAddType.Name + "." + DateTime.Now.Millisecond + ".xml");
            }
            XElement xmlElement =  toAdd.ToXElement();
            XmlReader reader = XmlReader.Create(_files[toAddType]);

            throw new NotImplementedException();

        }

        public void AddCollection(IEnumerable<object> toAdd)
        {
            foreach (object obj in toAdd)
            {
                Add(obj);
            }
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
