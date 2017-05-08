using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class LoginLogsViewModel : ViewModelBase<LoginLogs>
    {
        public class UserLoginLogs
        {
            public Guid UserID { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Date { get; set; }
            public string _Time { get; set; }
            public string Time
            {
                get
                {
                    return DateTime.Parse(_Time).ToShortTimeString();
                }
                set
                {
                    _Time = value;
                }
            }
        }

        public List<UserLoginLogs> GetLogs(DateTime fromDate, DateTime toDate)
        {
            using (var db = new AirlineBillingReportEntities())
            {
                var query = from logs in db.LoginLogs
                            join user in db.UserAccount on logs.UserAccountID equals user.ID
                            where logs.LoginDate >= fromDate && logs.LoginDate <= toDate
                            orderby logs.LoginDate descending
                            orderby logs.LoginTime descending
                            select new UserLoginLogs
                            {
                                UserID = user.ID,
                                Username = user.Username,
                                Name = user.FirstName + " " + user.LastName,
                                Date = logs.LoginDate.ToString(),
                                Time = logs.LoginTime.ToString()
                            };

                return query.ToList();
            }
        }

        public bool LoginLog(LoginLogs login)
        {
            try
            {
                var ifAlreadyLogged = GetEntity(r => r.UserAccountID == login.UserAccountID && r.LoginDate == DateTime.Now.Date);

                if (ifAlreadyLogged != null) //Update existing record
                {
                    ifAlreadyLogged.LoginTime = DateTime.Now.TimeOfDay;

                    Update(ifAlreadyLogged);
                }
                else //Create new record
                {
                    login.LoginDate = DateTime.Now.Date;

                    login.LoginTime = DateTime.Now.TimeOfDay;

                    Add(login);
                }

                return true;
            }
            catch
            { return false ; }
        }
    }
}
