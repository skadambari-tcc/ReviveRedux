using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;


namespace Revive.Web.Repository
{
    public class MenuMappingRepository : IMenuMappingRepository
    {
        private IGeneralRepository _IGeneralRepository;
        public MenuMappingRepository()
        {
            _IGeneralRepository = new GeneralRepository();
        }
        public List<MenuModel> GetAllMenuForRole(int UserLevelId)
        {
            var menuSecurityAttributes = new List<MenuModel>();
            int id = _IGeneralRepository.GetUserLevelId("SUPERADMIN");
            if (id == UserLevelId)
            {
                UserLevelId = _IGeneralRepository.GetUserLevelId("ADMIN");
             }
            
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var allmenus = dbContext.Menus.OrderBy(x=>x.SeqNo).ToList();
                   
                    var rolemenuMapping = dbContext.User_Level_Menu_Mapping.Where(map => map.User_Level_Id == UserLevelId).ToList();
                    

                    //var rolemenuMapping = (from d in dbContext.User_Level_Menu_Mapping
                    //                       join _menus in dbContext.Menus on d.MenuId equals _menus.MenuId
                    //                       orderby _menus.SeqNo
                    //                       select new MenuModel
                    //                       {
                    //                           Id = d.Id,
                    //                           MenuId = d.MenuId,
                    //                           menuSeqno=_menus.SeqNo
                    //                           //User_Level_Id = d.User_Level_Id,
                    //                           //CreatedOn = d.CreatedOn,
                    //                           //CreatedBy = d.CreatedBy,
                    //                           //ModifiedBy = d.ModifiedBy,
                    //                           //ModifiedOn = d.ModifiedOn
                    //                       }).OrderByDescending(x => x.menuSeqno).ToList();

                   

                    foreach (var menu in allmenus)
                    {
                        var menuSecurity = new MenuModel();
                        menuSecurity.UserLevelID = menu.Id;
                        menuSecurity.MenuId = menu.MenuId;
                        menuSecurity.MenuName = menu.MenuName;
                        menuSecurity.MenuType = menu.MenuType;
                        menuSecurity.ParentMenuId = menu.ParentMenuId;
                        //allMenuModel.Add(menuSecurity);
                    
                        foreach (var menuMapping in rolemenuMapping)
                        {
                            if (Convert.ToInt32(menuMapping.MenuId) == menu.MenuId)
                            {
                                menuSecurity.IsSelected = true;
                            }
                        }
                        menuSecurityAttributes.Add(menuSecurity);
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return menuSecurityAttributes;
        }
        public List<int> GetallMenuIdsForRole(Int32 roleId)
        {
            try
            {
                var allMenuIdsForRole = new List<int>();
                using (var dbContext = new ReviveDBEntities())
                {
                    allMenuIdsForRole = (from data in dbContext.User_Level_Menu_Mapping where data.User_Level_Id == roleId select data.MenuId).ToList();
                }
                return allMenuIdsForRole;
            }
            catch (Exception ex)
            {

                throw ;
            }

        }
        public bool SetMenuSecurityForRole(IEnumerable<MenuModel> menuSecurityAttributes, string roleId)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (!String.IsNullOrEmpty(roleId))
                    {
                        var intRoleId = Convert.ToInt32(roleId);
                        int id = _IGeneralRepository.GetUserLevelId("SUPERADMIN");
                        if (id == Convert.ToInt32(roleId))
                        {
                            intRoleId = _IGeneralRepository.GetUserLevelId("ADMIN");
                        }
                       
                        var userMenuMapping = dbContext.User_Level_Menu_Mapping.Where(cond => cond.User_Level_Id == intRoleId).ToList();
                        var menusToAdd = menuSecurityAttributes.Where(cond => cond.IsSelected == true).ToList();
                        foreach (var usermenuToDelete in userMenuMapping)
                        {
                            try
                            {
                                dbContext.User_Level_Menu_Mapping.Remove(usermenuToDelete);
                                dbContext.SaveChanges();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        foreach (var usermenuToAdd in menusToAdd)
                        {
                            var roleLevelMenu = new User_Level_Menu_Mapping();
                            roleLevelMenu.MenuId = usermenuToAdd.MenuId;
                            roleLevelMenu.User_Level_Id = intRoleId;
                            roleLevelMenu.CreatedOn = DateTime.Now;

                            dbContext.User_Level_Menu_Mapping.Add(roleLevelMenu);
                            dbContext.SaveChanges();
                        }
                        //for adding Menu_Id  to the controller 
                        // Remove code on 03-Apr-2017  it is not used this table getting menu
                        //var lstUserControllerMapping = dbContext.UserMap_Controller_Action.Where(cond => cond.User_Level_ID == intRoleId).ToList();
                        //foreach (var objUserControllerMapping in lstUserControllerMapping)
                        //{
                        //    dbContext.UserMap_Controller_Action.Remove(objUserControllerMapping);
                        //    dbContext.SaveChanges();                          
                        //}

                        //foreach (var usermenuToAdd in menusToAdd)
                        //{
                        //    var lstControllerActions = dbContext.ControllerActions.Where(cond => cond.Menu_Id == usermenuToAdd.MenuId).ToList();

                        //    foreach (var objControllerActions in lstControllerActions)
                        //    {
                        //        var objControllerAction = new UserMap_Controller_Action();
                        //        objControllerAction.Controller_Id = objControllerActions.Controller_Id;
                        //        objControllerAction.Action_Id = objControllerActions.Action_Id;
                        //        objControllerAction.User_Level_ID = intRoleId;
                        //        objControllerAction.Created_Date = Revive.Redux.Common.CommonMethods.GetCurrentDate();

                        //        dbContext.UserMap_Controller_Action.Add(objControllerAction);
                        //        dbContext.SaveChanges();
                        //    }
                        //}
                    }

                    flag = true;
                }
            }
            catch (Exception ex)
            {

                throw ;
            }
            return flag;
        }

    }
}
