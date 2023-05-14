using System;
using System.Globalization;
using System.Net;

namespace SysProgUniver.Presenters
{
    public class NetPresenter
    {
        public NetPresenter(INet view, string url)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view","INet view not provided");
            }
            string tmpDesc;
            HttpStatusCode statusCode;
            view.NetAccessible = Network.IsAccessable(url, out tmpDesc, out statusCode);
            view.Log = $"{((int)statusCode).ToString(CultureInfo.InvariantCulture)} {tmpDesc}";
        }
    }
}
