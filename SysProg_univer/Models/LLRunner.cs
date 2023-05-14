using System.Runtime.InteropServices;

namespace SysProgUniver
{
    static public class LLRunner
    {

        [DllImport(@"ASM_dll.dll", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall,EntryPoint = "call_asm_div")]
         public static extern int AsmDiv(int divident, int divisor);
    }
}
