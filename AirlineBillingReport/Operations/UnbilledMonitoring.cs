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
using AirlineBillingReport.Setup.Class;

namespace AirlineBillingReport.Operations
{
    public partial class UnbilledMonitoring : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        string agentCode1 = "";
        string agentCode2 = "";
        string agentCode3 = "";
        string agentCode4 = "";
        string agentCode5 = "";

        public UserAccount user;

        public List<Invoice> unbilled = new List<Invoice>();

        public List<Invoice> originalUnbilled = new List<Invoice>();

        public List<AgentCode> agentCodes = null;

        string department = "";

        public UnbilledMonitoring(UserAccount user, string agentCode1, string agentCode2, string agentCode3, string agentCode4,
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

            if(user.AccessRights == "BLM" || user.AccessRights == "MM" || user.AccessRights == "MCM") //Department Head
            {
                checkBoxAll.Visible = true;
            }
            else if(user.AccessRights == "ADM" || user.AccessRights == "ACM") // IT or Accounting
            {
                checkBoxAll.Visible =
                btnSubmit.Visible = 
                checkBoxAll.Visible = false;
            }
   

            if (agentCodes == null)
            {
                if (department == "")
                    agentCodes = new TravcomViewModel().GetAllAgent();
                else
                    agentCodes = new TravcomViewModel().GetAllAgentByDepartment(department);
            }           
        }

        Loading loadingForm = new Loading("");

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
                        loadingForm.timer1.Start();
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

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        public void GetAll(bool showUnbilled, bool showSubmitted, bool showNoRecord, string searchkey = "", DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (searchkey == "")
                StartProgress("");
            else
                StartProgress("Searching");

            listViewUnbilled.Items.Clear();

            listViewUnbilledManager.Items.Clear();

            List<AEFURNoRecord> noRecords = new List<AEFURNoRecord>();

           //List<Invoice> unpostedRecords = new TravcomViewModel().GetAllUnpostedViaSQL(dateTimeFrom.Value.Date, dateTimeTo.Value.Date);

            List<Invoice> postedRecords = new TravcomViewModel().GetAllPosted();

            unbilled = new List<Invoice>();

            if (user.AccessRights == "BL" || user.AccessRights == "M" || user.AccessRights == "MC")
            {
                //Travel Consultant

                var agent = new AgentCodeViewModel().GetSelectedAgent(user.AgentID);

                //unbilled = new Unbilled().GetAllUnbilledViaAgentCode(showUnbilled, showSubmitted ,unpostedRecords, postedRecords,agentCode1, 
                //    agentCode2, agentCode3, agentCode4, agentCode5, fromDate, toDate);

                unbilled = new TravcomViewModel().GetAllUnpostedViaSQLPerTC(agentCode1, agentCode2, agentCode3, agentCode4, agentCode5);

                if (showNoRecord)
                {
                    noRecords = new Unbilled().GetAllNoRecordViaSQLPerTC(agent.CebuPacific, agent.PAL, agent.IATA, agent.IASA, agent.AirAsia);
                }

                lblSearchKeyBL.Visible = true;

                checkBoxAll.Visible = false;
            }
            else if (user.AccessRights == "ADM" || user.AccessRights == "ACM")
            {
                //IT Admin or Acocunting manager

                //originalUnbilled = new Unbilled().GetAllUnbilled(checkBoxUnbilled.Checked, checkBoxSubmitted.Checked, fromDate, toDate); 

                //unbilled = new Unbilled().GetUnbilledViaAgent(originalUnbilled, agentCodes);

                if (showNoRecord)
                {
                    noRecords = new Unbilled().GetAllNoRecordViaSQL();
                }

                //unbilled = unpostedRecords;

                lblSearchKeyBLM.Visible = true;

                checkBoxAll.Visible = false;
            }
            else if(user.AccessRights == "BLM" || user.AccessRights == "MM" || user.AccessRights =="MCM")
            {
                if (checkBoxAll.Checked)
                {
                    if (checkBoxUnbilled.Checked || checkBoxSubmitted.Checked)
                    {
                        //originalUnbilled = new Unbilled().GetAllUnbilledByDepartment(checkBoxUnbilled.Checked, checkBoxSubmitted.Checked, department, fromDate, toDate);

                        //unbilled = new Unbilled().GetUnbilledViaAgent(originalUnbilled, agentCodes);

                        unbilled = new TravcomViewModel().GetAllUnpostedViaSQLPerDepartment(department, fromDate.Value.Date, toDate.Value.Date);

                    }
                }
                else
                {
                    unbilled = new TravcomViewModel().GetAllUnpostedViaSQLPerTC(agentCode1, agentCode2, agentCode3, agentCode4, agentCode5);
                }

                if (checkBoxNoRecord.Checked)
                {
                    noRecords = new Unbilled().GetAllNoRecordViaSQLPerDepartment(department);
                }

                lblSearchKeyBLM.Visible = checkBoxAll.Visible = true;
            }

            //originalUnbilled = originalUnbilled.Where(r => r.TicketNo != "").OrderBy(r => r.InvoiceDate).ToList();

            //originalUnbilled = originalUnbilled.GroupBy(x => new { x.TicketNo, x.RecordLocator}).Select(x => x.FirstOrDefault()).ToList();

            if (!checkBoxSubmitted.Checked)
            {
                unbilled = unbilled.Where(r => !r.FreeFields.Contains("AEFUR")).ToList();
            }
            if (!checkBoxUnbilled.Checked)
            {
                unbilled = unbilled.Where(r => r.FreeFields.Contains("AEFUR")).ToList();
            }

            unbilled = unbilled.Where(r => r.TicketNo != "").OrderBy(r => r.InvoiceDate).ToList();

            unbilled = unbilled.GroupBy(x => new { x.TicketNo}).Select(x => x.FirstOrDefault()).ToList();

            unbilled = unbilled.OrderBy(r => r.TransactionDate).ThenBy(r=>r.RecordLocator).ThenBy(r=>r.TicketNo).ToList();

            lblTotalUnbilled.Text = "Total Count: " + (unbilled.Count + noRecords.Count).ToString();
            
            if (searchkey != "")
            {
                if (user.AccessRights == "BL" || user.AccessRights == "M")
                {
                        unbilled = unbilled.Where(r => r.InvoiceDate.ToString().Contains(searchkey) || r.RecordLocator.ToLower().Contains(searchkey.ToLower())
                            || r.TicketNo.Contains(searchkey)).ToList();
                }
                else
                {
                        unbilled = unbilled.Where(r => r.InvoiceDate.ToString().Contains(searchkey) || r.RecordLocator.ToLower().Contains(searchkey.ToLower())
                            || r.TicketNo.Contains(searchkey) || r.BookingAgentName.ToLower().Contains(txtBoxSearchKey.Text.ToLower())).ToList();
                }
            }

            int ctr = 0;

            //Business and Leisure, Marine, Mice
            if (user.AccessRights == "BL" || user.AccessRights == "M" || user.AccessRights == "MC")
            {
                unbilled.ForEach(item =>
                {
                    ListViewItem lvi = new ListViewItem(DateTime.Parse(item.TransactionDate.ToString()).ToShortDateString());

                    ctr++;

                    if (ctr % 2 == 0)
                        lvi.BackColor = Color.FromArgb(224, 224, 224);

                    if (item.FreeFields.Contains("AEFUR") && item.FreeFields.Contains("SUBMITTED"))
                        lvi.ForeColor = Color.Green;

                    lvi.SubItems.Add(item.AirlineCode);

                    lvi.SubItems.Add(item.RecordLocator);

                    lvi.SubItems.Add(item.TicketNo);

                    lvi.SubItems.Add(item.Itinerary);

                    lvi.SubItems.Add(item.PassengerName);

                    lvi.SubItems.Add(item.ClientName);

                    lvi.SubItems.Add(item.Supplier);

                    lvi.SubItems.Add(item.Currency);

                    lvi.SubItems.Add(string.Format("{0:0.00}", item.GrossAmount));

                    listViewUnbilled.Items.Add(lvi);
                });

                noRecords.ForEach(item =>
                {
                    ListViewItem lvi = new ListViewItem(item.DateRange);

                    ctr++;

                    if (ctr % 2 == 0)
                        lvi.BackColor = Color.FromArgb(224, 224, 224);

                    lvi.ForeColor = Color.FromArgb(0,94,186);

                    lvi.SubItems.Add("5J");

                    lvi.SubItems.Add(item.RecordLocator);

                    lvi.SubItems.Add(item.TicketNo);

                    lvi.SubItems.Add(""); //Itinerary

                    lvi.SubItems.Add(""); //PassengerName

                    lvi.SubItems.Add(""); //ClientName

                    lvi.SubItems.Add(item.Airline);

                    lvi.SubItems.Add("");

                    lvi.SubItems.Add("");

                    listViewUnbilled.Items.Add(lvi);
                });

                listViewUnbilled.Visible = true;

                CloseProgress();
            }
            //IT, Marine Manager, Business and Leisure Manager, Mice Manager
            else if (user.AccessRights == "ADM" || user.AccessRights == "MM"
                || user.AccessRights == "BLM" || user.AccessRights == "MCM" || user.AccessRights == "ACM")
            {
                    unbilled.ForEach(item =>
                    {
                        ListViewItem lvi = new ListViewItem(DateTime.Parse(item.TransactionDate.ToString()).ToShortDateString());

                        ctr++;

                        if (ctr % 2 == 0)
                            lvi.BackColor = Color.FromArgb(224, 224, 224);


                        if (item.FreeFields.Contains("AEFUR-SUBMITTED"))
                            lvi.ForeColor = Color.Green;

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

                        lvi.SubItems.Add(item.PassengerName);

                        lvi.SubItems.Add(item.ClientName);

                        lvi.SubItems.Add(item.Supplier);

                        lvi.SubItems.Add(item.Currency);

                        lvi.SubItems.Add(string.Format("{0:0.00}", item.GrossAmount));

                        listViewUnbilledManager.Items.Add(lvi);
                    });

                noRecords.ForEach(item =>
                {
                    ListViewItem lvi = new ListViewItem(item.DateRange);

                    ctr++;

                    if (ctr % 2 == 0)
                        lvi.BackColor = Color.FromArgb(224, 224, 224);

                    lvi.ForeColor = Color.FromArgb(0, 94, 186);

                    var agent = new AgentCodeViewModel().GetAll().FirstOrDefault(r => r.AirAsia == item.AgentCode ||
                        r.CebuPacific == item.AgentCode || r.IASA == item.AgentCode || r.IATA == item.AgentCode || r.PAL == item.AgentCode
                        || r.PartnerAgent == item.AgentCode);

                    if (agent != null)
                    {
                        lvi.SubItems.Add(agent.Department);

                        lvi.SubItems.Add(agent.Name);
                    }
                    else
                    {
                        lvi.SubItems.Add("");

                        lvi.SubItems.Add(item.AgentName);
                    }

                    lvi.SubItems.Add(item.Airline); //Airline Code

                    lvi.SubItems.Add(item.RecordLocator);

                    lvi.SubItems.Add(item.TicketNo);

                    lvi.SubItems.Add(""); //Itinerary

                    lvi.SubItems.Add(""); //Passenger Name

                    lvi.SubItems.Add(""); //Client Name

                    lvi.SubItems.Add(item.Airline); //Supplier

                    lvi.SubItems.Add(""); // Currency

                    lvi.SubItems.Add(item.BookingAmount); // Gross Amount

                    listViewUnbilledManager.Items.Add(lvi);
                });

                listViewUnbilledManager.Visible = true;

                CloseProgress();
            }

            lblUpdatedDate.Text = DateTime.Now.ToString();            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (user.AccessRights == "ADM" || user.AccessRights == "AC" || user.AccessRights == "ACM")
                Close();
            else
                Application.Exit();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetAll(checkBoxUnbilled.Checked, checkBoxSubmitted.Checked, checkBoxNoRecord.Checked, txtBoxSearchKey.Text);
        }

        System.Timers.Timer timer = new System.Timers.Timer();

        private void UnbilledMonitoring_Load(object sender, EventArgs e)
        {
            //GetAll();

            if(user.AccessRights == "BL" || user.AccessRights == "MC" || user.AccessRights == "M")
            {
                btnFilter.Enabled = checkBoxAll.Visible = btnDate.Enabled = lblDateRange .Visible = label4 .Visible = false;

                panelDateRange.Hide();

                GetAll(checkBoxUnbilled.Checked, checkBoxSubmitted.Checked, checkBoxNoRecord.Checked);

            }

            dateTimeFrom.Value = dateTimeTo.Value = DateTime.Now;

            lblDateRange.Text = dateTimeFrom.Text + " To " + dateTimeTo.Text;

            lblUsername.Text = user.FirstName + " " + user.LastName;
        }

        DateTime time = DateTime.Now;

        int counter = 0;

      

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetAll(checkBoxUnbilled.Checked, checkBoxSubmitted.Checked, checkBoxNoRecord.Checked, "", dateTimeFrom.Value.Date, dateTimeTo.Value.Date);
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

        private void UnbilledMonitoring_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_duplicate;
            }
            else if(WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Normal;

                btnMaximized.BackgroundImage = AirlineBillingReport.Properties.Resources.rsz_tick_blank;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            int ctr = 0;
            foreach (T item in data)
            {
                ctr++;

                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;    
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "File saving location";

            btnExport.Text = "Exporting - 0%";

            btnExport.Refresh();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string path = fbd.SelectedPath + "\\";

                string tabName = "";

                Microsoft.Office.Interop.Excel.ExcelUtlity obj = new Microsoft.Office.Interop.Excel.ExcelUtlity();

                DataTable dt = ConvertToDataTable(unbilled);

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

                path += lblUpdatedDate.Text.Replace(":", ".").Replace("/",".") + ".xlsx";

                obj.WriteDataTableToExcel(dt, tabName , path, "Unbilled Summary", this);

                MessageBox.Show("Excel created " + path + " \\" + lblUpdatedDate.Text.Replace(":", ".").Replace("/", ".") + ".xlsx");

                btnExport.Text = "Export to Excel";
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePassword form = new ChangePassword(user);

            form.ShowDialog();
        }

      
        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            GetAll(checkBoxUnbilled.Checked, checkBoxSubmitted.Checked, checkBoxNoRecord.Checked);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            FilterForm form = new FilterForm(this);

            form.ShowDialog();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }       

        private void btnQuestion_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This question module is not yet available", "Note");
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            panelDateRange.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelDateRange.Visible = false;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            lblDateRange.Text = dateTimeFrom.Text + " To " + dateTimeTo.Text;

            GetAll(checkBoxUnbilled.Checked, checkBoxSubmitted.Checked, checkBoxNoRecord.Checked,
                "", dateTimeFrom.Value.Date, dateTimeTo.Value.Date);

            panelDateRange.Visible = false;
        }

        //For Already Submitted BI Function
        private void listViewUnbilled_DoubleClick(object sender, EventArgs e)
        {
            if (user.AccessRights == "BL" || user.AccessRights == "M" || user.AccessRights == "MC")
            {
                string ticketNo = listViewUnbilled.SelectedItems[0].SubItems[3].Text;

                string message = "Are you sure that you've already submitted" +
                    "\nBilling Instruction to Accounting? " +
                    "\nTicket No: " + ticketNo + " [Y/N]";

                DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {

                    BillingInstruction billing = new BillingInstruction();

                    if (billing.FreeFieldTick(ticketNo))
                    {
                        MessageBox.Show("Ticket No: " + ticketNo + " is now Submitted");

                        listViewUnbilled.SelectedItems[0].ForeColor = Color.Green;
                    }
                }
            }      
        }

        private void listViewUnbilledManager_DoubleClick(object sender, EventArgs e)
        {
            if (user.AccessRights == "BLM" || user.AccessRights == "MM" || user.AccessRights == "MCM")
            {
                string ticketNo = listViewUnbilledManager.SelectedItems[0].SubItems[5].Text;

                string message = "Are you sure that you've already submitted" +
                    "\nBilling Instruction to Accounting? " +
                    "\nTicket No: " + ticketNo + " [Y/N]";

                DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    BillingInstruction billing = new BillingInstruction();

                    if (billing.FreeFieldTick(ticketNo))
                    {
                        MessageBox.Show("Ticket No: " + ticketNo + " is now Submitted");

                        listViewUnbilledManager.SelectedItems[0].ForeColor = Color.Green;
                    }
                    else
                    {
                        MessageBox.Show("There was some kind of error please send ticket no: " + ticketNo + "\n"
                            + "To IT Department for verification", "Error");
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> ticketNumbers = new List<string>();

            string ticketMessage = "";

            if (user.AccessRights == "BL" || user.AccessRights == "M" || user.AccessRights == "MC")
            {
                foreach (ListViewItem item in listViewUnbilled.SelectedItems)
                {
                    ticketNumbers.Add(item.SubItems[3].Text);

                    ticketMessage += item.SubItems[3].Text + ", ";
                }
            }
            else if(user.AccessRights == "BLM" || user.AccessRights == "MM" || user.AccessRights == "MCM")
            {
                foreach (ListViewItem item in listViewUnbilledManager.SelectedItems)
                {
                    ticketNumbers.Add(item.SubItems[5].Text);

                    ticketMessage += item.SubItems[5].Text + ", ";
                }
            }

            if (ticketNumbers.Count > 0)
            {
                string message = "Are you sure that you've already submitted" +
                    "\nBilling Instruction to Accounting? [Y/N] " +
                    "\nTicket No: \n" + ticketMessage;

                DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string successful = "\n";

                    ticketNumbers.ForEach(item =>
                    {
                        BillingInstruction billing = new BillingInstruction();

                        if (billing.FreeFieldTick(item))
                        {
                            successful += item + "\n";
                           
                        }
                    });

                    foreach (ListViewItem item in listViewUnbilled.SelectedItems)
                    {
                        item.ForeColor = Color.Green;
                    }

                    MessageBox.Show("Ticket No: " + successful + " is now Submitted");
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBoxUnbilled_CheckedChanged(object sender, EventArgs e)
        {
            if(!checkBoxUnbilled.Checked)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to remove Unbilled from the list? [Y/N]", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    checkBoxUnbilled.Checked = false;
                else
                    checkBoxUnbilled.Checked = true;
            }
        }

        private void checkBoxNoRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxNoRecord.Checked)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to remove No Record from the list? [Y/N]", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    checkBoxNoRecord.Checked = false;
                else
                    checkBoxNoRecord.Checked = true;
            }
        }
    }
}
