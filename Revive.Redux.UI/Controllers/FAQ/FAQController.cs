using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Web.Script.Serialization;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Common;

namespace Revive.Redux.UI
{
    [Authorize]
    [ReviveAuth]
    public class FAQController : Controller
    {
        private IFAQService _IFAQService = null;
        IGeneralService _IGeneralService = null;
        public FAQController()
        {
            _IFAQService = new FAQService();
            _IGeneralService = new GeneralService();
        }

        #region Add FAQ
        public ActionResult Add()
        {
            List<DtoList> userType = new List<DtoList>();
            userType.Add(new DtoList { Id = 0, Text = "--Select--" });
            userType.AddRange(_IGeneralService.GetUserType());
            ViewBag.UserGroup = userType;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateFAQ()
        {
            try
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                if (Request.Form["JsonPostbackVal"] != null && Request.Form["JsonPostbackVal"] != "")
                {
                    _IFAQService.DeleteFAQs();
                    if (Request.Form["JsonPostbackVal"] != "EMPTY")
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        List<FAQModel> faqModel = serializer.Deserialize<List<FAQModel>>(Request.Form["JsonPostbackVal"]);
                        if (faqModel.Count > 0)
                        {
                            _IFAQService.InsertFAQs(faqModel, currentUser.User_Id);
                        }
                    }
                    TempData["FAQUpdated"] = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Add", "FAQ");
        }
        public ActionResult ManageFAQAjax([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<FAQModel> faqs = _IFAQService.GetFAQs(PageAccessCode.ADMIN);
            DataSourceResult result = faqs.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View FAQ
        public ActionResult Show()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetFAQsAjax()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                var faqs = _IFAQService.GetFAQs(currentUser.PageAccessCode);
                return Json(faqs, JsonRequestBehavior.AllowGet);
            }
            else
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}