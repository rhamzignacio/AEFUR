using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class ErrorLogViewModel : ViewModelBase<ErrorLog>
    {
        public void Save(ErrorLog _errorLog)
        {

            _errorLog.Date = DateTime.Now;

            Add(_errorLog);
        }
    }
}
