using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class NoRecordViewModel : ViewModelBase<NoRecord>
    {
        public int Count (string recordNo)
        {
            return Find(r => r.RecordNo == recordNo).Count;
        }

        public bool Save(NoRecord _noRecord)
        {
            try
            {
                Add(_noRecord);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
