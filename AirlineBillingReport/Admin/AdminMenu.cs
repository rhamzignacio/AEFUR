using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirlineBillingReportRepository;
using System.Runtime.InteropServices;
using AirlineBillingReport.Operations;

namespace AirlineBillingReport.Admin
{
    public partial class AdminMenu : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        UserAccount user;

        public AdminMenu(UserAccount user)
        {
            InitializeComponent();

            this.user = user;
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void btnReportExtraction_Click(object sender, EventArgs e)
        {
            MainWindow form = new MainWindow(user);

            form.ShowDialog();
        }

        private void btnUnbilledMonitoring_Click(object sender, EventArgs e)
        {
            UnbilledMonitoring form = new UnbilledMonitoring(user, "", "", "", "", "");

            form.ShowDialog();
        }

        private void btnConfiguration_Click(object sender, EventArgs e)
        {
            Configuration form = new Configuration();

            form.ShowDialog();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void btnUserLogs_Click(object sender, EventArgs e)
        {
            UserLogs form = new UserLogs();

            form.Show();
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
