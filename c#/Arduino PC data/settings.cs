using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_PC_data
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                Globals.form.setsettings(int.Parse(textBox1.Text));
            }
            this.Close();
        }
    }
}
