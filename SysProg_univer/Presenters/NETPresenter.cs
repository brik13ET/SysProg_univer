using System.Net;

namespace SysProg_univer.Presenters
{
    public class NETPresenter
    {
        private INet view;

        public NETPresenter(INet view, string url)
        {
            this.view = view;
            string tmpDesc = "";
            HttpStatusCode statusCode;
            view.NETAccessible = Net.isAccessable(url, out tmpDesc, out statusCode);
            view.NETStatusDesc = $"{(int)statusCode} {tmpDesc}";
        }
    }
}
