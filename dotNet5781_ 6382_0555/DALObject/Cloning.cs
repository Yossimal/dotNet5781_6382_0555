using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DALAPI.DAO;


namespace DAL
{
    static class Cloning
    {
        internal static DAOType Clone<DAOType>(this DAOType original) where DAOType:new()
        {
            DAOType copyToObject = (DAOType)Activator.CreateInstance(original.GetType());


            foreach (PropertyInfo propertyInfo in typeof(DAOType).GetProperties())
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);
                }
            }

            return copyToObject;
        }

        internal static IEnumerable<DAOType> Clone<DAOType>(this IEnumerable<DAOType> original) where DAOType : new()
        {
            List<DAOType> ret=new List<DAOType>();
            foreach (DAOType dao in original)
            {
                ret.Add(dao.Clone());
            }
            return ret;
        }
    }
}
