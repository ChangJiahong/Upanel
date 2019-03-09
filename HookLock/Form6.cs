using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form6 : Form
    {
        String filePath;

        public Form6(String filepath)
        {
            InitializeComponent();
            this.filePath = filepath;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //删除
        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154";
            conn.Open();

            string Name = "";
            bool ist = true;
            //OleDbDataReader reader;
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                DataRowView dv = ((DataRowView)checkedListBox1.CheckedItems[i]);
                String id = dv["U盘序列号"].ToString();

                //MessageBox.Show(id);

                string name = dv["U盘名"].ToString();
                String str = "delete from [usb] where [U盘序列号]='" + id + "'";
                OleDbCommand comm = new OleDbCommand(str, conn);
                //reader = comm.ExecuteReader()
                int k = comm.ExecuteNonQuery();
                Name = name;
                if (k <= 0)
                {
                    ist = false;
                }
            }
            if (ist)
            {

                string sql1 = "insert into [Log](日期,操作)values('" + DateTime.Now + "','删除" + Name + "U盘认证成功')";
                OleDbCommand com = new OleDbCommand(sql1, conn);
                com.ExecuteNonQuery();

                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                conn.Close();
                this.Close();
            }
            else
            {

                string sql1 = "insert into [Log](日期,操作)values('" + DateTime.Now + "','删除" + Name + "U盘认证失败')";
                OleDbCommand com = new OleDbCommand(sql1, conn);
                com.ExecuteNonQuery();

                MessageBox.Show("删除失败！请稍后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                conn.Close();
                this.Close();
            }
            conn.Close();

        }

        private void init()
        {
            Thread.Sleep(1);
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";

            conn.Open();
            string str = "select *from [usb]";
            OleDbCommand comm = new OleDbCommand(str, conn);
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = comm;

            DataTable dt = new DataTable();
            da.Fill(dt);
            checkedListBox1.DataSource = dt;
            checkedListBox1.ValueMember = "U盘序列号";   // 对应列的值
            checkedListBox1.DisplayMember = "U盘名";  // 可见列
            conn.Close();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(init);
            t.Start();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)  //被选中
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                    //checkedListBox1.SetItemChecked(i, true);
                }
            }
            else if (checkBox1.CheckState == CheckState.Unchecked)  // 未选中
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                    //checkedListBox1.SetItemChecked(i, false);
                }
            }


        }
    }
}
