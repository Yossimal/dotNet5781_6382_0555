using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    internal static class DAOToXML
    {
        /// <summary>
        /// convert XElement to DAO object
        /// </summary>
        /// <typeparam name="DAOType">the type of the DAO object</typeparam>
        /// <param name="xml">the XElemnt to convert from</param>
        /// <returns>the converted object</returns>
        public static object ToDAO<DAOType>(this XElement xml) where DAOType : class, new()
        {
            //create instance of the object to retuern
            object ret = Activator.CreateInstance(typeof(DAOType));
            Type retType = typeof(DAOType);
            //read all the data from the xml 
            foreach (XElement element in xml.Elements())
            {
                PropertyInfo objectProp = retType.GetProperty(element.Name.LocalName);
                if (objectProp.CanWrite)
                {
                   objectProp.SetValue(ret, StringToType(objectProp, element.Value));
                }
            }

            return ret;
        }
        /// <summary>
        /// convert DAO object to XElement
        /// </summary>
        /// <param name="dao">the object to convert</param>
        /// <returns>the converted XElement</returns>
        public static XElement ToXElement(this object dao)
        {
            Type daoType = dao.GetType();
            XElement ret = new XElement(daoType.Name);
            //read all teh data and write it to XML
            foreach (PropertyInfo prop in daoType.GetProperties())
            {
                XElement propElement = new XElement(prop.Name);
                propElement.Value = TypeToString(prop.PropertyType, prop.GetValue(dao));
                ret.Add(propElement);
            }
            return ret;
        }
        /// <summary>
        /// convert string to object
        /// </summary>
        /// <param name="prop">the property info that we want to convert</param>
        /// <param name="data">the string to convert</param>
        /// <returns>the converted object</returns>
        private static object StringToType(PropertyInfo prop, string data)
        {
            Type dataType = prop.PropertyType;
            if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
            {
                dataType = Nullable.GetUnderlyingType(dataType);
            }
            if (data == "NuN")
            {
                return null;
            }
            else if (dataType == typeof(DateTime))
            {
                return DateTime.Parse(data);
            }
            else if (dataType == typeof(int))
            {
                return int.Parse(data);
            }
            else if (dataType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(data);
            }
            else if (dataType == typeof(double))
            {
                return double.Parse(data);
            }
            else if (dataType == typeof(bool))
            {
                return bool.Parse(data);
            }
            else if (dataType == typeof(DALAPI.BusStatus))
            {
                return Enum.Parse(typeof(DALAPI.BusStatus), data);
            }
            else if (dataType == typeof(DALAPI.Area))
            {
                return Enum.Parse(prop.PropertyType, data);
            }
            return data;
        }
        /// <summary>
        /// convert object to string
        /// </summary>
        /// <param name="type">the type of the object to convert</param>
        /// <param name="obj">the object to convert</param>
        /// <returns>the converted string</returns>
        private static string TypeToString(Type type, object obj)
        {

            if (obj == null)
            {
                return "NuN";
            }
            return obj.ToString();
        }
    }
}
