using AirlineBillingReportRepository;
using Microsoft.Reporting.WinForms;
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


namespace AirlineBillingReport
{
    public partial class IATAPreparationReport : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        public IATAPreparationReport(string recordNo)
        {
            InitializeComponent();

            LoadReport(recordNo);
        }

        private void LoadReport(string recordNo)
        {
            using (var db = new AirlineBillingReportEntities())
            {
                var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == recordNo);

                ReportParameter[] rParams = new ReportParameter[]
                {
                    new ReportParameter("ReferenceNo", record.IATAReference),
                    new ReportParameter("DateRange", record.IATADateRange)
                };

                reportViewer.LocalReport.SetParameters(rParams);

                ticketedIATABindingSource.DataSource = db.TicketedIATA(recordNo);

                refundsIATABindingSource.DataSource = db.RefundsIATA(recordNo);

                debitMemoIATABindingSource.DataSource = db.DebitMemoIATA(recordNo);

                unpostedIATABindingSource.DataSource = db.UnpostedIATA(recordNo);

                creditMemoIATABindingSource.DataSource = db.CreditMemoIATA(recordNo);

                reportViewer.RefreshReport();
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximized_Click(object sender, EventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void IATAPreparationReport_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_duplicate;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Normal;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_tick_blank;
            }
        }

        private void IATAPreparationReport_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
        }

        private void AirlineBillingReportDataSetBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
