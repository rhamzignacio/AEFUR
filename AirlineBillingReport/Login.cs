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
using AirlineBillingReport.Operations;
using AirlineBillingReport.Admin;
using AirlineBillingReportRepository;
using System.Net;
using System.Net.Mail;

namespace AirlineBillingReport
{
    public partial class Login : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public Login()
        {
            InitializeComponent();
        }

        public void SignIn()
        {
            if (txtBoxUsername.Text == "")
                ErrorMessage(true, "Password is required");
            else
            {
                var message = new UserAccountViewModel().TryLogin(txtBoxUsername.Text, txtBoxPassword.Text);

                if (message == "Y") //successful login
                {
                    ErrorMessage(false, "");                    

                    var user = new UserAccountViewModel().GetSelectedUser(txtBoxUsername.Text);

                    LoginLogs loginLogs = new LoginLogs();

                    loginLogs.UserAccountID = user.ID;

                    var loginLogsVM = new LoginLogsViewModel();

                    if (loginLogsVM.LoginLog(loginLogs))
                    { } //Successfully saved

                    if (user.AccessRights == "AC" || user.AccessRights == "ACM")
                    {
                       
                            MainWindow form = new MainWindow(user);

                            form.Show();

                            Hide();
                        
                    }
                    else if(user.AccessRights == "ADM")
                    {
                        AdminMenu form = new AdminMenu(user);

                        form.Show();

                        Hide();
                    }
                    else //BL
                    {
                        if (user.AccessRights == "BL" || user.AccessRights == "M" || user.AccessRights == "MC")
                        {
                            var agentProfile = new AgentCodeViewModel().GetSelectedAgent(user.AgentID);

                            if (agentProfile != null)
                            {                               

                                UnbilledMonitoring form = new UnbilledMonitoring(user, agentProfile.TravCom1, agentProfile.TravCom2
                                , agentProfile.TravCom3, agentProfile.TravCom4,
                                agentProfile.TravCom5);

                                Hide();

                                form.Show();
                            }
                        }
                        else if (user.AccessRights == "BLM" || user.AccessRights == "MM" || user.AccessRights == "MCM")
                        {
                            var agentProfile = new AgentCodeViewModel().GetSelectedAgent(user.AgentID);

                            if (agentProfile != null)
                            {
                                UnbilledMonitoring form = new UnbilledMonitoring(user, agentProfile.TravCom1, agentProfile.TravCom2
                                    , agentProfile.TravCom3, agentProfile.TravCom4,
                                    agentProfile.TravCom5);

                                Hide();

                                form.Show();
                            }
                        }
                    }
                                   
                }
                else if (message == "N") //Invalid username password
                {
                    ErrorMessage(true, "Invalid username or password");

                    txtBoxPassword.Text = "";                   
                }
                else //System Error
                {
                    ErrorMessage(true, message);
                }
            }
        }

        private void ErrorMessage(bool show, string message)
        {
            lblErrorMessage.Text = message;

            lblErrorMessage.Visible = show;
        }

        private void txtBoxUsername_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (txtBoxUsername.Text != "")
                    SignIn();
        }

        private void txtBoxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if(txtBoxUsername.Text != "")
                    SignIn();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
                SignIn();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void txtBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblSystemVersion_Click(object sender, EventArgs e)
        {
            string patchNotes = "";

            patchNotes += "version 3.8";
            patchNotes += "\n-Fixes in excel extraction";

            patchNotes += "\n\nversion 3.7";
            patchNotes += "\n-Date and Time of AEFUR submitted is now recorded";

            patchNotes += "\n\nversion 3.6";
            patchNotes += "\n-No record for IASA fixes";

            patchNotes += "\n\nversion 3.5";
            patchNotes += "\nCebu Pacific Excel uploading";
            patchNotes += "\n- Auto adjust data reading based on column name";
            patchNotes += "\n- Added AP Analysis report";
            patchNotes += "\n- Improved data capture accuracy";

            patchNotes += "\n\nversion 3.4.5";
            patchNotes += "\n- Added Credit Memo in IATA AP Analysis";

            patchNotes += "\n\nversion 3.4.4";
            patchNotes += "\n- IATA AP Analysis missing ADMA amount fixes";

            patchNotes += "\n\nversion 3.4.3";
            patchNotes += "\n- PAL AP Analysis Post Column value fixes";

            patchNotes += "\n\nversion 3.4.2";
            patchNotes += "\n- Missing voided record in PAL AP Analysis fixes";

           MessageBox.Show(patchNotes, "Release Notes version " + lblSystemVersion.Text);
        }
    }
}
