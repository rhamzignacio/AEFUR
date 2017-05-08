﻿using AirlineBillingReportRepository;
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
    public partial class PALAPAnalysisReport : Form
    {
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

        public PALAPAnalysisReport(string _recordNo)
        {
            InitializeComponent();

            //=====Load Report=====
            using (var db = new AirlineBillingReportEntities())
            {
                var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == _recordNo);

                ReportParameter[] rParams = new ReportParameter[]
                {
                    new ReportParameter("ReportDate", record.PALReportDate),
                    new ReportParameter("ReportTime", record.PALReportTime),
                    new ReportParameter("TotalRecord", record.PALTotalRecord),
                    new ReportParameter("PCC", record.PALPCC),
                    new ReportParameter("DateRange", record.PALDateRange)
                };

                reportViewer.LocalReport.SetParameters(rParams);

                unpostedPALAPBindingSource.DataSource = db.UnpostedPALAP(_recordNo);

                postedPALAPBindingSource.DataSource = db.PostedPALAP(_recordNo);

                reportViewer.RefreshReport();
            }
        }

        private void PALAPAnalysisReport_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PALAPAnalysisReport_Resize(object sender, EventArgs e)
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
    }
}
