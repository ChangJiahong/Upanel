using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form2 : Form
    {
        Form1 f1 ;
        /// <summary>
        /// U盘验证是否成功
        /// </summary>
        public bool Key = false;

        static string filePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\data.accdb";

        public Form2()
        {
            InitializeComponent();
            /*
            //屏幕宽
            int iWidth = Screen.PrimaryScreen.Bounds.Width;
            //屏幕高
            int iHeight = Screen.PrimaryScreen.Bounds.Height;
            //按照屏幕宽高创建位图
            Image img = new Bitmap(iWidth, iHeight);
            //从一个继承自Image类的对象中创建Graphics对象
            Graphics gc = Graphics.FromImage(img);
            //抓屏并拷贝到myimage里
            gc.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));

            //文件路径
            //string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\123.jpg";
            //img.Save(@str1);

            //string[] image = Directory.GetFiles(str);
            //this.BackgroundImage = Image.FromFile(@image[i++]);
            //this.BackgroundImage = Image.FromFile(@str);
            this.BackgroundImage = img;
             * */
            
            f1 = new Form1(this);

        }

        

        private void Form2_Load(object sender, EventArgs e)
        {
            
            run();
            Thread t = new Thread(ShowForm);
            t.Start();   
            
        }

        private void ShowForm()
        {
            Thread.Sleep(2);
            f1.ShowDialog(); 
            
        }

        private void run()
        {

            if (!File.Exists(filePath))
            {
                //创建数据库
                ADOX.CatalogClass Student = new ADOX.CatalogClass();
                Student.Create("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;");

                //新建一个表[user]
                ADOX.TableClass user = new ADOX.TableClass();
                user.ParentCatalog = Student;
                user.Name = "usb";

                //增加一个自动增长的字段ID
                ADOX.ColumnClass ID = new ADOX.ColumnClass();
                ID.ParentCatalog = Student;
                ID.Type = ADOX.DataTypeEnum.adInteger; // 必须先设置字段类型
                ID.Name = "ID";
                ID.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                ID.Properties["AutoIncrement"].Value = true;
                user.Columns.Append(ID, ADOX.DataTypeEnum.adInteger, 0);

                //增加一个文本字段username
                ADOX.ColumnClass username = new ADOX.ColumnClass();
                username.ParentCatalog = Student;
                username.Name = "U盘名";
                username.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                user.Columns.Append(username, ADOX.DataTypeEnum.adVarChar, 20);

                //增加一个文本字段U——ID
                ADOX.ColumnClass U_ID = new ADOX.ColumnClass();
                U_ID.ParentCatalog = Student;
                U_ID.Name = "U盘序列号";
                U_ID.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                user.Columns.Append(U_ID, ADOX.DataTypeEnum.adVarChar, 20);

                //增加一个文本字段 密文password
                ADOX.ColumnClass U_password = new ADOX.ColumnClass();
                U_password.ParentCatalog = Student;
                U_password.Name = "U盘密文";
                U_password.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                user.Columns.Append(U_password, ADOX.DataTypeEnum.adLongVarChar, 20);

                //增加一个文本字段 密文路径
                ADOX.ColumnClass fileName = new ADOX.ColumnClass();
                fileName.ParentCatalog = Student;
                fileName.Name = "密文路径";
                fileName.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                user.Columns.Append(fileName, ADOX.DataTypeEnum.adVarChar, 20);

                //把表加进数据库
                Student.Tables.Append(user);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(user);

                //新建一个表[mail]
                ADOX.TableClass mail = new ADOX.TableClass();
                mail.ParentCatalog = Student;
                mail.Name = "Email";

                //增加一个文本字段mail
                ADOX.ColumnClass mail_name = new ADOX.ColumnClass();
                mail_name.ParentCatalog = Student;
                mail_name.Name = "Emial";
                mail_name.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                mail.Columns.Append(mail_name, ADOX.DataTypeEnum.adVarChar, 20);

                Student.Tables.Append(mail);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mail);

                //新建一个表[Log]
                ADOX.TableClass Log = new ADOX.TableClass();
                Log.ParentCatalog = Student;
                Log.Name = "Log";

                //增加一个自动增长的字段ID
                ADOX.ColumnClass log_ID = new ADOX.ColumnClass();
                log_ID.ParentCatalog = Student;
                log_ID.Type = ADOX.DataTypeEnum.adInteger; // 必须先设置字段类型
                log_ID.Name = "ID";
                log_ID.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                log_ID.Properties["AutoIncrement"].Value = true;
                Log.Columns.Append(log_ID, ADOX.DataTypeEnum.adInteger, 0);

                //增加一个文本字段  日期
                ADOX.ColumnClass date = new ADOX.ColumnClass();
                date.ParentCatalog = Student;
                date.Name = "日期";
                date.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                Log.Columns.Append(date, ADOX.DataTypeEnum.adDate, 20);

                //增加一个文本字段 type
                ADOX.ColumnClass type = new ADOX.ColumnClass();
                type.ParentCatalog = Student;
                type.Name = "操作";
                type.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                Log.Columns.Append(type, ADOX.DataTypeEnum.adVarChar, 20);

                //增加一个文本字段 方式
                ADOX.ColumnClass 方式 = new ADOX.ColumnClass();
                方式.ParentCatalog = Student;
                方式.Name = "方式";
                方式.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                Log.Columns.Append(方式, ADOX.DataTypeEnum.adVarChar, 20);

                Student.Tables.Append(Log);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Log);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(Student);

                user = null;
                mail = null;
                Log = null;
                Student = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            //isShake = true;//抖动
            
            /*
                SoundPlayer player = new SoundPlayer();
                string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\Windows Background.wav";
                //MessageBox.Show(str);
                if (File.Exists(@str))
                {

                    player.SoundLocation = str;
                    player.Load();
                    player.Play();
                    
                }
            f1.Shake(); //窗口抖动
                    //Form2.isShake = false;
             * */

            //f1.ShowDialog(); 
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            this.Dispose();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

    }
}
