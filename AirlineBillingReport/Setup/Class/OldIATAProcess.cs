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
    public class OldIATAProcess
    {
        public void Process(string _recordNo, string _path, MainWindow _mainWindow, List<Invoice> posted, List<Invoice> voided
            , List<AirlineBillingReportRepository.AgentProfile> agent)
        {
            _mainWindow.StartProgress("Processing IATA. . .");
            //configuration for IATA
            var config = new AirlineConfigurationViewModel().GetSelected("IATA");

            //Excel connection string
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _path + ";" +
                 "Extended Properties='Excel 12.0 Xml;HDR=No;'";

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();

                //Excel query string
                OleDbCommand command = new OleDbCommand("select * from [" + config.TabName + "$]", connection);

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    bool startReading = false, startDebit = false;

                    AirlineConfigurationViewModel airlineConfigVM = new AirlineConfigurationViewModel();

                    int dif = airlineConfigVM.ConvertColumnToInteger(config.StartColumn);

                    int counter = 0;

                    BilledTicket billed;

                    IATA iata;

                    UnbilledTicket unbilled;

                    NoRecord noRecord;

                    var iataVM = new IATAViewModel();

                    var travCom = new TravcomViewModel();

                    string recordLocator = "";

                    string ticketNo = "";

                    string currency = "PHP";

                    int transCol = 0;


                    var travcomCurrency = new TravcomViewModel().GetCurrency("USD"); //get USD rate from Client Magic                 

                    while (dr.Read())
                    {

                        for (int x = 0; x < dr.FieldCount; x++)
                        {
                            if (dr[x].ToString() == "Transaction" && x != 0)
                                transCol = x;
                        }

                        if (dr[0].ToString().Contains("Billing Period:"))
                        {
                            string row = dr[0].ToString();

                            string dateRange = row.Substring(row.IndexOf("(") + 1, row.IndexOf(")") - 2);

                            string reference = row.Substring(row.Length - 17, 8);

                            using (var db = new AirlineBillingReportEntities())
                            {
                                var record = db.RecordNoStorage.FirstOrDefault(r => r.RecordNo == _recordNo);

                                record.IATADateRange = dateRange;

                                record.IATAReference = reference;

                                db.Entry(record).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();
                            }
                        }

                        ticketNo = "";

                        recordLocator = "";

                        if (dr[0].ToString().Contains("END OF CURRENCY  USD"))
                        {
                            _mainWindow.CloseProgress();

                            connection.Close();

                            break;
                        }
                        else if (dr[0].ToString().Contains("END OF CURRENCY  PHP"))
                        {
                            currency = "USD";
                        }

                        if (dr[0].ToString().Contains("DEBIT MEMOS TOTAL"))
                        {
                            startDebit = false;
                        }

                        //Debit Memo
                        if (startDebit)
                        {
                            if (!dr[1].ToString().Contains("RTDN") &&
                                !dr[0].ToString().Contains("RTDN") && !dr[0].ToString().Contains("DEBIT MEMOS"))
                            {
                                iata = new IATA();

                                iata.RecordNo = _recordNo;
                                //USD Amount
                                if (currency != "USD")
                                {

                                    iata.CreatedDate = DateTime.Now;

                                    iata.TicketNo = dr[airlineConfigVM.ConvertColumnToInteger("C") - dif].ToString();

                                    iata.AirlineCode = dr[0].ToString();

                                    iata.Remarks = dr[1].ToString();

                                    iata.IssueDate = dr[airlineConfigVM.ConvertColumnToInteger("D") - dif].ToString();

                                    if (dr[transCol].ToString() != "")
                                    {
                                        double phpValue = double.Parse(dr[transCol].ToString().Replace("(", "").Replace(")", ""));

                                        if (phpValue > 0)
                                        {
                                            double usdValue = 0;

                                            usdValue = phpValue / travcomCurrency.ExchangeRate;

                                            iata.USDAmount = decimal.Round(decimal.Parse(usdValue.ToString()), 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            iata.USDAmount = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    string temp = dr[airlineConfigVM.ConvertColumnToInteger("A") - dif].ToString();
                                    iata.AirlineCode = temp.Substring(0, 3);

                                    iata.Remarks = temp.Substring(3, temp.Length - 3).Replace(" ", "");

                                    iata.CreatedDate = DateTime.Now;

                                    iata.TicketNo = dr[airlineConfigVM.ConvertColumnToInteger("C") - dif].ToString();

                                    iata.AirlineCode = dr[0].ToString().Substring(0, 3);

                                    string tempstring = dr[0].ToString().Replace(" ", "");

                                    iata.Remarks = dr[0].ToString().Replace(" ", "").Substring(3, tempstring.Length - 3);

                                    iata.IssueDate = dr[airlineConfigVM.ConvertColumnToInteger("D") - dif].ToString();

                                    iata.USDAmount = decimal.Parse(dr[airlineConfigVM.ConvertColumnToInteger("AT") - dif].ToString());
                                }

                                iata.IsPosted = "";

                                iataVM = new IATAViewModel();

                                if (iata.USDAmount.ToString() != "")
                                    if (iataVM.Save(iata))
                                    {
                                        //Successfully saved
                                    }
                            }
                        }

                        if (startReading)
                        {
                            if (dr[airlineConfigVM.ConvertColumnToInteger("A") - dif].ToString().Contains("*** DEBIT MEMOS")
                                && !dr[airlineConfigVM.ConvertColumnToInteger("A") - dif].ToString().Contains("TOTAL"))
                            {
                                startDebit = true;
                            }

                            //REFUNDS for IATA Table
                            if (dr[airlineConfigVM.ConvertColumnToInteger("A") - dif].ToString().Contains("RFND"))
                            {
                                iata = new IATA();

                                iata.RecordNo = _recordNo;

                                iata.CreatedDate = DateTime.Now;

                                iata.TicketNo = dr[airlineConfigVM.ConvertColumnToInteger("C") - dif].ToString();

                                if (iata.TicketNo != "")
                                {

                                    iata.Status = "";

                                    iata.Remarks = "RFND";

                                    iata.InvoiceStatus = "";

                                    iata.IssueDate = dr[airlineConfigVM.ConvertColumnToInteger("D") - dif].ToString();

                                    var temp = posted.FirstOrDefault(r => r.TicketNo.Contains(ticketNo));

                                    if (temp == null)
                                        temp = voided.FirstOrDefault(r => r.TicketNo.Contains(ticketNo));

                                    if (temp != null)
                                    {
                                        iata.RecordLocator = temp.RecordLocator;

                                        iata.ClientName = temp.ClientName;

                                        iata.PassengerName = temp.PassengerName;

                                        iata.InvoiceNo = temp.InvoiceNo;

                                       // iata.InvoiceDate = temp.InvoiceDate;

                                        iata.AgentName = temp.BookingAgentName;

                                        iata.Itinerary = temp.Itinerary;

                                        iata.AirlineCode = dr[0].ToString().Substring(0, 3);
                                    }
                                    else //No Record in Client Magic
                                    {
                                        iata.RecordLocator = iata.ClientName = iata.PassengerName =
                                            iata.InvoiceNo = iata.AgentName = iata.Itinerary = "";

                                        iata.AirlineCode = dr[0].ToString().Substring(0, 3);
                                    }

                                    if (currency != "USD")
                                    {

                                        double phpValue = double.Parse(dr[transCol].ToString().Replace("(", "").Replace(")", ""));

                                        if (phpValue != 0)
                                        {
                                            double usdValue = 0;

                                            usdValue = phpValue / travcomCurrency.ExchangeRate;

                                            iata.USDAmount = decimal.Round(decimal.Parse(usdValue.ToString()), 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            iata.USDAmount = 0;
                                        }
                                    }
                                    else
                                    {
                                        iata.USDAmount = decimal.Round(decimal.Parse(dr[transCol].ToString()), 2, MidpointRounding.AwayFromZero);
                                    }

                                    iata.IsPosted = "N";

                                    iataVM = new IATAViewModel();

                                    if (iataVM.Save(iata))
                                    {
                                        //Successfully Saved
                                    }
                                }
                            }
                            //End of REFUNDS for IATA Table

                            //To check if valid column
                            if (dr[1].ToString() == "TKTT") //Column B
                            {

                                if (config.TicketNo != "")
                                {
                                    ticketNo = dr[airlineConfigVM.ConvertColumnToInteger(config.TicketNo) - dif].ToString();
                                }

                                if (ticketNo != "")
                                {
                                    counter++;

                                    if (_mainWindow.loadingForm.lblProcessedCount.InvokeRequired)
                                    {
                                        _mainWindow.loadingForm.lblProcessedCount.Invoke(new MethodInvoker(delegate
                                        { _mainWindow.loadingForm.lblProcessedCount.Text = counter.ToString(); }));
                                    }

                                    //Checks if record is already posted
                                    var temp = posted.FirstOrDefault(r => r.TicketNo == ticketNo);

                                    //Already posted
                                    if (temp != null)
                                    {
                                        billed = new BilledTicket();

                                        iata = new IATA();

                                        iata.IsPosted = "Y";

                                        iata.CreatedDate = DateTime.Now;

                                        billed.TicketNo = ticketNo;
                                        iata.TicketNo = ticketNo;

                                        billed.RecordLocator = temp.RecordLocator;
                                        iata.RecordLocator = temp.RecordLocator;

                                        var agentInfo = agent.FirstOrDefault(r => r.TravCom1 == temp.BookingAgent || r.TravCom2 == temp.BookingAgent
                                            || r.TravCom3 == temp.BookingAgent || r.TravCom4 == temp.BookingAgent || r.TravCom5 == temp.BookingAgent);

                                        if (agentInfo != null)
                                        {
                                            billed.AgentName = agentInfo.Name;
                                            iata.AgentName = agentInfo.Name;

                                            billed.AgentCode = agentInfo.IATA;

                                            billed.Department = agentInfo.Department;
                                        }

                                        billed.InvoiceNo = temp.InvoiceNo;
                                        iata.InvoiceNo = temp.InvoiceNo;

                                        billed.InvoiceDate = temp.InvoiceDate.ToString();
                                        //iata.InvoiceDate = temp.InvoiceDate;

                                        if (config.AirlineCode != "")
                                            billed.AirlineCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AirlineCode) - dif].ToString();
                                        else
                                            billed.AirlineCode = "N/A";


                                        if (config.ConvertedAmountCol != "")
                                        {
                                            var converted = dr[airlineConfigVM.ConvertColumnToInteger(config.ConvertedAmountCol) - dif].ToString(); ;

                                            if (currency == "PHP")
                                                billed.ConvertedAmount = converted;
                                            else
                                                billed.CollectedAmount = converted;
                                        }

                                        billed.PassengerName = temp.PassengerName;

                                        billed.Airline = "IATA";

                                        billed.RecordNo = _recordNo;
                                        iata.RecordNo = _recordNo;

                                        billed.InvoiceNo = temp.InvoiceNo;

                                        billed.InvoiceDate = DateTime.Parse(temp.InvoiceDate.ToString()).ToShortDateString();

                                        //Client Name
                                        billed.ClientName = temp.ClientName;
                                        iata.ClientName = temp.ClientName;

                                        //Date Range - Issuance Date
                                        if (config.PaymentDate != "")
                                        {
                                            billed.DateRange = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentDate) - dif].ToString();
                                            iata.IssueDate = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentDate) - dif].ToString();
                                        }

                                        //<For IATA Table>
                                        iata.PassengerName = temp.PassengerName;

                                        iata.Itinerary = temp.Itinerary;

                                        iata.InvoiceStatus = "Posted";

                                        iata.Remarks = "TKTT";

                                        if (config.AirlineCode != "")
                                            iata.AirlineCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AirlineCode) - dif].ToString();

                                        if (config.AuthorizationStatusCol != "")//Status for IATA Table
                                            iata.Status = dr[airlineConfigVM.ConvertColumnToInteger(config.AuthorizationStatusCol) - dif].ToString();

                                        //USD Amount
                                        if (config.CollectedAmountCol != "")
                                        {

                                            if (currency != "USD")
                                            {

                                                double phpValue = double.Parse(dr[transCol].ToString().Replace("(", "").Replace(")", ""));

                                                if (phpValue > 0)
                                                {
                                                    double usdValue = 0;

                                                    usdValue = phpValue / travcomCurrency.ExchangeRate;

                                                    iata.USDAmount = decimal.Round(decimal.Parse(usdValue.ToString()), 2, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                {
                                                    iata.USDAmount = 0;
                                                }
                                            }
                                            else
                                            {
                                                iata.USDAmount = decimal.Round(decimal.Parse(dr[transCol].ToString()), 2, MidpointRounding.AwayFromZero);
                                            }
                                        }
                                        else
                                        {
                                            iata.USDAmount = 0;
                                        }
                                        //</For IATA Table>
                                        iataVM = new IATAViewModel();

                                        if (iataVM.Save(iata))
                                        {
                                            //Successfully saved
                                        }

                                        var billedVM = new BilledTicketViewModel();

                                        if (billedVM.Save(billed))
                                        {
                                            //Successfully saved
                                        }
                                    }
                                    else //unbilled ticket
                                    {
                                        temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo);

                                        if (temp == null)
                                        {
                                            noRecord = new NoRecord();
                                            iata = new IATA();

                                            iata.CreatedDate = DateTime.Now;

                                            noRecord.Airline = "IATA";

                                            noRecord.RecordNo = _recordNo;
                                            iata.RecordNo = _recordNo;

                                            noRecord.RecordLocator = recordLocator;
                                            iata.RecordLocator = recordLocator;

                                            noRecord.TicketNo = ticketNo;
                                            iata.TicketNo = ticketNo;

                                            iata.IsPosted = "N";

                                            noRecord.CreatedDate = DateTime.Now;

                                            //Date Range
                                            if (config.PaymentDate != "")
                                            {
                                                noRecord.DateRange = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentDate) - dif].ToString();
                                                iata.IssueDate = noRecord.DateRange;
                                            }

                                            if (config.AirlineCode != "")
                                                iata.AirlineCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AirlineCode) - dif].ToString();
                                            else
                                                iata.AirlineCode = "";

                                            iata.InvoiceNo = iata.ClientName =
                                            iata.AgentName = iata.PassengerName = iata.Itinerary = "";

                                            iata.InvoiceStatus = "No record";
                                            iata.Remarks = "TKTT";

                                            if (config.CollectedAmountCol != "")
                                            {

                                                if (currency != "USD")
                                                {
                                                    double phpValue = double.Parse(dr[transCol].ToString().Replace("(", "").Replace(")", ""));

                                                    if (phpValue > 0)
                                                    {
                                                        double usdValue = 0;

                                                        usdValue = phpValue / travcomCurrency.ExchangeRate;

                                                        iata.USDAmount = decimal.Round(decimal.Parse(usdValue.ToString()), 2, MidpointRounding.AwayFromZero);
                                                    }
                                                    else
                                                    {
                                                        iata.USDAmount = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    iata.USDAmount = decimal.Round(decimal.Parse(dr[transCol].ToString()), 2, MidpointRounding.AwayFromZero);
                                                }
                                            }
                                            else
                                                iata.USDAmount = 0;

                                            iataVM = new IATAViewModel();
                                            if (iataVM.Save(iata))
                                            {
                                                //Successfully saved
                                            }

                                            var noRecordVM = new NoRecordViewModel();

                                            if (noRecordVM.Save(noRecord))
                                            {
                                                //Successfully Save
                                            }
                                        }
                                        else
                                        {
                                            //Check if not Voided
                                            temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo && r.TransactionType == 2);

                                            if (temp == null)
                                            {
                                                temp = voided.FirstOrDefault(r => r.TicketNo == ticketNo);

                                                unbilled = new UnbilledTicket();
                                                iata = new IATA();

                                                iata.IsPosted = "N";

                                                iata.CreatedDate = DateTime.Now;

                                                unbilled.TicketNo = ticketNo;
                                                iata.TicketNo = ticketNo;

                                                unbilled.RecordLocator = temp.RecordLocator;
                                                iata.RecordLocator = temp.RecordLocator;

                                                var agentInfo = agent.FirstOrDefault(r => r.TravCom1 == temp.BookingAgent ||
                                                    r.TravCom2 == temp.BookingAgent || r.TravCom3 == temp.BookingAgent || r.TravCom4 == temp.BookingAgent
                                                    || r.TravCom5 == temp.BookingAgent);

                                                if (agentInfo != null)
                                                {
                                                    unbilled.AgentName = agentInfo.Name;
                                                    iata.AgentName = agentInfo.Name;

                                                    unbilled.AgentCode = agentInfo.IATA;

                                                    unbilled.Department = agentInfo.Department;
                                                }
                                                else
                                                    iata.AgentName = "";

                                                if (config.AirlineCode != "")
                                                {
                                                    unbilled.AirlineCode = dr[airlineConfigVM.ConvertColumnToInteger(config.AirlineCode) - dif].ToString();
                                                    iata.AirlineCode = unbilled.AirlineCode;
                                                }
                                                else
                                                {
                                                    unbilled.AirlineCode = "N/A";
                                                    iata.AirlineCode = "";
                                                }


                                                if (config.ConvertedAmountCol != "")
                                                {
                                                    var converted = dr[airlineConfigVM.ConvertColumnToInteger(config.ConvertedAmountCol) - dif].ToString(); ;

                                                    if (currency == "PHP")
                                                        unbilled.ConvertedAmount = converted;
                                                    else
                                                        unbilled.CollectedAmount = converted;
                                                }

                                                unbilled.PassengerName = temp.PassengerName;
                                                iata.PassengerName = temp.PassengerName;

                                                unbilled.Airline = "IATA";

                                                unbilled.RecordNo = _recordNo;
                                                iata.RecordNo = _recordNo;

                                                if (currency != "USD")
                                                {
                                                    double phpValue = double.Parse(dr[transCol].ToString().Replace("(", "").Replace(")", ""));

                                                    if (phpValue > 0)
                                                    {
                                                        double usdValue = 0;

                                                        usdValue = phpValue / travcomCurrency.ExchangeRate;

                                                        iata.USDAmount = decimal.Round(decimal.Parse(usdValue.ToString()), 2, MidpointRounding.AwayFromZero);
                                                    }
                                                    else
                                                    {
                                                        iata.USDAmount = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    iata.USDAmount = decimal.Round(decimal.Parse(dr[transCol].ToString()), 2, MidpointRounding.AwayFromZero);
                                                }


                                                if (config.AuthorizationStatusCol != "")//Status for IATA Table
                                                    iata.Status = dr[airlineConfigVM.ConvertColumnToInteger(config.AuthorizationStatusCol) - dif].ToString();

                                                //Client Name
                                                unbilled.ClientName = temp.ClientName;
                                                iata.ClientName = temp.ClientName;

                                                //Date Range
                                                if (config.PaymentDate != "")
                                                {
                                                    unbilled.DateRange = dr[airlineConfigVM.ConvertColumnToInteger(config.PaymentDate) - dif].ToString();
                                                    iata.IssueDate = unbilled.DateRange;
                                                }

                                                var unbilledVM = new UnbilledTicketViewModel();

                                                iata.Remarks = "TKTT";

                                                iataVM = new IATAViewModel();
                                                if (iataVM.Save(iata))
                                                {
                                                    //Successfully saved
                                                }

                                                if (unbilledVM.Save(unbilled))
                                                {
                                                    //Successfully saved
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }//end of While
                        if (dr[0].ToString() == "*** ISSUES")
                        {
                            startReading = true;
                        }
                    }
                }
            }
        }
    }
}
