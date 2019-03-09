using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form4 : Form
    {

        /// <summary>
        /// U盘验证是否成功
        /// </summary>
        //public static bool Key = false;

        /// <summary>
        /// 是否是验证码登陆
        /// </summary>
        public static bool isLogin = false;

        static string filePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\data.accdb";

        public static String pf = "C://";  //正确的盘符

        bool UisShow = true;

        Form2 f2;
        Form1 f1;

        public Form4()
        {
            InitializeComponent();
        }

        bool isHide = false;
        private void Form4_Load(object sender, EventArgs e)
        {
            Form1 f11 = new Form1();
            if (!f11.U_Disks1())
            {

                f2 = new Form2();
                f2.Show();
                //Thread t = new Thread(ShowForm);
                //f1 = new Form1();
                //f1.Show();
                //f1.Show();

                this.Visible = false;
                isHide = true;
            }
            f11.Close();
        }

        private void ShowForm()
        {
            //Thread.Sleep(2);
            f1 = new Form1();
            f1.Show();

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注意判断关闭事件Reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.ApplicationExitCall||e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                notifyIcon1.Visible = true;
                this.Hide();
                return;
            }

            notifyIcon1.Visible = false;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5(filePath);
            f5.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6(filePath);
            f6.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11(filePath);
            f11.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //contextMenuStrip1.Show();
        }

        private void 邮箱设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.ShowDialog();
           
        }

        private void 抓拍照片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.ShowDialog();
        }

        private void 日志记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11(filePath);
            f11.ShowDialog();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\2.png";
            button2.BackgroundImage = System.Drawing.Image.FromFile(@str);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\1.png";
            button2.BackgroundImage = System.Drawing.Image.FromFile(@str);
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\2.png";
            button3.BackgroundImage = System.Drawing.Image.FromFile(@str);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\1.png";
            button3.BackgroundImage = System.Drawing.Image.FromFile(@str);
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                contextMenuStrip1.Show((Button)sender, new Point(e.X, e.Y));
        }



        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)    //最小化到系统托盘
            {
                notifyIcon1.Visible = true;    //显示托盘图标
                this.Hide();    //隐藏窗口
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注意判断关闭事件Reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                notifyIcon1.Visible = true;
                this.Hide();
                return;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1_MouseClick(null, null);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString().Equals("Left"))
            {
                this.Show();
                WindowState = FormWindowState.Normal;
                this.Focus();
                this.Activate();
            }
        }


        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {

        }

        int top = 50;//初始位置
        public static List<Form12> Us = new List<Form12>();

        private void Form4_Shown(object sender, EventArgs e)
        {
            if (isHide)
            {
                this.Hide();
            }

            if (UisShow)
            {
                DriveInfo[] s = DriveInfo.GetDrives();
                foreach (DriveInfo drive in s)
                {
                    if (drive.DriveType == DriveType.Removable)
                    {
                        Form12 U = new Form12();
                        U.location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - 300, top);
                        top += 93;
                        //U.isOpen = false;
                        U.tag = drive.Name.ToString();
                        U.U_name = drive.Name.ToString() + " " + drive.VolumeLabel + "\n(" + Math.Round(drive.AvailableFreeSpace / 1098907648.0, 2) + "G)可用";
                        U.Show();
                        Us.Add(U);
                    }
                }
            }
        }

        private void 锁定ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            f2 = new Form2();
            f2.Show();

            //Thread t = new Thread(ShowForm);
            //f1 = new Form1();
            //f1.Show();
        }

        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
            this.Activate();
        }

        
        public const int WM_DEVICECHANGE = 0x219;//U盘插入后，OS的底层会自动检测到，然后向应用程序发送“硬件设备状态改变“的消息  
        public const int DBT_DEVICEARRIVAL = 0x8000;  //就是用来表示U盘可用的。一个设备或媒体已被插入一块，现在可用。  
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //一个设备或媒体片已被删除。  

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                switch (m.WParam.ToInt32())
                {
                    case DBT_DEVICEARRIVAL://U盘插入  
                        if (UisShow)
                        {
                            DriveInfo[] uin = DriveInfo.GetDrives();
                            foreach (DriveInfo drive in uin)
                            {
                                if (drive.DriveType == DriveType.Removable)
                                {
                                    int i;
                                    for (i = 0; i < Us.Count; i++)
                                    {
                                        if (Us[i].tag.Equals(drive.Name.ToString()))  //找到盘符
                                            break;
                                    }
                                    if (i == Us.Count)  //显示U盘悬浮窗
                                    {
                                        Form12 U = new Form12();
                                        U.tag = drive.Name.ToString();
                                        U.location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - 300, top);
                                        top += 93;
                                        //U.isOpen = false;
                                        U.U_name = drive.Name.ToString() + " " + drive.VolumeLabel + "\n(" + Math.Round(drive.AvailableFreeSpace / 1098907648.0, 2) + "G)可用";
                                        Us.Add(U);
                                        U.Show();
                                        

                                    }
                                    //break;
                                }
                            }
                        }
                        break;

                    case DBT_DEVICEREMOVECOMPLETE: //U盘卸载  
                        if (UisShow)
                        {
                            top -= 93;
                            if (Us.Count == 1)
                            {
                                Us[0].Dispose();
                            }
                            else if (Us.Count >= 2)
                            {
                                //找出拔出的U盘并释放
                                int i, j;
                                for (i = 0; i < Us.Count; i++)
                                {
                                    DriveInfo[] s = DriveInfo.GetDrives();
                                    for (j = 0; j < s.Length; j++)
                                    {
                                        if (s[j].DriveType == DriveType.Removable)
                                        {
                                            if (Us[i].tag.Equals(s[j].Name.ToString()))
                                                break;
                                        }
                                    }
                                    if (j == s.Length) //表明没有找到对应盘符 Us[i]
                                    {
                                        Us[i].Close();  //释放Us[i]
                                        Us[i].Dispose();
                                        Us.Remove(Us[i]);

                                    }
                                }

                            }
                        }

                        //new Form3("U盘已拔出").ShowDialog();
                        //label1.Text = "请插入认证U盘";
                        bool bo = true;
                        DriveInfo[] uin1 = DriveInfo.GetDrives();
                        //if (uin1.Equals(null))
                            //bo = true;
                        foreach (DriveInfo drive in uin1)
                        {
                                if (pf.Equals(drive.Name.ToString()))
                                {
                                    bo = false;
                                }
                                   // break;
                        }
                       if (f2!=null&&!f2.IsDisposed)
                       {
                            bo = false;
                        }


                        if (bo)
                        {
                            //Form1.Us
                            this.Hide();
                            f2 = new Form2();
                            f2.Show();
                            //f1 = new Form1();
                            //f1.Show();
                        }

                        break;
                    default:
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UisShow)
            {
                UisShow = false;
                toolStripMenuItem1.Text = "显示U盘悬浮窗";
                for (int i = 0; i < Us.Count; i++)
                {
                    top -= 93;
                    Us[i].Close();  //释放Us[i]
                    
                }
                Us.Clear();
            }
            else
            {
                UisShow = true;
                toolStripMenuItem1.Text = "取消U盘悬浮窗";

                DriveInfo[] s = DriveInfo.GetDrives();
                foreach (DriveInfo drive in s)
                {
                    if (drive.DriveType == DriveType.Removable)
                    {
                        Form12 U = new Form12();
                        U.location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - 300, top);
                        top += 93;
                        //U.isOpen = false;
                        U.tag = drive.Name.ToString();
                        U.U_name = drive.Name.ToString() + " " + drive.VolumeLabel + "\n(" + Math.Round(drive.AvailableFreeSpace / 1098907648.0, 2) + "G)可用";
                        U.Show();
                        Us.Add(U);
                    }
                }
            }
        }

        int count = 1; //分钟
        bool once = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isLogin&once)  //表示验证码登陆 开始计时 此代码执行一次
            {
                f2=null;
                timer2.Start();
                once = false;
                isLogin = false;
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            count--;
            if (count <= 0) //计时结束 恢复once
            {
                count = 5;
                timer2.Stop();
                once = true;
                if (!f2.IsDisposed)
                {
                    this.Hide();
                    f2 = new Form2();
                    f2.Show();
                }
            }
        }
        

    }
}
