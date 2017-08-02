using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public interface ISubAgentManagementRespository
    {
        IEnumerable<ManageSubAgentModel> GetSubAgentList(Guid userId, string _PageAccessCode);
        ManageSubAgentModel CreateSubAgent(ManageSubAgentModel AgentRecord);
        ManageSubAgentModel GetSubAgentDetailsById(int AgentID);
        Boolean CheckSubAgentExists(String Name, int AgentID);
        Boolean CheckSubAgentRefExists(String SubAgent_Ref_Code, int AgentID, int CustomerId);
    }
}
