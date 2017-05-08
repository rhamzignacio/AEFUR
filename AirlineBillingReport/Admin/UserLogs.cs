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

namespace AirlineBillingReport.Admin
{
    public partial class UserLogs : Form
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

        public UserLogs()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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

        private void UserLogs_Resize(object sender, EventArgs e)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            listViewUser.Items.Clear();

            int counter = 0;

            var logs = new LoginLogsViewModel().GetLogs(dateTimeFrom.Value.Date, dateTimeTo.Value.Date);

            logs.ForEach(item =>
            {
                ListViewItem lvi = new ListViewItem(item.Username);

                if (counter % 2 == 0)
                    lvi.BackColor = Color.FromArgb(171, 202, 245);

                counter++;

                lvi.SubItems.Add(item.Name);

                lvi.SubItems.Add(item.Date);

                lvi.SubItems.Add(item.Time);

                listViewUser.Items.Add(lvi);                
            });
        }

        private void lblHeader_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }
    }
}
