using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace SysProg_univer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            sa = new SyntaxAnalyzer();
        }

        SyntaxAnalyzer sa;
        RecordLocal[] _dataRecs = new RecordLocal[0];
        private string connstr = "Server=localhost;Database=Records;UID=sql_con;PWD=1111WWww;Integrated Security=True;";
        private string Logger
        {
            get => textBox4.Text;
            set => textBox4.Text = $"{value}\r\n";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == null)
                Logger += "Отсутствует текст";
            var res = sa.isContinuous(richTextBox1.Text);
            Logger += $"Больше одной итерации ? {res}";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int a, b;
                a = int.Parse(textBox1.Text);
                b = int.Parse(textBox2.Text);
                textBox3.Text = $"{LLRunner.call_asm_div(a, b)}";
            }
            catch (FormatException ex)
            {
                Logger += "Ошибка входных данных";
                Logger += ex.Message;
            }
            catch (ArgumentException ex)
            {
                Logger += "Ошибка аргумента";
                Logger += ex.Message;
            }
            catch (OverflowException ex)
            {
                Logger += "Ошибка размера входных данных";
                Logger += ex.Message;
            }
            catch (Exception ex)
            {
                Logger += ex.Message;
            }
        }

        // MVP
        private void button1_Click(object sender, EventArgs e)
        {
            if (this._dataRecs == null)
                _dataRecs = new RecordLocal[0];
            var newRecord = new RecordLocal[_dataRecs.Length + 1];
            string uri = "";
            Array.Copy(_dataRecs, newRecord, _dataRecs.Length);
            var isOk = false;
            if (Clipboard.ContainsText())
            {
                uri = Clipboard.GetText();
                isOk = Uri.IsWellFormedUriString(uri, UriKind.Absolute);
            }

            if (!isOk)
                uri = "https://ssau.ru";


            var created = new RecordLocal(uri, Net.isAccessable(uri));

            Logger += String.Format("Request to `{0}`\t{1} {2}", uri, (int)Net.StatusCode,
                Net.StatusDesc);
            this.Enabled = false;
            UpdateRecord f = new UpdateRecord(created);
            var dr = f.ShowDialog(this);
            this.Enabled = true;
            if (dr == DialogResult.OK)
            {
                newRecord[_dataRecs.Length] = created;
                _dataRecs = newRecord;
            }

            UpdateRecords();
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Count == 0 || listBox2.SelectedIndex == -1) return;
            RecordLocal re = _dataRecs[listBox2.SelectedIndex];
            this.Enabled = false;
            UpdateRecord f = new UpdateRecord(re);
            f.ShowDialog(this);
            this.Enabled = true;

            UpdateRecords();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;
            if (this._dataRecs == null)
                _dataRecs = new RecordLocal[0];
            var last = new RecordLocal[_dataRecs.Length - 1];


            Array.Copy(_dataRecs, last, listBox2.SelectedIndex);
            Array.Copy(
                _dataRecs,
                listBox2.SelectedIndex + 1,
                last,
                listBox2.SelectedIndex,
                _dataRecs.Length - listBox2.SelectedIndex - 1
            );
            _dataRecs = last;

            var asd = listBox2.SelectedIndex;
            UpdateRecords();
            if (_dataRecs.Length > asd)
                listBox2.SelectedIndex = asd;
        }

        private void UpdateRecords()
        {
            listBox2.Items.Clear();
            string[] uris = new string[_dataRecs.Length];
            foreach (RecordLocal rec in _dataRecs)
            {

                listBox2.Items.Add(String.Format("{0}\t{1}", rec.isOpen, rec.url));
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            var data = File.ReadAllText(openFileDialog1.FileName);
            _dataRecs = RecordLocal.Extract(data);
            UpdateRecords();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(saveFileDialog1.FileName)) File.Create(saveFileDialog1.FileName);
            if (_dataRecs == null) return;

            saveFileDialog1.OpenFile().Close();

            using (var file = File.Open(saveFileDialog1.FileName, FileMode.OpenOrCreate))
            using (var sr = new StreamWriter(file))
                sr.Write(RecordLocal.Compress(_dataRecs));
        }

        private void button10_Click(object sender, EventArgs ev)
        {
            using (var context = new DBE_Context(connstr))
            {
                for (int i = 0; i < _dataRecs.Length; i++)
                {
                    var dbe = new DbRecord(_dataRecs[i]);
                    context.records.AddOrUpdate(dbe);
                    
                }
                context.SaveChanges();
                _dataRecs = new RecordLocal[context.records.Count()];
                var x = (from r in context.records select r).ToList();
                for (int i = 0; i < _dataRecs.Length; i++)
                {
                    _dataRecs[i] = new RecordLocal(x[i]);
                }
            }
            UpdateRecords();
        }
    }
}