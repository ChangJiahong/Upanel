using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using Size = System.Drawing.Size;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using AForge.Imaging;
using System.Threading;

namespace HookLock
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        ///---声明变量  
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        public int selectedDeviceIndex = 0;

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


            if (videoDevices.Count <= 0)
                return null;
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            //videoSource.DesiredFrameSize = new System.Drawing.Size(214, 281);
            //videoSource.DesiredFrameRate = 1;

            videoSourcePlayer.VideoSource = videoSource;
            videoSourcePlayer.Start();

            return videoSource;
        }



        //--窗口加载事件  

        private void Form7_Load(object sender, EventArgs e)
        {
            GetDevices();
            VideoConnect();
        }

        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
            this.Close();
        }
    }
}
