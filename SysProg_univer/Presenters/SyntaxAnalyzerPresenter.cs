using System;

namespace SysProg_univer.Presenters
{
    public class SyntaxAnalyzerPresenter
    {
        ISyntaxAnalyzer isa;
        private SyntaxAnalyzerModel model;

        public SyntaxAnalyzerPresenter(ISyntaxAnalyzer isa)
        {
            model = new SyntaxAnalyzerModel();
            this.isa = isa;
        }
        public void Analyze()
        {
            string ret = "";
            if (isa.SAcode == null || isa.SAcode.Length == 0)
                ret += "Отсутствует текст";
            var res = model.isContinuous(isa.SAcode);
            ret += $"Больше одной итерации ? {res}";
            isa.SAResult = ret;
        }
    }
}
