using Revive.Redux.Services;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Revive.Redux.Common;
using Revive.Redux.Controllers.Common;
using System.Text;

namespace Revive.Redux.UI.Controllers.ManageShipping
{
    [Authorize]
    [ReviveAuth]
    [ReviveHandleErrorAttribute]
    public class ManageShippingController : Controller
    {
        private IUserManagmentService _ManageUserService = null;
        private IGeneralService _GeneralService = null;
        private IManageShippingService manageShippingService = null;
        private IMachineService machineService = null;
        // GET: ManageShipping
        public ManageShippingController()
        {

            try
            {
                _ManageUserService = new UserManagementService();
                _GeneralService = new GeneralService();
                manageShippingService = new ManageShippingService();
                machineService = new MachineService();
            }
            catch (Exception ex)
            {
                string s = ex.Message.ToString();
            }
            ViewBag.Servicelst = _GeneralService.GetServiceType();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewGroupShippingQueue()
        {
            ManageGroupShippingQueueModel objGroupShipModel = new ManageGroupShippingQueueModel();
            objGroupShipModel.GroupList = _GeneralService.GetGroups();
            return View("ViewGroupShippingQueue", objGroupShipModel);
        }

        public ActionResult AddGroupShippingQueue()
        {
            return View();
        }

        public ActionResult ManageGroupShippingQueue()
        {
            // var result = GetUpsResponse();
            return View();
        }

        public ActionResult ManageShippingConfiguration()
        {
            // var result = GetUpsResponse();
            return View();
        }
        //[HttpGet]
        public ActionResult AddShipping()
        {
            return View();
        }
        public ActionResult SubmitShipping(ShippingConfigModel Model)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (Model != null && !Model.IsEdit)
            {
                Model.CurrentUserId = currentUser.User_Id;
                var result = manageShippingService.AddShippingConfiguration(Model);
                TempData["isCreatedSuccess"] = true;
            }
            else
            {
                Model.CurrentUserId = currentUser.User_Id;
                var result = manageShippingService.UpdateShippingConfiguration(Model);
                TempData["isUpdatedSuccess"] = true;
            }
            return RedirectToAction("ManageShippingConfiguration");
        }

        public ActionResult EditShippingConfig(int Id)
        {
            ShippingConfigModel viewmodel = new ShippingConfigModel();
            if (Id > 0)
            {
                viewmodel = manageShippingService.GetShippingConfigurationById(Id);
                viewmodel.IsEdit = true;
            }
            return View("AddShipping", viewmodel);

        }

        public ActionResult EditUPSSetting()
        {
            UPSSettings model = new UPSSettings();

            var viewmodel = manageShippingService.GetUPSSettings();
            if (viewmodel != null)
            {
                model = viewmodel;
            }


            return View("UPSSettings", model);

        }
        public ActionResult SubmitUPSSettings(UPSSettings Model)
        {
            UPSSettings viewmodel = new UPSSettings();
            if (Model != null)
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                Model.CurrentUserId = currentUser.User_Id;
                viewmodel = manageShippingService.UpdateUPSSettings(Model);
                TempData["isUpdatedSuccess"] = true;

            }
            return RedirectToAction("ManageShippingConfiguration");

        }

        [AjaxHandleException]
        public ActionResult GetShippingConfigurations([DataSourceRequest] DataSourceRequest request)
        {

            var shippingDetailsData = manageShippingService.GetShippingConfigurations();
            DataSourceResult result = shippingDetailsData.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [AjaxHandleException]
        public ActionResult GetShippingQueue([DataSourceRequest] DataSourceRequest request)
        {

            var shippingDetailsData = manageShippingService.GetShippingQueueDetails();
            DataSourceResult result = shippingDetailsData.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [AjaxHandleException]
        public ActionResult GetShippingQueueShipMachinePage([DataSourceRequest] DataSourceRequest request)
        {

            var shippingDetailsData = manageShippingService.GetShippableCountByCustomerGroup();
            DataSourceResult result = shippingDetailsData.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        private ShipmentResponseData GetUpsResponse(ShipMachineModel objShipModel)
        {
            Revive.Redux.UI.WebApi.UPS.ShipClient obj = new Revive.Redux.UI.WebApi.UPS.ShipClient();

            ShipmentResponseData ShipmentResponseObj = new ShipmentResponseData();
            var ShipMenetRequestData = MapValuesShipmentRequest(objShipModel);
            ShipmentResponseObj = obj.MakeShipment(ShipMenetRequestData);
            return ShipmentResponseObj;
        }

        private ShipmentRequestData MapValuesShipmentRequest(ShipMachineModel objShipModel)
        {
            ShipmentRequestData ShipmentRequestObj = new ShipmentRequestData();
            ShipmentRequestObj.AccessLicenseNumber = "5D1619F5B3553568";

            UPSSettings model = new UPSSettings();


            var viewmodel = manageShippingService.GetUPSSettings();
            if (viewmodel != null)
            {
                ShipmentRequestObj.UserName = viewmodel.UserName;
                ShipmentRequestObj.Psw = viewmodel.Psw;
                ShipmentRequestObj.ShipmentDescription = viewmodel.ShipmentDescription;
                ShipmentRequestObj.ShipperNumber = viewmodel.ShipperNumber;
                ShipmentRequestObj.AccountNumber = viewmodel.AccountNumber;
                ShipmentRequestObj.ShipperAddressLine = viewmodel.ShipperAddressLine;
                ShipmentRequestObj.ShipperCity = viewmodel.ShipperCity;
                ShipmentRequestObj.ShipperPostalCode = viewmodel.ShipperPostalCode.ToString();
                ShipmentRequestObj.ShipperStateProvinceCode = viewmodel.ShipperStateProvinceCode;
                ShipmentRequestObj.ShipperCountryCode = viewmodel.ShipperCountryCode;
                ShipmentRequestObj.ShipperName = viewmodel.ShipperName;
                ShipmentRequestObj.ShipperAttentionName = viewmodel.ShipperAttentionName;
                ShipmentRequestObj.ShipperPhone = viewmodel.ShipperPhone.ToString();
                ShipmentRequestObj.ShipmentChargeType = viewmodel.ShipmentChargeType;
                ShipmentRequestObj.ServiceCode = viewmodel.ServiceCode;


                // Static As per client
                ShipmentRequestObj.ShipFromAddressLine = viewmodel.ShipFromAddressLine;
                ShipmentRequestObj.ShipFromCity = viewmodel.ShipFromCity;
                ShipmentRequestObj.ShipFromPostalCode = viewmodel.ShipFromPostalCode.ToString();
                ShipmentRequestObj.ShipFromStateProvinceCode = viewmodel.ShipFromStateProvinceCode;
                ShipmentRequestObj.ShipFromCountryCode = viewmodel.ShipFromCountryCode;
                ShipmentRequestObj.ShipFromAttentionName = viewmodel.ShipFromAttentionName;
                ShipmentRequestObj.ShipFromName = viewmodel.ShipFromName;
            }
            else
            {
                ShipmentRequestObj.UserName = "Redux@API";
                ShipmentRequestObj.Psw = "Doubie1!";
                ShipmentRequestObj.ShipmentDescription = "Machine deployment";
                ShipmentRequestObj.ShipperNumber = "899V5A";
                ShipmentRequestObj.AccountNumber = "899V5A";
                ShipmentRequestObj.ShipperAddressLine = "525 Congressional Blvd.";
                ShipmentRequestObj.ShipperCity = "Carmel";
                ShipmentRequestObj.ShipperPostalCode = "21093";
                ShipmentRequestObj.ShipperStateProvinceCode = "MD";
                ShipmentRequestObj.ShipperCountryCode = "US";
                ShipmentRequestObj.ShipperName = "Redux";
                ShipmentRequestObj.ShipperAttentionName = "Manufacturing";
                ShipmentRequestObj.ShipperPhone = "3178199078";

                // Static As per client
                ShipmentRequestObj.ShipFromAddressLine = "1475 S. Wheeling Road";
                ShipmentRequestObj.ShipFromCity = "Wheeling";
                ShipmentRequestObj.ShipFromPostalCode = "60090";
                ShipmentRequestObj.ShipFromStateProvinceCode = "IL";
                ShipmentRequestObj.ShipFromCountryCode = "US";
                ShipmentRequestObj.ShipFromAttentionName = "James Wozniak";
                ShipmentRequestObj.ShipFromName = "Redux";
            }

            // Dynamic  as per Location Master
            ShipmentRequestObj.ShipToAddressLine = objShipModel.LocationAddress; // Location address to be add.
            ShipmentRequestObj.Address2 = objShipModel.LocationAddress2;
            ShipmentRequestObj.ShipToCity = objShipModel.City_Name;
            ShipmentRequestObj.ShipToPostalCode = objShipModel.ZipCode;
            ShipmentRequestObj.ShipToStateProvinceCode = objShipModel.StateCode;
            ShipmentRequestObj.ShipToCountryCode = "US";
            ShipmentRequestObj.ShipToAttentionName = "Store Manager";
            ShipmentRequestObj.ShipToName = objShipModel.SubAgentName;
            ShipmentRequestObj.ShipToPhone = objShipModel.LocationPhone;




            // Dynamic  as per Location Master

            ShipmentRequestObj.SoldAttentionName = "Reuben Zielinski";
            ShipmentRequestObj.SoldName = "Redux";
            ShipmentRequestObj.soldPhoneNumber = "317-819-9082";
            ShipmentRequestObj.SoldExtension = "1234";
            ShipmentRequestObj.SoldAddressLine = "525 Congressional Blvd.";
            ShipmentRequestObj.SoldCity = "Carmel";
            ShipmentRequestObj.SoldPostalCode = "46032";
            ShipmentRequestObj.SoldCountryCode = "IN";






            ShipmentRequestObj.CommodityCode = "111222AA";
            ShipmentRequestObj.OriginCountryCode = "US";
            ShipmentRequestObj.UnitNumber = "1";
            ShipmentRequestObj.UnitValue = "1";
            ShipmentRequestObj.UOMProductCode = "BOX";
            ShipmentRequestObj.ProductDescription = "BOX";
            ShipmentRequestObj.ProductWeight = "25";
            ShipmentRequestObj.UOMForWeightCode = "LBS";
            ShipmentRequestObj.UOMForWeightDescription = "LBS";



            ShipmentRequestObj.InvoiceNumber = "Redux001";
            ShipmentRequestObj.InvoiceDate = System.DateTime.Now.Year.ToString() + (System.DateTime.Now.Month.ToString().Length == 1 ? "0" + System.DateTime.Now.Month.ToString() : System.DateTime.Now.Month.ToString()) + (System.DateTime.Now.Day.ToString().Length == 1 ? "0" + System.DateTime.Now.Day.ToString() : System.DateTime.Now.Day.ToString());//"20151225";
            ShipmentRequestObj.PurchaseOrderNumber = "NA";
            ShipmentRequestObj.TermsOfShipment = "CFR";
            ShipmentRequestObj.ReasonForExport = "Sale";
            ShipmentRequestObj.Comments = "Your Comments";
            ShipmentRequestObj.DeclarationStatement = "Declaration Statement";
            ShipmentRequestObj.DiscountMonetaryValue = "0";
            ShipmentRequestObj.FreightMonetaryValue = "0";
            ShipmentRequestObj.InsuranceMonetaryValue = "1250";
            ShipmentRequestObj.OtherChargeMonetaryValue = "0";
            ShipmentRequestObj.OtherChargeDescription = "Misc";
            ShipmentRequestObj.CurrencyCode = "USD";

            return ShipmentRequestObj;
        }

        public ActionResult ShipMachines()
        {
            //string DecryptedOrderId = CommonMethods.Decode(Id);

            ShipMachineModel objShipModel = new ShipMachineModel();
            var ShipModel = manageShippingService.GetLocationToMapByOrderId();
            //var ShipModelReadyToShip=

            if (ShipModel != null)
            {
                if (ShipModel.LocationAddress.IndexOf("\r\n") >= 0)
                {
                    ShipModel.LocationAddress = ShipModel.LocationAddress.Replace("\r\n", "");
                }

                if (ShipModel.LocationAddress2 != null && ShipModel.LocationAddress2 != "" && ShipModel.LocationAddress2.IndexOf("\r\n") >= 0)
                {
                    ShipModel.LocationAddress2 = ShipModel.LocationAddress2.Replace("\r\n", "");
                }
                if (ShipModel.LocationAddress2 != null && ShipModel.LocationAddress2 == "\r\n" && ShipModel.LocationAddress2.IndexOf("\r\n") >= 0)
                {
                    ShipModel.LocationAddress2 = ShipModel.LocationAddress2.Replace("\r\n", "");
                }

            }

            if (ShipModel == null)
            {
                objShipModel.GroupName = "";
                objShipModel.ResultMsg = "NoDataFound";
                ShipModel = objShipModel;
            }
            else
            {
                var viewmodel = manageShippingService.GetUPSSettings();
                ShipModel.ServiceCode = viewmodel.ServiceCode;


            }
            
            return View(ShipModel);
        }

        public JsonResult UPSRequestSent(ShipMachineModel objShipModel)
        {
            var IsMachineActive = machineService.CheckMachineShipped(objShipModel.MachineId_Val);
            ShipmentResponseData UPSResult = new ShipmentResponseData();
            if (IsMachineActive == string.Empty && IsMachineActive == "")
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];

                UPSResult = GetUpsResponse(objShipModel);

                // string ShipmentIdentificationNumber = "1Z899V5A6793493028";
                if (UPSResult.ShipmentIdentificationNumber != string.Empty && UPSResult.ShipmentIdentificationNumber != null)
                {
                    objShipModel.ResultMsg = "OK";
                    objShipModel.ImageData = UPSResult.LabelStreamData;

                    objShipModel.ShipmentId = UPSResult.ShipmentIdentificationNumber;//"ZER1234BHD465";
                    objShipModel.LastMachineId_Val = objShipModel.MachineId_Val;
                    objShipModel.ShipmentId = UPSResult.ShipmentIdentificationNumber;

                    // Updating shiping Id into JobOrder_Machine_Mapping table
                    var result = machineService.InsertShippedMachine(objShipModel, currentUser.User_Id);


                    if (result == true)
                    {
                        objShipModel.ResultMsg = "Ok";
                    }
                    else
                    {
                        objShipModel.ResultMsg = "Error";
                    }
                }
                else
                {
                    objShipModel.ResultMsg = UPSResult.ResponseResult;

                }
            }
            else
            {
                objShipModel.ResultMsg = "Machine with this id is either shipped or not valid.";
            }
            return Json(objShipModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateLocationStatus(int Id)
        {
            LocationModel objLocationModal = new LocationModel();
            objLocationModal.GroupNameList = _GeneralService.GetGroups();
            objLocationModal.StatusList = _GeneralService.GetMasterValuesByType(ConstantEntities.ShippingStatusName);
            objLocationModal.GroupId = Id;
            return View("UpdateLocationStatus", objLocationModal);
        }

        [AjaxHandleException]
        public ActionResult LocationStatusAjax([DataSourceRequest] DataSourceRequest request, CustomerLocationModel objCustomerLocation)
        {
            int? CustomerId = objCustomerLocation.Customer_ID;
            var currentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            var data = manageShippingService.GetGroupdLocationForMapByGroupId(objCustomerLocation.GroupId);
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public ActionResult Location_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<GroupedLocationForShip> objGroupedLocationForShip, CustomerLocationModel objCustomerLocation)
        {
            IManageShippingService ShippingService = new ManageShippingService();
            List<LocationList> ObjLocationList = new List<Entities.LocationList>();
            var SessionData = (CurrentUserDetail)Session["CurrentUser"];
            int GroupId = objCustomerLocation.GroupId;
            ObjLocationList = objGroupedLocationForShip.Select(m => new LocationList { LocationId = m.Location_ID, NoofMachines = m.NoofMachines, ShippingStatusId = m.ShippingStatusId }).ToList();
            var data = ShippingService.UpdateNoOfMachinesByGroupLocation(ObjLocationList, GroupId, SessionData.User_Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}