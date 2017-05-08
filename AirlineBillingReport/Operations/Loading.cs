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

namespace AirlineBillingReport.Operations
{
    public partial class Loading : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        public Loading(string message)
        {
            InitializeComponent();
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label1_MouseDown_1(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void Loading_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void Loading_Load(object sender, EventArgs e)
        {
        }
    }
}
