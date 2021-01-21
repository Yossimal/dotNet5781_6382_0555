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
        public static object ToDAO<DAOType>(this XElement xml) where DAOType : class, new()
        {
            object ret = Activator.CreateInstance(typeof(DAOType));
            Type retType = typeof(DAOType);
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
        public static XElement ToXElement(this object dao)
        {
            Type daoType = dao.GetType();
            XElement ret = new XElement(daoType.Name);
            foreach (PropertyInfo prop in daoType.GetProperties())
            {
                XElement propElement = new XElement(prop.Name);
                propElement.Value = TypeToString(prop.PropertyType, prop.GetValue(dao));
                ret.Add(propElement);
            }
            return ret;
        }
        private static object StringToType(PropertyInfo prop, string data)
        {
            if (data == "NuN")
            {
                return null;
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                return DateTime.Parse(data);
            }
            else if (prop.PropertyType == typeof(int))
            {
                return int.Parse(data);
            }
            else if (prop.PropertyType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(data);
            }
            else if (prop.PropertyType == typeof(double))
            {
                return double.Parse(data);
            }
            else if (prop.PropertyType == typeof(bool))
            {
                return bool.Parse(data);
            }
            else if (prop.PropertyType == typeof(DALAPI.BusStatus))
            {
                return Enum.Parse(typeof(DALAPI.BusStatus), data);
            }
            else if (prop.PropertyType == typeof(DALAPI.Area)) {
                return Enum.Parse(prop.PropertyType, data);
            }
            return data;
        }
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
