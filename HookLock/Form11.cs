using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form11 : Form
    {
        String filePath = "";
        public Form11(String filepath)
        {
            InitializeComponent();
            this.filePath = filepath;
        }

        public struct Log
        {
            public string date;
            public string log;
        }

        List<String> aa = new List<String>();

        private void Form11_Load(object sender, EventArgs e)
        {
            String st = "";

            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
            conn.Open();
            string str = "select * from [Log]";
            OleDbCommand cmd = new OleDbCommand(str, conn);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                st = reader["日期"].ToString()+"    "+ reader["操作"].ToString()+"\n";
                aa.Add(st);
            }
            conn.Close();

            textBox1.Lines = aa.ToArray();


        }
    }
}
