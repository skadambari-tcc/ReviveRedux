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
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using OfficeOpenXml;
using System.Net.Mime;
using Revive.Redux.Commn.Excel;
using Newtonsoft.Json;
namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class ReportController : Controller
    {

        #region Private Variables
        private IReportService _IReportService = null;
        private IGeneralService _IGeneralService = null;
        private IOrderManagementService _IOrderManagementService = null;
        private IMachineService _IMachineService = null;
        private IMembershipService _IMembershipService = null;
        private IManufacturersService _IManufacturerService = null;
        public const string Delimiter = "@";
        #endregion

        #region Contructor
        public ReportController()
        {
            _IReportService = new ReportService();
            _IGeneralService = new GeneralService();
            _IOrderManagementService = new OrderManagementService();
            _IMachineService = new MachineService();
            _IMembershipService = new MembershipService();
            _IManufacturerService = new ManufacturersService();
        }

        #endregion

        #region Action
        // GET: Report/Details/
        public ActionResult Details()
        {
            var objReportDetails = _IReportService.GetReports();
            var objReports = AuthReport(objReportDetails);

            return View("Report", objReports);
        }

        public ActionResult ActivityReportDetails()
        {

            var objReportConfigModel = new ReportConfigModel();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            objReportConfigModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();

            objReportConfigModel.SubsidiaryList = _IGeneralService.GetEmptyDDLWithoutSelect();

            objReportConfigModel.SubAgentList = _IGeneralService.GetEmptyDDLWithoutSelect();

            objReportConfigModel.LocationList = _IGeneralService.GetGroups();

            objReportConfigModel.ModeList = _IGeneralService.GetModes(currentUser.User_Id).ToList();
            objReportConfigModel.ModeId = 2; // Default Mode Live
            Session["ActivityReport"] = objReportConfigModel;
            return View("ActivityReportDetails", objReportConfigModel);
        }
        [HttpPost]
        public ActionResult ActivityReportDetails(ReportConfigModel objReportConfigM)
        {
            // var objReportConfigModel = new ReportConfigModel();
            var objReportConfigModel = (ReportConfigModel)Session["ActivityReport"];
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (objReportConfigModel != null)
            //  && objReportDetails.ReportMasterList.Authorization.Contains(GetAuthCode()) Authrization not requried
            {
                if (currentUser.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN ||
                    currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    objReportConfigModel.SubsidiaryList =
                        _IGeneralService.GetSubsidiaryByCustomerId(objReportConfigM.CustomerId,
                            (int)currentUser.SubsidiaryId);
                }
                else
                {
                    objReportConfigModel.SubsidiaryList =
                        _IGeneralService.GetSubsidiaryByCustomerId(objReportConfigM.CustomerId);
                }

                if (currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    objReportConfigModel.SubAgentList =
                        _IGeneralService.GetAgentsBySubsidiaryId(objReportConfigM.SubsidiaryID,
                            (int)currentUser.SubAgentId);
                }
                else
                {
                    objReportConfigModel.SubAgentList =
                        _IGeneralService.GetAgentsBySubsidiaryId(objReportConfigM.SubsidiaryID);
                }

                objReportConfigModel.LocationList = _IGeneralService.GetCustomerLocation(objReportConfigM.CustomerId,
                    objReportConfigM.SubsidiaryID, objReportConfigM.SubAgentID, true);
                objReportConfigModel.IsReportAvailable = true;
            }
            return View("ActivityReportDetails", objReportConfigModel);
        }

        // GET: Report/Details/

        public ActionResult VZ_ActivityReport()
        {
            var objReportConfigModel = new ReportConfigModel();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            objReportConfigModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();

            objReportConfigModel.SubsidiaryList = _IGeneralService.GetEmptyDDLWithoutSelect();

            objReportConfigModel.SubAgentList = _IGeneralService.GetEmptyDDLWithoutSelect();

            objReportConfigModel.LocationList = _IGeneralService.GetGroups();

            objReportConfigModel.ModeList = _IGeneralService.GetModes(currentUser.User_Id).ToList();
            objReportConfigModel.ModeId = 2; // Default Mode Live
            Session["ActivityReport"] = objReportConfigModel;
            return View("VZ_ActivityReport", objReportConfigModel);
        }
        [HttpPost]
        public ActionResult VZ_ActivityReport(ReportConfigModel objReportConfigM)
        {
            // var objReportConfigModel = new ReportConfigModel();
            var objReportConfigModel = (ReportConfigModel)Session["ActivityReport"];
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (objReportConfigModel != null)
            //  && objReportDetails.ReportMasterList.Authorization.Contains(GetAuthCode()) Authrization not requried
            {
                if (currentUser.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN ||
                    currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    objReportConfigModel.SubsidiaryList =
                        _IGeneralService.GetSubsidiaryByCustomerId(objReportConfigM.CustomerId,
                            (int)currentUser.SubsidiaryId);
                }
                else
                {
                    objReportConfigModel.SubsidiaryList =
                        _IGeneralService.GetSubsidiaryByCustomerId(objReportConfigM.CustomerId);
                }

                if (currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    objReportConfigModel.SubAgentList =
                        _IGeneralService.GetAgentsBySubsidiaryId(objReportConfigM.SubsidiaryID,
                            (int)currentUser.SubAgentId);
                }
                else
                {
                    objReportConfigModel.SubAgentList =
                        _IGeneralService.GetAgentsBySubsidiaryId(objReportConfigM.SubsidiaryID);
                }

                objReportConfigModel.LocationList = _IGeneralService.GetCustomerLocation(objReportConfigM.CustomerId,
                    objReportConfigM.SubsidiaryID, objReportConfigM.SubAgentID, true);
                objReportConfigModel.IsReportAvailable = true;
            }
            return View("VZ_ActivityReport", objReportConfigModel);
        }
        // GET: Report/Show/

        public ActionResult Show(int id)
        {
            ViewBag.IsPostBack = false;
            ViewBag.IsModalValid = true;
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            var objReportConfigModel = new ReportConfigModel();
            id = id % 1000;
            var objReportDetails = _IReportService.GetReportById(id);
            objReportDetails = GetReportParam(objReportDetails);
            objReportDetails.ReportId = id;
            objReportDetails.ModeId = 2; // Default Mode Live
            objReportDetails.DateFrom = DateTime.Now;
            objReportDetails.DateTo = DateTime.Now;
            Session["ReportConfigModel"] = objReportDetails;



            if (objReportDetails != null) //  && objReportDetails.ReportMasterList.Authorization.Contains(GetAuthCode()) Authrization not requried
            {
                if (objReportDetails.ReportFilterTypeList.Count() <= 0)
                {
                    ViewBag.LocationList = _IGeneralService.GetCustomerLocationService(objReportDetails.CustomerId, true);
                    //objReportDetails.ReportFilterTypeList = objReportDetails.ReportFilterTypeList;
                    objReportDetails.ReportModelList =
                        _IReportService.ShowReport(objReportDetails.ReportMasterList.ReportId, string.Empty,
                            objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);

                    objReportDetails.IsReportAvailable = true;
                }

                return View("ViewReport", objReportDetails);
            }
            else
            {
                return View("UnAuth");
            }
        }

        // GET: Report/Submit/
        [HttpPost]
        public ActionResult Show(ReportConfigModel objReportConfigM)
        {
            ViewBag.IsPostBack = true;
            var objReportDetails = (ReportConfigModel)Session["ReportConfigModel"];
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            ManageMachineSpecsService objManagermachineSpecService = new ManageMachineSpecsService();
            ViewBag.IsModalValid = true;
            if (objReportConfigM.flag)
            {
                ViewBag.ShowSubmit = false;
            }
            if (objReportDetails != null)
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                if (currentUser.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN || currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    objReportDetails.SubsidiaryList = _IGeneralService.GetSubsidiaryByCustomerId(objReportConfigM.CustomerId, (int)currentUser.SubsidiaryId);
                }
                else
                {
                    objReportDetails.SubsidiaryList = _IGeneralService.GetSubsidiaryByCustomerId(objReportConfigM.CustomerId);
                }

                if (currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    objReportDetails.SubAgentList = _IGeneralService.GetAgentsBySubsidiaryId(objReportConfigM.SubsidiaryID, (int)currentUser.SubAgentId);
                }
                else
                {
                    objReportDetails.SubAgentList = _IGeneralService.GetAgentsBySubsidiaryId(objReportConfigM.SubsidiaryID);
                }
                objReportDetails.MultiDeviceList = _IGeneralService.GetMasterValuesByType("MultiDevice");
                objReportDetails.ReportStatusList = _IGeneralService.GetMasterValuesByType("ReviveStatus");


                ViewBag.LocationList = _IGeneralService.GetCustomerLocation(objReportConfigM.CustomerId, objReportConfigM.SubsidiaryID, objReportConfigM.SubAgentID, true);

                string SubAgentList = "";
                List<DtoList> LocationListByMultiSub = new List<DtoList>();
                if (objReportConfigM.SubAgentIDMulti != null)
                {
                    for (int i = 0; i < objReportConfigM.SubAgentIDMulti.Length; i++)
                    {
                        if (SubAgentList == "")
                        {
                            SubAgentList = objReportConfigM.SubAgentIDMulti[i];
                        }
                        else
                        {
                            SubAgentList = SubAgentList + "#" + objReportConfigM.SubAgentIDMulti[i];
                        }
                        LocationListByMultiSub.AddRange(_IGeneralService.GetCustomerLocation(objReportConfigM.CustomerId, objReportConfigM.SubsidiaryID, Convert.ToInt32(objReportConfigM.SubAgentIDMulti[i]), true));
                    }
                }

                //Report Aggregate Summary
                if (objReportConfigM.ReportId == ReportNames.MemberShipAggregateSummary)
                {
                    ViewBag.LocationList = LocationListByMultiSub;
                    objReportDetails.SubAgentIDMultiValue = SubAgentList;
                }


                //_IGeneralService.GetCustomerLocationService(objReportConfigM.CustomerId, true);
                objReportConfigM.ReportFilterTypeList = objReportDetails.ReportFilterTypeList;
                //objReportDetails.ReportModelList = _IReportService.ShowReport(objReportDetails.ReportMasterList.ReportId, SetReportParam(objReportConfigM), objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);

                objReportDetails.IsReportAvailable = true;

                objReportDetails.MachineList = objManagermachineSpecService.GetShippedMachIds(objReportConfigM.CustomerId, objReportConfigM.LocationId);
                return View("ViewReport", objReportDetails);
                //return View("ViewReport");
            }
            else
            {
                //var objReportDetail = _IReportService.GetReports();
                //return View("Report", objReportDetail);
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Show_Read([DataSourceRequest] DataSourceRequest request, ReportConfigModel objReportConfigM)
        {
            var objReportDetails = (ReportConfigModel)Session["ReportConfigModel"];
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            objReportConfigM.ReportFilterTypeList = objReportDetails.ReportFilterTypeList;
            objReportDetails.IsReportAvailable = true;

            var ReportData = _IReportService.LoadReport(request, objReportDetails.ReportMasterList.ReportId, SetReportParam(objReportConfigM), objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);
            //return Json(ReportData, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(ReportData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
        public ActionResult Read_ActivityReport([DataSourceRequest] DataSourceRequest request, ReportConfigModel objReportConfigM)
        {

            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

            var ReportData = _IReportService.LoadActivityReport(request, objReportConfigM.CustomerId, objReportConfigM.SubsidiaryID, objReportConfigM.SubAgentID, objReportConfigM.LocationId, objReportConfigM.ModeId, objReportConfigM.DateFrom.ToString(), objReportConfigM.DateTo.ToString());
            //return Json(ReportData, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(ReportData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public ActionResult Read_ActivityReportDetails([DataSourceRequest] DataSourceRequest request, ReportConfigModel objReportConfigM)
        {

            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

            var ReportData = _IReportService.LoadActivityReportDetails(request, objReportConfigM.CustomerId, objReportConfigM.SubsidiaryID, objReportConfigM.SubAgentID, objReportConfigM.LocationId, objReportConfigM.ModeId, objReportConfigM.DateFrom.ToString(), objReportConfigM.DateTo.ToString());
            //return Json(ReportData, JsonRequestBehavior.AllowGet);
            // var ReportData = "";
            var jsonResult = Json(ReportData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        [AjaxHandleException]
        public JsonResult GetLocationList(int CustomerId, int SubsidiaryId, int SubAgentId, bool bActive = false)
        {
            var customers = _IGeneralService.GetCustomerLocation(CustomerId, SubsidiaryId, SubAgentId, bActive);
            //_IGeneralService.GetCustomerLocationService(CustomerId, bActive);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public JsonResult GetLocationListForMultiSelect(int CustomerId, int SubsidiaryId, List<string> SubAgentId, bool bActive = false)
        {
            List<DtoList> locationList = new List<DtoList>();
            for (int i = 0; i < SubAgentId.Count; i++)
            {
                if (SubAgentId[i] != string.Empty && SubAgentId[i] != null)
                {
                    var locationlst = _IGeneralService.GetCustomerLocation(CustomerId, SubsidiaryId, Convert.ToInt32(SubAgentId[i].ToString()), bActive);
                    if (locationlst != null)
                    {
                        for (int j = 0; j < locationlst.ToList().Count; j++)
                        {
                            locationList.Add(new DtoList { Id = Convert.ToInt32(locationlst.ElementAtOrDefault(j).Id), Text = locationlst.ElementAtOrDefault(j).Text.ToString() });
                        }
                    }
                }
            }
            return Json(locationList, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public JsonResult GetSubsidiaryList(int CustomeId, bool bActive = false)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            if (currentUser.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN || currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
            {
                var customers = _IGeneralService.GetSubsidiaryByCustomerId(CustomeId, (int)currentUser.SubsidiaryId);
                return Json(customers, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var customers = _IGeneralService.GetSubsidiaryByCustomerId(CustomeId);
                return Json(customers, JsonRequestBehavior.AllowGet);
            }

        }
        [AjaxHandleException]
        public JsonResult GetSubAgentList(int SubsidiaryId, bool bActive = false)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            if (currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
            {
                var customers = _IGeneralService.GetAgentsBySubsidiaryId(SubsidiaryId, (int)currentUser.SubAgentId);
                return Json(customers, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var customers = _IGeneralService.GetAgentsBySubsidiaryId(SubsidiaryId);
                return Json(customers, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetShippedMachinesByCustomer(int customerId, int subsidiaryId, int subAgentId, int locationId)
        {
            IOrderManagementService _IOrderManagementService = new OrderManagementService();
            ManageMachineSpecsService objManagermachineSpecService = new ManageMachineSpecsService();
            var MachineLst = objManagermachineSpecService.GetShippedMachIds(customerId, locationId);
            return Json(MachineLst, JsonRequestBehavior.AllowGet);

        }



        [AjaxHandleException]
        public ActionResult MachineActivityAjax([DataSourceRequest] DataSourceRequest request, string id, string isMember)
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            var objReportDetails = _IReportService.ShowReport(2018, id + "@1", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);
            //   var resultAfterFilter = objReportDetails.Where(x => x.custom5 == isMember).ToList();
            DataSourceResult result = objReportDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MachineActivityCheckInAjax([DataSourceRequest] DataSourceRequest request, string id, string isMember)
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            var objReportDetails = _IReportService.ShowReport(2018, id + "@2", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);
            //   var resultAfterFilter = objReportDetails.Where(x => x.custom5 == isMember).ToList();
            DataSourceResult result = objReportDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MachineActivityDebugValuesAjax([DataSourceRequest] DataSourceRequest request, string id, string isMember)
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            var objReportDetails = _IReportService.ShowReport(2018, id + "@3", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);
            //   var resultAfterFilter = objReportDetails.Where(x => x.custom5 == isMember).ToList();
            DataSourceResult result = objReportDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MachineActivityCheckOutAjax([DataSourceRequest] DataSourceRequest request, string id, string isMember)
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            var objReportDetails = _IReportService.ShowReport(2018, id + "@4", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);
            //   var resultAfterFilter = objReportDetails.Where(x => x.custom5 == isMember).ToList();
            DataSourceResult result = objReportDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Private Methods

        private ReportConfigModel GetReportParam(ReportConfigModel objReportDetails)
        {
            objReportDetails.PageHeader = objReportDetails.ReportMasterList.Name;
            objReportDetails.PageButtonSubmit = "Show Report";


            foreach (var objFilter in objReportDetails.ReportFilterTypeList)
            {
                if (string.Compare(objFilter.FilterType, "Customer", true) == 0)
                {
                    objFilter.IsCustomerAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "Location", true) == 0)
                {
                    objFilter.IsLocationAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "DateFrom", true) == 0)
                {
                    objFilter.IsDateFromAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "DateTo", true) == 0)
                {
                    objFilter.IsDateToAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "DateExpire", true) == 0)
                {
                    objFilter.IsDateExpireAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "Membership", true) == 0)
                {
                    objFilter.IsMembershipStatusAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "Phone", true) == 0)
                {
                    objFilter.IsPhoneManufacturerAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "Order", true) == 0)
                {
                    objFilter.IsOrderAvailable = true;
                }
                if (string.Compare(objFilter.FilterType, "Mode", true) == 0)
                {
                    objFilter.IsModeAvailable = true;


                }
                if (string.Compare(objFilter.FilterType, "Manufacturer", true) == 0)
                {
                    objFilter.IsManufacturerAvailable = true;
                }
                if (string.Compare(objFilter.FilterType, "OrderUnassignedMachines", true) == 0)
                {
                    objFilter.IsOrderMachineUnassigned = true;
                }
                if (string.Compare(objFilter.FilterType, "Subsidiary", true) == 0)
                {
                    objFilter.IsSubsidiaryAvailable = true;
                }
                if (string.Compare(objFilter.FilterType, "SubAgent", true) == 0)
                {
                    objFilter.IsSubAgentAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "SubAgentMulti", true) == 0)
                {
                    objFilter.IsSubAgentMultiAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "LocationGroup", true) == 0)
                {
                    objFilter.IsLocationGroupAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "IsMultiDevice", true) == 0)
                {
                    objFilter.IsMultiDeviceAvailable = true;
                }

                if (string.Compare(objFilter.FilterType, "Status", true) == 0)
                {
                    objFilter.IsReportStatusAvailable = true;
                }





                objFilter.IsMachineAvalable = string.Compare(objFilter.FilterType, "Machine", true) == 0;

                if (objFilter.IsCustomerAvailable)
                {
                    var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                    objReportDetails.CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
                }

                if (objFilter.IsModeAvailable)
                {
                    var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                    if (objReportDetails.ReportFilterTypeList.FirstOrDefault().ReportId == ReportNames.MachineAggregateRate || objReportDetails.ReportFilterTypeList.FirstOrDefault().ReportId == ReportNames.MachineMembersRate || objReportDetails.ReportFilterTypeList.FirstOrDefault().ReportId == ReportNames.MachinePhoneManufacRate)
                    {
                        var ModeList = new List<DtoList>();
                        ModeList = _IGeneralService.GetModes(currentUser.User_Id).ToList();
                        objReportDetails.ModeList = ModeList;//(from n in ModeList where n.Text == "Live" select n).ToList();
                    }
                    else
                    {
                        objReportDetails.ModeList = _IGeneralService.GetModes(currentUser.User_Id).ToList();
                    }
                }
                if (objFilter.IsLocationAvailable)
                {
                    ViewBag.LocationList = _IGeneralService.GetEmptyDDLWithoutSelect();
                }


                if (objFilter.IsPhoneManufacturerAvailable)
                {
                    objReportDetails.PhoneManufacturerList = _IGeneralService.GetPhoneManufacturer();
                }

                if (objFilter.IsOrderAvailable)
                {
                    objReportDetails.OrderList = GetOrderList();
                }
                if (objFilter.IsManufacturerAvailable)
                {
                    objReportDetails.ManufacturerList = _IGeneralService.GetManufacturers();
                }
                if (objFilter.IsOrderMachineUnassigned)
                {
                    objReportDetails.OrderList = GetOrderListUnassignedMachine();
                }



                if (objFilter.IsMachineAvalable)
                {
                    ManageMachineSpecsService obj = new ManageMachineSpecsService();
                    objReportDetails.MachineList = obj.GetShippedMachIds(0, 0);
                }

                if (objFilter.IsSubsidiaryAvailable)
                {

                    objReportDetails.SubsidiaryList = _IGeneralService.GetEmptyDDLWithoutSelect();
                }
                if (objFilter.IsSubAgentAvailable)
                {
                    objReportDetails.SubAgentList = _IGeneralService.GetEmptyDDLWithoutSelect();
                }
                if (objFilter.IsSubAgentMultiAvailable)
                {
                    objReportDetails.SubAgentList = _IGeneralService.GetEmptyDDLWithoutSelect();
                }

                if (objFilter.IsLocationGroupAvailable)
                {
                    objReportDetails.LocationGroupList = _IGeneralService.GetGroups();
                    ViewBag.ShowSubmit = false;
                }
                if (objFilter.IsMultiDeviceAvailable)
                {
                    objReportDetails.MultiDeviceList = _IGeneralService.GetMasterValuesByType("MultiDevice");

                }
                if (objFilter.IsReportStatusAvailable)
                {
                    objReportDetails.ReportStatusList = _IGeneralService.GetMasterValuesByType("ReviveStatus");

                }

            }

            return objReportDetails;
        }

        private string SetReportParam(ReportConfigModel objReportDetails)
        {
            string sReportParam = string.Empty;
            foreach (var objFilter in objReportDetails.ReportFilterTypeList)
            {
                if (string.Compare(objFilter.FilterType, "Customer", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.CustomerId + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "Location", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.LocationId + Delimiter;
                }
                if (string.Compare(objFilter.FilterType, "Subsidiary", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.SubsidiaryID + Delimiter;
                }
                if (string.Compare(objFilter.FilterType, "SubAgent", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.SubAgentID + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "SubAgentMulti", true) == 0)
                {

                    if (objReportDetails.SubAgentIDMulti != null)
                    {
                        sReportParam = sReportParam + objReportDetails.SubAgentIDMulti[0].ToString() + Delimiter;
                    }
                    else
                    {
                        sReportParam = sReportParam + "" + Delimiter;
                    }
                }




                if (string.Compare(objFilter.FilterType, "DateFrom", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.DateFrom + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "DateTo", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.DateTo + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "DateExpire", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.DateExpire + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "Membership", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.MembershipStatus + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "Phone", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.PhoneManufacturerId + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "Order", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.OrderId + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "Machine", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.MachineId + Delimiter;
                }
                if (string.Compare(objFilter.FilterType, "Mode", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.ModeId + Delimiter;
                }
                if (string.Compare(objFilter.FilterType, "Manufacturer", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.ManufacturerId + Delimiter;
                }
                if (string.Compare(objFilter.FilterType, "OrderUnassignedMachines", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.OrderId + Delimiter;
                }
                if (string.Compare(objFilter.FilterType, "LocationGroup", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.GroupId + Delimiter;
                }

                if (string.Compare(objFilter.FilterType, "IsMultiDevice", true) == 0)
                {

                    if (objReportDetails.MultiDeviceTypeCode == "MultiDevice")
                    {
                        sReportParam = sReportParam + "1" + Delimiter;
                    }
                    else if (objReportDetails.MultiDeviceTypeCode == "SingleDevice")
                    {
                        sReportParam = sReportParam + "0" + Delimiter;
                    }

                }

                if (string.Compare(objFilter.FilterType, "Status", true) == 0)
                {
                    sReportParam = sReportParam + objReportDetails.ReportStatusCode + Delimiter;
                    

                }



            }
            if (!string.IsNullOrEmpty(sReportParam))
                sReportParam = sReportParam.Remove(sReportParam.Length - 1, 1);

            return sReportParam;
        }

        private List<ReportMasterModel> AuthReport(List<ReportMasterModel> objReportDetails)
        {
            var objReports = new List<ReportMasterModel>();

            foreach (var item in objReportDetails)
            {
                string sAuth = "," + item.Authorization + ",";
                if (sAuth.Contains(GetAuthCode()))
                {
                    objReports.Add(item);
                }
            }

            return objReports;
        }

        private string GetAuthCode()
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            string sResult = string.Empty;

            if (objCurrentUserDetail != null)
            {
                sResult = objCurrentUserDetail.PageAccessCode;
            }
            return sResult;
        }
        private int GetManufacId()
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            int manufac_ID = 0;
            if (objCurrentUserDetail != null)
            {
                manufac_ID = Convert.ToInt32(objCurrentUserDetail.Manufacturer_Id);
            }
            return manufac_ID;
        }
        private Guid GetUserId()
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            var objResult = new Guid();

            if (objCurrentUserDetail != null)
            {
                objResult = objCurrentUserDetail.User_Id;
            }
            return objResult;
        }

        private List<DtoList> GetOrderList()
        {
            var objOrderList = new List<DtoList>();
            var objOrderDetail = _IOrderManagementService.GetOrdersList(GetAuthCode(), GetUserId(), GetManufacId());




            foreach (var obj in objOrderDetail)
            {
                var objOrders = new DtoList();

                objOrders.Id = (int)obj.JobOrderHeaderId;
                objOrders.Text = obj.JobOrderHeaderId.ToString();

                objOrderList.Add(objOrders);
            }

            return objOrderList;
        }

        private List<DtoList> GetOrderListUnassignedMachine()
        {
            var objOrderList = new List<DtoList>();
            var objOrderDetail = _IOrderManagementService.GetOrdersList(GetAuthCode(), GetUserId(), GetManufacId());
            // Not shipped Order
            var ordersNotShipped = objOrderDetail.Where(d => d.StatusName != OrderStatusType.Shipped).ToList();

            foreach (var obj in ordersNotShipped)
            {
                var objOrders = new DtoList();

                objOrders.Id = (int)obj.JobOrderHeaderId;
                objOrders.Text = obj.JobOrderHeaderId.ToString();

                objOrderList.Add(objOrders);
            }

            return objOrderList;
        }


        #endregion
        #region Report Import Member
        public void storeDocument(IEnumerable<HttpPostedFileBase> files, string dirPath)
        {
            var questionFileName = string.Empty;
            string Filename = "";
            if (files != null)
            {
                foreach (var item in files)
                {
                    var path = string.Format(@"{0}\{1}", dirPath, item.FileName);
                    Filename = Path.GetFileName(item.FileName);
                    if (System.IO.File.Exists(path))
                    {

                        var filetodelete = item.FileName;
                        //Delete File If Already Exist
                        System.IO.File.Delete(path);
                    }
                    // item.SaveAs(path);

                    questionFileName = Path.Combine(Server.MapPath("~/TempUpload/" + dirPath + ""), Filename);
                    item.SaveAs(questionFileName);




                }
            }

        }

        public ActionResult AddMemberByXLSFile(ReportConfigModel objReportConfigModel, IEnumerable<HttpPostedFileBase> files)
        {
            //Session["MembershipWebResult"] = null;
            //var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            //var objcustomerlst = new List<DtoList>();

            //if (objReportConfigModel.CustomerId > 0 && files != null && files.Count() > 0)
            //{

            //    string sDirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Membership\" + objReportConfigModel.CustomerId;
            //    DirectoryInfo dir = new DirectoryInfo(sDirPath);
            //    var sPath = string.Format(@"{0}", sDirPath);
            //    if (!Directory.Exists(sPath))
            //    {
            //        Directory.CreateDirectory(sPath);


            //    }
            //    if (Directory.Exists(sPath))
            //    {
            //        foreach (FileInfo item in dir.GetFiles())
            //        {
            //            item.Delete();
            //        }
            //    }

            //    string dirPath1 = @"\Membership\" + objReportConfigModel.CustomerId.ToString();

            //    storeDocument(files, dirPath1);

            //    foreach (var item in files)
            //    {
            //        objReportConfigModel.FileName = sPath + @"\\" + item.FileName;
            //    }
            //    ReportConfigModel objMembershipResult = new ReportConfigModel();
            //    objReportConfigModel.user_Id = currentUser.User_Id;
            //    bool bResult = _IMembershipService.MemberFileUpload(objReportConfigModel, out objMembershipResult);
            //    Session["MembershipWebResult"] = objMembershipResult;
            //    objMembershipResult.ReportId = 2;
            //    ViewBag.IsModalValid = false;

            //    var objReportDetails = (ReportConfigModel)Session["ReportConfigModel"];
            //    ManageMachineSpecsService objManagermachineSpecService = new ManageMachineSpecsService();
            //    if (objReportDetails != null)
            //    {
            //        objReportDetails.SubsidiaryList = _IGeneralService.GetSubsidiaryByCustomerId(objReportConfigModel.CustomerId);
            //        objReportDetails.SubAgentList = _IGeneralService.GetAgentsBySubsidiaryId(objReportConfigModel.SubsidiaryID);
            //        ViewBag.LocationList = _IGeneralService.GetCustomerLocation(objReportConfigModel.CustomerId, objReportConfigModel.SubsidiaryID, objReportConfigModel.SubAgentID, true);
            //        objReportConfigModel.ReportFilterTypeList = objReportDetails.ReportFilterTypeList;
            //        objReportDetails.ReportModelList = _IReportService.ShowReport(objReportDetails.ReportMasterList.ReportId, SetReportParam(objReportConfigModel), currentUser.User_Id, currentUser.PageAccessCode);
            //        objReportDetails.IsReportAvailable = true;
            //        objReportDetails.MachineList = objManagermachineSpecService.GetShippedMachIds(objReportConfigModel.CustomerId, objReportConfigModel.LocationId);
            //        return View("ViewReport", objReportDetails);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Login", "Account");
            //    }

            //    return View("ViewReport", objMembershipResult);
            //}

            return View("ViewReport", "Report");
        }

        //[AjaxHandleException]
        public ActionResult MemberResultAjax([DataSourceRequest] DataSourceRequest request)
        {
            var objMembershipWebResult = new ManageMember();
            if (Session["MembershipWebResult"] != null)
            {
                objMembershipWebResult = (ManageMember)Session["MembershipWebResult"];

            }

            DataSourceResult result = objMembershipWebResult.LocationResult.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region ExcelMethod
        [AjaxHandleException]
        public JsonResult ReportToExcel(ReportConfigModel objReportConfigM)
        {
            var objReportDetails = (ReportConfigModel)Session["ReportConfigModel"];
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            objReportConfigM.ReportFilterTypeList = objReportDetails.ReportFilterTypeList;
            objReportDetails.IsReportAvailable = true;
            var ReportData = _IReportService.ShowReport(objReportDetails.ReportMasterList.ReportId, SetReportParam(objReportConfigM), objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode);

            if (objReportConfigM.GridReportId == "18")
            {
                ReportData.ForEach(m => m.ActivityType = (_IReportService.ShowReport(2018, m.custom10 + "@1", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode)));
                ReportData.ForEach(m => m.CheckInDetails = (_IReportService.ShowReport(2018, m.custom10 + "@2", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode)));
                ReportData.ForEach(m => m.MachineActivity = (_IReportService.ShowReport(2018, m.custom10 + "@3", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode)));
                ReportData.ForEach(m => m.CheckOutDetails = (_IReportService.ShowReport(2018, m.custom10 + "@4", objCurrentUserDetail.User_Id, objCurrentUserDetail.PageAccessCode)));

                var objMachineActivityLst = BindMachineActivityForExcel(ReportData);
                var objRootLst = BindCoulumntoModel_MachineActivity();

                var modelObject = JsonConvert.DeserializeObject<List<RootObject>>(objReportConfigM.GridModel);
                int modelCount = modelObject.Count;
                int Countdiff = modelCount - 7;
                modelObject.RemoveRange(7, Countdiff);//remove extra column from model
                modelObject.AddRange(objRootLst);

                var javaScriptSerializerMemberActivity = new System.Web.Script.Serialization.JavaScriptSerializer();
                javaScriptSerializerMemberActivity.MaxJsonLength = 1360065408;

                string jsonStringTemplate = javaScriptSerializerMemberActivity.Serialize(modelObject);
                string jsonStringMemberActivity = javaScriptSerializerMemberActivity.Serialize(objMachineActivityLst);

                gridToExcel(jsonStringTemplate, jsonStringMemberActivity, objReportConfigM.GridTitle, objReportConfigM.GridReportId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = Json(ReportData, JsonRequestBehavior.AllowGet);
            // jsonResult.MaxJsonLength = 2097152000;//int.MaxValue;
            // jsonResult.ContentType = "application/json";

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = 1360065408;
            string jsonString = javaScriptSerializer.Serialize(ReportData);
            gridToExcel(objReportConfigM.GridModel, jsonString, objReportConfigM.GridTitle, objReportConfigM.GridReportId);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #region Report Raw Data
        public JsonResult ReportToExcelRawData(ReportConfigModel objReportConfigM)
        {
            var objReportDetails = (ReportConfigModel)Session["ReportConfigModel"];
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            objReportConfigM.ReportFilterTypeList = objReportDetails.ReportFilterTypeList;
            objReportDetails.IsReportAvailable = true;
            var ReportData = _IReportService.ExportActivityRawData(objReportConfigM.CustomerId, objReportConfigM.SubsidiaryID, objReportConfigM.SubAgentID, objReportConfigM.LocationId, objReportConfigM.ModeId, objReportConfigM.DateFrom.ToString(), objReportConfigM.DateTo.ToString());



            var objMachineActivityLst = BindMachineActivityRawDataForExcel(ReportData);

            var objRootLst = BindCoulumntoModel_MachineActivityRawData();

            //var modelObject = JsonConvert.DeserializeObject<List<RootObject>>(objReportConfigM.GridModel);
            //int modelCount = modelObject.Count;
            //int Countdiff = modelCount - 7;
            //modelObject.RemoveRange(7, Countdiff);//remove extra column from model
            //modelObject.AddRange(objRootLst);

            var javaScriptSerializerMemberActivity = new System.Web.Script.Serialization.JavaScriptSerializer();
            javaScriptSerializerMemberActivity.MaxJsonLength = 1360065408;

            string jsonStringTemplate = javaScriptSerializerMemberActivity.Serialize(objRootLst);
            string jsonStringMemberActivity = javaScriptSerializerMemberActivity.Serialize(objMachineActivityLst);

            gridToExcelRawData(jsonStringTemplate, jsonStringMemberActivity, "MachineActivityRawData", objReportConfigM.GridReportId);
            return Json(true, JsonRequestBehavior.AllowGet);

            
        }

        public List<MachineActivityRawData> BindMachineActivityRawDataForExcel(List<ActivityReport> ReportData)
        {
            List<MachineActivityRawData> objMachineActivityLst = new List<MachineActivityRawData>();
            for (int i = 0; i < ReportData.Count(); i++)
            {
                if (ReportData[i] != null)
                {
                    MachineActivityRawData objMachineActivityDetails = new MachineActivityRawData();

                    objMachineActivityDetails.ActivityDate = ReportData[i].ActivityDate;
                    objMachineActivityDetails.ProcessId = ReportData[i].ProcessId;
                    objMachineActivityDetails.MachineActivityId = ReportData[i].MachineActivityId;
                    objMachineActivityDetails.MachineId = ReportData[i].MachineId;
                    objMachineActivityDetails.UserName = ReportData[i].UserName;
                    objMachineActivityDetails.MemberName = ReportData[i].MemberName;
                    objMachineActivityDetails.Email = ReportData[i].Email;
                    objMachineActivityDetails.PhoneNumber = ReportData[i].PhoneNumber;
                    objMachineActivityDetails.Membership = ReportData[i].Membership;
                    objMachineActivityDetails.InvoiceNo = ReportData[i].InvoiceNo;
                    objMachineActivityDetails.MfrType = ReportData[i].MfrType;
                    objMachineActivityDetails.PluginYes = ReportData[i].PluginYes;
                    objMachineActivityDetails.ReviveSuccessStatus = ReportData[i].ReviveSuccessStatus;
                    objMachineActivityDetails.ModeName = ReportData[i].ModeName;
                    objMachineActivityDetails.TimeSincePeril = ReportData[i].TimeSincePeril;
                    objMachineActivityDetails.ActivityResults = ReportData[i].ActivityResults;
                    objMachineActivityDetails.CheckOutResults = ReportData[i].CheckOutResults;


                  
                    objMachineActivityLst.Add(objMachineActivityDetails);
                }
            }
            return objMachineActivityLst;
        }
        public List<RootObject> BindCoulumntoModel_MachineActivityRawData()
        {
            List<RootObject> objRootLst = new List<RootObject>();

          

            //Checkin Details
            var col1 = MapRootTable("Process Id");
            var col2 = MapRootTable("Activity Date");
            var col3 = MapRootTable("Machine Activity Id");
            var col4 = MapRootTable("Machine Id");
            var col5 = MapRootTable("User Name");
            var col6 = MapRootTable("Member Name");
            var col7 = MapRootTable("Membership");
            var col8 = MapRootTable("Email");

            //Machine Activity
            var col9 = MapRootTable("PhoneNumber");
            var col10 = MapRootTable("Invoice No");
            var col11 = MapRootTable("MfrType");
            var col12 = MapRootTable("PluginYes");
            var col13 = MapRootTable("ReviveSuccessStatus");
            var col14 = MapRootTable("Mode Name");
            var col15 = MapRootTable("TimeSincePeril");
            var col16 = MapRootTable("CheckOut Results");
            var col17 = MapRootTable("Activity Results");

            objRootLst.Add(col2);
            objRootLst.Add(col1);
            objRootLst.Add(col3);
            objRootLst.Add(col4);
            objRootLst.Add(col5);
            objRootLst.Add(col6);
            objRootLst.Add(col7);
            objRootLst.Add(col8);

            objRootLst.Add(col9);
            objRootLst.Add(col10);
            objRootLst.Add(col11);
            objRootLst.Add(col12);
            objRootLst.Add(col13);
            objRootLst.Add(col14);
            objRootLst.Add(col15);
            objRootLst.Add(col16);
            objRootLst.Add(col17);
            return objRootLst;
        }
        #endregion
        /// <summary>
        /// CR 21
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        /// <param name="title"></param>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        private bool gridToExcel(string model, string data, string title, string ReportId)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                GridToExcel gridToExcel = new GridToExcel();
                if (ReportId != "")
                {
                    ReportService objRepservice = new ReportService();
                    ReportConfigModel obj = null;
                    obj = objRepservice.GetReportById(Convert.ToInt32(ReportId));
                    if (obj != null)
                    {
                        gridToExcel.GenerateExcelFromGrid_Report(model, data, title, excelPackage, obj);
                    }
                    //gridToExcel.GenerateExcelFromGrid_Report(model, data, title, excelPackage, ReportId);
                }
                else
                {
                    gridToExcel.GenerateExcelFromGrid(model, data, title, excelPackage);
                }
                Session[title.Trim()] = excelPackage.GetAsByteArray();
            }
            return true;
        }

        private bool gridToExcelRawData(string model, string data, string title, string ReportId)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                GridToExcel gridToExcel = new GridToExcel();

                gridToExcel.GenerateExcelFromGrid_ReportRawData(model, data, title, excelPackage, null);
                Byte[] ExcelFileData = excelPackage.GetAsByteArray();

                Session[title.Trim()] = ExcelFileData;
              
            }
            return true;
        }


        #endregion

        #region Machine Activity Excel Methods

        

        public List<MachineActivity_Details> BindMachineActivityForExcel(List<ReportModel> ReportData)
        {
            List<MachineActivity_Details> objMachineActivityLst = new List<MachineActivity_Details>();
            for (int i = 0; i < ReportData.Count(); i++)
            {
                if (ReportData[i] != null)
                {
                    MachineActivity_Details objMachineActivityDetails = new MachineActivity_Details();
                    objMachineActivityDetails.custom1 = ReportData[i].custom1;
                    objMachineActivityDetails.custom2 = ReportData[i].custom2;
                    objMachineActivityDetails.custom3 = ReportData[i].custom3;
                    objMachineActivityDetails.custom4 = ReportData[i].custom4;
                    objMachineActivityDetails.custom5 = ReportData[i].custom5;
                    objMachineActivityDetails.custom6 = ReportData[i].custom6;
                    objMachineActivityDetails.custom7 = ReportData[i].custom7;

                    if (ReportData[i].ActivityType != null && ReportData[i].ActivityType.Count > 1)
                    {
                        for (int k = 0; k < ReportData[i].ActivityType.Count; k++)
                        {
                            objMachineActivityDetails.ActivityType += (ReportData[i].ActivityType[k].custom1 != null ? ReportData[i].ActivityType[k].custom1 : "") + " = " + (ReportData[i].ActivityType[k].custom2 != null ? ReportData[i].ActivityType[k].custom2 : "") + " \n ";
                        }
                    }

                    if (ReportData[i].CheckInDetails != null && ReportData[i].CheckInDetails.Count > 0)
                    {
                        objMachineActivityDetails.MembershipType = ReportData[i].CheckInDetails[0].custom2;
                        objMachineActivityDetails.MembershipId = ReportData[i].CheckInDetails[1].custom2;
                        objMachineActivityDetails.Member = ReportData[i].CheckInDetails[2].custom2;
                        objMachineActivityDetails.EmailId = ReportData[i].CheckInDetails[3].custom2;
                        objMachineActivityDetails.DeviceManufacturer = ReportData[i].CheckInDetails[4].custom2;
                        objMachineActivityDetails.HowLongAgo = ReportData[i].CheckInDetails[5].custom2;
                        objMachineActivityDetails.pluggedIn = ReportData[i].CheckInDetails[6].custom2;
                        objMachineActivityDetails.PhoneNumber = ReportData[i].CheckInDetails[7].custom2;
                    }

                    if (ReportData[i].MachineActivity != null && ReportData[i].MachineActivity.Count > 1)
                    {
                        objMachineActivityDetails.AccumulatedRHValue = ReportData[i].MachineActivity[0].custom2 != null ? ReportData[i].MachineActivity[0].custom2 : "";
                        objMachineActivityDetails.TotalCycles = ReportData[i].MachineActivity[1].custom2 != null ? ReportData[i].MachineActivity[1].custom2 : "";
                        objMachineActivityDetails.RemainingCycles = ReportData[i].MachineActivity[2].custom2 != null ? ReportData[i].MachineActivity[2].custom2 : "";
                        objMachineActivityDetails.KPIMaximumVacuum = ReportData[i].MachineActivity[3].custom2 != null ? ReportData[i].MachineActivity[3].custom2 : "";
                        objMachineActivityDetails.KPItimetoMaximumVacuum = ReportData[i].MachineActivity[4].custom2 != null ? ReportData[i].MachineActivity[4].custom2 : "";
                        objMachineActivityDetails.KPIInitialInjectionROR = ReportData[i].MachineActivity[5].custom2 != null ? ReportData[i].MachineActivity[5].custom2 : "";
                        objMachineActivityDetails.KPIPlatenROR = ReportData[i].MachineActivity[6].custom2 != null ? ReportData[i].MachineActivity[6].custom2 : "";
                        objMachineActivityDetails.KPIMinimumChamberHumidity = ReportData[i].MachineActivity[7].custom2 != null ? ReportData[i].MachineActivity[7].custom2 : "";
                        objMachineActivityDetails.KPIMaximumChamberHumidity = ReportData[i].MachineActivity[8].custom2 != null ? ReportData[i].MachineActivity[8].custom2 : "";
                        objMachineActivityDetails.KPIMaximumCharge = ReportData[i].MachineActivity[9].custom2 != null ? ReportData[i].MachineActivity[9].custom2 : "";
                        objMachineActivityDetails.KPINumberofCycles = ReportData[i].MachineActivity[10].custom2 != null ? ReportData[i].MachineActivity[10].custom2 : "";
                        objMachineActivityDetails.KPIDuration = ReportData[i].MachineActivity[11].custom2 != null ? ReportData[i].MachineActivity[11].custom2 : "";
                        objMachineActivityDetails.KPIProcessID = ReportData[i].MachineActivity[12].custom2 != null ? ReportData[i].MachineActivity[12].custom2 : "";
                        objMachineActivityDetails.KPIMaximumAmbientTemperatur = ReportData[i].MachineActivity[13].custom2 != null ? ReportData[i].MachineActivity[13].custom2 : "";
                        objMachineActivityDetails.ProcessState = ReportData[i].MachineActivity[14] != null ? ReportData[i].MachineActivity[14].custom2 : "";
                    }

                    if (ReportData[i].CheckOutDetails != null && ReportData[i].CheckOutDetails.Count > 1)
                    {
                        for (int j = 0; j < ReportData[i].CheckOutDetails.Count; j++)
                        {
                            objMachineActivityDetails.CheckoutDetails += (ReportData[i].CheckOutDetails[j].custom1 != null ? ReportData[i].CheckOutDetails[j].custom1 : "") + " = " + (ReportData[i].CheckOutDetails[j].custom2 != null ? ReportData[i].CheckOutDetails[j].custom2 : "") + " \n ";
                        }
                    }
                    objMachineActivityLst.Add(objMachineActivityDetails);
                }
            }
            return objMachineActivityLst;
        }

        public List<RootObject> BindCoulumntoModel_MachineActivity()
        {
            List<RootObject> objRootLst = new List<RootObject>();

            //Activity Type
            var ActivityType = MapRootTable("Activity Type");

            //Checkin Details
            var objRtMT = MapRootTable("Membership Type");
            var objRtMI = MapRootTable("Membership Id");
            var objRtM = MapRootTable("Member");
            var objRtE = MapRootTable("Email Id");
            var objRtDM = MapRootTable("Device Manufacturer");
            var objRtHLA = MapRootTable("HowLong Ago");
            var objRtPI = MapRootTable("plugged In");
            var objRtPN = MapRootTable("Phone Number");

            //Machine Activity
            var objRtARV = MapRootTable("Accumulated RH Value");
            var objRtTC = MapRootTable("Total Cycles");
            var objRtRM = MapRootTable("Remaining Cycles");
            var objRtMaxVac = MapRootTable("KPI Maximum Vacuum");
            var objRtTtMaxVac = MapRootTable("KPI time to Maximum Vacuum");
            var objRtIIR = MapRootTable("KPI Initial Injection ROR");
            var objRtPR = MapRootTable("KPI Platen ROR");
            var objRtMinCH = MapRootTable("KPI Minimum Chamber Humidity");
            var objRtMaxCH = MapRootTable("KPI Maximum Chamber Humidity");
            var objRtMaxC = MapRootTable("KPI Maximum Charge");
            var objRtNC = MapRootTable("KPI Number of Cycles");
            var objRtDur = MapRootTable("KPI Duration");
            var objRtPorId = MapRootTable("KPI Process ID");
            var objRtMaxAT = MapRootTable("KPI Maximum Ambient Temperatur");
            var objRtPrSt = MapRootTable("Process State");

            //Checkout Details
            var objRtHB = MapRootTable("Checkout Details");

            objRootLst.Add(ActivityType);

            objRootLst.Add(objRtMT);
            objRootLst.Add(objRtMI);
            objRootLst.Add(objRtM);
            objRootLst.Add(objRtE);
            objRootLst.Add(objRtDM);
            objRootLst.Add(objRtHLA);
            objRootLst.Add(objRtPI);
            objRootLst.Add(objRtPN);

            objRootLst.Add(objRtARV);
            objRootLst.Add(objRtTC);
            objRootLst.Add(objRtRM);
            objRootLst.Add(objRtMaxVac);
            objRootLst.Add(objRtTtMaxVac);
            objRootLst.Add(objRtIIR);
            objRootLst.Add(objRtPR);
            objRootLst.Add(objRtMinCH);
            objRootLst.Add(objRtMaxCH);
            objRootLst.Add(objRtMaxC);
            objRootLst.Add(objRtNC);
            objRootLst.Add(objRtDur);
            objRootLst.Add(objRtPorId);
            objRootLst.Add(objRtMaxAT);
            objRootLst.Add(objRtPrSt);

            objRootLst.Add(objRtHB);

            return objRootLst;
        }

        public RootObject MapRootTable(string Name)
        {
            RootObject objrt = new RootObject();
            objrt.title = Name;
            objrt.field = Name.Replace(" ", "");
            return objrt;
        }

        #endregion
    }
}
