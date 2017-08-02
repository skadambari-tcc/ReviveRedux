using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public class SubsidiaryManagement : ISubsidiaryManagement
    {
        ISubsidiaryManagementRespository _ISubsidiaryManagement = null;
        IGeneralRepository _generalRepository = null;

        public SubsidiaryManagement()
        {
            _ISubsidiaryManagement = new SubsidiaryManagementRespository();
            _generalRepository = new GeneralRepository();
        }

        public IEnumerable<ManageSubsidiaryModel> GetSubsidiaryList(Guid userId, string _PageAccessCode)
        {
            return _ISubsidiaryManagement.GetSubsidiaryList(userId, _PageAccessCode);
        }

        public ManageSubsidiaryModel CreateSubsidiary(ManageSubsidiaryModel SubsidiaryRecord)
        {
            return _ISubsidiaryManagement.CreateSubsidiary(SubsidiaryRecord);
        }
        public ManageSubsidiaryModel GetSubsidiaryDetailsById(int SubsidiaryID)
        {
            return _ISubsidiaryManagement.GetSubsidiaryDetailsById(SubsidiaryID);
        }

        public Boolean CheckSubsidiaryExists(String Name, int Subsidiary_ID)
        {
            return _ISubsidiaryManagement.CheckSubsidiaryExists(Name, Subsidiary_ID);
        }


        public string DeActivate(int SubsidiaryId, bool status, Guid modifiedBy)
        {

            string result = _generalRepository.checkValidSubdirectoryToActivate(SubsidiaryId);
            //string result=_
          //  return _ISubsidiaryManagement.DeActivate(SubsidiaryId, status, modifiedBy);
            return result;
        }

        public Boolean CheckSubsidiaryRefExists(String Subsidiary_Ref_Code, int Subsidiary_ID, int CustomerId)
        {
            return _ISubsidiaryManagement.CheckSubsidiaryRefExists(Subsidiary_Ref_Code, Subsidiary_ID, CustomerId);
        }
    }
}
