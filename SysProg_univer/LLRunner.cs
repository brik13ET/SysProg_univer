using System;
using System.Runtime.InteropServices;

namespace SysProg_univer
{
    internal class LLRunner
    {

        [DllImport(@"D:\Git\SysProg_univer\x64\Debug\ASM_dll.dll", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
         public static extern int call_asm_div(int a, int b);
    }
}
