using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Revive.Redux.UI
{
    public class AjaxHandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Exception != null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                int returnStatusCode = (int)HttpStatusCode.InternalServerError;
                if (filterContext.Exception is System.Web.HttpException)
                {
                    returnStatusCode = ((System.Web.HttpException)(filterContext.Exception)).GetHttpCode();
                    //filterContext.HttpContext.Response.StatusCode = ((System.Web.HttpException)(filterContext.Exception)).GetHttpCode();
                }
                filterContext.ExceptionHandled = true;
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { success = false, error = filterContext.Exception.Message, statusCode = returnStatusCode }
                };
            }
        }
    }
}