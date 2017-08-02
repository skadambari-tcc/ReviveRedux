using Revive.Redux.Commn;
using Revive.Redux.Common;
using Revive.Redux.Entities;
using Revive.Redux.Entities.Common;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Revive.Redux.UI
{

    public class ReviveApiAuth : AuthorizationFilterAttribute
    {
        #region Variable Declaration

        bool Active = true;
        LogService _log4netService = new LogService();
        WriteLog logwrite = null;

        #endregion

        #region Constructor

        public ReviveApiAuth()
        {
            logwrite = new WriteLog();
        }

        /// <summary>
        /// Overriden constructor to allow explicit disabling of this
        /// filter's behavior. Pass false to disable (same as no filter
        /// but declarative)
        /// </summary>
        /// <param name="active"></param>
        public ReviveApiAuth(bool active)
        {
            Active = active;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Override to Web API filter method to handle Basic Auth check
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Active)
            {
                var identity = ParseAuthorizationHeader(actionContext);
                if (identity == null)
                {
                    Challenge(actionContext);
                    return;
                }


                if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
                {
                    Challenge(actionContext);
                    return;
                }

                var principal = new GenericPrincipal(identity, null);

                Thread.CurrentPrincipal = principal;

                // inside of ASP.NET this is also required for some async scenarios
                //if (HttpContext.Current != null)
                //    HttpContext.Current.User = principal;

                base.OnAuthorization(actionContext);
            }
        }

        protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            string sAuthKey = string.Empty;
            string[] Param = password.Split('@');

            if (Param.Length < 2 )
            {
                return false;
            }

            string sIP = GetUserIP(actionContext);
            DateTime dCurrentDate = DateTime.Now.AddMinutes(-GetSessionTimeOut());
            DateTime dPrevCurrentDate = Convert.ToDateTime(Param[1]);

            // Commented by KD as per discusson with DEV , IP Not require for security purposes

            //if (string.Compare(sIP, Param[0].ToString()) == 0 && dPrevCurrentDate >= dCurrentDate && GetValidUser(username))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            var retFlag = false;

            if (dPrevCurrentDate >= dCurrentDate && GetValidUser(username))
            {
                retFlag = true;
            }




            return retFlag;
        }
        
        /// <summary>
        /// Parses the Authorization header and creates user credentials
        /// </summary>
        /// <param name="actionContext"></param>
        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string sAuthHeader = null;
           // EmailNotification("Web API Param", "TESTING");

            string sKey = ConfigurationSettings.AppSettings["Encrption"].ToString();
            var auth = actionContext.Request.Headers.Authorization;
            var action = actionContext.ActionDescriptor.ActionName;
            var controller = actionContext.ControllerContext.Controller;
            var Uri = actionContext.Request.RequestUri.OriginalString;

           



            if (auth != null && auth.Scheme == "Revive")
                sAuthHeader = auth.Parameter;

            if (string.IsNullOrEmpty(sAuthHeader))
                return null;

            sAuthHeader = Cryptography.DecryptText(sAuthHeader, sKey); 

            int idx = sAuthHeader.IndexOf('@');
            if (idx < 0)
                return null;

            string username = sAuthHeader.Substring(0, idx);
            string password = sAuthHeader.Substring(idx + 1);

            string sParam = "@ Action :- " + action + " - </br> Controller :- " + controller + " - </br> URI :- "+Uri;

            EmailNotification("Redux - Web API Param", sAuthHeader + sParam);

            // Log information

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = action + " Called [Authorization]";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Uri)),
                new Tuple<string,string>(" user Id:", username)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            /// End
            /// 



            return new BasicAuthenticationIdentity(username, password);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionContext"></param>
        void Challenge(HttpActionContext actionContext)
        {
            ReturnStatusCode objStatus = new ReturnStatusCode();
            APIResponseData objApiResponseData = new APIResponseData();
            objStatus.StatusId = APIStatusValues.NotAuthorized;
            objStatus.StatusMessages = APIStatusValues.NoAuthorized;
            objApiResponseData.StatusMessages = objStatus;
            objApiResponseData.responseData = "Not Authorized.";
            actionContext.Response = actionContext.Request.CreateResponse(objApiResponseData);
            
        }

        #endregion

        #region Private Method

        private string GetUserIP(HttpActionContext actionContext)
        {
            // Get the sender address
            var myRequest = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
            var sIP = myRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(sIP))
            {
                string[] ipRange = sIP.Split(',');
                int le = ipRange.Length - 1;
                string trueIP = ipRange[le];
            }
            else
            {
                sIP = myRequest.ServerVariables["REMOTE_ADDR"];
            }

            return sIP;
        }

        private bool GetValidUser(string UserId)
        {
            ILoginService ILoginService = new LoginService();

            bool bResult = ILoginService.ValidateUser(UserId);

            return bResult;
        }

        private int GetSessionTimeOut()
        {
            int nSessionValue = 200;
            string sKey = ConfigurationSettings.AppSettings["ApiSessionState"];

            if (sKey != null)
            {
                nSessionValue = Convert.ToInt16(sKey);
            }

            return nSessionValue;
        }

        #endregion

        #region Email Notification for Param

        private void EmailNotification(string sSubject, string sMessage)
        {
            bool bEmailEnable = false;
            string sEnableErrorNotification = ConfigurationSettings.AppSettings["EnableAPINotification"].ToString();

            if (!string.IsNullOrEmpty(sEnableErrorNotification))
            {
                bEmailEnable = Convert.ToBoolean(sEnableErrorNotification);
            }

            if (bEmailEnable)
            {
                EmailHelper emailHelper = new EmailHelper();

                string sEmailBody = sSubject + GetEmailBody(sMessage);
                string sEmailTo = string.Empty;
                sEmailTo = ConfigurationSettings.AppSettings["EmailTo"];

                if (!string.IsNullOrEmpty(sEmailTo))
                {
                    emailHelper.Send(sEmailTo, string.Empty, string.Empty, sSubject, sEmailBody);
                }
            }
        }

        private string GetEmailBody(string sMessage)
        {
            string[] Param = sMessage.Split('@');
            StringBuilder objEmailBody = new StringBuilder("</br></br>");
            objEmailBody.Append("<b>USER:</b>  ");
            objEmailBody.Append(Param[0]);
            objEmailBody.Append("</br></br>");
            objEmailBody.Append("<b>IP:</b>   ");
            objEmailBody.Append(Param[1]);
            objEmailBody.Append("</br></br>");
            objEmailBody.Append("<b>DATE:</b>   ");
            objEmailBody.Append(Param[2]);
            objEmailBody.Append("</br></br>");
            objEmailBody.Append("<b>Information</b>   ");
            objEmailBody.Append(Param[3]); 


            return objEmailBody.ToString();
        }

        #endregion
    }


    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            this.Password = password;
        }

        /// <summary>
        /// Basic Auth Password for custom authentication
        /// </summary>
        public string Password { get; set; }
    }
}