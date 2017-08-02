using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Kendo.Mvc.Extensions;
using System.IO;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Common;
using System.Collections;
using System.Web.Hosting;


namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class ManufacturersController : Controller
    {
        private IManufacturersService _IManufacturersService = null;
        IGeneralService _IGeneralService = null;

        public ManufacturersController()
        {
            _IManufacturersService = new ManufacturersService();
            _IGeneralService = new GeneralService();
        }

        #region Manage Manufacturer
        public ActionResult Manage()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new ManufacturersModel();
            model.PageAccessCode = currentUser.PageAccessCode;
            return View("Manage", model);
        }
        public ActionResult ManageManufacturersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                IEnumerable<ManufacturersModel> manufacturers = _IManufacturersService.GetAllMFs(currentUser.PageAccessCode);
                DataSourceResult result = manufacturers.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateMFStatus(ManufacturersModel record)
        {
            if (record.Manufacturer_Id != 0)
            {
                int mfID = (int)record.Manufacturer_Id;
                Session["CustomerID"] = record.Manufacturer_Id;
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                bool result = false;

                result = _IManufacturersService.UpdateMFStatus((int)record.Manufacturer_Id, (bool)record.Status, currentUser.User_Id);

                if (result == true)
                {
                    ManufacturersModel manufacturer = _IManufacturersService.GetMFById(mfID);
                    if (manufacturer != null)
                    {
                        // Old Code commented by KD as per Amit Feedback on Dated 23-07-2015

                        //string status = ((bool)manufacturer.Status) ? "Activated" : "Deactivated";
                        //manufacturer.Successmsg = "Manufacturer '<b>" + manufacturer.Manufacturer_Name + "</b>' has been " + status;
                        // End

                        string status = ((bool)manufacturer.Status) ? ReviveMessages.Active : ReviveMessages.Deactivate;
                        manufacturer.Successmsg = status;


                        ManufacturerNotification(manufacturer.Manufacturer_Name, status);
                        return Json(manufacturer, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return View("ManageCustomers");
        }
        private void ManufacturerNotification(string manufacturerName, string status)
        {
            string emailTo = Helper.GetEmailAddressByGroup(PageAccessCode.ADMIN);   // TO Active ADMINS

            if (!string.IsNullOrEmpty(emailTo))
            {
                string subject = "Redux - Manufacturer Status Notification";
                var templateVars = new Hashtable();
                templateVars.Add("Manufacturer", manufacturerName);
                templateVars.Add("Status", status);
                var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/Manufacturers/ManufacturerStatus.html"), templateVars);
                var emailBody = parser.Parse();
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.Send(emailTo, string.Empty, string.Empty, subject, emailBody);
            }
            else
            {
                // TODO: Pending. Implement is users not available.
            }
        }
        #endregion

        #region Create Manufacturer
        public ViewResult Create()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new ManufacturersModel();
            model.PageAccessCode = currentUser.PageAccessCode;
            return View("Create", model);
        }
        [HttpPost]
        public ActionResult CreateMF(ManufacturersModel model)
        {
            bool isInserted = false;
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                ManufacturersModel insert = new ManufacturersModel();
                insert.Manufacturer_Name = model.Manufacturer_Name;
                insert.Address = string.IsNullOrEmpty(model.Address) ? string.Empty : HttpUtility.HtmlEncode(model.Address);
                insert.Phone_Number = string.IsNullOrEmpty(model.Phone_Number) ? string.Empty : model.Phone_Number;
                insert.Company_Website = string.IsNullOrEmpty(model.Company_Website) ? string.Empty : model.Company_Website;
                insert.PM_Name = string.IsNullOrEmpty(model.PM_Name) ? string.Empty : model.PM_Name;
                insert.PM_Email = string.IsNullOrEmpty(model.PM_Email) ? string.Empty : model.PM_Email;
                insert.PM_Office_Phone = string.IsNullOrEmpty(model.PM_Office_Phone) ? string.Empty : model.PM_Office_Phone;
                insert.PM_Mobile = string.IsNullOrEmpty(model.PM_Mobile) ? string.Empty : model.PM_Mobile;
                insert.Status = true;
                insert.Created_by = currentUser.User_Id;
                insert.Manufacturer_Ref_Code = model.Manufacturer_Ref_Code;
                isInserted = _IManufacturersService.CreateMF(insert);
            }
            if (isInserted)
            {
                TempData["MFCreatedSuccess"] = true;
            }
            else
            {
                TempData["MFCreatedFailure"] = true;
            }
            return RedirectToAction("Manage", "Manufacturers");
        }
        #endregion

        #region View Details Manufacturer
        public ActionResult ViewMF(String id)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new ManufacturersModel();
            try
            {
                if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
                {
                    ManufacturersModel item = _IManufacturersService.GetMFById(Convert.ToInt32(id));
                    if (item != null)
                    {
                        model.Manufacturer_Id = item.Manufacturer_Id;
                        model.Manufacturer_Name = item.Manufacturer_Name;
                        model.Address = HttpUtility.HtmlDecode(item.Address);
                        model.Phone_Number = item.Phone_Number;
                        model.Company_Website = item.Company_Website;
                        model.PM_Name = item.PM_Name;
                        model.PM_Email = item.PM_Email;
                        model.PM_Office_Phone = item.PM_Office_Phone;
                        model.PM_Mobile = item.PM_Mobile;
                        model.Created_by = item.Created_by;
                        model.Created_Date = item.Created_Date;
                        model.PageAccessCode = currentUser.PageAccessCode;
                        model.Manufacturer_Ref_Code = item.Manufacturer_Ref_Code;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View("ViewMF", model);
        }
        #endregion

        #region Edit Manufacturer
        public ViewResult Edit(String id)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new ManufacturersModel();
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                var dbModel = _IManufacturersService.GetMFById(Convert.ToInt32(id));
                if (dbModel != null)
                {
                    model.Manufacturer_Id = dbModel.Manufacturer_Id;
                    model.Manufacturer_Name = dbModel.Manufacturer_Name;
                    model.Address = HttpUtility.HtmlDecode(dbModel.Address);
                    model.Phone_Number = dbModel.Phone_Number;
                    model.Company_Website = dbModel.Company_Website;
                    model.PM_Name = dbModel.PM_Name;
                    model.PM_Email = dbModel.PM_Email;
                    model.PM_Office_Phone = dbModel.PM_Office_Phone;
                    model.PM_Mobile = dbModel.PM_Mobile;
                    model.Manufacturer_Ref_Code = dbModel.Manufacturer_Ref_Code;
                }
                model.PageAccessCode = currentUser.PageAccessCode;
            }
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult UpdateMF(ManufacturersModel model)
        {
            bool ifUpdated = false;
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                ManufacturersModel update = new ManufacturersModel();
                update.Manufacturer_Id = model.Manufacturer_Id;
                update.Manufacturer_Name = model.Manufacturer_Name;
                update.Address = string.IsNullOrEmpty(model.Address) ? string.Empty : HttpUtility.HtmlEncode(model.Address);
                update.Phone_Number = string.IsNullOrEmpty(model.Phone_Number) ? string.Empty : model.Phone_Number;
                update.Company_Website = string.IsNullOrEmpty(model.Company_Website) ? string.Empty : model.Company_Website;
                update.PM_Name = string.IsNullOrEmpty(model.PM_Name) ? string.Empty : model.PM_Name;
                update.PM_Email = string.IsNullOrEmpty(model.PM_Email) ? string.Empty : model.PM_Email;
                update.PM_Office_Phone = string.IsNullOrEmpty(model.PM_Office_Phone) ? string.Empty : model.PM_Office_Phone;
                update.PM_Mobile = string.IsNullOrEmpty(model.PM_Mobile) ? string.Empty : model.PM_Mobile;
                update.Modified_by = currentUser.User_Id;
                update.Manufacturer_Ref_Code = model.Manufacturer_Ref_Code;
                ifUpdated = _IManufacturersService.UpdateMFDetails(update);
            }
            if (ifUpdated)
            {
                TempData["MFUpdatedSuccess"] = true;
            }
            else
            {
                TempData["MFUpdatedFailure"] = true;
            }
            return RedirectToAction("Manage", "Manufacturers");
        }
        #endregion

        #region Check Manufacturer Ref Code
        public JsonResult CheckManufacturerRefCode(string Manufacturer_Ref_Code, int? Manufacturer_Id)
        {
            var flag = false;
            if (Manufacturer_Id == null)
                Manufacturer_Id = 0;
            int _Manufacturer_Id = (int)Manufacturer_Id;
            try
            {
                flag = _IManufacturersService.CheckManufacturerRefCodeExists(Manufacturer_Ref_Code, _Manufacturer_Id);
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckEmailAddress", ex);
                throw;
            }
            return Json(flag);
        }
        #endregion
    }
}