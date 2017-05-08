using AirlineBillingReportRepository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository
{
    public class IASAAPAnalysisViewModel : ViewModelBase<IASA_APAnalysis>
    {
        public bool Save(IASA_APAnalysis _APAnalaysis)
        {
            try
            {
                Add(_APAnalaysis);

                return true;
            }
            catch(Exception Error) { return false; }
        }
    }
}
