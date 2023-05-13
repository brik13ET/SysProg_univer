using System;
using System.IO;
using System.Windows.Forms;
using SysProg_univer.Presenters;
using SysProg_univer.Views;

namespace SysProg_univer
{
    public partial class Main : Form, ILLCall, ISyntaxAnalyzer, IRecordsContainer
    {
        public Main()
        {
            InitializeComponent();
            sa = new SyntaxAnalyzerPresenter(this);
            llc = new LLCallPresenter(this);
            rcontainer = new RecordsContainerPresenter(Properties.Settings.Default.connstr, this);
        }

        SyntaxAnalyzerPresenter sa;
        RecordsContainerPresenter rcontainer;
        LLCallPresenter llc;

        private string Logger
        {
            get => textBox4.Text;
            set => textBox4.Text = $"{value}\r\n";
        }
        public string LLdivident
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
        public string LLdivisor
        {
            get => textBox2.Text;
            set => textBox2.Text = value;
        }
        public string LLresult
        {
            get => textBox3.Text;
            set => textBox3.Text = value;
        }
        public string LLLog { get => Logger; set => Logger += value; }
        public string SAResult
        {
            get => Logger;
            set => Logger += value;
        }
        public string SAcode
        {
            get => richTextBox1.Text;
            set => richTextBox1.Text = value;
        }
        public string[] RFancyOutput
        {
            get
            {
                string[] ret = new string[listBox2.Items.Count];
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    ret[i] = listBox2.Items[i].ToString();
                }
                return ret;

            }
            set
            {
                listBox2.Items.Clear();
                foreach (var item in value)
                    listBox2.Items.Add(item.ToString());
            }
        }
        public string RLog
        {
            get => Logger;
            set => Logger += value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string uri = "";
            bool isOk = false;
            if (Clipboard.ContainsText())
            {
                uri = Clipboard.GetText();
                isOk = Uri.IsWellFormedUriString(uri, UriKind.Absolute);
            }

            if (!isOk)
                uri = "https://ssau.ru";      
            
            this.Enabled = false;
            var created = new Record(uri, false);
            var  f = new UpdateRecord(created, true);
            var dr = f.ShowDialog(this);
            this.Enabled = true;
            Logger += f.NETStatusDesc;
            if (dr == DialogResult.OK)
            {
                this.rcontainer.AddOrUpdate(created);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
            if (listBox2.SelectedIndex >= rcontainer.Length)
                listBox2.SelectedIndex = rcontainer.Length - 1;

            var rec = rcontainer[listBox2.SelectedIndex];
            var f = new UpdateRecord(rec);
            var dr = f.ShowDialog(this);
            this.Enabled = true;
            Logger += f.NETStatusDesc;
            if (dr == DialogResult.OK)
            {
                this.rcontainer.Update(rec);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
            if (listBox2.SelectedIndex >= rcontainer.Length)
                listBox2.SelectedIndex = rcontainer.Length - 1;

            rcontainer.Remove(listBox2.SelectedIndex);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            rcontainer.Load(openFileDialog1.FileName);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            rcontainer.Save(openFileDialog1.FileName);
        }
        private void button10_Click(object sender, EventArgs ev)
        {
            rcontainer.DbPush();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            rcontainer.DbPull();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            sa.Analyze();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            llc.call();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
        }
    }
}