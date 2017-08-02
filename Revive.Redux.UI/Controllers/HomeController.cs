using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Common;
using System.Web.Security;
using ComponentSpace.SAML2;
namespace Revive.Redux.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IOrderManagementService orderService = null;
        IGeneralService generalService;
        public ActionResult Index()
        {
            Session["RedirectArchiveManage"] = "Home";
            return View();
        }

        public HomeController()
        {
            orderService = new OrderManagementService();
            generalService = new GeneralService();
        }
        public ActionResult GetOrderStatusDashboard([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                // Changes done for showing PendingApproval tasks to Admin & SuperAdmin also
                //if (currentUser.PageAccessCode == PageAccessCode.SUPERADMIN || currentUser.PageAccessCode == PageAccessCode.ADMIN || currentUser.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                if (currentUser.PageAccessCode == PageAccessCode.CUSTOMERADMIN || currentUser.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN || currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IEnumerable<OrderModel> orders = generalService.GetHomePagePendingTasks(currentUser.User_Id, currentUser.UserLevelId, currentUser.PageAccessCode, Convert.ToInt32(currentUser.Manufacturer_Id));//orderService.GetOrdersList(currentUser.PageAccessCode, currentUser.User_Id);
                    DataSourceResult result = orders.ToDataSourceResult(request);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }




        public ActionResult GetOrderStatusDashboardByRoleId([DataSourceRequest] DataSourceRequest request, string UserLevel_Id)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            

            if (UserLevel_Id != "")
            {
                currentUser.UserLevelId = Convert.ToInt32(UserLevel_Id);
                currentUser.PageAccessCode = generalService.GetUserByPageAccessCodeByUserLevelId(currentUser.UserLevelId);
            }
            else
            {
                currentUser.UserLevelId = 0;
                currentUser.PageAccessCode = PageAccessCode.ADMIN;
            }
            // Account manager ID setting Null values 

            currentUser.User_Id = new Guid();
            currentUser.Manufacturer_Id = 0;


            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                // Changes done for showing PendingApproval tasks to Admin & SuperAdmin also
                //if (currentUser.PageAccessCode == PageAccessCode.SUPERADMIN || currentUser.PageAccessCode == PageAccessCode.ADMIN || currentUser.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                if (currentUser.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                {
                    var myTasklst = new List<OrderModel>();
                    return Json(myTasklst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IEnumerable<OrderModel> orders = generalService.GetHomePagePendingTasks(currentUser.User_Id, currentUser.UserLevelId, currentUser.PageAccessCode, Convert.ToInt32(currentUser.Manufacturer_Id));//orderService.GetOrdersList(currentUser.PageAccessCode, currentUser.User_Id);
                    DataSourceResult result = orders.ToDataSourceResult(request);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var myTasklst1 = new List<OrderModel>();
                return Json(myTasklst1, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult GetNotification([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            IEnumerable<NotificationModel> notificationData = generalService.GetNotificationDashboard(currentUser.Email_Id);
            DataSourceResult result = notificationData.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowNotification(int id)
        {
            NotificationModel notificationdetails = new NotificationModel();
            notificationdetails.Notification_Id = id;
            bool flag = generalService.StoreNotification(notificationdetails);
            notificationdetails = generalService.GetNotificationDetiailById(id);
            return Json(notificationdetails.body_mail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteNotication(string id)
        {

            //Delete the record
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            string ErrorMsgs = generalService.DeleteNotification(Convert.ToInt32(id), currentUser.Email_Id);
            ViewBag.DeleteError = ErrorMsgs;
            if (ErrorMsgs == "")
            {
                return Json("Notification Deleted Successfully.", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       
    }
}