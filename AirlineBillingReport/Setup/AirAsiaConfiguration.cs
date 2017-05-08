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
    public partial class AirAsiaConfiguration : Form
    {
        public AirAsiaConfiguration()
        {
            InitializeComponent();

            GetConfiguration();
        }

        private void GetConfiguration()
        {
            var airAisaConfig = new AirlineConfigurationViewModel().GetSelected("CEBUPACIFIC");

            if (airAisaConfig != null)
            {
                txtBoxStartCol.Text = airAisaConfig.StartColumn;

                txtBoxStartRow.Text = airAisaConfig.StartRow.ToString();

                txtBoxAgentCode.Text = airAisaConfig.AgentCodeCol;

                txtBoxTabName.Text = airAisaConfig.TabName;

                txtBoxAgentFirstName.Text = airAisaConfig.FirstNameCol;

                txtBoxAgentLastName.Text = airAisaConfig.LastNameCol;

                txtBoxRecordLocator.Text = airAisaConfig.RecordLocatorCol;

                txtBoxCreatedOrganizationCode.Text = airAisaConfig.CreatedOrganizationCol;

                txtBoxSourceOrgCode.Text = airAisaConfig.SourceOrganizationCodeCol;

                txtBoxPaymentCode.Text = airAisaConfig.PaymentCodeCol;

                txtBoxPaymentID.Text = airAisaConfig.PaymentIDCol;

                txtBoxAuthorizationStatus.Text = airAisaConfig.AuthorizationStatusCol;

                txtBoxCurrencyCode.Text = airAisaConfig.CurrencyCodeCol;

                txtBoxBookingAmount.Text = airAisaConfig.BookingAmountCol;

                txtBoxCollectedCurrCode.Text = airAisaConfig.CollectedCurrencyCodeCol;

                txtBoxCollectedAmount.Text = airAisaConfig.CollectedAmountCol;

                txtBoxConvertedCurrCode.Text = airAisaConfig.ConvertedCurrencyCodeCol;

                txtBoxConvertedAmount.Text = airAisaConfig.ConvertedAmountCol;

                txtBoxPaymentText.Text = airAisaConfig.PaymentText;

                txtBoxPAXFirstName.Text = airAisaConfig.PassengerFirstName;

                txtBoxPAXLastName.Text = airAisaConfig.PassengerLastName;

                txtBoxDeparture.Text = airAisaConfig.RouteDeparture;

                txtBoxDestination.Text = airAisaConfig.RouteDestination;

                txtBoxPaymentDate.Text = airAisaConfig.PaymentDate;

                txtBoxTicketNo.Text = airAisaConfig.TicketNo;

                txtBoxAirlineCode.Text = airAisaConfig.AirlineCode;
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

                Airline = "AIRASIA"
            };

            var airAsiaVM = new AirlineConfigurationViewModel();

            if (airAsiaVM.UpdateConfiguration(airlineConfig))
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
