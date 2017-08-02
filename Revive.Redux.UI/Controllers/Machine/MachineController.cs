using Kendo.Mvc.UI;
using Revive.Redux.Services;
using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Revive.Redux.Entities;
using Revive.Redux.Common;
using Revive.Redux.Controllers.Common;
using Newtonsoft.Json;
using System.Collections;
using System.Configuration;
using System.Web.Hosting;
using System.IO;
using System.Linq;
namespace Revive.Redux.UI
{
    [Authorize]
    [ReviveAuth]
    public class MachineController : Controller
    {
        private IOrderManagementService _IOrderManagementService = null;
        private IMachineService _IMachineService = null;

        public MachineController()
        {
            _IOrderManagementService = new OrderManagementService();
            _IMachineService = new MachineService();

        }

        #region Machine More Details
        /// <summary>
        /// View detail of order and machine status
        /// </summary>
        /// <param name="JobOrderId"></param>
        /// <returns></returns>
        public ViewResult MoreDetails(String Id)
        {
            MachineModel machineModel = new MachineModel();
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    string decryptedId = CommonMethods.Decode(Id);
                    OrderModel order = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));
                    if (order != null)
                    {
                        ViewBag.OrderNumber = order.JobOrderHeaderId;
                        ViewBag.CustomerName = order.CustomerName;
                        ViewBag.NoOfMachines = order.NoOfMachines;
                        ViewBag.ExpectedDate = Convert.ToDateTime(order.ExpectedDate).ToString("MM/dd/yyyy");
                        ViewBag.MachineSpecs = order.MachineSpecs;
                        machineModel.JobOrderHeaderID = order.JobOrderHeaderId;
                        machineModel.JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(order.JobOrderHeaderId));
                    }
                    else
                        return View();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return View("MoreDetails", machineModel);
        }
        /// <summary>
        /// Get More details using jobOrderId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="jobOrderId"></param>
        /// <returns></returns>
        public ActionResult GetMoreDetailsAjax([DataSourceRequest] DataSourceRequest request, int? jobOrderId)
        {
            IEnumerable<MachineModel> items = _IMachineService.GetMappedMachinesByOrderId((int)jobOrderId);
            DataSourceResult result = items.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Shipping
        /// <summary>
        /// Shipping 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ViewResult Shipping(String Id)
        {
            if (Request.UrlReferrer != null)
                ViewBag.ReturnUrl = Request.UrlReferrer;
            else
                ViewBag.ReturnUrl = "#";

            if (!string.IsNullOrEmpty(Id))
            {
                string decryptedId = CommonMethods.Decode(Id);
                OrderModel order = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));
                if (order != null)
                {
                    ViewBag.OrderNumber = order.JobOrderHeaderId;
                    ViewBag.CustomerName = order.CustomerName;
                    ViewBag.NoOfMachines = order.NoOfMachines;
                    ViewBag.ExpectedDate = Convert.ToDateTime(order.ExpectedDate).ToString("MM/dd/yyyy");
                    // TODO: Implement machine specs functionality.
                    ViewBag.MachineSpecs = order.MachineSpecs;
                    MachineModel model = new MachineModel();
                    model.JobOrderHeaderID = order.JobOrderHeaderId;
                    ViewBag.IsModalValid = true;
                    return View("Shipping", model);
                }
                else
                    ViewBag.IsModalValid = true;
                return View();
            }
            else
                ViewBag.IsModalValid = true;
            return View();
        }
        /// <summary>
        /// Get Shipping Info using jobOrderId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="jobOrderId"></param>
        /// <returns></returns>
        public ActionResult GetShippingInfoAjax([DataSourceRequest] DataSourceRequest request, int jobOrderId)
        {
            IEnumerable<MachineModel> items = _IMachineService.GetMachinesByOrderIdForShipping(jobOrderId);
            DataSourceResult result = items.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Shipping Info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateShippingInfo([DataSourceRequest] DataSourceRequest request, MachineModel model)
        {
            try
            {
                _IMachineService.UpdateShippingId((int)model.MachineMappingId, model.ShippingId, (int)model.JobOrderHeaderID);
            }
            catch (Exception ex)
            {
                // TODO: Pending.
                throw;
            }
            return Json(new[] { model }.ToDataSourceResult(request));
        }
        #endregion

        #region Map Machines
        /// <summary>
        /// Map Machines using id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult MapMachines(String Id)
        {
            if (Request.UrlReferrer != null)
                ViewBag.ReturnUrl = Request.UrlReferrer;
            else
                ViewBag.ReturnUrl = "#";
            OrderModel model = new OrderModel();
            if (!string.IsNullOrEmpty(Id))
            {
                string decryptedId = CommonMethods.Decode(Id);
                OrderModel item = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));

                if (item != null)
                {
                    model.JobOrderHeaderId = item.JobOrderHeaderId;
                    model.CustomerName = item.CustomerName;
                    model.ExpectedOrderDate = Convert.ToDateTime(item.ExpectedDate).ToString("MM/dd/yyyy");
                    model.MachineSpecs = item.MachineSpecs;
                    model.NoOfMachines = item.NoOfMachines;
                    //model.LocationsList = GetOrderLocations(Id);
                    model.DownloadSWUrl = string.IsNullOrEmpty(item.DownloadSWUrl)
                            ? "#"
                            :OrderController.GetSiteUrl(Request.Url) + item.DownloadSWUrl;
                    model.MachineIds = _IMachineService.GetUnassignedMachineID();
                    model.softwareVersionId = item.softwareVersionId;
                    model.MachineSpecsId = item.MachineSpecsId;
                    model.NoOfMachinesMapped = item.NoOfMachinesMapped;
                }
            }
            return View(model);
        }

        /// <summary>
        /// Get Order Locations using Id
        /// </summary>
        /// <param name="jobOrderId"></param>
        /// <returns></returns>
        public ActionResult GetOrderLocations([DataSourceRequest] DataSourceRequest request, int jobOrderId)
        {
            List<DtoList> locs = new List<DtoList>();
            IEnumerable<OrderLocationsModel> locModel = _IOrderManagementService.GetOrderLocationsByOrderId(jobOrderId);
            foreach (OrderLocationsModel current in locModel)
            {
                if (current.NumberOfMachines != null && current.NumberOfMachines > 0 && current.JobOrderLocationId != null)
                {
                    if (current.NumberOfMachinesMapped < current.NumberOfMachines)
                    {
                        DtoList item = new DtoList();
                        item.Id = (int)current.JobOrderLocationId;
                        item.Text = current.LocationName;
                        item.otherIntVal = current.NumberOfMachines;
                        locs.Add(item);
                    }

                }
            }
            var loc = locs;
            return Json(loc, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrderLocationsEdit([DataSourceRequest] DataSourceRequest request, int jobOrderId)
        {
            List<DtoList> locs = new List<DtoList>();
            IEnumerable<OrderLocationsModel> locModel = _IOrderManagementService.GetOrderLocationsByOrderId(jobOrderId);
            foreach (OrderLocationsModel current in locModel)
            {
                if (current.NumberOfMachines != null && current.NumberOfMachines > 0 && current.JobOrderLocationId != null)
                {
                   
                        DtoList item = new DtoList();
                        item.Id = (int)current.JobOrderLocationId;
                        item.Text = current.LocationName;
                        item.otherIntVal = current.NumberOfMachines;
                        locs.Add(item);
                    

                }
            }
            var loc = locs;
            return Json(loc, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get already mapped machines for particular order ID.
        /// </summary>
        /// <param name="id">Order header ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMappedMachinesByOrderId(int id)
        {
            var loc = _IMachineService.GetMappedMachinesByOrderId(id);
            return Json(loc, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get list of all un-assigned machine IDs which has assigned status=false
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUnassignedMachineID()
        {


            IEnumerable<DtoList> dto = _IMachineService.GetUnassignedMachineID();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            string manufacturer_ref_id = currentUser.Manufacturer_Ref_Id;

            var _dtoLst = (from dt in dto
                           select new DtoList
                           {
                               Id = dt.Id,
                               Text = dt.Text,
                               otherStrVal = dt.Text.Substring(3, 1) // finding 4th letter of Machine Id  from first to match manufacturer Id
                           });

            if (manufacturer_ref_id != null && manufacturer_ref_id.Length > 0)
            {
                dto = _dtoLst.Where(x => x.otherStrVal == manufacturer_ref_id).ToList();
            }

            return Json(dto, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetUnassignedMachineIDForUpdate(int? machineMapId)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            string manufacturer_ref_id = currentUser.Manufacturer_Ref_Id;

            if (machineMapId == null)
            {
                machineMapId = 0;
            }
            IEnumerable<DtoList> dto = _IMachineService.GetUnassignedMachineIDForUpdate((int)machineMapId);

            var _dtoLst = (from dt in dto
                           select new DtoList
                           {
                               Id = dt.Id,
                               Text = dt.Text,
                               otherStrVal = dt.Text.Substring(3, 1) // finding 4th letter of Machine Id  from first to match manufacturer Id
                           });

            if (manufacturer_ref_id != null && manufacturer_ref_id.Length > 0)
            {
                dto = _dtoLst.Where(x => x.otherStrVal == manufacturer_ref_id).ToList();
            }
            return Json(dto, JsonRequestBehavior.AllowGet);

        }



        /// <summary>
        /// Insert Machine ID and Location ID
        /// </summary>
        /// <param name="jobOrdHeaderId">Job order number</param>
        /// <param name="jobOrdLocId">Location ID</param>
        /// <param name="machineId">Machine ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertMachineWithLocation(int jobOrdHeaderId, int jobOrdLocId, int machineId, int softwareVersion_id)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            bool isValidate = true;
            if (jobOrdLocId > 0)
            {
                isValidate = _IMachineService.isValidMappingByLocation(jobOrdLocId, jobOrdHeaderId, 1, 0);
            }

            if (isValidate)
            {
                _IMachineService.InsertMappedMachine(jobOrdHeaderId, jobOrdLocId, machineId, currentUser.User_Id, softwareVersion_id);
                var loc = _IMachineService.GetMappedMachinesByOrderId(jobOrdHeaderId);
                return Json(loc, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var loc = _IMachineService.GetMappedMachinesByOrderId(0);
                return Json(loc, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Update Machine ID and Location ID
        /// </summary>
        /// <param name="machineMappingId">Machine mapping ID (currently edited row PK)</param>
        /// <param name="jobOrdLocId">Newly selected Location ID</param>
        /// <param name="newMachineId">Newly selected Machine ID</param>
        /// <param name="oldMachineId">Old machine ID which is already set.</param>
        /// <param name="jobOrdHeaderId">Job order number</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateMachineWithLocation(int machineMappingId, int jobOrdLocId, int newMachineId, int oldMachineId, int jobOrdHeaderId, int softwareVersion_id)
        {

            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            bool isValidate = true;

            if (jobOrdLocId > 0)
            {
                isValidate = _IMachineService.isValidMappingByLocation(jobOrdLocId, jobOrdHeaderId, 0, machineMappingId);
            }


            if (isValidate)
            {
                _IMachineService.UpdateMappedMachine(machineMappingId, jobOrdLocId, newMachineId, oldMachineId, currentUser.User_Id, softwareVersion_id);
                var loc = _IMachineService.GetMappedMachinesByOrderId(jobOrdHeaderId);
                return Json(loc, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var loc = _IMachineService.GetMappedMachinesByOrderId(0);
                return Json(loc, JsonRequestBehavior.AllowGet);
            }




        }
        #endregion

        #region Upload Shipping Details
        public ActionResult AddShippingDetailsByXLSFile(MachineModel objMachineModel, IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null && files.Count() > 0)
            {
                string sDirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\UploadMachine\ShippingDetails";
                DirectoryInfo dir = new DirectoryInfo(sDirPath);
                var sPath = string.Format(@"{0}", sDirPath);
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                if (Directory.Exists(sPath))
                {
                    foreach (FileInfo item in dir.GetFiles())
                    {
                        item.Delete();
                    }
                }

                string dirPath1 = @"\UploadMachine\ShippingDetails";

                storeDocument(files, dirPath1);

                foreach (var item in files)
                {
                    objMachineModel.FileName = sPath + @"\\" + item.FileName;
                }

                MachineModel objMachineResult = new MachineModel();
                bool bResult = _IMachineService.ShippingDetailsFileUpload(objMachineModel, out objMachineResult);
                Session["MachineResult"] = objMachineResult;

                string EncryptedId = CommonMethods.Encode(objMachineModel.strJobOrderHeaderID);
                ViewBag.IsModalValid = false;
                string decryptedId = CommonMethods.Decode(EncryptedId);
                OrderModel order = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));
                if (order != null)
                {
                    ViewBag.OrderNumber = order.JobOrderHeaderId;
                    ViewBag.CustomerName = order.CustomerName;
                    ViewBag.NoOfMachines = order.NoOfMachines;
                    ViewBag.ExpectedDate = Convert.ToDateTime(order.ExpectedDate).ToString("MM/dd/yyyy");
                    // TODO: Implement machine specs functionality.
                    ViewBag.MachineSpecs = order.MachineSpecs;
                    MachineModel model = new MachineModel();
                    model.JobOrderHeaderID = order.JobOrderHeaderId;
                    ViewBag.IsModalValid = false;
                    return View("Shipping", model);
                }
                else
                    ViewBag.IsModalValid = false;
                return View();
                //return RedirectToAction("Shipping", "Machine", new { id = "MzIz" });
                //, new { id = EncryptedId }
            }
            return RedirectToAction("Shipping", "Machine");
        }

        public ActionResult ShippingDetailResultAjax([DataSourceRequest] DataSourceRequest request)
        {
            var objShippingDetailResult = new MachineModel();
            if (Session["MachineResult"] != null)
            {
                objShippingDetailResult = (MachineModel)Session["MachineResult"];
            }
            DataSourceResult result = objShippingDetailResult.MachineResult.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

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
        #endregion
    }
}