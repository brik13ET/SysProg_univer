using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace SysProg_univer
{
    public partial class UpdateRecord : Form
    {
        private RecordLocal record;
        public UpdateRecord(RecordLocal r)
        {
            InitializeComponent();
            record = r;
            textBox1.Text = r.url;
            radioButton1.Checked = r.isOpen;
            radioButton2.Checked = !r.isOpen;
        }

        private void UpdateRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
			record.url = textBox1.Text;
			record.isOpen = radioButton1.Checked;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
    }
}
