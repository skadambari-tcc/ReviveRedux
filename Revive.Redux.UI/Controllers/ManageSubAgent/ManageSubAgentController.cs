using Kendo.Mvc.UI;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Revive.Redux.Common;
using System.Collections;
using System.Web.Hosting;
namespace Revive.Redux.UI.Controllers.ManageSubAgent
{
    [Authorize]
    [ReviveAuth]
    public class ManageSubAgentController : Controller
    {
        #region Private Variables
        private ISubAgentManagement _ISubAgentManagement = null;
        IGeneralService _IGeneralService = null;
        NotificationModel objNotification = null;
        private ILogService logService = null;
        private IFormManagementService _IFormManagementService = null;
        private IUserManagmentService _ManageUserService = null;
        private CurrentUserDetail objSessionData = null;
        #endregion

        #region Contructor
        public ManageSubAgentController()
        {
            _ISubAgentManagement = new SubAgentManagement();
            _IGeneralService = new GeneralService();
            _ManageUserService = new UserManagementService();
            ViewBag.Statelst = _IGeneralService.GetState();
            ViewBag.AccountMgrlst = _IGeneralService.GetAccountManagerDetail();
            logService = new LogService();
            _IFormManagementService = new FormManagementService();
            ViewBag.File = false;
            ViewBag.CustomerLocationBtn = false;
            ViewBag.AccountMgrlst = _IGeneralService.GetAccountManagerDetail();
        }

        #endregion
        // GET: ManageSubAgent
        public ActionResult ManageSubAgents()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new ManageSubAgentModel();
            model.PageAccessCode = currentUser.PageAccessCode;
            return View("ManageSubAgents", model);
        }
        [AjaxHandleException]
        public ActionResult ManageSubAgentAjax([DataSourceRequest] DataSourceRequest request)
        {
            objSessionData = (CurrentUserDetail)Session["CurrentUser"];
            IEnumerable<ManageSubAgentModel> objSubAgent = null;
            objSubAgent = _ISubAgentManagement.GetSubAgentList(objSessionData.User_Id, objSessionData.PageAccessCode);
            DataSourceResult result = objSubAgent.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ViewResult CreateSubAgent()
        {
            ViewData["Create"] = "false";
            objSessionData = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.Customerlst = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);
            ViewBag.Subsidiarylst = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Text");
            ViewBag.UserTypelst = _IGeneralService.GetMasterValuesByType("SubAgent");
            return View();
        }
        [HttpPost]
        public JsonResult GetSubsidiaryByCustomer(int CustomerId)
        {
            try
            {
                objSessionData = (CurrentUserDetail)Session["CurrentUser"];

                if (PageAccessCode.SUBSIDIARYADMIN == objSessionData.PageAccessCode)
                {
                    var ListSubsidary = _IGeneralService.GetSubsidiaryByCustomerId(CustomerId, (int)objSessionData.SubsidiaryId);
                    return Json(ListSubsidary, JsonRequestBehavior.AllowGet);
                }
                else if (PageAccessCode.SUBAGENTADMIN == objSessionData.PageAccessCode)
                {
                    var ListSubsidary = _IGeneralService.GetSubsidiaryByCustomerId(CustomerId, (int)objSessionData.SubsidiaryId);
                    return Json(ListSubsidary, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ListSubsidary = _IGeneralService.GetSubsidiaryByCustomerId(CustomerId);
                    return Json(ListSubsidary, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ActionResult AddSubAgent(ManageSubAgentModel AgentRecord)
        {
            var flag = false;
            try
            {
                flag = _ISubAgentManagement.CheckSubAgentRefExists((AgentRecord.SubAgent_Ref_Code).ToString(), Convert.ToInt32(AgentRecord.SubAgent_ID), Convert.ToInt32(AgentRecord.CustomerId));
            }
            catch { flag = false; }

            string sErrorMsg = string.Empty;
            ManageSubAgentModel CreateAgent = new ManageSubAgentModel();
            DateTime? StartDate = null;
            DateTime? EndDate = null;
            if (Session["AgentID"] != null)
            {
                if (flag == true)
                {
                    int CustID = Convert.ToInt16(Session["AgentID"]);
                    CreateAgent = Records(AgentRecord);
                    StartDate = CreateAgent.Lease_Start_Date;
                    EndDate = CreateAgent.Lease_end_date;
                    ManageSubAgentModel Result = _ISubAgentManagement.CreateSubAgent(CreateAgent);
                    TempData["isUpdatedSuccess"] = true;

                    if (Result != null)
                    {
                        RedirectToAction("ManageSubAgents");
                    }
                }
                else
                {
                    TempData["isDuplicateSubAgentId"] = true;
                    return RedirectToAction("EditSubAgent", "ManageSubAgent", new { id = Convert.ToInt32(AgentRecord.SubAgent_ID) });
                }

            }
            else
            {
                if (flag == true)
                {
                    CreateAgent = Records(AgentRecord);
                    StartDate = CreateAgent.Lease_Start_Date;
                    EndDate = CreateAgent.Lease_end_date;

                    int CustID = Convert.ToInt16(Session["AgentID"]);

                    ManageSubAgentModel Result = _ISubAgentManagement.CreateSubAgent(CreateAgent);
                    TempData["isCreatedSuccess"] = true;
                    TempData["Success"] = Result.Successmsg;
                }
                else
                {
                    TempData["isDuplicateSubAgentId"] = true;
                    return RedirectToAction("CreateSubAgent", "ManageSubAgent");
                }
            }
            Session["AgentID"] = null;
            Session["files"] = null;
            return RedirectToAction("ManageSubAgents");
        }

        public ActionResult EditSubAgent(int id)
        {
            ViewBag.Title = "Edit Sub-Agent";
            var model = new ManageSubAgentModel();
            if (id != 0)
            {
                Session["AgentID"] = id;
                ViewData["Create"] = "false";
                model = _ISubAgentManagement.GetSubAgentDetailsById(id);
                int State_Id = Convert.ToInt16(model.Primary_State);
                ViewBag.isUpdateMode = true;
                objSessionData = (CurrentUserDetail)Session["CurrentUser"];
                ViewBag.Customerlst = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);

                if (objSessionData.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                { 
                    ViewBag.Subsidiarylst = _IGeneralService.GetSubsidiaryByCustomerId(model.CustomerId, (int)objSessionData.SubsidiaryId);
                }
                else if (objSessionData.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    ViewBag.Subsidiarylst = _IGeneralService.GetSubsidiaryByCustomerId(model.CustomerId, (int)objSessionData.SubsidiaryId);
                }
                else
                { 
                    ViewBag.Subsidiarylst = _IGeneralService.GetSubsidiaryByCustomerId(model.CustomerId);
                }
                
                ViewBag.UserTypelst = _IGeneralService.GetMasterValuesByType("SubAgent");
            }
            return View("CreateSubAgent", model);
        }

        public ManageSubAgentModel Records(ManageSubAgentModel agentRecord)
        {
            ManageSubAgentModel CreateAgent = new ManageSubAgentModel();
            CreateAgent.SubAgent_ID = Convert.ToInt16(Session["AgentID"]);
            CreateAgent.SubsidiaryId = agentRecord.SubsidiaryId;
            CreateAgent.CustomerId = agentRecord.CustomerId;
            CreateAgent.UserTypeID = agentRecord.UserTypeID;

            CreateAgent.SubAgent_Name = agentRecord.SubAgent_Name.Trim();
            CreateAgent.ShiptoAttention = agentRecord.ShiptoAttention.Trim();
            //CreateAgent.CustomerDocs = agentRecord.CustomerDocs;
            //CreateAgent.Customer_Name = agentRecord.Customer_Name.Trim();
            CreateAgent.Acct_manager_Name = agentRecord.Acct_manager_Name.Trim();
            CreateAgent.AccountmanagerUserID = agentRecord.AccountmanagerUserID;
            CreateAgent.Primary_City_Name = agentRecord.Primary_City_Name;
            CreateAgent.Acct_manager_Primary_City_Name = agentRecord.Acct_manager_Primary_City_Name;
            CreateAgent.SubAgent_Ref_Code = agentRecord.SubAgent_Ref_Code;
            if (agentRecord.Email_ID != null)
            {
                CreateAgent.Email_ID = agentRecord.Email_ID;
            }
            if (agentRecord.Lease_end_date != null)
            {
                CreateAgent.Lease_end_date = agentRecord.Lease_end_date;
            }
            if (agentRecord.Lease_Start_Date != null)
            {
                CreateAgent.Lease_Start_Date = agentRecord.Lease_Start_Date;
            }
            CreateAgent.Primary_Address = agentRecord.Primary_Address;
            //CreateCustomer.Additional_Address_info = CustomerRecord.Additional_Address_info;
            if (agentRecord.Primary_Phone != null)
            {
                CreateAgent.Primary_Phone = agentRecord.Primary_Phone;
            }
            // var record = CustomerRecord.CityList;
            CreateAgent.Primary_City = agentRecord.Primary_City;
            if (agentRecord.Primary_State != null)
            {
                CreateAgent.Primary_State = agentRecord.Primary_State;
            }
            if (agentRecord.Primary_ZipCode != null)
            {
                CreateAgent.Primary_ZipCode = agentRecord.Primary_ZipCode;
            }
            // CreateCustomer.Acct_manager_Additional_Address_info = CustomerRecord.Acct_manager_Additional_Address_info;
            CreateAgent.Acct_manager_Primary_Address = agentRecord.Acct_manager_Primary_Address;
            CreateAgent.Acct_manager_Primary_City = agentRecord.Acct_manager_Primary_City;
            if (agentRecord.Acct_manager_Primary_State != null)
            {
                CreateAgent.Acct_manager_Primary_State = agentRecord.Acct_manager_Primary_State;
            }
            if (agentRecord.Acct_manager_Primary_Zip != null)
            {
                CreateAgent.Acct_manager_Primary_Zip = agentRecord.Acct_manager_Primary_Zip;
            }
            if (agentRecord.Acct_manager_Primary_Phone != null)
            {
                CreateAgent.Acct_manager_Primary_Phone = agentRecord.Acct_manager_Primary_Phone;
            }
            return CreateAgent;

        }

        public ActionResult ActivateDeactivateSubAgent(ManageSubAgentModel SubAgentRecord)
        {

            if (SubAgentRecord.SubAgent_ID > 0)
            {
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                if (objCurrentUserDetail != null)
                {
                    SubAgentRecord.Modified_by = (Guid)objCurrentUserDetail.User_Id;
                    SubAgentRecord.Modified_Date = Common.CommonMethods.GetCurrentDate();
                }
                bool bResult = false;
                string result = "";
                ManageSubAgentModel subagentDetails = new ManageSubAgentModel();

                //  bool bResult = _ILocationManagement.ActivateDeactivateLocation(objCustomerLocation);
                if (SubAgentRecord.Status == true)
                {
                    bResult = _IGeneralService.DeActivateBySubAgentId(SubAgentRecord.SubAgent_ID, objCurrentUserDetail.User_Id);
                    SubAgentRecord.Status = false;
                }
                else
                {
                    result = _IGeneralService.ActivateBySubAgentId(SubAgentRecord.SubAgent_ID, objCurrentUserDetail.User_Id);
                    SubAgentRecord.Status = true;
                }

                if (bResult == true || result == "")
                {
                    SendNotification(SubAgentRecord);
                    // string sStatus = string.Empty;
                    //sStatus = (locationDetails.Status) ? "Activated" : "Deactivated";
                    //locationDetails.Successmsg = "Location [locationName] has been " + sStatus;
                    if (SubAgentRecord.Status)
                    {
                        SubAgentRecord.Successmsg = ReviveMessages.Active;
                    }
                    else
                    {
                        SubAgentRecord.Successmsg = ReviveMessages.Deactivate;
                    }
                    return Json(SubAgentRecord, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //UserModels UserDetails = SendNotification(objUserModel);
                    string sStatus = string.Empty;
                    SubAgentRecord.ErrorMsgs = result;
                    SubAgentRecord.Successmsg = "";
                    return Json(SubAgentRecord, JsonRequestBehavior.AllowGet);
                }

            }

            return View("ManageSubsidiary");
            return View("ManageSubAgents");
        }
        private ManageSubAgentModel SendNotification(ManageSubAgentModel objCustomer)
        {
            string ccMailId = Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);   // CC Account Managers       
            string sEmailTo = Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, objCustomer.CustomerId);   // CC Account Managers  
            ManageSubAgentModel AgentDetails = _ISubAgentManagement.GetSubAgentDetailsById(objCustomer.SubAgent_ID);
            objNotification = new NotificationModel();
            if (!string.IsNullOrEmpty(sEmailTo))
            {
                EmailHelper objEmailHelper = new EmailHelper();
                string sStatus = string.Empty;
                sStatus = (AgentDetails.Status) ? "Activated" : "Deactivated";
                var objKeyWords = new Hashtable();
                objKeyWords.Add("SUBAGENTNAME", AgentDetails.SubAgent_Name);
                objKeyWords.Add("STATUS", sStatus);
                var objParser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ManageCustomer/SubAgentStatus.html"), objKeyWords);
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
            return AgentDetails;
        }
        public JsonResult CheckSubAgentName(String SubAgent_Name, int? SubAgent_ID)
        {
            var flag = false;
            if (SubAgent_ID == null)
            {
                SubAgent_ID = 0;
            }
            int _subAgent_Id = (int)SubAgent_ID;
            try
            {
                flag = _ISubAgentManagement.CheckSubAgentExists(SubAgent_Name, _subAgent_Id);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(flag);
        }

        public JsonResult CheckSubAgentRefCode(String SubAgent_Ref_Code, string SubAgent_ID, string CustomerId)
        {

            int _subAgent_ID = 0;
            int _customer_Id = 0;

            var flag = false;

            if (SubAgent_ID != null && SubAgent_ID != "")
            {
                _subAgent_ID = Convert.ToInt32(SubAgent_ID);
            }

            if (CustomerId != null && CustomerId != "")
            {
                _customer_Id = Convert.ToInt32(CustomerId);
            }

            try
            {
                flag = _ISubAgentManagement.CheckSubAgentRefExists(SubAgent_Ref_Code, _subAgent_ID, _customer_Id);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}