using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form9 : Form
    {

        String Email_num; //邮箱号码
        String str = "";//验证码
        string filePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\data.accdb";

        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
            conn.Open();

            string str = "select * from [Email]";
            OleDbCommand cmd = new OleDbCommand(str, conn);
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                Email_num = reader["Emial"].ToString();

            conn.Close();

            //Email_num = "2327085154@qq.com";
            label2.Text = ReturnEmail(Email_num);
            button5.Enabled = false;
        }

        /// <summary> 
        /// 替换邮箱中间几位为*号  
        /// </summary>  
        /// <param name="Email"></param>  
        /// <returns></returns>  
        protected static string ReturnEmail(string Email)
        {
            Regex re = new Regex(@"\w{3}(?=@\w+?.\S+)", RegexOptions.None);
            Email = re.Replace(Email, "***");
            return Email;
        }

        //更换
        private void button1_Click(object sender, EventArgs e)
        {
            Show1(false);
            Show2(true);
        }

        private void Show1(bool look)
        {
            label1.Visible = look;
            label2.Visible = look;
            button1.Visible = look;
        }

        private void Show2(bool look)
        {
            label3.Visible = look;
            textBox1.Visible = look;
            button2.Visible = look;
            button3.Visible = look;
        }

        private void Show3(bool look)
        {
            textBox2.Visible = look;
            button4.Visible = look;
            button5.Visible = look;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Show2(false);
            Show1(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("邮箱号码不能为空");
                return;
            }

            //检查邮箱格式准确性
            if (!check(textBox1.Text))
            {
                MessageBox.Show("请输入正确的邮箱地址");
                return;
            }

            if(textBox1.Text.Equals(Email_num))
            {
                MessageBox.Show("邮箱号码与之前重复！");
                return ;
            }
            Show2(false);
            Show3(true);
        }

        //确定
        private void button5_Click(object sender, EventArgs e)
        {

            if (textBox2.Text.Equals(str))
            {
                //更改邮箱
                OleDbConnection conn = new OleDbConnection();
                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
                conn.Open();


                string str1 = "update [Email] set [Emial] = '" + textBox1.Text+"'";
                OleDbCommand cmd = new OleDbCommand(str1, conn);
                int k = cmd.ExecuteNonQuery();

                string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','修改邮箱" + Email_num + "为"+ textBox1.Text+"')";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                comm.ExecuteNonQuery();

                conn.Close();
                if(k>0)
                    MessageBox.Show("更改成功");
                this.Close();
            }
            else
            {
                MessageBox.Show("验证码错误！");
            }
        }

        

        //获取验证码
        private void button4_Click(object sender, EventArgs e)
        {


            button4.Enabled = false;
            timer1.Start();
            //Label(true);

            //生成 6位 验证码
            str = GenerateCheckCode(6);
            //发送邮件
            String msg = "  你好，你的验证码是：" + str + "  ，在一分钟内有效，如非本人操作请忽略此邮件。";
            try
            {

                Email.send_Email("321168813@qq.com", textBox1.Text, "oojlutazjvckbifa", "电脑认证系统验证码邮件", msg);
            }
            catch
            {
            }
        }

        int count = 60; //读秒

        private void timer1_Tick(object sender, EventArgs e)
        {
            button4.Text = count-- + "s后重试";

            if (count <= 0)
            {
                timer1.Stop();
                count = 60;
                button4.Enabled = true;
                button4.Text = "获取验证码";
                //Label(false);
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


        /// <summary>
        /// 检查邮箱地址
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        bool check(string strIn)
        {
            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (!textBox2.Text.Equals("") && e.KeyCode == Keys.Enter)
            {
                this.button5_Click(null, null);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                button5.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        } 



    }
}
