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
using AirlineBillingReport.Class;
using System.Threading;
using System.Timers;
using AirlineBillingReportRepository;
using AirlineBillingReportRepository.ViewModel;
using NsExcel = Microsoft.Office.Interop.Excel;
namespace AirlineBillingReport.Operations
{
    public partial class BilledMonitoring : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        string agentCode1 = "",agentCode2 = "", agentCode3 = "", agentCode4 = "", agentCode5 = "", department = "";

        public UserAccount user;

        public List<Invoice> billed = new List<Invoice>();

        public List<Invoice> originalBilled = new List<Invoice>();

        public List<AgentCode> agentCodes = null;

        public BilledMonitoring(UserAccount user, string agentCode1, string agentCode2, string agentCode3, string agentCode4,
            string agentCode5)
        {
            InitializeComponent();

            this.agentCode1 = agentCode1;

            this.agentCode2 = agentCode2;

            this.agentCode3 = agentCode3;

            this.agentCode4 = agentCode4;

            this.agentCode5 = agentCode5;

            this.user = user;

            if (user.AccessRights == "BLM")
                department = "Business & Leisure";
            else if (user.AccessRights == "MM")
                department = "Marine";
            else if (user.AccessRights == "MCM")
                department = "Mice";

            if (agentCodes == null)
            {
                if (department == "")
                    agentCodes = new TravcomViewModel().GetAllAgent();
                else
                    agentCodes = new TravcomViewModel().GetAllAgentByDepartment(department);
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
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
            if (user.AccessRights == "ADM" || user.AccessRights == "AC" || user.AccessRights == "ACM")
                Close();
            else
                Application.Exit();
        }

        private void BilledMonitoring_Resize(object sender, EventArgs e)
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

        private void BilledMonitoring_Load(object sender, EventArgs e)
        {
            //GetAll();

            lblUsername.Text = user.FirstName + " " + user.LastName;
        }

        int counter = 0;

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetAll();
        }

        Loading loadingForm = new Loading("");

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Text = "Exporting - 0%";

            btnExport.Refresh();

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "File saving location";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string path = fbd.SelectedPath + "\\";

                string tabName = "";

                Microsoft.Office.Interop.Excel.ExcelUtlity obj = new Microsoft.Office.Interop.Excel.ExcelUtlity();

                DataTable dt = ConvertToDataTable(billed);

                if (user.AccessRights == "BL" || user.AccessRights == "M")
                {
                    path += lblUsername.Text + " ";

                    tabName = lblUsername.Text;
                }
                else
                {
                    path += user.Department + " ";

                    tabName = user.Department;
                }

                path += lblUpdatedDate.Text.Replace(":", ".").Replace("/", ".") + ".xlsx";

                obj.WriteDataTableToExcel(dt, tabName, path, "Unbilled Summary", null, this);

                MessageBox.Show("Excel created " + path + " \\" + lblUpdatedDate.Text.Replace(":", ".").Replace("/", ".") + ".xlsx");

                btnExport.Text = "Export to Excel";
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePassword form = new ChangePassword(user);

            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (user.AccessRights == "ADM" || user.AccessRights == "AC" || user.AccessRights == "ACM")
            {
                Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetAll(txtBoxSearchKey.Text);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            lblDateRange.Text = dateTimeFrom.Text + " To " + dateTimeTo.Text;

            GetAll("",dateTimeFrom.Value, dateTimeTo.Value);

            panelDateRange.Visible = false;
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            panelDateRange.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelDateRange.Visible = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        public void StartProgress(string message)
        {
            this.Hide();

            loadingForm = new Loading(message);

            ShowProgress();
        }

        public void CloseProgress()
        {
            Thread.Sleep(0);

            this.Show();

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
                    catch{}
                }
                else
                {
                    Thread th = new Thread(ShowProgress);

                    th.IsBackground = false;

                    th.Start();
                }
            }
            catch{}
        }

        public void GetAll(string searchKey = "", DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (searchKey == "")
                StartProgress("");
            else
                StartProgress("Searching");

            listViewBilled.Items.Clear();

            listViewBilledManager.Items.Clear();

            if(user.AccessRights == "ADM" || user.AccessRights == "ACM")
            {
                //IT Admin or Accounting manager
                originalBilled = new Billed().GetAllBilled(fromDate, toDate);

                billed = new Billed().GetBilledViaAgent(originalBilled, agentCodes);

                lblSearchKeyBLM.Visible = true;
            }

            originalBilled = originalBilled.Where(r => r.TicketNo != "").OrderBy(r => r.BookingDate).ToList();

            originalBilled = originalBilled.GroupBy(x => new { x.TicketNo, x.RecordLocator, x.PassengerName }).Select(x => x.FirstOrDefault()).ToList();

            billed = billed.Where(r => r.TicketNo != "").OrderBy(r => r.BookingDate).ToList();

            billed = billed.GroupBy(x => new { x.TicketNo, x.RecordLocator, x.PassengerName }).Select(x => x.FirstOrDefault()).ToList();

            if (fromDate != null && toDate != null)
                billed = billed.Where(r => r.BookingDate >= fromDate && r.BookingDate <= toDate).ToList();

            lblTotalUnbilled.Text = "Total Unbilled: " + billed.Count.ToString();

            if(searchKey != "")
            {
                if (user.AccessRights == "BL" || user.AccessRights == "M")
                    billed = billed.Where(r => r.BookingDate.ToString().Contains(searchKey) || r.RecordLocator.ToLower().Contains(searchKey.ToLower())
                        || r.TicketNo.Contains(searchKey)).ToList();
                else
                    billed = billed.Where(r => r.BookingDate.ToString().Contains(searchKey) || r.RecordLocator.ToLower().Contains(searchKey.ToLower())
                        || r.TicketNo.Contains(searchKey)).ToList();
            }

            int ctr = 0;

            //Business and Leisure, Marine, Mice
            if(user.AccessRights == "BL" || user.AccessRights == "M" || user.AccessRights == "MC")
            {
                billed.ForEach(item =>
                {
                    ListViewItem lvi = new ListViewItem(DateTime.Parse(item.BookingDate.ToString()).ToShortDateString());

                    ctr++;

                    if (ctr % 2 == 0)
                        lvi.BackColor = Color.FromArgb(209, 238, 255);

                    lvi.ForeColor = Color.Black;

                    lvi.SubItems.Add(item.AirlineCode);

                    lvi.SubItems.Add(item.RecordLocator);

                    lvi.SubItems.Add(item.TicketNo);

                    lvi.SubItems.Add(item.Itinerary);

                    lvi.SubItems.Add(item.ClientName);

                    lvi.SubItems.Add(item.Supplier);

                    lvi.SubItems.Add(item.Currency);

                    lvi.SubItems.Add(string.Format("{0:0.00}", item.GrossAmount));

                    listViewBilled.Items.Add(lvi);
                });

                listViewBilled.Visible = true;

                CloseProgress();
            }
            //IT, Marine Manager, Business and Leisure Manager, Mice Manager
            else if(user.AccessRights == "ADM" || user.AccessRights == "MM" || user.AccessRights =="BLM"
                || user.AccessRights == "MCM" || user.AccessRights == "ACM")
            {
                billed.ForEach(item =>
                {
                    ListViewItem lvi = new ListViewItem(DateTime.Parse(item.BookingDate.ToString()).ToShortDateString());

                    ctr++;

                    if (ctr % 2 == 0)
                        lvi.BackColor = Color.FromArgb(209, 238, 255);

                    lvi.ForeColor = Color.Black;

                    var agent = new AgentCodeViewModel().GetAll().FirstOrDefault(r => r.TravCom1 == item.BookingAgent ||
                        r.TravCom2 == item.BookingAgent || r.TravCom3 == item.BookingAgent || r.TravCom4 == item.BookingAgent
                        || r.TravCom5 == item.BookingAgent);

                    if (agent != null)
                        lvi.SubItems.Add(agent.Department);
                    else
                        lvi.SubItems.Add("");

                    lvi.SubItems.Add(item.BookingAgentName);

                    lvi.SubItems.Add(item.AirlineCode);

                    lvi.SubItems.Add(item.RecordLocator);

                    lvi.SubItems.Add(item.TicketNo);

                    lvi.SubItems.Add(item.Itinerary);

                    lvi.SubItems.Add(item.ClientName);

                    lvi.SubItems.Add(item.Supplier);

                    lvi.SubItems.Add(item.Currency);

                    lvi.SubItems.Add(string.Format("{0:0.00}", item.GrossAmount));

                    listViewBilledManager.Items.Add(lvi);
                });

                listViewBilledManager.Visible = true;

                CloseProgress();
            }

            lblUpdatedDate.Text = DateTime.Now.ToString();
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
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
