using System.Collections.Generic;
using Revive.Redux.Entities;
using Revive.Web.Repository;
namespace Revive.Redux.Services
{
    public class MenuMappingService:IMenuMappingService
    {
        IMenuMappingRepository menuMappingRepository=null;
        
        public MenuMappingService()
        {
            menuMappingRepository = new MenuMappingRepository();
        }
        public List<int> GetallMenuIdsForRole(int roleId)
        {
            return menuMappingRepository.GetallMenuIdsForRole(roleId);
        }
        public List<int> GetallMenuIdsForUser(string userName)
        {
            RoleManagementService roleService = new RoleManagementService();
            int roleId = roleService.GetRoleIDForUser(userName);
            return GetallMenuIdsForRole(roleId);


        }
        public List<MenuModel> GetAllMenuForRole(int UserLevelId)
        {
            return menuMappingRepository.GetAllMenuForRole(UserLevelId);
        }
        public bool SetMenuSecurityForRole(IEnumerable<MenuModel> menuSecurityAttributes, string roleId)
        {
            return menuMappingRepository.SetMenuSecurityForRole(menuSecurityAttributes, roleId);

        }
    }
}
