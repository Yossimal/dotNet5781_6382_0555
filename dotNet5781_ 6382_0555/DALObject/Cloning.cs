using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DALAPI.DAO;


namespace DAL
{
    /// <summary>
    /// Deep Clone
    /// improved clone - search for deepest son and copy from it.
    /// </summary>
    static class Cloning
    {
        /// <summary>
        /// clone an object
        /// </summary>
        /// <typeparam name="DAOType">the object type</typeparam>
        /// <param name="original">the object to clone</param>
        /// <returns>cloned object</returns>
        internal static DAOType Clone<DAOType>(this DAOType original) where DAOType:new()
        {
            //create an instance of the object that we want to clone
            DAOType copyToObject = (DAOType)Activator.CreateInstance(original.GetType());

            //clone the object
            foreach (PropertyInfo propertyInfo in original.GetType().GetProperties())
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);
                }
            }

            return copyToObject;
        }
        /// <summary>
        /// clone collection
        /// </summary>
        /// <typeparam name="DAOType">the type of the object to clone</typeparam>
        /// <param name="original">the object to clone</param>
        /// <returns>the cloned object</returns>
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
