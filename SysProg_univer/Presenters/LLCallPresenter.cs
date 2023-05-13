using System;

namespace SysProg_univer.Presenters
{
    public class LLCallPresenter
    {
        private ILLCall view;
        public LLCallPresenter(ILLCall view) { this.view = view; }
        public void call()
        {
            string ret = "";
            try
            {
                int a, b;
                a = int.Parse(view.LLdivident);
                b = int.Parse(view.LLdivisor);
                view.LLresult = $"{LLRunner.call_asm_div(a, b)}";
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
            view.LLLog = ret;
        }
    }
}
