using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Revive.Redux.Services;
using Revive.Redux.Entities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Common;
using System.Web.Hosting;
using System.Configuration;
using System.Collections;


namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class ModeConfigurationController : Controller
    {
        private IGeneralService _generalService = null;

        public ModeConfigurationController()
        {
            _generalService = new GeneralService();

            ViewBag.Locationlst = _generalService.GetEmptyDDL();
        }

        public ActionResult Details()
        {
            ViewBag.CustomerLst = bindCustomer(); // Bind customer
            ModeConfigurationModel objModeConfigurationList = new ModeConfigurationModel();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.IsModalValid = true;
            objModeConfigurationList.CustomerNameList = _generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
            objModeConfigurationList.LocationList = _generalService.GetEmptyDDLWithoutSelect();
            objModeConfigurationList.SubsidiaryList = _generalService.GetEmptyDDLWithoutSelect();
            objModeConfigurationList.SubAgentList = _generalService.GetEmptyDDLWithoutSelect();
            return View(objModeConfigurationList);
        }

        public JsonResult GetLocationList(int Customer_Id)
        {
            var LocationLst = _generalService.GetCustomerLocationService(Customer_Id);
            return Json(LocationLst, JsonRequestBehavior.AllowGet);

        }

        private List<DtoList> GetLocations(int custID)
        {
            var LocationLst = _generalService.GetCustomerLocationService(custID).ToList();
            return LocationLst;
        }

        private List<DtoList> bindCustomer()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            // var objcustomerlst = new List<DtoList>();

            var objcustomerlst = _generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();

            if (objcustomerlst.Count == 0)
            {
                objcustomerlst = _generalService.GetEmptyDDL().ToList();
            }



            return objcustomerlst;

        }

        public ActionResult ModeConfig_Read([DataSourceRequest] DataSourceRequest request, ModeConfigurationModel objModeConfigDetails)
        {
            var modes = new List<ModeConfigurationModel>();
            modes = _generalService.GetAllConfigModes(objModeConfigDetails);
            DataSourceResult result = modes.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewModeConfiguration()
        {
            ViewBag.CustomerLst = bindCustomer(); // Bind customer
            ViewBag.AgentNameList = _generalService.GetEmptyDDL();
            ViewBag.SubsidiaryNameList = _generalService.GetEmptyDDL();
            return View();
        }

        public ActionResult SaveNewModeConfig(ModeConfigurationModel newModeConfig)
        {
            string mesg = "";
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null)
            {
                if (newModeConfig != null)
                {
                    newModeConfig.Created_by = currentUser.User_Id;
                    mesg = _generalService.AddNewModeConfig(newModeConfig);
                    TempData["Success"] = ReviveMessages.Add;
                }
            }
            return RedirectToAction("Details");
        }

        public ActionResult EditModeConfig(int Id)
        {
            var existingModeConfig = new ModeConfigurationModel();
            if (Id != 0)
            {

                existingModeConfig = _generalService.GetModeConfigById(Id);
                ViewBag.CustomerLst = bindCustomer();
                ViewBag.Locationlst = GetLocations(existingModeConfig.customerID.Value);
                ViewBag.AgentNameList = _generalService.GetAgentsBySubsidiaryId(existingModeConfig.SubsidiaryId);
                ViewBag.SubsidiaryNameList = _generalService.GetSubsidiaryByCustomerId(existingModeConfig.customerID.Value);
            }
            return View(existingModeConfig);
        }

        public ActionResult UpdateModeConfig(ModeConfigurationModel modeConfig)
        {
            string mesg = "";
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null)
            {
                if (modeConfig != null)
                {
                    modeConfig.Modified_by = currentUser.User_Id;
                    var flag = _generalService.UpdateModeConfig(modeConfig);
                    if (flag == true)
                    {
                        mesg = ReviveMessages.Update;
                        TempData["Success"] = mesg;
                    }
                }
            }
            return RedirectToAction("Details");
        }

        public JsonResult ValidateReduxData(string loc, string from, string to, string modeId = "")
        {
            var isExists = false;
            var intLoc = Convert.ToInt32(loc);
            var fromDate = Convert.ToDateTime(from);
            var toDate = Convert.ToDateTime(to);
            int intModeId = 0;
            if (!string.IsNullOrEmpty(modeId))
            {
                intModeId = Convert.ToInt32(modeId);
            }
            isExists = _generalService.ValidateData(intLoc, fromDate, toDate, intModeId);
            return Json(new { isDuplicate = isExists });
            //return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetLocationList(int Customer_Id)
        //{
        //    var LocationLst = _generalService.GetCustomerLocationService(Customer_Id);
        //    return Json(LocationLst, JsonRequestBehavior.AllowGet);

        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult ModeConfig_Delete([DataSourceRequest] DataSourceRequest request, ModeConfigurationModel modeConfig)
        //{

        //        if (modeConfig != null)
        //        {
        //            _generalService.DeleteModeConfig(modeConfig);
        //        }
        //        return Json(new[] { modeConfig }.ToDataSourceResult(request, ModelState));
        //    }

        public ActionResult ModeConfig_Delete(string modeID)
        {
            if (!string.IsNullOrEmpty(modeID))
            {
                _generalService.DeleteModeConfig(Convert.ToInt32(modeID));
                return Json("Deleted Successfully!", JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Details");
        }

        [HttpPost]
        public JsonResult GetSubsidiaryByCustomer(int CustomerId)
        {
            try
            {
                var ListSubsidary = _generalService.GetSubsidiaryByCustomerId(CustomerId);
                return Json(ListSubsidary, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetAgentBySubsidiary(int SubsidiaryId)
        {
            try
            {
                var ListAgent = _generalService.GetAgentsBySubsidiaryId(SubsidiaryId);
                return Json(ListAgent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetCustomerLocationBySubsidiary(int CustomerId, int SubsidiaryId, bool IsActive = false)
        {
            try
            {
                var LisLocation = _generalService.GetCustomerLocation(CustomerId, SubsidiaryId, IsActive);
                return Json(LisLocation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public JsonResult GetCustomerLocationbyAgent(int CustomerId, int SubsidiaryId, int AgentId, bool IsActive = false)
        {
            try
            {
                var LisLocation = _generalService.GetCustomerLocation(CustomerId, SubsidiaryId, AgentId, IsActive);
                return Json(LisLocation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [AjaxHandleException]
        public JsonResult GetSubsidiaryList(int CustomeId, bool bActive = false)
        {
            var customers = _generalService.GetSubsidiaryByCustomerId(CustomeId);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
        [AjaxHandleException]
        public JsonResult GetSubAgentList(int SubsidiaryId, bool bActive = false)
        {
            var customers = _generalService.GetAgentsBySubsidiaryId(SubsidiaryId);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public JsonResult GetLocationListByCus_Subsidary_SubAgent(int CustomerId, int SubsidiaryId, int SubAgentId, bool bActive = false)
        {
            var customers = _generalService.GetCustomerLocation(CustomerId, SubsidiaryId, SubAgentId, bActive);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddMultipleDemos(ModeConfigurationModel newModeConfig, List<ModeConfigurationModel> data)
        {
            var result = "Demos no added";
            if (data != null && newModeConfig.locationID != 0)
            {
                result = _generalService.AddMultipleModeConfig(newModeConfig, data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}