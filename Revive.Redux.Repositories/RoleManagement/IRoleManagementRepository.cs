using Revive.Redux.Repositories;
using Revive.Redux.Entities;
using System.Collections.Generic;

namespace Revive.Redux.Repositories
{
    public interface IRoleManagementRepository
    {

        string AddRole(User_LevelModel role);
        User_LevelModel GetRoleForUser(string userName);
        User_LevelModel GetRoleById(int ID);
        int GetRoleIDForUser(string username);
        string DeleteRole(User_LevelModel role);
        bool UpdateRole(User_LevelModel role);
        IEnumerable<RoleDetailsModel> GetRoleList();
        IEnumerable<Role> GetAuthUserRole(int UserLevelId);
        bool CheckRoleExists(User_LevelModel Object);
        string AddNewRole(User_LevelModel role);
        bool CheckDuplicateRoleName(string User_Level_Name, int User_Level_ID);
    }
}
