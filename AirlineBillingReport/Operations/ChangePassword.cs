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
using AirlineBillingReportRepository;
using AirlineBillingReportRepository.ViewModel;

namespace AirlineBillingReport.Operations
{
    public partial class ChangePassword : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        UserAccount user;

        public ChangePassword(UserAccount user)
        {
            InitializeComponent();

            this.user = user;
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            string errorMessage = "";

            if (txtBoxNewPassword.Text != txtBoxConfirmPassword.Text)
                errorMessage += "Password not match \n";

            if (user.Password != txtBoxCurrentPassword.Text)
                errorMessage += "Current password incorrect\n";

            if(errorMessage != "")
            {
                lblErrorMessage.Text = errorMessage;

                lblErrorMessage.Visible = true;
            }
            else
            {
                var userVM = new UserAccountViewModel();

                if(userVM.ChangePassword(user.ID, txtBoxConfirmPassword.Text))
                {
                    SuccessForm form = new SuccessForm("Successful", "Password changed");
                    form.ShowDialog();

                    Close();
                }
                else
                {
                    ErrorForm form = new ErrorForm("Error", errorMessage);

                    form.ShowDialog();
                }
            }
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {

        }
    }
}
