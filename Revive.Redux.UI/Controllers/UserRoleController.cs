using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Revive.Redux.Entities;
using Kendo.Mvc.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Kendo.Mvc.Extensions;
using Revive.Redux.Controllers.Common;

namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class UserRoleController : Controller
    {
        public string RoleId = string.Empty;
        public string RoleName = string.Empty;
        private IGeneralService _IGeneralService = null;
        //private IUserManagementRepository _IUserManagementRepository = null;
        private IRoleManagementService _IRoleManagementService = null;
        private IMenuMappingService _IMenuMappingService = null;
        public List<MenuModel> _allmenus = new List<MenuModel>();
        private DtoList role = null;
        public UserRoleController()
        {
           // _IUserManagementRepository = new UserManagementRepository();
            _IGeneralService = new GeneralService();
            _IRoleManagementService = new RoleManagementService();
            _IMenuMappingService = new MenuMappingService();
            ViewBag.UserTypelst = _IGeneralService.GetUserRoleService();
            ViewBag.isUpdateMode = false;
            //ViewBag.RoleBack = false;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        
        #region Role Management
        /// <summary>
        /// Get All User Types And Bind Data To DropDown
        /// </summary>
        /// <returns></returns>
        public JsonResult AddUserType()
        {
            var records = _IGeneralService.GetUserType();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

       /// <summary>
       /// Get all User Roles And bind data to grid
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
        public ActionResult GetRolesData([DataSourceRequest]DataSourceRequest request)
        {

            IEnumerable<RoleDetailsModel> RoleDetails = _IRoleManagementService.GetRoleList();
            DataSourceResult result = RoleDetails.ToDataSourceResult(request, role => new User_LevelModel
            {
                User_Level_Name = role.Role_Name,
                UserType = role.UserType,
                User_Level_ID = role.Role_id,
                page_access_code=role.page_access_code
            });
           return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Delete User role on tha basis of id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteRole(string id)
        {
            if (id != null)
            {
                RoleId = id.Split(',').First();
                RoleName = id.Split(',').Last();
            }

            if (!string.IsNullOrEmpty(RoleId))
            {
                Session["roleId"] = RoleId;
                Session["RoleName"] = RoleName;
            }
            else
            {
                if (Session["roleId"] != null)
                    RoleId = (string)Session["roleId"];
                if (Session["RoleName"] != null)
                    RoleName = (string)Session["RoleName"];
            }
            int RecordID = Convert.ToInt32(RoleId);
            var Record = _IRoleManagementService.GetRoleById(RecordID);
            User_LevelModel DeleteRole = new User_LevelModel();
            if (Record != null)
            {
                DeleteRole.User_Level_ID = Record.User_Level_ID;
            }
            //Delete the record
            string ErrorMsgs=_IRoleManagementService.DeleteRole(DeleteRole);
            TempData["DeleteError"] = ErrorMsgs;
            if (ErrorMsgs != null)
            {
                User_LevelModel obj = new User_LevelModel();
                obj._ErrorMsg = (string)TempData["DeleteError"];
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("GetRoles");
        }
        
        
        public ActionResult GetRoles()
        {
            User_LevelModel obj = new User_LevelModel();
            obj._ErrorMsg = (string)TempData["DeleteError"];
            Session["BackUrl"] = "RedisrectToRolePage";
            return View("GetRoles", obj);
        }

        public ActionResult AddNewRole()
        {
            var obj = new User_LevelModel();
            obj.UserTypeList = GetData();
            return View(obj);
            
        }

        public ActionResult CreateNewRole(User_LevelModel role)
        {
            string mesg = "";
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null)
            {
                if (role != null)
                {
                    role.Created_by = currentUser.User_Id;
                    mesg = _IRoleManagementService.AddNewRole(role);
                    TempData["Success"] = ReviveMessages.Add; //mesg;
                }
            }
            return RedirectToAction("GetRoles");
        }

        public ActionResult EditRole(int Id)
        {
            var existingRole = new User_LevelModel();
            if (Id != 0)
            {
                
                existingRole = _IRoleManagementService.GetRoleById(Id);
                existingRole.UserTypeList = GetData();
            }
            return View(existingRole);
        }

        public ActionResult UpdateRole(User_LevelModel role)
        {
            string mesg = "";
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null)
            {
                if (role != null)
                {
                    role.Modified_by = currentUser.User_Id;
                    var flag = _IRoleManagementService.UpdateRole(role);
                    if (flag == true)
                    { 
                        mesg = role.User_Level_Name + " successfully updated";
                        TempData["Success"] = ReviveMessages.Update;//mesg;
                    }
                }
            }
            return RedirectToAction("GetRoles");
        }

        public JsonResult CheckDuplicateRoleName(string User_Level_Name, int User_Level_ID)
        {
                var data = !_IRoleManagementService.CheckDuplicateRoleName(User_Level_Name, User_Level_ID);
                return Json(data);
            
        }

        #endregion

        #region Private Methods

        private IEnumerable<DtoList> GetData()
        {
            var objModel = _IGeneralService.GetUserType();
            //objModel.UserTypeList 
            
            return objModel;
        }


        #endregion

        #region MenuMapping
        /// <summary>
        /// Menu Mapping for login user
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuMappingForLoginUser()
        {
            var currentUserDetail = new CurrentUserDetail();
            User_LevelModel currentUserRole = new User_LevelModel();
             role = new DtoList();
            currentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            currentUserRole = (User_LevelModel)Session["Roles"];
            string id = string.Empty;
            if (currentUserDetail != null)
            {
                RoleId = Convert.ToString(currentUserDetail.UserLevelId);
                RoleName = Convert.ToString(currentUserRole.User_Level_Name);

            }
            role = _IGeneralService.GetUserRoleByAccessCode(currentUserDetail.PageAccessCode);
            if (role.Id == Convert.ToInt16(RoleId))
            {
                role = _IGeneralService.GetUserRoleByAccessCode("ADMIN");
                RoleId = Convert.ToString(role.Id);
                RoleName = Convert.ToString(role.Text);
            }
            if (!string.IsNullOrEmpty(RoleId))
            {
                Session["roleId"] = RoleId;
                Session["RoleName"] = RoleName;
            }
            else
            {
                if (Session["roleId"] != null)
                    RoleId = (string)Session["roleId"];
                if (Session["RoleName"] != null)
                    RoleName = (string)Session["RoleName"];
            }



            GetMenuMappingListHtmlWithCheckBoxSelection();
            var localModel = new MenuMappingLocalModel();

            if (Session["menus"] != null)
            {
                localModel.Menus = (string)(Session["menus"]);
            }
            
            localModel.User_Level_ID = Convert.ToInt16(role.Id);          
            ViewBag.RoleId = RoleId;
             ViewBag.RoleName = RoleName;
             Session["BackUrl"] = "RedirectToHomePageMyTask";
            return View("MenuMapping",localModel);
        }

        //public ActionResult MenuBind()
        //{
        //    MenuMappingLocalModel localModel = new MenuMappingLocalModel();      
        //    localModel.User_Level_ID = Convert.ToInt16(Session["roleId"]);
        //    localModel.Menus = (string)(Session["menus"]);
        //    ViewBag.RoleId = Session["roleId"];
        //    ViewBag.RoleName = Session["RoleName"];
        //    //Session["CancelBtn"] = "ManageRole";
        //    return View("MenuMapping",localModel);

        //}
        /// <summary>
        /// Menu Mapping on the basis of User 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MenuMapping(string id)
        {
            var localModel = new MenuMappingLocalModel();
            localModel = Menu(id);
            return View(localModel);
            //return Json(localModel, JsonRequestBehavior.AllowGet);
           // return RedirectToAction("MenuBind");
        }
        public JsonResult MenuBind(string id)
        {
            var localModel = new MenuMappingLocalModel();
            localModel = Menu(id);
            return Json(localModel, JsonRequestBehavior.AllowGet);
        }
        public MenuMappingLocalModel Menu(string id)
        {
           
            if (id != null)
            {
                RoleId = id.Split(',').First();
                RoleName = id.Split(',').Last();

            }
            var currentUserDetail = new CurrentUserDetail();
            currentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            if (!string.IsNullOrEmpty(RoleId))
            {
                Session["roleId"] = RoleId;
                Session["RoleName"] = RoleName;
            }
            else
            {
                if (Session["roleId"] != null)
                    RoleId = (string)Session["roleId"];
                if (Session["RoleName"] != null)
                    RoleName = (string)Session["RoleName"];
            }
            ////For Super Admin Only
            //role = new DtoList();
            //role = _IGeneralService.GetUserRoleByAccessCode(currentUserDetail.PageAccessCode);
            //if (RoleId != "")
            //{
            //    if (role.Id == Convert.ToInt16(RoleId))
            //    {
            //        role = _IGeneralService.GetUserRoleByAccessCode("ADMIN");
            //        RoleId = Convert.ToString(role.Id);
            //        RoleName = Convert.ToString(role.Text);
            //        Session["roleId"] = RoleId;
            //        Session["RoleName"] = RoleName;
            //    }
            //}
            //End

            GetMenuMappingListHtmlWithCheckBoxSelection();
            var localModel = new MenuMappingLocalModel();

            if (Session["menus"] != null)
            {
                localModel.Menus = (string)(Session["menus"]);
            }

            ViewBag.RoleId = RoleId;
            ViewBag.RoleName = RoleName;
            localModel.User_Level_ID = Convert.ToInt16(RoleId);
            localModel.RoleName = RoleName;
            return localModel;
        }
        private void GetMenuMappingListHtmlWithCheckBoxSelection()
        {
            try
            {
                var clientUserName = string.Empty;

                _allmenus = _IMenuMappingService.GetAllMenuForRole(Convert.ToInt32((string)Session["roleId"]));
                Session["MenuList"] = _allmenus;
            }
            catch (Exception ex)
            {
                throw;
            }

            #region Parent Table Declaration

            var parentTable = new Table { ID = "MenuAndFeatureTable", CssClass = "MenuAndFeatureTable" };
            var parentTableRow = new TableRow();
            var parentTableRowCell = new TableCell { CssClass = "MenuAndFeatureTableTd" };


            #endregion

            #region Menus

            IEnumerable<MenuModel> sublistOfMenus = (from a in _allmenus
                                                     where a.MenuType.ToUpperInvariant().Contains("Menu".ToUpperInvariant())
                                                                       select a).ToList();

            #region Menu Table Declaration

            // Table for Menus 
            var containerTableForMenus = new Table();
            containerTableForMenus.CssClass = "MenuTable";
            var containerTableRowForMenus = new TableRow();
            var containerTableCellForMenus = new TableCell();

            #endregion

            #region Menu Table Header Creation

            // Menu Header -- To Display Header Row
            var headerForMenuTable = new Label { Text = "Menus" };
            var containerTableHeaderCellForMenus = new TableHeaderCell();
            containerTableHeaderCellForMenus.Controls.Add(headerForMenuTable);
            var containerTableHeaderRowForMenus = new TableHeaderRow();
            containerTableHeaderRowForMenus.Cells.Add(containerTableHeaderCellForMenus);
            containerTableForMenus.Rows.Add(containerTableHeaderRowForMenus);

            #endregion

            #region Create Menu Table Rows

            if (sublistOfMenus.Any())
            {
                string htmLofMenuList = BuildMenuHtmlList(sublistOfMenus, 0);
                var listOfMenus = new Literal();
                listOfMenus.Text = htmLofMenuList;
                containerTableCellForMenus.Controls.Add(listOfMenus);
                containerTableRowForMenus.Cells.Add(containerTableCellForMenus);
                containerTableForMenus.Rows.Add(containerTableRowForMenus);

                var obj = new MenuModel { MenuItemsHtml = htmLofMenuList };
                Session["menus"] = obj.MenuItemsHtml;
            }

            #endregion

            #endregion

            #region Append to Parent Table

            parentTableRowCell.Controls.Add(containerTableForMenus);
            parentTableRow.Cells.Add(parentTableRowCell);
            parentTable.Rows.Add(parentTableRow);
            parentTableRowCell = new TableCell();
            parentTableRow = new TableRow();

            parentTableRow.Cells.Add(parentTableRowCell);
            parentTableRow.Width = 100;
            parentTable.Rows.Add(parentTableRow);

            #endregion

            #region Table Width Adjustment

            // Width Adjustment 
            parentTable.Width = Unit.Percentage(100);

            #endregion

            var htmlForMenuListMapping = new PlaceHolder();
            htmlForMenuListMapping.Controls.Clear();
            htmlForMenuListMapping.Controls.Add(parentTable);
            htmlForMenuListMapping.Visible = true;
        }
        private string BuildMenuHtmlList(IEnumerable<MenuModel> sublistOfMenus, decimal menuKey)
        {
            // Loop through each Menu Item in the list of Menus
            var ofMenus = sublistOfMenus as IList<MenuModel> ?? sublistOfMenus.ToList();
            IEnumerable<MenuModel> sublistOfTopMenus = (from c in ofMenus
                                                                          where c.ParentMenuId == menuKey
                                                                          select c).ToList();

            var subsublistmenus = new List<MenuModel>();
            var htmLofMenuList = "<ul class=\"manage-users-menus\"><div class=\"clear-floats\"></div>";
            
            foreach (MenuModel menusTopListItem in sublistOfTopMenus)
            {
                string floatValue = "left";
                string clearValue = "inherit";
                if (menusTopListItem.MenuId == 35)
                {
                    floatValue = "right";
                } 
                if (menusTopListItem.MenuId == 22)
                {
                    clearValue = "left";
                }

                IEnumerable<MenuModel> sublistOfChildMenus = (from e in ofMenus
                                                                                where e.ParentMenuId == menusTopListItem.MenuId
                                                                                select e).ToList();


                htmLofMenuList += "<li style=\"float: " + floatValue + "; clear: " + clearValue + "; width: 25%; padding: 1%; box-sizing: border-box;\"><input class='headerCheckBox' name='" + "chkBox" +
                                  menusTopListItem.MenuId.ToString() + "' id='" + "chkBox" +
                                  menusTopListItem.MenuId.ToString() + "' type='checkbox' value='" +
                                  menusTopListItem.MenuId.ToString() + "' type='checkbox' value='" +
                                  menusTopListItem.MenuId.ToString();
                htmLofMenuList += "' " + (menusTopListItem.IsSelected ? "checked='checked'" : "") + " />";
                if (menusTopListItem.MenuType.Contains("Menu"))
                {
                    htmLofMenuList += "<label style=\"font-size:medium;color:#7c0000\">" + menusTopListItem.MenuName + "</label>";
                }
                if (menusTopListItem.MenuType.Contains("SubMenu"))
                {
                    htmLofMenuList += "<label style=\"font-size:normal\">" + menusTopListItem.MenuName + "</label>";
                }
                if (menusTopListItem.MenuType.Contains("SubToSubMenu"))
                {
                    htmLofMenuList += "<label style=\"font-size:small\">" + menusTopListItem.MenuName + "</label>";
                }
                if (sublistOfChildMenus.Any())
                {
                    htmLofMenuList += CreateCheckBoxRowsForSubMenuItems(sublistOfChildMenus);
                }


                htmLofMenuList += "</li>";
            }
            htmLofMenuList += "</ul>";


            return htmLofMenuList;
        }
        //private string BuildMenuHtmlList(IEnumerable<MenuModel> sublistOfMenus, decimal menuKey)
        //{
        //    // Loop through each Menu Item in the list of Menus
        //    var ofMenus = sublistOfMenus as IList<MenuModel> ?? sublistOfMenus.ToList();
        //    IEnumerable<MenuModel> sublistOfTopMenus = (from c in ofMenus
        //                                                where c.ParentMenuId == menuKey
        //                                                select c).ToList();

        //    var subsublistmenus = new List<MenuModel>();
        //    var htmLofMenuList = "<ul class=\"manage-users-menus\"><div class=\"clear-floats\"></div>";

        //    foreach (MenuModel menusTopListItem in sublistOfTopMenus)
        //    {
        //        IEnumerable<MenuModel> sublistOfChildMenus = (from e in ofMenus
        //                                                      where e.ParentMenuId == menusTopListItem.MenuId
        //                                                      select e).ToList();


        //        htmLofMenuList += "<li><input class='headerCheckBox' name='" + "chkBox" +
        //                          menusTopListItem.MenuId.ToString() + "' id='" + "chkBox" +
        //                          menusTopListItem.MenuId.ToString() + "' type='checkbox' value='" +
        //                          menusTopListItem.MenuId.ToString() + "' type='checkbox' value='" +
        //                          menusTopListItem.MenuId.ToString();
        //        htmLofMenuList += "' " + (menusTopListItem.IsSelected ? "checked='checked'" : "") + " />";
        //        if (menusTopListItem.MenuType.Contains("Menu"))
        //        {
        //            htmLofMenuList += "<label style=\"font-size:medium;color:#7c0000\">" + menusTopListItem.MenuName + "</label>";
        //        }
        //        if (menusTopListItem.MenuType.Contains("SubMenu"))
        //        {
        //            htmLofMenuList += "<label style=\"font-size:normal\">" + menusTopListItem.MenuName + "</label>";
        //        }
        //        if (menusTopListItem.MenuType.Contains("SubToSubMenu"))
        //        {
        //            htmLofMenuList += "<label style=\"font-size:small\">" + menusTopListItem.MenuName + "</label>";
        //        }
        //        if (sublistOfChildMenus.Any())
        //        {
        //            htmLofMenuList += CreateCheckBoxRowsForSubMenuItems(sublistOfChildMenus);
        //        }


        //        htmLofMenuList += "</li>";
        //    }
        //    htmLofMenuList += "</ul>";


        //    return htmLofMenuList;
        //}
        /// <summary>
        /// Create Check Boxes
        /// </summary>
        /// <param name="ListOfSubItems"></param>
        /// <returns></returns>
        private string CreateCheckBoxRowsForSubMenuItems(IEnumerable<MenuModel> ListOfSubItems)
        {
            string htmLofMenuList = "<ul>";

            foreach (MenuModel subMenuListItem in ListOfSubItems)
            {
                htmLofMenuList += "<li class='parent'>";
                htmLofMenuList += "<div class='subMenudiv'><input class='subMenuFeatureCheckBox' name='" + "chkBox" +
                                  subMenuListItem.MenuId.ToString(CultureInfo.InvariantCulture) + "' id='" + "chkBox" +
                                  subMenuListItem.MenuId.ToString(CultureInfo.InvariantCulture) + "' type='checkbox' value='" +
                                  subMenuListItem.MenuId.ToString(CultureInfo.InvariantCulture);
                htmLofMenuList += "' " + (subMenuListItem.IsSelected ? "checked='checked'" : "") + " /></div>";
                htmLofMenuList += "<div class='labeldiv'><label>" + subMenuListItem.MenuName + "</label></div>";


                var item = subMenuListItem;
                IEnumerable<MenuModel> subsublistmenus = (from e in _allmenus
                                                                            where e.ParentMenuId == item.MenuId
                                                                            select e).ToList();
                if (subsublistmenus.Any())
                {
                    htmLofMenuList += CreateCheckBoxRowsForSubMenuItems(subsublistmenus);
                }

                htmLofMenuList += "</li>";
            }

            htmLofMenuList += "</ul>";

            return htmLofMenuList;
        }
        /// <summary>
        /// Save Menus
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveMenuMapping(FormCollection form)
        {
            var cntElements = 0;
            var count = 0;
            var model = new MenuMappingLocalModel();
            var visibilityProvider = new AuthenticatedVisibilityProvider();
            User_LevelModel currentUserRole = new User_LevelModel();

            if (Session["roleId"] != null)
            {
                RoleId = (string)(Session["roleId"]);
            }
            if (Session["Roles"] != null)
            {
                currentUserRole = (User_LevelModel)(Session["Roles"]);
            }
            if (Session["RoleName"] != null)
            {
                ViewBag.RoleName = (string)(Session["RoleName"]);
            }

            if (Session["MenuList"] != null)
            {
                var userRoleMenusAndFeatures = (IEnumerable<MenuModel>)Session["MenuList"];
                 foreach (var menuOrFeature in userRoleMenusAndFeatures)
                {
                    if (
                        !(Request.Form["chkBox" + userRoleMenusAndFeatures.ElementAt(cntElements).MenuId.ToString()] !=
                          null))
                        userRoleMenusAndFeatures.ElementAt(cntElements).IsSelected = false;
                    else
                        userRoleMenusAndFeatures.ElementAt(cntElements).IsSelected = true;

                    cntElements++;
                }


                if (Session["roleId"] != null)
                {
                    model.RoleId = (string)(Session["roleId"]);
                }


                model.AllMenus = userRoleMenusAndFeatures.ToList();
                foreach (var menu in model.AllMenus)
                {
                    if (menu.IsSelected)
                    {
                        count++;
                    }
                }

                _IMenuMappingService.SetMenuSecurityForRole(model.AllMenus, model.RoleId);
                Session["allMenudIdsForRoleUser"] = null;
                TempData["isSuccess"] = true;
                // SuccessNotification("Records have been saved successfully.");
                visibilityProvider.GetMenuIdsForUserName();
                MvcSiteMapProvider.SiteMaps.ReleaseSiteMap(); //Refreshing the MvcSiteMapProvider cache in case there is any change in menu mapping


                //if (count > 0)
                //{
                //    _IMenuMappingService.SetMenuSecurityForRole(model.AllMenus, model.RoleId);
                //    Session["allMenudIdsForRoleUser"] = null;
                //    TempData["isSuccess"] = true;
                //    // SuccessNotification("Records have been saved successfully.");
                //    visibilityProvider.GetMenuIdsForUserName();
                //    MvcSiteMapProvider.SiteMaps.ReleaseSiteMap(); //Refreshing the MvcSiteMapProvider cache in case there is any change in menu mapping
                //}
                //else
                //{
                //    TempData["isFailure"] = true;
                //}
            }
            //GetMenuMappingListHtmlWithCheckBoxSelection();
            //   model.AllMenus = MenuMappingService.GetallMenusForRole(Convert.ToInt32((string)(Session["roleId"])));
            //return RedirectToAction("MenuBind");
            return RedirectToAction("MenuMapping");
        }

        #endregion
       

    }
}