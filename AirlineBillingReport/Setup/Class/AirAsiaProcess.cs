using AirlineBillingReportRepository;
using AirlineBillingReportRepository.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirlineBillingReport.Class
{
    public class AirAsiaProcess
    {
        public void Process(string _recordNo, string _path, MainWindow _mainWindow, List<Invoice> posted, List<Invoice> voided
            , List<AirlineBillingReportRepository.AgentProfile> agent)
        {
            _mainWindow.StartProgress("Processing Air Asia . . .");
            //configuration for cebu pacifc
            var config = new AirlineConfigurationViewModel().GetSelected("AIRASIA");

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
                    bool startReading = false;

                    AirlineConfigurationViewModel airlineConfigVM = new AirlineConfigurationViewModel();

                    int dif = airlineConfigVM.ConvertColumnToInteger(config.StartColumn);

                    string dateRange = "";

                    int counter = 0;

                    BilledTicket billed;

                    UnbilledTicket unbilled;

                    NoRecord noRecord;

                    var travCom = new TravcomViewModel();

                    string recordLocator = "";

                    string ticketNo = "";

                    while (dr.Read())
                    {
                        agentName = "";

                        if (dr[0].ToString() == "PREPARED BY:")
                        {
                            _mainWindow.CloseProgress();

                            connection.Close();

                            break;
                        }

                        if (startReading)
                        {                           
                            counter++;

                            if (_mainWindow.loadingForm.lblProcessedCount.InvokeRequired)
                            {
                                _mainWindow.loadingForm.lblProcessedCount.Invoke(new MethodInvoker(delegate
                                { _mainWindow.loadingForm.lblProcessedCount.Text = counter.ToString(); }));
                            }
                            
                            if (dr[airlineConfigVM.ConvertColumnToInteger(config.RecordLocatorCol) - dif].ToString() != "")
                            {
                                //Record Locator
                                if (config.RecordLocatorCol != "")
                                    recordLocator = dr[airlineConfigVM.ConvertColumnToInteger(config.RecordLocatorCol) - dif].ToString().Replace(" ","");
                                else
                                    recordLocator = "N/A";

                                if (config.TicketNo != "")
                                    ticketNo = dr[airlineConfigVM.ConvertColumnToInteger(config.TicketNo) - dif].ToString();          

                                string invoiceNo = "";

                                var temp = posted.FirstOrDefault(r => r.TicketNo == ticketNo);

                                if (temp != null)
                                    invoiceNo = temp.InvoiceNo;

                                if (config.PaymentDate != "")
                                    dateRange = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentDate) - dif].ToString();

                                if (invoiceNo != "") //Already posted
                                {
                                    billed = new BilledTicket();

                                    if(ticketNo == "")
                                    {
                                        ticketNo = temp.TicketNo;
                                    }

                                    //Agent Code
                                    if (config.AgentCodeCol != "")
                                    {
                                        billed.AgentCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AgentCodeCol) - dif].ToString();

                                        var tempAgent = agent.FirstOrDefault(r => r.AirAsiaSafeName == billed.AgentCode.ToUpper());

                                        if (tempAgent != null)
                                        {
                                            billed.Department = tempAgent.Department;
                                        }
                                        else
                                            billed.Department = "";
                                    }
                                    else
                                    {
                                        billed.AgentCode = "";

                                        billed.Department = "";
                                    }

                                    //Agent Name
                                    if (config.FirstNameCol != "")
                                    {
                                        agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.FirstNameCol) - dif].ToString();
                                        
                                    }

                                    if (config.LastNameCol != "")
                                    {
                                        agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.LastNameCol) - dif].ToString();
                                    }

                                    //Department
                                    if (agentName != "")
                                    {
                                        var tempAgent = agent.FirstOrDefault(r => r.SafeName == agentName.ToUpper());

                                        if (tempAgent != null)
                                            billed.Department = tempAgent.Department;                                     
                                        else
                                            billed.Department = "N/A";
                                    }

                                    billed.DateRange = dateRange;

                                    billed.AgentName = agentName;

                                    //Record Locator
                                    billed.RecordLocator = recordLocator;

                                    //TicketNo
                                    billed.TicketNo = ticketNo;

                                    //AirlineCode
                                    if (config.AirlineCode != "")
                                        billed.AirlineCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AirlineCode) - dif].ToString();
                                    else
                                        billed.AirlineCode = "N/A";

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

                                    //Exchange Rate
                                    if (billed.CollectedAmount != "0.00")
                                        billed.ExchangeRate = (double.Parse(billed.ConvertedAmount) / double.Parse(billed.CollectedAmount)).ToString();
                                    else
                                        billed.ExchangeRate = "0.00";

                                    if (temp.InvoiceDate != null)
                                        billed.InvoiceDate = DateTime.Parse(temp.InvoiceDate.ToString()).ToShortDateString();

                                    //Airline
                                    billed.Airline = "AIR ASIA";

                                    billed.RecordNo = _recordNo;

                                    billed.InvoiceNo = invoiceNo;

                                    //Client Name
                                    billed.ClientName = temp.ClientName;

                                    var billedVM = new BilledTicketViewModel();

                                    if (billedVM.Save(billed))
                                    {
                                        //Successfully saved
                                    }
                                }
                                else //not yet posted
                                {
                                    temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo);

                                    if (temp == null)
                                    {
                                        noRecord = new NoRecord();

                                        if (config.AgentCodeCol != "")
                                            noRecord.AgentCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AgentCodeCol) - dif].ToString();
                                        else
                                            noRecord.AgentCode = "";


                                        //Agent Name
                                        if (config.FirstNameCol != "")
                                        {
                                            agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.FirstNameCol) - dif].ToString() + " ";
                                        }

                                        if (config.LastNameCol != "")
                                        {
                                            agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.LastNameCol) - dif].ToString();
                                        }

                                        noRecord.AgentName = agentName;

                                        noRecord.Airline = "Air Asia";

                                        noRecord.DateRange = dateRange;

                                        noRecord.RecordLocator = recordLocator;

                                        noRecord.TicketNo = ticketNo;

                                        noRecord.RecordNo = _recordNo;

                                        noRecord.CreatedDate = DateTime.Now;

                                        var noRecordVM = new NoRecordViewModel();

                                        if (noRecordVM.Save(noRecord))
                                        {
                                            //Successfully Save
                                        }
                                    }
                                    else
                                    {
                                        //Check if not voided
                                        temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo && r.TransactionType == 2);

                                        if (temp == null)
                                        {
                                            unbilled = new UnbilledTicket();

                                            temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo);

                                            if (ticketNo == "")
                                            {
                                                ticketNo = temp.TicketNo;
                                            }

                                            //Agent Code
                                            if (config.AgentCodeCol != "")
                                            {
                                                unbilled.AgentCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AgentCodeCol) - dif].ToString();

                                                var tempAgent = agent.FirstOrDefault(r => r.AirAsiaSafeName == unbilled.AgentCode.ToUpper());

                                                if (tempAgent != null)
                                                {
                                                    unbilled.Department = tempAgent.Department;
                                                }
                                                else
                                                    unbilled.Department = "";
                                            }
                                            else
                                            {
                                                unbilled.AgentCode = "";

                                                unbilled.Department = "";
                                            }

                                            //Agent Name
                                            if (config.FirstNameCol != "")
                                                agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.FirstNameCol) - dif].ToString() + " ";

                                            if (config.LastNameCol != "")
                                                agentName += dr[airlineConfigVM.ConvertColumnToInteger(config.LastNameCol) - dif].ToString();

                                            unbilled.DateRange = dateRange;

                                            unbilled.AgentName = agentName;
                                            //Record Locator
                                            unbilled.RecordLocator = recordLocator;

                                            //Ticket No
                                            unbilled.TicketNo = ticketNo;

                                            //Airline Code
                                            if (config.AirlineCode != "")
                                                unbilled.AirlineCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AirlineCode) - dif].ToString();
                                            else
                                                unbilled.AirlineCode = "N/A";

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

                                            //Department
                                            if (agentName != "")
                                            {
                                                var tempAgent = agent.FirstOrDefault(r => r.SafeName == agentName.ToUpper());

                                                if (tempAgent != null)
                                                    unbilled.Department = tempAgent.Department;
                                                else
                                                    unbilled.Department = "N/A";
                                            }

                                            //Exchange Rate
                                            if (unbilled.CollectedAmount != "0.00")
                                                unbilled.ExchangeRate = (double.Parse(unbilled.ConvertedAmount) / double.Parse(unbilled.CollectedAmount)).ToString();
                                            else
                                                unbilled.ExchangeRate = "0.00";

                                            //Client Name
                                            unbilled.ClientName = temp.ClientName;

                                            //Airline
                                            unbilled.Airline = "AIR ASIA";

                                            unbilled.RecordNo = _recordNo;

                                            var unbilledVM = new UnbilledTicketViewModel();

                                            if (unbilledVM.Save(unbilled))
                                            {
                                                //Successfully saved
                                            }
                                        }
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

                        if (dr[0].ToString() == "Transaction ID")
                        {
                            startReading = true;
                        }                        
                    }
                }
            }
        }
    }  
}
