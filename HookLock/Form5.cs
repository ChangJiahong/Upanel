using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form5 : Form
    {
        String filePath;
        String fileName = "";
        String ID = "";
        String name = "";
        String U_pass = "";
        String pf = "G:\\";
        USB this_usb;

        public struct USB
        {
            public String name;
            public String ID;
            public String pf;
        };

        List<USB> usbs = new List<USB>();

        public Form5(String filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i;
            USB[] uu = usbs.ToArray();
            name = comboBox1.Text;
            for (i = 0; i < uu.Length; i++)
            {
                if (name.Equals(uu[i].name))
                {
                    ID = uu[i].ID;
                    this_usb = uu[i];
                    pf = uu[i].pf;
                    break;
                }
            }

            //MessageBox.Show(name+"\n"+ID+"\n"+pf);
        }

        //确定
        private void button1_Click(object sender, EventArgs e)
        {
            //int i;
            USB[] uu = usbs.ToArray();
            if (fileName.ToArray()[0] != pf.ToArray()[0])
            {
                //MessageBox.Show("请选择"+pf.ToArray()[0]+"盘，即U盘内的文件");
                //return;
            }
            Read(fileName);  //获取U盘密文;

            //MessageBox.Show(name + "\n" + ID + "\n" + pf+"\n\n"+U_pass);
            if (name.Equals("")||fileName.Equals(""))
                MessageBox.Show("请先选择U盘");
            else
            {
                if (!ID.Equals(""))   //记录U盘序列号和U盘名称
                {
                    OleDbConnection conn = new OleDbConnection();
                    conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
                    conn.Open();

                    string str = "select * from [usb] where [U盘序列号]='" + ID + "'";
                    OleDbCommand cmd = new OleDbCommand(str, conn);
                    OleDbDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show("此U盘已认证");
                    }
                    else
                    {
                        String[] st = fileName.Split(':');
                        string sql = "insert into [usb](U盘名,U盘序列号,U盘密文,密文路径)values('" + name + "','" + ID + "','"+U_pass+"','"+st[1]+"')";
                        OleDbCommand comm = new OleDbCommand(sql, conn);
                        int k;
                        try
                        {

                            k = comm.ExecuteNonQuery();
                        }
                        catch (Exception )
                        {
                            MessageBox.Show(this,"文件太大，请保证文本字段在255以内","提示",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                            U_pass = "";
                            return;
                        }
                        //conn.Close();
                        if (k > 0)
                        {

                            string sql1 = "insert into [Log](日期,操作)values('" + DateTime.Now + "','添加" + name + "U盘认证成功')";
                            OleDbCommand com = new OleDbCommand(sql1, conn);
                            com.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("认证成功！");
                            this.Close();
                        }
                        else
                        {
                            string sql1 = "insert into [Log](日期,操作)values('" + DateTime.Now + "','添加" + name + "U盘认证失败')";
                            OleDbCommand com = new OleDbCommand(sql1, conn);
                            com.ExecuteNonQuery();
                            conn.Close();

                            MessageBox.Show("认证失败!");
                        }
                    }


                    conn.Close();
                }

            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Get_USB();
        }


        #region 获取U盘信息

        private void Get_USB()
        {

            USB usb = new USB();
            DriveInfo[] s = DriveInfo.GetDrives();
            foreach (DriveInfo drive in s)
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    //盘符
                    usb.pf = drive.Name.ToString();
                    break;
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
                        //mo.Properties["Caption"].Value.ToString();

                        //总容量
                        //mo.Properties["Size"].Value.ToString();


                        string[] info = mo.Properties["PNPDeviceID"].Value.ToString().Split('&');
                        string[] xx = info[3].Split('\\');
                        //序列号 xx[1]
                        //MessageBox.Show("U盘序列号:" + xx[1] + "\n" + xx[0]);

                        usb.name = mo.Properties["Caption"].Value.ToString();
                        usb.ID = xx[1];
                        usbs.Add(usb);
                        comboBox1.Items.Add(usb.name);

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
        #endregion


        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            name = "";
            textBox1.Text = "";
            Get_USB();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog DaKai = new OpenFileDialog();
            DaKai.InitialDirectory = pf;
            DaKai.Filter = "txt files(*.txt)|*.txt";
            DaKai.FilterIndex = 0;
            if (DaKai.ShowDialog() == DialogResult.OK)
            {
                fileName = DaKai.FileName;
                textBox1.Text = fileName;
            }

        }

        public void Read(string path)
        {
            if (path.Equals(""))
                return;
            else
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    U_pass += line.ToString();
                }
            }
        }

        
    }
}
