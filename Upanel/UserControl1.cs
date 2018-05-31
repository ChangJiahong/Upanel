using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upanel
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private Point ptMouseCurrrnetPos, ptFormPos, ptFormNewPos;
        private bool blnMouseDown = false;


        private void panel3_MouseMove(object sender, MouseEventArgs e)
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

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                //Return back signal
                blnMouseDown = false;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = true;

                // Save window position and mouse position
                ptMouseCurrrnetPos = Control.MousePosition;
                ptFormPos = Location;
            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = Properties.Resources._2;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = Properties.Resources._1;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources._2;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources._1;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            System.Drawing.Point formPoint = panel1.PointToClient(Control.MousePosition);
            //System.Console.WriteLine(formPoint.X+","+formPoint.Y);
            if (formPoint.X >= 0 && formPoint.X <= 188 && formPoint.Y >= 0 && formPoint.Y <= 83)
            {
                if (panel3.Location.Y < -3)
                {
                    panel3.Location = new System.Drawing.Point(panel3.Location.X, panel3.Location.Y + 3);
                    //System.Console.WriteLine(panel2.Location.Y);
                }

            }
            else if (panel3.Location.Y > -89)
            {
                panel3.Location = new System.Drawing.Point(panel3.Location.X, panel3.Location.Y - 3);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

    }
}
