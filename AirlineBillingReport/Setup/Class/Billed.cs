using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirlineBillingReportRepository.ViewModel;
using AirlineBillingReportRepository;

namespace AirlineBillingReport.Class
{
    public class Billed
    {
        public List<Invoice> GetAllBilledByDepartment(string department)
        {
            List<Invoice> filteredTickets = new List<Invoice>();

            var agents = new AgentCodeViewModel().GetAll().Where(r => r.Department == department);

            var billedTickets = new TravcomViewModel().GetBilledTickets();

            billedTickets.ForEach(item =>
            {
                var temp = agents.FirstOrDefault(r => r.TravCom1 == item.BookingAgent || r.TravCom2 == item.BookingAgent
                    || r.TravCom3 == item.BookingAgent || r.TravCom4 == item.BookingAgent || r.TravCom5 == item.BookingAgent);

                if(temp != null)
                {
                    item.Department = temp.Department;

                    filteredTickets.Add(item);
                }
            });

            return filteredTickets.OrderByDescending(r => r.InvoiceDate).ToList();
        }

        public List<Invoice> GetBilledViaAgent(List<Invoice> OriginalList, List<AgentCode> agents)
        {
            List<Invoice> returnList = new List<Invoice>();

            OriginalList.ForEach(item =>
            {
                var temp = agents.FirstOrDefault(r => r.TravCom1 == item.BookingAgent || r.TravCom2 == item.BookingAgent
                    || r.TravCom3 == item.BookingAgent || r.TravCom4 == item.BookingAgent || r.TravCom5 == item.BookingAgent);

                if(temp != null)
                {
                    item.Department = temp.Department;

                    returnList.Add(item);
                }
            });

            return returnList.OrderByDescending(r => r.InvoiceDate).ToList(); ;
        }

        public List<Invoice> GetAllBilledByDepartment(string department, List<AgentCode> agents)
        {
            List<Invoice> filteredTickets = new List<Invoice>();

            var billedTickets = new TravcomViewModel().GetBilledTickets();

            billedTickets.ForEach(item =>
            {
                var temp = agents.FirstOrDefault(r => (r.TravCom1 == item.BookingAgent || r.TravCom2 == item.BookingAgent
                    || r.TravCom3 == item.BookingAgent || r.TravCom4 == item.BookingAgent || r.TravCom5 == item.BookingAgent) && r.Tick == true);

                if (temp != null)
                {
                    item.Department = temp.Department;

                    filteredTickets.Add(item);
                }
            });

            return filteredTickets.OrderByDescending(r => r.InvoiceDate).ToList();
        }

        public List<Invoice> GetAllBilledViaAgentCode(string agentCode1, string agentCode2, string agentCode3, string agentCode4, string agentCode5)
        { 
            return new TravcomViewModel().GetBilledTickets().Where
                (r => r.BookingAgent == agentCode1 || r.BookingAgent == agentCode2 || r.BookingAgent == agentCode3
                || r.BookingAgent == agentCode4 || r.BookingAgent == agentCode5).ToList();
        }

        public List<Invoice> GetAllBilled(DateTime? toDate = null, DateTime? fromDate = null)
        {
            return new TravcomViewModel().GetBilledTickets(toDate, fromDate);
        }
    }
}