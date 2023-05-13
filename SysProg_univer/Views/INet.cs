using System.Net;

namespace SysProg_univer.Presenters
{
    public interface INet
    {

        bool NETAccessible { get; set; }
        string NETStatusDesc { get; set; }
    }
}
