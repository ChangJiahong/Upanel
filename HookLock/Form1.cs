using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Media;
using System.IO;
using System.Management;
using System.Data.OleDb;
///---添加名称空间  
using AForge;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using Size = System.Drawing.Size;
using System.Windows.Media.Imaging;
using System.Windows;
using AForge.Imaging;

namespace HookLock
{
    public partial class Form1 : Form
    {
        Form2 f2;
        //Form4 f4 = new Form4();
        static string filePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\data.accdb";
        String pf = "";

        String Email_num = "";
        String U_name = "unknow";

        ///---声明变量  
        private static FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        public int selectedDeviceIndex = 0;
        String path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\image\\";
        
        //文件路径
        List<String> images = new List<string> () ;


        static int Count = 0; //错误次数
        bool OnceTime = true;
        int Img_count = 0; //照片数

        public Form1(Form2 f2)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.f2 = f2;
            //屏幕宽
            int iWidth = Screen.PrimaryScreen.Bounds.Width;
            //屏幕高
            int iHeight = Screen.PrimaryScreen.Bounds.Height;
            //按照屏幕宽高创建位图
            System.Drawing.Image img = new Bitmap(iWidth, iHeight);
            //从一个继承自Image类的对象中创建Graphics对象
            Graphics gc = Graphics.FromImage(img);
            //抓屏并拷贝到myimage里
            gc.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new Size(iWidth, iHeight));

            //文件路径
            //string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\123.jpg";
            //img.Save(@str1);

            //string[] image = Directory.GetFiles(str);
            //this.BackgroundImage = Image.FromFile(@image[i++]);
            //this.BackgroundImage = Image.FromFile(@str);
            this.BackgroundImage = img;
        }

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            //屏幕宽
            int iWidth = Screen.PrimaryScreen.Bounds.Width;
            //屏幕高
            int iHeight = Screen.PrimaryScreen.Bounds.Height;
            //按照屏幕宽高创建位图
            System.Drawing.Image img = new Bitmap(iWidth, iHeight);
            //从一个继承自Image类的对象中创建Graphics对象
            Graphics gc = Graphics.FromImage(img);
            //抓屏并拷贝到myimage里
            gc.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new Size(iWidth, iHeight));

            //文件路径
            //string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\123.jpg";
            //img.Save(@str1);

            //string[] image = Directory.GetFiles(str);
            //this.BackgroundImage = Image.FromFile(@image[i++]);
            //this.BackgroundImage = Image.FromFile(@str);
            this.BackgroundImage = img;
        }


        Hook h = new Hook();


        private void Form1_Load(object sender, EventArgs e)
        {
            //控件布局加载
            windows_init();


            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
            conn.Open();

            string str = "select * from [Email]";
            OleDbCommand cmd = new OleDbCommand(str, conn);
            OleDbDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            Email_num = reader["Emial"].ToString();

            conn.Close();

            h.InstallHook();
            GetDevices();

            //窗口显示后启动程序
            //Shown += U_Disks;

            //panel2.Top = 50;
            //panel2.Left = Screen.PrimaryScreen.Bounds.Width - 300;


        }

        //窗口布局
        private void windows_init()
        {
            //this.WindowState = FormWindowState.Maximized;
            panel1.Location = new System.Drawing.Point((this.Width - panel1.Width) / 2, (this.Height - panel1.Height) / 2);
            button2.Location = new System.Drawing.Point(225, 273);
            button3.Location = new System.Drawing.Point(373, 273);
            label1.Location = new System.Drawing.Point(207, 117);
        }


        /// <summary>
        /// 暂时没用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void U_Disks(object sender, EventArgs e)
        {
            if (GetID().Length != 0)
            {
                int i;
                //MessageBox.Show(GetID().Length+"\n"+GetID()[0]+"\n"+GetID()[1]);
                String[] ID = GetID();
                for(i=0;i<ID.Length;i++)
                {
                    //查找数据库
                    OleDbConnection conn = new OleDbConnection();
                    conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
                    conn.Open();

                    string str = "select * from [usb] where [U盘序列号]='" + ID[i] + "'";
                    OleDbCommand cmd = new OleDbCommand(str, conn);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    //待写。。。考虑在数据库中加入密文_文件名 以便查找

                    if (reader.Read())  //序列号正确
                    {
                        String file = Read(pf + reader["密文路径"].ToString());
                        //MessageBox.Show(file);
                        if (file.Equals(reader["U盘密文"])) //密文正确
                        {
                            //new Form3("U盘已认证").ShowDialog();  
                            //System.Environment.Exit(0); 
                            f2.Key = true;

                            string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','" + reader["U盘名"] + "U盘登陆成功')";
                            OleDbCommand comm = new OleDbCommand(sql, conn);
                            int k = comm.ExecuteNonQuery();
                            conn.Close();

                            break;  //匹配成功返回
                            //此处有bug
                        }
                    }
                    conn.Close();
                    
                }


                if (i < ID.Length)
                {
                    //f2.Hide();
                    //this.TopMost = false;
                    //h.UnInstallHook();
                    //this.Hide();
                    //f4.ShowDialog();

                    new Form3("U盘已认证").ShowDialog();
                    System.Environment.Exit(0); 
                }
                else
                {
                    new Form3("认证失败\n请检查U盘").ShowDialog();
                    label1.Text = "请插入认证U盘";
                    Count++;
                    //break;
                }
            }
        }

        public void U_Disks()
        {
            //查找数据库
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
            conn.Open();

            if (GetID().Length != 0)
            {
                int i;
                //MessageBox.Show(GetID().Length+"\n"+GetID()[0]+"\n"+GetID()[1]);
                String[] ID = GetID();
                for (i = 0; i < ID.Length; i++)
                {
                    //MessageBox.Show(i+"");

                    string str = "select * from [usb] where [U盘序列号]='" + ID[i] + "'";
                    OleDbCommand cmd = new OleDbCommand(str, conn);
                    OleDbDataReader reader = cmd.ExecuteReader();
                    //待写。。。考虑在数据库中加入密文_文件名 以便查找
                    

                    if (reader.Read())  //序列号正确
                    {
                        String file = Read(pf + reader["密文路径"].ToString());
                        //MessageBox.Show(file);
                        if(file.Equals(reader["U盘密文"])) //密文正确
                        {
                            //new Form3("U盘已认证").ShowDialog();  
                            //System.Environment.Exit(0); 
                            f2.Key = true;

                            string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','"+reader["U盘名"]+"U盘登陆成功')";
                            OleDbCommand comm = new OleDbCommand(sql, conn);
                            int k = comm.ExecuteNonQuery();
                            conn.Close();

                            break;  //匹配成功返回
                            //此处有bug
                        }
                        
                    }

                }

                if (i < ID.Length)
                {
                    //f2.Hide();
                    //this.TopMost = false;
                    //h.UnInstallHook();
                    //this.Hide();
                   // f4.ShowDialog();
                    new Form3("U盘已认证").ShowDialog();

                    Form4.pf = pf;
                    //Application.Exit();
                    conn.Close();
                    this.Close();
                    //System.Environment.Exit(0); 

                }
                else
                {
                    new Form3("认证失败\n请检查U盘").ShowDialog();
                    label1.Text = "请插入认证U盘";
                    Count++;

                    string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','" + U_name + "U盘登陆失败')";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    int k = comm.ExecuteNonQuery();
                    conn.Close();

                    //break;
                }


            }

            conn.Close();

        }


        public String Read(string path)
        {
            try
            {
                String U_pass = "";
                if (path.Equals(""))
                    return "";
                else
                {
                    StreamReader sr = new StreamReader(path, Encoding.Default);
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        U_pass += line.ToString();
                    }
                    return U_pass;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public bool U_Disks1()
        {
            //查找数据库
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
            conn.Open();

            if (GetID().Length != 0)
            {
                int i;
                //MessageBox.Show(GetID().Length+"\n"+GetID()[0]+"\n"+GetID()[1]);
                String[] ID = GetID();
                for (i = 0; i < ID.Length; i++)
                {
                    //MessageBox.Show(i+"");
                    

                    string str = "select * from [usb] where [U盘序列号]='" + ID[i] + "'";
                    OleDbCommand cmd = new OleDbCommand(str, conn);
                    OleDbDataReader reader = cmd.ExecuteReader();
                    
                    //待写。。。考虑在数据库中加入密文_文件名 以便查找

                    if (reader.Read())  //序列号正确
                    {
                        String file = Read(pf + reader["密文路径"].ToString());
                        //MessageBox.Show(file);
                        if (file.Equals(reader["U盘密文"])) //密文正确
                        {
                            //new Form3("U盘已认证").ShowDialog();  
                            //System.Environment.Exit(0); 
                            //f2.Key = true;

                            string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','" + reader["U盘名"] + "U盘登陆成功')";
                            OleDbCommand comm = new OleDbCommand(sql, conn);
                            int k = comm.ExecuteNonQuery();
                            conn.Close();

                            break;  //匹配成功返回
                            //此处有bug
                        }
                    }

                }

                if (i < ID.Length)
                {
                    //this.TopMost = false;
                    //h.UnInstallHook();
                    //this.Hide();
                    //f4.ShowDialog();

                    Form4.pf = pf;

                    return true;
                }
                else
                {
                    Count++;

                    string sql = "insert into [Log](日期,操作)values('" + DateTime.Now + "','" + U_name + "U盘登陆失败')";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    int k = comm.ExecuteNonQuery();
                    conn.Close();

                    return false;
                }


            }
            conn.Close();
            return false;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
             Process [] p = Process.GetProcesses();

            foreach (Process p1 in p)
            {
                try
                {
                    if (p1.ProcessName.ToLower().Trim() == "taskmgr") 
                    {
                        p1.Kill();
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }

            if (Count >= 2)
            {
                VideoConnect();  //连接相机
                Count = 0; //归零
            }

            //抓拍照片判断

            if (videoSourcePlayer.IsRunning && OnceTime)
            {
                System.Timers.Timer t = new System.Timers.Timer(3000);   //实例化Timer类，设置间隔时间为10000毫秒； 
                t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //到达时间的时候执行事件；   
                t.AutoReset = false;   //设置是执行一次（false）还是一直执行(true)；   
                t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件； 
                OnceTime = false;
            }

            if (Img_count > 3)
            {

                //照片抓取完毕，关闭相机
                videoSourcePlayer.SignalToStop();
                videoSourcePlayer.WaitForStop();



                //
                //发送邮件,通知
                String msg = "  你好，这是在电脑认证系统重要文件，你的电脑正在被非法操作，如非本人操作请及时查看电脑。";
                try
                {

                    Email.send_Email("321168813@qq.com", Email_num, "oojlutazjvckbifa", "电脑认证系统验证码邮件", msg, images.ToArray());
                }
                catch
                {
                }

                images.Clear();
                Img_count = 0;
                OnceTime = true;
            }

            
        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            GrabBitmap();
            Img_count++;
            OnceTime = true;
        }  

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                //this.button1_Click(null, null);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            h.UnInstallHook();
            this.timer1.Stop();
            if(f2!=null)
                f2.Close();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        

        //窗口抖动
        public void Shake()
        {
            Random ran = new Random((int)DateTime.Now.Ticks);

            System.Drawing.Point point = panel1.Location;


            for (int i = 0; i < 5; i++)
            {
                panel1.Location = new System.Drawing.Point(point.X + ran.Next(8) - 4, point.Y + ran.Next(8) - 4);

                System.Threading.Thread.Sleep(15);

                panel1.Location = point;

                System.Threading.Thread.Sleep(15);
            }
        }

        List<Upanel> Us = new List<Upanel>();

        public const int WM_DEVICECHANGE = 0x219;//U盘插入后，OS的底层会自动检测到，然后向应用程序发送“硬件设备状态改变“的消息  
        public const int DBT_DEVICEARRIVAL = 0x8000;  //就是用来表示U盘可用的。一个设备或媒体已被插入一块，现在可用。  
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //一个设备或媒体片已被删除。  

        int top = 50;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                switch (m.WParam.ToInt32())
                {
                    case DBT_DEVICEARRIVAL://U盘插入  
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
                                if (i == Us.Count)
                                {
                                    pf = drive.Name.ToString(); //U盘  盘符

                                    Upanel U = new Upanel();
                                    U.tag = drive.Name.ToString();
                                    U.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - 300, top);
                                    top += 93;
                                    U.isOpen = false;
                                    U.u_name = drive.Name.ToString() + " " + drive.VolumeLabel + "\n(" + Math.Round(drive.AvailableFreeSpace / 1098907648.0, 2) + "G)可用";
                                    this.Controls.Add(U);
                                    Us.Add(U);


                                    label1.Text = "U盘已插入,正在检测...";
                                    new Form3("U盘已插入").ShowDialog();

                                    //Form4.pf = drive.Name.ToString(); //U盘  盘符


                                    if (f2.Key == false)   //key 为false 表示未验证成功
                                        this.U_Disks();  //U盘验证
                                }
                                //break;
                            }
                        }
                        break;
                    case DBT_DEVICEREMOVECOMPLETE: //U盘卸载  

                        top -= 93;
                        if (Us.Count==1)
                        {
                            Us[0].Dispose();
                            Us.Remove(Us[0]);
                        }
                        else if (Us.Count >= 2)
                       {
                           //找出拔出的U盘并释放
                           int i,j;
                           for (i = 0; i < Us.Count; i++)
                           {
                               DriveInfo[] s = DriveInfo.GetDrives();
                               for (j=0;j<s.Length;j++)
                               {
                                   if (s[j].DriveType == DriveType.Removable)
                                   {
                                       if(Us[i].tag.Equals(s[j].Name.ToString()))
                                        break;
                                   }
                               }
                               if (j == s.Length ) //表明没有找到对应盘符 Us[i]
                               {
                                   Us[i].Dispose();  //释放Us[i]
                                   Us.Remove(Us[i]);
                                   
                               }
                           }

                         }
                        new Form3("U盘已拔出").ShowDialog();  
                        label1.Text = "请插入认证U盘";
                        break;
                    default:
                        break;
                }
            }
            base.WndProc(ref m);
        }

        
        private List<string> _serialNumber = new List<string>();
        /// <summary>
        /// 调用这个函数将本机所有U盘序列号存储到_serialNumber中
        /// </summary>
        /// 
        /*
        private void matchDriveLetterWithSerial()
        {
            string[] diskArray;
            string driveNumber;
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");
            foreach (ManagementObject dm in searcher.Get())
            {
                getValueInQuotes(dm["Dependent"].ToString());
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                driveNumber = diskArray[0].Remove(0, 6).Trim();
                var disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject disk in disks.Get())
                {
                    if (disk["Name"].ToString() == ("\\\\.\\PHYSICALDRIVE" + driveNumber) & disk["InterfaceType"].ToString() == "USB")
                    {
                        _serialNumber.Add(parseSerialFromDeviceID(disk["PNPDeviceID"].ToString()));
                    }
                }
            }
        }
        private static string parseSerialFromDeviceID(string deviceId)
        {
            var splitDeviceId = deviceId.Split('\\');
            var arrayLen = splitDeviceId.Length - 1;
            var serialArray = splitDeviceId[arrayLen].Split('&');
            var serial = serialArray[0];
            return serial;
        }
        private static string getValueInQuotes(string inValue)
        {
            var posFoundStart = inValue.IndexOf("\"");
            var posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1);
            var parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);
            return parsedValue;
        }

         * */

        List<String> PF = new List<string>();

        private String[] GetID()
        {
            //matchDriveLetterWithSerial();
            U_ID();
            String[] str = _serialNumber.ToArray();
            
            return str;
        }

        /// <summary>
        /// 优化后获取U盘ID
        /// </summary>
        private void U_ID()
        {
            DriveInfo[] s = DriveInfo.GetDrives();
            foreach (DriveInfo drive in s)
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    //盘符
                    pf = drive.Name.ToString();
                    PF.Add(drive.Name.ToString());

                    //String s1 = drive.VolumeLabel.ToCharArray().ToString();
                    //break;
                }
            }
            ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo.Properties["InterfaceType"].Value.ToString() == "USB")
                {
                    try
                    {
                        //产品名称
                        U_name = mo.Properties["Caption"].Value.ToString();

                        //总容量
                        //Size.Text = mo.Properties["Size"].Value.ToString();


                        string[] info = mo.Properties["PNPDeviceID"].Value.ToString().Split('&');
                        //string s1 = mo.Properties["Name"].ToString() ;
                        string[] xx = info[3].Split('\\');
                        //序列号
                        //MessageBox.Show("U盘序列号:" + xx[1]);

                        //mo.Properties["Caption"].Value.ToString();
                        _serialNumber.Add(xx[1]);

                        //PNPDeviceID.Text = xx[1];
                        //xx = xx[0].Split('_');

                        //版本号
                        //REV.Text = xx[1];

                        //制造商ID
                        //xx = info[1].Split('_');
                        //VID.Text = xx[1];

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        public FilterInfoCollection GetDevices()
        {
            try
            {
                //枚举所有视频输入设备  
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count != 0)
                {
                    //LogClass.WriteFile("已找到视频设备.");
                    return videoDevices;
                }
                else
                    return null;
            }
            catch (Exception )
            {
                //LogClass.WriteFile("error:没有找到视频设备！具体原因：" + ex.Message);
                return null;
            }
        }

        /// <summary>  
        /// 连接视频摄像头  
        /// </summary>  
        /// <param name="deviceIndex"></param>  
        /// <param name="resolutionIndex"></param>  
        /// <returns></returns>  
        public VideoCaptureDevice VideoConnect(int deviceIndex = 0, int resolutionIndex = 0)
        {

            //MessageBox.Show(videoDevices.Count + "");

            if (videoDevices.Count <= 0)
                return null;
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            //videoSource.DesiredFrameSize = new System.Drawing.Size(214, 281);
            //videoSource.DesiredFrameRate = 1;

            videoSourcePlayer.VideoSource = videoSource;
            videoSourcePlayer.Start();

            return videoSource;
        }

        //抓图，拍照，单帧  

        public void GrabBitmap()
        {
            if (videoSource == null)
                return;
            //g_Path = path;

            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
        }

        void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            string fullPath = path;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            string img = fullPath + DateTime.Now.ToString("yyyyMMdd hhmmss") + ".jpg";
            images.Add(img);
            bmp.Save(img);
            //如果这里不写这个，一会儿会不停的拍照，  
            videoSource.NewFrame -= new NewFrameEventHandler(videoSource_NewFrame);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form10 f10 = new Form10(this);
            f10.ShowDialog();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\2.png";
            button2.BackgroundImage = Properties.Resources._2;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\1.png";
            button2.BackgroundImage = Properties.Resources._1;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\2.png";
            button3.BackgroundImage = Properties.Resources._2;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\1.png";
            button3.BackgroundImage = Properties.Resources._1;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //System.Environment.Exit(0);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            SoundPlayer player = new SoundPlayer();
            string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\Windows Background.wav";
            //MessageBox.Show(str);
            if (File.Exists(@str))
            {

                player.SoundLocation = str;
                player.Load();
                player.Play();

            }
            this.Shake(); //窗口抖动
        }



        private void Form1_Shown(object sender, EventArgs e)
        {
            DriveInfo[] s = DriveInfo.GetDrives();
            foreach (DriveInfo drive in s)
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    Upanel U = new Upanel();
                    U.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - 300, top);
                    top += 93;
                    U.isOpen = false;
                    U.tag = drive.Name.ToString();
                    U.u_name = drive.Name.ToString() + " " + drive.VolumeLabel + "\n(" + Math.Round(drive.AvailableFreeSpace / 1098907648.0,2) + "G)可用";
                    this.Controls.Add(U);
                    Us.Add(U);
                }
            }
        }

    }
}