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
using System.IO;
using Revive.Redux.Common;

namespace Revive.Redux.UI.Controllers
{
    [Authorize]
    [ReviveAuth]
    public class ManageMembershipConfigController : Controller
    {
        #region Private Variables

        private ICustomerManagement _ICustomerManagement = null;
        private IEnumerable<MembershipConfigModel> objMembershipConfig = null;
        private IGeneralService generalService = null;
        private IMembershipService memberService = null;

        #endregion

        #region Construtors

        public ManageMembershipConfigController()
        {
            _ICustomerManagement = new CustomerManagement();
            generalService = new GeneralService();
            memberService = new MembershipService();

        }

        #endregion

        #region Actions
        // GET: ManageMembershipConfig/Details/5
        public ActionResult Details()
        {
            objMembershipConfig = _ICustomerManagement.GetCustomerMembership(0);
            return View("ManageMembership", objMembershipConfig);
        }

        // POST: ManageMembershipConfig/Create
        [HttpPost]
        public ActionResult Create(MembershipConfigModel objCustomerMembership)
        {
            bool bResult = false;

            bResult = _ICustomerManagement.UpdateCustomerMembership(objCustomerMembership);
            ViewBag.ViewModel = bResult;

            if (bResult)
            {
                ViewBag.ViewModelMsg = ReviveMessages.Update;
                TempData["isUpdatedSuccess"] = true;

            }
            else
            {
                ViewBag.ViewModelMsg = "Error in Membership update.";
            }

            objCustomerMembership.PageButtonSubmit = "Save";
            objCustomerMembership.PageHeader = "Manage Membership Configurations";
            objCustomerMembership.PageMode = 1;

            if (bResult == true)
            {
                return RedirectToAction("Details");
            }
            else
            {
                return View("EditMembership", objCustomerMembership);
            }


        }

        // GET: ManageMembershipConfig/Edit/5
        public ActionResult Edit(int id)
        {
            objMembershipConfig = _ICustomerManagement.GetCustomerMembership(id);
            var objMembership = new MembershipConfigModel();

            objMembership.PageButtonSubmit = "Save";
            objMembership.PageHeader = "Manage Membership Configurations";
            objMembership.PageMode = 2;
            objMembership.CustomerId = id;

            if (objMembershipConfig.Count() == 1)
            {
                foreach (var objMembershipSingle in objMembershipConfig)
                {
                    objMembership.TotalRenewedReviveAllowed = objMembershipSingle.TotalRenewedReviveAllowed;
                    objMembership.TotalReviveAllowed = objMembershipSingle.TotalReviveAllowed;
                    objMembership.MembershipDuration = objMembershipSingle.MembershipDuration;
                    objMembership.MembershipRenewDuration = objMembershipSingle.MembershipRenewDuration;
                    objMembership.eligibiltyWaitPeriod = objMembershipSingle.eligibiltyWaitPeriod;
                    objMembership.VoidMembershipDays = objMembershipSingle.VoidMembershipDays;
                    objMembership.IsMultiDevice = objMembershipSingle.IsMultiDevice;
                    objMembership.NoOfDevices = objMembershipSingle.NoOfDevices;

                }
            }

            return View("EditMembership", objMembership);
        }

        [AjaxHandleException]
        public ActionResult MembershipAjax([DataSourceRequest] DataSourceRequest request)
        {
            objMembershipConfig = _ICustomerManagement.GetCustomerMembership(0);
            DataSourceResult result = objMembershipConfig.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        [AjaxHandleException]
        public JsonResult GetSubsidiaryList(int CustomeId, bool bActive = false)
        {
            var customers = generalService.GetSubsidiaryByCustomerId(CustomeId);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
        [AjaxHandleException]
        public JsonResult GetSubAgentList(int SubsidiaryId, bool bActive = false)
        {
            var customers = generalService.GetAgentsBySubsidiaryId(SubsidiaryId);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public JsonResult GetLocationList(int CustomerId, int SubsidiaryId, int SubAgentId, bool bActive = false)
        {
            var customers = generalService.GetCustomerLocation(CustomerId, SubsidiaryId, SubAgentId, bActive);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageMember()
        {

            ViewBag.IsGridShow = false;
            ManageMember objMemberList = new ManageMember();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.IsModalValid = true;
            objMemberList.CustomerNameList = generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
            objMemberList.LocationList = generalService.GetEmptyDDLWithoutSelect();
            objMemberList.SubsidiaryList = generalService.GetEmptyDDLWithoutSelect();
            objMemberList.SubAgentList = generalService.GetEmptyDDLWithoutSelect();
            return View(objMemberList);
        }
        /// <summary>
        /// CR15
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageMemberShow()
        {

            ViewBag.IsGridShow = true;
            ManageMember objMemberList = new ManageMember();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.IsModalValid = true;
            objMemberList.CustomerNameList = generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();

            objMemberList.LocationList = generalService.GetEmptyDDLWithoutSelect();
            objMemberList.SubsidiaryList = generalService.GetEmptyDDLWithoutSelect();
            objMemberList.SubAgentList = generalService.GetEmptyDDLWithoutSelect();
            return View("ManageMember", objMemberList);
        }


        [AjaxHandleException]
        public ActionResult SearchMembershipsAjax([DataSourceRequest] DataSourceRequest request, MemberShipParameters objMemberParameter)
        {
            var _data = memberService.GetMembershipDetailsES(request, objMemberParameter);
            // DataSourceResult result = _data.ToDataSourceResult(request);
            ///return Json(result, JsonRequestBehavior.AllowGet);


            var jsonResult = Json(_data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        public ActionResult EditMember(int Id)
        {
            MemberShipParameters objMemberList = new MemberShipParameters();
            objMemberList.Membership_Details_ID = Id;
            var Memberdata = memberService.GetMemberDetailsByMemberDtlsID(objMemberList);
            if (Memberdata.MobileNumber != null && Memberdata.MobileNumber != "" && Memberdata.IsMultiDevice == true)
            {

                var PhoneNumberArray = Memberdata.MobileNumber.Split(',');
                Memberdata.custMaxNoOfDevices = PhoneNumberArray.Length;
                for (int i = 0; i <= PhoneNumberArray.Length - 1; i++)
                {

                    switch (i)
                    {
                        case 0:
                            Memberdata.Phone_Number = PhoneNumberArray[i];
                            break;
                        case 1:
                            Memberdata.Device2 = PhoneNumberArray[i];
                            break;
                        case 2:
                            Memberdata.Device3 = PhoneNumberArray[i];
                            break;
                        case 3:
                            Memberdata.Device4 = PhoneNumberArray[i];
                            break;
                        case 4:
                            Memberdata.Device5 = PhoneNumberArray[i];
                            break;

                    }

                }


            }
            return View(Memberdata);
        }

        public ActionResult UpdateMember(ManageMember objMemberDetails)
        {

            string DeviceNumber = objMemberDetails.MobileNumber;
            if (objMemberDetails.IsMultiDevice == true)
            {
                switch (objMemberDetails.custMaxNoOfDevices)
                {
                    case 1:
                        ModelState.Remove("Device2");
                        ModelState.Remove("Device3");
                        ModelState.Remove("Device4");
                        ModelState.Remove("Device5");
                        break;
                    case 2:
                        ModelState.Remove("Device3");
                        ModelState.Remove("Device4");
                        ModelState.Remove("Device5");
                        DeviceNumber += "," + objMemberDetails.Device2;
                        break;
                    case 3:
                        ModelState.Remove("Device4");
                        ModelState.Remove("Device5");
                        DeviceNumber += "," + objMemberDetails.Device2 + "," + objMemberDetails.Device3;
                        break;
                    case 4:
                        ModelState.Remove("Device5");
                        DeviceNumber += "," + objMemberDetails.Device2 + "," + objMemberDetails.Device3 + "," + objMemberDetails.Device4;
                        break;
                    case 5:
                        DeviceNumber += "," + objMemberDetails.Device2 + "," + objMemberDetails.Device3 + "," + objMemberDetails.Device4 + "," + objMemberDetails.Device5;
                        break;

                }
            }
            else
            {
                ModelState.Remove("Device2");
                ModelState.Remove("Device3");
                ModelState.Remove("Device4");
                ModelState.Remove("Device5");
            }


            if (ModelState.IsValid)
            {


                var lastReviveLastDatetime = memberService.GetReviveLastDateByMembershipId(objMemberDetails.MembershipId);
                if (lastReviveLastDatetime != null)
                {
                    if (objMemberDetails.MembershipStartDate > lastReviveLastDatetime && lastReviveLastDatetime != Convert.ToDateTime("01/01/1900"))
                    {

                        TempData["isMemberUpdatedSuccess"] = false;
                        TempData["ErrorMessages"] = "Membership start date should be greater than " + lastReviveLastDatetime.ToString();
                        return RedirectToAction("EditMember", new { id = objMemberDetails.Membership_Det_ID });
                    }

                    objMemberDetails.MobileNumber = DeviceNumber;
                    var result = memberService.updateMemberDetailsByMemberDtlsID(objMemberDetails);
                    TempData["isMemberUpdatedSuccess"] = true;
                    return RedirectToAction("ManageMember");
                }

            }
            return RedirectToAction("EditMember", objMemberDetails.Membership_Det_ID);
        }

        public ActionResult ValidateVoidMembership(MemberShipParameters MembershipPara)
        {

            MemberShipParameters MemberResult = new MemberShipParameters();
            var MemberShipDetails = memberService.GetMemberDetailsByMemberDtlsID(MembershipPara);
            MemberResult.VoidDays = MemberShipDetails.voidDays;
            MemberResult.DryAttemptCount = MemberShipDetails.DryAttemptCount;
            MemberResult.VoidExpire = MemberShipDetails.VoidExpire;
            return Json(MemberResult, JsonRequestBehavior.AllowGet);


        }

        public ActionResult UpdateVoidMembership(MemberShipParameters MembershipPara)
        {
            MemberShipParameters MemberResult = new MemberShipParameters();
            var Result = memberService.voidMemberShipByMembershipId(MembershipPara);
            if (Result == true)
            {
                MemberResult.WarningMessage = "updated";
                TempData["isVoidMemberUpdatedSuccess"] = true;
            }
            else
            {
                MemberResult.WarningMessage = "failed";
                TempData["isVoidMemberUpdatedFailed"] = true;
            }


            //MemberShipParameters MemberResult = new MemberShipParameters();
            //var MemberShipDetails = memberService.GetMemberDetailsByMemberDtlsID(MembershipPara);

            //if (MembershipPara.IsVoidUpdate == true)
            //{
            //    var Result = memberService.voidMemberShipByMembershipId(MembershipPara);
            //    if (Result == true)
            //    {
            //        MemberResult.WarningMessage = "updated";
            //        TempData["isVoidMemberUpdatedSuccess"] = true;
            //    }
            //    else
            //    {
            //        MemberResult.WarningMessage = "failed";
            //        TempData["isVoidMemberUpdatedFailed"] = true;
            //    }
            //}
            //else
            //{
            //    MemberResult.IsVoidUpdate = true;
            //    MemberResult.VoidDays = MemberShipDetails.voidDays;
            //    MemberResult.DryAttemptCount = MemberShipDetails.DryAttemptCount;
            //    MemberResult.VoidExpire = MemberShipDetails.VoidExpire;
            //}

            return Json(MemberResult, JsonRequestBehavior.AllowGet);
        }

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

        public ActionResult AddMemberByXLSFile(ManageMember objManageMember, IEnumerable<HttpPostedFileBase> files)
        {
            Session["MembershipWebResult"] = null;
            ViewBag.IsGridShow = false;
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            var objcustomerlst = new List<DtoList>();

            var customerDetails = _ICustomerManagement.GetCustomersList(objManageMember.CustomerId).FirstOrDefault();

            objManageMember.no_of_FreeDries = customerDetails.ReviveAllowed;
            objManageMember.membershipDuration = (int)customerDetails.Membership_Duration;
            objManageMember.custMaxNoOfDevices = customerDetails.CustomerMaxDevices;
            objManageMember.IsMultiDevice = customerDetails.IsCustomerMultiDevice;


            if (objManageMember.CustomerId > 0 && files != null && files.Count() > 0)
            {
                string sDirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Membership\" + objManageMember.CustomerId;
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

                string dirPath1 = @"\Membership\" + objManageMember.CustomerId.ToString();

                storeDocument(files, dirPath1);

                foreach (var item in files)
                {
                    objManageMember.FileName = sPath + @"\\" + item.FileName;
                }
                ManageMember objMembershipResult = new ManageMember();
                objManageMember.user_Id = currentUser.User_Id;
                bool bResult = false;
                if (customerDetails.TempLateId == ConstantEntities.CustomerTemplateAW)
                {
                    bResult = memberService.MemberFileUploadAW(objManageMember, out objMembershipResult);
                }
                else
                {
                    bResult = memberService.MemberFileUpload(objManageMember, out objMembershipResult);
                }
                //bool bResult = memberService.MemberFileUpload(objManageMember, out objMembershipResult);
                Session["MembershipWebResult"] = objMembershipResult;

                ViewBag.IsModalValid = false;

                objMembershipResult.CustomerNameList = generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
                objMembershipResult.LocationList = generalService.GetEmptyDDLWithoutSelect();
                objMembershipResult.SubsidiaryList = generalService.GetEmptyDDLWithoutSelect();
                objMembershipResult.SubAgentList = generalService.GetEmptyDDLWithoutSelect();

                return View("ManageMember", objMembershipResult);
            }
            return View("ManageMember", "ManageMembershipConfig");
        }

        //[AjaxHandleException]
        public ActionResult MemberResultAjax([DataSourceRequest] DataSourceRequest request)
        {
            var objMembershipWebResult = new ReportConfigModel();
            if (Session["MembershipWebResult"] != null)
            {
                objMembershipWebResult = (ReportConfigModel)Session["MembershipWebResult"];
            }

            DataSourceResult result = objMembershipWebResult.LocationResult.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}