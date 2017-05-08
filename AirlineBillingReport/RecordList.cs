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
using System.Runtime.InteropServices;

namespace AirlineBillingReport
{
    public partial class RecordList : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public RecordList(Guid _userID, string _accessRights)
        {
            InitializeComponent();

            GetRecordList(_userID, _accessRights);
        }

        public void GetRecordList(Guid _userID, string _accessRights)
        {
            listView1.Items.Clear();

            List<RecordNoStorage> records = new List<RecordNoStorage>();

            if (_accessRights == "AC")
                records = new RecordNoStorageViewModel().GetRecordList(_userID);
            else if (_accessRights == "BLM" || _accessRights == "ADM" || 
                _accessRights == "ACM")
                records = new RecordNoStorageViewModel().GetAll();

            records.ForEach(item =>
            {
                ListViewItem lvi = new ListViewItem(item.RecordNo);

                lvi.SubItems.Add(item.Date.ToString());

                var user = new UserAccountViewModel().GetUser(item.UserID);

                if (user != null)
                    lvi.SubItems.Add(user.FirstName + " " + user.LastName);
                else
                    lvi.SubItems.Add("");

                if (item.C5J != null)
                    lvi.SubItems.Add("X");
                else
                    lvi.SubItems.Add("");

                if (item.PAL != null)
                    lvi.SubItems.Add("X");
                else
                    lvi.SubItems.Add("");

                if (item.IATA != null)
                    lvi.SubItems.Add("X");
                else
                    lvi.SubItems.Add("");

                if (item.IASA != null)
                    lvi.SubItems.Add("X");
                else
                    lvi.SubItems.Add("");

                if (item.AIRASIA != null)
                    lvi.SubItems.Add("X");
                else
                    lvi.SubItems.Add("");


                listView1.Items.Add(lvi);
            });
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string recordNo = listView1.SelectedItems[0].SubItems[0].Text;

            FormSelection form = new FormSelection(recordNo);

            form.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
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
    }
}
