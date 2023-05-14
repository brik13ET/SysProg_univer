using SysProgUniver.Views;
using System.Net;

namespace SysProgUniver.Presenters
{
    public interface INet : ILog
    {

        bool NetAccessible { get; set; }
    }
}
