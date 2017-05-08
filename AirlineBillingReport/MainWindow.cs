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
using AirlineBillingReportRepository;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using AirlineBillingReport.LoadingForm;
using System.Threading;
using AirlineBillingReport.Setup;
using AirlineBillingReport.Class;
using System.Runtime.InteropServices;
using AirlineBillingReport.Operations;

using NsExcel = Microsoft.Office.Interop.Excel;
using AirlineBillingReport.Setup.Class;

namespace AirlineBillingReport
{
    public partial class MainWindow : Form
    {
        string recordNo = "";

        UserAccount user;

        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public MainWindow(UserAccount _user)
        {
            InitializeComponent();

            user = _user; //current user logged-in

            timer1.Start();

            lblUserName.Text = user.FirstName + " " + user.LastName;

            if (_user.AccessRights == "ACM")
                unbilledMonitoringToolStripMenuItem.Visible = true;
            else
                unbilledMonitoringToolStripMenuItem.Visible = false;
        }

        private void HideReportButtons()
        {
            
        }
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (user.AccessRights != "ADM")
                Application.Exit();
            else
                Close();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void btn5J_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx";

            openFileDialog.FilterIndex = 1;

            var config = new AirlineConfigurationViewModel().GetSelected("CEBUPACIFIC");

            if(config != null)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBox5J.Text = openFileDialog.FileName;                 
                }                
            }
            else
                MessageBox.Show("No configuration available for Cebu Pacific", "Error");
        }  

        public CebuPacificLoading loadingForm;
      
        public void StartProgress(string message)
        {
            loadingForm = new CebuPacificLoading(this, message);

            ShowProgress();
        }

        public void CloseProgress()
        {
            Thread.Sleep(200);

            loadingForm.Invoke(new Action(loadingForm.Close));
        }

        public void ShowProgress()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    try
                    {
                        loadingForm.ShowDialog();                        
                    }
                    catch
                    {

                    }
                }
                else
                {
                    Thread th = new Thread(ShowProgress);

                    th.IsBackground = false;

                    th.Start();
                }
            }
            catch
            {

            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtBox5J.Text != "" || txtBoxAirAsia.Text != "" || txtBoxIASA.Text != ""
                || txtBoxIATA.Text != "" || txtBoxPAL.Text != "")
            {
                recordNo = new RecordNoStorageViewModel().GenerateRecordNo(user.ID, txtBox5J.Text,
                    txtBoxAirAsia.Text, txtBoxIASA.Text, txtBoxIATA.Text, txtBoxPAL.Text);

                //One time query for back office record checking to improve performance
                var posted = new TravcomViewModel().GetAllPosted();

                var voided = new TravcomViewModel().GetAllUnposted();

                var unbilled = new TravcomViewModel().GetUnbilledTickets(true,true);

                var agent = new AgentCodeViewModel().GetAll();

                bool ShowReport = false;

                if (txtBox5J.Text != "")
                {
                    var cebuPac = new CebuPacificProcess();

                    cebuPac.Process(recordNo, txtBox5J.Text, this, posted, voided, agent);

                    ShowReport = true;
                }

                if(txtBoxIASA.Text !="")
                {
                    ShowReport = false;

                    var IASA = new IASAProcess();

                    IASA.Process(recordNo, txtBoxIASA.Text, this, posted, voided, agent);

                    ShowReport = true;
                }

                if(txtBoxPAL.Text != "")
                {
                    ShowReport = false;

                    var PAL = new PALProcess();

                    PAL.Process(recordNo, txtBoxPAL.Text, this, posted, voided, agent);

                   // StartPAL(txtBoxPAL.Text);

                    ShowReport = true;
                }

                if(txtBoxAirAsia.Text != "")
                {
                    ShowReport = false;

                    var Air = new AirAsiaProcess();

                    Air.Process(recordNo, txtBoxAirAsia.Text, this, posted, voided, agent);

                    ShowReport = true;
                }

                if(txtBoxIATA.Text != "")
                {
                    ShowReport = false;

                    var Iata = new IATAProcess();

                    Iata.Process(recordNo, txtBoxIATA.Text, this, posted, voided, agent);

                    ShowReport = true;
                }

                if (ShowReport)
                {
                    FormSelection form = new FormSelection(recordNo);

                    form.ShowDialog();

                    lblRecordNo.Text = "Record No: " + recordNo;

                    lblRecordNo.Visible = true;
                }
            }
            else
                MessageBox.Show("No file uploaded", "Warning");
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblRecordNo.Visible = false;

            txtBox5J.Text = txtBoxAirAsia.Text = txtBoxIASA.Text = 
            txtBoxIATA.Text = txtBoxPAL.Text = "";
        }

        private void biiledReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BilledReport form = new BilledReport(recordNo);

            form.Show();
        }

        private void retrieveRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void viaRecordNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RetrieveRecord form = new RetrieveRecord();

            form.ShowDialog();
        }

        private void recordListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordList form = new RecordList(user.ID, user.AccessRights);

            form.Show();
        }

        private void jToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CebuPacificConfiguration form = new CebuPacificConfiguration();

            form.ShowDialog();
        }

        private void btnIASA_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx";

            openFileDialog.FilterIndex = 1;

            var config = new AirlineConfigurationViewModel().GetSelected("IASA");

            if (config != null)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBoxIASA.Text = openFileDialog.FileName;
                }
            }
            else
                MessageBox.Show("No configuration available for IASA", "Error");
        }

        private void btnPAL_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx";

            openFileDialog.FilterIndex = 1;

            var config = new AirlineConfigurationViewModel().GetSelected("PAL");

            if (config != null)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBoxPAL.Text = openFileDialog.FileName;
                }
            }
            else
                MessageBox.Show("No configuration available for Philippine Airlines", "Error");
        }

        private void btnAirAsia_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx";

            openFileDialog.FilterIndex = 1;

            var config = new AirlineConfigurationViewModel().GetSelected("AIRASIA");

            if (config != null)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBoxAirAsia.Text = openFileDialog.FileName;
                }
            }
            else
                MessageBox.Show("No configuration available for Air Asia", "Error");
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void button1_Click(object sender, EventArgs e) //Close button
        {
            if (user.AccessRights != "ADM")
                Application.Exit();
            else
                Close();
        }

        private void button2_Click(object sender, EventArgs e) //Minimize button
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void btnIATA_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx";

            openFileDialog.FilterIndex = 1;

            var config = new AirlineConfigurationViewModel().GetSelected("IATA");

            if (config != null)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBoxIATA.Text = openFileDialog.FileName;
                }
            }
            else
                MessageBox.Show("No configuration available for IATA", "Error");
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword form = new ChangePassword(user);

            form.ShowDialog();
        }

        private void unbilledMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void unbilledMonitoringToolStripMenuItem1_Click(object sender, EventArgs e) //Unbilled Monitoring
        {
            UnbilledMonitoring form = new UnbilledMonitoring(user, "", "", "", "", "");

            form.Show();
        }

        private void billedMonitoringToolStripMenuItem_Click(object sender, EventArgs e) //Billed Monitoring
        {
            BilledMonitoring form = new BilledMonitoring(user, "", "", "", "", "");

            form.Show();
        }

        private void billedUnbilledExportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBilledAndUnbilled.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelBilledAndUnbilled.Visible = false;
        }

        private void btnOkay_Click(object sender, EventArgs e) //Unbilled & Billed Monitoring
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "File saving location";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                StartProgress("Exporting");

                List<Invoice> originalUnbilled = new List<Invoice>();
                List<Invoice> billed = new List<Invoice>();
                List<Invoice> unbilled = new List<Invoice>();
                List<Invoice> submitted = new List<Invoice>();

                IEnumerable<Invoice> finalList = new List<Invoice>();

                originalUnbilled = new TravcomViewModel().GetAllUnpostedviaSQL(dateTimeFrom.Value.Date, dateTimeTo.Value.Date);

                billed = new Billed().GetAllBilled(dateTimeFrom.Value.Date, dateTimeTo.Value.Date);

                //Remove all submitted from list
                unbilled = originalUnbilled.Where(r => !r.FreeFields.Contains("AEFUR-SUBMITTED")).ToList();

                //Get all subbmited
                submitted = originalUnbilled.Where(r => r.FreeFields.Contains("AEFUR-SUBMITTED")).ToList();

                var noRecords = new Unbilled().GetAllNoRecord();

                noRecords = new Unbilled().RemoveDuplicateInNoRecord(noRecords);

                //Remove all duplicate
                unbilled = unbilled.GroupBy(x => new { x.TicketNo }).Select(x => x.FirstOrDefault()).ToList();

                submitted = submitted.GroupBy(x => new { x.TicketNo }).Select(x => x.FirstOrDefault()).ToList();

                noRecords = noRecords.GroupBy(x => new { x.TicketNo }).Select(x => x.FirstOrDefault()).ToList();

                finalList = finalList.Concat(unbilled.OrderBy(r => r.TransactionDate));

                finalList = finalList.Concat(submitted.OrderBy(r => r.TransactionDate));

                finalList = finalList.Concat(billed.OrderBy(r => r.TransactionDate));

                finalList = finalList.Concat(new Unbilled().ConvertNoRecordToInvoice(noRecords));

                //Get Department

                using (var db = new AirlineBillingReportEntities())
                {
                    var agents = db.AgentProfile.Where(r => r.ID != null);

                    finalList.ToList().ForEach(item =>
                    {
                        var temp = agents.FirstOrDefault(r => r.TravCom1 == item.BookingAgent || r.TravCom2 == item.BookingAgent
                            || r.TravCom3 == item.BookingAgent || r.TravCom4 == item.BookingAgent || r.TravCom5 == item.BookingAgent);

                        if(temp != null)
                        {
                            item.Department = temp.Department;
                        }
                    });
                }

                string path = fbd.SelectedPath + "\\" + "Billed and Unbilled Report ";

                string tabName = "Billed and Unbilled";         

                Microsoft.Office.Interop.Excel.ExcelUtlity obj = new Microsoft.Office.Interop.Excel.ExcelUtlity();

                DataTable dt = ConvertToDataTable(finalList);

                path += dateTimeFrom.Value.ToShortDateString().Replace("/", ".");

                path += " to " + dateTimeTo.Value.ToShortDateString().Replace("/", ".") + ".xlsx";

                obj.WriteDataTableToExcel(dt, tabName, path, "Billed and Unbilled Summary");

                CloseProgress();

                MessageBox.Show("Excel created " + path);            
            }
        }

        public DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
