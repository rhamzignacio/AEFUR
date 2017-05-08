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
    public class CebuPacificProcess
    {
        public void Process(string _recordNo, string _path, MainWindow _mainWindow, List<Invoice> posted, List<Invoice> voided
            , List<AirlineBillingReportRepository.AgentProfile> agent)
        {
            _mainWindow.StartProgress("Processing Cebu Pacific . . .");
            //configuration for cebu pacifc
            var config = new AirlineConfigurationViewModel().GetSelected("CEBUPACIFIC");

            //Excel connection string
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";" +
                 "Extended Properties='Excel 12.0 Xml;HDR=No;'";

            string agentName = "";

            int counter = 0;

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();

                OleDbCommand command = new OleDbCommand("select * from ["+config.TabName+"$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    bool ifStartReading = false, startReadColumn = false;

                    string dateRange = "", organizationCode = "", currencyCode = "", locationCode = "", FOP = "", deparment = ""; 

                    AirlineConfigurationViewModel airlineConfigVM = new AirlineConfigurationViewModel();

                    int dif = airlineConfigVM.ConvertColumnToInteger(config.StartColumn);

                    int agentCodeCol = -1, firstNameCol = -1, lastNameCol = -1, recordLocatorCol = -1, createdOrgCodeCol = -1, sourceOrgCodeCol = -1,
                        paymentCodeCol = -1, paymentIDCol = -1, authorizationStatusCol = -1, currencyCol = -1, bookingAmountCol = -1, collectedCurrencyCol = -1,
                        collectedAmountCol = -1, convertedCurrencyCol = -1, convertedAmountCol = -1, paymentTextCol = -1, passengerNameCol = -1, routeCol = -1;

                    BilledTicket billed;

                    UnbilledTicket unbilled;

                    NoRecord noRecord;

                    string recordLocator = "";

                    string passengerName = "";

                    var travComVM = new TravcomViewModel();

                    while (dr.Read())
                    {
                        agentName = "";

                        //====READ HEADER====
                        if (!startReadColumn && !ifStartReading)
                        {
                            //Date Range
                            if (dr[0].ToString() == "Organization Code")
                            {
                                for (int ctr = 0; ctr < dr.FieldCount; ctr++)
                                {
                                    if (dr[ctr].ToString().Contains(":"))
                                    {
                                        if (organizationCode == "")
                                            if (dr[ctr + 1].ToString() == "")
                                                organizationCode = " ";
                                            else
                                                organizationCode = dr[ctr + 1].ToString();
                                        else
                                            locationCode = dr[ctr + 1].ToString();
                                    }
                                }
                            }
                            else if(dr[0].ToString() == "Currency Code")
                            {
                                for(int ctr = 0; ctr < dr.FieldCount; ctr++)
                                {
                                    if(dr[ctr].ToString().Contains(":"))
                                    {
                                        if (currencyCode == "")
                                            if (dr[ctr + 1].ToString() == "")
                                                currencyCode = " ";
                                            else
                                                currencyCode = dr[ctr + 1].ToString();
                                        else
                                            deparment = dr[ctr + 1].ToString();
                                    }
                                }
                            }
                            else if(dr[0].ToString() == "FOP")
                            {
                                for(int ctr = 0; ctr < dr.FieldCount; ctr++)
                                {
                                    if (dr[ctr].ToString().Contains(":"))
                                    {
                                        if (FOP == "")
                                        {
                                            if (dr[ctr + 1].ToString() == "")
                                                FOP = " ";
                                            else
                                                FOP = dr[ctr + 1].ToString();
                                        }
                                        else
                                            dateRange = dr[ctr + 1].ToString();
                                    }
                                }

                                using (var db = new AirlineBillingReportEntities())
                                {
                                    var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == _recordNo);

                                    record.C5JCurrency = currencyCode;

                                    record.C5JFOP = FOP;

                                    record.C5JOrgCode = organizationCode;

                                    record.C5JPaymentDate = dateRange;

                                    db.Entry(record).State = System.Data.Entity.EntityState.Modified;

                                    db.SaveChanges();
                                }
                            }
                        }//END OF READ HEADER

                        if (startReadColumn)
                        {
                            for(int ctr = 0; ctr < dr.FieldCount; ctr++)
                            {
                                string temp = dr[ctr].ToString().ToLower();

                                if (temp.Contains("agent code"))
                                    agentCodeCol = ctr;
                                else if (temp.Contains("first name"))
                                    firstNameCol = ctr;
                                else if (temp.Contains("last name"))
                                    lastNameCol = ctr;
                                else if (temp.Contains("record locator"))
                                    recordLocatorCol = ctr;
                                else if (temp.Contains("created organization code"))
                                    createdOrgCodeCol = ctr;
                                else if (temp.Contains("source organization code"))
                                    sourceOrgCodeCol = ctr;
                                else if (temp.Contains("payment code"))
                                    paymentCodeCol = ctr;
                                else if (temp.Contains("payment id"))
                                    paymentIDCol = ctr;
                                else if (temp.Contains("authorization status"))
                                    authorizationStatusCol = ctr;                                
                                else if (temp.Contains("booking amount"))
                                    bookingAmountCol = ctr;
                                else if (temp.Contains("collected currency"))
                                    collectedCurrencyCol = ctr;
                                else if (temp.Contains("collected amount"))
                                    collectedAmountCol = ctr;
                                else if (temp.Contains("converted currency code"))
                                    convertedCurrencyCol = ctr;
                                else if (temp.Contains("converted amount"))
                                    convertedAmountCol = ctr;
                                else if (temp.Contains("payment text"))
                                    paymentTextCol = ctr;
                                else if (temp.Contains("passenger name"))
                                    passengerNameCol = ctr;
                                else if (temp.Contains("route"))
                                    routeCol = ctr;
                                else if (temp.Contains("currency code"))
                                    currencyCol = ctr;
                            }//End of For Loop
                        }//End of IfReadColumn

                        if (ifStartReading)
                        {
                            counter++;

                            if (_mainWindow.loadingForm.lblProcessedCount.InvokeRequired)
                                _mainWindow.loadingForm.lblProcessedCount.Invoke(new MethodInvoker(delegate{ _mainWindow.loadingForm.lblProcessedCount.Text = counter.ToString(); }));

                            if (dr[airlineConfigVM.ConvertColumnToInteger(config.RecordLocatorCol) - dif].ToString() != "") //Record Locator
                            {
                                if (dr[airlineConfigVM.ConvertColumnToInteger(config.AuthorizationStatusCol) - dif].ToString().Replace(" ","") == "Approved" &&
                                    !dr[airlineConfigVM.ConvertColumnToInteger(config.CollectedCurrencyCodeCol) - dif].ToString().Contains("("))
                                {
                                    string currentRecordLocator = "", ticketNo = "", invoiceNo = "", currentPassengerName = "";

                                    int ticketCounting = 0;

                                    currentRecordLocator = dr[recordLocatorCol].ToString();

                                    if (recordLocator != currentRecordLocator)
                                    {
                                        recordLocator = currentRecordLocator;

                                        ticketCounting = 0;
                                    }

                                    //Passenger name
                                    currentPassengerName = dr[passengerNameCol].ToString();

                                    currentPassengerName += " " + dr[passengerNameCol + 1].ToString();

                                    ticketNo = dr[paymentIDCol].ToString();

                                    if (passengerName != currentPassengerName)
                                    {
                                        passengerName = currentPassengerName;

                                        if (ticketCounting <= 10) //Ticket No: X123456789
                                            ticketNo = ticketCounting.ToString() + ticketNo;
                                        else if (ticketCounting >= 11 && ticketCounting <= 100) //Ticket No: XX23456789
                                            ticketNo = ticketCounting.ToString() + ticketNo.Substring(1, 8);
                                        else if (ticketCounting > 100) //Ticket No: XXX3456789
                                            ticketNo = ticketCounting.ToString() + ticketNo.Substring(2, 7);

                                        ticketCounting++;
                                    }
                                    else
                                        ticketNo = ticketCounting.ToString() + ticketNo;

                                    C5J_APAnalysis apAnalysis = new C5J_APAnalysis();

                                    apAnalysis.RecordNo = _recordNo;

                                    apAnalysis.CreatedDate = DateTime.Now;

                                    apAnalysis.PaymentCode = dr[paymentCodeCol].ToString();

                                    apAnalysis.PaymentID = dr[paymentIDCol].ToString();

                                    apAnalysis.AuthorizationStatus = dr[authorizationStatusCol].ToString();

                                    apAnalysis.ConvertedCurrency = dr[convertedCurrencyCol].ToString();

                                    apAnalysis.ConvertedAmount = dr[convertedAmountCol].ToString() != "" ? decimal.Parse(dr[convertedAmountCol].ToString()) : 0;

                                    apAnalysis.PassengerName = dr[passengerNameCol + 1].ToString() + "/" + dr[passengerNameCol].ToString();

                                    apAnalysis.Itinerary = dr[routeCol].ToString() + "-" + dr[routeCol + 1].ToString();

                                    apAnalysis.AgentCode = dr[agentCodeCol].ToString();

                                    apAnalysis.DateRange = dateRange;

                                    var tempAgent = agent.FirstOrDefault(r => r.CebuPacificSafeName == dr[agentCodeCol].ToString().ToUpper());

                                    if (tempAgent != null)
                                    {
                                        apAnalysis.TCName = tempAgent.Name;
                                    }

                                    //Validate if posted via 7 Digit ticket no and via RecordLocator
                                    //To avoid wrong manual encoding of Ticket No
                                    var temp = posted.FirstOrDefault(r => r.TicketNo.Contains(ticketNo.Substring(2,7)) 
                                    && r.RecordLocator == recordLocator);

                                    if (temp != null)
                                        invoiceNo = temp.InvoiceNo;

                                    if (invoiceNo != "") //already posted
                                    {
                                        billed = new BilledTicket();

                                        //AP Analysis
                                        apAnalysis.ClientName = temp.ClientName;

                                        apAnalysis.IsPosted = "Y";

                                        apAnalysis.InvoiceNo = temp.InvoiceNo;

                                        //Passenger Name
                                        billed.PassengerName = passengerName;

                                        //Agent Code
                                        billed.AgentCode = dr[agentCodeCol].ToString();                                    

                                        if (tempAgent != null)
                                        {
                                            billed.Department = tempAgent.Department;

                                            billed.AgentName = tempAgent.Name;
                                        }
                                        else
                                        {
                                            billed.Department = "N/A";

                                            billed.AgentName = "No profile in AEFUR";
                                        }

                                        if (ticketNo == "")
                                            ticketNo = temp.TicketNo;

                                        billed.DateRange = dateRange;

                                        //Record Locator
                                        billed.RecordLocator = recordLocator;

                                        //Created Organization Code
                                        billed.CreatedOrganizationCode = dr[createdOrgCodeCol].ToString();

                                        //Source Organization Code
                                        billed.SourceOrganizationCode = dr[sourceOrgCodeCol].ToString();

                                        //Payment Code
                                        billed.PaymentCode = dr[paymentCodeCol].ToString();

                                        //Payment ID
                                        billed.PaymentID = dr[paymentIDCol].ToString();

                                        //Authorization Status
                                        billed.AuthorizationStatus = dr[authorizationStatusCol].ToString();

                                        //Currency Code
                                        billed.CurrencyCode = dr[currencyCol].ToString();

                                        //Booking Amount
                                        billed.BookingAmount = dr[bookingAmountCol].ToString();

                                        //Collected Currency Code
                                        billed.CollectedCurrencyCode = dr[collectedCurrencyCol].ToString();

                                        //Collected Amount
                                        billed.CollectedAmount = dr[collectedAmountCol].ToString();

                                        //Converted Currency Code
                                        billed.ConvertedCurrencyCode = dr[convertedCurrencyCol].ToString();

                                        //Converted Amount
                                        billed.ConvertedAmount = dr[convertedAmountCol].ToString();

                                        //Payment Text
                                        billed.PaymentText = dr[paymentTextCol].ToString();                                   

                                        //Departure City
                                        billed.DepartureCity = dr[routeCol].ToString();

                                        //Destination City
                                        billed.DestinationCity = dr[routeCol + 1].ToString();

                                        //Exchange Rate
                                        if (Double.Parse(billed.CollectedAmount) > 0)
                                            billed.ExchangeRate = (double.Parse(billed.ConvertedAmount.Replace("(", "").Replace(")", "")) / double.Parse(billed.CollectedAmount.Replace("(", "").Replace(")", ""))).ToString();
                                        else
                                            billed.ExchangeRate = "0.00";

                                        if (config.BookingAmountCol != "")
                                            billed.BookingAmount = dr[airlineConfigVM.ConvertColumnToInteger(config.BookingAmountCol) - dif].ToString();
                                        else
                                            billed.BookingAmount = "0.00";

                                        //Airline
                                        billed.Airline = "Cebu Pacific";

                                        billed.RecordNo = _recordNo;

                                        billed.InvoiceNo = invoiceNo;

                                        billed.TicketNo = temp.TicketNo;

                                        billed.PassengerName = passengerName;

                                        //Client name
                                        billed.ClientName = temp.ClientName;

                                        billed.InvoiceDate = DateTime.Parse(temp.InvoiceDate.ToString()).ToShortDateString();

                                        var billedVM = new BilledTicketViewModel();

                                        if (billedVM.Save(billed)) { }//Successfully saved

                                        _5JApAnalysisViewModel apVM = new _5JApAnalysisViewModel();

                                        apVM.Save(apAnalysis);

                                    }
                                    else //not posted
                                    {
                                        if (config.TicketNo != "")

                                        //Check if record is saved in Back office
                                        temp = voided.FirstOrDefault(r => r.TicketNo.Contains(ticketNo.Substring(2, 7))
                                            && r.RecordLocator == recordLocator);

                                        if (temp == null)
                                        {
                                            noRecord = new NoRecord();

                                            noRecord.AgentCode = dr[agentCodeCol].ToString();

                                            apAnalysis.IsPosted = "N";

                                            //Agent Name

                                            if(tempAgent != null)
                                            {
                                                noRecord.Department = tempAgent.Department;

                                                noRecord.AgentName = tempAgent.Name;
                                            }
                                            else
                                            {
                                                noRecord.Department = "N/A";

                                                noRecord.AgentName = "No profile in AEFUR";
                                            }

                                            noRecord.BookingAmount = dr[bookingAmountCol].ToString();

                                            noRecord.PassengerName = passengerName;
                                            //End of Passenger Name

                                            string itinerary = "";
                                            //Itinerary
                                            itinerary += dr[routeCol].ToString();

                                            itinerary += " - " + dr[routeCol + 1].ToString();

                                            noRecord.Itinerary = itinerary;
                                            //End of Itinerary

                                            noRecord.Currency = dr[currencyCol].ToString();                                                                   

                                            noRecord.Airline = "Cebu Pacific";

                                            noRecord.DateRange = dateRange;

                                            noRecord.RecordLocator = recordLocator;

                                            noRecord.RecordNo = _recordNo;

                                            noRecord.TicketNo = ticketNo;

                                            noRecord.CreatedDate = DateTime.Now;

                                            var noRecordVM = new NoRecordViewModel();

                                            if (noRecordVM.Save(noRecord)) { }//Successfully Save

                                            _5JApAnalysisViewModel apVM = new _5JApAnalysisViewModel();

                                            apVM.Save(apAnalysis);
                                        }
                                        else
                                        {

                                            //Voided Records
                                            temp = voided.FirstOrDefault(r => r.TicketNo.Contains(ticketNo.Substring(2, 7))
                                                && r.RecordLocator == recordLocator && r.GrossAmount < 0 && r.TransactionType == 2);

                                            if (temp == null)
                                            {
                                                temp = voided.FirstOrDefault(r => r.TicketNo.Contains(ticketNo.Substring(2, 7))
                                                    && r.RecordLocator == recordLocator);

                                                unbilled = new UnbilledTicket();

                                                //AP Analysis
                                                apAnalysis.IsPosted = "N";

                                                apAnalysis.ClientName = temp.ClientName;

                                                //Agent Code 
                                                if (config.AgentCodeCol != "")
                                                {
                                                    unbilled.AgentCode = dr[agentCodeCol].ToString();

                                                    if (tempAgent != null)
                                                    {
                                                        unbilled.Department = tempAgent.Department;

                                                        unbilled.AgentName = tempAgent.Name;
                                                    }
                                                    else
                                                    {
                                                        unbilled.Department = "N/A";

                                                        unbilled.AgentName = "No profile in AEFUR";
                                                    }
                                                }
                                                else
                                                {
                                                    unbilled.Department = "";

                                                    unbilled.AgentCode = "";
                                                }

                                                if (ticketNo == "")
                                                {
                                                    ticketNo = temp.TicketNo;
                                                }

                                                unbilled.PassengerName = passengerName;

                                                unbilled.DateRange = dateRange;

                                                unbilled.AgentName = agentName;
                                                //Record Locator
                                                unbilled.RecordLocator = recordLocator;

                                                unbilled.BookingAmount = dr[bookingAmountCol].ToString();

                                                //Created Organization Code
                                                unbilled.CreatedOrganizationCode = dr[createdOrgCodeCol].ToString();

                                                //Source Organization Code
                                                unbilled.SourceOrganizationCode = dr[sourceOrgCodeCol].ToString();

                                                //Payment Code
                                                unbilled.PaymentCode = dr[paymentCodeCol].ToString();

                                                //Payment ID
                                                unbilled.PaymentID = dr[paymentIDCol].ToString();

                                                //Authorization Status
                                                unbilled.AuthorizationStatus = dr[authorizationStatusCol].ToString();

                                                //Currency Code
                                                unbilled.CurrencyCode = dr[currencyCol].ToString();

                                                //Booking Amount
                                                unbilled.BookingAmount = dr[bookingAmountCol].ToString();

                                                //Collected Currency Code
                                                unbilled.CollectedCurrencyCode = dr[collectedCurrencyCol].ToString();

                                                //Collected Amount
                                                unbilled.CollectedAmount = dr[collectedAmountCol].ToString();

                                                //Converted Currency Code
                                                unbilled.ConvertedCurrencyCode = dr[convertedCurrencyCol].ToString();

                                                //Converted Amount
                                                unbilled.ConvertedAmount = dr[convertedAmountCol].ToString();

                                                //Payment Text
                                                unbilled.PaymentText = dr[paymentTextCol].ToString();

                                                //Passenger Name
                                                unbilled.PassengerName = dr[passengerNameCol].ToString();

                                                unbilled.PassengerName = unbilled.PassengerName + " " + dr[passengerNameCol + 1].ToString();

                                                //Departure City
                                                unbilled.DepartureCity = dr[routeCol].ToString();

                                                //Destination City
                                                unbilled.DestinationCity = dr[routeCol + 1].ToString();

                                                //Exchange Rate
                                                if (unbilled.CollectedAmount != "0.00")
                                                    unbilled.ExchangeRate = (double.Parse(unbilled.ConvertedAmount.Replace("(", "").Replace(")", "")) / double.Parse(unbilled.CollectedAmount.Replace("(", "").Replace(")", ""))).ToString();
                                                else
                                                    unbilled.ExchangeRate = "0.00";

                                                unbilled.TicketNo = temp.TicketNo;

                                                //Airline
                                                unbilled.Airline = "Cebu Pacific";

                                                unbilled.RecordNo = _recordNo;

                                                unbilled.ClientName = temp.ClientName;

                                                var unbilledVM = new UnbilledTicketViewModel();

                                                if (unbilledVM.Save(unbilled)) { }//Successfully saved

                                                _5JApAnalysisViewModel apVM = new _5JApAnalysisViewModel();

                                                apVM.Save(apAnalysis);
                                            }
                                        }                                   
                                    }
                                }
                            }
                            else //if no record locator
                            {
                                _mainWindow.CloseProgress();

                                connection.Close();

                                break;
                            }
                        }

                        if (dr[0].ToString().Contains("Agent Code"))
                        {
                            ifStartReading = true;

                            startReadColumn = false;
                        }
                        else if(dr[0].ToString().ToLower().Contains("fop"))
                        {
                            startReadColumn = true;
                        }
                    }
                }
            }
        }
    }
}
