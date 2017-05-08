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
    public partial class IATAConfiguration : Form
    {
        public IATAConfiguration()
        {
            InitializeComponent();

            GetConfiguration();
        }

        private void GetConfiguration()
        {
            var IATAConfig = new AirlineConfigurationViewModel().GetSelected("IATA");

            if(IATAConfig != null)
            {
                txtBoxStartCol.Text = IATAConfig.StartColumn;

                txtBoxStartRow.Text = IATAConfig.StartRow.ToString();

                txtBoxAgentCode.Text = IATAConfig.AgentCodeCol;

                txtBoxTabName.Text = IATAConfig.TabName;

                txtBoxAgentFirstName.Text = IATAConfig.FirstNameCol;

                txtBoxAgentLastName.Text = IATAConfig.LastNameCol;

                txtBoxRecordLocator.Text = IATAConfig.RecordLocatorCol;

                txtBoxCreatedOrganizationCode.Text = IATAConfig.CreatedOrganizationCol;

                txtBoxSourceOrgCode.Text = IATAConfig.SourceOrganizationCodeCol;

                txtBoxPaymentCode.Text = IATAConfig.PaymentCodeCol;

                txtBoxPaymentID.Text = IATAConfig.PaymentIDCol;

                txtBoxAuthorizationStatus.Text = IATAConfig.AuthorizationStatusCol;

                txtBoxCurrencyCode.Text = IATAConfig.CurrencyCodeCol;

                txtBoxBookingAmount.Text = IATAConfig.BookingAmountCol;

                txtBoxCollectedCurrCode.Text = IATAConfig.CollectedCurrencyCodeCol;

                txtBoxCollectedAmount.Text = IATAConfig.CollectedAmountCol;

                txtBoxConvertedCurrCode.Text = IATAConfig.ConvertedCurrencyCodeCol;

                txtBoxConvertedAmount.Text = IATAConfig.ConvertedAmountCol;

                txtBoxPaymentText.Text = IATAConfig.PaymentText;

                txtBoxPAXFirstName.Text = IATAConfig.PassengerFirstName;

                txtBoxPAXLastName.Text = IATAConfig.PassengerLastName;

                txtBoxDeparture.Text = IATAConfig.RouteDeparture;

                txtBoxDestination.Text = IATAConfig.RouteDestination;

                txtBoxPaymentDate.Text = IATAConfig.PaymentDate;

                txtBoxTicketNo.Text = IATAConfig.TicketNo;

                txtBoxAirlineCode.Text = IATAConfig.AirlineCode;
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

                Airline = "IATA"
            };

            var IATAVM = new AirlineConfigurationViewModel();

            if (IATAVM.UpdateConfiguration(airlineConfig))
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
