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
    sealed public class DALXML : IDAL
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
        /// <summary>
        /// data of all the available files
        /// </summary>
        private Dictionary<Type, string> _files;
        /// <summary>
        /// the path of the running ids file
        /// </summary>
        private string _runningIds;
        /// <summary>
        /// the directory to save all the files
        /// </summary>
        private string _saveDirectory;
        /// <summary>
        ///the path of the metadata file in the _saveDirectory
        /// </summary>
        private const string METADATA_NAME = "metadata.xml";
        /// <summary>
        /// for random calculations
        /// </summary>
        private Random _rand = new Random(DateTime.Now.Millisecond);
        #endregion

        #region implementaion
        public int Add(object toAdd)
        {
            //the type of the object to add 
            Type toAddType = toAdd.GetType();
            
            XElement listToAdd = GetListByType(toAddType);
            if (toAdd.IsRunningId())//if we have running id
            {
                //load the file
                XElement runningIdXML = XElement.Load(_saveDirectory + '\\' + _runningIds);
                IEnumerable<XElement> query = runningIdXML.Elements().Where(element => element.Name.LocalName == toAddType.Name);
                if (query.Count() == 0)//if there is no running id fuild for that object ->create one
                {
                    int currentId = 0;
                    toAdd.SetId(currentId);
                    currentId++;
                    //save the data
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
                    runningIdXML.Save(_saveDirectory+'\\'+_runningIds);
                }
            }
            else
            {
                //check if the givent item have unique primary key
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
            //add the element to the file
            XElement toAddXML = toAdd.ToXElement();
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
            //remove the old data
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
            //add the new data
            lst.Add(toUpdate.ToXElement());
            lst.Save(_saveDirectory + '\\' + _files[toUpdateType]);
        }

        public IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType : class, new()
        {
            return All<DAOType>().Where(dao => condition(dao));
        }
        #endregion
        #region private methods
        /// <summary>
        /// get the XElement of a file by the type of the file in the _files
        /// </summary>
        /// <param name="type">typ</param>
        /// <returns>the XELment of the file</returns>
        private XElement GetListByType(Type type)
        {
            if (!_files.ContainsKey(type))//if the file do not exists ->create it
            {
                return CreateFile(type);
            }
            return XElement.Load(_saveDirectory + '\\' + _files[type]);
        }
        /// <summary>
        /// create new xml file and register it to the metadata
        /// </summary>
        /// <param name="type">the type of the file objects</param>
        /// <returns>XElement of teh file root</returns>
        private XElement CreateFile(Type type)
        {
            //generate random file name
            string fileName = $"{_rand.Next((int)Math.Pow(10, 8), (int)Math.Pow(10, 9))}.{_rand.Next((int)Math.Pow(10, 8), (int)Math.Pow(10, 9))}.xml";
            //set the file path
            string filePath = _saveDirectory + "\\" + fileName;
            XElement root = new XElement("root");
            _files.Add(type, fileName);
            root.Save(filePath);
            //save the metadata with the new file
            SaveMetadata();
            return root;
        }
        /// <summary>
        /// read the metadata file and initialize all the fileds in DALXML
        /// </summary>
        /// <param name="path">the path of teh metadata file</param>
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
        }
        /// <summary>
        /// save the metadata file according to the DALXML properties
        /// </summary>
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
            //Save the metadata file
            metadataXML.Save(_saveDirectory + '\\' + METADATA_NAME);
        }
        #endregion
    }
}
