using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form3 : Form
    {
        public Form3(String text)
        {
            InitializeComponent();
            if(text!=null)
                label1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(thisclose);
            t.Start();  
        }
        private void thisclose()
        {
            Thread.Sleep(2000);
            this.Close();
        }
    }
}
