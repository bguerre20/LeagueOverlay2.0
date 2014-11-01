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

namespace League_of_Legends_Overlay
{
    public partial class Form1 : Form
    {
        bool start = true;
        League_State_Checker lsc = new League_State_Checker();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Start Button
        private void button1_Click(object sender, EventArgs e)
        {
            if (start)
            {
                button1.Text = "PAUSE";
                start = false;
                lsc.start();
            }
            else
            {
                button1.Text = "START";
                start = true;
                lsc.stop();

            }
                
        }

        #region Dragging window without title bar
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
       
        #endregion

    }
}
