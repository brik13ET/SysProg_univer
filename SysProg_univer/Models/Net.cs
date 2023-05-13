using SysProg_univer.Presenters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysProg_univer
{
    static public class Net
    {
        static Net ()
        {
            //SSL workaround
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate
                {
                    return true; // **** Always accept
                };
        }

        public static bool isAccessable(string uri, out string desc, out HttpStatusCode code)
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
                desc= response.StatusDescription;
                code = response.StatusCode;
            }
            catch (WebException ex)
            {
                code = HttpStatusCode.InternalServerError;
                desc = ex.Status.ToString();
            }
            return open;
        }
    }
}
