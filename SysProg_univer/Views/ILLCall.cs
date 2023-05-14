using SysProgUniver.Views;

namespace SysProgUniver.Presenters
{
    public interface ILLCall : ILog
    {
        string LLdivident { get; set; }
        string LLdivisor { get; set; }
        string LLresult { get; set; }
    }
}
