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

namespace DALXML
{
    internal static class DAOToXML
    {
        public static object ToDAO<DAOType>(XmlElement xml) where DAOType : class, new()
        {
            object ret = Activator.CreateInstance(typeof(DAOType));
            Type retType = typeof(DAOType);
            foreach (XmlElement element in xml)
            {
                PropertyInfo objectProp = retType.GetProperty(element.Name);
                if (objectProp.CanWrite)
                {
                    objectProp.SetValue(ret, stringToType(objectProp, xml.InnerText));
                }

            }

            return null;
        }

        private static object stringToType(PropertyInfo prop, string data)
        {
            if (prop.PropertyType == typeof(DateTime))
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
            return data;
        }

        public static XElement ToXElement(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(obj.GetType());
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}
