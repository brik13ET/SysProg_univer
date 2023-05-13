using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProg_univer.Views
{
    public interface IRecord
    {
        bool Accessable { get; set; }
        string URL { get; set; }
    }
}
