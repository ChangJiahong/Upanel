using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace HookLock
{
    class USB
    {
        /*

        String pf = ""; //盘符
        String U_name = "unknow";  //U盘名
        private List<string> _serialNumber = new List<string>();

        public String[] GetID()
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
                        U_name = mo.Properties["Caption"].Value.ToString();

                        //总容量
                        //Size.Text = mo.Properties["Size"].Value.ToString();


                        string[] info = mo.Properties["PNPDeviceID"].Value.ToString().Split('&');
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
                        //MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        public void U_Disks()
        {
            //查找数据库
            OleDbConnection conn = new OleDbConnection();
            //conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
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
                            //Form2.Key = true;

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
                    //f2.Hide();
                    //this.TopMost = false;
                    //h.UnInstallHook();
                    //this.Hide();
                    // f4.ShowDialog();
                    new Form3("U盘已认证").ShowDialog();
                    System.Environment.Exit(0);

                }
                else
                {
                    new Form3("认证失败\n请检查U盘").ShowDialog();
                    //label1.Text = "请插入认证U盘";
                    //Count++;

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

        public bool U_Disks1(Form1 f1)
        {
            //查找数据库
            OleDbConnection conn = new OleDbConnection();
            //conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Jet OLEDB:Database Password=2327085154;";
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
                            //Form2.Key = true;

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
                    //f1.TopMost = false;
                    //h.UnInstallHook();
                    //this.Hide();
                    //f4.ShowDialog();
                    return true;
                }
                else
                {
                    //Count++;

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

        */

        

    } // 类
}
