using SysProg_univer.Presenters;
using SysProg_univer.Views;
using System;
using System.Security.Policy;
using System.Windows.Forms;

namespace SysProg_univer
{
    public partial class UpdateRecord : Form, INet, IRecord
    {
        private RecordPresenter record;

        private NETPresenter NETPresenter;
        public string URL
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
        public string NETStatusDesc
        {
            get;
            set;
        }
        public bool NETAccessible
        {
            get => this.radioButton1.Checked;
            set => this.radioButton2.Checked = !(this.radioButton1.Checked = value);
        }
        public bool Accessable
        {
            get => this.radioButton1.Checked;
            set => this.radioButton2.Checked = !(this.radioButton1.Checked = value);
        }
        public UpdateRecord(Record r, bool updateAccess=false)
        {
            InitializeComponent();
            record = new RecordPresenter(r, this);
            if (updateAccess)
            {
                NETPresenter = new NETPresenter(this, r.Url);
                this.Accessable = this.NETAccessible;
            }
            record.RAccessable = this.Accessable;
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
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
    }
}
