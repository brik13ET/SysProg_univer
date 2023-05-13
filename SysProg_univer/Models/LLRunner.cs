using System.Runtime.InteropServices;

namespace SysProg_univer
{
    public class LLRunner
    {

        [DllImport(@"ASM_dll.dll", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
         public static extern int call_asm_div(int a, int b);
    }
}
