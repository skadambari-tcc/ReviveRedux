using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.UI;
using Revive.Redux.Services;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Revive.Redux.Entities;
using Revive.Redux.Controllers.Common;
using Revive.Redux.Common;
using System.Web.Hosting;
using System.Collections;
using System.Web;
using System.IO;
using Revive.Redux.Commn;


namespace Revive.Redux.UI.Controllers.ManageMachineSpecs
{
    [Authorize]
    [ReviveAuth]
    public class ManageMachineSpecsController : Controller
    {

        #region Private Members

        IManageMachineSpecsService manageMachineService = null;
        IGeneralService generalServices = null;
        IMachineService machineServ = null;
        #endregion

        // GET: ManageMachineSpecs

        public ActionResult ManageMachineSpecs()
        {


            return View(new MachineSpecsModel());
        }


        //Constructor
        public ManageMachineSpecsController()
        {
            manageMachineService = new ManageMachineSpecsService();
            generalServices = new GeneralService();
            ViewBag.culst = generalServices.GetCustomerService();
            ViewBag.GetFirmware = generalServices.GetFirmware();
            ViewBag.GetCustomerList = generalServices.GetCustomerService();
            ViewBag.Locations = generalServices.GetEmptyDDL();
            ViewBag.Parameters = generalServices.GetParameters();
            ViewBag.templatelst = generalServices.GetTemplateServices();

            ViewBag.SubsidiaryLst = generalServices.GetEmptyDDL();
            ViewBag.AgentLst = generalServices.GetEmptyDDL();
            machineServ = new MachineService();
        }

        private List<DtoList> bindCustomer()
        {


            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var objcustomerlst = new List<DtoList>();

            objcustomerlst = generalServices.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();

            if (objcustomerlst.Count == 0)
            {
                objcustomerlst = generalServices.GetEmptyDDL().ToList();
            }



            return objcustomerlst;

        }

        /// <summary>
        /// function to fetch all machine specs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AjaxHandleException]
        public ActionResult ManageMachineAjaxSpecs([DataSourceRequest] DataSourceRequest request)
        {
            var machines = manageMachineService.GetMachineSpecs();

            DataSourceResult result = machines.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Add Or Edit Machine Specs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddEditMachineSpec(MachineSpecsModel model)
        {
            return View(model);
        }

        /// <summary>
        /// function to add new software specs aur edit existing specs
        /// </summary>
        /// <param name="softmodel"></param>
        /// <returns></returns>
        public ActionResult AddEditSoftware()
        {

            //return View(softmodel);
            ViewBag.PageMode = 1; // For Inserting
            return View();
        }

        /// <summary>
        /// Edit Machines
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditMachineSpecs(int id)
        {
            ViewBag.Title = "Edit Machine";
            var model = new MachineSpecsModel();
            if (id != 0)
            {
                ViewBag.PageMode = 2;
                Session["MachineId"] = id;
                model = manageMachineService.GetMachineDetailsById(id);
                model.Created_Date = DateTime.Now;
                TempData["Edit"] = true;

            }
            return View("AddEditMachineSpec", model);
        }

        public JsonResult ViewMachineSpecs(string id)
        {
            var model = new MachineSpecsModel();
            if (!String.IsNullOrEmpty(id))
            {
                model = manageMachineService.GetMachineDetailsById(Convert.ToInt32(id));
            }
            return Json(new { Generation = model.Generation, software_version = model.software_version, Pump = model.Pump, PCB_Version = model.PCB_Version, INjection_Heater = model.INjection_Heater, PowerSupply_1 = model.PowerSupply_1, Platen_heater = model.Platen_heater, PowerSupply_2 = model.PowerSupply_2, Valve_2 = model.Valve_2, Valve_1 = model.Valve_1, Debug_Parameter_default_value = model.Debug_Parameter_default_value, Valve_3 = model.Valve_3, Customfield2 = model.Customfield2, Customfield1 = model.Customfield1, Customfield4 = model.Customfield4, Customfield3 = model.Customfield3, Customfield5 = model.Customfield5 });
        }


        /// <summary>
        /// Edit Software specifications
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult EditSoftDetail(int id)
        {
            ViewBag.Title = "Edit Machine";
            var model = new ManageSoftwareVersionModel();
            if (id != 0)
            {
                ViewBag.PageMode = 2;
                Session["MasterData_Type_ID"] = id;
                model = manageMachineService.GetSoftwareDetailsById(id);
                model.Created_Date = DateTime.Now;
                ViewBag.isUpdateMode = true;
                TempData["EditSoft"] = true;

            }
            return View("AddEditSoftware", model);
        }


        /// <summary>
        /// function called on addedit for registration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RegisterNewMachine(MachineSpecsModel model)
        {
            if (TempData["Edit"] != null)
            {

                model.MachineSpec_ID = Convert.ToInt16(Session["MachineId"]);

            }

            var result = false;
            var isNew = model.MachineSpec_ID != 0 ? false : true;


            if (ModelState.IsValid)
            {

                result = manageMachineService.AddEditMachineSpec(model);


            }

            if (isNew == true)
            {
                TempData["isCreatedSuccess"] = true;
            }
            else
            {
                TempData["isUpdatedSuccess"] = true;
            }

            return RedirectToAction("ManageMachineSpecs");
        }


        public ManageSoftwareVersionModel storeSoftwareVersion(IEnumerable<HttpPostedFileBase> files)
        {
            var questionFileName = string.Empty;
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + "ReduxFirmware";

            ManageSoftwareVersionModel objManageSoftwareVersionModel = new ManageSoftwareVersionModel();


            DirectoryInfo dir = new DirectoryInfo(dirPath);
            var sPath = string.Format(@"{0}", dirPath);
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }


            string Filename = "";
            if (files != null)
            {
                foreach (var item in files)
                {
                    //var path = string.Format(@"{0}\{1}", dirPath, Path.Combine(Server.MapPath(item.FileName)));
                    Filename = Path.GetFileName(item.FileName);
                    var path = Path.Combine(dirPath, Filename);
                    if (System.IO.File.Exists(path))
                    {
                        var filetodelete = item.FileName;
                        //Delete File If Already Exist
                        System.IO.File.Delete(path);
                    }
                    // item.SaveAs(path);


                    item.SaveAs(path);
                    string checkSumKey = Cryptography.GetMD5CheckSumKey(path);
                    objManageSoftwareVersionModel.CheckSumKey = checkSumKey;



                }
            }
            Filename = "ReduxFirmware/" + Filename;
            objManageSoftwareVersionModel.Custom_Field1 = Filename;
            return objManageSoftwareVersionModel;

        }

        /// <summary>
        /// function called once the form begins in addedit software
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RegisterSoftware(ManageSoftwareVersionModel model, IEnumerable<HttpPostedFileBase> files)
        {
            string packagePath = "";
            string checkSumKey = "";

            if (files != null)
            {
                if (files.Count() > 0)
                {
                    var objStoreFiles = storeSoftwareVersion(files);
                    packagePath = objStoreFiles.Custom_Field1;
                    checkSumKey = objStoreFiles.CheckSumKey;
                }
            }


            if (TempData["EditSoft"] != null)
            {
                model.MasterData_Type_ID = Convert.ToInt16(Session["MasterData_Type_ID"]);
            }
            var result = false;
            var isNew = model.MasterData_Type_ID != 0 ? false : true;
            model.Custom_Field1 = packagePath;


            if (ModelState.IsValid)
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                ManageSoftwareVersionModel modelToSave = new ManageSoftwareVersionModel();
                modelToSave.MasterData_Value = model.MasterData_Value.Trim();
                modelToSave.Custom_Field1 = model.Custom_Field1.Trim();
                modelToSave.CustomField2 = model.CustomField2.Trim();
                modelToSave.CustomField3 = !string.IsNullOrEmpty(model.CustomField3) ? model.CustomField3.Trim() : string.Empty;
                modelToSave.CheckSumKey = checkSumKey;
                modelToSave.MasterData_Type_ID = model.MasterData_Type_ID;
                if (isNew == true)
                {
                    if (currentUser != null && currentUser.User_Id != null)
                        modelToSave.Created_by = currentUser.User_Id;
                    modelToSave.Created_Date = DateTime.Now;
                }
                else
                {
                    if (currentUser != null && currentUser.User_Id != null)
                        modelToSave.Modified_by = currentUser.User_Id;
                    modelToSave.Modified_Date = DateTime.Now;
                }

                result = manageMachineService.AddEditSoftware(modelToSave);
                if (result)
                {
                    if (isNew == true)
                    {
                        TempData["isCreatedSuccess"] = true;
                    }
                    else
                    {
                        TempData["isUpdatedSuccess"] = true;
                    }
                }
            }
            return View("ManageSoftwareVersion");
        }

        public JsonResult UpdateArchiveStatus(string ID)
        {
            var result = "Not OK";
            if (!string.IsNullOrEmpty(ID))
            {
                int intID = Convert.ToInt32(ID);
                manageMachineService.UpdateArchiveStatus(intID);
                result = "OK";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateLatestSoftware()
        {
            ViewBag.culst = bindCustomer();
            ViewBag.SubsidiaryLst = generalServices.GetEmptyDDL();
            ViewBag.AgentLst = generalServices.GetEmptyDDL();
            return View();
        }

        /// <summary>
        /// Fetch existing software details on software update page 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cusdata"></param>
        /// <param name="locdata"></param>
        /// <returns></returns>
        public ActionResult SoftUpdtAjax([DataSourceRequest] DataSourceRequest request, string cusdata, string locdata, int subsidiarydata, int subagentdata)
        {
            IEnumerable<MachineModel> softPara = null;

            int _SubsidiaryId = 0;
            int _SubAgnetId = 0;
            if (cusdata == "" && locdata == "")
            {
                cusdata = null;
                locdata = null;

            }
            else if (cusdata != "" && locdata == "")
            {
                locdata = null;
            }
            int _LocationId = 0;
            if (locdata != "0" || locdata == "")
            {
                _LocationId = Convert.ToInt32(locdata);
            }
            int _customerId = 0;
            if (cusdata != "")
            {
                _customerId = Convert.ToInt32(cusdata);
            }

            if (subsidiarydata > 0)
            {
                _SubsidiaryId = subsidiarydata;
            }

            if (subagentdata > 0)
            {
                _SubAgnetId = subagentdata;
            }


            softPara = manageMachineService.GetMappedMachinesByCustomer(_customerId, _LocationId, _SubsidiaryId, _SubAgnetId);
            DataSourceResult result = softPara.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdSoftware(List<MachineModel> data, int softversn)
        {

            //        var errors = ModelState
            //.Where(x => x.Value.Errors.Count > 0)
            //.Select(x => new { x.Key, x.Value.Errors })
            //.ToArray();

            //        ModelState.Remove("ShippingId"); //not required in this case
            manageMachineService.UpdSoftware(data, softversn);

            return Json(JsonRequestBehavior.AllowGet);
        }


        //fetch customer list
        [AjaxHandleException]
        public JsonResult GetCustomerList()
        {

            var lst = generalServices.GetCustomerService();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        //fetch tmplates
        [AjaxHandleException]
        public JsonResult GetTemplateList()
        {

            var lst = generalServices.GetTemplateServices();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public JsonResult GetLocationList(int id, bool bActive = false)
        {
            var customers = generalServices.GetCustomerLocationService(id, bActive);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Config Debug Parameter
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageDebugPara()
        {
            ViewBag.culst = bindCustomer();
            ViewBag.SubsidiaryLst = generalServices.GetEmptyDDL();
            ViewBag.AgentLst = generalServices.GetEmptyDDL();
            return View();
        }

        /// <summary>
        /// retrieve grid for config debug parameter and templates
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filtervalu"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public ActionResult ManageDebugAjaxPara([DataSourceRequest] DataSourceRequest request, string CustomerId, string LocationId, int SubsidiaryId, int SubAgentId)
        {
            IEnumerable<ManageDebugParaModel> debugparas = null;
            int _SubsidiaryId = 0;
            int _SubAgnetId = 0;
            if (CustomerId == "" && LocationId == "")
            {
                CustomerId = null;
                LocationId = null;

            }
            else if (CustomerId != "" && LocationId == "")
            {
                LocationId = null;
            }
            int _LocationId = 0;
            if (LocationId != "0" || LocationId == "")
            {
                _LocationId = Convert.ToInt32(LocationId);
            }
            int _customerId = 0;
            if (CustomerId != "")
            {
                _customerId = Convert.ToInt32(CustomerId);
            }

            if (SubsidiaryId > 0)
            {
                _SubsidiaryId = SubsidiaryId;
            }

            if (SubAgentId > 0)
            {
                _SubAgnetId = SubAgentId;
            }


            debugparas = manageMachineService.GetManageDebugPara(_customerId, _LocationId, _SubsidiaryId, _SubAgnetId);
            DataSourceResult result = debugparas.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);



            //// int filter = 0;
            //IEnumerable<ManageDebugParaModel> debugparas = null;



            //if (filtervalu == "" && loc == "")
            //{
            //    filtervalu = null;
            //    loc = null;

            //}
            //else if (filtervalu != "" && loc == "")
            //{
            //    loc = null;
            //}

            //debugparas = manageMachineService.GetManageDebugPara(Convert.ToInt32(filtervalu), Convert.ToInt32(loc));
            //DataSourceResult result = debugparas.ToDataSourceResult(request);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// service call to update parameter
        /// </summary>
        /// <param name="da"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdTemplate(List<ManageDebugParaModel> da, int templ)
        {

            if (da != null && ModelState.IsValid)
            {


                manageMachineService.UpdtTempl(da, templ);
            }

            return Json(JsonRequestBehavior.AllowGet);
        }




        public ActionResult ManageSoftwareVersion()
        {

            return View();
        }

        public ActionResult ManageSoftwareAjaxVersion([DataSourceRequest] DataSourceRequest request)
        {
            var softwareDetails = manageMachineService.GetSoftwareDetail();

            DataSourceResult result = softwareDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// get list of all firmware versions
        /// </summary>
        /// <returns></returns>
        [AjaxHandleException]
        public JsonResult GetFirmware()
        {

            var lst = generalServices.GetFirmware();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KillRecovr()
        {
            return View();
        }


        #region Kill Machine

        public ActionResult Details(KillMachineModel objKillMachineModel)
        {
            objKillMachineModel.PageMode = 0;
            objKillMachineModel.Message = "Do you want to kill the machine?";
            objKillMachineModel.MachineLocationDetails = GetMachineStatus(objKillMachineModel);
            objKillMachineModel.CustomerLocationDetails = generalServices.GetEmptyDDL();
            //generalServices.GetCustomerLocationService(0, true);
            objKillMachineModel.CustomerNameList = generalServices.GetCustomerService(true);
            objKillMachineModel.SubsidiaryNameList = generalServices.GetEmptyDDL();
            objKillMachineModel.AgentNameList = generalServices.GetEmptyDDL();
            objKillMachineModel.PageHeader = "Kill Machine";

            return View("KillMachine", objKillMachineModel);
        }



        public ActionResult ViewRecover(KillMachineModel objKillMachineModel)
        {
            objKillMachineModel.PageMode = 1;
            objKillMachineModel.Message = "Do you want to recover the machine?";
            objKillMachineModel.MachineLocationDetails = GetMachineStatus(objKillMachineModel);
            objKillMachineModel.CustomerLocationDetails = generalServices.GetEmptyDDL();//generalServices.GetCustomerLocationService(0, true);
            objKillMachineModel.CustomerNameList = generalServices.GetCustomerService(true);
            objKillMachineModel.SubsidiaryNameList = generalServices.GetEmptyDDL();
            objKillMachineModel.AgentNameList = generalServices.GetEmptyDDL();

            objKillMachineModel.PageHeader = "Recover Machine";

            return View("KillMachine", objKillMachineModel);
        }

        public ActionResult MachineAjax([DataSourceRequest] DataSourceRequest request, KillMachineModel objMachineModel)
        {
            var objKillMachineModel = GetMachineStatus(objMachineModel);

            DataSourceResult result = objKillMachineModel.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KillMachine(MachineModel objMachineModel)
        {
            var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            if (objCurrentUserDetail != null)
            {
                objMachineModel.ModifiedBy = objCurrentUserDetail.User_Id;
                objMachineModel.ModifiedDate = Common.CommonMethods.GetCurrentDate();
                objMachineModel.CreatedBy = objCurrentUserDetail.User_Id;
                objMachineModel.CreatedDate = Common.CommonMethods.GetCurrentDate();

            }

            objMachineModel = manageMachineService.KillMachine(objMachineModel);
            objMachineModel.Message = "Error in status update.";
            if (objMachineModel.Result)
            {
                objMachineModel.ActivityType = manageMachineService.GetActivityName(objMachineModel.ActivityTypeId);
                if (!string.IsNullOrEmpty(objMachineModel.ActivityType))
                {
                    SendKillNotification(objMachineModel);
                }
                objMachineModel.Message = ReviveMessages.Update;
            }

            return Json(objMachineModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Upon confirming the kill command the system will also send a notification to the account manager stating the customer, location and machine that was disabled.
        /// </summary>
        /// <param name="MachineModel"></param>
        private void SendKillNotification(MachineModel objMachineModel)
        {

            string sEmailTo = Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);   // CC Account Managers            

            if (!string.IsNullOrEmpty(sEmailTo))
            {
                EmailHelper objEmailHelper = new EmailHelper();
                var objKeyWords = new Hashtable();
                objKeyWords.Add("MACHINE", objMachineModel.MachineId_Val);
                objKeyWords.Add("LOCATION", objMachineModel.Location);
                objKeyWords.Add("STATUS", objMachineModel.ActivityType);
                objKeyWords.Add("CUSTOMER", objMachineModel.CustomerName);
                var objParser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/KillMachine.html"), objKeyWords);
                var sEmailbody = objParser.Parse();

                objEmailHelper.Send(sEmailTo, string.Empty, string.Empty, "Redux - Machine Status Notification", sEmailbody);
            }
        }

        private List<MachineModel> GetMachineStatus(KillMachineModel objKillMachineModel)
        {
            List<MachineModel> lstMachine = manageMachineService.GetKillMachineByLocationAndCustomer(objKillMachineModel.LocationId, objKillMachineModel.CustomerId, objKillMachineModel.SubsidiaryId, objKillMachineModel.SubAgentId);
            List<MachineModel> lstMachineResult = new List<MachineModel>();

            foreach (var lst in lstMachine)
            {
                if (objKillMachineModel.PageMode == 0 && string.Compare(lst.ActivityType, "Kill", true) == 0)
                {
                    lstMachineResult.Add(lst);
                }
                else if (objKillMachineModel.PageMode == 1 && string.Compare(lst.ActivityType, "Recover", true) == 0)
                {
                    lstMachineResult.Add(lst);
                }
            }
            return lstMachineResult;
        }

        #endregion



        public ActionResult DebugParaTempl()
        {
            ViewBag.culst = bindCustomer();
            return View();

        }





        [AjaxHandleException]
        public JsonResult GetParaMeters()
        {
            var parameters = generalServices.GetParameters();
            return Json(parameters, JsonRequestBehavior.AllowGet);
        }



        [AjaxHandleException]
        public JsonResult GetParaMetersById()
        {
            int id = Convert.ToInt16(Session["Template_ID"]);
            var parameters = manageMachineService.GetTemplatesParaById(id);
            return Json(parameters, JsonRequestBehavior.AllowGet);
        }



        [AjaxHandleException]
        public JsonResult GetExceptPara()
        {
            int id = Convert.ToInt16(Session["Template_ID"]);
            var parawithoutid = generalServices.GetParameters(id);
            //var parameterswithid = manageMachineService.GetTemplatesParaById(id);
            var result = parawithoutid.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }








        public JsonResult GetDebugParaTemplateAjax([DataSourceRequest] DataSourceRequest request)
        {
            var parameters = manageMachineService.GetTemplatesPara();
            DataSourceResult result = parameters.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEditTemplate()
        {
            return View();
        }

        public ActionResult EditPtmpl(string id)
        {
            ViewBag.Title = "Edit Template";
            int nTemplateId = 0;
            string sTemplateName = string.Empty;

            nTemplateId = Convert.ToInt16(id.Split('@').First());
            sTemplateName = id.Split('@').Last();

            var model = new Debug_Templates_DefinitionModel();
            model.PageMode = 1;
            if (nTemplateId != 0)
            {

                Session["Template_ID"] = nTemplateId;

                model.Template_Name = sTemplateName;
                model.Template_ID = nTemplateId;

                //model.Created_Date = DateTime.Now;
                ViewBag.isUpdateMode = true;
                // ViewBag.Data = model;
                TempData["isUpdateMode"] = true;

            }
            //return Json(model, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("AddEditTemplate");
            return View("EditPtmpl", model);
        }

        [HttpPost]
        public ActionResult AddTemplate(List<Debug_Templates_DefinitionModel> debugTemplatesDefinitions)
        {
            Boolean bResult = false;

            if (debugTemplatesDefinitions != null && ModelState.IsValid)
            {
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

                for (int count = 0; count < debugTemplatesDefinitions.Count; count++)
                {
                    debugTemplatesDefinitions[count].ModifiedBy = objCurrentUserDetail.User_Id;
                    debugTemplatesDefinitions[count].ModifiedDate = Common.CommonMethods.GetCurrentDate();
                    if (debugTemplatesDefinitions[count].PageMode == 0)
                    {
                        debugTemplatesDefinitions[count].CreatedBy = objCurrentUserDetail.User_Id;
                        debugTemplatesDefinitions[count].CreatedDate = Common.CommonMethods.GetCurrentDate();
                    }
                }

                bResult = manageMachineService.AddTemplate(debugTemplatesDefinitions);
                if (bResult == true && debugTemplatesDefinitions[0].Template_ID > 0)
                {
                    TempData["isUpdateSuccess"] = true; // Update Record

                }
                else
                {
                    TempData["isCreatedSuccess"] = true; // New Record
                }

            }



            return Json(bResult, JsonRequestBehavior.AllowGet);

        }



        #region Replace Machine
        public ViewResult ReplaceMachine()
        {
            IEnumerable<DtoList> list = new List<DtoList>();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            var replaceMachModel = new ReplaceMachineModel
            {
                CustomerList = generalServices.GetCustomerService(),
                SubsidiaryNameList = list,
                AgentNameList = list,
                LocationList = list,
                ShippedMachines = list,
                UnassignedMachines = machineServ.GetUnassignedMachineID(),
                Machine_LocationList = generalServices.GetLocations()
            };
            return View(replaceMachModel);
        }
        [HttpPost]
        public JsonResult GetCustomerLocations(int customerId)
        {
            IOrderManagementService _IOrderManagementService = new OrderManagementService();
            var loc = generalServices.GetCustomerLocationService(customerId);
            return Json(loc, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetShippedMachines(int customerId, int locationId)
        {
            IOrderManagementService _IOrderManagementService = new OrderManagementService();
            var loc = manageMachineService.GetShippedMachIds(customerId, locationId);
            return Json(loc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateMachine(ReplaceMachineModel model)
        {
            if (model.ShippedMachId != null && model.UnassignedMachId != null)
            {

                if (model.Machine_LocationId == null)
                {
                    model.Machine_LocationId = 0;
                }
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                bool isSuccess = manageMachineService.ReplaceMachine((int)model.ShippedMachId, (int)model.UnassignedMachId, currentUser.User_Id, (int)model.Machine_LocationId);
                if (isSuccess)
                {
                    TempData["MachineReplaced"] = true;
                }
            }
            return RedirectToAction("ReplaceMachine");
        }
        #endregion


        [HttpPost]
        public JsonResult GetSubsidiaryByCustomer(int CustomerId)
        {
            try
            {
                var ListSubsidary = generalServices.GetSubsidiaryByCustomerId(CustomerId);
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
                var ListAgent = generalServices.GetAgentsBySubsidiaryId(SubsidiaryId);
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
                var LisLocation = generalServices.GetCustomerLocation(CustomerId, SubsidiaryId, IsActive);
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
                var LisLocation = generalServices.GetCustomerLocation(CustomerId, SubsidiaryId, AgentId);
                return Json(LisLocation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult GetUnAssignedMachines([DataSourceRequest] DataSourceRequest request)
        {
            var result = machineServ.GetUnAssignedMachines().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Delete machine view
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteMachines()
        {
            return View();
        }
        /// <summary>
        /// CR 12
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMachineIDFromDB(List<MachineModel> data)
        {


            var DeletedMachineId = data;


            if (DeletedMachineId != null)
            {

                manageMachineService.DeleteMachine(DeletedMachineId);
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckSoftwareVersion(String MasterData_Value, int? MasterData_Type_ID)
        {
            var flag = false;
            if (MasterData_Type_ID == null)
            {
                MasterData_Type_ID = 0;
            }
            int _MasterData_Type_ID = (int)MasterData_Type_ID;
            try
            {
                flag = manageMachineService.CheckSoftwareVersion(MasterData_Value, _MasterData_Type_ID);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(flag);
        }

        public ActionResult DownloadFile(string FileName, int MasterData_Type_ID)
        {
            string ReduxFirmwareFiles = System.Web.HttpContext.Current.Server.MapPath(@"\");
            string CurrentFileName = ReduxFirmwareFiles + FileName;

            string contentType = string.Empty;
            if (FileName != null)
            {
                if (System.IO.File.Exists(CurrentFileName))
                {
                    if (CurrentFileName.ToLower().Contains(".bin"))
                    {
                        contentType = "application/bin";
                        return File(CurrentFileName, contentType, (FileName));
                    }
                }
            }

            TempData["IsFileExists"] = false;
            return RedirectToAction("EditSoftDetail", new { id = MasterData_Type_ID });
        }

        #region Move Machine
        public ViewResult MoveMachine()
        {
            IEnumerable<DtoList> list = new List<DtoList>();
            IEnumerable<DtoList> listCustomer = generalServices.GetCustomerService();
            var customerList = listCustomer as DtoList[] ?? listCustomer.ToArray();
            var transactionTypeLst = manageMachineService.GetMachineHistoryConfigTypeByValue(ConstantEntities.MachineHistoryTransaction, ConstantEntities.MachineConfigureHistoryTypeName);

            var replaceMachModel = new MoveMachineModel
            {
                CustomerListFrom = customerList,
                SubsidiaryNameListFrom = list,
                SubAgentNameListFrom = list,
                LocationListFrom = list,
                ShippedMachines = list,
                CustomerListTo = customerList,
                SubsidiaryNameListTo = list,
                SubAgentNameListTo = list,
                LocationListTo = list,
                TransactionTypeLst = transactionTypeLst,
            };
            return View(replaceMachModel);
        }

        public ActionResult UpdateMoveMachine(MoveMachineModel model)
        {
            if (model == null) return RedirectToAction("MoveMachine");

            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            bool isSuccess = manageMachineService.MoveMachine(model, currentUser.User_Id);
            if (isSuccess)
                TempData["MachineMoved"] = true;
            return RedirectToAction("MoveMachine");
        }

        #endregion

        #region ConfigureMachine
        public ActionResult ConfigureMachineHistory()
        {
            ConfigureMachineHistoryModel obj = new ConfigureMachineHistoryModel();
            return View("ConfigureMachineHistory", obj);
        }

        public ActionResult GetConfigureMachineHistoryData([DataSourceRequest]DataSourceRequest request)
        {

            //IEnumerable<RoleDetailsModel> RoleDetails = _IRoleManagementService.GetRoleList();
            List<ConfigureMachineHistoryModel> ObjConfigureMachineHistList = manageMachineService.GetConfigureMachineHistory();

            DataSourceResult result = ObjConfigureMachineHistList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEditConfigureMachineHistory(ConfigureMachineHistoryModel ConfigMachineHistModel)
        {
            var Result = false;
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            Result = manageMachineService.AddEditConfigureMachineHistory(ConfigMachineHistModel, currentUser.User_Id);
            if (Result == true && ConfigMachineHistModel.ConfigureMachineHistoryId > 0)
            {

                TempData["isUpdatedSuccess"] = true;
            }
            else
            {
                TempData["isCreatedSuccess"] = true;
            }

            return RedirectToAction("ConfigureMachineHistory");
        }

        public ActionResult AddConfigureMachineHistory()
        {
            var ObjExistingConfigureMachineHistory = new ConfigureMachineHistoryModel();
            ObjExistingConfigureMachineHistory.ConfigureTypeList = generalServices.GetMasterValuesByType(ConstantEntities.MachineConfigureHistoryTypeName);
            ObjExistingConfigureMachineHistory.ConfigureMachineHistoryId = 0;
            return View(ObjExistingConfigureMachineHistory);
        }

        public ActionResult EditConfigureMachineHistory(int Id)
        {
            var ObjExistingConfigureMachineHistory = new ConfigureMachineHistoryModel();
            if (Id != 0)
            {
                ObjExistingConfigureMachineHistory = manageMachineService.GetConfigureMachineHistoryById(Id);
                ObjExistingConfigureMachineHistory.ConfigureTypeList = generalServices.GetMasterValuesByType(ConstantEntities.MachineConfigureHistoryTypeName);
            }
            return View(ObjExistingConfigureMachineHistory);
        }

        public JsonResult CheckDuplicateCustomeFieldName(string ConfiguredValue, string ConfigureTypeId, string ConfigureMachineHistoryId)
        {
            int _ConfigureTypeId = 0;
            if (ConfigureTypeId != null && ConfigureTypeId != string.Empty)
            {
                _ConfigureTypeId = Convert.ToInt32(ConfigureTypeId);
            }
            int _ConfigureMachineHistoryId = 0;
            if (ConfigureMachineHistoryId != null && ConfigureMachineHistoryId != string.Empty && ConfigureMachineHistoryId != "undefined")
            {
                _ConfigureMachineHistoryId = Convert.ToInt32(ConfigureMachineHistoryId);
            }
            var data = !manageMachineService.CheckDuplicateConfigureHistoryName(ConfiguredValue, _ConfigureTypeId, _ConfigureMachineHistoryId);
            return Json(data);
        }

        #endregion

        #region MachineHistory
        public ActionResult MachineHistory()
        {
            MachineHistory objMachineHistoryList = new MachineHistory();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.IsModalValid = true;
            //objMachineHistoryList.CustomerNameList = generalServices.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
            // objMachineHistoryList.LocationList = generalServices.GetEmptyDDLWithoutSelect();
            //objMachineHistoryList.SubsidiaryList = generalServices.GetEmptyDDLWithoutSelect();
            // objMachineHistoryList.SubAgentList = generalServices.GetEmptyDDLWithoutSelect();
            ViewBag.culst = bindCustomer();
            ViewBag.SubsidiaryLst = generalServices.GetEmptyDDL();
            ViewBag.AgentLst = generalServices.GetEmptyDDL();
            ViewBag.Locations = generalServices.GetEmptyDDL();

            return View();
        }

        [AjaxHandleException]
        public ActionResult ViewMachineHistory([DataSourceRequest] DataSourceRequest request, MachineHistoryParameters objMachineHistoryParameter)
        {
            var data = manageMachineService.GetMachineHistoryLst(objMachineHistoryParameter.CustomerId, objMachineHistoryParameter.LocationId, objMachineHistoryParameter.SubsidiaryID, objMachineHistoryParameter.SubAgentID);
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EditMachineHistory
        public ActionResult EditMachineHistory(int Id)
        {
            EditMachineHistory ObjEditMachineHistory = new EditMachineHistory();
            ObjEditMachineHistory = manageMachineService.GetMachineHistoryByMachineId(Id);
            ObjEditMachineHistory.MachineId = Id;
            ObjEditMachineHistory.ReasonTypeList = manageMachineService.GetMachineHistoryConfigTypeByValue(ConstantEntities.MachineHistoryReason, ConstantEntities.MachineConfigureHistoryTypeName);
            ObjEditMachineHistory.StatusTypeList = manageMachineService.GetMachineHistoryConfigTypeByValue(ConstantEntities.MachineHistoryStatus, ConstantEntities.MachineConfigureHistoryTypeName);
            return View(ObjEditMachineHistory);
        }

        [AjaxHandleException]
        public ActionResult ViewEditMachineHistory([DataSourceRequest] DataSourceRequest request, MachineHistoryParameters objMachineHistoryParameter)
        {
            var _data = manageMachineService.GetEditMachineHistoryDetailsByMachineId(objMachineHistoryParameter.MachineId);
            DataSourceResult result = _data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteMachineHistory(int MachineHistoryId)
        {
            try
            {
                if (MachineHistoryId > 0)
                {
                    var result = manageMachineService.DeleteMachineHistoryById(MachineHistoryId);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View("MachineHistory");
        }

        public ActionResult DeleteConfigureMachineHistory(int ConfigureMachineHistoryID)
        {
            try
            {
                if (ConfigureMachineHistoryID > 0)
                {
                    var result = manageMachineService.DeleteConfigureMachineHistoryById(ConfigureMachineHistoryID);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View("MachineHistory");
        }

        public ActionResult UpdateMachineHistory(EditMachineHistory EditMachineHist, IEnumerable<HttpPostedFileBase> files)
        {
            int MachineId = EditMachineHist.MachineId;
            string dirPathToUpload = Convert.ToString(EditMachineHist.MachineId);
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Machine";
            string SubDirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Machine\Machine_" + EditMachineHist.MachineId;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                if (!Directory.Exists(SubDirPath))
                {
                    Directory.CreateDirectory(SubDirPath);
                }
            }
            else
            {
                if (!Directory.Exists(SubDirPath))
                {
                    Directory.CreateDirectory(SubDirPath);
                }
            }
            EditMachineHist.MachineDocs = storeAttachmentFileName(files, SubDirPath, MachineId);
            storeAttachment(files, dirPathToUpload, MachineId);

            bool IsUpdated = manageMachineService.AddMachineHistory(EditMachineHist);
            TempData["isUpdatedSuccess"] = true;
            TempData["ViewModelMsg"] = "Machine History Updated Successfully.";
            return RedirectToAction("MachineHistory");
        }


        /// <summary>
        /// store Attachment in the folder
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dirPath"></param>
        ///  /// <param name="MachineId"></param>
        public void storeAttachment(IEnumerable<HttpPostedFileBase> files, string dirPath, int MachineId)
        {
            var AttachmentFileName = string.Empty;
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

                    AttachmentFileName = Path.Combine(Server.MapPath("~/TempUpload/Machine/Machine_" + MachineId + "/"), Filename);
                    item.SaveAs(AttachmentFileName);
                }
            }
        }

        public IEnumerable<MachineDocumentsModel> storeAttachmentFileName(IEnumerable<HttpPostedFileBase> files, string dirPath, int MachineId)
        {
            List<MachineDocumentsModel> fileUploadModel = new List<MachineDocumentsModel>();
            EditMachineHistory MacHist = new EditMachineHistory();
            if (files != null)
            {
                foreach (var item in files)
                {
                    var path = string.Format(@"{0}\{1}", dirPath, item.FileName);
                    MachineDocumentsModel records = new MachineDocumentsModel();
                    records.Machine_ID = MachineId;
                    records.Machine_Doc_Name = Path.GetFileNameWithoutExtension(item.FileName); ;
                    records.Doc_Path = item.FileName;// It can be complete Path of the file;
                    records.Created_Date = DateTime.Now;
                    records.Machine_Doc_type = Path.GetExtension(path);
                    fileUploadModel.Add(records);

                }
            }
            MacHist.MachineDocs = fileUploadModel;
            return MacHist.MachineDocs;
        }

        /// <summary>
        /// Get All Records of a particular Machine on the basis of ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="objMachineHistoryParameter"></param>
        /// <returns></returns>
        public ActionResult ManageMachineDocumentsAjax([DataSourceRequest] DataSourceRequest request, string Id, string MacId)
        {
            EditMachineHistory ob = new EditMachineHistory();
            if (Id != string.Empty && MacId != string.Empty)
            {
                int MachineHistoryId = Convert.ToInt32(Id);
                int MachineId = Convert.ToInt32(MacId);
                IEnumerable<MachineDocumentsModel> objMacDoc = manageMachineService.GetMachineDocsList(MachineId, MachineHistoryId);
                ob.MachineDocs = objMacDoc;
                ob.MachineDocs = getDocument(ob, MachineId);
                DataSourceResult result = ob.MachineDocs.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Fetch Documents From folder On the Basis of Machine
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="Custid"></param>
        /// <returns></returns>
        public IEnumerable<MachineDocumentsModel> getDocument(EditMachineHistory Obj, int MachineId)
        {
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Machine\Machine_" + MachineId;
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            EditMachineHistory ob = new EditMachineHistory();
            List<MachineDocumentsModel> file = new List<MachineDocumentsModel>();
            foreach (var item in Obj.MachineDocs)
            {
                var path = string.Format(@"{0}\{1}", dirPath, (item.Machine_Doc_Name + item.Machine_Doc_type));
                if (System.IO.File.Exists(path))
                {
                    MachineDocumentsModel records = new MachineDocumentsModel();
                    records.Machine_ID = item.Machine_ID;
                    records.Machine_Doc_Name = item.Machine_Doc_Name;
                    records.Machine_Doc_type = item.Machine_Doc_type;
                    records.Created_Date = item.Created_Date;

                    file.Add(records);
                }
            }
            ob.MachineDocs = file;
            return ob.MachineDocs;

        }

        /// <summary>
        /// Download Machine Document
        /// </summary>
        /// <param name="DocDetails"></param>
        /// <returns></returns>
        public FileResult DownloadFileActual(string DocDetails)
        {

            MachineDocumentsModel DocumentDetails = new MachineDocumentsModel();
            if (DocDetails != null)
            {
                DocumentDetails.Machine_ID = Convert.ToInt16(DocDetails.Split(',').First());
                DocumentDetails.Machine_Doc_Name = DocDetails.Split(',').Last();
            }

            DocumentDetails = manageMachineService.GetMachineDocumentDetailById(DocumentDetails);
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Machine\Machine_" + DocumentDetails.Machine_ID;
            string CurrentFileName = dirPath + @"\" + DocumentDetails.Machine_Doc_Name + DocumentDetails.Machine_Doc_type;

            string contentType = string.Empty;

            //if (CurrentFileName.ToLower().Contains(".pdf"))
            //{
            //    contentType = "application/pdf";
            //}

            //else if (CurrentFileName.ToLower().Contains(".docx"))
            //{
            //    contentType = "application/docx";
            //}
            //else if (CurrentFileName.ToLower().Contains(".xlsx"))
            //{
            //    contentType = "application/docx";
            //}
            //else
            try
            {
                if (CurrentFileName.ToLower().Contains(".jpg"))
                {
                    contentType = "image/jpg";
                }
                else if (CurrentFileName.ToLower().Contains(".png"))
                {
                    contentType = "image/png";
                }

                return File(CurrentFileName, contentType, (DocumentDetails.Machine_Doc_Name + DocumentDetails.Machine_Doc_type));
            }
            catch (Exception ex)
            {
                throw;
            }
            return File(CurrentFileName, contentType, (DocumentDetails.Machine_Doc_Name + DocumentDetails.Machine_Doc_type));
        }
        #endregion
    }
}
