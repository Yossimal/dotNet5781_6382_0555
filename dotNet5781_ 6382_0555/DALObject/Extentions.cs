using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DALObject
{
    /// <summary>
    /// extention functions for DAL Objects
    /// </summary>
    static class Extentions
    {
        /// <summary>
        /// get the Id property data of object
        /// </summary>
        /// <param name="obj">the object with the id</param>
        /// <returns>the id</returns>
        internal static int GetId(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo idProp = type.GetProperty("Id");
            if (idProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contains an Id property");
            }
            return Convert.ToInt32(idProp.GetValue(obj));
        }
        /// <summary>
        /// get the IsRunningId property value 
        /// </summary>
        /// <param name="obj">the object with the property</param>
        /// <returns>the value of the property</returns>
        internal static bool IsRunningId(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo isRunningIdProp = type.GetProperty("IsRunningId");
            if (isRunningIdProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contain IsRunningId property");
            }
            return Convert.ToBoolean(isRunningIdProp.GetValue(obj));
        }
        /// <summary>
        /// get the IsDeleted property value 
        /// </summary>
        /// <param name="obj">the object with the property</param>
        /// <returns>the value of the property</returns>
        internal static bool IsDeleted(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo isDeletedProp = type.GetProperty("IsDeleted");
            if (isDeletedProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contain IsDeleted property");
            }
            return Convert.ToBoolean(isDeletedProp.GetValue(obj));
        }
        /// <summary>
        /// set the value of the IsDeleted property in an object to true
        /// </summary>
        /// <param name="obj">the object with the IsDeleted property</param>
        internal static void Delete(this object obj)
        {
            Type type = obj.GetType();

            PropertyInfo isDeletedProp = type.GetProperty("IsDeleted");
            if (isDeletedProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contains an IsDeleted property");
            }
            isDeletedProp.SetValue(obj, true);
        }
        /// <summary>
        /// set the value of the Id property in object
        /// </summary>
        /// <param name="obj">the object with the id property</param>
        /// <param name="setTo">the new value of the Id property</param>
        internal static void SetId(this object obj, int setTo)
        {
            Type type = obj.GetType();
            PropertyInfo idProp = type.GetProperty("Id");
            if (idProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contains an Id property");
            }
            idProp.SetValue(obj, setTo);
        }
    }
}
