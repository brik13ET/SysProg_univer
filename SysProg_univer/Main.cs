using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
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
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				delegate 
				{
					return true; // **** Always accept
				};
		}

		Record[] data_recs;
		string Logger
		{
			get { return textBox4.Text; }
			set { textBox4.Text = value + "\r\n"; }
		}
		private readonly string[] code_pieces =
		{
			"class RuntimeClass\n"+
			"{\n" +
			"   public bool continious = false;\n" +
			"   public byte iter_count = 0;\n" +
			"   public RuntimeClass() {\n" ,
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
		private void button9_Click(object sender, EventArgs e)
		{
			
			string norm_piece = richTextBox1.Text;
			int paste_index = Regex.Match(norm_piece, @"\}\s*while\s*\(\s*").Index;

			norm_piece = norm_piece.Insert(paste_index, code_pieces[1]);
			string norm_code = code_pieces[0] + norm_piece + code_pieces[2];
			//Logger += norm_code;

			var cSharpCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
			var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" });
			parameters.GenerateExecutable = false;
			CompilerResults results = cSharpCodeProvider.CompileAssemblyFromSource(parameters, norm_code);
			results.Errors.Cast<CompilerError>().ToList().ForEach(error => Logger += error.ErrorText);
			Type RC = results.CompiledAssembly.GetType("RuntimeClass");
			var obj = Activator.CreateInstance(RC);
			var RC_C = RC.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
			RC_C.Invoke(obj, null);
			bool cont = (Boolean)RC.GetField("continious").GetValue(obj);
			//var flds = RC.GetFields();
	//         for (int i = 0; i < flds.Length; i++)
	//         {
	//             Logger += String.Format("   {0}:\t'{1}\t[{2}]'",
	//                 flds[i].Name, flds[i].GetValue(obj), flds[i].FieldType);
	//          }
			Logger += "Итераций > 1 ? " + (cont ? "Да" : "Нет");
		}

		private void button6_Click(object sender, EventArgs e)
		{
			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (this.data_recs == null)
				data_recs = new Record[0];
			var aaaaaaaaaaaaa = new Record[data_recs.Length + 1];
			string uri = "";
			Array.Copy(data_recs, aaaaaaaaaaaaa, data_recs.Length);
			var isOk = false;
			if (Clipboard.ContainsText())
			{
				uri = Clipboard.GetText();
				isOk = Uri.IsWellFormedUriString(uri, UriKind.Absolute);
			}
			if (!isOk)
				uri = "https://ssau.ru";

			HttpWebRequest webRequest = (HttpWebRequest)WebRequest
										   .Create(uri);
			webRequest.AllowAutoRedirect = false;
			webRequest.Timeout = 1000;
			bool open = false;
			try
			{
				HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
				open = response.StatusCode == HttpStatusCode.OK;
				Logger += String.Format("Request to `{0}`\t{1} {2}", uri, (int)response.StatusCode, response.StatusDescription);
			} catch (WebException ex)
			{
				Logger += String.Format("Request to `{0}`\t{1}", uri, ex.Status);
				open = false;
			}

			var bbbbbbbbbb = new Record(uri, open);
			this.Enabled = false;
			UpdateRecord f = new UpdateRecord(bbbbbbbbbb);
			var dr = f.ShowDialog(this);
			this.Enabled = true;
			if (dr == DialogResult.OK)
			{
				aaaaaaaaaaaaa[data_recs.Length] = bbbbbbbbbb;
				data_recs = aaaaaaaaaaaaa;
			}
			UpdateRecords();
		}

		private void UpdateRecords()
		{

			listBox2.Items.Clear();
			foreach (Record rec in data_recs)
			{

				listBox2.Items.Add(String.Format("{0}\t{1}", rec.isOpen, rec.Address));
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (listBox2.SelectedItems.Count == 0 || listBox2.SelectedIndex == -1) return;
			Record re = data_recs[listBox2.SelectedIndex];
			this.Enabled = false;
			UpdateRecord f = new UpdateRecord(re);
			f.ShowDialog(this);
			this.Enabled = true;
			UpdateRecords();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (listBox2.SelectedIndex == -1) return;
			if (this.data_recs == null)
				data_recs = new Record[0];
			var aaaaaaaaaaaaa = new Record[data_recs.Length - 1];
			Array.Copy(data_recs, aaaaaaaaaaaaa, listBox2.SelectedIndex);
			Array.Copy(
				data_recs,
				listBox2.SelectedIndex + 1,
				aaaaaaaaaaaaa,
				listBox2.SelectedIndex,
				data_recs.Length - listBox2.SelectedIndex - 1
			);
			data_recs = aaaaaaaaaaaaa;

			var asd = listBox2.SelectedIndex;
			UpdateRecords();
			if (data_recs.Length > asd)
			 listBox2.SelectedIndex = asd;

		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
				return;
			var data = File.ReadAllText(openFileDialog1.FileName);
			data_recs = Record.Extract(data);
			UpdateRecords();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
			if (!File.Exists(saveFileDialog1.FileName)) File.Create(saveFileDialog1.FileName);
			if (data_recs == null) return;

			saveFileDialog1.OpenFile().Close();

			using (var file = File.Open(saveFileDialog1.FileName,FileMode.OpenOrCreate))
			using (var sr = new StreamWriter(file))
				sr.Write(Record.Compress(data_recs));
		}
	}
}
