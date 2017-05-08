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

namespace AirlineBillingReport.Operations
{
    public partial class UnbilledMonitoringByAgent : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public UnbilledMonitoringByAgent(string agentName, string travCom1, string travCom2, 
            string travCom3, string travCom4, string travCom5)
        {
            InitializeComponent();

            LoadReport(agentName, travCom1, travCom2, travCom3, travCom4, travCom5);
        }
        
        private void LoadReport(string agentName, string travCom1, string travCom2, string travCom3, string travCom4, string travCom5)
        {
            using (var db = new AirlineBillingReportEntities())
            {
                unbilledMonitoringByAgentBindingSource.DataSource = db.UnbilledMonitoringByAgent(travCom1,
                    travCom2, travCom3, travCom4, travCom5);

               ReportParameter[] rParams = new ReportParameter[]
               {
                    new ReportParameter("AgentName",agentName),
                    new ReportParameter("DateToday", DateTime.Now.ToString())
               };

                reportViewer.LocalReport.SetParameters(rParams);

                reportViewer.RefreshReport();
            }
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void UnbilledMonitoringByAgent_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
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
            WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UnbilledMonitoringByAgent_Resize(object sender, EventArgs e)
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
    }
}
