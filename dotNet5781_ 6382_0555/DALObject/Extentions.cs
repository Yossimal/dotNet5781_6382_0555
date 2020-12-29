using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DALObject
{
    static class Extentions
    {
        internal static int GetId(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo idProp = type.GetProperty("Id");
            if (idProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contains an Id property");
            }
            return Convert.ToInt32(idProp.GetValue(null));
        }

        internal static bool IsRunningId(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo isRunningIdProp = type.GetProperty("IsRunningId");
            if (isRunningIdProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contain IsRunningId property");
            }
            return Convert.ToBoolean(isRunningIdProp.GetValue(null));
        }
        internal static bool IsDeleted(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo isDeletedProp = type.GetProperty("IsDeleted");
            if (isDeletedProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contain IsDeleted property");
            }
            return Convert.ToBoolean(isDeletedProp.GetValue(null));
        }

        internal static void Delete(this object obj)
        {
            Type type = obj.GetType();

            PropertyInfo isDeletedProp = type.GetProperty("IsDeleted");
            if (isDeletedProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contains an IsDeleted property");
            }
            isDeletedProp.SetValue(null, true);
        }

        internal static void SetId(this object obj,int setTo)
        {
            Type type = obj.GetType();

            PropertyInfo idProp=type.GetProperty("Id");
            if (idProp == null)
            {
                throw new InvalidOperationException($"The type {type.Name} not contains an Id property");
            }
            idProp.SetValue(null,setTo);
        }
    }
}
