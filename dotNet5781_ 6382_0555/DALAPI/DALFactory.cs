using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DAL;



namespace DALAPI
{
    /// <summary>
    /// Data Access Layer FACTORY
    /// Singleton Pattern
    /// </summary>
    public static class DALFactory
    {
        private struct DALASMData
        {
            public string asmName;
            public string asmType;
        }
        public static IDAL API
        {
            //Get an instance of the IDAL
            get
            {
                
                DALASMData asmData = GetAPI();
                Assembly APIAssembly = Assembly.Load(asmData.asmName);
                Type APIType = APIAssembly.GetType(asmData.asmType);
                IDAL ret = APIType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) as IDAL;
                return ret;
            }
        }
        private static DALASMData GetAPI()
        {
            DALASMData ret;
            XElement config = XElement.Parse(Resources.Config);
            XElement implementationConfig = config.Elements().First(element => element.Name.LocalName == "implementation");
            string selectedAPITagValue = implementationConfig.Elements()
                                                       .Where(element => element.Name.LocalName == "current")
                                                       .Select(e => e.Value)
                                                       .First();
            XElement implementationConfigOptions = implementationConfig.Elements()
                                                                .First(element => element.Name.LocalName == "options");
            XElement currentAPITag = implementationConfigOptions.Elements()
                                                                .First(element => element.Name.LocalName == selectedAPITagValue);
            ret.asmName = currentAPITag.Elements()
                                        .Where(element => element.Name.LocalName == "assembly")
                                        .Select(e => e.Value)
                                        .First();

            ret.asmType = currentAPITag.Elements()
                                        .Where(element => element.Name.LocalName == "type")
                                        .Select(e => e.Value)
                                        .First();
            return ret;
        }
        private static void DoNothing() {
            Type temp = typeof(DAL.DALObject);
            temp = typeof(DAL.DALXML);
        }
    }
}
