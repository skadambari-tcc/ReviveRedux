using Kendo.Mvc.UI;
using Revive.Redux.Common;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Collections;
using System.Web.Hosting;
namespace Revive.Redux.UI.Controllers.ManageSubsidiary
{
    [Authorize]
    [ReviveAuth]
    public class ManageSubsidiaryController : Controller
    {
        #region Private Variables
        private ISubsidiaryManagement _ISubsidiaryManagement = null;
        IGeneralService _IGeneralService = null;
        private ILocationManagement _ILocationManagement = null;
        NotificationModel objNotification = null;
        private ILogService logService = null;
        private IFormManagementService _IFormManagementService = null;
        private IUserManagmentService _ManageUserService = null;
        private CurrentUserDetail objSessionData = null;
        #endregion

        #region Contructor
        public ManageSubsidiaryController()
        {
            _ISubsidiaryManagement = new SubsidiaryManagement();
            _IGeneralService = new GeneralService();
            _ILocationManagement = new LocationManagement();
            _ManageUserService = new UserManagementService();
            // ViewBag.Citylst = _IGeneralService.GetEmptyDDL();
            ViewBag.Statelst = _IGeneralService.GetState();
            ViewBag.AccountMgrlst = _IGeneralService.GetAccountManagerDetail();
            ViewBag.CustomeDocumentUpload = false;
            logService = new LogService();
            _IFormManagementService = new FormManagementService();
            ViewBag.File = false;
            ViewBag.CustomerLocationBtn = false;
        }

        #endregion
        // GET: ManageSubsidiary
        public ActionResult ManageSubsidiary()
        {

            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new ManageSubsidiaryModel();
            model.PageAccessCode = currentUser.PageAccessCode;
            return View("ManageSubsidiary", model);
        }

        [AjaxHandleException]
        public ActionResult ManageSubsidiaryAjax([DataSourceRequest] DataSourceRequest request)
        {
            objSessionData = (CurrentUserDetail)Session["CurrentUser"];
            IEnumerable<ManageSubsidiaryModel> objSubsidiary = null;
            objSubsidiary = _ISubsidiaryManagement.GetSubsidiaryList(objSessionData.User_Id, objSessionData.PageAccessCode);
            DataSourceResult result = objSubsidiary.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ViewResult CreateSubsidiary()
        {
            ViewData["Create"] = "false";
            objSessionData = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.Customerlst = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);
            return View();
        }

        public JsonResult GetCascadeCity(int Id)
        {
            ManageSubsidiaryModel City = new ManageSubsidiaryModel();
            var CityList = _IGeneralService.GetCity(Id);
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckSubsidiaryName(String Subsidiary_Name, int? Subsidiary_ID)
        {
            var flag = false;
            if (Subsidiary_ID == null)
                Subsidiary_ID = 0;
            int _subsidiary_Id = (int)Subsidiary_ID;
            try
            {
                flag = _ISubsidiaryManagement.CheckSubsidiaryExists(Subsidiary_Name, _subsidiary_Id);
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckEmailAddress", ex);
                throw;
            }
            return Json(flag);
        }

        public ActionResult AddSubsidiary(ManageSubsidiaryModel SubsidiaryRecord)
        {
            string sErrorMsg = string.Empty;
            var flag = false;
            try
            {
                flag = _ISubsidiaryManagement.CheckSubsidiaryRefExists((SubsidiaryRecord.Subsidiary_Ref_Code).ToString(), Convert.ToInt32(SubsidiaryRecord.Subsidiary_ID), Convert.ToInt32(SubsidiaryRecord.CustomerId));
            }
            catch { flag = false; }

            ManageSubsidiaryModel CreateSubsidiary = new ManageSubsidiaryModel();
            if (Session["SubsidiaryID"] != null)
            {
                if (flag == true)
                {
                    CreateSubsidiary = Records(SubsidiaryRecord);
                    ManageSubsidiaryModel Result = _ISubsidiaryManagement.CreateSubsidiary(CreateSubsidiary);
                    TempData["isUpdatedSuccess"] = true;

                    if (Result != null)
                    {
                        RedirectToAction("ManageSubsidiary");
                    }
                }
                else
                {
                    TempData["isDuplicate"] = true;
                    return RedirectToAction("EditSubsidiary", "ManageSubsidiary", new { id = Convert.ToInt32(SubsidiaryRecord.Subsidiary_ID) });
                }
            }
            else
            {
                if (flag == true)
                {
                    CreateSubsidiary = Records(SubsidiaryRecord);
                    int CustID = Convert.ToInt16(Session["SubsidiaryID"]);
                    ManageSubsidiaryModel Result = _ISubsidiaryManagement.CreateSubsidiary(CreateSubsidiary);
                    TempData["isCreatedSuccess"] = true;
                    TempData["Success"] = Result.Successmsg;

                    if (Result != null)
                    {
                        RedirectToAction("ManageSubsidiary");
                    }
                }
                else
                {
                    TempData["isDuplicate"] = true;
                    ViewData["Create"] = "false";
                    objSessionData = (CurrentUserDetail)Session["CurrentUser"];
                    ViewBag.Customerlst = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);
                    return View("CreateSubsidiary", SubsidiaryRecord);
                }
            }
            Session["SubsidiaryID"] = null;
            return RedirectToAction("ManageSubsidiary");
        }

        public ManageSubsidiaryModel Records(ManageSubsidiaryModel SubsidiaryRecord)
        {
            ManageSubsidiaryModel CreateSubsidiary = new ManageSubsidiaryModel();
            CreateSubsidiary.Subsidiary_ID = Convert.ToInt16(Session["SubsidiaryID"]);
            CreateSubsidiary.CustomerId = SubsidiaryRecord.CustomerId;
            CreateSubsidiary.Subsidiary_Name = SubsidiaryRecord.Subsidiary_Name.Trim();
            CreateSubsidiary.Primary_City_Name = SubsidiaryRecord.Primary_City_Name;
            CreateSubsidiary.Acct_manager_Primary_City_Name = SubsidiaryRecord.Acct_manager_Primary_City_Name;
            CreateSubsidiary.Subsidiary_Ref_Code = SubsidiaryRecord.Subsidiary_Ref_Code;
            if (SubsidiaryRecord.Acct_manager_Name != null)
            {
                CreateSubsidiary.Acct_manager_Name = SubsidiaryRecord.Acct_manager_Name;
            }
            if (SubsidiaryRecord.Email_ID != null)
            {
                CreateSubsidiary.Email_ID = SubsidiaryRecord.Email_ID;
            }
            CreateSubsidiary.Primary_Address = SubsidiaryRecord.Primary_Address;
            if (SubsidiaryRecord.Primary_Phone != null)
            {
                CreateSubsidiary.Primary_Phone = SubsidiaryRecord.Primary_Phone;
            }
            CreateSubsidiary.Primary_City = SubsidiaryRecord.Primary_City;
            if (SubsidiaryRecord.Primary_State != null)
            {
                CreateSubsidiary.Primary_State = SubsidiaryRecord.Primary_State;
            }
            if (SubsidiaryRecord.Primary_ZipCode != null)
            {
                CreateSubsidiary.Primary_ZipCode = SubsidiaryRecord.Primary_ZipCode;
            }
            CreateSubsidiary.Acct_manager_Primary_Address = SubsidiaryRecord.Acct_manager_Primary_Address;
            CreateSubsidiary.Acct_manager_Primary_City = SubsidiaryRecord.Acct_manager_Primary_City;
            if (SubsidiaryRecord.Acct_manager_Primary_State != null)
            {
                CreateSubsidiary.Acct_manager_Primary_State = SubsidiaryRecord.Acct_manager_Primary_State;
            }
            if (SubsidiaryRecord.Acct_manager_Primary_Zip != null)
            {
                CreateSubsidiary.Acct_manager_Primary_Zip = SubsidiaryRecord.Acct_manager_Primary_Zip;
            }
            if (SubsidiaryRecord.Acct_manager_Primary_Phone != null)
            {
                CreateSubsidiary.Acct_manager_Primary_Phone = SubsidiaryRecord.Acct_manager_Primary_Phone;
            }
            return CreateSubsidiary;
        }

        public ActionResult EditSubsidiary(int id)
        {
            ViewBag.Title = "Edit Subsidiary";
            var model = new ManageSubsidiaryModel();
            if (id != 0)
            {
                Session["SubsidiaryID"] = id;
                ViewData["Create"] = "false";
                model = _ISubsidiaryManagement.GetSubsidiaryDetailsById(id);
                int State_Id = Convert.ToInt16(model.Primary_State);
                ViewBag.isUpdateMode = true;
                objSessionData = (CurrentUserDetail)Session["CurrentUser"];
                ViewBag.Customerlst = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);
            }
            return View("CreateSubsidiary", model);
        }

        public ActionResult ActivateDeactivateCustomer(ManageSubsidiaryModel SubsidiaryRecord)
        {


            if (SubsidiaryRecord.Subsidiary_ID > 0)
            {
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                if (objCurrentUserDetail != null)
                {
                    SubsidiaryRecord.Modified_by = (Guid)objCurrentUserDetail.User_Id;
                    SubsidiaryRecord.Modified_Date = Common.CommonMethods.GetCurrentDate();
                }
                bool bResult = false;
                string result = "";
                ManageSubsidiaryModel subsidiaryDetails = new ManageSubsidiaryModel();

                //  bool bResult = _ILocationManagement.ActivateDeactivateLocation(objCustomerLocation);
                if (SubsidiaryRecord.Status == true)
                {
                    bResult = _IGeneralService.DeActivateBySubdirectoryId(SubsidiaryRecord.Subsidiary_ID, objCurrentUserDetail.User_Id);
                    SubsidiaryRecord.Status = false;
                }
                else
                {
                    result = _IGeneralService.ActivateBySubdirectoryId(SubsidiaryRecord.Subsidiary_ID, objCurrentUserDetail.User_Id);
                    SubsidiaryRecord.Status = true;
                }

                if (bResult == true || result == "")
                {
                    SendNotification(SubsidiaryRecord);
                    // string sStatus = string.Empty;
                    //sStatus = (locationDetails.Status) ? "Activated" : "Deactivated";
                    //locationDetails.Successmsg = "Location [locationName] has been " + sStatus;
                    if (SubsidiaryRecord.Status)
                    {
                        SubsidiaryRecord.Successmsg = ReviveMessages.Active;

                    }
                    else
                    {
                        SubsidiaryRecord.Successmsg = ReviveMessages.Deactivate;
                    }
                    return Json(SubsidiaryRecord, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //UserModels UserDetails = SendNotification(objUserModel);
                    string sStatus = string.Empty;
                    SubsidiaryRecord.ErrorMsgs = result;
                    SubsidiaryRecord.Successmsg = "";
                    return Json(SubsidiaryRecord, JsonRequestBehavior.AllowGet);
                }

            }

            return View("ManageSubsidiary");
        }
        private ManageSubsidiaryModel SendNotification(ManageSubsidiaryModel objCustomer)
        {
            string ccMailId = Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);   // CC Account Managers       
            string sEmailTo = Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, objCustomer.CustomerId);   // CC Account Managers  
            ManageSubsidiaryModel CustomerDetails = _ISubsidiaryManagement.GetSubsidiaryDetailsById(objCustomer.Subsidiary_ID);
            objNotification = new NotificationModel();
            if (!string.IsNullOrEmpty(sEmailTo))
            {
                EmailHelper objEmailHelper = new EmailHelper();
                string sStatus = string.Empty;
                sStatus = (CustomerDetails.Status) ? "Activated" : "Deactivated";
                var objKeyWords = new Hashtable();
                objKeyWords.Add("SUBSIDIARYNAME", CustomerDetails.Subsidiary_Name);
                objKeyWords.Add("STATUS", sStatus);
                var objParser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ManageCustomer/SubsidiaryStatus.html"), objKeyWords);
                var sEmailbody = objParser.Parse();
                objNotification.body_mail = sEmailbody;
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                objNotification.Created_by = objCurrentUserDetail.User_Id;
                objNotification.NotificationMessages = "Subsidiary Status Notification";
                objNotification.Notification_Date = DateTime.Now;
                objNotification.Mail_Ids = sEmailTo;
                _IGeneralService.StoreNotification(objNotification);
                objEmailHelper.Send(sEmailTo, ccMailId, string.Empty, "Redux - Subsidiary Status Notification", sEmailbody);
            }
            return CustomerDetails;
        }

        public JsonResult CheckSubsidiaryRefCode(String Subsidiary_Ref_Code, string Subsidiary_ID, string CustomerId)
        {
            var flag = false;
            int _subsidiaryId = 0;
            int _customerId = 0;
            if (Subsidiary_ID != "" && Subsidiary_ID != null)
            {
                _subsidiaryId = Convert.ToInt32(Subsidiary_ID);
            }
            if (CustomerId != "" && CustomerId != null)
            {
                _customerId = Convert.ToInt32(CustomerId);
            }
            try
            {
                flag = _ISubsidiaryManagement.CheckSubsidiaryRefExists(Subsidiary_Ref_Code, _subsidiaryId, _customerId);
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckEmailAddress", ex);
                throw;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}