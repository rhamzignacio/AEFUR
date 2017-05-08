using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class BilledTicketViewModel : ViewModelBase<BilledTicket>
    {
        public List<BilledTicket> GetAll()
        {
            return Find();
        }

        public int Count(string recordNo)
        {
            return Find(r => r.RecordNo == recordNo).Count;
        }

        public bool Save(BilledTicket _billedTicket)
        {
            try
            {
                _billedTicket.CreatedDate = DateTime.Now;

                _billedTicket.RecordLocator = _billedTicket.RecordLocator.Replace(" ", "");

                //Record locator or Ticket No
                if (_billedTicket.RecordLocator.Length == 6 || _billedTicket.RecordLocator.Length == 10)
                    Add(_billedTicket);
                else
                    return true;

                return true;
            }
            catch(Exception error)
            {
                var errorVM = new ErrorLogViewModel();

                ErrorLog newErrorLog = new ErrorLog();

                newErrorLog.Airline = _billedTicket.Airline;

                newErrorLog.RecordNo = _billedTicket.RecordNo;

                newErrorLog.Type = "Unbilled";

                newErrorLog.RecordLocator = _billedTicket.RecordLocator;

                newErrorLog.Message = error.Message;

                errorVM.Save(newErrorLog);

                return false;
            }
        }
    }
}
