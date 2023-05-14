using SysProgUniver.Views;

namespace SysProgUniver.Presenters
{
    public interface ISyntaxAnalyzer : ILog
    {
        string SAcode { get; set; }
    }
}
