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
    public class PALProcess
    {
        public void Process(string _recordNo, string _path, MainWindow _mainWindow, List<Invoice> posted, List<Invoice> voided
            , List<AirlineBillingReportRepository.AgentProfile> agent)
        {         
            //configuration for cebu pacifc
            var config = new AirlineConfigurationViewModel().GetSelected("PAL");

            //Excel connection string
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";" +"Extended Properties='Excel 12.0 Xml;HDR=No;'";

            string agentName = "";

            int counter = 0;

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();

                OleDbCommand command = new OleDbCommand("select * from [" + config.TabName + "$]", connection);

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    bool ifStartReading = false, ifReadColumn = false;

                    string dateRange = "";

                    BilledTicket billed;

                    UnbilledTicket unbilled;

                    NoRecord noRecord;

                    PAL_APAnalysis apAnalysis;

                    //=============COLUMN NO==============
                    int dateOfIssueCol = -1, carrierCol = -1, saleTypeCol = -1, passengerNameCol = -1, ticketNoCol = -1, marketCol = -1, fareCol = -1,
                        saleStatusCol = -1, recordLocatorCol = -1, segCol = -1, cityPair1Col = -1, cityPair2Col = -1, cityPair3Col = -1, cityPair4Col = -1,
                        IATANoCol = -1, signInCodeCol = -1, remarksCol = -1, currencyCol = -1, totalTaxCol = -1, cashCol = -1, creditCol = -1, commCol = -1,
                        otherCol = -1, baseCol = -1, netCol = -1, payableCol = -1, flightNoCol = -1, departureDateCol = -1, tourCodeCol = -1, bookingClassCol = -1, 
                        fareBasisCol = -1, taxCol = -1, ODCol = -1, PVCol = -1, PDCol = -1, LICol = -1, NSCol = -1;

                    var travCom = new TravcomViewModel();

                    _mainWindow.StartProgress("Processing PAL . . .");

                    string ticketNo = "";

                    string recordLocator = "";

                    while (dr.Read())
                    {
                        agentName = "";

                        ticketNo = "";

                        if(dr[0].ToString().Contains("Report date"))
                        {
                            string reportDate = "", reportTime = "", totalRecord = "", pcc = "", dateRangeHeader = "";

                            for(int ctr=0; ctr <dr.FieldCount; ctr++)
                            {
                                if (!dr[ctr].ToString().ToLower().Contains("report date") && !dr[ctr].ToString().ToLower().Contains("pcc") && dr[ctr].ToString() != "")
                                {
                                    if (reportDate == "")
                                        reportDate = dr[ctr].ToString();
                                    else
                                        pcc = dr[ctr].ToString();
                                }
                            }

                            dr.Read(); //Move Down

                            for (int ctr = 0; ctr < dr.FieldCount; ctr++)
                            {
                                if (!dr[ctr].ToString().ToLower().Contains("report time") && !dr[ctr].ToString().ToLower().Contains("agency name") && dr[ctr].ToString() != "")
                                {
                                    if (reportTime == "")
                                        reportTime = dr[ctr].ToString();
                                }
                            }

                            dr.Read(); // Move Down

                            for (int ctr = 0; ctr < dr.FieldCount; ctr++)
                            {
                                if (!dr[ctr].ToString().ToLower().Contains("total records") && !dr[ctr].ToString().ToLower().Contains("date range") && dr[ctr].ToString() != "")
                                {
                                    if (totalRecord == "")
                                        totalRecord = dr[ctr].ToString();
                                    else
                                        dateRangeHeader = dr[ctr].ToString();
                                }
                            }
                            //============Save AP Analysis Header============
                            using (var db = new AirlineBillingReportEntities())
                            {
                                var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == _recordNo);

                                record.PALReportDate = reportDate;

                                record.PALReportTime = reportTime;

                                record.PALTotalRecord = totalRecord;

                                record.PALPCC = pcc;

                                record.PALDateRange = dateRangeHeader;

                                db.Entry(record).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();
                            }
                        }

                        if (ifReadColumn) //Get Column No
                        {
                            for(int ctr= 0; ctr < dr.FieldCount; ctr++)
                            {
                                string temp = dr[ctr].ToString();

                                if (temp.Contains("Date of Issue"))
                                    dateOfIssueCol = ctr;
                                else if (temp.Contains("Carrier"))
                                    carrierCol = ctr;
                                else if (temp.Contains("Sale Type"))
                                    saleTypeCol = ctr;
                                else if (temp.Contains("Pax Name"))
                                    passengerNameCol = ctr;
                                else if (temp.Contains("Ticket Number"))
                                    ticketNoCol = ctr;
                                else if (temp.Contains("Market"))
                                    marketCol = ctr;
                                else if (temp.Contains("Fare Source"))
                                    fareCol = ctr;
                                else if (temp.Contains("Sale Status"))
                                    saleStatusCol = ctr;
                                else if (temp.Contains("PNR")) //reloc
                                    recordLocatorCol = ctr;
                                else if (temp.Contains("Seg"))
                                    segCol = ctr;
                                else if (temp.Contains("Citypair1"))
                                    cityPair1Col = ctr;
                                else if (temp.Contains("Citypair2"))
                                    cityPair2Col = ctr;
                                else if (temp.Contains("Citypair3"))
                                    cityPair3Col = ctr;
                                else if (temp.Contains("Citypair4"))
                                    cityPair4Col = ctr;
                                else if (temp.Contains("IATA No"))
                                    IATANoCol = ctr;
                                else if (temp.Contains("SINE"))//Sign in code
                                    signInCodeCol = ctr;
                                else if (temp.Contains("Remarks"))
                                    remarksCol = ctr;
                                else if (temp.Contains("CURR"))
                                    currencyCol = ctr;
                                else if (temp.Contains("Total Tax"))
                                    totalTaxCol = ctr;
                                else if (temp.Contains("Cash"))
                                    cashCol = ctr;
                                else if (temp.Contains("Credit"))
                                    creditCol = ctr;
                                else if (temp.Contains("Comm"))
                                    commCol = ctr;
                                else if (temp.Contains("Others"))
                                    otherCol = ctr;
                                else if (temp.Contains("Base Fare"))
                                    baseCol = ctr;
                                else if (temp.Contains("Net Fare"))
                                    netCol = ctr;
                                else if (temp.Contains("Payable"))
                                    payableCol = ctr;
                                else if (temp.Contains("Flight Number"))
                                    flightNoCol = ctr;
                                else if (temp.Contains("Departure Date"))
                                    departureDateCol = ctr;
                                else if (temp.Contains("Tour Code"))
                                    tourCodeCol = ctr;
                                else if (temp.Contains("Booking Class"))
                                    bookingClassCol = ctr;
                                else if (temp.Contains("Fare Basis Code"))
                                    fareBasisCol = ctr;
                                else if (temp.Contains("Tax"))
                                    taxCol = ctr;
                                else if (temp.Contains("OD"))
                                    ODCol = ctr;
                                else if (temp.Contains("PV"))
                                    PVCol = ctr;
                                else if (temp.Contains("PD"))
                                    PDCol = ctr;
                                else if (temp.Contains("LI"))
                                    LICol = ctr;
                                else if (temp.Contains("NS"))
                                    NSCol = ctr;                                
                            }//End of For Loop
                        } //End of ifReadColumn

                        if (ifStartReading)
                        {
                            counter++;

                            if (_mainWindow.loadingForm.lblProcessedCount.InvokeRequired)
                                _mainWindow.loadingForm.lblProcessedCount.Invoke(new MethodInvoker(delegate{ _mainWindow.loadingForm.lblProcessedCount.Text = counter.ToString(); }));

                            //TicketNo
                            ticketNo = dr[ticketNoCol].ToString();

                            string status = dr[saleStatusCol].ToString();

                            if(ticketNo == "2315916990")
                            {

                            }

                            if (ticketNo != "")
                            {
                                recordLocator = dr[recordLocatorCol].ToString();

                                var temp = posted.FirstOrDefault(r => r.TicketNo == ticketNo);

                                string invoiceNo = "";

                                if (temp != null)
                                    invoiceNo = temp.InvoiceNo;

                                var tempAgent = agent.FirstOrDefault(r => r.PALSafeName == dr[signInCodeCol].ToString().ToUpper());

                                if (tempAgent != null)
                                    agentName = tempAgent.Name;
                                else
                                    agentName = "";

                                dateRange = dr[dateOfIssueCol].ToString();

                                //==========AP Analysis=========
                                apAnalysis = new PAL_APAnalysis();

                                apAnalysis.DateOfIssue = dr[dateOfIssueCol].ToString();

                                apAnalysis.Carrier = dr[carrierCol].ToString();

                                apAnalysis.PassengerName = dr[passengerNameCol].ToString();

                                apAnalysis.TicketNo = dr[ticketNoCol].ToString();

                                apAnalysis.Market = dr[marketCol].ToString();

                                apAnalysis.FareSource = dr[fareCol].ToString();

                                apAnalysis.SaleStatus = dr[saleStatusCol].ToString();

                                apAnalysis.RecordLocator = dr[recordLocatorCol].ToString();

                                apAnalysis.Seg = dr[segCol].ToString();

                                apAnalysis.Itinerary = dr[cityPair1Col].ToString() + "-" + dr[cityPair2Col].ToString();

                                apAnalysis.IATANo = dr[IATANoCol].ToString();

                                apAnalysis.SignInCode = dr[signInCodeCol].ToString();

                                apAnalysis.TCName = agentName;

                                apAnalysis.Remarks = dr[remarksCol].ToString();

                                apAnalysis.Currency = dr[currencyCol].ToString();

                                apAnalysis.TotalTax = decimal.Parse(dr[totalTaxCol].ToString());

                                apAnalysis.Cash = decimal.Parse(dr[cashCol].ToString());

                                apAnalysis.Credit = decimal.Parse(dr[creditCol].ToString());

                                apAnalysis.Commission = decimal.Parse(dr[commCol].ToString());

                                apAnalysis.Others = decimal.Parse(dr[otherCol].ToString());

                                apAnalysis.BaseFare = decimal.Parse(dr[baseCol].ToString());

                                apAnalysis.NetFare = decimal.Parse(dr[netCol].ToString());

                                apAnalysis.Payable = decimal.Parse(dr[payableCol].ToString());

                                apAnalysis.FlightNumber = dr[flightNoCol].ToString();

                                apAnalysis.DepartureDate = dr[departureDateCol].ToString();

                                apAnalysis.TourCode = dr[tourCodeCol].ToString();

                                apAnalysis.BookingClass = dr[bookingClassCol].ToString();

                                apAnalysis.FareBasisCode = dr[fareBasisCol].ToString();

                                apAnalysis.TaxBreakdown = "PD:" + dr[PDCol].ToString() + "," + "LI:" + dr[LICol].ToString() + "," + "PV:" + dr[PVCol].ToString();

                                apAnalysis.PD = dr[PDCol].ToString() != "" ? decimal.Parse(dr[PDCol].ToString()) : 0;

                                apAnalysis.LI = dr[LICol].ToString() != "" ? decimal.Parse(dr[LICol].ToString()) : 0;

                                apAnalysis.PV = dr[PVCol].ToString() != "" ? decimal.Parse(dr[PVCol].ToString()) : 0;

                                apAnalysis.OD = dr[ODCol].ToString() != "" ? decimal.Parse(dr[ODCol].ToString()) : 0;

                                apAnalysis.NS = dr[NSCol].ToString() != "" ? decimal.Parse(dr[NSCol].ToString()) : 0;

                                apAnalysis.RecordNo = _recordNo;

                                if (invoiceNo != "") //Already posted
                                {
                                    billed = new BilledTicket();

                                    billed.AgentCode = dr[signInCodeCol].ToString();

                                    if (tempAgent != null)
                                    {
                                        billed.Department = tempAgent.Department;

                                        agentName = tempAgent.Name;
                                    }
                                    else
                                        billed.Department = "";

                                    billed.DateRange = dateRange;


                                    billed.AgentName = temp.BookingAgentName;

                                    var agentProfile = agent.FirstOrDefault(r => r.TravCom1 == temp.BookingAgent || r.TravCom2 == temp.BookingAgent ||
                                        r.TravCom3 == temp.BookingAgent || r.TravCom4 == temp.BookingAgent || r.TravCom5 == temp.BookingAgent);

                                    if(agentProfile != null)
                                        billed.Department = agentProfile.Department;

                                    billed.RecordLocator = recordLocator;

                                    billed.TicketNo = ticketNo;
                                                          
                                    billed.CurrencyCode = dr[currencyCol].ToString();
                         
                                    billed.ConvertedCurrencyCode = dr[currencyCol].ToString();

                                    billed.ConvertedAmount = dr[cashCol].ToString();

                                    billed.PassengerName = dr[passengerNameCol].ToString();

                                    billed.DepartureCity = dr[cityPair1Col].ToString();

                                    billed.DestinationCity = dr[cityPair2Col].ToString();

                                    billed.AirlineCode = dr[carrierCol].ToString();

                                    billed.Airline = "Philippine Airlines";

                                    billed.RecordNo = _recordNo;

                                    billed.InvoiceNo = invoiceNo;

                                    billed.ClientName = temp.ClientName;

                                   //==========AP Analysis=========
                                    apAnalysis.IsPosted = "Y";

                                    apAnalysis.InvoiceNo = invoiceNo;

                                    apAnalysis.ClientName = temp.ClientName;
                                    
                                    var billedVM = new BilledTicketViewModel();
                                    var apVM = new PALAPAnalysisViewModel();

                                    if (!status.Contains('X'))
                                    {
                                        if (billedVM.Save(billed)) { }//Successfully saved
                                    }

                                    if (apVM.Save(apAnalysis)) { }//Successfully saved
                                }
                                else //not yet posted
                                {
                                    temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo);

                                    if (temp == null)
                                    {
                                        noRecord = new NoRecord();

                                        noRecord.AgentCode = dr[signInCodeCol].ToString();

                                        agentName = "";

                                        if (tempAgent != null)
                                        {
                                            noRecord.Department = tempAgent.Department;

                                            agentName = tempAgent.Name;
                                        }

                                        noRecord.AgentName = agentName;

                                        noRecord.Airline = "Philippine Airlines";

                                        noRecord.DateRange = dateRange;

                                        noRecord.RecordLocator = recordLocator;

                                        noRecord.TicketNo = ticketNo;

                                        noRecord.RecordNo = _recordNo;

                                        noRecord.CreatedDate = DateTime.Now;

                                        //==========AP Analysis=========
                                        apAnalysis.IsPosted = "N";

                                        var noRecordVM = new NoRecordViewModel();

                                        if (!status.Contains('X'))
                                        {
                                            if (noRecordVM.Save(noRecord)) { }//Successfully Save
                                        }

                                        var apVM = new PALAPAnalysisViewModel();

                                        if (apVM.Save(apAnalysis)) { } //Successfully saved
                                    }
                                    else
                                    {
                                        //Check if voided
                                        temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo && r.TransactionType == 2);

                                        if (temp == null)
                                        {
                                            unbilled = new UnbilledTicket();

                                            temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo);

                                            unbilled.ClientName = temp.ClientName;

                                            unbilled.DepartureCity = temp.Itinerary;

                                            unbilled.AgentName = temp.BookingAgentName;

                                            var agentProfile = agent.FirstOrDefault(r => r.TravCom1 == temp.BookingAgent || r.TravCom2 == temp.BookingAgent ||
                                                r.TravCom3 == temp.BookingAgent || r.TravCom4 == temp.BookingAgent || r.TravCom5 == temp.BookingAgent);

                                            if (agentProfile != null)
                                            {
                                                unbilled.Department = agentProfile.Department;
                                            }

                                            unbilled.DateRange = dateRange;                                            

                                            //Department
                                            if (agentName != "")
                                            {
                                                if (tempAgent != null)
                                                    unbilled.Department = tempAgent.Department;
                                                else
                                                    unbilled.Department = "N/A";
                                            }

                                            unbilled.RecordLocator = recordLocator;

                                            unbilled.TicketNo = ticketNo;
                                      
                                            unbilled.CurrencyCode = dr[currencyCol].ToString();

                                            unbilled.ConvertedCurrencyCode = dr[currencyCol].ToString();

                                            unbilled.ConvertedAmount = dr[cashCol].ToString();
                                       
                                            unbilled.PassengerName = dr[passengerNameCol].ToString();

                                            unbilled.DepartureCity = dr[cityPair1Col].ToString();

                                            unbilled.DestinationCity = dr[cityPair2Col].ToString();

                                            unbilled.Airline = "Philippine Airlines";

                                            unbilled.AirlineCode = dr[carrierCol].ToString();

                                            unbilled.RecordNo = _recordNo;

                                            unbilled.ClientName = temp.ClientName;

                                            var unbilledVM = new UnbilledTicketViewModel();

                                            if (!status.Contains('X'))
                                            {
                                                if (unbilledVM.Save(unbilled)) { }//Successfully saved
                                            }
                                        }
                                        //==========AP Analysis=========
                                        apAnalysis.IsPosted = "N";

                                        apAnalysis.ClientName = temp.ClientName;

                                        var apVM = new PALAPAnalysisViewModel();

                                        if (apVM.Save(apAnalysis)) { } //Successfully saved
                                    }                                 
                                }                         
                            }

                            if(dr[0].ToString().Contains("Sum (Total Records)"))// no record locator
                            {
                                _mainWindow.CloseProgress();

                                connection.Close();

                                break;
                            }                   
                        }

                        if (dr[0].ToString() == "Date of Issue")
                        {
                            ifStartReading = true;

                            ifReadColumn = false;
                        }
                        else if (dr[0].ToString().Contains("Data Not available for"))
                        {
                            ifReadColumn = true;
                        }
                    }
                }
            }
        }
    }
}
