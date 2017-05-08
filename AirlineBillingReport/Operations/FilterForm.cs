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
    public partial class FilterForm : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        UnbilledMonitoring parentForm;

        List<AgentCode> originalAgentCode;

        List<AgentCode> tempCode = new List<AgentCode>();

        public FilterForm(UnbilledMonitoring parentForm)
        {
            InitializeComponent();

            this.parentForm = parentForm;

            if (parentForm.user.AccessRights == "ADM" || parentForm.user.AccessRights == "MM" ||
                parentForm.user.AccessRights == "MCM" || parentForm.user.AccessRights == "BLM" || 
                parentForm.user.AccessRights == "ACM")
            {
                originalAgentCode = parentForm.agentCodes;

                originalAgentCode.ForEach(item =>
                {
                    AgentCode temp = new AgentCode
                    {
                        ID = item.ID,
                        Tick = item.Tick,
                        Department = item.Department,
                        Name = item.Name,
                        TravCom1 = item.TravCom1,
                        TravCom2 = item.TravCom2,
                        TravCom3 = item.TravCom3,
                        TravCom4 = item.TravCom4,
                        TravCom5 = item.TravCom5
                    };

                    tempCode.Add(temp);
                });

                tempCode = tempCode.OrderBy(r => r.Department).ToList();


                LoadList();
            }
            else
            {
                lblTotalCount.Visible = lblTotalUnbilled.Visible = false;
            }
        }

        private void LoadList()
        {
            listViewFilter.Items.Clear();

            tempCode.ForEach(item =>
            {
                ListViewItem lvi = new ListViewItem(item.ID.ToString());

                lvi.SubItems.Add(item.Name);

                lvi.SubItems.Add(item.Department);

                int temp = parentForm.unbilled.Where(r => r.BookingAgent == item.TravCom1 ||
                r.BookingAgent == item.TravCom2 || r.BookingAgent == item.TravCom3 || r.BookingAgent == item.TravCom4
                || r.BookingAgent == item.TravCom5).ToList().Count;

                lvi.SubItems.Add(temp.ToString());

                //totalCount += temp;

                if (item.Tick)
                    lvi.BackColor = Color.PaleGreen;
                else
                    lvi.BackColor = Color.Salmon;

                listViewFilter.Items.Add(lvi);
            });

            lblTotalCount.Text = parentForm.originalUnbilled.Count.ToString();

            parentForm.lblTotalUnbilled.Text = "Total Unbilled: " + parentForm.unbilled.Count.ToString();
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

        private void listViewFilter_DoubleClick(object sender, EventArgs e)
        {
            Guid ID = Guid.Parse(listViewFilter.SelectedItems[0].SubItems[0].Text);

            var temp = tempCode.FirstOrDefault(r => r.ID == ID);

            if (temp.Tick)
                temp.Tick = false;
            else
                temp.Tick = true;

            LoadList();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            parentForm.agentCodes = tempCode;

            Close();

            parentForm.GetAll(true, true,true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {

        }

        private void FilterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //parentForm.GetAll();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            parentForm.agentCodes = tempCode;

            parentForm.GetAll(true,true,true,"", parentForm.dateTimeFrom.Value.Date, parentForm.dateTimeTo.Value.Date);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {

        }

    }
}
