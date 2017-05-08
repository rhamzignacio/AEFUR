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

namespace AirlineBillingReport
{
    public partial class BilledReport : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public BilledReport(string _recordNo)
        {
            InitializeComponent();

            LoadReport(_recordNo);
        }

        private void LoadReport(string _recordNo)
        {
            using (var db = new AirlineBillingReportEntities())
            {
                airlineBillingReportDataSetBindingSource.DataSource =
                    db.BilledSummaryReport(_recordNo);

                reportViewer1.RefreshReport();
            }
        }

        private void BilledReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'AirlineBillingReportDataSet.BilledSummaryReport' table. You can move, or remove it, as needed.
            this.reportViewer1.RefreshReport();
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }


        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }


        private void btnMaximized_Click_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_tick_blank;
            }
            else
            {
                WindowState = FormWindowState.Maximized;

                btnMaximized.Image = null;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_duplicate;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
             WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BilledReport_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_duplicate;
            }
            else
            {
                WindowState = FormWindowState.Normal;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_tick_blank;
            }
        }

        private void label7_MouseDown_1(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }
    }
}
