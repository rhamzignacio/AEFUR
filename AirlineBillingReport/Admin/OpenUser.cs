using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirlineBillingReportRepository;
using AirlineBillingReportRepository.ViewModel;

namespace AirlineBillingReport.Admin
{
    public partial class OpenUser : Form
    {
        string type = "";

        string userFirst = "", userLast = "";

        Configuration mainWindow;

        Guid userID = Guid.Empty;

        Guid? agentID = Guid.Empty;

        public OpenUser(string username, Configuration mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            if (username == "")
            {
                type = "NEW";

                btnSave.Text = "Create";
            }
            else
            {
                type = "EDIT";

                btnSave.Text = "Update";

                GetUserInfo(username);
            }
        }

        private void GetUserInfo(string username)
        {
            var user = new UserAccountViewModel().GetSelectedUser(username);

            if (user != null)
            {
                userID = user.ID;

                agentID = user.AgentID;

                txtBoxUserName.Text = user.Username;

                txtBoxFirstName.Text = user.FirstName;

                txtBoxMiddleName.Text = user.MiddleName;

                txtBoxLastName.Text = user.LastName;

                cmbBoxDepartment.Text = user.Department;

                if (user.AccessRights == "BLM" || user.AccessRights == "MCM" || user.AccessRights == "MM")
                    checkBoxDepartmentHead.Checked = true;
                else
                    checkBoxDepartmentHead.Checked = false;

                if(agentID != null)
                {
                    var agentProfile = new AgentCodeViewModel().GetSelectedAgent(agentID);

                    if(agentProfile != null)
                    {
                        txtBox5J.Text = agentProfile.CebuPacific;

                        txtBoxPartnerAgent.Text = agentProfile.PartnerAgent;

                        txtBoxAirAsia.Text = agentProfile.AirAsia;

                        txtBoxIASA.Text = agentProfile.IASA;

                        txtBoxIATA.Text = agentProfile.IATA;

                        txtBoxPAL.Text = agentProfile.PAL;

                        txtBoxTravcom1.Text = agentProfile.TravCom1;

                        txtBoxTravcom2.Text = agentProfile.TravCom2;

                        txtBoxTravcom3.Text = agentProfile.TravCom3;

                        txtBoxTravcom4.Text = agentProfile.TravCom4;

                        txtBoxTravcom5.Text = agentProfile.TravCom5;
                    }
                }

                if (cmbBoxDepartment.Text == "Information Technology" || cmbBoxDepartment.Text == "Accounting")
                    groupBoxAgentCodes.Visible = checkBoxDepartmentHead.Visible = false;
                else
                    groupBoxAgentCodes.Visible = checkBoxDepartmentHead.Visible = true;
            }
        }

        private void txtBoxLastName_TextChanged(object sender, EventArgs e)
        {
            if (txtBoxLastName.Text != "")
            {
                userLast = txtBoxLastName.Text.ToLower();

                txtBoxUserName.Text = userFirst + userLast.Replace(" ","");
            }
        }

        private void ClearAgentCodes()
        {
            txtBox5J.Text = txtBoxAirAsia.Text = txtBoxIASA.Text = txtBoxIATA.Text =
                txtBoxPAL.Text = txtBoxTravcom1.Text = txtBoxTravcom2.Text =
                txtBoxTravcom3.Text = txtBoxTravcom4.Text = txtBoxTravcom5.Text = "";

            checkBoxDepartmentHead.Checked = false;
        }

        private void cmbBoxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAgentCodes();

            if (cmbBoxDepartment.Text == "Information Technology")
                groupBoxAgentCodes.Visible = checkBoxDepartmentHead.Visible = false;
            else if (cmbBoxDepartment.Text == "Accounting")
                checkBoxDepartmentHead.Visible = true;
            else
                groupBoxAgentCodes.Visible = checkBoxDepartmentHead.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mainWindow.panelMain.Controls.Clear();

            UserList form = new UserList(mainWindow);

            form.TopLevel = false;

            form.Visible = true;

            form.Dock = DockStyle.Fill;

            mainWindow.panelMain.Controls.Add(form);

            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string errorMessage = "";

            if (txtBoxFirstName.Text == "")
                errorMessage += "First name is required\n";

            if (txtBoxLastName.Text == "")
                errorMessage += "Last name is required\n";

            if (cmbBoxDepartment.Text == "")
                errorMessage += "Department is required\n";

            //Check if Sign in code already exist
            var userVM = new UserAccountViewModel();
            //5J
            UserAccount temp = userVM.CheckIfExist(txtBox5J.Text, userID);

            if (temp != null)
                errorMessage += "Sign in code for Cebu pacific already used by " + temp.FirstName + " " + temp.LastName;

            //Partner Agent
            temp = userVM.CheckIfExist(txtBoxPartnerAgent.Text, userID);

            if (temp != null)
                errorMessage += "Sign in code for Partner Agent already used by " + temp.FirstName + " " + temp.LastName;

            //Client Agent
            temp = userVM.CheckIfExist(txtBoxClientAgent.Text, userID);
            if (temp != null)
                errorMessage += "Sign in code for Client Agent already used by " + temp.FirstName + " " + temp.LastName;
            
            //PAL
            temp = userVM.CheckIfExist(txtBoxPAL.Text, userID);

            if (temp != null)
                errorMessage += "Sign in code for PAL already used by " + temp.FirstName + " " + temp.LastName;

            //IATA
            temp = userVM.CheckIfExist(txtBoxIATA.Text, userID);

            if (temp != null)
                errorMessage += "Sign in code for IATA already used by " + temp.FirstName + " " + temp.LastName;

            //IASA
            temp = userVM.CheckIfExist(txtBoxIASA.Text, userID);

            if (temp != null)
                errorMessage += "Sign in code for IASA already used by " + temp.FirstName + " " + temp.LastName;

            //AIR ASIA
            temp = userVM.CheckIfExist(txtBoxAirAsia.Text, userID);

            if (temp != null)
                errorMessage += "Sign in code for Air Asia already used by " + temp.FirstName + " " + temp.LastName;

            if (errorMessage != "")
                MessageBox.Show(errorMessage, "Error");
            else
            {
                UserAccount user = new UserAccount();

                user.FirstName = txtBoxFirstName.Text;

                user.MiddleName = txtBoxMiddleName.Text;

                user.LastName = txtBoxLastName.Text;

                user.Department = cmbBoxDepartment.Text;

                user.Username = txtBoxUserName.Text.ToLower();

                user.Password = txtBoxPassword.Text;

                user.ID = userID;

                string accessRights = "";

                if (cmbBoxDepartment.Text == "Information Technology")
                    accessRights = "ADM";
                else if (cmbBoxDepartment.Text == "Accounting")
                    accessRights = "AC";
                else if (cmbBoxDepartment.Text == "Business & Leisure")
                    accessRights = "BL";
                else if (cmbBoxDepartment.Text == "Marine")
                    accessRights = "M";
                else if (cmbBoxDepartment.Text == "Mice")
                    accessRights = "MC";

                if (checkBoxDepartmentHead.Checked)
                    accessRights += "M";

                user.AccessRights = accessRights;

                AirlineBillingReportRepository.AgentProfile agent = null;


                if(groupBoxAgentCodes.Visible == true)
                {
                    agent = new AirlineBillingReportRepository.AgentProfile();

                    agent.Department = cmbBoxDepartment.Text;

                    agent.Name = txtBoxFirstName.Text + " " + txtBoxLastName.Text;

                    agent.CebuPacific = txtBox5J.Text;

                    agent.ClientPartnerAgent = txtBoxClientAgent.Text;

                    agent.PartnerAgent = txtBoxPartnerAgent.Text;

                    agent.IASA = txtBoxIASA.Text;

                    agent.IATA = txtBoxIATA.Text;

                    agent.PAL = txtBoxPAL.Text;

                    agent.AirAsia = txtBoxAirAsia.Text;

                    agent.TravCom1 = txtBoxTravcom1.Text;

                    agent.TravCom2 = txtBoxTravcom2.Text;

                    agent.TravCom3 = txtBoxTravcom3.Text;

                    agent.TravCom4 = txtBoxTravcom4.Text;

                    agent.TravCom5 = txtBoxTravcom5.Text;

                    agent.Parameter = txtBoxParameter.Text;
                }

                if(agentID != Guid.Empty)
                {
                    agent = new AgentCodeViewModel().GetSelectedAgent(agentID);

                    agent.Department = cmbBoxDepartment.Text;

                    agent.Name = txtBoxFirstName.Text + " " + txtBoxLastName.Text;

                    agent.CebuPacific = txtBox5J.Text;

                    agent.PartnerAgent = txtBoxPartnerAgent.Text;

                    agent.ClientPartnerAgent = txtBoxClientAgent.Text;

                    agent.IASA = txtBoxIASA.Text;

                    agent.IATA = txtBoxIATA.Text;

                    agent.PAL = txtBoxPAL.Text;

                    agent.AirAsia = txtBoxAirAsia.Text;

                    agent.TravCom1 = txtBoxTravcom1.Text;

                    agent.TravCom2 = txtBoxTravcom2.Text;

                    agent.TravCom3 = txtBoxTravcom3.Text;

                    agent.TravCom4 = txtBoxTravcom4.Text;

                    agent.TravCom5 = txtBoxTravcom5.Text;

                    agent.Parameter = txtBoxParameter.Text;
                }

                    
                if (type == "NEW")//Create User
                {
                    string createResult = new UserAccountViewModel().Save(user, agent);

                    if (createResult == "Y")
                    {
                        MessageBox.Show("Successfully saved");

                        btnSave.Enabled = false; //To prevent from duplicate saving
                    }
                    else
                    {
                        MessageBox.Show(createResult, "Error");
                    }
                }
                else //Update User
                {
                    string updateResult = new UserAccountViewModel().Edit(user, agent);

                    if (updateResult == "Y")
                    {
                        MessageBox.Show("Successfully updated");

                        btnSave.Enabled = false; //To prevent from duplicate saving
                    }
                    else
                    {
                        MessageBox.Show(updateResult, "Error");
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtBoxFirstName_TextChanged(object sender, EventArgs e)
        {
            if (txtBoxFirstName.Text != "")
            {
                userFirst = txtBoxFirstName.Text.ToLower().Substring(0, 1);

                txtBoxUserName.Text = userFirst + userLast;
            }
        }
    }
}
