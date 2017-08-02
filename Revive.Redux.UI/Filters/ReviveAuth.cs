using MvcSiteMapProvider;
using Revive.Redux.Common;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;


namespace Revive.Redux.UI
{

    public class ReviveAuth : System.Web.Mvc.AuthorizeAttribute
    {

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    //custom authentication logic
        //    if (filterContext == null)
        //    {
        //        throw new ArgumentNullException("filterContext");
        //    }

        //    if (AuthorizeCore(filterContext.HttpContext) && filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
        //        cachePolicy.SetProxyMaxAge(new TimeSpan(0));
        //        cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
        //    }
        //    else
        //    {
        //        HandleUnauthorizedRequest(filterContext);
        //    }
        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = (CurrentUserDetail)HttpContext.Current.Session["CurrentUser"];

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
            //Role role = null;
            if (HttpContext.Current.Session["allMenudIdsForRoleUser"] != null)
            {
                allMenusForRole = (List<int>)(HttpContext.Current.Session["allMenudIdsForRoleUser"]);
            }
            else
            {
                if (user != null)
                {
                    IMenuMappingService menuService = new MenuMappingService();

                    allMenusForRole = menuService.GetallMenuIdsForRole(user.UserLevelId);
                }
            }
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //if (HttpContext.Current.Session["Roles"] != null)
                //{
                //    role = (Role)(HttpContext.Current.Session["Roles"]);
                //}

                var nodeColl = ChildrenOf(MvcSiteMapProvider.SiteMaps.Current.RootNode);
                bool unAuthorize = false;
                if (user.UserLevelId > 0)
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
                        HandleUnauthorizedRequest(filterContext);

                    }
                }
                else
                {
                    HandleUnauthorizedRequest(filterContext);
                }

            }
        }
        public IEnumerable<ISiteMapNode> ChildrenOf(ISiteMapNode root)
        {
            yield return root;
            foreach (var c in root.ChildNodes)
                foreach (var cc in ChildrenOf(c))
                    yield return cc;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Auth" }));
        }
    }



}
