using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Revive.Redux.Services;
using Revive.Redux.Entities;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Revive.Redux.Controllers.Common;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web;
using System.Web.Http.Cors;
using Revive.Redux.Common;
using System.Configuration;
using System.Collections;
using System.Web.Hosting;
using Revive.Redux.Entities.Common;
using Revive.Redux.Commn;
using System.Web.Configuration;


namespace Revive.Redux.UI.WebApi.ManageUsers
{
    // [Authorize]


    public class ManageUserController : ApiController
    {
        ILoginService _loginService = null;
        ReturnStatusCode objStatus = null;
        APIResponseData objApiResponseData = null;
        IUserManagmentService objUserService = null;
        ILogService _log4netService = null;
        WriteLog logwrite = null;


        public ManageUserController()
        {
            _loginService = new LoginService();
            objStatus = new ReturnStatusCode();
            objApiResponseData = new APIResponseData();
            objUserService = new UserManagementService();
            _log4netService = new LogService();

        }

        [ActionName("UserList")]
        [HttpGet]
        public DtoUser GetUserListByUserName()
        {
            DtoUser UserList = _loginService.GetUserDetails("ksahoo@svam.com");
            return UserList;
        }
        [ReviveApiAuth]
        [HttpPost]
        public APIResponseData Logoff()
        {
            /// HttpContext.Current.Session["CurrentUser"] = null;
            //HttpContext.Current.Session.Abandon();
            objStatus.StatusId = APIStatusValues.SuccessId;
            objStatus.StatusMessages = APIStatusValues.StatusSuccess;
            objApiResponseData.StatusMessages = objStatus;
            objApiResponseData.responseData = "Logout Successfully Done.";

            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData ForgotPassword(MembersShipModel objMembershipModel)
        {
            //  HttpResponseMessage response = null;
            var userFullName = String.Empty;
            APIResponseData objResponseData = new APIResponseData();
            string email_Id = objMembershipModel.Email_Id;
            try
            {
                var passwordText = CommonMethods.GetRandomPassword();
                var validateLogin = new CustomMemberShip();
                var emailLink = ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString();
                var templateVars = new Hashtable();
                templateVars.Add("Password", passwordText);
                var temppasswordFlag = false;
                var userRecord = validateLogin.GetUserDetails(email_Id);
                if (userRecord != null)
                {
                    temppasswordFlag = validateLogin.ChangePassword(email_Id, "", passwordText.ToMD5HashForPassword());

                    if (userRecord.LastName != null)
                    {
                        userFullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                    }
                    else
                    {
                        userFullName = userRecord.FirstName;
                    }
                    templateVars.Add("UserFullName", userFullName);
                    var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/TabletForgetPassword.html"), templateVars);
                    var emailBody = parser.Parse(); // string.Format(ConstantEntities.NewCustomerEmailBody, emailLink, customer.Email, pasword);
                    EmailHelper emailHelper = new EmailHelper();
                    emailHelper.Send(email_Id, string.Empty, string.Empty, "Redux - Forgot Password", emailBody);
                    //response = Request.CreateResponse(HttpStatusCode.OK, "Temporary password sent to your registered Email ID");


                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    objResponseData.responseData = "Temporary password sent to your registered Email ID";

                }
                else
                {
                    //  response = Request.CreateResponse(HttpStatusCode.NoContent, "Email ID not Registered");
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    objResponseData.responseData = "Email ID not Registered";
                }
            }
            catch (Exception ex)
            {
                //response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Error in Password Changes ");
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = ex.Message.ToString();
                objResponseData.responseData = "Error in Forgot password Calling";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "ForgotPassword Called-----";
            var list1 = new List<Tuple<string, string>>
                {
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages.ToString()),
                new Tuple<string,string>(" Email_Id:", objMembershipModel.Email_Id.ToString())
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objResponseData.StatusMessages = objStatus;
            return objResponseData;
        }


        [HttpPost]
        public APIResponseData Login(MembersShipModel objMembershipModel)
        {
            try
            {

                String UserName = objMembershipModel.userName;
                String password = objMembershipModel.password;
                var customMemberShip = new CustomMemberShip();
                //var session = HttpContext.Current.Session;
                var flag = Membership.ValidateUser(UserName, password.ToMD5HashForPassword());
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                var currentUserDetail = new CurrentUserDetail();
                if (flag)
                {
                    var userRecord = customMemberShip.GetUserDetails(UserName);
                    if (userRecord.status != null && (!userRecord.IsLockedOut && userRecord.status.Value))
                    {
                        // session["Time"] = DateTime.Now;

                        currentUserDetail.User_Id = userRecord.User_ID;
                        currentUserDetail.FirstName = userRecord.FirstName;
                        currentUserDetail.LastName = userRecord.LastName;
                        currentUserDetail.Email_Id = userRecord.Email_ID;
                        currentUserDetail.FullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                        currentUserDetail.UserLevelId = userRecord.User_Level_ID;
                        currentUserDetail.Customer_Id = userRecord.Customer_ID;
                        currentUserDetail.Location_Id = userRecord.Location_ID;
                        currentUserDetail.Key = GetAuthKey(userRecord.User_ID);
                        currentUserDetail.userType = userRecord.UserType;
                        currentUserDetail.userLevelName = userRecord.PageAccessCode;
                        currentUserDetail.IsLoggingAllowed = userRecord.IsStoreUserLog;
                        currentUserDetail.LoggingTypeCode = userRecord.LoggingTypeCode;
                        currentUserDetail.IsMultiDeviceSupported = userRecord.IsMultiDeviceSupported;
                        currentUserDetail.CustomerNoOfDevices = userRecord.CustomerMaxDevices;

                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(currentUserDetail);

                        objStatus.StatusId = APIStatusValues.SuccessId;
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                        objApiResponseData.responseData = currentUserDetail;
                        objApiResponseData.StatusMessages = objStatus;

                    }
                    else
                    {

                        objStatus.StatusId = APIStatusValues.FailureId;
                        objStatus.StatusMessages = APIStatusValues.StatusFailure;
                        objApiResponseData.responseData = "Account deactivated. Please contact Redux at " + Convert.ToString(WebConfigurationManager.AppSettings["ReviveContactNum"]) + " for more information.";
                        objApiResponseData.StatusMessages = objStatus;
                    }
                    // var user = (CurrentUserDetail)HttpContext.Current.Session["CurrentUser"];
                    currentUserDetail.isChangePassword = false;
                    if (!userRecord.LastPasswordChangeDate.HasValue)
                    {
                        currentUserDetail.isChangePassword = true;
                    }
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Invalid User Name & Password.";
                }


            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = ex.Message.ToString();
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "Login Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" userName:", objMembershipModel.userName)
             };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        private string GetAuthKey(Guid guid)
        {
            string sAuthKey = string.Empty;
            string sKey = ConfigurationSettings.AppSettings["Encrption"].ToString();
            string sIP = HttpContext.Current.Request.UserHostAddress;
            // sIP = GetClientIp();
            string sCurrentDate = Convert.ToString(DateTime.Now);

            sAuthKey = guid + "@" + sIP + "@" + sCurrentDate;

            sAuthKey = Cryptography.EncryptText(sAuthKey, sKey);

            return sAuthKey;
        }




        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
        [ReviveApiAuth]
        [HttpPost]
        public APIResponseData ChangePassword(MembersShipModel objMemberModel)
        {
            bool changePasswordFlag = false;
            bool ValidUserId = false;
            bool bOldPassword = false;
            try
            {
                var UserRecord = objUserService.GetUserById(objMemberModel.user_Id);
                if (UserRecord != null)
                {
                    ValidUserId = true;
                    bOldPassword = _loginService.CheckOldPassword(UserRecord.Email, objMemberModel.newPassword.ToMD5HashForPassword(), objMemberModel.oldPassword.ToMD5HashForPassword());
                    if (bOldPassword)
                    {
                        changePasswordFlag = _loginService.ChangePassword(UserRecord.Email, objMemberModel.newPassword.ToMD5HashForPassword(), objMemberModel.oldPassword);
                    }
                }

                if (changePasswordFlag == true)
                {
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Password has been changed successfully";
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Error changing the password!";
                }
                if (ValidUserId == false)
                {
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "User Id Not Registered.";
                }

                if (!bOldPassword)
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Old Password is not Correct.";
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = ex.Message.ToString();
                objApiResponseData.responseData = "Error in Changing Password";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "Change Password Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages.ToString()),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusId.ToString()),
                new Tuple<string,string>(" Email_Id:", objMemberModel.user_Id.ToString())

             };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;

        }

        [HttpPost]
        public APIResponseData LoginIDP(MembersShipModel objMembershipModel)
        {
            try
            {

                String UserName = objMembershipModel.userName;
                String password = objMembershipModel.password;
                var customMemberShip = new CustomMemberShip();
                //var session = HttpContext.Current.Session;
                LoginService loginser = new LoginService();

                var flag = loginser.ValidateIDPUser(objMembershipModel.userName);

                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                var currentUserDetail = new CurrentUserDetail();
                if (flag)
                {
                    var userRecord = customMemberShip.GetUserDetails(UserName);
                    if (userRecord.status != null && (!userRecord.IsLockedOut && userRecord.status.Value))
                    {
                        // session["Time"] = DateTime.Now;

                        currentUserDetail.User_Id = userRecord.User_ID;
                        currentUserDetail.FirstName = userRecord.FirstName;
                        currentUserDetail.LastName = userRecord.LastName;
                        currentUserDetail.Email_Id = userRecord.Email_ID;
                        currentUserDetail.FullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                        currentUserDetail.UserLevelId = userRecord.User_Level_ID;
                        currentUserDetail.Customer_Id = userRecord.Customer_ID;
                        currentUserDetail.Location_Id = userRecord.Location_ID;
                        currentUserDetail.Key = GetAuthKey(userRecord.User_ID);
                        currentUserDetail.userType = userRecord.UserType;
                        currentUserDetail.userLevelName = userRecord.PageAccessCode;
                        currentUserDetail.IsLoggingAllowed = userRecord.IsStoreUserLog;
                        currentUserDetail.LoggingTypeCode = userRecord.LoggingTypeCode;

                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(currentUserDetail);

                        objStatus.StatusId = APIStatusValues.SuccessId;
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                        objApiResponseData.responseData = currentUserDetail;
                        objApiResponseData.StatusMessages = objStatus;

                    }
                    else
                    {

                        objStatus.StatusId = APIStatusValues.FailureId;
                        objStatus.StatusMessages = APIStatusValues.StatusFailure;
                        objApiResponseData.responseData = "Account deactivated. Please contact Redux at " + Convert.ToString(WebConfigurationManager.AppSettings["ReviveContactNum"]) + " for more information.";
                        objApiResponseData.StatusMessages = objStatus;
                    }
                    // var user = (CurrentUserDetail)HttpContext.Current.Session["CurrentUser"];
                    currentUserDetail.isChangePassword = false;
                    if (!userRecord.LastPasswordChangeDate.HasValue)
                    {
                        currentUserDetail.isChangePassword = true;
                    }
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Invalid User Name & Password.";
                }


            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = ex.Message.ToString();
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "Login Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" userName:", objMembershipModel.userName)
             };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }


    }
}
