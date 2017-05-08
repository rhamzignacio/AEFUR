using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirlineBillingReportRepository.ViewModel;
using System.Runtime.InteropServices;

namespace AirlineBillingReport
{
    public partial class FormSelection : Form
    {
        string recordNo;

        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public FormSelection(string _recordNo)
        {
            InitializeComponent();

            recordNo = _recordNo;

            GetCount();            
        }

        private void GetCount()
        {
            lblBilledCount.Text = new BilledTicketViewModel().Count(recordNo).ToString();

            lblNoRecordCount.Text = new NoRecordViewModel().Count(recordNo).ToString();

            lblUnbilledCount.Text = new UnbilledTicketViewModel().Count(recordNo).ToString();

            lblTotal.Text = (int.Parse(lblBilledCount.Text) + int.Parse(lblNoRecordCount.Text) +
                int.Parse(lblUnbilledCount.Text)).ToString();

            if (lblBilledCount.Text == "0")
                btnBilled.Enabled = false;

            if (lblUnbilledCount.Text == "0")
                btnUnbilled.Enabled = false;

            if (lblNoRecordCount.Text == "0")
                btnNoRecord.Enabled = false;
        }

        private void btnBilled_Click(object sender, EventArgs e)
        {
            BilledReport form = new BilledReport(recordNo);

            form.Show();           
        }

        private void btnUnbilled_Click(object sender, EventArgs e)
        {
            UnbilledReport form = new UnbilledReport(recordNo);

            form.Show();
        }

        private void btnNoRecord_Click(object sender, EventArgs e)
        {
            NoRecordReport form = new NoRecordReport(recordNo);

            form.Show();
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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

        private void btnPreparation_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Not yet available for this version", "Warning");

            IATAPreparationReport form = new IATAPreparationReport(recordNo);

            form.ShowDialog();
        }

        private void btnIATAPreparationUSD_Click(object sender, EventArgs e)
        {
            IATAPreparationUSD form = new IATAPreparationUSD(recordNo);

            form.ShowDialog();
        }

        private void btnPALAPAnalysis_Click(object sender, EventArgs e)
        {
            PALAPAnalysisReport form = new PALAPAnalysisReport(recordNo);

            form.ShowDialog();
        }

        private void FormSelection_Load(object sender, EventArgs e)
        {
            using (var db = new AirlineBillingReportRepository.AirlineBillingReportEntities())
            {
                var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == recordNo);

                if (record != null)
                {
                    if (record.IATA == "" || record.IATA == null)
                    {
                        btnPreparation.Enabled = false;
                        btnIATAPreparationUSD.Enabled = false;
                    }
                    else
                    {
                        btnPreparation.Enabled = true;
                        btnIATAPreparationUSD.Enabled = true;
                    }

                    if (record.PAL == "" || record.PAL == null)
                        btnPALAPAnalysis.Enabled = false;
                    else
                        btnPALAPAnalysis.Enabled = true;

                    if (record.IASA == "" || record.IASA == null)
                        btnIASAAPAnalysisPHP.Enabled = false;
                    else
                        btnIASAAPAnalysisPHP.Enabled = true;

                    if (record.C5J == "" || record.C5J == null)
                        btnCebuPacific.Enabled = false;
                    else
                        btnCebuPacific.Enabled = true;
                }
            }
        }

        private void btnIASAAPAnalysisPHP_Click(object sender, EventArgs e)
        {
            var db = new AirlineBillingReportRepository.AirlineBillingReportEntities();

            var tempRecord = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == recordNo);

            IASAAPAnalysisReport form = new IASAAPAnalysisReport(recordNo, tempRecord.IASACurrency);

            form.ShowDialog();
        }

        private void btnCebuPacific_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Cebu Pacific AP Analysis in not yet available in this version");  

            CebuPacificAPAnalysisReport form = new CebuPacificAPAnalysisReport(recordNo);

            form.ShowDialog();
        }
    }
}
