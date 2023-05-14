using System;

namespace SysProgUniver.Views
{
    public interface IRecord
    {
        bool Accessable { get; set; }
        string Url { get; set; }
        DateTime DT { get; set; }
    }
}
