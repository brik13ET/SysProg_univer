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
        private Record record;
        public UpdateRecord(Record r)
        {
            InitializeComponent();
            record = r;
            textBox1.Text = r.Address;
            radioButton1.Checked = r.isOpen;
            radioButton2.Checked = !r.isOpen;
        }

        private void UpdateRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            record.Address = textBox1.Text;
            record.isOpen = radioButton1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
