using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProg_univer.Presenters
{
    public interface ISyntaxAnalyzer
    {
        string SAResult { get; set; }
        string SAcode { get; set; }
    }
}
