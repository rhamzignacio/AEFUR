using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{

    public class RecordNoStorageViewModel : ViewModelBase<RecordNoStorage>
    {
        public List<RecordNoStorage> GetAll()
        {
            return Find().OrderByDescending(r=>r.Date).ToList();
        }

        public List<RecordNoStorage> GetRecordList(Guid _userID)
        {
            return Find(r => r.UserID == _userID).OrderByDescending(r => r.Date).ToList();
        }

        public bool IfExist(string _recordNo)
        {
            var record = GetEntity(r => r.RecordNo == _recordNo);

            if (record != null)
                return true;
            else
                return false;
        }
        private void SaveRecordNo(Guid _userID, string _recordNo, string cebuPac, string airAsia,
            string IASA, string IATA, string PAl)
        {
            RecordNoStorage newRecordNo = new RecordNoStorage();

            newRecordNo.RecordNo = _recordNo;

            newRecordNo.Date = DateTime.Now;

            newRecordNo.UserID = _userID;

            if (cebuPac != "")
                newRecordNo.C5J = "Y";

            if (airAsia != "")
                newRecordNo.AIRASIA = "Y";

            if (IATA != "")
                newRecordNo.IATA = "Y";

            if (IASA != "")
                newRecordNo.IASA = "Y";

            if (PAl != "")
                newRecordNo.PAL = "Y";

            Add(newRecordNo);
        }

        public string GenerateRecordNo(Guid _userID, string cebuPac, string airAsia,
            string IASA, string IATA, string PAl)
        {
            string recordNo = "";

            string month = DateTime.Now.Month.ToString();

            string day = DateTime.Now.Day.ToString();

            string year = DateTime.Now.Year.ToString();

            string counter = "";

            if (month.Length < 2)
                month = "0" + month;

            if (day.Length < 2)
                day = "0" + day;

            var ctr = Find(r => r.RecordNo.Contains(month + day + year)).Count;

            ctr++;

            counter = ctr.ToString();

            while(counter.Length < 3)
            {
                counter = "0" + counter;
            }

            recordNo = month + day + year + counter; // mm/dd/yyyy/000

            SaveRecordNo(_userID, recordNo, cebuPac, airAsia, IASA, IATA, PAl); //Save Record no to database

            return recordNo;
        }
    }
}
