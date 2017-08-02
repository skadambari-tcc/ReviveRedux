using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Revive.Redux.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //Custom Error handling with logging.
            GlobalFilters.Filters.Add(new ReviveHandleErrorAttribute());
        }
        private void Session_Start(object sender, EventArgs e)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            //log.InfoFormat("Starting session: {0} from {1}.", Session.SessionID, ip);
            if ((HttpContext.Current != null) &&
                (HttpContext.Current.User != null) &&
                (HttpContext.Current.User.Identity.IsAuthenticated))
            {
                var loginHelper = new LoginHelper();
                loginHelper.LoginHelperInitialize();
            }
        }
    }
}
