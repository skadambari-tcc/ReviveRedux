using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using Revive.Redux.Entities;
using System.Web.Mvc;
using Revive.Redux.Services;


namespace Revive.Redux.UI.Filters
{
    public class ValidateUserRoleMenus : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext == null || filterContext.HttpContext == null)
                return;
            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;
            string actionName = string.Empty;
            string controllerName = string.Empty;
            if (filterContext.RouteData.Values["controller"] != null &&
                filterContext.RouteData.Values["action"] != null &&
                filterContext.RouteData.Values["controller"].ToString().Trim().Length > 0 &&
                filterContext.RouteData.Values["action"].ToString().Trim().Length > 0)
            {
                controllerName = filterContext.RouteData.Values["controller"].ToString();
                actionName = filterContext.RouteData.Values["action"].ToString();
            }
            else
            {
                return;
            }


            List<int> allMenusForRole = new List<int>();

            User_LevelModel role = null;
            if (HttpContext.Current.Session["allMenudIdsForRoleUser"] != null)
            {
                allMenusForRole = (List<int>)(HttpContext.Current.Session["allMenudIdsForRoleUser"]);
            }
            else
            {
                allMenusForRole = GetCurrentUserRoleIds();
            }
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.Current.Session["Roles"] != null)
                {
                    role = (User_LevelModel)(HttpContext.Current.Session["Roles"]);
                }

                var nodeColl = ChildrenOf(MvcSiteMapProvider.SiteMaps.Current.RootNode);
                bool unAuthorize = false;
                if (role != null)
                {
                    foreach (var node in nodeColl)
                    {
                        //if the controller and action is not part of XML sitemap do not use role based security
                        if (node.Controller.ToLower().Trim() == controllerName.ToLower().Trim() &&
                            node.Action.ToLower().Trim() == actionName.ToLower().Trim())
                        {
                            int id = Convert.ToInt32(node.Attributes["id"]);
                            if (allMenusForRole.Contains(id))
                            {
                                unAuthorize = false;
                                break;
                            }
                            else
                            {
                                unAuthorize = true;
                            }
                        }
                    }

                    if (unAuthorize)
                    {
                        ViewResult result = new ViewResult { ViewName = "SecurityError" };
                        result.ViewBag.ErrorMessage =
                            "You are not authorized to view this page. Please contact administrator!";
                        filterContext.Result = result;
                    }
                }
                else
                {
                    ViewResult result = new ViewResult { ViewName = "SecurityError" };
                    result.ViewBag.ErrorMessage =
                        "You are not authorized to view this page. Please contact administrator!";
                    filterContext.Result = result;
                }

            }
        }

        private List<int> GetCurrentUserRoleIds()
        {

            IMenuMappingService _objRoleService = new MenuMappingService();
             return _objRoleService.GetallMenuIdsForUser(HttpContext.Current.User.Identity.Name);
        }

        public IEnumerable<ISiteMapNode> ChildrenOf(ISiteMapNode root)
        {
            yield return root;
            foreach (var c in root.ChildNodes)
                foreach (var cc in ChildrenOf(c))
                    yield return cc;
        }

    }
}