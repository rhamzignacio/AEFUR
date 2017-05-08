using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class UserAccountViewModel : ViewModelBase<UserAccount>
    {
        public string Edit(UserAccount user, AgentProfile agent)
        {
            try
            {
                //Update Agent Profile
                if (agent != null)
                {
                    string agentUpdate = new AgentCodeViewModel().Edit(agent);

                    if (agentUpdate != "Y")
                        return agentUpdate;
                }

                var updateUser = GetEntity(r => r.ID == user.ID);

                updateUser.Username = user.Username;

                updateUser.Password = user.Password;

                updateUser.FirstName = user.FirstName;

                updateUser.MiddleName = user.MiddleName;

                updateUser.LastName = user.LastName;

                updateUser.Department = user.Department;

                updateUser.AccessRights = user.AccessRights;

                Update(updateUser);

                return "Y";
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }

        public UserAccount CheckIfExist(string signInCode, Guid? userID)
        {
            if (signInCode != "")
            {
                signInCode = signInCode.ToUpper();

                var db = new AirlineBillingReportEntities();

                var query = from agent in db.AgentProfile
                            join user in db.UserAccount on agent.ID equals user.AgentID
                            where agent.CebuPacificSafeName == signInCode ||
                            agent.AirAsiaSafeName == signInCode ||
                            agent.IASASafeName == signInCode ||
                            agent.IATASafeName == signInCode ||
                            agent.PALSafeName == signInCode ||
                            agent.PartnerAgent == signInCode ||
                            agent.ClientPartnerAgent == signInCode || 
                            agent.ClientPartnerAgentSafeName == signInCode
                            where user.ID != userID
                            select user;

                return query.FirstOrDefault();
            }
            else
                return null;
        }

        public UserAccount GetUser(Guid? userID)
        {
            return GetEntity(r => r.ID == userID);
        }

        public string Save(UserAccount user, AgentProfile agent)
        {
            try
            {
                Guid? agentID = null;

                if (agent != null)
                {
                    var agentVM = new AgentCodeViewModel();

                    agentID = agentVM.Save(agent);
                }

                if (agentID != Guid.Empty)
                    user.AgentID = agentID;

                Add(user);

                return "Y";
            }
            catch(Exception error)
            {
                return error.Message;
            }
        }

        public List<UserAccount> GetAll()
        {
            return Find();
        }

        public List<UserAccount> GetAll(string searchKey)
        {
            return Find(r => r.FirstName.ToLower().Contains(searchKey.ToLower())
                || r.LastName.ToLower().Contains(searchKey.ToLower()) || 
                r.Department.ToLower().Contains(searchKey.ToLower()));
        }

        public bool ChangePassword(Guid userID, string newPassword)
        {
            try
            {
                var user = GetEntity(r => r.ID == userID);

                user.Password = newPassword;

                Update(user);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string TryLogin(string _username, string _password)
        {
            try
            {
                var user = GetEntity(r => r.Username == _username && r.Password == _password);

                if (user != null)
                    return "Y"; //Successful login
                else
                    return "N"; //Invalid Username or Password
            }
            catch(Exception error)
            {
                return error.Message;
            }
        }

        public UserAccount GetUserID(string _agentCode)
        {
            return GetEntity(r => r.AgentCode == _agentCode);
        }

        public UserAccount GetSelectedUser(string _username)
        {
            return GetEntity(r => r.Username == _username);
        }
    }
}
