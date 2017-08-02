using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Revive.Redux.Models;
using System.Web.Mvc.Html;
using MvcSiteMapProvider.Reflection;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Services;


namespace Revive.Redux.Helper
{
    public class AuthenticatedVisibilityProvider : SiteMapNodeVisibilityProviderBase
    {

        IMenuMappingService menuMappingService { get; set; }

        public AuthenticatedVisibilityProvider()
        {
            this.menuMappingService = new MenuMappingService();
        }
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
          //  return IsMenuActiveForUser(node);
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                if (sourceMetadata.ContainsKey("HtmlHelper") &&
                    sourceMetadata["HtmlHelper"].ToString().Equals("MvcSiteMapProvider.Web.Html.MenuHelper"))
                {
                    if (node.Attributes.ContainsKey("visibility") && node.Attributes["visibility"] != null && (string)node.Attributes["visibility"] == "false")
                    {
                        return false;
                    }
                }
                return IsMenuActiveForUser(node);
            }
            else
                return false;
        }

       
        public bool IsMenuActiveForUser(ISiteMapNode node)
        {
            // return true;
            List<int> allMenudIdsForRoleUser = GetMenuIdsForUserName();
            var menuId = Convert.ToInt32(node.Attributes["id"]);
            return allMenudIdsForRoleUser.Contains(menuId);
        }


        public List<int> GetMenuIdsForUserName()
        {
            var allMenudIdsForRoleUser = new List<int>();
            
            if (HttpContext.Current.Session["CurrentUser"] != null)
            {
                var user = (CurrentUserDetail)HttpContext.Current.Session["CurrentUser"];
                
                //if (user.UserLevelId == 1)
                //{
                //    allMenudIdsForRoleUser.Add(2);
                //    //allMenudIdsForRoleUser.Add();
                //}
                //else
                //{
                //    allMenudIdsForRoleUser.Add(1);
                //    allMenudIdsForRoleUser.Add(2);
                //}
                if (HttpContext.Current.Session["allMenudIdsForRoleUser"] == null)
                {
                    var currentInfo = (CurrentUserDetail)HttpContext.Current.Session["CurrentUser"];

                    allMenudIdsForRoleUser = menuMappingService.GetallMenuIdsForRole(user.UserLevelId);

                    HttpContext.Current.Session["allMenudIdsForRoleUser"] = allMenudIdsForRoleUser;
                }
                else
                {
                    if (HttpContext.Current.Session["allMenudIdsForRoleUser"] != null)
                        allMenudIdsForRoleUser = (List<int>)HttpContext.Current.Session["allMenudIdsForRoleUser"];
                }
                //if()
            }

            return allMenudIdsForRoleUser;
        }
    }
}
