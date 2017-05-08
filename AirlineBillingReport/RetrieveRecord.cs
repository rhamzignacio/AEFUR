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
    public partial class RetrieveRecord : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public RetrieveRecord()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtBoxRecordNo.Text != "")
            {
                var recVM = new RecordNoStorageViewModel();

                if (recVM.IfExist(txtBoxRecordNo.Text))
                {
                    BilledReport form = new BilledReport(txtBoxRecordNo.Text);

                    form.Show();

                    UnbilledReport form2 = new UnbilledReport(txtBoxRecordNo.Text);

                    form2.Show();

                    Close();
                }
                else
                    MessageBox.Show("No record found", "Error");
            }
            else
                MessageBox.Show("Record no is required", "Warning");
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
