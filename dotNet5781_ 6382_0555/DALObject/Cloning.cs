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
        internal static DAOType Clone<DAOType>(this DAOType original) where DAOType : DAOBasic, new()
        {
            DAOType copyToObject = new DAOType();

            foreach (PropertyInfo propertyInfo in typeof(DAOType).GetProperties())
                propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);

            return copyToObject;
        }
    }
}
