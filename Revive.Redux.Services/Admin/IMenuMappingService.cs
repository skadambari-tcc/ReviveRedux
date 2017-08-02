using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;
namespace Revive.Redux.Services
{
    public interface IMenuMappingService
    {
        List<int> GetallMenuIdsForRole(int roleId);
        List<int> GetallMenuIdsForUser(string userName);
        List<MenuModel> GetAllMenuForRole(int UserLevelId);
        bool SetMenuSecurityForRole(IEnumerable<MenuModel> menuSecurityAttributes, string roleId);
    }
}
