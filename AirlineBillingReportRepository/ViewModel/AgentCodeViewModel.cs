using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class AgentCodeViewModel : ViewModelBase<AgentProfile>
    {
        public string Edit(AgentProfile _agent)
        {
            try
            {
                var agent = GetEntity(r => r.ID == _agent.ID);

                if (agent != null)
                {
                    agent.AirAsia = _agent.AirAsia;

                    agent.CebuPacific = _agent.CebuPacific;

                    agent.PartnerAgent = _agent.PartnerAgent;

                    agent.Department = _agent.Department;

                    agent.IASA = _agent.IASA;

                    agent.IATA = _agent.IATA;

                    agent.PAL = _agent.PAL;

                    agent.TravCom1 = _agent.TravCom1;

                    agent.TravCom2 = _agent.TravCom2;

                    agent.TravCom3 = _agent.TravCom3;

                    agent.TravCom4 = _agent.TravCom4;

                    agent.TravCom5 = _agent.TravCom5;

                    //Safe Names
                    agent.CebuPacificSafeName = agent.CebuPacific.ToUpper();

                    agent.PartnerAgentSafeName = agent.PartnerAgent.ToUpper();

                    agent.AirAsiaSafeName = agent.AirAsia.ToUpper();

                    agent.PALSafeName = agent.PAL.ToUpper();

                    agent.IASASafeName = agent.IASA.ToUpper();

                    agent.IATASafeName = agent.IATA.ToUpper();

                    agent.SafeName = agent.Name.ToUpper();

                    Update(agent);
                }

                return "Y";
            }
            catch(Exception error)
            {
                return error.Message;
            }
        }

        public Guid Save(AgentProfile agent)
        {
            try
            {
                agent.CebuPacificSafeName = agent.CebuPacific.ToUpper();

                agent.PartnerAgentSafeName = agent.PartnerAgent.ToUpper();

                agent.ClientPartnerAgentSafeName = agent.PartnerAgent.ToUpper();

                agent.AirAsiaSafeName = agent.AirAsia.ToUpper();

                agent.PALSafeName = agent.PAL.ToUpper();

                agent.IASASafeName = agent.IASA.ToUpper();

                agent.IATASafeName = agent.IATA.ToUpper();

                agent.SafeName = agent.Name.ToUpper();

                Add(agent);

                return agent.ID;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public AgentProfile GetSelectedAgent(Guid? ID)
        {
            return GetEntity(r => r.ID == ID);
        }

        public List<AgentProfile> Search(string _searchKey)
        {
            string search = _searchKey.ToUpper();

            return Find(r => r.SafeName.Contains(search) || r.PALSafeName.Contains(search) || r.CebuPacificSafeName.Contains(search)
                || r.IASASafeName.Contains(search) || r.IATASafeName.Contains(search) || r.AirAsiaSafeName.Contains(search)
                || r.PartnerAgentSafeName.Contains(search));
        }

        public List<AgentProfile> GetAll()
        {
            return Find();
        }

       
    }
}
