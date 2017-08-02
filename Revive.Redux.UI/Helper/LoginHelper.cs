using Revive.Redux.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using Revive.Redux.Services;
using System.Web.Routing;
using System.Web.Mvc;

namespace Revive.Redux.UI
{
    public class LoginHelper
    {
        public bool LoginHelperInitialize()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (authCookie == null)
                return false;
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var customMemberShip = new CustomMemberShip();
            if (authTicket != null)
            {
                var userRecord = customMemberShip.GetUserDetails(authTicket.Name);


                if (userRecord.status != null && (!userRecord.IsLockedOut && userRecord.status.Value))
                {
                    var currentUserDetail = new CurrentUserDetail();
                    currentUserDetail.User_Id = userRecord.User_ID;
                    currentUserDetail.FirstName = userRecord.FirstName;
                    currentUserDetail.LastName = userRecord.LastName;
                    currentUserDetail.Email_Id = authTicket.Name;
                    currentUserDetail.FullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                    currentUserDetail.IsCustomer = true; // To Do :
                    currentUserDetail.Customer_Id = userRecord.Customer_ID; 
                    var serializer = new JavaScriptSerializer();
                    string userData = serializer.Serialize(currentUserDetail);

                    IRoleManagementService _RoleManageServive = new RoleManagementService();
                    HttpContext.Current.Session["CurrentUser"] = currentUserDetail;
                    HttpContext.Current.Session["Roles"] = _RoleManageServive.GetRoleForUser(authTicket.Name);
                    return true;

                }
                else
                {
                    SignOut();
                    //redirect to login page
                    var virtualPathData =
                        RouteTable.Routes.GetVirtualPath(((MvcHandler)HttpContext.Current.CurrentHandler).RequestContext,
                            new RouteValueDictionary(
                                new { controller = "Account", action = "Login", id = "" }));
                    if (virtualPathData !=
                        null)
                    {
                        string url = virtualPathData.VirtualPath;
                        HttpContext.Current.Response.Redirect(url);
                        return false;
                    }
                }
            }
            return false;

        }
        public void SignOut()
        {
            HttpContext.Current.Session["allMenudIdsForRoleUser"] = null;
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);
        }
    }
}