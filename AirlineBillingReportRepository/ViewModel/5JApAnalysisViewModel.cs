using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class _5JApAnalysisViewModel : ViewModelBase<C5J_APAnalysis>
    {
        public bool Save(C5J_APAnalysis _APAnalysis)
        {
            try
            {
                Add(_APAnalysis);

                return true;
            }
            catch(Exception error) { return false; }
        }
    }
}
