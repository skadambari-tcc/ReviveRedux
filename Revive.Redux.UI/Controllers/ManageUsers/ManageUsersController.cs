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
using System.IO;
using System.Text;
namespace Revive.Redux.UI.Controllers.ManageUsers
{
    [Authorize]
    [ReviveAuth]
    [ReviveHandleErrorAttribute]
    public class ManageUsersController : Controller
    {
        // GET: ManageUsersprivate IGeneralService _IGeneralService = null;
        private IUserManagmentService _ManageUserService = null;
        private IGeneralService _generalService = null;
        private ILogService logService = null;
        NotificationModel objNotification = null;
        private IManageShippingService manageShippingService = null;



        public ManageUsersController()
        {
            try
            {

                _ManageUserService = new UserManagementService();
                _generalService = new GeneralService();
                logService = new LogService();
                manageShippingService = new ManageShippingService();
                //ViewBag.CustomerLst = _generalService.GetCustomerService(true);
                ViewBag.UserTypeLst = _generalService.GetUserLevelType("ALL");
                //ViewBag.StoreUserTypeLst = _generalService.GetStoreUserLevelType();
                //ViewBag.Locationlst = _generalService.GetEmptyDDL();
                //ViewBag.SubsidiaryLst = _generalService.GetEmptyDDL();
                ViewBag.AgentLst = _generalService.GetEmptyDDLWithoutSelect();
                ViewBag.CustomerLst = bindCustomer();

            }
            catch (Exception ex)
            {
                string s = ex.Message.ToString();
            }
        }
        public ActionResult ManageUser()
        {
            ViewBag.CustomerLst = bindCustomer();
            return View();

        }
        /// <summary>
        /// CR18
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagePassword()
        {
            UserModels objMemberList = new UserModels();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            objMemberList.CustomerNameList = _generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
            objMemberList.SubsidiaryList = _generalService.GetEmptyDDLWithoutSelect();
            objMemberList.SubAgentList = _generalService.GetEmptyDDLWithoutSelect();

            return View(objMemberList);
        }

        /// <summary>
        /// View Loging CR 17
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult ViewLoging()
        {
            UserModels objMemberList = new UserModels();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            objMemberList.CustomerNameList = _generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();
            objMemberList.LocationList = _generalService.GetEmptyDDLWithoutSelect();
            objMemberList.SubsidiaryList = _generalService.GetEmptyDDLWithoutSelect();
            objMemberList.SubAgentList = _generalService.GetEmptyDDLWithoutSelect();
            return View(objMemberList);

        }
        public ActionResult ConfigureLogging(Guid UserId, string Fullname, bool IsUserLogging, int? LogTypeId)
        {
            //int CustomerId = 0;
            //int UserTypeId = 0;
            IEnumerable<DtoList> Types = null;
            //List<DtoList> objLoggingTypes = new List<DtoList>();
            Types = _generalService.GetLoggingTypes();
            ViewBag.StoreUsers = Types;
            ViewBag.StoreUsersCount = Types.Count();
            //DtoUser user = new DtoUser();
            //  user = _ManageUserService.GetUserDetailsByUserId(UserId);
            UserModels UserRecord = new UserModels();
            UserRecord.UserId = UserId;
            UserRecord.FirstName = Fullname;
            UserRecord.Status = IsUserLogging;
            UserRecord.IsStoreUserLogging = IsUserLogging;
            UserRecord.LoggTypeId = Convert.ToInt16(LogTypeId);
            // UserRecord.LoggTypeId=ue
            return View(UserRecord);
        }

        /// <summary>
        /// GetUserLogValue by userId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetUserLogValue(Guid UserId)
        {
            DtoUser user = new DtoUser();
            user = _ManageUserService.GetUserDetailsByUserId(UserId);
            return Json(user.IsUserLogging, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get StoreUser Count BY Location
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetStoreUserByLocation(Guid UserId)
        {
            int count = _ManageUserService.GetStoreUserCountbyLocation(UserId);
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Log Store User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult LogStoreUserByLocation(Guid UserId, int count, bool Status, int LogType)
        {
            bool value = _ManageUserService.LogStoreUserbyLocation(UserId, count, Status, LogType);
            DtoUser user = new DtoUser();
            user = _ManageUserService.GetUserDetailsByUserId(UserId);
            user.Count = count;
            return Json(user, JsonRequestBehavior.AllowGet);
        }
        [AjaxHandleException]
        public ActionResult ManageUsersAjax([DataSourceRequest] DataSourceRequest request, string UserLevel_Id)
        {
            int UserTypeId = 0;
            if (UserLevel_Id != "")
            {
                UserTypeId = Convert.ToInt32(UserLevel_Id);
            }
            IEnumerable<DtoUser> objUser = _ManageUserService.GetUserDetails(UserTypeId);
            List<DtoUser> UserDetails = new List<DtoUser>();
            UserDetails = ConvertUserDetails(objUser);
            DataSourceResult result = UserDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public List<DtoUser> ConvertUserDetails(IEnumerable<DtoUser> obj)
        {
            List<DtoUser> objUser = new List<DtoUser>();
            foreach (var item in obj)
            {
                DtoUser User = new DtoUser();
                User.UserName = item.UserName;
                User.Email_ID = item.Email_ID;
                User.Cell_Phone = item.Cell_Phone;
                User.Customer_ID = item.Customer_ID;
                User.User_Level_Name = item.User_Level_Name;
                User.FirstName = item.FirstName;
                User.LastName = item.LastName;
                User.FullName = item.FirstName + " " + item.LastName;
                User.User_ID = item.User_ID;
                User.Location_ID = item.Location_ID;
                User.Location_Name = item.Location_Name;
                User.status = item.status;
                User.User_ID = item.User_ID;
                User.Deactivate_Flag = item.Deactivate_Flag;
                User.UserType = item.UserType;
                User.IsStoreUserLog = item.IsUserLogging;
                User.LogTypeId = item.LogTypeId;
                User.SubAgentName = item.SubAgentName;
                User.CustomerName = item.CustomerName;
                if (item.IsUserLogging == true)
                    User.UserLogging = "Yes";
                else
                {
                    User.UserLogging = "No";
                }
                objUser.Add(User);
            }
            return objUser;
        }
        [AjaxHandleException]
        public JsonResult GetUserTypeList()
        {
            var lst = _generalService.GetUserType();
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCustomerList()
        {
            var CustomerLst = _generalService.GetCustomerService();
            return Json(CustomerLst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLocationList(int Customer_Id)
        {
            var LocationLst = _generalService.GetCustomerLocationService(Customer_Id);
            return Json(LocationLst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetUserType(int User_Level_Id)
        {
            var LocationLst = _generalService.GetUserTypeByUserLevelId(User_Level_Id);

            return Json(LocationLst, JsonRequestBehavior.AllowGet);

        }
        public DtoList GetUserTypeName(int User_Level_Id)
        {
            var UserTypeTable = _generalService.GetUserTypeByUserLevelId(User_Level_Id);

            return UserTypeTable.FirstOrDefault();//UserTypeTable.Select(x => x.Text).FirstOrDefault();

        }


        private List<DtoList> bindCustomer()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var objcustomerlst = new List<DtoList>();

            objcustomerlst = _generalService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode).ToList();

            if (objcustomerlst.Count == 0)
            {
                objcustomerlst = _generalService.GetEmptyDDL().ToList();
            }



            return objcustomerlst;

        }
        public ActionResult AddEditUsers(Guid? uid)
        {
            var model = new UserModels();
            Session["UserUid"] = null;
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.CustomerLst = bindCustomer(); // Bind customer
            ViewBag.ManufacturerLst = _generalService.GetManufacturers();
            model.PageAccessCode = currentUser.PageAccessCode;
            if (uid == null)
            {
                uid = new Guid();
            }
            try
            {
                ViewBag.Title = "Add New User";

                if (uid != new Guid())
                {
                    Session["UserUid"] = uid;
                    model = _ManageUserService.GetUserById(uid);
                    model.UserId = (System.Guid)uid;
                    var UserLevelInfo = GetUserTypeName(model.User_Level_Id);
                    model.UserType = UserLevelInfo.Text;
                    ViewBag.isUpdateMode = true;
                    if (model.Location_Id == 0)
                    {
                        model.Location_Id = -1;
                    }
                    string cust_Id = model.Customer_Id.ToString();
                    if (cust_Id != "")
                    {
                        ViewBag.LocationLst = _generalService.GetCustomerLocationService(Convert.ToInt32(cust_Id));
                        ViewBag.SubsidiaryLst = _generalService.GetSubsidiaryByCustomerId(Convert.ToInt32(cust_Id));
                        ViewBag.AgentLst = _generalService.GetAgentsBySubsidiaryId(model.SubsidiaryId);
                    }
                    else
                    {
                        ViewBag.LocationLst = _generalService.GetEmptyDDLWithoutSelect();
                        ViewBag.SubsidiaryLst = _generalService.GetEmptyDDLWithoutSelect();
                        ViewBag.AgentLst = _generalService.GetEmptyDDLWithoutSelect();
                    }
                }
                else
                {
                    model.Status = true;
                    ViewBag.LocationLst = _generalService.GetEmptyDDLWithoutSelect();
                    ViewBag.SubsidiaryLst = _generalService.GetEmptyDDLWithoutSelect();
                    ViewBag.AgentLst = _generalService.GetEmptyDDLWithoutSelect();
                }
            }
            catch (Exception ex)
            {
                //  logService.LogError("Error in Add Edit User Page Loading...", ex);
                throw;
            }
            return View(model);




        }

        public ActionResult ActivateDeactivateUser(UserModels objUserModel)
        {
            if (objUserModel.UserId != null)
            {
                //Session["CustomerID"] = CustomerRecord.Customer_ID;

                string result = _ManageUserService.ActivateDeactivateUserByGuid(objUserModel);
                if (result == "")
                {
                    UserModels UserDetails = SendNotification(objUserModel);
                    string sStatus = string.Empty;



                    // Old Code Commented by KD as Amit Changes for standardise
                    //sStatus = (UserDetails.Status) ? "Activated" : "Deactivated";
                    // UserDetails.Successmsg = "User " + UserDetails.FirstName + " " + UserDetails.LastName + " has been " + sStatus;

                    sStatus = (UserDetails.Status) ? ReviveMessages.Active : ReviveMessages.Deactivate;
                    UserDetails.Successmsg = sStatus;

                    return Json(UserDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //UserModels UserDetails = SendNotification(objUserModel);
                    string sStatus = string.Empty;
                    objUserModel.ErrorMsgs = result;
                    objUserModel.Successmsg = "";
                    return Json(objUserModel, JsonRequestBehavior.AllowGet);
                }
                //model.Created_Date = DateTime.Now;
                ViewBag.isUpdateMode = true;
            }
            return View("ManageUser");
        }
        private UserModels SendNotification(UserModels objUser)
        {

            string sEmailTo = Helper.GetEmailAddressByGroup(PageAccessCode.ADMIN);   // To all active Admins
            UserModels UserDetails = _ManageUserService.GetUserById(objUser.UserId);
            objNotification = new NotificationModel();

            if (!string.IsNullOrEmpty(sEmailTo))
            {
                EmailHelper objEmailHelper = new EmailHelper();
                string sStatus = string.Empty;

                sStatus = (UserDetails.Status) ? "Activated" : "Deactivated";

                var objKeyWords = new Hashtable();
                objKeyWords.Add("User", UserDetails.FirstName + " " + Convert.ToString(UserDetails.LastName));
                objKeyWords.Add("STATUS", sStatus);

                string pageAccessCode = UserDetails.PageAccessCode;
                if ((pageAccessCode == PageAccessCode.MFPC || pageAccessCode == PageAccessCode.MFASSEMBLY || pageAccessCode == PageAccessCode.MFSHIP)
                    && UserDetails.Manufacturer_Id != null && UserDetails.Manufacturer_Id > 0)
                {
                    // Manufacturer
                    IManufacturersService serv = new ManufacturersService();
                    objKeyWords.Add("Department", "from " + serv.GetMFById((int)UserDetails.Manufacturer_Id).Manufacturer_Name);
                }
                else if ((pageAccessCode == PageAccessCode.CUSTOMERADMIN || pageAccessCode == PageAccessCode.STOREUSER) &&
                    UserDetails.Customer_Id != null && UserDetails.Customer_Id > 0)
                {
                    // Customer
                    ICustomerManagement objCust = new CustomerManagement();
                    objKeyWords.Add("Department", "from " + objCust.GetCustomerDetailsById((int)UserDetails.Customer_Id).Customer_Name);
                }
                else
                {
                    objKeyWords.Add("Department", string.Empty);
                }
                var objParser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ManageUser/UserStatus.html"), objKeyWords);
                var sEmailbody = objParser.Parse();
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                objNotification.Created_by = objCurrentUserDetail.User_Id;
                objNotification.body_mail = sEmailbody;
                objNotification.NotificationMessages = "User Status Notification";
                objNotification.Notification_Date = DateTime.Now;
                objNotification.Mail_Ids = sEmailTo;
                _generalService.StoreNotification(objNotification);
                objEmailHelper.Send(sEmailTo, string.Empty, string.Empty, "Redux - User Status Notification", sEmailbody);
            }
            return UserDetails;
        }
        public ActionResult RegisterUser(UserModels model)
        {

            var result = false;
            if (Session["UserUid"] != null)
            {
                model.UserId = (Guid)Session["UserUid"];
            }

            var isNew = model.UserId != new Guid() ? false : true;
            var passwordText = "";
            if (model.UserId != new Guid()) // in case of Edit user 
            {
                ModelState.Remove("Email"); //
            }
            else
            {
                ModelState.Remove("emailEdit"); //
                ModelState.Remove("UserId");
            }

            var UserLevelInfo = GetUserTypeName(model.User_Level_Id);
            string UserTypeName = UserLevelInfo.Text;
            string PageAccessCodeval = UserLevelInfo.otherStrVal;

            if (UserTypeName != "Customer")
            {
                ModelState.Remove("Customer_Id");
                ModelState.Remove("Location_Id");
                ModelState.Remove("SubsidiaryId");
                ModelState.Remove("SubAgentId");
            }
            if (PageAccessCodeval == PageAccessCode.CUSTOMERADMIN)
            {
                ModelState.Remove("Location_Id");
                ModelState.Remove("SubsidiaryId");
                ModelState.Remove("SubAgentId");
            }
            if (PageAccessCodeval == PageAccessCode.SUBSIDIARYADMIN)
            {
                ModelState.Remove("Location_Id");
                ModelState.Remove("SubAgentId");
            }
            if (PageAccessCodeval == PageAccessCode.SUBAGENTADMIN)
            {
                ModelState.Remove("Location_Id");
            }
            if (UserTypeName.ToLower() != "Manufacturer".ToLower())
            {
                ModelState.Remove("Manufacturer_Id");
            }



            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["CurrentUser"] != null)
                    {
                        var currentUser = (CurrentUserDetail)Session["CurrentUser"];

                        if (isNew == false)
                        {
                            model.Modified_by = currentUser.User_Id;
                        }
                        else
                        {
                            model.Status = true;
                            model.Created_by = currentUser.User_Id;
                            passwordText = CommonMethods.GetRandomPassword();
                            model.Pasword = passwordText.ToMD5HashForPassword();

                        }
                        result = _ManageUserService.AddEditUser(model);
                        // Send Mail;
                        if (result == true && isNew == true)
                        {
                            var templateVars = new Hashtable();
                            templateVars.Add("LoginURL", ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString());
                            templateVars.Add("Password", passwordText);
                            templateVars.Add("UserName", model.Email);
                            templateVars.Add("UserFullName", model.FirstName);
                            var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/RegisterUser.html"), templateVars);
                            var emailBody = parser.Parse();
                            EmailHelper emailHelper = new EmailHelper();
                            emailHelper.Send(model.Email, string.Empty, string.Empty, "Redux - Password Notification", emailBody);
                            TempData["isCreatedSuccess"] = true;

                        }
                        else
                        {
                            TempData["isUpdatedSuccess"] = true;
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                //logService.LogError("Error in Add Edit User", ex);
                throw;
            }
            return RedirectToAction("ManageUser", "ManageUsers");

        }

        #region StoreUserManagement

        public ActionResult ViewUser()
        {
            var objCurrentUserLogedIn = (CurrentUserDetail)Session["CurrentUser"];
            IEnumerable<DtoList> objCustomerRecord = _generalService.GetCustomerStoreUser(objCurrentUserLogedIn.User_Id, objCurrentUserLogedIn.PageAccessCode);
            ViewBag.StoreUserCustomer = objCustomerRecord;
            ViewBag.StoreUserCustomerCount = objCustomerRecord.Count();




            if (objCustomerRecord.Count() > 0)
            {
                Session["StoreUserCustomerCount"] = objCustomerRecord.Count();
            }
            else
            {
                Session["StoreUserCustomerCount"] = 0;
            }

            return View();
        }
        //public ActionResult AddEditStoreUser()
        //{
        //    ViewBag.CustomerLst = bindCustomer(); // Bind customer
        //    return View();
        //}
        public ActionResult ActivateDeactivateStoreUser(UserModels objUserModel)
        {
            if (objUserModel.UserId != null)
            {
                //Session["CustomerID"] = CustomerRecord.Customer_ID;

                string result = _ManageUserService.ActivateDeactivateUserByGuid(objUserModel);
                if (result == "")
                {

                    // UserModels UserDetails = _ManageUserService.GetUserById(objUserModel.UserId);
                    UserModels UserDetails = SendNotification(objUserModel);
                    // string sStatus = string.Empty;
                    //sStatus = (UserDetails.Status) ? "Activated" : "Deactivated";
                    if (UserDetails.Status == true)
                    {
                        UserDetails.Successmsg = ReviveMessages.Active;
                    }
                    else
                    {
                        UserDetails.Successmsg = ReviveMessages.Deactivate;
                    }

                    // UserDetails.Successmsg = "Store User " + UserDetails.FirstName + " " + UserDetails.LastName + " has been " + sStatus;
                    return Json(UserDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //UserModels UserDetails = SendNotification(objUserModel);
                    string sStatus = string.Empty;
                    objUserModel.ErrorMsgs = result;
                    objUserModel.Successmsg = "";
                    return Json(objUserModel, JsonRequestBehavior.AllowGet);
                }
                //model.Created_Date = DateTime.Now;
                ViewBag.isUpdateMode = true;
            }
            return View("ManageUser");
        }
        /// <summary>
        /// Get All Store User And bind data to grid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="UserLevel_Id"></param>
        /// <param name="Customer_ID"></param>
        /// <returns></returns>
        [AjaxHandleException]
        public ActionResult ManageStoreUsersAjax([DataSourceRequest] DataSourceRequest request, string UserLevel_Id, string Customer_ID)
        {
            int CustomerId = 0;
            int UserTypeId = 0;
            List<DtoUser> StoreUserDetails = new List<DtoUser>();
            if (Customer_ID != "" && UserLevel_Id != "")
            {
                CustomerId = Convert.ToInt32(Customer_ID);
                UserTypeId = Convert.ToInt32(UserLevel_Id);
            }

            IEnumerable<DtoUser> objUser = null;
            int nStoreUserCustomerCount = (int)Session["StoreUserCustomerCount"];

            if (nStoreUserCustomerCount > 0) // If Customer Exists 
            {
                objUser = _ManageUserService.GetStoreUserDetails(UserTypeId, CustomerId);
            }
            else
            {
                objUser = null;
            }
            if (objUser != null)
            {
                StoreUserDetails = ConvertUserDetails(objUser);
            }


            DataSourceResult result = StoreUserDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [AjaxHandleException]
        public ActionResult AllStoreUsers([DataSourceRequest] DataSourceRequest request, UserModels objUserModel)
        {
            var objCurrentUserLogedIn = (CurrentUserDetail)Session["CurrentUser"];
            if (PageAccessCode.SUBAGENTADMIN == objCurrentUserLogedIn.PageAccessCode)
            {
                objUserModel.SubAgentId = (int)objCurrentUserLogedIn.SubAgentId;
            }
            else if (PageAccessCode.SUBSIDIARYADMIN == objCurrentUserLogedIn.PageAccessCode)
            {
                objUserModel.SubsidiaryId = (int)objCurrentUserLogedIn.SubsidiaryId;
            }

            IEnumerable<DtoUser> objUser = _ManageUserService.GetStoreUserDetails(objCurrentUserLogedIn.UserLevelId, objUserModel);
            DataSourceResult result = objUser.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Get All Store User And bind data to grid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="UserLevel_Id"></param>
        /// <param name="Customer_ID"></param>
        /// <returns></returns>
        [AjaxHandleException]
        public ActionResult ManageUsersLoggingAjax([DataSourceRequest] DataSourceRequest request, string UserLevel_Id, UserModels objMemberParameter)
        {
            //int CustomerId = 0;
            int UserTypeId = 0;
            List<DtoUser> StoreUserDetails = new List<DtoUser>();
            if (UserLevel_Id != "")
            {
                UserTypeId = Convert.ToInt32(UserLevel_Id);
            }
            if (objMemberParameter.Customer_Id == null)
            {
                objMemberParameter.Customer_Id = 0;
            }
            if (objMemberParameter.Location_Id == null)
            {
                objMemberParameter.Location_Id = 0;
            }

            IEnumerable<DtoUser> objUser = null;
            //int nStoreUserCustomerCount = (int)Session["StoreUserCustomerCount"];


            objUser = _ManageUserService.GetStoreUserDetails(UserTypeId, objMemberParameter);



            StoreUserDetails = ConvertUserDetails(objUser);



            DataSourceResult result = StoreUserDetails.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Add New Store User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RegisterStoreUser(UserModels model)
        {

            var result = false;
            if (Session["UserUid"] != null)
            {
                model.UserId = (Guid)Session["UserUid"];
            }

            model.User_Level_Id = _generalService.GetUserLevelId(PageAccessCode.STOREUSER);
            if (model.User_Level_Id != 0)
            {
                var isNew = model.UserId != new Guid() ? false : true;
                var passwordText = "";
                ModelState.Remove("Manufacturer_Id");
                if (model.UserId != new Guid()) // in case of Edit user
                {
                    ModelState.Remove("Email");
                    ModelState.Remove("Status");

                }
                else
                {
                    ModelState.Remove("emailEdit");
                    ModelState.Remove("UserId");
                    ModelState.Remove("Status");
                }
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (Session["CurrentUser"] != null)
                        {
                            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

                            if (isNew == false)
                            {
                                model.Modified_by = currentUser.User_Id;
                            }
                            else
                            {
                                model.Status = true;
                                model.Created_by = currentUser.User_Id;
                                passwordText = CommonMethods.GetRandomPassword();
                                model.Pasword = passwordText.ToMD5HashForPassword();

                            }
                            result = _ManageUserService.AddEditUser(model);
                            // Send Mail;
                            if (result == true && isNew == true)
                            {
                                //var templateVars = new Hashtable();
                                //templateVars.Add("LoginURL", ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString());
                                //templateVars.Add("Password", passwordText);
                                //templateVars.Add("UserFullName", model.FirstName);
                                //var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/RegisterUser.html"), templateVars);
                                //var emailBody = parser.Parse();
                                //EmailHelper emailHelper = new EmailHelper();
                                //emailHelper.Send(model.Email, string.Empty, string.Empty, "test", emailBody);
                                TempData["isCreatedSuccess"] = true;

                            }
                            else
                            {
                                TempData["isUpdatedSuccess"] = true;
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    logService.LogError("Error in Add Edit User", ex);
                }
            }
            else
            {
                //If pageacees code= customeradmin not found then show error in toastr 
                TempData["CustomerRole"] = false;
            }

            return RedirectToAction("ViewUser", "ManageUsers");

        }
        /// <summary>
        /// Edit Store User
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult AddEditStoreUser(Guid? uid)
        {
            var model = new UserModels();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            ViewBag.CustomerLst = bindCustomer(); // Bind customer
            Session["UserUid"] = null;
            if (uid == null)
            {
                uid = new Guid();
            }

            try
            {
                ViewBag.Title = "Add New User";

                if (uid != new Guid())
                {
                    Session["UserUid"] = uid;
                    model = _ManageUserService.GetUserById(uid);
                    model.UserId = (System.Guid)uid;
                    var UserLevelInfo = GetUserTypeName(model.User_Level_Id);
                    model.UserType = UserLevelInfo.Text;
                    ViewBag.isUpdateMode = true;
                    string cust_Id = model.Customer_Id.ToString();
                    if (cust_Id != "")
                    {
                        if (currentUser.PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                        {
                            ViewBag.LocationLst = _generalService.GetCustomerLocationService(Convert.ToInt32(cust_Id));
                            ViewBag.SubsidiaryLst = _generalService.GetSubsidiaryByCustomerId(Convert.ToInt32(cust_Id), (int)currentUser.SubsidiaryId);
                            ViewBag.SubAgentLst = _generalService.GetAgentsBySubsidiaryId(Convert.ToInt32(model.SubsidiaryId));

                        }
                        else if (currentUser.PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                        {
                            ViewBag.LocationLst = _generalService.GetCustomerLocationService(Convert.ToInt32(cust_Id));
                            ViewBag.SubsidiaryLst = _generalService.GetSubsidiaryByCustomerId(Convert.ToInt32(cust_Id), (int)currentUser.SubsidiaryId);
                            ViewBag.SubAgentLst = _generalService.GetAgentsBySubsidiaryId(Convert.ToInt32(model.SubsidiaryId), (int)currentUser.SubAgentId);
                        }
                        else
                        {
                            ViewBag.LocationLst = _generalService.GetCustomerLocationService(Convert.ToInt32(cust_Id));
                            ViewBag.SubsidiaryLst = _generalService.GetSubsidiaryByCustomerId(Convert.ToInt32(cust_Id));
                            ViewBag.SubAgentLst = _generalService.GetAgentsBySubsidiaryId(Convert.ToInt32(model.SubsidiaryId));
                        }

                    }
                    else
                    {
                        ViewBag.LocationLst = _generalService.GetEmptyDDLWithoutSelect();
                        ViewBag.SubsidiaryLst = _generalService.GetEmptyDDLWithoutSelect();
                        ViewBag.SubAgentLst = _generalService.GetEmptyDDLWithoutSelect();
                    }
                }
                else
                {
                    model.Status = true;
                    ViewBag.LocationLst = _generalService.GetEmptyDDLWithoutSelect();
                    ViewBag.SubsidiaryLst = _generalService.GetEmptyDDLWithoutSelect();
                    ViewBag.SubAgentLst = _generalService.GetEmptyDDLWithoutSelect();
                }
            }
            catch (Exception ex)
            {
                logService.LogError("Error in Add Edit User Page Loading...", ex);
            }
            return View("AddEditStoreUser", model);




        }

        public ActionResult ViewPendingTask()
        {
            ViewBag.UserTypeLst = _generalService.GetUserLevelType("PAGEACCESCODE");
            Session["RedirectArchiveManage"] = "ViewPendingTask";
            return View();
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
        public ActionResult AddStoreUserByXLSFile(UserModels objUserModel, IEnumerable<HttpPostedFileBase> files)
        {
            Session["StoreUserResult"] = null;
            string bulkUserPassword = ConfigurationSettings.AppSettings["StoreUserBulkUploadPassword"].ToString();
            if (objUserModel.Customer_Id > 0 && files != null && files.Count() > 0)
            {

                string sDirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\TempUpload\Users\" + objUserModel.Customer_Id;
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

                string dirPath1 = @"\Users\" + objUserModel.Customer_Id.ToString();

                storeDocument(files, dirPath1);

                foreach (var item in files)
                {
                    objUserModel.FileName = sPath + @"\\" + item.FileName;
                }

                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                objUserModel.Created_by = objCurrentUserDetail.User_Id;
                objUserModel.Created_Date = Common.CommonMethods.GetCurrentDate();


                UserModels objCustomerUserResult = new UserModels();

                objUserModel.Pasword = bulkUserPassword;

                bool bResult = _ManageUserService.UserFileUpload(objUserModel, out objCustomerUserResult);

                ModelState.AddModelError("Name", "test");
                ViewBag.CustomerLst = bindCustomer();
                ViewBag.ManufacturerLst = _generalService.GetManufacturers();
                ViewBag.SubsidiaryLst = _generalService.GetEmptyDDLWithoutSelect();
                ViewBag.AgentLst = _generalService.GetEmptyDDLWithoutSelect();
                ViewBag.LocationLst = _generalService.GetEmptyDDLWithoutSelect();

                ModelState.Remove("Email");
                ModelState.Remove("emailEdit");
                ModelState.Remove("FirstName");
                ModelState.Remove("UserMobile");
                ModelState.Remove("User_Level_Id");
                ModelState.Remove("Manufacturer_Id");

                Session["StoreUserResult"] = objCustomerUserResult;
                if (objCustomerUserResult.ValidUserNameToSendMail.Count > 0)
                {
                    SendMailBulkUploadStoreUser(bulkUserPassword, objCustomerUserResult.ValidUserNameToSendMail);
                }
                return View("AddEditUsers", objCustomerUserResult);
            }

            return RedirectToAction("AddEditUser", "ManageUsers");
        }
        private void SendMailBulkUploadStoreUser(string userPassword, List<ValidMailIdForBulkUpload> _ValidmailId)
        {
            string strMailId = "";
            string Comma = "";
            foreach (var mailIdObj in _ValidmailId)
            {
                strMailId = strMailId + Comma + mailIdObj.Email_Id;
                Comma = ";";
            }
            var templateVars = new Hashtable();
            templateVars.Add("LoginURL", ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString());
            templateVars.Add("Password", userPassword);
            var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/RegisterStoreUserBulk.html"), templateVars);
            var emailBody = parser.Parse();
            EmailHelper emailHelper = new EmailHelper();
            emailHelper.Send(string.Empty, string.Empty, strMailId, "Redux - Password Notification", emailBody);
            TempData["isCreatedSuccess"] = true;
        }

        [AjaxHandleException]
        public ActionResult UserResultAjax([DataSourceRequest] DataSourceRequest request)
        {
            var objCustomerLocationResult = new LocationModel();
            if (Session["LocationResult"] != null)
            {
                objCustomerLocationResult = (LocationModel)Session["LocationResult"];

            }
            DataSourceResult result = objCustomerLocationResult.LocationResult.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public JsonResult GetSubsidiaryByCustomer(int CustomerId)
        {
            try
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                if (PageAccessCode.SUBSIDIARYADMIN == currentUser.PageAccessCode)
                {
                    var ListSubsidary = _generalService.GetSubsidiaryByCustomerId(CustomerId, (int)currentUser.SubsidiaryId);
                    return Json(ListSubsidary, JsonRequestBehavior.AllowGet);
                }
                else if (PageAccessCode.SUBAGENTADMIN == currentUser.PageAccessCode)
                {
                    var ListSubsidary = _generalService.GetSubsidiaryByCustomerId(CustomerId, (int)currentUser.SubsidiaryId);
                    return Json(ListSubsidary, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ListSubsidary = _generalService.GetSubsidiaryByCustomerId(CustomerId);
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
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                if (PageAccessCode.SUBAGENTADMIN == currentUser.PageAccessCode)
                {
                    var ListAgent = _generalService.GetAgentsBySubsidiaryId(SubsidiaryId, (int)currentUser.SubAgentId);
                    return Json(ListAgent, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ListAgent = _generalService.GetAgentsBySubsidiaryId(SubsidiaryId);
                    return Json(ListAgent, JsonRequestBehavior.AllowGet);
                }
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

        public ActionResult UpdatePassword(UserModels passwordModel)
        {
            var flag = false;
            try
            {
                string _newPassword = passwordModel.NewPassword;
                passwordModel.NewPassword = passwordModel.NewPassword.ToMD5HashForPassword();
                ILoginService LoginService = new LoginService();
                flag = LoginService.UpdatePasswordByUserID(passwordModel);
                if (flag == true)
                {
                    TempData["isUpdated"] = true;
                    if (passwordModel.IsEmailNotification == true)
                    {
                        sendNotification(_newPassword, passwordModel.FirstName, passwordModel.emailEdit);
                    }
                }
                else
                {
                    TempData["isUpdated"] = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("ManagePassword", "ManageUsers");

        }
        private void sendNotification(string newPassword, string userFullName, string Email)
        {
            var emailLink = ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString();
            var templateVars = new Hashtable();
            templateVars.Add("LoginURL", emailLink);
            templateVars.Add("Password", newPassword);
            //Updated changes for bug #6128
            templateVars.Add("UserFullName", userFullName);
            templateVars.Add("UserName", Email);
            var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ManagePassword.html"), templateVars);
            var emailBody = parser.Parse(); // string.Format(ConstantEntities.NewCustomerEmailBody, emailLink, customer.Email, pasword);
            EmailHelper emailHelper = new EmailHelper();

            emailHelper.Send(Email, string.Empty, Convert.ToString(ConfigurationManager.AppSettings["EmailBcc"]), "Redux - New Password", emailBody);
            // Session["TempFrgtMsg"] = "Password has been sent to your registered email address successfully.";


        }

        #region UPS integration & Group allocation in location

        public ActionResult ManageGroups()
        {
            GroupModel obj = new GroupModel();          
            return View("ManageGroups", obj);
        }

        public ActionResult GetGroups([DataSourceRequest]DataSourceRequest request)
        {
            List<GroupModel> ObjGroupModelList = _ManageUserService.GetGroupList();
            DataSourceResult result = ObjGroupModelList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddGroup()
        {
            string fromBase64 = "";
            var ObjGroup = new GroupModel();
            ObjGroup.PriorityList = _generalService.GetPriority();
            
            ObjGroup.GroupId = 0;
            return View(ObjGroup);
        }

        public ActionResult EditGroup(int Id, string path)
        {
            var ObjGroup = new GroupModel();
            int totalmachines = 0;
            int shippedmachines = 0;
            if (Id != 0)
            {
                ObjGroup = _ManageUserService.GetGroupById(Id);
                ObjGroup.PriorityList = _generalService.GetPriority();
                var shippingDetailsData = manageShippingService.GetShippingQueueDetails();
                shippingDetailsData = shippingDetailsData.Where(x => x.GroupId == Id).ToList();
                foreach (var item in shippingDetailsData)
                {
                    totalmachines = totalmachines + item.NoOfMachinesOrdered;
                    shippedmachines = shippedmachines + item.TotalShipped;
                }
                if(shippedmachines>0 && shippedmachines<totalmachines)
                {
                    ObjGroup.Machinestatus = "Started";
                }
                else if (shippedmachines == totalmachines)
                {
                     ObjGroup.Machinestatus = "Completed";
                }
                else
                {
                    ObjGroup.Machinestatus = "Pending";
                }
                if (path != "ManageGroup")
                {
                    TempData["fromShippingQueue"] = true;
                }
                //shippingDetailsData = shippingDetailsData.GroupBy(s => s.GroupId).Select(n => new GroupModel { MachineTotalount = n.Key., Value = n.Count() }).ToList();
            }
            return View(ObjGroup);
        }

        public ActionResult AddEditGroup(GroupModel objGroup)
        {
            var Result = false;
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];  

            Result = _ManageUserService.AddEditGroup(objGroup, currentUser.User_Id);
            if (Result == true && objGroup.GroupId > 0)
            {
                TempData["isUpdatedSuccess"] = true;
            }
            else
            {
                TempData["isCreatedSuccess"] = true;
            }
            if (objGroup.Redirectpath!=null && objGroup.Redirectpath!=string.Empty)
            {
                return RedirectToAction("ManageGroupShippingQueue", "ManageShipping");
            }
            return RedirectToAction("ManageGroups");
        }

        public JsonResult CheckDuplicateGroupName(string GroupName, string GroupId)
        {
            int _GroupId = 0;
            if (GroupId != null && GroupId != string.Empty && GroupId != "undefined")
            {
                _GroupId = Convert.ToInt32(GroupId);
            }
            var data = !_ManageUserService.CheckDuplicateGroupName(GroupName, _GroupId);
            return Json(data);
        }

        public JsonResult CheckDuplicatePriority(string Priority_Id, string GroupId)
        {
            int _Priority_Id = 0;
            if (Priority_Id != null && Priority_Id != string.Empty)
            {
                _Priority_Id = Convert.ToInt32(Priority_Id);
            }
            int _GroupId = 0;
            if (GroupId != null && GroupId != string.Empty && GroupId != "undefined")
            {
                _GroupId = Convert.ToInt32(GroupId);
            }
            var data = !_ManageUserService.CheckDuplicatePriority(_Priority_Id, _GroupId);
            return Json(data);
        }

        public ActionResult ChangeStatusByGroupId(GroupModel objGroupModel)
        {
            if (objGroupModel.GroupId > 0)
            {
                var objCurrentUserDetail = (CurrentUserDetail)Session["CurrentUser"];
                bool bResult = false;
                GroupModel objGpModel = new GroupModel();
                if (objGroupModel.GroupStatus == true)
                {
                    objGpModel.GroupStatus = false;
                }
                else
                {
                    objGpModel.GroupStatus = true;
                }

                bResult = _ManageUserService.ChangeStatusByGroupId(objGroupModel.GroupId, objCurrentUserDetail.User_Id, objGpModel.GroupStatus);

                if (bResult == true)
                {
                    objGroupModel.Result = "OK";
                    if (objGpModel.GroupStatus)
                    {
                        objGpModel.Successmsg = ReviveMessages.Active;
                    }
                    else
                    {
                        objGpModel.Successmsg = ReviveMessages.Delete;
                    }
                    return Json(objGpModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objGroupModel.Result = "OK";
                    string sStatus = string.Empty;
                    objGpModel.ErrorMsgs = "Group not deleted.Locations are allocated to this group.";
                    objGpModel.Successmsg = "";
                    return Json(objGpModel, JsonRequestBehavior.AllowGet);
                }
            }

            return View("ManageGroups");
        }
       
        #endregion
    }


}