using Revive.Redux.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Revive.Redux.UI
{
    public class ReviveHandleErrorAttribute : HandleErrorAttribute
    {

        #region Private Variable

        private ILogService logService = null;

        #endregion

        #region Contructor

        public ReviveHandleErrorAttribute()
        {
            logService = new LogService();
        }

        #endregion
        
        #region Exception Handling

            public override void OnException(ExceptionContext filterContext)
            {

                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var UserName = filterContext.HttpContext.User.Identity.Name;

                if (filterContext.ExceptionHandled)
                {
                    return;
                }

                if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
                {
                    return;
                }

                if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
                {
                    return;
                }

                // if the request is AJAX return JSON else view.
                if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            error = true,
                            message = filterContext.Exception.Message
                        }
                    };
                }
                else
                {
                   var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                    filterContext.Result = new ViewResult
                    {
                        ViewName = View,
                        MasterName = Master,
                        ViewData = new ViewDataDictionary(model),
                        TempData = filterContext.Controller.TempData
                    };
                }

               
                // log the error 
                LogError(controllerName, actionName, UserName, filterContext.Exception.Message, filterContext.Exception, filterContext.HttpContext.Request);

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;

                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        #endregion

        #region Log Handling

            private void LogError(string sControllerName, string sActionName, string sUserName, string sExceptionMessage, Exception exception, HttpRequestBase Request)
            {
                string sMessage = "Redux - Testing team Error Notification for the Controller " + sControllerName + " for Action " + sActionName + " for User " + sUserName + " - ";
                string sResult = GetRequestData(Request);
                logService.LogError(sMessage + sResult, exception);
                EmailNotification(sMessage, sResult, exception);
            }

        #endregion

        #region Email Notification for Error

            private void EmailNotification(string sMessage, string sExceptionMessage, Exception exception)
            {
                bool bEmailEnable = false;
                string sEnableErrorNotification = ConfigurationSettings.AppSettings["EnableErrorNotification"].ToString();
                
                if (!string.IsNullOrEmpty(sEnableErrorNotification))
                {
                    bEmailEnable = Convert.ToBoolean(sEnableErrorNotification);
                }

                if (bEmailEnable)
                { 
                EmailHelper emailHelper = new EmailHelper();
                string sSubject = sMessage;
                string sEmailBody = sSubject + GetEmailBody(exception, sExceptionMessage);
                string sEmailTo = string.Empty;
                sEmailTo = ConfigurationSettings.AppSettings["EmailTo"];

                    if (!string.IsNullOrEmpty(sEmailTo))
                    {
                        emailHelper.Send(sEmailTo, string.Empty, string.Empty, sSubject, sEmailBody);
                    }
                }
            }

            private string GetEmailBody(Exception exception, string sExceptionMessage)
            {
                StringBuilder objEmailBody = new StringBuilder("</br></br>");
                objEmailBody.Append("<b>Error Message:</b>  ");
                objEmailBody.Append(exception.Message);
                objEmailBody.Append("</br></br>");
                objEmailBody.Append("<b>Request Information:</b>   "); ;
                objEmailBody.Append(sExceptionMessage);
                objEmailBody.Append("</br></br>");
                objEmailBody.Append("<b>Error Source:</b>   "); ;
                objEmailBody.Append(exception.Source);
                objEmailBody.Append("</br></br>");
                objEmailBody.Append("<b>Error StackTrace:</b>  ");
                objEmailBody.Append(exception.StackTrace);


                return objEmailBody.ToString();
            }

            #endregion

        #region Private Funtion

            private string GetRequestData(HttpRequestBase Request)
        {
            string sResult = string.Empty;
            string sFormKeysValues = string.Empty;
            string sFormValues = string.Empty;

            int nFormVariable = Request.Form.Count;
            int nQueryVariable = Request.QueryString.Count;

            for (int ncount = 0; ncount < nFormVariable; ncount++)
            {
                sFormKeysValues += Request.Form.AllKeys[ncount] + " :- " + Request.Form[ncount] + Environment.NewLine;               
            }

            for (int ncount = 0; ncount < nQueryVariable; ncount++)
            {
                sFormKeysValues += Request.QueryString.AllKeys[ncount] + " :- " + Request.QueryString[ncount] + Environment.NewLine;
            }

            sResult = sFormKeysValues;


            return sResult;
        }

            #endregion
    }
}