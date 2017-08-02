using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;

using Revive.Redux.Entities;
namespace Revive.Redux.Services
{
    public class RoleManagementService : IRoleManagementService
    {

        private IRoleManagementRepository _IRoleManagementRepository = null;

        public RoleManagementService()
        {
            _IRoleManagementRepository = new RoleManagementRepository();

        }

        public string AddRole(User_LevelModel role)
        {
            return _IRoleManagementRepository.AddRole(role);
        }

        public User_LevelModel GetRoleForUser(string userName)
        {
            return _IRoleManagementRepository.GetRoleForUser(userName);
        }
        public int GetRoleIDForUser(string username)
        {
            return _IRoleManagementRepository.GetRoleIDForUser(username);
        }
        public User_LevelModel GetRoleById(int ID)
        {
            return _IRoleManagementRepository.GetRoleById(ID);
        }

        public string DeleteRole(User_LevelModel role)
        {
            return _IRoleManagementRepository.DeleteRole(role);
        }

        public bool UpdateRole(User_LevelModel role)
        {
            return _IRoleManagementRepository.UpdateRole(role);
        }
        public IEnumerable<RoleDetailsModel> GetRoleList()
        {
            return _IRoleManagementRepository.GetRoleList();
        }
        
        public IEnumerable<Role> GetAuthUserRole(int UserLevelId)
        {
            return _IRoleManagementRepository.GetAuthUserRole(UserLevelId);
        }
        public bool CheckRoleExists(User_LevelModel Object)
        {
            return _IRoleManagementRepository.CheckRoleExists(Object);
        }
        public string AddNewRole(User_LevelModel role)
        {
            return _IRoleManagementRepository.AddNewRole(role);
        }
        public bool CheckDuplicateRoleName(string User_Level_Name, int User_Level_ID)
        {
            return _IRoleManagementRepository.CheckDuplicateRoleName(User_Level_Name, User_Level_ID);
        }
    }

}
