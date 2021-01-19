using DALAPI;
using DALXML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DAL
{
    public class DALXML : IDAL
    {
        #region singelton
        /// <summary>
        /// singleton implementation
        /// </summary>
        private static readonly DALXML _instance;
        public static DALXML Instance => _instance;
        private DALXML() { }
        private DALXML(string runningIdsPath, string saveDirectory)
        {
            _files = new Dictionary<Type, string>();
            _runningIds = runningIdsPath;
            _saveDirectory = saveDirectory;
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
        }
         static DALXML() {
            _instance = new DALXML("runningIds.xml","xmlData");
        }
        #endregion
        #region private variables
        private Dictionary<Type, string> _files;
        private string _runningIds;
        private string _saveDirectory;
        #endregion


        public int Add(object toAdd)
        {
            Type toAddType = toAdd.GetType();
            XElement toAddXML = toAdd.ToXElement();
            XElement listToAdd = GetListByType(toAddType);
            if (toAdd.IsRunningId())
            {
                XElement runningIdXML = XElement.Load(_runningIds);
                IEnumerable<XElement> query = runningIdXML.Elements().Where(element => element.Name.LocalName == toAddType.Name);
                if (query.Count() == 0)
                {
                    int currentId = 0;
                    toAdd.SetId(currentId);
                    currentId++;
                    XElement runningIdElement = new XElement(toAddType.Name);
                    runningIdElement.Value = currentId.ToString();
                    runningIdXML.Add(runningIdElement);
                    runningIdXML.Save(_runningIds);
                }
                else
                {
                    int currentId = int.Parse(query.First().Value);
                    toAdd.SetId(currentId);
                    currentId++;
                    query.First().Value = currentId.ToString();
                    runningIdXML.Save(_runningIds);
                }
            }
            else
            {
                var checkForExists = listToAdd.Elements().Where(element =>
                {
                    XElement idElement = element.Elements()
                                                .Where(innerElement => innerElement.Name.LocalName == "Id")
                                                .First();
                    int id = int.Parse(idElement.Value);
                    return toAdd.GetId() == id;
                }).FirstOrDefault();
                if (checkForExists != null)
                {
                    throw new ItemAlreadyExistsException($"There is already an item with type {toAddType.Name} and Id {toAdd.GetId()} in the data storge.");
                }
            }
            listToAdd.Add(toAddXML);
            listToAdd.Save(_saveDirectory + '\\' + _files[toAddType]);
            return toAdd.GetId();
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
            XElement asXML = GetListByType(typeof(DAOType));
            IEnumerable<DAOType> ret = asXML.Elements()
                                            .Select(element => (element.ToDAO<DAOType>() as DAOType))
                                            .Where(obj => !obj.IsDeleted());
            return ret;
        }

        public DAOType GetById<DAOType>(int id) where DAOType : class, new()
        {
            return Where<DAOType>(dao => dao.GetId() == id).FirstOrDefault();

        }

        public bool Remove(object toRemove)
        {
            toRemove.Delete();
            Update(toRemove);

            return true;
        }

        public void Update(object toUpdate)
        {
            Type toUpdateType = toUpdate.GetType();
            XElement lst = GetListByType(toUpdateType);
            XElement oldXML = lst.Elements().Where(element =>
            {
                XElement idElement = element.Elements()
                                            .Where(innerElement => innerElement.Name.LocalName == "Id")
                                            .First();
                int id = int.Parse(idElement.Value);
                return toUpdate.GetId() == id;
            }).FirstOrDefault();
            if (oldXML == null)
            {
                throw new ItemNotFoundException($"Can't find the item with type {toUpdateType.Name} and Id {toUpdate.GetId()}");
            }
            oldXML.Remove();
            lst.Add(toUpdate.ToXElement());
            lst.Save(_saveDirectory + '\\' + _files[toUpdateType]);
        }

        public IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType : class, new()
        {
            return All<DAOType>().Where(dao => condition(dao));
        }

        #region private methods
        private XElement GetListByType(Type type)
        {
            if (!_files.ContainsKey(type))
            {
                string fileName = type.Name + "." + DateTime.Now.Millisecond + ".xml";
                string filePath = _saveDirectory + "\\" + fileName;
                //FileStream file = File.Create(filePath); file.Close();
                XElement root = new XElement("root");
                _files.Add(type, fileName);
                
                root.Save(filePath);
            }
            return XElement.Load(_saveDirectory + '\\' + _files[type]);
        }
        #endregion
    }
}
