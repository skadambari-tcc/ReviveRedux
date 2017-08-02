using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;
namespace Revive.Web.Repository
{
    public interface IMenuMappingRepository
    {
        List<int> GetallMenuIdsForRole(Int32 roleId);
        List<MenuModel> GetAllMenuForRole(int UserLevelId);
        bool SetMenuSecurityForRole(IEnumerable<MenuModel> menuSecurityAttributes, string roleId);
    }
}
