using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace DALAPI
{
    /// <summary>
    /// Data Access Layer FACTORY
    /// Singleton Pattern
    /// </summary>
    public static class DALFactory
    {

       public static IDAL API{
           get
           {
                return DAL.DALObject.Instance;
           }
       }
    }
}
