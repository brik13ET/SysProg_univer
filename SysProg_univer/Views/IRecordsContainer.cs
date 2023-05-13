using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProg_univer.Views
{
    public interface IRecordsContainer
    {
        string[] RFancyOutput { get; set; }
        string RLog { get; set; }
    }
}
