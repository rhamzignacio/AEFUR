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
    public partial class PhilippineAirlinesConfiguration : Form
    {
        public PhilippineAirlinesConfiguration()
        {
            InitializeComponent();

            GetConfiguration();
        }

        private void GetConfiguration()
        {
            var PALConfig = new AirlineConfigurationViewModel().GetSelected("CEBUPACIFIC");

            if (PALConfig != null)
            {
                txtBoxStartCol.Text = PALConfig.StartColumn;

                txtBoxStartRow.Text = PALConfig.StartRow.ToString();

                txtBoxAgentCode.Text = PALConfig.AgentCodeCol;

                txtBoxTabName.Text = PALConfig.TabName;

                txtBoxAgentFirstName.Text = PALConfig.FirstNameCol;

                txtBoxAgentLastName.Text = PALConfig.LastNameCol;

                txtBoxRecordLocator.Text = PALConfig.RecordLocatorCol;

                txtBoxCreatedOrganizationCode.Text = PALConfig.CreatedOrganizationCol;

                txtBoxSourceOrgCode.Text = PALConfig.SourceOrganizationCodeCol;

                txtBoxPaymentCode.Text = PALConfig.PaymentCodeCol;

                txtBoxPaymentID.Text = PALConfig.PaymentIDCol;

                txtBoxAuthorizationStatus.Text = PALConfig.AuthorizationStatusCol;

                txtBoxCurrencyCode.Text = PALConfig.CurrencyCodeCol;

                txtBoxBookingAmount.Text = PALConfig.BookingAmountCol;

                txtBoxCollectedCurrCode.Text = PALConfig.CollectedCurrencyCodeCol;

                txtBoxCollectedAmount.Text = PALConfig.CollectedAmountCol;

                txtBoxConvertedCurrCode.Text = PALConfig.ConvertedCurrencyCodeCol;

                txtBoxConvertedAmount.Text = PALConfig.ConvertedAmountCol;

                txtBoxPaymentText.Text = PALConfig.PaymentText;

                txtBoxPAXFirstName.Text = PALConfig.PassengerFirstName;

                txtBoxPAXLastName.Text = PALConfig.PassengerLastName;

                txtBoxDeparture.Text = PALConfig.RouteDeparture;

                txtBoxDestination.Text = PALConfig.RouteDestination;

                txtBoxPaymentDate.Text = PALConfig.PaymentDate;

                txtBoxTicketNo.Text = PALConfig.TicketNo;

                txtBoxAirlineCode.Text = PALConfig.AirlineCode;
            }
        }

        private void Save()
        {
            AirlineConfiguration airlineConfig = new AirlineConfiguration
            {
                StartRow = int.Parse(txtBoxStartRow.Text),

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

                Airline = "PAL"
            };

            var cebuVM = new AirlineConfigurationViewModel();

            if (cebuVM.UpdateConfiguration(airlineConfig))
            {
                MessageBox.Show("Successfully updated configuration", "Successfull");
            }
            else
                MessageBox.Show("Cannot update configuration there was some kind of error", "Error on saving");
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
