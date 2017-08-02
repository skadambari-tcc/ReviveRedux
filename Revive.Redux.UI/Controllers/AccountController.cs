using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Revive.Redux.UI;
using Revive.Redux.Services;
using Revive.Redux.Common;
using Revive.Redux.Controllers.Common;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web.Hosting;
using System.Collections;
using Revive.Redux.UI.Filters;
using System.Web.Configuration;
using ComponentSpace.SAML2;
using System.Web.Configuration;
using System.Collections.Generic;


namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        ILoginService LoginService = null;
        IRoleManagementService roleManagementService = null;
        private ICustomerManagement _ICustomerManagement = null;
        private ILogService logService = null;
        public const string AttributesSessionKey = "";

        public AccountController()
        {
            LoginService = new LoginService();
            _ICustomerManagement = new CustomerManagement();
            roleManagementService = new RoleManagementService();
            logService = new LogService();


        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //Session["CurrentUser"] = null;
            ViewBag.ReturnUrl = returnUrl;
            if (Session["CurrentUser"] != null && Session["allMenudIdsForRoleUser"] != null)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                if (Session["TempFrgtMsg"] != null)
                {
                    TempData["isForgotPwdSuccess"] = true;
                    Session["TempFrgtMsg"] = null;
                }
                Session["isForgotPwdSuccess"] = null;
                KillSession();
                return View();
            }
        }

        private void KillSession()
        {
            Session["allMenudIdsForRoleUser"] = null;
            Session["CurrentUser"] = null;
            Session.Abandon();

        }
        [AllowAnonymous]
        public JsonResult CheckEmailAddress(String Email)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                flag = validateLogin.CheckEmailAddressExists(Email);
                // ModelState.AddModelError("UserIdExistOrNot" ,"Username not exists.");
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckEmailAddress", ex);
                throw;
            }
            return Json(flag);
        }

        [AllowAnonymous]
        public JsonResult CheckEmailAddressRegistation(String Email)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                flag = !validateLogin.CheckEmailAddressExists(Email);
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckEmailAddressRegistation", ex);
                throw;
            }
            return Json(flag);
        }

        [AllowAnonymous]
        public JsonResult CheckEmailAddressEdit(String emailEdit, Guid? UserId)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                flag = !validateLogin.CheckDuplicateEmailByUserId(emailEdit, (System.Guid)UserId);
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckEmailAddressRegistation", ex);
                throw;
            }
            return Json(flag);
        }



        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            int attempts = 0;
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                Session["allMenudIdsForRoleUser"] = null;
                var customMemberShip = new CustomMemberShip();
                // Check Lock user 
                var IslockUser = false;
                IslockUser = customMemberShip.ValidateLockUser(model.Email);

                if (IslockUser == true)
                {
                    ModelState.AddModelError("ErrorMessage", "Your Account has been locked.You can re-generate the password by click on forgot password link. ");
                    return View(model);

                }
                var IsStoreUser = false;
                IsStoreUser = customMemberShip.ValidateStoreUserByUserName(model.Email);

                if (IsStoreUser == true)
                {
                    ModelState.AddModelError("ErrorMessage", "Store user can not login into Web portal.");
                    return View(model);
                }

                // Check Lock user 

                var flag = Membership.ValidateUser(model.Email, model.Password.ToMD5HashForPassword());
                if (flag)
                {
                    Session["AttemptCount"] = null;

                    var userRecord = customMemberShip.GetUserDetails(model.Email);

                    if (userRecord.status != null && (!userRecord.IsLockedOut && userRecord.status.Value))
                    {
                        var currentUserDetail = new CurrentUserDetail();
                        currentUserDetail.User_Id = userRecord.User_ID;
                        currentUserDetail.FirstName = userRecord.FirstName;
                        currentUserDetail.LastName = userRecord.LastName;
                        currentUserDetail.Email_Id = model.Email;
                        currentUserDetail.FullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                        currentUserDetail.UserLevelId = userRecord.User_Level_ID;
                        currentUserDetail.Customer_Id = userRecord.Customer_ID;
                        currentUserDetail.PageAccessCode = userRecord.PageAccessCode;
                        currentUserDetail.LastLoginActivity = userRecord.LastLoginActivity;
                        currentUserDetail.userLevelName = userRecord.User_Level_Name;
                        currentUserDetail.Manufacturer_Id = Convert.ToInt32(userRecord.Manufacturer_Id);
                        currentUserDetail.SubsidiaryId = userRecord.SubsidiaryId;
                        currentUserDetail.SubAgentId = userRecord.SubAgentId;
                        currentUserDetail.Manufacturer_Ref_Id = userRecord.Manufacturer_Ref_Id;
                        //currentUserDetail.IsCustomer = userRecord.RoleId != 1; // To Do :
                        //currentUserDetail.TierId = userRecord.TierId;
                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(currentUserDetail);

                        Session["CurrentUser"] = currentUserDetail;
                        Session["Roles"] = roleManagementService.GetRoleForUser(model.Email);

                        //var roles = roleManagementService.GetAuthUserRole(roleManagementService.GetRoleForUser(model.Email).User_Level_ID);
                        //Session["Controller_Role"] = roles;

                        var authTicket = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), model.RememberMe, userData, FormsAuthentication.FormsCookiePath);

                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);


                        Response.Cookies.Add(faCookie);
                        //await SignInAsync(user, model.RememberMe);
                        if (userRecord.LastPasswordChangeDate.HasValue)
                            return RedirectToLocal(returnUrl);
                        else
                            return RedirectToAction("ChangePassword");
                    }
                    else
                    {
                        if (userRecord.IsLockedOut)
                            ModelState.AddModelError("ErrorMessage", "Your Account has been locked.");
                        if (userRecord.status != null && !userRecord.status.Value)
                            ModelState.AddModelError("ErrorMessage", "Account deactivated. Please contact Redux at " + Convert.ToString(WebConfigurationManager.AppSettings["ReviveContactNum"]) + " for more information.");
                    }
                }
                else
                {

                    if (Session["AttemptCount"] == null)
                    {
                        Session["AttemptCount"] = 1;
                    }
                    else
                    {
                        //ModelState.Clear();
                        attempts = (Int32)Session["AttemptCount"];

                        if (attempts == 5)
                        {
                            attempts = 5;
                        }
                        else
                        {
                            ++attempts;
                        }
                        if (attempts == 4)
                        {
                            ModelState.AddModelError("ErrorMessage", "This is your last attempt to enter the password.");
                        }
                        if (attempts == 5)
                        {
                            attempts = 5;
                            var objCustomMembership = new CustomMemberShip();
                            var lockUser = objCustomMembership.LockUser(model.Email);
                            if (lockUser == true)
                            {
                                Session["AttemptCount"] = null;
                                return RedirectToAction("ForgotPassword");
                            }
                            else
                            {
                                ModelState.AddModelError("ErrorMessage", "Your Account has been locked, Please Contact to Redux Admin.");
                            }
                        }
                        Session["AttemptCount"] = attempts;

                    }
                    ModelState.AddModelError("ErrorMessage", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult LogOff()
        {
            Session["allMenudIdsForRoleUser"] = null;
            Session["CurrentUser"] = null;
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            return RedirectToAction("Login", "Account");
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var flag = false;
            var userFullName = String.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Name = model.Email;
                    var passwordText = CommonMethods.GetRandomPassword();
                    var validateLogin = new CustomMemberShip();
                    flag = validateLogin.ChangePassword(model.Email, "", passwordText.ToMD5HashForPassword());

                    if (flag)
                    {
                        var userRecord = validateLogin.GetUserDetails(model.Email);
                        if (userRecord.LastName != null)
                        {
                            userFullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                        }
                        else
                        {
                            userFullName = userRecord.FirstName;
                        }

                        var emailLink = ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString();
                        var templateVars = new Hashtable();
                        templateVars.Add("LoginURL", emailLink);
                        templateVars.Add("Password", passwordText);
                        //Updated changes for bug #6128
                        templateVars.Add("UserFullName", userFullName);
                        var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ForgotPassword.html"), templateVars);
                        var emailBody = parser.Parse(); // string.Format(ConstantEntities.NewCustomerEmailBody, emailLink, customer.Email, pasword);
                        EmailHelper emailHelper = new EmailHelper();

                        emailHelper.Send(model.Email, string.Empty, Convert.ToString(ConfigurationManager.AppSettings["EmailBcc"]), "Redux - Forgot Password", emailBody);
                        Session["TempFrgtMsg"] = "Password has been sent to your registered email address successfully.";

                    }

                }

            }
            catch (Exception ex)
            {

                throw;


            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Login", "Account");
        }


        public ActionResult ChangePassword(String message)
        {
            ViewData["Message"] = message;
            return View();
        }




        public ActionResult UpdatePassword(ManageUserViewModel passwordModel)
        {
            var flag = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var validateLogin = new CustomMemberShip();
                    if (Session["CurrentUser"] != null)
                    {
                        var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                        flag = validateLogin.ChangePassword(currentUser.Email_Id, passwordModel.OldPassword, passwordModel.NewPassword.ToMD5HashForPassword());
                        if (flag)
                        {
                            Session["Tempmsg"] = "[[[Password has been changed successfully]]]";
                            TempData["PasswordChanged"] = true;
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in UpdatePassword", ex);
                throw;
            }

            return View("ChangePassword", passwordModel);
            //return RedirectToAction("Index", "Home");
        }

        public ActionResult SkipPassword()
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                if (Session["CurrentUser"] != null)
                {
                    var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                    flag = validateLogin.ChangePassword(currentUser.Email_Id, "SKIP", "");
                    if (flag)
                        Session["Tempmsg"] = "[[[Password has been changed successfully]]]";
                }
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                // logService.LogError("Error in UpdatePassword", ex);
                throw;
            }

            //return View("ChangePassword", passwordModel);
            //return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public JsonResult CheckOldPassword(String OldPassword)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                if (Session["CurrentUser"] != null)
                {
                    var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                    var userRecord = validateLogin.GetUserDetails(currentUser.Email_Id);
                    if (userRecord.Password == OldPassword.ToMD5HashForPassword())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckOldPassword", ex);
                throw;
            }
            return Json(flag);
        }
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }


        #region SSO implementation
        [AllowAnonymous]
        public ActionResult SingleSignOn()
        {
            // To login at the service provider, initiate single sign-on to the identity provider (SP-initiated SSO).

           
            string partnerIdP = ConfigurationManager.AppSettings["PartnerIdP"].ToString();
            SAMLServiceProvider.InitiateSSO(Response, null, partnerIdP);

            return new EmptyResult();
        }
        //[AllowAnonymous]
        //public ActionResult SingleSignOn()
        //{
        //    return View();
        //}

        


        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}