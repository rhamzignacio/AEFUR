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
    public partial class IASAConfiguration : Form
    {
        public IASAConfiguration()
        {
            InitializeComponent();

            GetConfiguration();
        }

        private void GetConfiguration()
        {
            var IASAConfig = new AirlineConfigurationViewModel().GetSelected("IASA");

            if(IASAConfig != null)
            {
                txtBoxStartCol.Text = IASAConfig.StartColumn;

                txtBoxStartRow.Text = IASAConfig.StartRow.ToString();

                txtBoxAgentCode.Text = IASAConfig.AgentCodeCol;

                txtBoxTabName.Text = IASAConfig.TabName;

                txtBoxAgentFirstName.Text = IASAConfig.FirstNameCol;

                txtBoxAgentLastName.Text = IASAConfig.LastNameCol;

                txtBoxRecordLocator.Text = IASAConfig.RecordLocatorCol;

                txtBoxCreatedOrganizationCode.Text = IASAConfig.CreatedOrganizationCol;

                txtBoxSourceOrgCode.Text = IASAConfig.SourceOrganizationCodeCol;

                txtBoxPaymentCode.Text = IASAConfig.PaymentCodeCol;

                txtBoxPaymentID.Text = IASAConfig.PaymentIDCol;

                txtBoxAuthorizationStatus.Text = IASAConfig.AuthorizationStatusCol;

                txtBoxCurrencyCode.Text = IASAConfig.CurrencyCodeCol;

                txtBoxBookingAmount.Text = IASAConfig.BookingAmountCol;

                txtBoxCollectedCurrCode.Text = IASAConfig.CollectedCurrencyCodeCol;

                txtBoxCollectedAmount.Text = IASAConfig.CollectedAmountCol;

                txtBoxConvertedCurrCode.Text = IASAConfig.ConvertedCurrencyCodeCol;

                txtBoxConvertedAmount.Text = IASAConfig.ConvertedAmountCol;

                txtBoxPaymentText.Text = IASAConfig.PaymentText;

                txtBoxPAXFirstName.Text = IASAConfig.PassengerFirstName;

                txtBoxPAXLastName.Text = IASAConfig.PassengerLastName;

                txtBoxDeparture.Text = IASAConfig.RouteDeparture;

                txtBoxDestination.Text = IASAConfig.RouteDestination;

                txtBoxPaymentDate.Text = IASAConfig.PaymentDate;

                txtBoxTicketNo.Text = IASAConfig.TicketNo;

                txtBoxAirlineCode.Text = IASAConfig.AirlineCode;
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

                Airline = "IASA"
            };

            var IASAVM = new AirlineConfigurationViewModel();

            if (IASAVM.UpdateConfiguration(airlineConfig))
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
