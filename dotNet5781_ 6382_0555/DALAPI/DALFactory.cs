using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DAL;



namespace DALAPI
{
    public static class DALFactory
    {

       public static IDAL API{
           get
           {
               return DALObject.Instance;
           }
       }
    }
}
