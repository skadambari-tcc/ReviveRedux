using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    /// <summary>
    /// manageMember for API
    /// </summary>
    public interface IMembershipRepository
    {
        RepositoryResult AddEditMembership(MembersShipModel objmembership);
        IEnumerable<MemberShipDetailsModel> GetMembmershipDetails(MembersShipModel objMembership);
        Boolean CheckEmailAddressExists(String emailAddress, string membershipId, Guid user_Id);
        bool updateMembership(MembersShipModel objMembership);
        RepositoryResult renewMembership(MembersShipModel objMembership);

        string CheckMachineActive(int machine_id);
        int GetCustomizedTemplateIdByMachineId(int machine_id);
        IEnumerable<CustomizedParameterModel> GetCustomizedDebugParameter(int Template_Id);
        int GetMachineIdByMachineIdVal(string machine_Id_Val);
        SoftwareVersionModel GetSoftwareVersionByMachineId(int machine_Id);
        RepositoryResult InitiateCheckinProcess(MemberActivityModel objMembershipActivity);
        RepositoryResult UpdateReviveStartResult(MemberActivityModel objMembershipActivity);
        RepositoryResult UpdateReviveEndResult(MemberActivityModel objMembershipActivity);
        RepositoryResult SendDebugInformation(MemberActivityModel objMembershipActivity);
        RepositoryResult ReviveCheckOut(MemberActivityModel objMembershipActivity);
        RepositoryResult MachineFault(MemberActivityModel objMemberActivityModel);
        RepositoryResult updateSoftwareVersion(MemberActivityModel objMemberActivityModel);
        ModeConfigurationAPIModel GetModeValidation(MemberActivityModel objMembership);
        bool CheckMembershipActive(string membership_Id, Guid userId);
        int GetLastModeByMachineId(int machineId, int mode_id);
        bool DeleteTemplateByMachineId(int machineId);
        RepositoryResult DeleteMemberShipById(MembersShipModel objmembership);
        RepositoryResult CheckValidMemberByEmail(MembersShipModel objmembership);
        IsDOBValid IsValidMemberByDOB(MembersShipModel objmembership);
        RepositoryResult UpdateReviveFaultResult(MemberActivityModel objMembershipActivity);
        ReviveDetails GetReviveDetails(APIParameter APIParameterObj);
        RepositoryResult UploadMember(MembershipWebModel objmembership);
        Boolean CheckDuplicateMailByCustomerID(string emailAddress, int CustomerId);

        RepositoryResult VoidMembership(MembersShipModel objmembership);
        ManageMember GetMemberDetailsByMemberDtlsID(MemberShipParameters ObjMemberShipParameters);
        bool updateMemberDetailsByMemberDtlsID(ManageMember objMembershipDetails);
        bool voidMemberShipByMembershipId(MemberShipParameters MembershipPara);
        bool IsReviveStopped(MemberActivityModel objMembershipActivity, string ActivityTypeCode);
        //List<MemberShipDetails> GetMembershipDetailsES(DataSourceRequest req, MemberShipParameters ObjMemberShipParameters);
        DataSourceResult GetMembershipDetailsES(DataSourceRequest req, MemberShipParameters ObjMemberShipParameters);
        IEnumerable<ReviveDataByPhoneParam> GetReviveData(MembersShipModel objMembership);
        bool IsAPIAccessByClient(string CustRefCode, Guid APIKey);
        IEnumerable<MemberShipDetailsModel> SearchMemberByCustomerId(MembersShipModel objMembership);
        DateTime GetReviveLastDateByMembershipId(string MembershipID);
        string UpdateTabletNonMemberFieldAsEncryption(string key);
        IEnumerable<AppMachineHistoryModel> GetAppMachineHistoryByMachineId(MembersShipModel objMembership);
        AppMachineHistoryModel GetAppMachineHistoryByProcessId(MembersShipModel objMembership);
        int GetMachineIdValidateForReviveByMachineId(int machine_Id);







    }
}
