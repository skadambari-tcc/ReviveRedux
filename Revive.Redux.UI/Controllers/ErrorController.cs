using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Revive.Redux.UI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Auth
        public ActionResult Auth()
        {
            return View("UnAuth");
        }

        // GET: Error
        public ActionResult Error()
        {
            return View("Error");
        }
    }
}