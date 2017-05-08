using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class IATAViewModel : ViewModelBase<IATA>
    {
        public bool Save(IATA _iata)
        {
            try
            {
                Add(_iata);

                return true;
            }
            catch(Exception error){ return false; }
        }
    }
}
