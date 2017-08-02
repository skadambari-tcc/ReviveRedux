using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux;
using Revive.Redux.Entities;

namespace Revive.Redux.Services
{
    public interface IUserManagmentService
    {
        bool AddEditUser(UserModels objUser);
        IEnumerable<DtoUser> GetUserDetails(int User_Level_Id);
        IEnumerable<DtoUser> GetUserDetails(int User_Level_Id, int Customer_Id);
        UserModels GetUserById(Nullable<System.Guid> uId);
        string ActivateDeactivateUserByGuid(UserModels objUsermodel);
        IEnumerable<DtoUser> GetStoreUserDetails(int User_Level_Id, int Customer_Id);
        string checkValidUserToActivate(Guid userId);
        bool UserFileUpload(UserModels objUser, out UserModels objUserResult);
        int GetStoreUserCountbyLocation(Guid UserId);
        bool LogStoreUserbyLocation(Guid UserId, int count, bool Status,int LogType);
        DtoUser GetUserDetailsByUserId(Guid UserId);
        IEnumerable<DtoUser> GetStoreUserDetails(int User_Level_Id, UserModels UserDetails);
        List<GroupModel> GetGroupList();
        GroupModel GetGroupById(int Id);
        bool AddEditGroup(GroupModel ObjGroupModel, Guid UserId);
        bool CheckDuplicateGroupName(string GroupName, int GroupId);
        bool CheckDuplicatePriority(int PriorityId, int GroupId);
        bool ChangeStatusByGroupId(int GroupId, Guid modifiedBy, bool Status);
    }
}
