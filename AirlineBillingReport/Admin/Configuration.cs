using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirlineBillingReport.Setup;
using System.Runtime.InteropServices;

namespace AirlineBillingReport.Admin
{
    public partial class Configuration : Form
    {
        //For menu border style
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public Configuration()
        {
            InitializeComponent();
        }

        private void MenuMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void amadeusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            CebuPacificConfiguration cebuPac = new CebuPacificConfiguration();

            cebuPac.TopLevel = false;

            cebuPac.Visible = true;

            cebuPac.Dock = DockStyle.Fill;

            panelMain.Controls.Add(cebuPac);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void Configuration_Resize(object sender, EventArgs e)
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

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MenuMove(e);
        }

        private void iATAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            IATAConfiguration IATA = new IATAConfiguration();

            IATA.TopLevel = false;

            IATA.Visible = true;

            IATA.Dock = DockStyle.Fill;

            panelMain.Controls.Add(IATA);
        }

        private void iASAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            IASAConfiguration IASA = new IASAConfiguration();

            IASA.TopLevel = false;

            IASA.Visible = true;

            IASA.Dock = DockStyle.Fill;

            panelMain.Controls.Add(IASA);
        }

        private void philippineAirlinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            PhilippineAirlinesConfiguration PAL = new PhilippineAirlinesConfiguration();

            PAL.TopLevel = false;

            PAL.Visible = true;

            PAL.Dock = DockStyle.Fill;

            panelMain.Controls.Add(PAL);
        }

        private void airAsiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            AirAsiaConfiguration airAsia = new AirAsiaConfiguration();

            airAsia.TopLevel = false;

            airAsia.Visible = true;

            airAsia.Dock = DockStyle.Fill;

            panelMain.Controls.Add(airAsia);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void userAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            UserList user = new UserList(this);

            user.TopLevel = false;

            user.Visible = true;

            user.Dock = DockStyle.Fill;

            panelMain.Controls.Add(user);
        }
    }
}
