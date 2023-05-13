using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysProg_univer
{
    public static class Net
    {
        static Net (){
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate
                {
                    return true; // **** Always accept
                };
        }

        public static bool isAccessable(string uri)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create(uri);
            webRequest.AllowAutoRedirect = false;
            webRequest.Timeout = 1000;
            bool open = false;
            try
            {
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                open = response.StatusCode == HttpStatusCode.OK;
                StatusCode = response.StatusCode;
                StatusDesc = response.StatusDescription;
            }
            catch (WebException ex)
            {
                StatusDesc = ex.Status.ToString();
                open = false;
            }
            return open;
        }

        public static HttpStatusCode StatusCode { get; set; }
        public static string StatusDesc { get; set; }
    }
}
