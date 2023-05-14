using System;
using System.Globalization;

namespace SysProgUniver.Presenters
{
    public class LLCallPresenter
    {
        private ILLCall view;
        public LLCallPresenter(ILLCall view) { this.view = view; }
        public void Invoke()
        {
            string ret = "";
            try
            {
                int a, b;
                a = int.Parse(view.LLdivident, CultureInfo.InvariantCulture);
                b = int.Parse(view.LLdivisor, CultureInfo.InvariantCulture);
                view.LLresult = $"{LLRunner.AsmDiv(a, b).ToString(CultureInfo.InvariantCulture)}";
                ret = "Success LLCall";
            }
            catch (FormatException ex)
            {
                ret += "Ошибка входных данных";
                ret += ex.Message;
            }
            catch (ArgumentException ex)
            {
                ret += "Ошибка аргумента";
                ret += ex.Message;
            }
            catch (OverflowException ex)
            {
                ret += "Ошибка размера входных данных";
                ret += ex.Message;
            }
            catch (Exception ex)
            {
                ret += ex.Message;
            }
            view.Log = ret;
        }
    }
}
