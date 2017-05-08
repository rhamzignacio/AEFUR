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
    public class IASAProcess
    {

        public void Process(string _recordNo, string _path, MainWindow _mainWindow, List<Invoice> posted, List<Invoice> voided
            , List<AirlineBillingReportRepository.AgentProfile> agent)
        {
            _mainWindow.StartProgress("Processing IASA . . .");
            //configuration for cebu pacifc
            var config = new AirlineConfigurationViewModel().GetSelected("IASA");

            //Excel connection string
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";" +
                 "Extended Properties='Excel 12.0 Xml;HDR=No;'";

            string agentName = "", currency = "";

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();

                OleDbCommand command = new OleDbCommand("select * from [" + config.TabName + "$]", connection);

                //===============COLUMN NO=====================
                int dateCol = -1, transCol = -1, refCol = -1, vesselCol = -1, refOrderCol = -1, travelledCol = -1, paxNameCol = -1, routeCol = -1,
                    documentCol = -1, airTicketCol = -1, classCol = -1, netFareCol = -1, taxesCol = -1, amtDRCRCol = -1;

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    bool startReading = false, startReadColumn = false;

                    string dateRange = "";

                    var travCom = new TravcomViewModel();

                    int counter = 0;

                    BilledTicket billed;

                    UnbilledTicket unbilled;

                    NoRecord noRecord;

                    IASA_APAnalysis apAnalysis;

                    string ticketNo = "";

                    while (dr.Read())
                    {
                        agentName = "";

                        //====================ASSIGN VAlUE TO COLUMN NO===============
                        if(dr[4].ToString().ToLower().Contains("run on"))
                        {
                            string account = "", runOn = "", asAt = "";

                            runOn = dr[5].ToString();

                            dr.Read();

                            for(int ctr = 0; ctr < dr.FieldCount; ctr++)
                            {
                                string temp = dr[ctr].ToString().ToLower();

                                if(!temp.Contains("account") && !temp.Contains("name") && !temp.Contains("philscan") &&
                                    !temp.Contains("as at") && !temp.Contains("currency") && temp != "")
                                {
                                    if (account == "")
                                        account = dr[ctr].ToString();
                                    else if (asAt == "")
                                        asAt = dr[ctr].ToString();
                                    else if (currency == "")
                                    {
                                        currency = dr[ctr].ToString();

                                        break;
                                    }
                                }
                            }

                            using (var db = new AirlineBillingReportEntities())
                            {
                                var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == _recordNo);

                                record.IASAAccount = account;

                                record.IASAAsAt = asAt;

                                record.IASACurrency = currency;

                                record.IASADueDate = "";

                                record.IASARunOn = runOn;

                                db.Entry(record).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();
                            }
                        }

                        if (startReadColumn)
                        {
                            for(int ctr = 0; ctr < dr.FieldCount; ctr++)
                            {
                                string temp = dr[ctr].ToString().ToLower();

                                if (temp.Contains("date"))
                                    dateCol = ctr;
                                else if (temp.Contains("trans"))
                                    transCol = ctr;
                                else if (temp.Contains("ref/order"))
                                    refOrderCol = ctr;
                                else if (temp.Contains("vessel"))
                                    vesselCol = ctr;
                                else if (temp.Contains("ref"))
                                    refCol = ctr;
                                else if (temp.Contains("travelled"))
                                    travelledCol = ctr;
                                else if (temp.Contains("pax name"))
                                    paxNameCol = ctr;
                                else if (temp.Contains("route"))
                                    routeCol = ctr;
                                else if (temp.Contains("document"))
                                    documentCol = ctr;
                                else if (temp.Contains("air ticket"))
                                    airTicketCol = ctr;
                                else if (temp.Contains("class"))
                                    classCol = ctr;
                                else if (temp.Contains("net fare"))
                                    netFareCol = ctr;
                                else if (temp.Contains("taxes"))
                                    taxesCol = ctr;
                                else if (temp.Contains("amt dr"))
                                    amtDRCRCol = ctr;                                 
                            }//End of For Loop
                        }//End of IfReadColumn

                        if (startReading)
                        {
                            ticketNo = "";

                            counter++;

                            if (_mainWindow.loadingForm.lblProcessedCount.InvokeRequired)
                            {
                                _mainWindow.loadingForm.lblProcessedCount.Invoke(new MethodInvoker(delegate
                                { _mainWindow.loadingForm.lblProcessedCount.Text = counter.ToString(); }));
                            }

                            if (dr[airTicketCol].ToString() != "")
                            {
                                ticketNo = dr[airTicketCol].ToString();

                                //Get 10 digit ticket no
                                ticketNo = ticketNo.Substring(ticketNo.Length - 14, 10);                           

                                string invoiceNo = "";

                                var temp = posted.FirstOrDefault(r => r.TicketNo == ticketNo);

                                if (temp != null)
                                    invoiceNo = temp.InvoiceNo;

                                apAnalysis = new IASA_APAnalysis();

                                apAnalysis.Date = dr[dateCol].ToString();

                                apAnalysis.TransactionType = dr[transCol].ToString();

                                apAnalysis.Reference = dr[refCol].ToString();

                                apAnalysis.Vessel = dr[vesselCol].ToString();

                                apAnalysis.ReferenceOrder = dr[refOrderCol].ToString();

                                apAnalysis.TravelledDate = dr[travelledCol].ToString();

                                apAnalysis.PassengerName = dr[paxNameCol].ToString();

                                apAnalysis.Route = dr[routeCol].ToString();

                                apAnalysis.Document = dr[documentCol].ToString();

                                apAnalysis.AirTicket = dr[airTicketCol].ToString();

                                apAnalysis.BookingClass = dr[classCol].ToString();

                                apAnalysis.NetFare = dr[netFareCol].ToString() != "" ? decimal.Parse(dr[netFareCol].ToString()) : 0;

                                apAnalysis.Taxes = dr[taxesCol].ToString() != "" ? decimal.Parse(dr[taxesCol].ToString()) : 0;

                                apAnalysis.Amount_DR_CR = dr[amtDRCRCol].ToString() != "" ? decimal.Parse(dr[amtDRCRCol].ToString()) : 0;

                                apAnalysis.RecordNo = _recordNo;

                                apAnalysis.CreatedDate = DateTime.Now;

                                if (invoiceNo != "") //Already posted
                                {
                                    billed = new BilledTicket();

                                    //RecordLocator
                                    billed.RecordLocator = temp.RecordLocator;

                                    //TicketNo
                                    billed.TicketNo = ticketNo;

                                    billed.InvoiceDate = DateTime.Parse(temp.InvoiceDate.ToString()).ToShortDateString();

                                    billed.DateRange = dateRange;

                                    billed.AgentName = temp.BookingAgentName;

                                    billed.ClientName = temp.ClientName;

                                    billed.CurrencyCode = billed.ConvertedCurrencyCode = currency;

                                    billed.ConvertedAmount = dr[amtDRCRCol].ToString();

                                    billed.PassengerName = dr[paxNameCol].ToString();

                                    billed.DepartureCity = dr[routeCol].ToString();

                                    billed.Department = temp.Department;

                                    //Airline
                                    billed.Airline = "IASA";

                                    billed.RecordNo = _recordNo;

                                    billed.InvoiceNo = invoiceNo;

                                    //======AP ANALYSIS
                                    apAnalysis.IsPosted = "Y";

                                    apAnalysis.InvoiceNo = invoiceNo;

                                    var billedVM = new BilledTicketViewModel();

                                    if (billedVM.Save(billed)){ }//Successfully saved
                                }
                                else //not yet posted
                                {
                                    temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo);

                                    if (temp == null)
                                    {
                                        noRecord = new NoRecord();

                                        noRecord.Airline = "IASA";

                                        noRecord.DateRange = dateRange;

                                        noRecord.TicketNo = ticketNo;

                                        noRecord.RecordLocator = "";

                                        noRecord.RecordNo = _recordNo;

                                        noRecord.TicketNo = ticketNo;

                                        noRecord.CreatedDate = DateTime.Now;

                                        var noRecordVM = new NoRecordViewModel();

                                        //========AP ANALYSIS==========
                                        apAnalysis.IsPosted = "N";

                                        if (noRecordVM.Save(noRecord)){ }//Successfully Save                                        
                                    }
                                    else
                                    {

                                        temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo && r.TransactionType == 2);

                                        if (temp == null)//check if ticket is voided
                                        {

                                            temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo && r.TransactionType != 2);

                                            unbilled = new UnbilledTicket();

                                            unbilled.DateRange = dateRange;

                                            unbilled.AgentName = temp.BookingAgentName;

                                            if(unbilled.AgentCode != "")
                                            {

                                                var tempAgentCode = "";

                                                if (unbilled.AgentCode != null)
                                                    tempAgentCode = unbilled.AgentCode.ToUpper();

                                                var agentProfile = agent.FirstOrDefault(r => r.IASASafeName == tempAgentCode);

                                                if(agentProfile != null)
                                                {
                                                    unbilled.AgentName = agentProfile.Name;

                                                    unbilled.Department = agentProfile.Department;
                                                }
                                                else
                                                {
                                                    unbilled.AgentName = "";

                                                    unbilled.Department = "";
                                                }
                                            }

                                            //Record Locator
                                            unbilled.RecordLocator = temp.RecordLocator;

                                            unbilled.TicketNo = ticketNo;

                                            unbilled.CurrencyCode = unbilled.ConvertedCurrencyCode = currency;

                                            unbilled.ConvertedAmount = dr[amtDRCRCol].ToString();

                                            unbilled.PassengerName = dr[paxNameCol].ToString();

                                            unbilled.DepartureCity = dr[routeCol].ToString();

                                            unbilled.Department = temp.Department;

                                            //Airline
                                            unbilled.Airline = "IASA";

                                            unbilled.RecordNo = _recordNo;

                                            var unbilledVM = new UnbilledTicketViewModel();

                                            //==========AP ANALYSIS==========
                                            apAnalysis.IsPosted = "N";

                                            if (unbilledVM.Save(unbilled)){ }//Successfully saved                                          
                                        }
                                    }
                                }
                                var apVM = new IASAAPAnalysisViewModel();

                                if (apVM.Save(apAnalysis)) { }//Successfully saved
                            }
                            else// no record locator
                            {
                                _mainWindow.CloseProgress();

                                connection.Close();

                                break;
                            }
                        }

                        if (dr[0].ToString() == "Date")
                        {
                            startReading = true;

                            startReadColumn = false;
                        }
                        else if (dr[0].ToString().Contains("Account"))
                        {
                            startReadColumn = true;

                            for(int ctr = 0; ctr < dr.FieldCount; ctr++)
                            {
                                if (dr[ctr].ToString().ToLower().Contains("currency"))
                                {
                                    currency = dr[ctr + 1].ToString();

                                    break;
                                }
                            }//end of for loop
                        }
                    }
                }
            }
        }
    }
}