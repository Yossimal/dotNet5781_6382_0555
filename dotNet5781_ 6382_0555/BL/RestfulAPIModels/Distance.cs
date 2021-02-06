using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.RestfulAPIModels
{
    /// <summary>
    /// object theat help working with the api
    /// </summary>
    class Distance
    {
        /// <summary>
        /// the api key
        /// </summary>
        public const string APIKey = "aMQyd9-ACNp0p0gkUGKWswllHoVFYmUOjtDXk-pyGIw";
        /// <summary>
        /// coordinate data for the api
        /// </summary>
        public double FromLat { get; set; }
        public double FromLon { get; set; }
        public double ToLat { get; set; }
        public double ToLon { get; set; }
        /// <summary>
        /// the request url (based on the other 
        /// </summary>
        public string RequestURL
        {
            get
            {
                string ret = $@"
                                    https://m.fleet.ls.hereapi.com/2/calculateroute.json?
                                    apiKey={APIKey}
                                    &routeMatch=1
                                    &mode=fastest;car;traffic:disabled
                                    &waypoint0={FromLat},{FromLon}
                                    &waypoint1={ToLat},{ToLon}";
                return ret.Replace("\r", "").Replace("\t", "").Replace(" ", "").Replace("\n", "");
            }
        }
        /// <summary>
        /// gets the distance form the response text
        /// </summary>
        /// <param name="responseJSONText">the response text in JSON format</param>
        /// <returns>the distance</returns>
        public double GetDistance(string responseJSONText)
        {
            JObject responseAsJSON = JObject.Parse(responseJSONText);
            double? result = (responseAsJSON["response"]["route"][0]["waypoint"][0]["matchDistance"].ToObject(typeof(double)) as double?);
            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                throw new InvalidOperationException("The input not contains the distance");
            }
        }
    }
}
