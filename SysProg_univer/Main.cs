using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysProg_univer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        string Logger
        {
            get { return textBox4.Text; }
            set { textBox4.Text = value + "\r\n"; }
        }
        private readonly string[] code_pieces =
        {
            "class RuntimeClass" +
            "{" +
                "public bool continious = false;" +
                "public byte iter_count = 0;" +
                "public RuntimeClass() {" ,
                    // do {
                    "iter_count++;" +
                        "if (iter_count > 1)" +
                        "   break;",
                    // } while (cont) ;
                "}" +
            "}"
        };
        private void button9_Click(object sender, EventArgs e)
        {
            
            string norm_piece = richTextBox1.Text;
            norm_piece.Insert(Regex.Match(norm_piece, @"\}\s*while\s*\(\s*").Index, code_pieces[1]);
            string norm_code = code_pieces[0] + norm_piece + code_pieces[2];
            var cSharpCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" });
            parameters.GenerateExecutable = false;
            CompilerResults results = cSharpCodeProvider.CompileAssemblyFromSource(parameters, norm_code);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Logger += error.ErrorText);
            Type RC = results.CompiledAssembly.GetType("RuntimeClass");
            var obj = Activator.CreateInstance(RC);
            RC.GetConstructor(new[] { }, )
            bool cont = (Boolean)RC.GetField("continious").GetValue(obj);
            var flds = RC.GetFields();
            for (int i = 0; i < flds.Length; i++)
            {
                Logger += String.Format("   {0}:\t'{1}\t[{2}]'",
                    flds[i].Name, flds[i].GetValue(obj), flds[i].FieldType);
            }
            Logger += "Итераций > 1 ? " + (cont ? "Да" : "Нет");
        }
    }
}
