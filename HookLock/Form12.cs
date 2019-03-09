using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form12 : Form
    {
        public String U_name = "加载中...";
        public bool isOpen_U = false;
        public String tag = "";
        public Point location = new Point(Screen.PrimaryScreen.Bounds.Width  - 300, 50);

        public Form12(String U_name)
        {
            InitializeComponent();
            this.U_name = U_name;
        }

        public Form12()
        {
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            //this.Top = 50;
            //this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width - 50;
            this.Location = location;
            label1.Text = this.U_name;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Point formPoint = this.PointToClient(Control.MousePosition);
            //System.Console.WriteLine(formPoint.X+","+formPoint.Y);
            if (formPoint.X >= 0 && formPoint.X <= 188 && formPoint.Y >= 0 && formPoint.Y <= 83)
            {
                if (panel2.Location.Y < -3)
                {
                    panel2.Location = new Point(panel2.Location.X, panel2.Location.Y + 3);
                    //System.Console.WriteLine(panel2.Location.Y);
                }

            }
            else if (panel2.Location.Y > -89)
            {
                panel2.Location = new Point(panel2.Location.X, panel2.Location.Y - 3);
            }

        }

        private Point ptMouseCurrrnetPos, ptFormPos, ptFormNewPos;
        private bool blnMouseDown = false;


        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                //Get the current position of the mouse in the screen
                Point ptMouseNewPos = Control.MousePosition;

                //Set window position
                ptFormNewPos.X = ptMouseNewPos.X - ptMouseCurrrnetPos.X + ptFormPos.X;
                ptFormNewPos.Y = ptMouseNewPos.Y - ptMouseCurrrnetPos.Y + ptFormPos.Y;

                //Save window position
                Location = ptFormNewPos;
                ptFormPos = ptFormNewPos;

                //Save mouse position
                ptMouseCurrrnetPos = ptMouseNewPos;
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                //Return back signal
                blnMouseDown = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = true;

                // Save window position and mouse position
                ptMouseCurrrnetPos = Control.MousePosition;
                ptFormPos = Location;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //打开U盘
            System.Diagnostics.Process.Start(tag);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\2.png";
            button1.BackgroundImage = Properties.Resources._2;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            String str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\1.png";
            button1.BackgroundImage = Properties.Resources._1; 
        }

        public const uint GENERIC_READ = 0x80000000;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
         string lpFileName,
         uint dwDesireAccess,
         uint dwShareMode,
         IntPtr SecurityAttributes,
         uint dwCreationDisposition,
         uint dwFlagsAndAttributes,
         IntPtr hTemplateFile);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped
        );

        private void button2_Click(object sender, EventArgs e)
        {
            //卸载U盘
            //第一个参数filename与普通文件名有所不同，设备驱动的“文件名”(常称为“设备路径”)形式固定为“\\.\DeviceName”, 如 string filename = @"\\.\I:";
            string filename = @"\\.\" + tag.Remove(2);
            //打开设备，得到设备的句柄handle.
            IntPtr handle = CreateFile(filename, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, 0x3, 0, IntPtr.Zero);

            // 向目标设备发送设备控制码，也就是发送命令。IOCTL_STORAGE_EJECT_MEDIA  弹出U盘。
            uint byteReturned;
            bool result = DeviceIoControl(handle, IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);

            new Form3(result ? "U盘已退出" : "U盘退出失败").Show();
            if (result)
            {
                this.Close();
            }
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
    }
}
