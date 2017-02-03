using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace stalker
{
    public partial class landing : Form
    {
        public landing()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Hide();           
            //this.Close();
            //this.Dispose();

            Form1 f1 = new Form1();
            f1.ShowDialog();
        }

        private void landing_Load(object sender, EventArgs e)
        {
            int strt;
            strt = Properties.Settings.Default.startWait;
            //Thread.Sleep(strt);            
        }

       

       
    }
}
