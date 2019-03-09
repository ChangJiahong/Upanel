using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

using System.Data.OleDb;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HookLock
{
    static class Program
    {

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string frmText);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]

        public static extern int ShowWindow(IntPtr hwnd, int showWay);

        public static IntPtr frmHwnd;


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 下面的代码，放到该放的地方去
            try
            {
                //MessageBox.Show(Process.GetCurrentProcess().ProcessName.ToString() + "程序已启动!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                Process[] pros = Process.GetProcessesByName("U盘认证系统");
                if (pros.Length > 1)//存在该进程
                {

                    IntPtr handle = pros[0].MainWindowHandle;
                    //MessageBox.Show(Process.GetCurrentProcess().ToString()+"程序已启动!" + pros.Length + "   ___" + handle.ToInt32(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (handle.ToInt32() == 0)
                    {
                        frmHwnd = FindWindow(null, "Form4");
                        ShowWindow(frmHwnd, 9);//显示窗体
                        
                    }
                    else
                    {
                        //ShowWindow(handle, 0);//  隐藏窗体
                    }
                }
                else//不存在，则启动该程序
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form4());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示");
            }



            
            //Form1 f1 = new Form1();
            //if (f1.U_Disks1())
            {
                //Application.Run(new Form4());
            }
           // else
                //Application.Run(new Form2());

        }



    }
}