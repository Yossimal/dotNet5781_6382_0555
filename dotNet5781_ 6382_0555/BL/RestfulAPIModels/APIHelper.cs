using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BL.RestfulAPIModels
{
    static class APIHelper
    {
        /// <summary>
        /// The http client, we want only one client so we will use only one port
        /// </summary>
        public static HttpClient ApiClient { get; set; }
        static APIHelper() {
            InitializeClient();
        }
        /// <summary>
        /// initializing the http client basic data
        /// </summary>
        private static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("https://m.fleet.ls.hereapi.com/2/calculateroute.json");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
