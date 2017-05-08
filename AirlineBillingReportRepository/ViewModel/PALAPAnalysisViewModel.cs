using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class PALAPAnalysisViewModel : ViewModelBase<PAL_APAnalysis>
    {
        public bool Save(PAL_APAnalysis _APAnalysis)
        {
            try
            {
                Add(_APAnalysis);

                return true;
            }
            catch (Exception error) { return false; }
        }
    }
}
