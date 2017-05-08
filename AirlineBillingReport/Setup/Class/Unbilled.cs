using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirlineBillingReportRepository.ViewModel;
using AirlineBillingReportRepository;

namespace AirlineBillingReport.Class
{


    public class Unbilled
    {
        public List<Invoice> GetAllUnbilledByDepartment(bool showUnbilled, bool showSubmitted, string department, DateTime? fromDate = null
            , DateTime? toDate = null)
        {
            List<Invoice> filteredTickets = new List<Invoice>();

            var agents = new AgentCodeViewModel().GetAll().Where(r => r.Department == department);

            var unbilledTickets = GetAllUnbilled(showUnbilled, showSubmitted, fromDate,toDate);

            var posted = new TravcomViewModel().GetAllPosted();

            var tempList = CheckIfPosted(unbilledTickets, posted, agents.ToList());

            return tempList.OrderByDescending(r => r.InvoiceDate).ToList();
        }

        public List<Invoice> ConvertNoRecordToInvoice(List<NoRecord> noRecords)
        {
            List<Invoice> returnList = new List<Invoice>();

            noRecords.ForEach(item =>
            {
                Invoice temp = new Invoice();

                temp.RecordType = "No Record";
                temp.RecordLocator = item.RecordLocator;
                temp.TicketNo = item.TicketNo;

                if (item.BookingAmount != "" && item.BookingAmount != null)
                    temp.GrossAmount = decimal.Parse(item.BookingAmount);
                else
                    temp.GrossAmount = 0;

                temp.Department = item.Department;
                temp.PassengerName = item.PassengerName;
                temp.Itinerary = item.Itinerary;
                temp.Currency = item.Currency;
                temp.BookingAgentName = item.AgentName;
                temp.BookingAgent = item.AgentCode;

                returnList.Add(temp);
            });

            return returnList;
        }

        public List<NoRecord> RemoveDuplicateInNoRecord(List<NoRecord> originalList)
        {
            List<NoRecord> returnList = new List<NoRecord>();

            var agentProfiles = new AirlineBillingReportEntities().AgentProfile.Where(r => r.ID != null);

            IEnumerable<Invoice> tempList;

            var unposted = new TravcomViewModel().GetAllUnposted();
            var posted = new TravcomViewModel().GetAllPosted();
            var voided = new TravcomViewModel().GetUnbilledTickets(true, true);
            

            tempList = unposted.Concat(voided);
            tempList = tempList.Concat(posted);

            originalList.ForEach(item =>
            {
                var temp = tempList.FirstOrDefault(r => r.TicketNo == item.TicketNo);

                if (temp == null)
                {
                    var tempProfile = agentProfiles.FirstOrDefault(r => r.CebuPacific == item.AgentCode);

                    if (tempProfile != null)
                        item.AgentName = tempProfile.Name;

                    returnList.Add(item);
                }
            });

            return returnList;
        }

        public List<Invoice> GetUnbilledViaAgent(List<Invoice> OriginalList, List<AgentCode> agents)
        {
            List<Invoice> returnList = new List<Invoice>();

            //var posted = new TravcomViewModel().GetAllPosted();

            //var filtered = CheckIfPosted(OriginalList, posted);

            OriginalList.ForEach(item =>
            {
                var temp = agents.FirstOrDefault(r => (r.TravCom1 == item.BookingAgent || r.TravCom2 == item.BookingAgent
                    || r.TravCom3 == item.BookingAgent || r.TravCom4 == item.BookingAgent || r.TravCom5 == item.BookingAgent) && r.Tick == true);

                if (temp != null)
                {
                    item.Department = temp.Department;

                    returnList.Add(item);
                }
            });

            return returnList.OrderByDescending(r => r.InvoiceDate).ToList();
        }

        public List<Invoice> GetAllUnbilledByDepartment(bool showUnbilled, bool showSubmitted, string department , List<AgentProfile> agents)
        {
            List<Invoice> filteredTickets = new List<Invoice>();

            var unbilledTickets = GetAllUnbilled(showUnbilled, showSubmitted);

            var posted = new TravcomViewModel().GetAllPosted();

            filteredTickets = CheckIfPosted(unbilledTickets, posted, agents);

            //unbilledTickets.ForEach(item =>
            //{
            //    var temp = agents.FirstOrDefault(r => (r.TravCom1 == item.BookingAgent || r.TravCom2 == item.BookingAgent
            //        || r.TravCom3 == item.BookingAgent || r.TravCom4 == item.BookingAgent || r.TravCom5 == item.BookingAgent) && r.Tick == true);

            //    if (temp != null)
            //    {
            //        item.Department = temp.Department;

            //        filteredTickets.Add(item);
            //    }
            //});

            return filteredTickets.OrderByDescending(r => r.InvoiceDate).ToList();
        }

        public List<Invoice> GetAllUnbilled(bool showUnbilled, bool showSubmitted,DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<Invoice> filteredTickets = new List<Invoice>();

            var unbilledTickets = new TravcomViewModel().GetUnbilledTickets(showUnbilled, showSubmitted,fromDate, toDate)
                .Where(r=>r.TransactionType != 2 && r.GrossAmount > 0).ToList();

            return unbilledTickets;
        }

        public List<Invoice> GetAllUnbilledViaAgentCode(bool showUnbilled, bool showSubmitted ,List<Invoice> unbilledTickets, List<Invoice> posted, string agentCode1, string agentCode2, string agentCode3, string agentCode4, string agentCode5,
            DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<Invoice> filteredTickets = new List<Invoice>();

            //var unbilledTickets = new TravcomViewModel().GetUnbilledTickets(fromDate, toDate).Where
            //    (r => (r.BookingAgent == agentCode1 || r.BookingAgent == agentCode2 || r.BookingAgent == agentCode3
            //    || r.BookingAgent == agentCode4 || r.BookingAgent == agentCode5) && r.TransactionType != 2 && r.GrossAmount > 0).ToList();

            unbilledTickets = unbilledTickets.Where(r => r.TransactionDate >= fromDate && r.TransactionDate <= toDate && (r.BookingAgent == agentCode1 ||
                r.BookingAgent == agentCode2 || r.BookingAgent == agentCode3 || r.BookingAgent == agentCode4 || r.BookingAgent == agentCode5)
                && r.TransactionType != 2 && r.GrossAmount >= 0).ToList();

            if(!showSubmitted)
            {
                unbilledTickets = unbilledTickets.Where(r => !r.FreeFields.Contains("AEFUR")).ToList();
            }

            if (!showUnbilled)
            {
                unbilledTickets = unbilledTickets.Where(r => r.FreeFields.Contains("AEFUR")).ToList();
            }

            //Check if posted in TravCom
            filteredTickets = CheckIfPosted(unbilledTickets, posted);          
            
            return filteredTickets;
        }         
        
        private List<Invoice> CheckIfPosted(List<Invoice>unbilledTickets, List<Invoice> posted, List<AgentProfile> agentCodes = null)
        {            
            List<Invoice> filteredTicket = new List<Invoice> ();

            unbilledTickets.ForEach(item =>
            {
                var temp = posted.FirstOrDefault(r => r.TicketNo == item.TicketNo);

                if (temp == null)
                {
                    if (agentCodes != null)
                    {
                        var tempAgent = agentCodes.FirstOrDefault(r => r.TravCom1 == item.BookingAgent || r.TravCom2 == item.BookingAgent || r.TravCom3 == item.BookingAgent
                            || r.TravCom4 == item.BookingAgent || r.TravCom5 == item.BookingAgent);

                        if (tempAgent != null)
                        {
                            item.BookingAgentName = tempAgent.Name;

                            item.Department = tempAgent.Department;
                        }
                    }

                    filteredTicket.Add(item);
                }
            });

            return filteredTicket.ToList();
        }

        public List<NoRecord> GetAllNoRecord()
        {
            using (var db = new AirlineBillingReportEntities())
            {
                var noRecords = db.NoRecord.Where(r=>r.TicketNo != "" && r.Airline == "Cebu Pacific")
                    .GroupBy(x => new { x.TicketNo, x.RecordLocator }).Select(x => x.FirstOrDefault());

                return noRecords.ToList();
            }
        }

        public List<NoRecord> GetNoRecordViaDepartment(List<Invoice> unbilledTickets, List<Invoice> posted, string department)
        {
            using (var db = new AirlineBillingReportEntities())
            {
                List<NoRecord> returnList = new List<NoRecord>();

                var agents = new AgentCodeViewModel().GetAll().Where(r => r.Department.ToLower() == department.ToLower());

                var noRecords = GetAllNoRecord();

                noRecords.ForEach(item =>
                {
                    bool isNoRecord = true;

                    var temp = agents.FirstOrDefault(r => ( r.CebuPacific == item.AgentCode || r.PartnerAgent == item.AgentCode )
                        && item.Department == department);


                    var temp1 = unbilledTickets.FirstOrDefault(r => r.TicketNo.Contains(item.TicketNo.Substring(2, 7))
                        && r.RecordLocator == item.RecordLocator);

                    if (temp1 != null)
                        isNoRecord = false;

                    var temp2 = posted.FirstOrDefault(r => r.TicketNo.Contains(item.TicketNo.Substring(2, 7))
                        && r.RecordLocator == item.RecordLocator);

                    if (temp2 != null)
                        isNoRecord = false;

                    if (isNoRecord)
                    {
                        var tempAgent = agents.FirstOrDefault(r => r.CebuPacific == item.AgentCode || r.PartnerAgent == item.AgentCode);

                        if(tempAgent != null)
                            returnList.Add(item);
                    }
                    
                });

                return returnList;
            }
        }

        public List<NoRecord> GetNoRecordViaAgentCode(List<Invoice> unbilledTickets, List<Invoice> posted, string cebuPac, string partnerAgent, string pal, string airAsia,
            string iasa, string iata, string clientAgent)
        {
            using (var db = new AirlineBillingReportEntities())
            {
                List<NoRecord> returnList = new List<NoRecord>();

                var noRecords = db.NoRecord.Where(r => r.AgentCode != "" && (r.AgentCode == cebuPac 
                    || r.AgentCode == partnerAgent || r.AgentCode == clientAgent) && r.Airline == "Cebu Pacific").ToList();

                //Remove Duplicate Entries
                noRecords = noRecords.GroupBy(x => new { x.TicketNo, x.RecordLocator}).Select(x => x.FirstOrDefault()).ToList();

                //Validate in posted and unposted
                noRecords.ForEach(item =>
                {
                    bool isNoRecord = true;

                    if (item.TicketNo != "")
                    {
                        var temp = unbilledTickets.FirstOrDefault(r => r.TicketNo.Contains(item.TicketNo));

                        if (temp != null)
                            isNoRecord = false;

                        var temp1 = posted.FirstOrDefault(r => r.TicketNo.Contains(item.TicketNo));

                        if (temp1 != null)
                            isNoRecord = false;

                        if (isNoRecord)
                            returnList.Add(item);
                    }
                });

                return returnList;
            }
        }

        private List<Invoice> CheckIfPostedViaAirline(List<Invoice> unbilledTickets)
        {
            var posted = new BilledTicketViewModel().GetAll();

            List<Invoice> filteredTicket = new List<Invoice>();

            BilledTicket temp = new BilledTicket();

            unbilledTickets.ForEach(item =>
            {
                temp = posted.FirstOrDefault(r => r.TicketNo == item.TicketNo);

                if(temp == null)
                {
                    filteredTicket.Add(item);
                }
            });

            return filteredTicket;
        }

        public List<AEFURNoRecord> GetAllNoRecordViaSQL()
        {
            var db = new AirlineBillingReportEntities();

            var noRecords = db.AEFURNoRecord.Where(r => r.TicketNo != "" && r.TicketNo != null).ToList();

            return noRecords;
        }

        public List<AEFURNoRecord> GetAllNoRecordViaSQLPerDepartment(string department)
        {
            var db = new AirlineBillingReportEntities();

            var noRecords = db.AEFURNoRecord.Where(r => (r.Department == department || r.Department == null || r.Department == "") && (r.TicketNo != "" && r.TicketNo != null)).ToList();

            return noRecords;
        }

        public List<AEFURNoRecord> GetAllNoRecordViaSQLPerTC(string cebuPac, string Pal, string iata, string iasa, string airAsia)
        {
            var db = new AirlineBillingReportEntities();

            var noRecords = db.AEFURNoRecord.Where(r => (r.AgentCode == cebuPac || r.AgentCode == Pal || r.AgentCode == iata || r.AgentCode == iasa 
                || r.AgentCode == airAsia) && (r.AgentCode != "" && r.AgentCode != null &&r.TicketNo != "" && r.TicketNo != null)).ToList();

            return noRecords;
        }
    }
}
