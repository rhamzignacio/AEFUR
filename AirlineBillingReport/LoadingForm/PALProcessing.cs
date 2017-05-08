using AirlineBillingReportRepository;
using AirlineBillingReportRepository.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirlineBillingReport.LoadingForm
{
    public partial class PALProcessing : Form
    {
        string _recordNo;
        string _path;
        MainWindow _mainWindow;

        public PALProcessing(string _recordNo, string _path, MainWindow _mainWindow)
        {
            InitializeComponent();

            this._recordNo = _recordNo;

            this._path = _path;

            this._mainWindow = _mainWindow;      
        }

        public void Process(string _recordNo, string _path, MainWindow _mainWindow)
        {
            //configuration for cebu pacifc
            var config = new AirlineConfigurationViewModel().GetSelected("PAL");

            //Excel connection string
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";" +
                 "Extended Properties='Excel 12.0 Xml;HDR=Yes;'";

            string agentName = "";

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();

                OleDbCommand command = new OleDbCommand("select * from [" + config.TabName + "$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    bool ifStartReading = false;

                    string dateRange = "";

                    AirlineConfigurationViewModel airlineConfigVM = new AirlineConfigurationViewModel();

                    int dif = airlineConfigVM.ConvertColumnToInteger(config.StartColumn);

                    int counter = 0;

                    while (dr.Read())
                    {
                        agentName = "";

                        string ticketNo = "";

                        if (ifStartReading)
                        {
                            if (dr[airlineConfigVM.ConvertColumnToInteger(config.RecordLocatorCol) - dif].ToString() != "")
                            {
                                counter++;

                                lblProcessedCount.Text = counter.ToString();

                                ticketNo = dr[airlineConfigVM.ConvertColumnToInteger(config.RecordLocatorCol) - dif].ToString();

                                var travCom = new TravcomViewModel();

                                string invoiceNo = travCom.CheckIfPosted(ticketNo);

                                if (config.PaymentDate != "")
                                    dateRange = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentDate) - dif].ToString();

                                if (invoiceNo != "") //Already posted
                                {
                                    BilledTicket billed = new BilledTicket();

                                    //Agent Code

                                    if (config.AgentCodeCol != "")
                                        billed.AgentCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AgentCodeCol) - dif].ToString();
                                    else
                                        billed.AgentCode = "";

                                    //Agent Name
                                    if (config.FirstNameCol != "")
                                    {
                                        agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.FirstNameCol) - dif].ToString();
                                    }

                                    if (config.LastNameCol != "")
                                    {
                                        agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.LastNameCol) - dif].ToString();
                                    }

                                    billed.DateRange = dateRange;

                                    billed.AgentName = agentName;

                                    //Record Locator
                                    billed.RecordLocator = ticketNo;

                                    //Created Organization Code
                                    if (config.CreatedOrganizationCol != "")
                                        billed.CreatedOrganizationCode = dr[airlineConfigVM.ConvertColumnToInteger(config.CreatedOrganizationCol) - dif].ToString();
                                    else
                                        billed.CreatedOrganizationCode = "";

                                    //Source Organization Code
                                    if (config.SourceOrganizationCodeCol != "")
                                        billed.SourceOrganizationCode = dr[airlineConfigVM.ConvertColumnToInteger(config.SourceOrganizationCodeCol) - dif].ToString();
                                    else
                                        billed.SourceOrganizationCode = "";

                                    //Payment Code
                                    if (config.PaymentCodeCol != "")
                                        billed.PaymentCode = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentCodeCol) - dif].ToString();
                                    else
                                        billed.PaymentCode = "";

                                    //Payment ID
                                    if (config.PaymentIDCol != "")
                                        billed.PaymentID = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentIDCol) - dif].ToString();
                                    else
                                        billed.PaymentID = "";

                                    //Authorization Status
                                    if (config.AuthorizationStatusCol != "")
                                        billed.AuthorizationStatus = dr[airlineConfigVM.ConvertColumnToInteger(config.AuthorizationStatusCol) - dif].ToString();
                                    else
                                        billed.AuthorizationStatus = "";

                                    //Currency Code
                                    if (config.CurrencyCodeCol != "")
                                        billed.CurrencyCode = dr[airlineConfigVM.ConvertColumnToInteger(config.CurrencyCodeCol) - dif].ToString();
                                    else
                                        billed.CurrencyCode = "";

                                    //Booking Amount
                                    if (config.BookingAmountCol != "")
                                        billed.BookingAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.BookingAmountCol) - dif].ToString();
                                    else
                                        billed.BookingAmount = "0.00";

                                    //Collected Currency Code
                                    if (config.CollectedCurrencyCodeCol != "")
                                        billed.CollectedCurrencyCode = dr[airlineConfigVM.ConvertColumnToInteger(config.CollectedCurrencyCodeCol) - dif].ToString();
                                    else
                                        billed.CollectedCurrencyCode = "";

                                    //Collected Amount
                                    if (config.CollectedAmountCol != "")
                                        billed.CollectedAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.CollectedAmountCol) - dif].ToString();
                                    else
                                        billed.CollectedAmount = "0.00";

                                    //Converted Currency Code
                                    if (config.ConvertedCurrencyCodeCol != "")
                                        billed.ConvertedCurrencyCode = dr[airlineConfigVM.ConvertColumnToInteger(config.ConvertedCurrencyCodeCol) - dif].ToString();
                                    else
                                        billed.CollectedCurrencyCode = "";

                                    //Converted Amount
                                    if (config.ConvertedAmountCol != "")
                                        billed.ConvertedAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.ConvertedAmountCol) - dif].ToString();
                                    else
                                        billed.ConvertedAmount = "0.00";

                                    //Payment Text
                                    if (config.PaymentText != "")
                                        billed.PaymentText = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentText) - dif].ToString();
                                    else
                                        billed.PaymentText = "";

                                    //Passenger Name
                                    if (config.PassengerFirstName != "")
                                        billed.PassengerName = dr[airlineConfigVM.ConvertColumnToInteger(config.PassengerFirstName) - dif].ToString();
                                    else
                                        billed.PassengerName = "";

                                    if (config.PassengerLastName != "")
                                        billed.PassengerName = billed.PassengerName + " " +
                                           dr[airlineConfigVM.ConvertColumnToInteger(config.PassengerLastName) - dif].ToString();

                                    //Departure City
                                    if (config.RouteDeparture != "")
                                        billed.DepartureCity = dr[airlineConfigVM.ConvertColumnToInteger(config.RouteDeparture) - dif].ToString();
                                    else
                                        billed.DepartureCity = "";

                                    //Destination City
                                    if (config.RouteDestination != "")
                                        billed.DestinationCity = dr[airlineConfigVM.ConvertColumnToInteger(config.RouteDestination) - dif].ToString();
                                    else
                                        billed.DestinationCity = "";

                                    //Airline
                                    billed.Airline = "IASA";

                                    billed.RecordNo = _recordNo;

                                    billed.InvoiceNo = invoiceNo;

                                    var billedVM = new BilledTicketViewModel();

                                    if (billedVM.Save(billed))
                                    {
                                        //Successfully saved
                                    }
                                }
                                else //not yet posted
                                {
                                    UnbilledTicket unbilled = new UnbilledTicket();

                                    //Agent Code
                                    if (config.AgentCodeCol != "")
                                        unbilled.AgentCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AgentCodeCol) - dif].ToString();
                                    else
                                        unbilled.AgentCode = "";

                                    //Agent Name
                                    if (config.FirstNameCol != "")
                                    {
                                        agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.FirstNameCol) - dif].ToString() + " ";
                                    }

                                    if (config.LastNameCol != "")
                                    {
                                        agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.LastNameCol) - dif].ToString();
                                    }

                                    unbilled.DateRange = dateRange;

                                    unbilled.AgentName = agentName;
                                    //Record Locator
                                    unbilled.RecordLocator = ticketNo;

                                    //Created Organization Code
                                    if (config.CreatedOrganizationCol != "")
                                        unbilled.CreatedOrganizationCode = dr[airlineConfigVM.ConvertColumnToInteger(config.CreatedOrganizationCol) - dif].ToString();
                                    else
                                        unbilled.CreatedOrganizationCode = "";

                                    //Source Organization Code
                                    if (config.SourceOrganizationCodeCol != "")
                                        unbilled.SourceOrganizationCode = dr[airlineConfigVM.ConvertColumnToInteger(config.SourceOrganizationCodeCol) - dif].ToString();
                                    else
                                        unbilled.SourceOrganizationCode = "";

                                    //Payment Code
                                    if (config.PaymentCodeCol != "")
                                        unbilled.PaymentCode = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentCodeCol) - dif].ToString();
                                    else
                                        unbilled.PaymentCode = "";

                                    //Payment ID
                                    if (config.PaymentIDCol != "")
                                        unbilled.PaymentID = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentIDCol) - dif].ToString();
                                    else
                                        unbilled.PaymentID = "";

                                    //Authorization Status
                                    if (config.AuthorizationStatusCol != "")
                                        unbilled.AuthorizationStatus = dr[airlineConfigVM.ConvertColumnToInteger(config.AuthorizationStatusCol) - dif].ToString();
                                    else
                                        unbilled.AuthorizationStatus = "";

                                    //Currency Code
                                    if (config.CurrencyCodeCol != "")
                                        unbilled.CurrencyCode = dr[airlineConfigVM.ConvertColumnToInteger(config.CurrencyCodeCol) - dif].ToString();
                                    else
                                        unbilled.CurrencyCode = "";

                                    //Booking Amount
                                    if (config.BookingAmountCol != "")
                                        unbilled.BookingAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.BookingAmountCol) - dif].ToString();
                                    else
                                        unbilled.BookingAmount = "0.00";

                                    //Collected Currency Code
                                    if (config.CollectedCurrencyCodeCol != "")
                                        unbilled.CollectedCurrencyCode = dr[airlineConfigVM.ConvertColumnToInteger(config.CollectedCurrencyCodeCol) - dif].ToString();
                                    else
                                        unbilled.CollectedCurrencyCode = "";

                                    //Collected Amount
                                    if (config.CollectedAmountCol != "")
                                        unbilled.CollectedAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.CollectedAmountCol) - dif].ToString();

                                    else
                                        unbilled.CollectedAmount = "0.00";

                                    //Converted Currency Code
                                    if (config.ConvertedCurrencyCodeCol != "")
                                        unbilled.ConvertedCurrencyCode = dr[airlineConfigVM.ConvertColumnToInteger(config.ConvertedCurrencyCodeCol) - dif].ToString();
                                    else
                                        unbilled.ConvertedCurrencyCode = "";

                                    //Converted Amount
                                    if (config.ConvertedAmountCol != "")
                                        unbilled.ConvertedAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.ConvertedAmountCol) - dif].ToString();
                                    else
                                        unbilled.ConvertedAmount = "0.00";

                                    //Collected Amount
                                    if (config.CollectedAmountCol != "")
                                        unbilled.CollectedAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.CollectedAmountCol) - dif].ToString();
                                    else
                                        unbilled.CollectedAmount = "0.00";

                                    //Payment Text
                                    if (config.PaymentText != "")
                                        unbilled.PaymentText = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentText) - dif].ToString();
                                    else
                                        unbilled.PaymentText = "";

                                    //Passenger Name
                                    if (config.PassengerFirstName != "")
                                        unbilled.PassengerName = dr[airlineConfigVM.ConvertColumnToInteger(config.PassengerFirstName) - dif].ToString();
                                    else
                                        unbilled.PassengerName = "";

                                    if (config.PassengerLastName != "")
                                        unbilled.PassengerName = unbilled.PassengerName + " " +
                                        dr[airlineConfigVM.ConvertColumnToInteger(config.PassengerLastName) - dif].ToString();

                                    //Departure City
                                    if (config.RouteDeparture != "")
                                        unbilled.DepartureCity = dr[airlineConfigVM.ConvertColumnToInteger(config.RouteDeparture) - dif].ToString();
                                    else
                                        unbilled.DepartureCity = "";

                                    //Destination City
                                    if (config.RouteDestination != "")
                                        unbilled.DestinationCity = dr[airlineConfigVM.ConvertColumnToInteger(config.RouteDestination) - dif].ToString();
                                    else
                                        unbilled.DestinationCity = "";

                                    //Airline
                                    unbilled.Airline = "Philippine Airlines";

                                    unbilled.RecordNo = _recordNo;

                                    var unbilledVM = new UnbilledTicketViewModel();

                                    if (unbilledVM.Save(unbilled))
                                    {
                                        //Successfully saved
                                    }
                                }
                            }
                            else// no record locator
                            {
                                _mainWindow.CloseProgress();

                                connection.Close();

                                break;
                            }
                        }

                        if (dr[0].ToString() == "Date of Issue")
                        {
                            ifStartReading = true;
                        }
                    }
                }
            }
        }

        private void PALProcessing_Load(object sender, EventArgs e)
        {
            Process(_recordNo, _path, _mainWindow);
        }
    }
}
