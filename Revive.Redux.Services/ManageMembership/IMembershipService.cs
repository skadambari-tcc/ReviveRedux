using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Revive.Redux.Services
{
    public interface IMembershipService
    {
        RepositoryResult AddEditMembership(MembersShipModel objmembership);
        IEnumerable<MemberShipDetailsModel> GetMembmershipDetails(MembersShipModel objMembership);
        Boolean CheckEmailAddressExists(String emailAddress, string membershipId, Guid user_Id);
        bool updateMembership(MembersShipModel objMembership);
        RepositoryResult renewMembership(MembersShipModel objMembership);
        IEnumerable<DtoList> GetDevivemanufacturer();
        IEnumerable<DtoList> GetHowlongago();
        int GetCustomizedTemplateIdByMachineId(string machine_id_val);
        IEnumerable<CustomizedParameterModel> GetCustomizedDebugParameter(int Template_Id);
        SoftwareVersionModel GetSoftwareVersionByMachineId(int machine_Id);
        RepositoryResult InitiateCheckinProcess(MemberActivityModel objMembershipActivity);
        RepositoryResult UpdateReviveStartResult(MemberActivityModel objMembershipActivity);
        RepositoryResult UpdateReviveEndResult(MemberActivityModel objMembershipActivity);
        RepositoryResult SendDebugInformation(MemberActivityModel objMembershipActivity);
        RepositoryResult ReviveCheckOut(MemberActivityModel objMembershipActivity);
        RepositoryResult MachineFault(MemberActivityModel objMemberActivityModel);
        RepositoryResult updateSoftwareVersion(MemberActivityModel objMemberActivityModel);
        IEnumerable<DtoList> GetModes(Guid user_id);
        ModeConfigurationAPIModel GetModeValidation(MemberActivityModel objMembership);
        RepositoryResult DeleteMemberShipById(MembersShipModel objmembership);
        RepositoryResult CheckValidMemberByEmail(MembersShipModel objmembership);
        IsDOBValid IsValidMemberByDOB(MembersShipModel objmembership);
        ReviveDetails GetReviveDetails(APIParameter APIParameterObj);
        bool MemberFileUpload(ManageMember objMember, out ManageMember objMemberResult);
        bool MemberFileUploadAW(ManageMember objMember, out ManageMember objMemberResult);
        RepositoryResult UploadMember(MembershipWebModel objmembership);
        Boolean CheckDuplicateMailByCustomerID(string emailAddress, int CustomerId);
        SoftwareVersionModel CheckFirmWareUpdate(string machine_id_Val);

        RepositoryResult VoidMembership(MembersShipModel objmembership);
        ManageMember GetMemberDetailsByMemberDtlsID(MemberShipParameters ObjMemberShipParameters);
        bool updateMemberDetailsByMemberDtlsID(ManageMember objMembershipDetails);
        bool voidMemberShipByMembershipId(MemberShipParameters MembershipPara);
        // List<MemberShipDetails> GetMembershipDetailsES(DataSourceRequest req, MemberShipParameters ObjMemberShipParameters);
        DataSourceResult GetMembershipDetailsES(DataSourceRequest req, MemberShipParameters ObjMemberShipParameters);
        IEnumerable<ReviveDataByPhoneParam> GetReviveData(MembersShipModel objMembership);
        bool IsAPIAccessByClient(string CustRefCode, Guid APIKey);
        IEnumerable<MemberShipDetailsModel> SearchMemberByCustomerId(MembersShipModel objMembership);
        DateTime GetReviveLastDateByMembershipId(string MembershipID);
        string UpdateTabletNonMemberFieldAsEncryption(string key);
        IEnumerable<AppMachineHistoryModel> GetAppMachineHistoryByMachineId(MembersShipModel objMembership);
        AppMachineHistoryModel GetAppMachineHistoryByProcessId(MembersShipModel objMembership);
        
    }
}
