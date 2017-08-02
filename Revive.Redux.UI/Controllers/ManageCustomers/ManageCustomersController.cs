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

namespace Revive.Redux.UI.Controllers.ManageCustomers
{
    [Authorize]
    [ReviveAuth]
    public class ManageCustomersController : Controller
    {
        #region Private Variables
        private ICustomerManagement _ICustomerManagement = null;
        IGeneralService _IGeneralService = null;
        private ILocationManagement _ILocationManagement = null;
        List<CustomerLocationModel> lstLocations = null;
        public string CustomerID = string.Empty;
        public string DocName = string.Empty;
        LocationModel objLocationModel = null;
        NotificationModel objNotification = null;
        //string DocType = string.Empty;
        private ILogService logService = null;
        private IFormManagementService _IFormManagementService = null;
        private IUserManagmentService _ManageUserService = null;
        private CurrentUserDetail objSessionData = null;


        #endregion

        #region Contructor
        public ManageCustomersController()
        {
            _ICustomerManagement = new CustomerManagement();
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

        # region Manage Customers
        // GET: ManageCustomers
        public ActionResult ManageCustomers()
        {

            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new ManageCustomersModel();
            model.PageAccessCode = currentUser.PageAccessCode;

            return View("ManageCustomers", model);
        }

        /// <summary>
        /// Get All Customers And Bind Data to Customer Grid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AjaxHandleException]
        public ActionResult ManageCustomersAjax([DataSourceRequest] DataSourceRequest request)
        {
            objSessionData = (CurrentUserDetail)Session["CurrentUser"];
            IEnumerable<ManageCustomersModel> objUser = null;


            if (objSessionData.PageAccessCode == PageAccessCode.ACCNTMGR)
            {
                objUser = _ICustomerManagement.GetCustomersList(objSessionData.User_Id);
            }
            else if (objSessionData.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
            {
                objUser = _ICustomerManagement.GetCustomersList((int)objSessionData.Customer_Id);
            }
            else
            {
                objUser = _ICustomerManagement.GetCustomersList();
            }

            DataSourceResult result = objUser.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AjaxHandleException]

        public ViewResult CreateCustomer()
        {
            ViewData["Create"] = "false";
            return View();
        }
        /// <summary>
        /// Get City On the Basis of State Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetCascadeCity(int Id)
        {
            ManageCustomersModel City = new ManageCustomersModel();

            var CityList = _IGeneralService.GetCity(Id);

            return Json(CityList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add and update Customer Records
        /// </summary>
        /// <param name="CustomerRecord"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public ActionResult AddCustomer(ManageCustomersModel CustomerRecord, IEnumerable<HttpPostedFileBase> files)
        {
            string sErrorMsg = string.Empty;
            ManageCustomersModel CreateCustomer = new ManageCustomersModel();
            DateTime? StartDate = null;
            DateTime? EndDate = null;
            if (Session["CustomerID"] != null)
            {
                Session["files"] = files;
                int CustID = Convert.ToInt16(Session["CustomerID"]);
                string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\" + CustID;
                string dirPath1 = CustID.ToString();
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                    CustomerRecord.CustomerDocs = storeFile(files, dirPath, CustID);
                    storeDocument(files, dirPath1);
                }
                else
                {
                    CustomerRecord.CustomerDocs = storeFile(files, dirPath, CustID);
                    storeDocument(files, dirPath1);
                }

                CreateCustomer = Records(CustomerRecord);
                StartDate = CreateCustomer.Lease_Start_Date;
                EndDate = CreateCustomer.Lease_end_date;
                //bool flag = _IGeneralService.CompareDate(StartDate, EndDate);
                //if (flag == false)
                //{
                //    ViewData["Create"] = "true";
                //    sErrorMsg = "Start date should be less than end date";
                //    ModelState.AddModelError("Name", sErrorMsg);
                //    return View("CreateCustomer", CreateCustomer);
                //}
                ManageCustomersModel Result = _ICustomerManagement.CreateCustomer(CreateCustomer);
                TempData["isUpdatedSuccess"] = true;

                if (Result != null)
                {
                    RedirectToAction("ManageCustomers");
                }

            }
            else
            {
                CreateCustomer = Records(CustomerRecord);
                StartDate = CreateCustomer.Lease_Start_Date;
                EndDate = CreateCustomer.Lease_end_date;
                //bool flag = _IGeneralService.CompareDate(StartDate, EndDate);
                //if (flag == false)
                //{
                //    ViewData["Create"] = "false";
                //    sErrorMsg = "Start date should be less than end date";
                //    ModelState.AddModelError("Name", sErrorMsg);
                //    return View("CreateCustomer", CreateCustomer);
                //}
                int CustID = Convert.ToInt16(Session["CustomerID"]);
                string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\" + CustID;
                CustomerRecord.CustomerDocs = storeFile(files, dirPath, CustID);
                CreateCustomer.CustomerDocs = CustomerRecord.CustomerDocs;
                ManageCustomersModel Result = _ICustomerManagement.CreateCustomer(CreateCustomer);
                TempData["isCreatedSuccess"] = true;
                TempData["Success"] = Result.Successmsg;
                if (Result != null)
                {
                    CustID = Result.Customer_ID;
                    dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\" + CustID;
                    string dirPath1 = CustID.ToString();
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);

                        storeDocument(files, dirPath1);
                    }

                    RedirectToAction("ManageCustomers");
                }
            }
            Session["CustomerID"] = null;
            Session["files"] = null;
            return RedirectToAction("ManageCustomers");
        }
        /// <summary>
        /// Mapping function
        /// </summary>
        /// <param name="CustomerRecord"></param>
        /// <returns></returns>
        public ManageCustomersModel Records(ManageCustomersModel CustomerRecord)
        {
            ManageCustomersModel CreateCustomer = new ManageCustomersModel();
            CreateCustomer.Customer_ID = Convert.ToInt16(Session["CustomerID"]);
            if (CustomerRecord.Customer_Ref_Code != null)
            {
                CreateCustomer.Customer_Ref_Code = CustomerRecord.Customer_Ref_Code.Trim();
            }
            CreateCustomer.CustomerDocs = CustomerRecord.CustomerDocs;
            CreateCustomer.Customer_Name = CustomerRecord.Customer_Name.Trim();
            CreateCustomer.Acct_manager_Name = CustomerRecord.Acct_manager_Name.Trim();
            CreateCustomer.AccountmanagerUserID = CustomerRecord.AccountmanagerUserID;
            CreateCustomer.Primary_City_Name = CustomerRecord.Primary_City_Name;
            CreateCustomer.Acct_manager_Primary_City_Name = CustomerRecord.Acct_manager_Primary_City_Name;
            if (CustomerRecord.Email_ID != null)
            {
                CreateCustomer.Email_ID = CustomerRecord.Email_ID;
            }
            if (CustomerRecord.Lease_end_date != null)
            {
                CreateCustomer.Lease_end_date = CustomerRecord.Lease_end_date;
            }
            if (CustomerRecord.Lease_Start_Date != null)
            {
                CreateCustomer.Lease_Start_Date = CustomerRecord.Lease_Start_Date;
            }
            CreateCustomer.Primary_Address = CustomerRecord.Primary_Address;
            //CreateCustomer.Additional_Address_info = CustomerRecord.Additional_Address_info;
            if (CustomerRecord.Primary_Phone != null)
            {
                CreateCustomer.Primary_Phone = CustomerRecord.Primary_Phone;
            }
            // var record = CustomerRecord.CityList;
            CreateCustomer.Primary_City = CustomerRecord.Primary_City;
            if (CustomerRecord.Primary_State != null)
            {
                CreateCustomer.Primary_State = CustomerRecord.Primary_State;
            }
            if (CustomerRecord.Primary_ZipCode != null)
            {
                CreateCustomer.Primary_ZipCode = CustomerRecord.Primary_ZipCode;
            }
            // CreateCustomer.Acct_manager_Additional_Address_info = CustomerRecord.Acct_manager_Additional_Address_info;
            CreateCustomer.Acct_manager_Primary_Address = CustomerRecord.Acct_manager_Primary_Address;
            CreateCustomer.Acct_manager_Primary_City = CustomerRecord.Acct_manager_Primary_City;
            if (CustomerRecord.Acct_manager_Primary_State != null)
            {
                CreateCustomer.Acct_manager_Primary_State = CustomerRecord.Acct_manager_Primary_State;
            }
            if (CustomerRecord.Acct_manager_Primary_Zip != null)
            {
                CreateCustomer.Acct_manager_Primary_Zip = CustomerRecord.Acct_manager_Primary_Zip;
            }
            if (CustomerRecord.Acct_manager_Primary_Phone != null)
            {
                CreateCustomer.Acct_manager_Primary_Phone = CustomerRecord.Acct_manager_Primary_Phone;
            }
            return CreateCustomer;

        }
        public IEnumerable<CustomerDocumentsModel> storeFile(IEnumerable<HttpPostedFileBase> files, string dirPath, int CustomerID)
        {
            List<CustomerDocumentsModel> fileUploadModel = new List<CustomerDocumentsModel>();
            ManageCustomersModel coll = new ManageCustomersModel();
            if (files != null)
            {
                foreach (var item in files)
                {
                    var path = string.Format(@"{0}\{1}", dirPath, item.FileName);
                    CustomerDocumentsModel records = new CustomerDocumentsModel();
                    records.Customer_ID = CustomerID;
                    records.Customer_Doc_Name = Path.GetFileNameWithoutExtension(item.FileName); ;
                    records.Doc_Path = item.FileName;// It can be complete Path of the file;
                    records.Created_Date = DateTime.Now;
                    records.Customer_Doc_type = Path.GetExtension(path);

                    fileUploadModel.Add(records);

                }
            }
            coll.CustomerDocs = fileUploadModel;
            return coll.CustomerDocs;
            // _ICustomerManagement.FilePathUpload(fileUploadModel);
        }
        /// <summary>
        /// Get Customer Details in the basis of ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCustomer(int id)
        {
            ViewBag.Title = "Edit Customer";
            var model = new ManageCustomersModel();
            if (id != 0)
            {

                Session["CustomerID"] = id;
                ViewData["Create"] = "false";
                model = _ICustomerManagement.GetCustomerDetailsById(id);
                int State_Id = Convert.ToInt16(model.Primary_State);
                //Check If File Exist to apply validation On Upload File Control
                bool fileexist = CheckFileExists(id);
                bool DocDataExists = _ICustomerManagement.CheckCustomerDocExists(id);

                if (fileexist == true && DocDataExists == true)
                {
                    ViewBag.File = true;
                    ViewData["Create"] = "true";
                }
                //End
                //if (State_Id != 0)
                //{
                //    ViewBag.Citylst = _IGeneralService.GetCity(State_Id);
                //}
                //else
                //{
                //    ViewBag.Citylst = _IGeneralService.GetEmptyDDL();
                //}
                //model.Created_Date = DateTime.Now;
                ViewBag.isUpdateMode = true;

            }
            return View("CreateCustomer", model);
        }
        /// <summary>
        /// Activate And Deactivate Customer Status
        /// </summary>
        /// <param name="CustomerRecord"></param>
        /// <returns></returns>
        public ActionResult ActivateDeactivateCustomer(ManageCustomersModel CustomerRecord)
        {
            if (CustomerRecord.Customer_ID != 0)
            {

                Session["CustomerID"] = CustomerRecord.Customer_ID;
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

                //bool result = _ICustomerManagement.ActivateDeactivateCustomerById(CustomerRecord);
                bool result = false;

                if (CustomerRecord.Status == true)
                {
                    result = _IGeneralService.DeActivateByCustomerId(CustomerRecord.Customer_ID, objCurrentUserDetail.User_Id);
                }
                else
                {
                    result = _IGeneralService.ActivateByCustomerId(CustomerRecord.Customer_ID, objCurrentUserDetail.User_Id);
                }

                if (result == true)
                {
                    ManageCustomersModel CustomerDetails = SendNotification(CustomerRecord);
                    string sStatus = string.Empty;
                    // sStatus = (CustomerDetails.Status) ? "Activated" : "Deactivated";
                    if (CustomerDetails.Status)
                    {
                        CustomerDetails.Successmsg = ReviveMessages.Active;
                    }
                    else
                    {
                        CustomerDetails.Successmsg = ReviveMessages.Deactivate;
                    }
                    // CustomerDetails.Successmsg = "Customer "+ CustomerDetails.Customer_Name + " has been " + sStatus;
                    return Json(CustomerDetails, JsonRequestBehavior.AllowGet);
                }
                //model.Created_Date = DateTime.Now;
                ViewBag.isUpdateMode = true;
            }
            return View("ManageCustomers");
        }
        /// <summary>
        /// Function to send email notification on activation or on deactivate of customer
        /// </summary>
        /// <param name="objCustomer"></param>
        /// <returns></returns>
        private ManageCustomersModel SendNotification(ManageCustomersModel objCustomer)
        {

            // Mail ID commented  as client excel file item no 258
            string ccMailId = "";// Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);   // CC Account Managers       
            string sEmailTo = ConfigurationManager.AppSettings["ReduxSupport"].ToString();//Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, objCustomer.Customer_ID);   // CC Account Managers  
            ManageCustomersModel CustomerDetails = _ICustomerManagement.GetCustomerDetailsById(objCustomer.Customer_ID);
            objNotification = new NotificationModel();
            if (!string.IsNullOrEmpty(sEmailTo))
            {
                EmailHelper objEmailHelper = new EmailHelper();
                string sStatus = string.Empty;

                sStatus = (CustomerDetails.Status) ? "Activated" : "Deactivated";

                var objKeyWords = new Hashtable();
                objKeyWords.Add("CUSTOMER", CustomerDetails.Customer_Name);
                //objKeyWords.Add("LOCATION", objCustomerLocation.Location_Name);
                objKeyWords.Add("STATUS", sStatus);

                var objParser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ManageCustomer/CustomerStatus.html"), objKeyWords);
                var sEmailbody = objParser.Parse();
                objNotification.body_mail = sEmailbody;
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                objNotification.Created_by = objCurrentUserDetail.User_Id;
                objNotification.NotificationMessages = "Customer Status Notification";
                objNotification.Notification_Date = DateTime.Now;
                objNotification.Mail_Ids = sEmailTo;
                _IGeneralService.StoreNotification(objNotification);
                objEmailHelper.Send(sEmailTo, ccMailId, string.Empty, "Redux - Customer Status Notification", sEmailbody);
            }
            return CustomerDetails;
        }
        public JsonResult CheckCustomerName(String Customer_Name, int? Customer_ID)
        {
            var flag = false;
            if (Customer_ID == null)
            {
                Customer_ID = 0;
            }
            int _customer_Id = (int)Customer_ID;
            try
            {
                //var validateLogin = new CustomMemberShip();
                flag = _ICustomerManagement.CheckCheckCustomerNameExists(Customer_Name, _customer_Id);
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in CheckEmailAddress", ex);
                throw;
            }
            return Json(flag);
        }


        #endregion

        # region Manage Customer Documents

        public bool CheckFileExists(int id)
        {
            if (id != 0)
            {
                string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\" + id;
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (Directory.Exists(dirPath))
                {
                    int count = dir.GetFiles().Length;
                    if (count > 0)
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Get All Records of ap particular Customer on the basis of ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult ManageCustomersDocumentsAjax([DataSourceRequest] DataSourceRequest request)
        {
            int Custid = Convert.ToInt16(Session["CustomerID"]);
            IEnumerable<CustomerDocumentsModel> objUser = _ICustomerManagement.GetCustomerDocsList(Custid);
            ManageCustomersModel ob = new ManageCustomersModel();
            ob.CustomerDocs = objUser;
            ob.CustomerDocs = getDocument(ob, Custid);
            DataSourceResult result = ob.CustomerDocs.ToDataSourceResult(request);
            ViewBag.CustomeDocumentUpload = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Fetch Documents From folder On the Basis of Customer
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="Custid"></param>
        /// <returns></returns>
        public IEnumerable<CustomerDocumentsModel> getDocument(ManageCustomersModel Obj, int Custid)
        {
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\" + Custid;
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            ManageCustomersModel ob = new ManageCustomersModel();
            List<CustomerDocumentsModel> file = new List<CustomerDocumentsModel>();
            foreach (var item in Obj.CustomerDocs)
            {
                var path = string.Format(@"{0}\{1}", dirPath, (item.Customer_Doc_Name + item.Customer_Doc_type));
                if (System.IO.File.Exists(path))
                {
                    CustomerDocumentsModel records = new CustomerDocumentsModel();
                    records.Customer_ID = item.Customer_ID;
                    records.Customer_Doc_Name = item.Customer_Doc_Name;
                    records.Customer_Doc_type = item.Customer_Doc_type;
                    records.Created_Date = item.Created_Date;

                    file.Add(records);
                }
            }
            ob.CustomerDocs = file;
            return ob.CustomerDocs;

        }
        /// <summary>
        /// store Documents in the folder
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dirPath"></param>
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
        /// <summary>
        /// Delete Documents from Folder
        /// </summary>
        /// <param name="DocumentDetails"></param>
        public void DeleteDocument(CustomerDocumentsModel DocumentDetails)
        {
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\" + DocumentDetails.Customer_ID;
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (DocumentDetails.Customer_ID != null && DocumentDetails.Customer_Doc_Name != null)
            {
                var path = string.Format(@"{0}", dirPath);
                if (Directory.Exists(path))
                {
                    // string[] fileNames = System.IO.Directory.GetFiles(path);
                    foreach (FileInfo item in dir.GetFiles())
                    {
                        if (item.Name == (DocumentDetails.Customer_Doc_Name + DocumentDetails.Customer_Doc_type))
                        {
                            item.Delete();

                        }

                    }
                }
            }


        }
        /// <summary>
        /// Delete Customer Document Record
        /// </summary>
        /// <param name="DocDetails"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeleteCustomerDocs(string DocDetails)
        {
            if (DocDetails != null)
            {
                CustomerID = DocDetails.Split(',').First();
                DocName = DocDetails.Split(',').Last();

            }
            CustomerDocumentsModel DocumentDetails = new CustomerDocumentsModel();
            DocumentDetails.Customer_ID = Convert.ToInt16(CustomerID);
            DocumentDetails.Customer_Doc_Name = DocName;
            DocumentDetails = _ICustomerManagement.DeleteCustomerDocumentById(DocumentDetails);

            if (DocumentDetails != null)
            {
                DeleteDocument(DocumentDetails);
                //return RedirectToAction("CreateCustomer");
            }
            bool fileexist = CheckFileExists(Convert.ToInt16(CustomerID));
            bool DocDataExists = _ICustomerManagement.CheckCustomerDocExists(Convert.ToInt16(CustomerID));
            if (fileexist == true && DocDataExists == true)
            {
                ViewBag.File = true;
            }
            else
            {
                ViewBag.File = false;
            }
            DocumentDetails.FileStatus = ViewBag.File;

            return Json(DocumentDetails, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Download Customer Document
        /// </summary>
        /// <param name="DocDetails"></param>
        /// <returns></returns>
        public FileResult DownloadFileActual(string DocDetails)
        {

            CustomerDocumentsModel DocumentDetails = new CustomerDocumentsModel();
            if (DocDetails != null)
            {

                DocumentDetails.Customer_ID = Convert.ToInt16(DocDetails.Split(',').First());
                DocumentDetails.Customer_Doc_Name = DocDetails.Split(',').Last();

            }
            DocumentDetails = _ICustomerManagement.GetCustomerDocumentDetailById(DocumentDetails);
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\" + DocumentDetails.Customer_ID;
            string CurrentFileName = dirPath + @"\" + DocumentDetails.Customer_Doc_Name + DocumentDetails.Customer_Doc_type;


            string contentType = string.Empty;

            if (CurrentFileName.ToLower().Contains(".pdf"))
            {
                contentType = "application/pdf";
            }

            else if (CurrentFileName.ToLower().Contains(".docx"))
            {
                contentType = "application/docx";
            }

            //if (CustomerID != null && DocName != null)
            //{
            //    var path = string.Format(@"{0}", dirPath);
            //    if (Directory.Exists(path))
            //    {


            //    }
            //    else
            //    {
            //         FileNt = false;
            //        //return Json(FileNt, JsonRequestBehavior.AllowGet);

            //    }
            //}
            // return View("CreateCustomer");
            return File(CurrentFileName, contentType, (DocumentDetails.Customer_Doc_Name + DocumentDetails.Customer_Doc_type));
        }

        #endregion

        #region Manage Customer Location

        // GET: CustomerLocation
        public ActionResult Locations()
        {
            objLocationModel = new LocationModel();
            objLocationModel.CustomerLocationDetails = GetCustomerLocation(0);


            objLocationModel.StateList = _IGeneralService.GetState();
            var GroupLst = _IGeneralService.GetGroups();

            objLocationModel.GroupNameList = GroupLst.Where(c => c.otherIntVal == 0).ToList();
            int customerCount = 0;
            objSessionData = (CurrentUserDetail)Session["CurrentUser"];

            if (objSessionData.PageAccessCode == PageAccessCode.ACCNTMGR)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);
            }
            else if (objSessionData.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);
            }
            else if (objSessionData.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);

            }
            else if (objSessionData.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(objSessionData.User_Id, objSessionData.PageAccessCode);

            }
            else
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerService();
                objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(10070);
                objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(0);
            }

           

        
            customerCount = objLocationModel.CustomerNameList.Count();

            ViewBag.CustomerExist = false;
            if (customerCount > 0)
            {
                ViewBag.CustomerExist = true;
            }

            int id = Convert.ToInt16(Request.QueryString["Id"]);
            if (id != 0)
            {
                ViewBag.CustomerLocationBtn = true;
            }
            objLocationModel.PageHeader = "Location management";
            //Session["CustomerLocationBtn"] = true;

            return View("ManageLocation", objLocationModel);
        }
        public ActionResult viewCustomerLocation()
        {


            objLocationModel = new LocationModel();
            // objLocationModel.CustomerLocationDetails = GetCustomerLocation(0);
            objLocationModel.CustomerNameList = _IGeneralService.GetCustomerService();
            int id = Convert.ToInt16(Request.QueryString["Id"]);
            if (id != 0)
            {
                ViewBag.CustomerLocationBtn = true;
            }
            objLocationModel.PageHeader = "Location management";
            //Session["CustomerLocationBtn"] = true;

            return View("viewCustomerLocation", objLocationModel);
        }


        public ActionResult LocationByCustomerId(int id)
        {
            objLocationModel = new LocationModel();
            Session["LocationByCustomerId"] = id;
            objLocationModel.CustomerLocationDetails = GetCustomerLocation(id);
            objLocationModel.CustomerNameList = _IGeneralService.GetCustomerService();
            objLocationModel.PageHeader = "Location management";
            ViewBag.CustomerLocationBtn = true;
            // Session["CustomerLocationBtn"] = true;

            return View("ManageLocation", objLocationModel);
        }
        [AjaxHandleException]
        public ActionResult LocationAjax([DataSourceRequest] DataSourceRequest request, CustomerLocationModel objCustomerLocation)
        {
            int? CustomerId = objCustomerLocation.Customer_ID;
            var currentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
            lstLocations = _ILocationManagement.GetCustomerLocations(objCustomerLocation, currentUserDetail.User_Id, currentUserDetail.PageAccessCode);
            //GetCustomerLocation(CustomerId);
            //ViewBag.GridCount = lstLocations.Count();

            DataSourceResult result = lstLocations.ToDataSourceResult(request);


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public ActionResult LocationResultAjax([DataSourceRequest] DataSourceRequest request)
        {
            var objCustomerLocationResult = new LocationModel();
            if (Session["LocationResult"] != null)
            {
                objCustomerLocationResult = (LocationModel)Session["LocationResult"];

            }
            DataSourceResult result = objCustomerLocationResult.LocationResult.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivateDeactivateLocation(CustomerLocationModel objCustomerLocation)
        {
            if (objCustomerLocation.Location_ID > 0)
            {
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                if (objCurrentUserDetail != null)
                {
                    objCustomerLocation.Modified_by = (Guid)objCurrentUserDetail.User_Id;
                    objCustomerLocation.Modified_Date = Common.CommonMethods.GetCurrentDate();
                }
                bool bResult = false;
                string result = "";
                CustomerLocationModel locationDetails = new CustomerLocationModel();

                //  bool bResult = _ILocationManagement.ActivateDeactivateLocation(objCustomerLocation);
                if (objCustomerLocation.Status == true)
                {
                    string GrpStatus = _IGeneralService.GroupStatusByLocationId(objCustomerLocation.Location_ID);
                    if (GrpStatus == null && GrpStatus == string.Empty)
                    {
                        bResult = _IGeneralService.DeActivateByLocationId(objCustomerLocation.Location_ID, objCurrentUserDetail.User_Id);
                        locationDetails.Status = false;
                    }
                    else
                    {
                        locationDetails.ErrorMsgs = GrpStatus;
                        locationDetails.Successmsg = "";
                        return Json(locationDetails, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = _IGeneralService.ActivateByLocationId(objCustomerLocation.Location_ID, objCurrentUserDetail.User_Id);
                    locationDetails.Status = true;
                }

                if (bResult == true || result == "")
                {
                    SendStatusNotification(objCustomerLocation);
                    // string sStatus = string.Empty;
                    //sStatus = (locationDetails.Status) ? "Activated" : "Deactivated";
                    //locationDetails.Successmsg = "Location [locationName] has been " + sStatus;
                    if (locationDetails.Status)
                    {
                        locationDetails.Successmsg = ReviveMessages.Active;

                    }
                    else
                    {
                        locationDetails.Successmsg = ReviveMessages.Deactivate;
                    }
                    return Json(locationDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //UserModels UserDetails = SendNotification(objUserModel);
                    string sStatus = string.Empty;
                    locationDetails.ErrorMsgs = result;
                    locationDetails.Successmsg = "";
                    return Json(locationDetails, JsonRequestBehavior.AllowGet);
                }

            }
            return View("Location");
        }

        public ActionResult CustomerFileUpload(LocationModel objCustomerLocation, IEnumerable<HttpPostedFileBase> files)
        {
            Session["LocationResult"] = null;
            if (objCustomerLocation.CustomerId > 0 && files != null && files.Count() > 0)
            {

                string sDirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Location\" + objCustomerLocation.CustomerId;
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

                string dirPath1 = @"\Location\" + objCustomerLocation.CustomerId.ToString();

                storeDocument(files, dirPath1);

                foreach (var item in files)
                {
                    objCustomerLocation.FileName = sPath + @"\\" + item.FileName;
                }

                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                objCustomerLocation.Modified_by = objCurrentUserDetail.User_Id;
                objCustomerLocation.Modified_Date = Common.CommonMethods.GetCurrentDate();
                if (objCustomerLocation.PageMode == 0)
                {
                    objCustomerLocation.Created_by = objCurrentUserDetail.User_Id;
                    objCustomerLocation.Created_Date = Common.CommonMethods.GetCurrentDate();
                }

                LocationModel objCustomerLocationResult = new LocationModel();
                bool bResult = _ILocationManagement.CustomerFileUpload(objCustomerLocation, out objCustomerLocationResult);

                ModelState.AddModelError("Name", "test");
                //  return Json(objCustomerLocationResult.LocationResult, JsonRequestBehavior.AllowGet);
                objCustomerLocationResult.CustomerNameList = _IGeneralService.GetCustomerService();

                var GroupLst = _IGeneralService.GetGroups();

                objCustomerLocationResult.GroupNameList = GroupLst.Where(c => c.otherIntVal == 0).ToList();

               

                Session["LocationResult"] = objCustomerLocationResult;
                return View("ManageLocation", objCustomerLocationResult);
            }

            return RedirectToAction("Locations", "ManageCustomers");
        }

        // GET: CustomerLocation/Create
        //Route: Customer/Location/Add
        public ActionResult Add(int? Id, string adrs1, int? stateId, string phoneNo, string locname, string adrs2, int? cityId, string zipCode, string StoreNumber, string openTime, string closeTime)
        {
            var currentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

            LocationModel objLocationModel = new LocationModel();
            objLocationModel.StateList = _IGeneralService.GetState();

            var GroupLst = _IGeneralService.GetGroups();

            objLocationModel.GroupNameList = GroupLst.Where(c => c.otherIntVal == 0).ToList();

        
            if (currentUserDetail.PageAccessCode == PageAccessCode.ACCNTMGR)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUserDetail.User_Id, currentUserDetail.PageAccessCode);
                objLocationModel.SubsidiaryNameList = _IGeneralService.GetEmptyDDL();
                objLocationModel.AgentNameList = _IGeneralService.GetEmptyDDL();
            }
            else if (currentUserDetail.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUserDetail.User_Id, currentUserDetail.PageAccessCode);
                objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId((int)currentUserDetail.Customer_Id, (int)currentUserDetail.SubsidiaryId);
                objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId((int)currentUserDetail.SubsidiaryId);
            }
            else if (currentUserDetail.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUserDetail.User_Id, currentUserDetail.PageAccessCode);
                objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId((int)currentUserDetail.Customer_Id, (int)currentUserDetail.SubsidiaryId);
                objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId((int)currentUserDetail.SubsidiaryId, (int)currentUserDetail.SubAgentId);
            }
            else if (currentUserDetail.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUserDetail.User_Id, currentUserDetail.PageAccessCode);
                objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId((int)currentUserDetail.Customer_Id);
                objLocationModel.AgentNameList = _IGeneralService.GetEmptyDDLWithoutSelect();
            }
            else
            {
                objLocationModel.CustomerNameList = _IGeneralService.GetCustomerService();
                objLocationModel.SubsidiaryNameList = _IGeneralService.GetEmptyDDLWithoutSelect();
                objLocationModel.AgentNameList = _IGeneralService.GetEmptyDDLWithoutSelect();
            }

            objLocationModel.PageHeader = "Add Location";
            objLocationModel.PageButtonSubmit = "Add Location";


            if (currentUserDetail.Customer_Id != null) objLocationModel.CustomerId = (int)currentUserDetail.Customer_Id;
            if (currentUserDetail.SubsidiaryId != null) objLocationModel.SubsidiaryId = (int)currentUserDetail.SubsidiaryId;
            if (currentUserDetail.SubAgentId != null) objLocationModel.SubAgentId = (int)currentUserDetail.SubAgentId;

            if (Id != null)
            {
                objLocationModel.CustomerId = (int)Id;
                objLocationModel = GetCustomFields(objLocationModel);

                objLocationModel.AddressLine1 = adrs1;
                objLocationModel.State = stateId != null ? (int)stateId : 0;
                objLocationModel.PrimaryPhone = phoneNo;
                objLocationModel.LocationName = locname;
                objLocationModel.AddressLine2 = adrs2;
                objLocationModel.City = cityId != null ? (int)cityId : 0;
                objLocationModel.ZipCode = zipCode;
                objLocationModel.StoreNumber = StoreNumber;
                objLocationModel.StoreOpeningTime = openTime;
                objLocationModel.StoreClosingTime = closeTime;
                return Json(objLocationModel, JsonRequestBehavior.AllowGet);

            }
            else
            {
                objLocationModel.CustomField1Valid = true;
                objLocationModel.CustomField2Valid = true;
                objLocationModel.CustomField3Valid = true;
                objLocationModel.CustomField4Valid = true;
                objLocationModel.CustomField5Valid = true;
                objLocationModel.CustomField1Text = "Custom Field 1";
                objLocationModel.CustomField2Text = "Custom Field 2";
                objLocationModel.CustomField3Text = "Custom Field 3";
                objLocationModel.CustomField4Text = "Custom Field 4";
                objLocationModel.CustomField5Text = "Custom Field 5";
                objLocationModel.CustomField1 = "";
                objLocationModel.CustomField2 = "";
                objLocationModel.CustomField3 = "";
                objLocationModel.CustomField4 = "";
                objLocationModel.CustomField5 = "";





            }


            return View("AddEditLocation", objLocationModel);
        }

        /// <summary>
        /// Checks duplicacy of Store Number for a given Customer while adding/editing Customer Location.
        /// </summary>
        /// <param name="storeNo">New Store Number entered by the User</param>
        /// <param name="custID">Customer ID selected by the User</param>
        /// <param name="locID">Location ID (Primary Key) used only while editing Customer Location</param>
        /// <returns></returns>
        public JsonResult CheckDuplicateStoreNo(string StoreNumber, int CustomerId, int LocationId)
        {
            try
            {
                ILocationManagement service = new LocationManagement();
                var data = !service.CheckDuplicateStoreNo(StoreNumber, CustomerId, LocationId);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks duplicacy of Customer Reference Code while adding/editing Customer.
        /// </summary>
        /// <param name="custRefCode">New Customer Reference entered by the User</param>
        /// <param name="custID">Customer ID (Primary Key) used only while editing Customer</param>
        /// <returns></returns>
        public JsonResult CheckDuplicateCustID(string Customer_Ref_Code, String Customer_ID)
        {
            try
            {
                ILocationManagement service = new LocationManagement();
                var data = !service.CheckDuplicateCustID(Customer_Ref_Code, Customer_ID);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(LocationModel objLocationModel)
        {
            Boolean bIsAdded = false;
            Boolean bValid = false;
            try
            {
                if (ModelState.IsValid && objLocationModel.PageMode != 2)
                {
                    ILocationManagement service = new LocationManagement();
                    List<CustomerLocationModel> list = new List<CustomerLocationModel>();
                    objLocationModel.Country = 1; //for USA 

                    var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                    objLocationModel.Modified_by = objCurrentUserDetail.User_Id;
                    objLocationModel.Modified_Date = Common.CommonMethods.GetCurrentDate();
                    if (objLocationModel.PageMode == 0)
                    {
                        objLocationModel.Created_by = objCurrentUserDetail.User_Id;
                        objLocationModel.Created_Date = Common.CommonMethods.GetCurrentDate();
                    }





                    bIsAdded = service.AddEditLocation(objLocationModel);

                    ViewBag.ViewModel = bIsAdded;

                    if (objLocationModel.PageMode == 0)
                    {
                        ViewBag.ViewModelMsg = "Location has been added successfully.";
                        objLocationModel.PageButtonSubmit = "Add Location";
                        objLocationModel.PageHeader = "Add Location";
                        TempData["isCreatedSuccess"] = true;
                        return RedirectToAction("Locations");
                    }
                    else
                    {
                        ViewBag.ViewModelMsg = "Location has been updated successfully.";
                        objLocationModel.PageButtonSubmit = "Save Location";
                        objLocationModel.PageHeader = "Edit Location";
                        TempData["isUpdatedSuccess"] = true;
                        return RedirectToAction("Locations");
                    }

                    if (!bIsAdded)
                    {
                        ViewBag.ViewModelMsg = "Error in Location processing.";

                        objLocationModel.StateList = _IGeneralService.GetState();
                        objLocationModel.CustomerNameList = _IGeneralService.GetCustomerService();
                        objLocationModel.CityList = _IGeneralService.GetCity(objLocationModel.State);
                        ViewBag.Citylst = objLocationModel.CityList;

                        objLocationModel = GetCustomFields(objLocationModel);

                        return View("AddEditLocation", objLocationModel);
                    }
                }
                else
                {
                    return RedirectToAction("Locations");
                }


            }
            catch (Exception ex)
            {
                throw;

            }
        }


        public ActionResult View(int Id)
        {
            var objLocationModel = GetCustomerLocationDetail(Id, true, "View Location", "Back");

            var GroupLst = _IGeneralService.GetGroups();
            objLocationModel.GroupNameList = GroupLst.Where(c => c.otherIntVal == 0).ToList();

           
            objLocationModel = GetCustomFields(objLocationModel);
            ViewBag.RecordId = objLocationModel.CustomerId;

            if (objLocationModel.Created_Date != null)
            {

                objLocationModel.Created_Date.Value.ToString("MM/dd/yyyy");
                //objLocationModel.DateCreated = objLocationModel.Created_Date;

            }

            return View("AddEditLocation", objLocationModel);
        }


        // GET: Location/Edit/5
        public ActionResult Edit(int Id, int CustId = 0)
        {
            var objLocationModel = new LocationModel();
            if (Id > 0)
            {
                objLocationModel = GetCustomerLocationDetail(Id, false, "Edit Location", "Save Location");
            }

            var GroupLst = _IGeneralService.GetGroups();

            objLocationModel.GroupNameList = GroupLst;//GroupLst.Where(c => c.otherIntVal == 0).ToList();


           
            //shipping Status Id
            var objLocation = _ILocationManagement.GetCustomerLocation(Id);
            objLocationModel.ShippingStatusId = objLocation.ShippingStatusId;

            objSessionData = (CurrentUserDetail)Session["CurrentUser"];
            if (CustId > 0)
            {
                //
                if (objLocationModel.CustomerId == CustId)
                {
                    if (objSessionData.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId, objLocationModel.SubsidiaryId);
                        objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryId);
                    }
                    else if (objSessionData.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId, objLocationModel.SubsidiaryId);
                        objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryId, objLocationModel.SubAgentId);
                    }
                    else if (objSessionData.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId);
                        objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryId);
                    }
                    else
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId);
                        objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryId).ToList();
                    }
                }
                else
                {
                    if (objSessionData.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId, objLocationModel.SubsidiaryId);
                        objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryId);
                    }
                    else if (objSessionData.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId, objLocationModel.SubsidiaryId);
                        objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryId, objLocationModel.SubAgentId);
                    }
                    else if (objSessionData.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId);
                        objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryNameList.FirstOrDefault().Id);
                    }
                    else
                    {
                        objLocationModel.SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(CustId);
                        if (objLocationModel.SubsidiaryNameList.Count() > 0)
                        {
                            objLocationModel.AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocationModel.SubsidiaryNameList.FirstOrDefault().Id);
                        }
                    }

                }
                objLocationModel.CustomerId = CustId;
                objLocationModel.CustomField1 = "";
                objLocationModel.CustomField2 = "";
                objLocationModel.CustomField3 = "";
                objLocationModel.CustomField4 = "";
                objLocationModel.CustomField5 = "";
            }

            objLocationModel = GetCustomFields(objLocationModel);

            return View("AddEditLocation", objLocationModel);
        }

        public ActionResult DeleteLocation(int LocationID)
        {
            try
            {
                if (LocationID > 0)
                {
                    string result = _IGeneralService.DeleteLocationByLocId(LocationID);

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View("Location");
        }
        #endregion

        #region Private Methods

        private LocationModel GetCustomerLocationDetail(int nLocationId, bool bReadOnly, string sPageHeader, string sPageButtonSubmit)
        {
            ILocationManagement service = new LocationManagement();
            var currentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

            var objLocation = service.GetCustomerLocation(nLocationId);
            var nCity = objLocation.City != null ? (int)objLocation.City : 0;
            var nState = objLocation.State != null ? (int)objLocation.State : 0;

            var objLocationModel = new LocationModel();
            if (currentUserDetail.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
            {
                objLocationModel = new LocationModel
               {
                   CityList = _IGeneralService.GetCity(nState),
                   StateList = _IGeneralService.GetState(),
                   CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUserDetail.User_Id, currentUserDetail.PageAccessCode),
                   SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId((int)currentUserDetail.Customer_Id, (int)currentUserDetail.SubsidiaryId),
                   AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId((int)currentUserDetail.SubsidiaryId)
               };
            }
            else if (currentUserDetail.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
            {
                objLocationModel = new LocationModel
                {
                    CityList = _IGeneralService.GetCity(nState),
                    StateList = _IGeneralService.GetState(),
                    CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUserDetail.User_Id, currentUserDetail.PageAccessCode),
                    SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId((int)currentUserDetail.Customer_Id, (int)currentUserDetail.SubsidiaryId),
                    AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId((int)currentUserDetail.SubsidiaryId, (int)currentUserDetail.SubAgentId)
                };
            }
            else if (currentUserDetail.PageAccessCode == PageAccessCode.CUSTOMERADMIN)
            {
                objLocationModel = new LocationModel
                {
                    CityList = _IGeneralService.GetCity(nState),
                    StateList = _IGeneralService.GetState(),
                    CustomerNameList = _IGeneralService.GetCustomerStoreUser(currentUserDetail.User_Id, currentUserDetail.PageAccessCode),
                    SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(objLocation.Customer_ID),
                    AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocation.SubsidiaryId)
                };
            }
            else
            {
                objLocationModel = new LocationModel
               {
                   CityList = _IGeneralService.GetCity(nState),
                   StateList = _IGeneralService.GetState(),
                   CustomerNameList = _IGeneralService.GetCustomerService(),
                   SubsidiaryNameList = _IGeneralService.GetSubsidiaryByCustomerId(objLocation.Customer_ID),
                   AgentNameList = _IGeneralService.GetAgentsBySubsidiaryId(objLocation.SubsidiaryId)
               };

            }


            ViewBag.Citylst = objLocationModel.CityList;

            objLocationModel.PageHeader = sPageHeader;
            objLocationModel.PageButtonSubmit = sPageButtonSubmit;
            objLocationModel.PageMode = bReadOnly ? 2 : 1;
            objLocationModel.LocationId = objLocation.Location_ID;
            objLocationModel.StoreNumber = objLocation.StoreNumber;
            objLocationModel.LocationName = objLocation.Location_Name;
            objLocationModel.AddressLine1 = objLocation.Address;
            objLocationModel.AddressLine2 = objLocation.Additional_Address_info;
            objLocationModel.CustomerId = objLocation.Customer_ID;
            objLocationModel.City = nCity;
            objLocationModel.State = nState;
            objLocationModel.ZipCode = objLocation.ZipCode;
            objLocationModel.PrimaryPhone = objLocation.Phone;
            objLocationModel.StoreOpeningTime = objLocation.SToreOpentime;
            objLocationModel.StoreClosingTime = objLocation.SToreClosetime;
            objLocationModel.CustomField1 = objLocation.Customfield1;
            objLocationModel.CustomField2 = objLocation.Customfield2;
            objLocationModel.CustomField3 = objLocation.Customfield3;
            objLocationModel.CustomField4 = objLocation.Customfield4;
            objLocationModel.CustomField5 = objLocation.Customfield5;

            objLocationModel.Created_Date = objLocation.Created_Date;
            objLocationModel.Modified_Date = objLocation.Modified_Date;
            objLocationModel.Created_by = objLocation.Created_by;
            objLocationModel.Modified_by = objLocation.Modified_by;
            objLocationModel.Status = objLocation.Status;
            objLocationModel.CityName = objLocation.CityName;
            objLocationModel.GroupId = objLocation.GroupId;


            objLocationModel.SubsidiaryId = objLocation.SubsidiaryId;
            objLocationModel.SubAgentId = objLocation.SubAgentId;

            if (bReadOnly)
            {
                objLocationModel.DateCreated = String.Format("{0:MM/dd/yyyy}", objLocation.Created_Date);
                objLocationModel.DateModified = String.Format("{0:MM/dd/yyyy}", objLocation.Modified_Date);
                objLocationModel.CreatedBy = GetCustomerName(objLocation.Created_by);
                objLocationModel.ModifiedBy = GetCustomerName(objLocation.Modified_by);
            }

            return objLocationModel;
        }

        private string GetCustomerName(Guid? UserId)
        {
            IUserManagmentService objUserMgr = new UserManagementService();
            string UserName = string.Empty;

            if (UserId != null)
            {
                var CustomerDetail = objUserMgr.GetUserById((Guid)UserId);
                UserName = CustomerDetail.FirstName + " " + CustomerDetail.LastName;
            }

            return UserName;
        }

        private string GetCustomerName(int CustomerId)
        {
            ICustomerManagement objCustMgr = new CustomerManagement();
            string CustomerName = string.Empty;

            var CustomerDetail = objCustMgr.GetCustomerDetailsById(CustomerId);

            CustomerName = CustomerDetail.Customer_Name;

            return CustomerName;
        }

        private List<CustomerLocationModel> GetCustomerLocation(int? CustomerId)
        {
            objLocationModel = new LocationModel();
            CurrentUserDetail objCurrentUserDetail = new CurrentUserDetail();
            objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];

            if ((int)CustomerId > 0)
            {
                lstLocations = _ILocationManagement.GetCustomerLocationsById((int)CustomerId);
            }
            else
            {

                if (objCurrentUserDetail.PageAccessCode == PageAccessCode.ACCNTMGR)
                {
                    lstLocations = _ILocationManagement.GetCustomerLocationsById(0, objCurrentUserDetail.User_Id); // For Accout Manager
                }
                else if (objCurrentUserDetail.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                {
                    lstLocations = _ILocationManagement.GetCustomerLocationsBySubsidiaryAdminUser(objCurrentUserDetail.User_Id);//for Subsidiary Admin
                }
                else if (objCurrentUserDetail.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                {
                    lstLocations = _ILocationManagement.GetCustomerLocationsBySubAgentAdminUser(objCurrentUserDetail.User_Id);//for SubAgent Admin
                }
                else if (objCurrentUserDetail.Customer_Id != null && objCurrentUserDetail.Customer_Id > 0)
                {
                    lstLocations = _ILocationManagement.GetCustomerLocationsById((int)objCurrentUserDetail.Customer_Id); // Customer ADMIN
                }
                else
                {
                    lstLocations = _ILocationManagement.GetCustomerLocationsById(0);// FOr Other user other than Acctmgr & customer ADMIn
                }
            }


            return lstLocations;
        }


        /// <summary>
        /// 	The system will send a notification to the client execinforming them of customer location addition, modification or deletion.
        /// </summary>
        /// <param name="MachineModel"></param>
        private void SendStatusNotification(CustomerLocationModel objCustomerLocation)
        {

            string sEmailTo = Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);   // TO CLIENTEXEC       
            string ccMailId = Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, objCustomerLocation.Customer_ID);   // CC Account Managers       
            objNotification = new NotificationModel();
            if (!string.IsNullOrEmpty(sEmailTo))
            {
                EmailHelper objEmailHelper = new EmailHelper();
                string sStatus = string.Empty;

                sStatus = (!objCustomerLocation.Status) ? "Activated" : "Deactivated";
                var objKeyWords = new Hashtable();
                objKeyWords.Add("LOCATION", objCustomerLocation.Location_Name);
                objKeyWords.Add("STATUS", sStatus);
                objKeyWords.Add("CUSTOMER", objCustomerLocation.CustomerName);
                var objParser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/Location/LocationStatus.html"), objKeyWords);
                var sEmailbody = objParser.Parse();
                objNotification.body_mail = sEmailbody;
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                objNotification.Created_by = objCurrentUserDetail.User_Id;
                objNotification.NotificationMessages = "Location Status Notification";
                objNotification.Notification_Date = DateTime.Now;
                objNotification.Mail_Ids = sEmailTo;
                if (ccMailId != null)
                {
                    objNotification.Mail_Ids = sEmailTo + ccMailId;
                }
                _IGeneralService.StoreNotification(objNotification);

                objEmailHelper.Send(sEmailTo, ccMailId, string.Empty, "Redux - Location Status Notification", sEmailbody);
            }
        }

        private LocationModel GetCustomFields(LocationModel objLocationModel)
        {
            int nCount = 0;
            List<FormModel> lstFormModel = _IFormManagementService.GetCustomerCustomFieldsById(objLocationModel.CustomerId);

            foreach (var Form in lstFormModel)
            {
                if (nCount == 0)
                {
                    objLocationModel.CustomField1Text = Form.CustomFieldName;
                    objLocationModel.CustomField1Valid = true;
                }
                else if (nCount == 1)
                {
                    objLocationModel.CustomField2Text = Form.CustomFieldName;
                    objLocationModel.CustomField2Valid = true;
                }
                else if (nCount == 2)
                {
                    objLocationModel.CustomField3Text = Form.CustomFieldName;
                    objLocationModel.CustomField3Valid = true;
                }
                else if (nCount == 3)
                {
                    objLocationModel.CustomField4Text = Form.CustomFieldName;
                    objLocationModel.CustomField4Valid = true;
                }
                else if (nCount == 4)
                {
                    objLocationModel.CustomField5Text = Form.CustomFieldName;
                    objLocationModel.CustomField5Valid = true;
                }
                nCount++;
            }
            return objLocationModel;
        }

        #endregion

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
        [HttpPost]
        public JsonResult GetAgentBySubsidiary(int SubsidiaryId)
        {
            try
            {
                objSessionData = (CurrentUserDetail)Session["CurrentUser"];
                if (PageAccessCode.SUBAGENTADMIN == objSessionData.PageAccessCode)
                {
                    var ListAgent = _IGeneralService.GetAgentsBySubsidiaryId(SubsidiaryId, (int)objSessionData.SubAgentId);
                    return Json(ListAgent, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ListAgent = _IGeneralService.GetAgentsBySubsidiaryId(SubsidiaryId);
                    return Json(ListAgent, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost]
        public ActionResult UpdLocationStatus(List<LocationList> LocationList, int LocationStatusId, int GroupId)
        {
            //update Location Status
            IManageShippingService ShippingService = new ManageShippingService();
            var SessionData = (CurrentUserDetail)Session["CurrentUser"];

            ShippingService.UpdShippingStatus(LocationList, LocationStatusId, GroupId, SessionData.User_Id);
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}