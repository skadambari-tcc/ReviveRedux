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
using Revive.Redux.Models;
using Revive.Redux.Services;
using Revive.Redux.Common;
using Revive.Redux.Controllers;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web.Hosting;
using System.Collections;

namespace Revive.Redux.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ILoginService LoginService = null;
        IRoleManagementService roleManagementService = null;
        private ILogService logService = null;

        public AccountController()
        {
            LoginService = new LoginService();
            roleManagementService = new RoleManagementService();
            logService = new LogService();


        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if (Session["allMenudIdsForRoleUser"] != null && Session["CurrentUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                if (Session["TempFrgtMsg"] != null)
                {
                    TempData["isForgotPwdSuccess"] = true;
                    Session["TempFrgtMsg"] = null;
                }
                Session["isForgotPwdSuccess"] = null;

                return View();
            }
        }


       [AllowAnonymous]
        public JsonResult CheckEmailAddress(String Email)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                flag = validateLogin.CheckEmailAddressExists(Email);
            }
            catch (Exception ex)
            {
                //logService.LogError("Error in CheckEmailAddress", ex);
            }
            return Json(flag);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                Session["allMenudIdsForRoleUser"] = null;
                var flag = Membership.ValidateUser(model.Email, model.Password.ToMD5HashForPassword());
                if (flag)
                {
                    Session["AttemptCount"] = null;
                    var customMemberShip = new CustomMemberShip();
                    var userRecord = customMemberShip.GetUserDetails(model.Email);

                    if (userRecord.status != null && (!userRecord.IsLockedOut && userRecord.status.Value))
                    {
                        var currentUserDetail = new CurrentUserDetail();
                        currentUserDetail.UserId = userRecord.User_ID;
                        currentUserDetail.FirstName = userRecord.FirstName;
                        currentUserDetail.LastName = userRecord.LastName;
                        currentUserDetail.Email = model.Email;
                        currentUserDetail.FullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                        currentUserDetail.UserLevelId = userRecord.User_Level_ID;
                        //currentUserDetail.IsCustomer = userRecord.RoleId != 1; // To Do :
                        //currentUserDetail.TierId = userRecord.TierId;
                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(currentUserDetail);

                        Session["CurrentUser"] = currentUserDetail;
                        Session["Roles"] = roleManagementService.GetRoleForUser(model.Email);
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
                            ModelState.AddModelError("ErrorMessage", "Your Account has been locked, Please Contact to Administration.");
                        if (userRecord.status != null && !userRecord.status.Value)
                            ModelState.AddModelError("ErrorMessage", "User is inactive.");
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
                        var attempts = (Int32)Session["AttemptCount"];

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
                            var customMemberShip = new CustomMemberShip();
                            customMemberShip.LockUser(model.Email);
                            ModelState.AddModelError("ErrorMessage", "Your Account has been locked, Please Contact to Administration.");
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
            return View(new ForgotPasswordViewModel());
        }

        //
        // POST: /Account/ForgotPassword
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendForgotPassword(ForgotPasswordViewModel model)
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
                        //send email with email queqe
                        //mailer.Send(model.Email, string.Empty, ConstantEntities.ForgotPasswordEmailSubject, emailBody);
                        emailHelper.Send(model.Email, "test", emailBody);
                        Session["TempFrgtMsg"] = "Password has been sent to your email address successfully";

                    }
                    return RedirectToAction("Login", "Account");
                }
                return View("ForgotPassword", model);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Login", "Account");


            }
            //return View("Login");

            // If we got this far, something failed, redisplay form

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
                        flag = validateLogin.ChangePassword(currentUser.Email, passwordModel.OldPassword, passwordModel.NewPassword.ToMD5HashForPassword());
                        if (flag)
                            Session["Tempmsg"] = "[[[Password has been changed successfully]]]";
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                logService.LogError("Error in UpdatePassword", ex);
            }

            return View("ChangePassword", passwordModel);
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
                    var userRecord = validateLogin.GetUserDetails(currentUser.Email);
                    if (userRecord.Password == OldPassword.ToMD5HashForPassword())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logService.LogError("Error in CheckOldPassword", ex);
            }
            return Json(flag);
        }


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

        #endregion
    }
}