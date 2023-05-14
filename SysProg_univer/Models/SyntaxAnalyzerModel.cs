using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SysProgUniver
{
    public class SyntaxAnalyzerModel
    {
        private static readonly string[] _codePieces;

        static SyntaxAnalyzerModel()
        {

            _codePieces = new[] {
                "class RuntimeClass\n" +
                "{\n" +
                "   public bool continious = false;\n" +
                "   public byte iter_count = 0;\n" +
                "   public RuntimeClass() {\n",
                // do {
                "       if (iter_count > 1)\n" +
                "       {\n" +
                "          continious = true;" +
                "          break;\n" +
                "       }\n" +
                "       iter_count++;\n",
                // } while (cont) ;
                "   }\n" +
                "}"
            };
        }

        public string Output { get; set; }

       static private string InjectClass(string code)
        {
            int pasteIndex = Regex.Match(code, @"\}\s*while\s*\(\s*").Index;
            if (pasteIndex == 0)
                throw new ArgumentException("Цикл не найден");

            code = code.Insert(pasteIndex, _codePieces[1]);
            return string.Format("{0}{1}{2}", _codePieces[0], code, _codePieces[2]);
        }

        static private CompilerResults Compile(string code)
        {
            CSharpCodeProvider cSharpCodeProvider;
            var options = new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } };
            cSharpCodeProvider = new CSharpCodeProvider(options);
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" });
            parameters.GenerateExecutable = false;
            var ret = cSharpCodeProvider.CompileAssemblyFromSource(parameters, code);
            cSharpCodeProvider.Dispose();
            return ret;
        }

        public bool IsContinuous(string code)
        {
            var normCode = InjectClass(code);
            CompilerResults results = Compile(normCode);

            if (results.Errors.Count != 0)
            {
                results.Errors.Cast<CompilerError>().ToList().ForEach(error =>
                {
                    Output += '\n' + error.ErrorText;
                });
                return false;
            }
            else
            {
                Output += '\n' + "Compiled Successful";
            }

            Type rc = results.CompiledAssembly.GetType("RuntimeClass");
            var obj = Activator.CreateInstance(rc);
            var rcC = rc.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            rcC.Invoke(obj, null);
            bool cont = (Boolean)rc.GetField("continious").GetValue(obj);
            return cont;
        }

    }
}
