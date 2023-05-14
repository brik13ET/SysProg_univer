using System;
using System.IO;
using System.Windows.Forms;
using SysProgUniver.Presenters;
using SysProgUniver.Views;

namespace SysProgUniver
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

        private SyntaxAnalyzerPresenter sa;
        private RecordsContainerPresenter rcontainer;
        private LLCallPresenter llc;

        private string Logger
        {
            get => textBox4.Text;
            set
            {
                textBox4.Invoke(
                    new MethodInvoker(
                        
                        () => {
                            textBox4.Text = $"{value}\r\n";
                        }
                    )
                );
            }
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
        public string Log { get => Logger; set => Logger += value; }
        public string SAcode
        {
            get => richTextBox1.Text;
            set => richTextBox1.Text = value;
        }
        public string[] RFancyOutput
        {
            get
            {
                string[] output = new string[dataGridView1.Rows.Count];
                int i = 0;
                int j = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach(DataGridViewCell cell in row.Cells)
                    {
                        output[i] += cell.Value.ToString();
                        if (j < row.Cells.Count)
                            output[i] += '\t';
                        j++;
                    }
                    i++;
                }
                return output;
            }
            set
            {

                if (value == null)
                    return;
                dataGridView1.Invoke(
                    new MethodInvoker(
                        () => {
                            dataGridView1.Rows.Clear();
                            for (int i = 0; i < value.Length; i++)
                            {
                                string[] cells = value[i].Split('\t');
                                dataGridView1.Rows.Add(cells);
                            }
                        }
                    )
                );
            }
        }

        private void Button1Click(object sender, EventArgs e)
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
            Logger += f.Log;
            f.Dispose();
            if (dr == DialogResult.OK)
            {
                this.rcontainer.AddOrUpdate(created);
            }
        }
        private void Button2Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            if (dataGridView1.SelectedRows[0].Index >= rcontainer.Length)
                dataGridView1.Rows[rcontainer.Length - 1].Selected = true;

            var rec = rcontainer[dataGridView1.SelectedRows[0].Index];
            var f = new UpdateRecord(rec);
            var dr = f.ShowDialog(this);
            this.Enabled = true;
            Logger += f.Log;
            f.Dispose();
            if (dr == DialogResult.OK)
            {
                this.rcontainer.Update(rec);
            }
        }
        private void Button3Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
            if (dataGridView1.SelectedRows[0].Index >= rcontainer.Length)
                dataGridView1.Rows[rcontainer.Length - 1].Selected = true;

            rcontainer.Remove(dataGridView1.SelectedRows[0].Index);
        }
        private void Button5Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            rcontainer.Load(openFileDialog1.FileName);
        }
        private void Button4Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            rcontainer.Save(openFileDialog1.FileName);
        }
        private void Button10Click(object sender, EventArgs ev)
        {
            rcontainer.DbPush();
        }
        private void Button11Click(object sender, EventArgs e)
        {
            rcontainer.DbPull();
        }
        private void Button9Click(object sender, EventArgs e)
        {
            sa.Analyze();
        }
        private void Button6Click(object sender, EventArgs e)
        {
            llc.Invoke();
        }

        private void Button7Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
        }

        private void Button8Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
        }
    }
}