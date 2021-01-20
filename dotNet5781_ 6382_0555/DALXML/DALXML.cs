using DALAPI;
using DALXML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DALXML.XMLSchemas;

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
            _saveDirectory = saveDirectory;

            if (File.Exists(saveDirectory + '\\' + METADATA_NAME))
            {
                ReadMetadata(saveDirectory + '\\' + METADATA_NAME);
            }
            else
            {
                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }
                _runningIds = runningIdsPath;
                if (!File.Exists(saveDirectory + '\\' + _runningIds))
                {
                    XElement runningIds = new XElement("root");
                    runningIds.Save(saveDirectory + '\\' + _runningIds);
                }
                SaveMetadata();
            }

        }
        static DALXML()
        {
            _instance = new DALXML("runningIds.xml", "xmlData");
        }
        #endregion
        #region private variables
        private Dictionary<Type, string> _files;
        private string _runningIds;
        private string _saveDirectory;
        private const string METADATA_NAME = "metadata.xml";
        #endregion

        #region implementaion
        public int Add(object toAdd)
        {
            Type toAddType = toAdd.GetType();
            XElement toAddXML = toAdd.ToXElement();
            XElement listToAdd = GetListByType(toAddType);
            if (toAdd.IsRunningId())
            {
                XElement runningIdXML = XElement.Load(_saveDirectory + '\\' + _runningIds);
                IEnumerable<XElement> query = runningIdXML.Elements().Where(element => element.Name.LocalName == toAddType.Name);
                if (query.Count() == 0)
                {
                    int currentId = 0;
                    toAdd.SetId(currentId);
                    currentId++;
                    XElement runningIdElement = new XElement(toAddType.Name);
                    runningIdElement.Value = currentId.ToString();
                    runningIdXML.Add(runningIdElement);
                    runningIdXML.Save(_saveDirectory + '\\' + _runningIds);
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
        #endregion
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
                SaveMetadata();
            }
            return XElement.Load(_saveDirectory + '\\' + _files[type]);
        }
        private void ReadMetadata(string path)
        {
            XElement metadataXML = XElement.Load(path);
            //read the xml files from the metadata
            XElement files = metadataXML.Elements()
                                        .First(element => element.Name.LocalName == "files");
            foreach (XElement file in files.Elements())
            {
                MetadataTypeFile typeFile = MetadataTypeFile.Deserialize(file);
                KeyValuePair<Type, string> pairToAdd = typeFile.GetType();
                _files.Add(pairToAdd.Key, pairToAdd.Value);
            }
            //read the running ids file path from the metadata
            _runningIds = metadataXML.Elements()
                                           .First(element => element.Name.LocalName == "running-ids-path")
                                           .Value;
            _runningIds = _saveDirectory + '\\' + _runningIds;
        }
        private void SaveMetadata()
        {
            XElement metadataXML = new XElement("root");
            //Add the files list to the metadata
            XElement files = new XElement("files");
            foreach (KeyValuePair<Type, string> pair in _files)
            {
                MetadataTypeFile fileType = MetadataTypeFile.GetMetadataTypeFile(pair);
                XElement fileTypeXML = fileType.Serialize();
                files.Add(fileTypeXML);
            }
            metadataXML.Add(files);
            //add the running ids file path to the metadata
            XElement runningIds = new XElement("running-ids-path");
            runningIds.Value = _runningIds;
            metadataXML.Add(runningIds);
            metadataXML.Save(_saveDirectory + '\\' + METADATA_NAME);
        }
        #endregion
    }
}
