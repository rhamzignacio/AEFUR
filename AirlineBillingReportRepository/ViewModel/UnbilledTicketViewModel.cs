using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class UnbilledTicketViewModel : ViewModelBase<UnbilledTicket>
    {
        public int Count(string recordNo)
        {
            return Find(r => r.RecordNo == recordNo).Count;
        }

        public bool Save(UnbilledTicket _unbilledTicket)
        {
            try
            {
                _unbilledTicket.CreatedDate = DateTime.Now;

                _unbilledTicket.RecordLocator = _unbilledTicket.RecordLocator.Replace(" ", "");

                //Record locator or Ticket No
                if (_unbilledTicket.RecordLocator.Length == 6 || _unbilledTicket.RecordLocator.Length == 10)
                    Add(_unbilledTicket);
                else
                    return true;

                return true;
            }
            catch(Exception error)
            {
                var errorVM = new ErrorLogViewModel();

                ErrorLog newErrorLog = new ErrorLog();

                newErrorLog.Airline = _unbilledTicket.Airline;

                newErrorLog.Type = "Unbilled";

                newErrorLog.RecordLocator = _unbilledTicket.RecordLocator;

                newErrorLog.Message = error.Message;

                newErrorLog.RecordNo = _unbilledTicket.RecordNo;

                errorVM.Save(newErrorLog);

                return false;
            }
        }
    }
}
