using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookLock
{
    public partial class Form8 : Form
    {

        //文件路径
        string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data\\image\\";
        string[] image ;

        public Form8()
        {
            InitializeComponent();
            image = Directory.GetFiles(str);
        }

        private void Form8_Load(object sender, EventArgs e)
        {

           

            //listView1.View = View.LargeIcon;
            //listView1.LargeImageList = imageList1;

            load();
        }

        //Image imgdelete ;

        //List<Image> images = new List<Image>();

        //ImageList images = new ImageList();
        private void load()
        {

            for (int i = 0; i < image.Length; i++)
            {
                //images.Add(Image.FromFile(image[i]));
                Image img = Image.FromFile(image[i]);
                this.imageList1.Images.Add(img);
                img.Dispose();
                listView1.Items.Add("图片" + (i + 1));
                listView1.Items[i].ImageIndex = i;
            }
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.FocusedItem.ImageIndex;//焦点索引
            //MessageBox.Show(str);
            String path = image[index];

            Show_image(path);
             
        }

        //刷新
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            image = Directory.GetFiles(str);
            this.imageList1.Images.Clear();
            this.listView1.Items.Clear();
            load();
        }

        //打开
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int index = listView1.FocusedItem.ImageIndex;//焦点索引
                //MessageBox.Show(str);
                String path = image[index];
                Show_image(path);
            }
        }

        private void Show_image(String path)
        {
            //建立新的系统进程      
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //设置文件名，此处为图片的真实路径+文件名      
            process.StartInfo.FileName = path;
            //此为关键部分。设置进程运行参数，此时为最大化窗口显示图片。      
            process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll,ImageView_Fullscreen";
            //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true      
            process.StartInfo.UseShellExecute = true;
            //此处可以更改进程所打开窗体的显示样式，可以不设      
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            process.Start();
            process.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                toolStripMenuItem1.Enabled = true;
                toolStripMenuItem2.Enabled = true;
            }
            else
            {
                toolStripMenuItem1.Enabled = false;
                toolStripMenuItem2.Enabled = false;
            }
        }
        
        //删除
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                //遍历所有焦点的索引，删除
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    int index = listView1.SelectedItems[i].Index;
                    String path = image[index];
                    File.Delete(@path);
                }
                //int index = listView1.FocusedItem.ImageIndex;//焦点索引
                //MessageBox.Show(str);
                
                
                //刷新
                image = Directory.GetFiles(str);
                this.imageList1.Images.Clear();
                this.listView1.Items.Clear();
                load();
            }

        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
        }
    }
}
