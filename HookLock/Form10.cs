using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form10 : Form
    {

        String Email_nam;
        String str;//验证码
        string filePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\data.accdb";

        Form1 f1;

        public Form10(Form1 f1)
        {
            InitializeComponent();
            this.f1 = f1;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
            conn.Open();

            if (textBox1.Text.Equals(str))
            {
                new Form3("验证码正确！").ShowDialog();
                
                string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','" + Email_nam + "邮箱登陆成功')";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                int k = comm.ExecuteNonQuery();
                conn.Close();
                Form4.isLogin = true;
                
                //关闭锁定
                this.Close();
                f1.Close();
            }
            else
            {
                string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','" + Email_nam + "邮箱登陆失败')";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                int k = comm.ExecuteNonQuery();
                conn.Close();

                new Form3("验证码错误，\n请稍后重试！").ShowDialog();
            }
        }

        /// <summary> 
        /// 替换邮箱中间几位为*号  
        /// </summary>  
        /// <param name="Email"></param>  
        /// <returns></returns>  
        protected static string ReturnEmail(string Email)
        {
            Regex re = new Regex(@"\w{4}(?=@\w+?.\S+)", RegexOptions.None);
            Email = re.Replace(Email, "****");
            return Email;
        }

        private void Form10_Load(object sender, EventArgs e)
        {

            button2.Enabled = false;

            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
            conn.Open();

            
            string str = "select * from [Email]";
            OleDbCommand cmd = new OleDbCommand(str, conn);
            OleDbDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
                Email_nam = reader["Emial"].ToString();

            conn.Close();
            label2.Text = ReturnEmail(Email_nam);
        }


        //获取验证码
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            timer1.Start();
            Label(true);

            //生成 6位 验证码
            str = GenerateCheckCode(6);
            //发送邮件
            String msg = "  你好，这是在电脑认证系统重要文件，你的验证码是：" +str+"  ，在一分钟内有效，如非本人操作请忽略此邮件。";
            try
            {
                Email.send_Email("321168813@qq.com", Email_nam, "oojlutazjvckbifa", "电脑认证系统验证码邮件", msg);
            }
            catch(Exception)
            {
               
            }


        }

        private void Label(bool look)
        {
            label1.Visible = look;
            label2.Visible = look;
            label3.Visible = look;
        }

        int count = 60; //读秒

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.Text = count-- + "s后重试";

            if (count <= 0)
            {
                timer1.Stop();
                count = 60;
                button1.Enabled = true;
                button1.Text = "获取验证码";
                Label(false);
            }
        }

        /// <summary>
        /// 动态生成指定数目的随机数或字母
        /// </summary>
        /// <param name="num">整数</param>
        /// <returns>返回验证码字符串</returns>
        private string GenerateCheckCode(int num)
        {
            int number;  //定义变量
            char code;
            string checkCode = String.Empty;  //空字符串，只读
            Random random = new Random();    //定义随机变量实例
            for (int i = 0; i < num; i++)
            {
                //利用for循环生成指定数目的随机数或字母
                number = random.Next(); //返回一个小于指定的最大值的非负的随机数 next有三个构造函数 
                if (number % 2 == 0)
                {
                    //产生一个一位数
                    code = (char)('0' + (char)(number % 10));
                }
                else
                {
                    //产生一个字母
                    code = (char)('C' + (char)(number % 26));
                }
                checkCode += code.ToString();
            }
            return checkCode;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Equals(""))
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(!textBox1.Text.Equals("")&&e.KeyCode == Keys.Enter)
            {
                this.button2_Click(null, null);
            }
        }

        private void Form10_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

    }
}
