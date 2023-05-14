using SysProgUniver.Presenters;
using SysProgUniver.Views;
using System;
using System.Security.Policy;
using System.Windows.Forms;

namespace SysProgUniver
{
    public partial class UpdateRecord : Form, INet, IRecord
    {

        public UpdateRecord(Record r, bool updateAccess = false)
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            record = new RecordPresenter(r, this);
            if (updateAccess)
            {
                NETPresenter = new NetPresenter(this, r.Url);
                record.DT = DateTime.Now;
                this.Accessable = this.NetAccessible;
            }
            record.RAccessable = this.Accessable;
            record.DT = this.dateTimePicker1.Value;
        }

        private RecordPresenter record;

        private NetPresenter NETPresenter;
        public string Url
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
        public string Log
        {
            get;
            set;
        }
        public bool NetAccessible
        {
            get => this.radioButton1.Checked;
            set => this.radioButton2.Checked = !(this.radioButton1.Checked = value);
        }
        public bool Accessable
        {
            get => this.radioButton1.Checked;
            set => this.radioButton2.Checked = !(this.radioButton1.Checked = value);
        }
        public DateTime DT
        {
            get => dateTimePicker1.Value;
            set => dateTimePicker1.Value = value;
        }
        private void UpdateRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dlgResult = MessageBox.Show("Сохранить?", "", MessageBoxButtons.YesNoCancel);
            if (dlgResult == DialogResult.Cancel)
            {
                e.Cancel = dlgResult == DialogResult.Cancel;
                return;
            }
            if (dlgResult == DialogResult.Yes)
            {
                record.RUri = textBox1.Text;
                record.RAccessable = radioButton1.Checked;
                record.DT = this.dateTimePicker1.Value;
            }
        }
        private void Button1Click(object sender, EventArgs e)
        {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
    }
}
