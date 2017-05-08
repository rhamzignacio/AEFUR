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
    public class IATAProcess
    {
        public void Process(string _recordNo, string _path, MainWindow _mainWindow, List<Invoice> posted, List<Invoice> voided
            , List<AirlineBillingReportRepository.AgentProfile> agent)
        {
            _mainWindow.StartProgress("Processing IATA. . .");
            //========================CONFIGURATION FOR IATA=========================
            var config = new AirlineConfigurationViewModel().GetSelected("IATA");

            //======================EXCEL CONNECTION STRING=========================
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";" + "Extended Properties='Excel 12.0 Xml;HDR=No;'";

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();

                //=======================EXCEL QUERY STRING==============================
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    bool startReading = false, startDebit = false, startRefunds = false, startCredit = false;

                    AirlineConfigurationViewModel airlineConfigVM = new AirlineConfigurationViewModel();

                    int dif = airlineConfigVM.ConvertColumnToInteger(config.StartColumn);

                    int counter = 0;

                    BilledTicket billed;

                    IATA iata;

                    UnbilledTicket unbilled;

                    NoRecord noRecord;

                    var iataVM = new IATAViewModel();

                    var travCom = new TravcomViewModel();

                    string ticketNo = "", airlineCode = "", issueDate = "", CPUI = "", remarks = "", currency = "PHP";

                    decimal transAmount = 0;

                    var travcomCurrency = new TravcomViewModel().GetCurrency("USD"); //get USD rate from Client Magic                 

                    while (dr.Read())
                    {
                        transAmount = 0;

                        //==================GET IATA REFERENCE NUMBER & DATE RANGE==================
                        if(dr[0].ToString().Contains("Billing Period") && dr[2].ToString().Contains("REFERENCE"))
                        {
                            string temp = dr[0].ToString();

                            var billingPeriod = dr[0].ToString().Substring(temp.IndexOf("("), (temp.Length - temp.IndexOf("("))).Replace("(", "").Replace(")", "") ;

                            temp = dr[2].ToString();

                            var refNo = dr[2].ToString().Substring(temp.IndexOf(":") + 1, 8).Replace(" ", "");

                            using (var db = new AirlineBillingReportEntities())
                            {
                                var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == _recordNo);

                                record.IATADateRange = billingPeriod;

                                record.IATAReference = refNo;

                                db.Entry(record).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();
                            }
                        }

                        string forCurr = "";

                        for(int ctr =0; ctr<dr.FieldCount; ctr++)
                        {
                            forCurr += dr[ctr].ToString();
                        }

                        if (forCurr.Contains("END") && forCurr.Contains("CURRENCY") && forCurr.Contains("PHP"))
                        {
                            currency = "USD";
                        }

                        if (forCurr.Contains("END") && forCurr.Contains("CURRENCY") && forCurr.Contains("USD"))
                        {
                            _mainWindow.CloseProgress();

                            connection.Close();

                            break;
                        }

                        //===================TURN OFF TRIGGER FOR REFUNDS || DEBIT MEMO=====================
                        if (forCurr.Contains("REFUNDS") && forCurr.Contains("TOTAL"))
                            startRefunds = false;

                        if (forCurr.Contains("DEBIT") && forCurr.Contains("MEMOS") && forCurr.Contains("TOTAL"))
                            startDebit = false;


                        //====================REFUNDS===========================
                        if(startRefunds)
                        {
                            if(dr[1].ToString().Contains("REFUNDS TOTAL"))
                            {
                                startRefunds = false;                             
                            }

                            if(dr[1].ToString() == "RFND" && dr[2].ToString() != null)
                            {
                                iata = new IATA();

                                iata.AirlineCode = dr[0].ToString();

                                iata.TicketNo = dr[2].ToString();

                                iata.IssueDate = dr[3].ToString();

                                double amount = 0;

                                if (currency == "PHP")
                                {
                                    int ctr = 1;

                                    while(dr[dr.FieldCount - ctr].ToString() == "")
                                    {
                                        ctr++;
                                    }

                                    amount = double.Parse(dr[dr.FieldCount - ctr].ToString());

                                    iata.PHPAmount = decimal.Round(decimal.Parse(amount.ToString()), 2, MidpointRounding.AwayFromZero);
                                }
                                else //USD
                                {
                                    int ctr = 1;

                                    while (dr[dr.FieldCount - ctr].ToString() == "")
                                    {
                                        ctr++;
                                    }

                                    amount = double.Parse(dr[dr.FieldCount - ctr].ToString());

                                    iata.USDAmount = decimal.Round(decimal.Parse(amount.ToString()), 2, MidpointRounding.AwayFromZero);
                                }

                                var refund = posted.FirstOrDefault(r => r.TicketNo == iata.TicketNo);//check in posted record

                                if (refund == null)
                                    refund = voided.FirstOrDefault(r => r.TicketNo == iata.TicketNo);//check in unposted record

                                if (refund != null)
                                {
                                    iata.InvoiceDate = DateTime.Parse(refund.InvoiceDate.ToString()).ToShortDateString();

                                    iata.InvoiceNo = refund.InvoiceNo;

                                    iata.RecordLocator = refund.RecordLocator;

                                    iata.PassengerName = refund.PassengerName;

                                    iata.Itinerary = refund.Itinerary;

                                    iata.AgentName = refund.BookingAgentName;

                                    iata.ClientName = refund.ClientName;

                                    iata.Status = "";
                                }
                                else
                                {
                                    //======================BLANK FIELDS FOR IATA=========================
                                    iata.Status = iata.InvoiceDate = iata.InvoiceStatus = iata.InvoiceNo = iata.RecordLocator
                                        = iata.PassengerName = iata.Itinerary = iata.AgentName = iata.ClientName = "";
                                }

                                var tempRefund = posted.FirstOrDefault(r => r.TicketNo.Contains(ticketNo));

                                if (tempRefund == null)
                                    tempRefund = voided.FirstOrDefault(r => r.TicketNo.Contains(ticketNo));

                                if (tempRefund != null)
                                {
                                    iata.Itinerary = tempRefund.Itinerary;
                                }

                                iata.CreatedDate = DateTime.Now;

                                iata.IsPosted = "R";

                                iata.IATACurrency = currency;

                                iata.RecordNo = _recordNo;

                                iataVM = new IATAViewModel();

                                iata.Remarks = "RFND";

                                if (iataVM.Save(iata)) { } //Successfully saved
                            }
                        }//END OF REFUNDS

                        //====================START DEBIT MEMO=====================
                        if (startDebit)
                        {
                            if (dr[1].ToString() == "ADMA" && dr[2].ToString() != "")
                            {                              
                                int ctr = 1;

                                while (dr[dr.FieldCount - ctr].ToString() == "")
                                    ctr++;

                                try
                                {
                                    transAmount = decimal.Parse(dr[dr.FieldCount - ctr].ToString());
                                }
                                catch
                                {
                                    transAmount = 0;
                                }

                                //=====AP Analysis=====
                                iata = new IATA();

                                iata.AirlineCode = dr[0].ToString();

                                iata.Remarks = dr[1].ToString();

                                iata.TicketNo = dr[2].ToString();

                                iata.IssueDate = dr[3].ToString();

                                if (currency == "PHP")
                                    iata.PHPAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);
                                else
                                    iata.USDAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);

                                iata.Status = iata.InvoiceDate = iata.InvoiceStatus = iata.InvoiceNo = iata.RecordLocator =
                                        iata.PassengerName = iata.Itinerary = iata.AgentName = iata.ClientName = "";

                                iata.CreatedDate = DateTime.Now;

                                iata.IsPosted = "D";

                                iata.RecordNo = _recordNo;

                                iata.IATACurrency = currency;

                                iataVM = new IATAViewModel();

                                if (iataVM.Save(iata)) { } //Successfully saved
                            }
                        }//END OF DEBIT


                        //====================CREDIT MEMO===================
                        if (startCredit)
                        {
                            if (dr[1].ToString() == "ACMA" && dr[2].ToString() != "")
                            {
                                int ctr = 1;

                                while (dr[dr.FieldCount - ctr].ToString() == "")
                                    ctr++;

                                try
                                {
                                    transAmount = decimal.Parse(dr[dr.FieldCount - ctr].ToString());
                                }
                                catch
                                {
                                    transAmount = 0;
                                }

                                iata = new IATA();

                                iata.AirlineCode = dr[0].ToString();

                                iata.Remarks = dr[1].ToString();

                                iata.TicketNo = dr[2].ToString();

                                iata.IssueDate = dr[3].ToString();

                                if (currency == "PHP")
                                    iata.PHPAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);
                                else
                                    iata.USDAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);

                                iata.CreatedDate = DateTime.Now;

                                iata.IsPosted = "C";

                                iata.RecordNo = _recordNo;

                                iata.IATACurrency = currency;

                                iataVM = new IATAViewModel();

                                if (iataVM.Save(iata)) { } //Successfully Saved
                            }
                        }//END OF CREDIT
                        
                        //====================UNBILLED || POSTED || NO RECORD==================
                        if (startReading)
                        {
                            airlineCode = dr[0].ToString();

                            ticketNo = dr[2].ToString();

                            issueDate = dr[3].ToString();

                            CPUI = dr[4].ToString();

                            if (dr[1].ToString() == "TKTT" || dr[1].ToString() == "EMDS")
                            {
                                int ctr = 1;

                                while (dr[dr.FieldCount - ctr].ToString() == "")
                                    ctr++;

                                try
                                {
                                    transAmount = decimal.Parse(dr[dr.FieldCount - ctr].ToString());
                                }
                                catch
                                {
                                    transAmount = 0;
                                }
                            }

                            remarks = dr[1].ToString();

                            if((remarks == "TKTT" || remarks == "EMDS") && ticketNo != "")
                            {
                                counter++;

                                if (_mainWindow.loadingForm.lblProcessedCount.InvokeRequired)
                                    _mainWindow.loadingForm.lblProcessedCount.Invoke(new MethodInvoker(delegate{ _mainWindow.loadingForm.lblProcessedCount.Text = counter.ToString(); }));

                                //========================Check if already posted===========================
                                var temp = posted.FirstOrDefault(r => r.TicketNo.Contains(ticketNo));

                                if(temp != null)//Already Posted
                                {
                                    billed = new BilledTicket();
                                    iata = new IATA();

                                    iata.IsPosted = "Y";

                                    iata.CreatedDate = DateTime.Now;

                                    billed.TicketNo = iata.TicketNo = ticketNo; //TicketNo

                                    billed.RecordLocator = iata.RecordLocator = temp.RecordLocator; //RecordLocator

                                    var agentInfo = agent.FirstOrDefault(r => r.TravCom1 == temp.BookingAgent || r.TravCom2 == temp.BookingAgent ||
                                        r.TravCom3 == temp.BookingAgent || r.TravCom4 == temp.BookingAgent || r.TravCom5 == temp.BookingAgent);

                                    if(agentInfo != null)
                                    {
                                        billed.AgentName = iata.AgentName = agentInfo.Name;

                                        billed.AgentCode = agentInfo.IATA;

                                        billed.Department = agentInfo.Department;
                                    }

                                    billed.InvoiceNo = iata.InvoiceNo = temp.InvoiceNo; //InvoiceNo

                                    billed.InvoiceDate = iata.InvoiceDate = DateTime.Parse(temp.InvoiceDate.ToString()).ToShortDateString();

                                    billed.AirlineCode = iata.AirlineCode = airlineCode; //AirlineCode

                                    billed.ConvertedAmount = transAmount.ToString(); //Transaction Amount

                                    billed.ConvertedCurrencyCode = currency;

                                    billed.PassengerName = iata.PassengerName = temp.PassengerName;

                                    billed.Airline = "IATA";

                                    billed.RecordNo = iata.RecordNo = _recordNo;

                                    billed.ClientName = iata.ClientName = temp.ClientName; //ClientName

                                    billed.DateRange = iata.IssueDate = issueDate;
                                  
                                    //=========FOR IATA TABLE===========
                                    iata.Itinerary = temp.Itinerary;

                                    iata.InvoiceStatus = "Posted";

                                    iata.Remarks = remarks;

                                    iata.Status = CPUI;

                                    if (currency == "PHP")
                                        iata.PHPAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);
                                    else
                                        iata.USDAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);

                                    iataVM = new IATAViewModel();

                                    iata.IATACurrency = currency;

                                    if (iataVM.Save(iata)) { }

                                    var billedVM = new BilledTicketViewModel();

                                    if (billedVM.Save(billed)) { }
                                }//=====END OF ALREADY POSTED=====
                                else
                                {
                                    //===============Check if no Record==================
                                    temp = voided.FirstOrDefault(r => r.TicketNo.Contains(ticketNo));

                                    if(temp == null)
                                    {
                                        noRecord = new NoRecord();
                                        iata = new IATA();

                                        noRecord.CreatedDate = iata.CreatedDate = DateTime.Now; //Created Date

                                        noRecord.Airline = "IATA";

                                        noRecord.RecordNo = iata.RecordNo = _recordNo;

                                        noRecord.RecordLocator = iata.RecordLocator = ""; //Record Locator

                                        noRecord.TicketNo = iata.TicketNo = ticketNo; //TicketNo

                                        iata.IsPosted = "N";

                                        iata.Status = CPUI;

                                        noRecord.DateRange = iata.IssueDate = issueDate; //IssueDate

                                        iata.AirlineCode = airlineCode; //AirlineCode

                                        if (currency == "PHP")
                                            iata.PHPAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);
                                        else
                                            iata.USDAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);

                                        iataVM = new IATAViewModel();

                                        iata.Remarks = remarks;

                                        iata.IATACurrency = currency;

                                        iata.InvoiceStatus = "No record";

                                        if (iataVM.Save(iata)) { }//Successfully Saved

                                        var noRecordVM = new NoRecordViewModel();

                                        if (noRecordVM.Save(noRecord)) { }; //Successfully Saved
                                    }
                                    else //==========UNPOSTED RECORD===========
                                    {
                                        temp = voided.FirstOrDefault(r => r.TicketNo.Contains(ticketNo) && r.TransactionType == 1);

                                        if(temp != null)
                                        {
                                            unbilled = new UnbilledTicket();
                                            iata = new IATA();

                                            iata.IsPosted = "N";

                                            unbilled.CreatedDate = iata.CreatedDate = DateTime.Now;

                                            unbilled.TicketNo = iata.TicketNo = ticketNo;

                                            unbilled.RecordLocator = iata.RecordLocator = temp.RecordLocator;

                                            var agentInfo = agent.FirstOrDefault(r => r.TravCom1 == temp.BookingAgent || r.TravCom2 == temp.BookingAgent ||
                                              r.TravCom3 == temp.BookingAgent || r.TravCom4 == temp.BookingAgent || r.TravCom5 == temp.BookingAgent);

                                            if (agentInfo != null)
                                            {
                                                unbilled.AgentName = iata.AgentName = agentInfo.Name;

                                                unbilled.AgentCode = agentInfo.IATA;

                                                unbilled.Department = agentInfo.Department;
                                            }
                                            else
                                                unbilled.AgentName = iata.AgentName = "";

                                            unbilled.AirlineCode = iata.AirlineCode = airlineCode;

                                            unbilled.ConvertedAmount = transAmount.ToString();

                                            unbilled.PassengerName = iata.PassengerName = temp.PassengerName;

                                            unbilled.Airline = "IATA";

                                            unbilled.RecordNo = iata.RecordNo = _recordNo;

                                            if (currency == "PHP")
                                            {
                                                double phpValue = double.Parse(transAmount.ToString());

                                                if (phpValue > 0)
                                                {

                                                    iata.PHPAmount = decimal.Round(decimal.Parse(phpValue.ToString()), 2, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                    iata.PHPAmount = 0;
                                            }
                                            else
                                                iata.USDAmount = decimal.Round(decimal.Parse(transAmount.ToString()), 2, MidpointRounding.AwayFromZero);

                                            unbilled.ClientName = iata.ClientName = temp.ClientName;

                                            unbilled.DateRange = iata.IssueDate = issueDate;

                                            iataVM = new IATAViewModel();

                                            //=============for IATA Table====================

                                            iata.Itinerary = temp.Itinerary;

                                            iata.InvoiceStatus = "Unposted";

                                            iata.Remarks = remarks;

                                            iata.InvoiceDate = "";

                                            iata.InvoiceNo = "";

                                            iata.IATACurrency = currency;

                                            iata.Status = CPUI;

                                            iata.TicketNo = ticketNo;

                                            if (iataVM.Save(iata)) { } //Successfully Saved

                                            var unbilledVM = new UnbilledTicketViewModel();

                                            if (unbilledVM.Save(unbilled)) { } //Successfully Saved
                                        }
                                    }
                                }
                            }
                        }

                        //======================Should Be on BOTTOM===========================
                        if(forCurr.Contains("***") && forCurr.Contains("ISSUES"))
                        {
                            startReading = true;

                            startCredit = startDebit = startRefunds = false;
                        }         
                        else if(forCurr.Contains("***") && forCurr.Contains("REFUNDS"))
                        {
                            startReading = startDebit = startCredit = false;

                            startRefunds = true;
                        }
                        else if(forCurr.Contains("***") && forCurr.Contains("DEBIT") && forCurr.Contains("MEMOS"))
                        {
                            startDebit = true;

                            startReading = startRefunds = startCredit = false;
                        }
                        else if(forCurr.Contains("***") && forCurr.Contains("CREDIT") && forCurr.Contains("MEMOS"))
                        {
                            startCredit = true;

                            startDebit = startRefunds = startReading = false;
                        }
                    }
                }
            }
        }
    }
}
