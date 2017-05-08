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

namespace AirlineBillingReport.Setup
{
    public partial class CebuPacificConfiguration : Form
    {
        public CebuPacificConfiguration()
        {
            InitializeComponent();

            GetConfiguration();
        }

        private void Save()
        {
            AirlineConfiguration airlineConfig = new AirlineConfiguration
            {
                StartRow = int.Parse(txtBoxStartRow.Text),

                ConvertedAmountCol = txtBoxConvertedAmount.Text,

                AgentCodeCol = txtBoxAgentCode.Text,

                StartColumn = txtBoxStartCol.Text,

                FirstNameCol = txtBoxAgentFirstName.Text,

                LastNameCol = txtBoxAgentLastName.Text,

                RecordLocatorCol = txtBoxRecordLocator.Text,

                CreatedOrganizationCol = txtBoxCreatedOrganizationCode.Text,

                SourceOrganizationCodeCol = txtBoxSourceOrgCode.Text,

                PaymentCodeCol = txtBoxPaymentCode.Text,

                PaymentIDCol = txtBoxPaymentID.Text,

                AuthorizationStatusCol = txtBoxAuthorizationStatus.Text,

                CurrencyCodeCol = txtBoxCurrencyCode.Text,

                BookingAmountCol = txtBoxBookingAmount.Text,

                CollectedCurrencyCodeCol = txtBoxCollectedCurrCode.Text,

                CollectedAmountCol = txtBoxCollectedAmount.Text,

                ConvertedCurrencyCodeCol = txtBoxConvertedCurrCode.Text,

                PaymentText = txtBoxPaymentText.Text,

                PassengerFirstName = txtBoxPAXFirstName.Text,

                PassengerLastName = txtBoxPAXLastName.Text,

                RouteDeparture = txtBoxDeparture.Text,

                RouteDestination = txtBoxDestination.Text,

                PaymentDate = txtBoxPaymentDate.Text,

                TabName = txtBoxTabName.Text,

                AirlineCode = txtBoxAirlineCode.Text,

                TicketNo = txtBoxTicketNo.Text,

                Airline = "CEBUPACIFIC"
            };

            var cebuVM = new AirlineConfigurationViewModel();

            if (cebuVM.UpdateConfiguration(airlineConfig))
            {
                MessageBox.Show("Successfully updated configuration","Successfull");
            }
            else
                MessageBox.Show("Cannot update configuration there was some kind of error", "Error on saving");
        }

        private void GetConfiguration()
        {
            var cebuPacConfig = new AirlineConfigurationViewModel().GetSelected("CEBUPACIFIC");

            if(cebuPacConfig != null)
            {
                txtBoxStartCol.Text = cebuPacConfig.StartColumn;

                txtBoxStartRow.Text = cebuPacConfig.StartRow.ToString();

                txtBoxAgentCode.Text = cebuPacConfig.AgentCodeCol;

                txtBoxTabName.Text = cebuPacConfig.TabName;

                txtBoxAgentFirstName.Text = cebuPacConfig.FirstNameCol;

                txtBoxAgentLastName.Text = cebuPacConfig.LastNameCol;

                txtBoxRecordLocator.Text = cebuPacConfig.RecordLocatorCol;

                txtBoxCreatedOrganizationCode.Text = cebuPacConfig.CreatedOrganizationCol;

                txtBoxSourceOrgCode.Text = cebuPacConfig.SourceOrganizationCodeCol;

                txtBoxPaymentCode.Text = cebuPacConfig.PaymentCodeCol;

                txtBoxPaymentID.Text = cebuPacConfig.PaymentIDCol;

                txtBoxAuthorizationStatus.Text = cebuPacConfig.AuthorizationStatusCol;

                txtBoxCurrencyCode.Text = cebuPacConfig.CurrencyCodeCol;

                txtBoxBookingAmount.Text = cebuPacConfig.BookingAmountCol;

                txtBoxCollectedCurrCode.Text = cebuPacConfig.CollectedCurrencyCodeCol;

                txtBoxCollectedAmount.Text = cebuPacConfig.CollectedAmountCol;

                txtBoxConvertedCurrCode.Text = cebuPacConfig.ConvertedCurrencyCodeCol;

                txtBoxConvertedAmount.Text = cebuPacConfig.ConvertedAmountCol;

                txtBoxPaymentText.Text = cebuPacConfig.PaymentText;

                txtBoxPAXFirstName.Text = cebuPacConfig.PassengerFirstName;

                txtBoxPAXLastName.Text = cebuPacConfig.PassengerLastName;

                txtBoxDeparture.Text = cebuPacConfig.RouteDeparture;

                txtBoxDestination.Text = cebuPacConfig.RouteDestination;

                txtBoxPaymentDate.Text = cebuPacConfig.PaymentDate;

                txtBoxTicketNo.Text = cebuPacConfig.TicketNo;

                txtBoxAirlineCode.Text = cebuPacConfig.AirlineCode;
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errorMessage = "";

            if (txtBoxStartRow.Text == "")
                errorMessage += "Start Row is required\n";

            if (txtBoxStartCol.Text == "")
                errorMessage += "Start Col is required\n";

            if (txtBoxRecordLocator.Text == "")
                errorMessage += "Record locator is required\n";

            if (txtBoxAgentFirstName.Text == "")
                errorMessage += "Agent First name is required\n";

            if (txtBoxAgentLastName.Text == "")
                errorMessage += "Agent Last name is required\n";

            if (errorMessage == "")
                Save();
            else
                MessageBox.Show(errorMessage, "Warning");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
