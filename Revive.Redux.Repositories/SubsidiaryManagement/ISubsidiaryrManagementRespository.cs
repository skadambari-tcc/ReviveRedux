using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public interface ISubsidiaryManagementRespository
    {
        IEnumerable<ManageSubsidiaryModel> GetSubsidiaryList(Guid userId, string _PageAccessCode);
        ManageSubsidiaryModel CreateSubsidiary(ManageSubsidiaryModel SubsidiaryRecord);
        ManageSubsidiaryModel GetSubsidiaryDetailsById(int SubsidiaryID);
        Boolean CheckSubsidiaryExists(String Name, int Subsidiary_ID);
        Boolean CheckSubsidiaryRefExists(String Subsidiary_Ref_Code, int Subsidiary_ID, int CustomerId);
    }
}
