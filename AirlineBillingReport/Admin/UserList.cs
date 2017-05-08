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

namespace AirlineBillingReport.Admin
{
    public partial class UserList : Form
    {
        Configuration mainWindow;
        public UserList(Configuration mainWindow)
        {
            InitializeComponent();

            GetUsers();

            this.mainWindow = mainWindow;
        }

        private void GetUsers(string searchKey = "")
        {
            listViewUser.Items.Clear();

            var users = new List<UserAccount>();

            if (searchKey == "")
                users = new UserAccountViewModel().GetAll();
            else
                users = new UserAccountViewModel().GetAll(searchKey);

            int ctr = 0;

            users.ForEach(item =>
            {
                ListViewItem lvi = new ListViewItem(item.Username);

                //For color only
                ctr++;

                if(ctr % 2 == 0)
                {
                    lvi.BackColor = Color.FromArgb(209, 238, 255);
                }

                lvi.SubItems.Add(item.AccessRights);

                lvi.SubItems.Add(item.FirstName + " " + item.LastName);

                lvi.SubItems.Add(item.Department);

                var agent = new AgentCodeViewModel().GetSelectedAgent(item.AgentID);

                if (agent != null)
                {
                    lvi.SubItems.Add(agent.CebuPacific);

                    lvi.SubItems.Add(agent.IATA);

                    lvi.SubItems.Add(agent.IASA);

                    lvi.SubItems.Add(agent.PAL);

                    lvi.SubItems.Add(agent.AirAsia);

                    lvi.SubItems.Add(agent.TravCom1);

                    lvi.SubItems.Add(agent.TravCom2);

                    lvi.SubItems.Add(agent.TravCom3);

                    lvi.SubItems.Add(agent.TravCom4);

                    lvi.SubItems.Add(agent.TravCom5);
                }

                listViewUser.Items.Add(lvi);
            });
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetUsers();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            mainWindow.panelMain.Controls.Clear();

            OpenUser form = new OpenUser("", mainWindow);

            form.TopLevel = false;

            form.Visible = true;

            form.Dock = DockStyle.Fill;

            mainWindow.panelMain.Controls.Add(form);
        }

        private void SelectUser()
        {
            if (listViewUser.SelectedItems.Count > 0)
            {
                string username = listViewUser.SelectedItems[0].SubItems[0].Text;

                mainWindow.panelMain.Controls.Clear();

                OpenUser form = new OpenUser(username, mainWindow);

                form.TopLevel = false;

                form.Visible = true;

                form.Dock = DockStyle.Fill;

                mainWindow.panelMain.Controls.Add(form);
            }
            else
            {
                MessageBox.Show("Please select data from list", "Error");
            }
        }

        private void listViewUser_DoubleClick(object sender, EventArgs e)
        {
            SelectUser();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            SelectUser();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetUsers(txtBoxSearchKey.Text);
        }
    }
}
