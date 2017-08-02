using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public class SubAgentManagement : ISubAgentManagement
    {
        ISubAgentManagementRespository _ISubAgentManagement = null;

        public SubAgentManagement()
        {
            _ISubAgentManagement = new SubAgentManagementRespository();
        }

        public IEnumerable<ManageSubAgentModel> GetSubAgentList(Guid userId, string _PageAccessCode)
        {
            return _ISubAgentManagement.GetSubAgentList(userId, _PageAccessCode);
        }

        public ManageSubAgentModel CreateSubAgent(ManageSubAgentModel AgentRecord)
        {
            return _ISubAgentManagement.CreateSubAgent(AgentRecord);
        }
        public ManageSubAgentModel GetSubAgentDetailsById(int AgentID)
        {
            return _ISubAgentManagement.GetSubAgentDetailsById(AgentID);
        }

        public Boolean CheckSubAgentExists(String Name, int AgentID)
        {
            return _ISubAgentManagement.CheckSubAgentExists(Name, AgentID);
        }


        

        public Boolean CheckSubAgentRefExists(String SubAgent_Ref_Code, int AgentID, int CustomerId)
        {
            return _ISubAgentManagement.CheckSubAgentRefExists(SubAgent_Ref_Code, AgentID, CustomerId);
        }
    }
}
