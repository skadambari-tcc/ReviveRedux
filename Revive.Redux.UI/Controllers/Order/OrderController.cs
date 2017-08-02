using Kendo.Mvc.UI;
using Revive.Redux.Services;
using System;
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
using System.Web;
using System.Linq;
using System.IO;

namespace Revive.Redux.UI
{
    [Authorize]
    [ReviveAuth]
    public class OrderController : Controller
    {
        // TODO: Pending. Implement if currentUser is null.
        private IOrderManagementService _IOrderManagementService = null;
        IGeneralService _IGeneralService = null;
        public OrderController()
        {
            _IOrderManagementService = new OrderManagementService();
            _IGeneralService = new GeneralService();
        }

        #region Manage Orders
        /// <summary>
        /// Default View Manage Orders
        /// </summary>
        /// <returns></returns>
        public ActionResult Manage()
        {
            Session["RedirectArchiveManage"] = "Manage";
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new OrderModel();
            model.PageAccessCode = currentUser.PageAccessCode;
            return View("Manage", model);
        }

        public ActionResult ManageMachineOrder()
        {
            Session["RedirectArchiveManage"] = "Manage";
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new OrderModel();
            model.PageAccessCode = currentUser.PageAccessCode;
            return View("ManageMachineOrder", model);
        }
        public ActionResult ManageOrdersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                int manufac_ID = Convert.ToInt32(currentUser.Manufacturer_Id);// (currentUser.Manufacturer_Id == null) ? 0 : (int)currentUser.Manufacturer_Id;
                IEnumerable<OrderModel> orders = _IOrderManagementService.GetOrdersList(currentUser.PageAccessCode, currentUser.User_Id, manufac_ID);
                DataSourceResult result = orders.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ManageMachineOrdersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode))
            {
                int manufac_ID = Convert.ToInt32(currentUser.Manufacturer_Id);// (currentUser.Manufacturer_Id == null) ? 0 : (int)currentUser.Manufacturer_Id;
                IEnumerable<OrderModel> orders = _IOrderManagementService.GetMachineOrdersList();
                DataSourceResult result = orders.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RemoveOrder(OrderModel model)
        {
            OrderModel Viewmodel = new OrderModel();
            Viewmodel.result = _IOrderManagementService.RemoveOrder(model);
            if (Viewmodel.result)
            {
                Viewmodel.SuccessMsg = ReviveMessages.OrderRemove;
            }
            return Json(Viewmodel, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Approve/Reject Order
        /// <summary>
        /// Get order data by id for approval
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Approve(String id)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (Request.UrlReferrer != null)
                ViewBag.ReturnUrl = Request.UrlReferrer;
            else
                ViewBag.ReturnUrl = "#";

            // TODO: change sp 
            var model = new OrderModel();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string decryptedId = CommonMethods.Decode(id);
                    OrderModel item = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));
                    if (item != null)
                    {
                        model.CustomerName = item.CustomerName;
                        model.CustomerId = item.CustomerId;
                        model.JobOrderHeaderId = item.JobOrderHeaderId;
                        model.JobOrderHeaderIdEncoded = item.JobOrderHeaderIdEncoded;
                        model.CustomerPONumber = item.CustomerPONumber;
                        model.CustomerDocName = item.CustomerDocName;
                        model.NoOfMachines = item.NoOfMachines;
                        model.ExpectedOrderDate = Convert.ToDateTime(item.ExpectedDate).ToString("MM/dd/yyyy");
                        model.ApproverComments = string.IsNullOrEmpty(item.ApproverComments) ? string.Empty : HttpUtility.HtmlDecode(item.ApproverComments);
                        model.ClientExecComments = string.IsNullOrEmpty(item.ClientExecComments) ? string.Empty : HttpUtility.HtmlDecode(item.ClientExecComments);
                        model.MachineSpecs = item.MachineSpecs;
                        model.MachineSpecsId = item.MachineSpecsId;
                        model.StatusName = item.StatusName;
                        model.CreatedByUserId = item.CreatedByUserId;
                        model.ManufacturerId = item.ManufacturerId;
                        model.PageAccessCode = currentUser.PageAccessCode;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View("Approve", model);
        }

        /// <summary>
        /// Approve/Reject order by Approver.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public ActionResult ApproveRejectOrder(OrderModel model)
        {
            try
            {
                string command = Request.Form["hdnActionTaken"];
                if (!string.IsNullOrEmpty(command) && command.Equals("Approve") || command.Equals("Reject"))
                {
                    var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                    OrderModel objOrder = new OrderModel();
                    objOrder.JobOrderHeaderId = model.JobOrderHeaderId;
                    objOrder.ApproverComments = string.IsNullOrEmpty(model.ApproverComments) ? string.Empty : HttpUtility.HtmlEncode(model.ApproverComments.Trim());
                    if (Session != null && Session["CurrentUser"] != null)
                    {
                        objOrder.ApprovedRejectedUserId = currentUser.User_Id;
                        objOrder.ModifiedByUserId = currentUser.User_Id;
                    }
                    if (command.Equals("Approve"))
                    {
                        objOrder.JobOrderStatusId = _IOrderManagementService.GetOrderStatusId(OrderStatusType.PendingPC);
                        _IOrderManagementService.UpdateOrderStatus(objOrder);
                        ApproveRejectNotification(currentUser.FullName, OrderStatusType.Approved, model);
                        TempData["OrderApproved"] = true;
                    }
                    else if (command.Equals("Reject"))
                    {
                        objOrder.JobOrderStatusId = _IOrderManagementService.GetOrderStatusId(OrderStatusType.Rejected);
                        _IOrderManagementService.UpdateOrderStatus(objOrder);
                        ApproveRejectNotification(currentUser.FullName, OrderStatusType.Rejected, model);
                        TempData["OrderRejected"] = true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Manage");
        }

        /// <summary>
        /// Approve Reject order Notification email to users 
        /// </summary>
        /// <param name="currentUserName"></param>
        /// <param name="status"></param>
        /// <param name="orderId"></param>
        private void ApproveRejectNotification(string currentUserName, string status, OrderModel model)
        {
            int orderId = (int)model.JobOrderHeaderId;
            int customerId = (int)model.CustomerId;

            string emailTo = string.Empty;
            string emailCC = string.Empty;
            if (status == OrderStatusType.Approved)
            {
                emailTo = Helper.GetEmailAddressByGroup(PageAccessCode.MFPC, (int)model.ManufacturerId);        // TO Assigned Manufacturer's MFPC users
            }
            else if (status == OrderStatusType.Rejected)
            {
                IUserManagmentService service = new UserManagementService();
                emailTo = service.GetUserById((Guid)model.CreatedByUserId).Email;       // TO Owner of order                
            }
            emailCC = Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);              // CC client executives
            emailCC += Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, customerId);    // CC Assigned Account managers

            if (!string.IsNullOrEmpty(emailTo))
            {
                string subject = string.Format("Redux - Order {0} has been {1}", orderId, status);
                var templateVars = new Hashtable();
                templateVars.Add("OrderStatus", status);
                templateVars.Add("OrderId", orderId);
                templateVars.Add("CurrentUserName", currentUserName);
                templateVars.Add("CustomerName", model.CustomerName);
                if (status == OrderStatusType.Approved)
                {
                    UserModels user = Helper.GetAMbyCustId(PageAccessCode.ACCNTMGR, customerId);
                    if (user != null)
                    {
                        string AMName = !string.IsNullOrEmpty(user.FirstName) ? user.FirstName : string.Empty;
                        AMName += " " + (!string.IsNullOrEmpty(user.LastName) ? user.LastName : string.Empty);
                        templateVars.Add("ActiveAssignedAM", string.Format("Contact {0} if there are any questions.", AMName));
                    }
                }
                templateVars.Add("ViewOrderPath", ConfigurationSettings.AppSettings["ViewOrderPath"].ToString() + CommonMethods.Encode(Convert.ToString(orderId)));
                var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/Order/ApproveReject.html"), templateVars);
                var emailBody = parser.Parse();
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.Send(emailTo, emailCC, string.Empty, subject, emailBody);
            }
            else
            {
                // TODO: Pending. Implement if emailTo not available.
            }
        }
        #endregion

        #region Archive Orders
        /// <summary>
        /// View Archive orders
        /// </summary>
        /// <returns></returns>
        public ViewResult Archive()
        {
            Session["RedirectArchiveManage"] = "Archive";
            return View();
        }
        /// <summary>
        /// View Archive Orders - only for Account Manager & Client Exec
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult ArchiveOrdersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PageAccessCode) &&
                (currentUser.PageAccessCode == PageAccessCode.CLIENTEXEC || currentUser.PageAccessCode == PageAccessCode.ACCNTMGR || currentUser.PageAccessCode == PageAccessCode.ADMIN || currentUser.PageAccessCode == PageAccessCode.SUPERADMIN))
            {
                IEnumerable<OrderModel> orders = _IOrderManagementService.GetArchiveOrders(currentUser.PageAccessCode);
                DataSourceResult result = orders.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateArchiveStatus([DataSourceRequest] DataSourceRequest request, OrderModel model)
        {
            try
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                _IOrderManagementService.SetArchiveStatus((int)model.JobOrderHeaderId);
                ArchiveOrderNotification(currentUser.FullName, (int)model.JobOrderHeaderId, (int)model.CustomerId);
            }
            catch (Exception ex)
            {
                // TODO: Pending.
                throw;
            }
            return Json(new[] { model }.ToDataSourceResult(request));
        }
        /// <summary>
        /// Archive Order Notification TO Approver, Client Executives,Account Managers
        /// </summary>                 
        /// <param name="currentUserNamTO Account Managerse"></param>
        /// <param name="OrderId"></param>
        private void ArchiveOrderNotification(string currentUserName, int OrderId, int customerId)
        {
            string emailTo = Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, customerId);   // TO Assigned Account Managers
            emailTo += Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);                   // TO  All Client Executives
            emailTo += Helper.GetEmailAddressByGroup(PageAccessCode.APPROVER);                     // TO All Approvers

            string emailCC = string.Empty;

            if (!string.IsNullOrEmpty(emailTo))
            {
                string subject = "Redux - Order " + OrderId + " has been Archived";
                var templateVars = new Hashtable();
                templateVars.Add("CurrentUserName", currentUserName);
                templateVars.Add("OrderId", OrderId);
                var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/Order/Archive.html"), templateVars);
                var emailBody = parser.Parse();
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.Send(emailTo, emailCC, string.Empty, subject, emailBody);
            }
            else
            {
                // TODO: Pending. Implement is users not available.
            }
        }
        #endregion

        #region Assign Order
        /// <summary>
        /// View Assign Order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Assign(String id)
        {

            var model = new OrderModel();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string decryptedId = CommonMethods.Decode(id);
                    OrderModel item = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));
                    if (item != null)
                    {
                        model.CustomerName = item.CustomerName;
                        model.CustomerId = item.CustomerId;
                        model.JobOrderHeaderId = item.JobOrderHeaderId;
                        model.JobOrderHeaderIdEncoded = item.JobOrderHeaderIdEncoded;
                        model.CustomerPONumber = item.CustomerPONumber;
                        model.NoOfMachines = item.NoOfMachines;
                        model.ExpectedDate = item.ExpectedDate;

                        model.ExpectedOrderDate = Convert.ToDateTime(item.ExpectedDate).ToString("MM/dd/yyyy");
                        model.ApproverComments = string.IsNullOrEmpty(item.ApproverComments) ? string.Empty : HttpUtility.HtmlDecode(item.ApproverComments);
                        model.ClientExecComments = string.IsNullOrEmpty(item.ClientExecComments) ? string.Empty : HttpUtility.HtmlDecode(item.ClientExecComments);
                        model.ManufacturerComments = string.IsNullOrEmpty(item.ManufacturerComments) ? string.Empty : HttpUtility.HtmlDecode(item.ManufacturerComments);
                        model.MachineSpecs = item.MachineSpecs;
                        model.MachineSpecsId = item.MachineSpecsId;
                        model.StatusName = item.StatusName;
                        model.DownloadSWUrl = string.IsNullOrEmpty(item.DownloadSWUrl)
                            ? "#"
                            : GetSiteUrl(Request.Url) + item.DownloadSWUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View("Assign", model);
        }
        /// <summary>
        /// Assign Order
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AssignOrder(OrderModel orderModel)
        {
            try
            {
                string oldExpectedDate = Request.Form["OriginalExpectedDate"];
                string newExpectedDate = Convert.ToDateTime(orderModel.ExpectedDate).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(oldExpectedDate))
                {
                    oldExpectedDate = Convert.ToDateTime(oldExpectedDate).ToString("MM/dd/yyyy");
                    if (DateTime.Compare(Convert.ToDateTime(oldExpectedDate), Convert.ToDateTime(newExpectedDate)) == 0)
                    {
                        oldExpectedDate = string.Empty;
                    }
                }
                _IOrderManagementService.AssignOrder(orderModel);
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                AssignOrderNotification(currentUser.FullName, (int)orderModel.JobOrderHeaderId, oldExpectedDate, newExpectedDate, (int)orderModel.CustomerId);
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Manage", "Order");
        }
        /// <summary>
        /// Assign Order Notification to Approver,Client Executives,Account Managers
        /// </summary>
        /// <param name="currentUserName"></param>
        /// <param name="OrderId"></param>
        /// <param name="oldExpectedDate"></param>
        /// <param name="newExpectedDate"></param>
        private void AssignOrderNotification(string currentUserName, int OrderId, string oldExpectedDate, string newExpectedDate, int customerId)
        {
            string emailTo = Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, customerId);     // TO Assigned AM
            emailTo += Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);         // TO all Client Executives
            emailTo += Helper.GetEmailAddressByGroup(PageAccessCode.APPROVER);           // TO all Approvers

            string emailCC = string.Empty;

            if (!string.IsNullOrEmpty(emailTo))
            {
                var templateVars = new Hashtable();
                templateVars.Add("ViewOrderPath", ConfigurationSettings.AppSettings["ViewOrderPath"].ToString() + CommonMethods.Encode(Convert.ToString(OrderId)));
                templateVars.Add("CurrentUserName", currentUserName);
                templateVars.Add("OrderId", OrderId);
                // Implement notification in case of expected date changed.
                if (!string.IsNullOrEmpty(oldExpectedDate) && !string.IsNullOrEmpty(newExpectedDate))
                {
                    string temp = string.Format("<p class=MsoNormal>&nbsp;</p><p class=MsoNormal>{0} has edited the Expected Delivery Date.</p><p class=MsoNormal>Old date was {1}</p>" +
                       "<p class=MsoNormal>New date is {2}</p><p class=MsoNormal>&nbsp;</p>", currentUserName, oldExpectedDate, newExpectedDate);
                    templateVars.Add("DateChangeNotification", temp);
                }
                else
                    templateVars.Add("DateChangeNotification", "<p class=MsoNormal>&nbsp;</p>");
                string subject = string.Format("Redux - Order {0} has been Assigned", OrderId);
                var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/Order/Assign.html"), templateVars);
                var emailBody = parser.Parse();
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.Send(emailTo, emailCC, string.Empty, subject, emailBody);
                TempData["OrderAssigned"] = true;
            }
            else
            {
                // TODO: Pending. Implement is users not available.
            }
        }
        #endregion

        #region View Orders
        /// <summary>
        /// View Order by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult ViewOrder(String id)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new OrderModel();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string decryptedId = CommonMethods.Decode(id);
                    OrderModel item = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));
                    if (item != null)
                    {
                        model.CustomerDocName = item.CustomerDocName;
                        if (item.ApprovedRejectedDate != "01/01/0001")
                            model.ApprovedRejectedDate = item.ApprovedRejectedDate;
                        model.ApproverComments = string.IsNullOrEmpty(item.ApproverComments) ? string.Empty : HttpUtility.HtmlDecode(item.ApproverComments);
                        model.ClientExecComments = string.IsNullOrEmpty(item.ClientExecComments) ? string.Empty : HttpUtility.HtmlDecode(item.ClientExecComments);
                        model.ManufacturerComments = string.IsNullOrEmpty(item.ManufacturerComments) ? string.Empty : HttpUtility.HtmlDecode(item.ManufacturerComments);
                        model.ManufacturerName = item.ManufacturerName;
                        model.ApprovedRejectedUserName = item.ApprovedRejectedUserName;
                        model.StatusName = item.StatusName;
                        model.NoOfMachinesDelivered = item.NoOfMachinesDelivered;
                        model.CustomerName = item.CustomerName;
                        model.CustomerId = item.CustomerId;
                        model.JobOrderHeaderId = item.JobOrderHeaderId;
                        model.JobOrderHeaderIdEncoded = item.JobOrderHeaderIdEncoded;
                        model.CustomerPONumber = item.CustomerPONumber;
                        model.CustomerDocName = item.CustomerDocName;
                        model.NoOfMachines = item.NoOfMachines;
                        model.ExpectedOrderDate = Convert.ToDateTime(item.ExpectedDate).ToString("MM/dd/yyyy");
                        model.MachineSpecs = item.MachineSpecs;
                        model.MachineSpecsId = item.MachineSpecsId;
                        model.PageAccessCode = currentUser.PageAccessCode;
                        model.AccountManagerName = item.AccountManagerName;
                        model.SubsidiaryId = item.SubsidiaryId;
                        model.SubAgentId = item.SubAgentId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return View("ViewOrder", model);
        }
        #endregion

      

        #region Create Order
        /// <summary>
        /// Load create new order 
        /// </summary>
        /// <returns></returns>
        public ViewResult Create()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            // TODO: Pending. // SR 32.11, 32.12
            var customers = _IGeneralService.GetCustomerService();
            var orderModel = new OrderModel
            {
                CustomerList = _IGeneralService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode),
                CustomerDocList = _IGeneralService.GetEmptyDDL(),
                MachineSpecsList = _IGeneralService.GetMachineSpecs(),
                PageAccessCode = currentUser.PageAccessCode,
                ManufacturerList = _IGeneralService.GetManufacturers()
            };
            return View("Create", orderModel);
        }

        public ViewResult CreateMachineOrder()
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            // TODO: Pending. // SR 32.11, 32.12
            var customers = _IGeneralService.GetCustomerService();
            var orderModel = new OrderModel
            {
                //CustomerList = _IGeneralService.GetCustomerStoreUser(currentUser.User_Id, currentUser.PageAccessCode),
               // CustomerDocList = _IGeneralService.GetEmptyDDL(),
                MachineSpecsList = _IGeneralService.GetMachineSpecs(),
                PageAccessCode = currentUser.PageAccessCode,
                ManufacturerList = _IGeneralService.GetManufacturers()
            };
            return View("CreateMachineOrder", orderModel);
        }
        /// <summary>
        /// Create new Order 
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrder(OrderModel orderModel)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            OrderModel newOrder = new OrderModel();
            newOrder.CustomerId = orderModel.CustomerId;
            newOrder.JobOrderStatusId = _IOrderManagementService.GetOrderStatusId(OrderStatusType.PendingApproval);
            newOrder.CustomerPONumber = Convert.ToString(orderModel.CustomerDocId);
            newOrder.CustomerDocId = orderModel.CustomerDocId;
            //objDtoUser.CustomerDocName = cust.CustomerDocName;
            newOrder.NoOfMachines = orderModel.NoOfMachines;
            newOrder.ExpectedDate = orderModel.ExpectedDate;
            newOrder.MachineSpecsId = orderModel.MachineSpecsId;
            newOrder.DownloadSWUrl = _IOrderManagementService.GetDownloadSoftwareUrl((int)orderModel.MachineSpecsId);
            newOrder.ClientExecComments = string.IsNullOrEmpty(orderModel.ClientExecComments) ? string.Empty : HttpUtility.HtmlEncode(orderModel.ClientExecComments.Trim());
            newOrder.ManufacturerId = orderModel.ManufacturerId;
            //objDtoUser.ApproverComments = cust.ApproverComments;
            newOrder.ArchiveFlag = false;
            newOrder.CreatedByUserId = currentUser.User_Id;
            newOrder.CreatedDate = DateTime.Now;

            newOrder.SubsidiaryId = orderModel.SubsidiaryId;
            newOrder.SubAgentId = orderModel.SubAgentId;
            List<OrderLocationsModel> orderLocations = new List<OrderLocationsModel>();
            if (!string.IsNullOrEmpty(orderModel.JsonPostbackVal))
            {
                orderLocations = JsonConvert.DeserializeObject<List<OrderLocationsModel>>(orderModel.JsonPostbackVal);
            }
            try
            {
                string orderId = _IOrderManagementService.CreateOrder(newOrder, orderLocations);
                CreateOrderNotification(currentUser.FullName, Convert.ToInt16(orderModel.CustomerId), orderId);
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("Manage", "Order");
        }

        [HttpPost]
        public ActionResult CreateMachineOrder(OrderModel orderModel)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            OrderModel newOrder = new OrderModel();          
           
            newOrder.NoOfMachines = orderModel.NoOfMachines;      
            newOrder.MachineSpecsId = orderModel.MachineSpecsId;    
            newOrder.ClientExecComments = string.IsNullOrEmpty(orderModel.ClientExecComments) ? string.Empty : HttpUtility.HtmlEncode(orderModel.ClientExecComments.Trim());
            newOrder.ManufacturerId = orderModel.ManufacturerId;           
            newOrder.CreatedByUserId = currentUser.User_Id;
            newOrder.CreatedDate = DateTime.Now;

            try
            {
                string orderId = _IOrderManagementService.CreatemachineOrder(newOrder);
                TempData["OrderCreated"] = true;
                //CreateOrderNotification(currentUser.FullName, Convert.ToInt16(orderModel.CustomerId), orderId);
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("ManageMachineOrder", "Order");
        }
        /// <summary>
        /// Create Order Notification to Approver,Client Executives,Account Managers
        /// </summary>
        /// <param name="currentUserName"></param>
        private void CreateOrderNotification(string currentUserName, int customerId, string orderId)
        {
            ICustomerManagement _ICustomerManagement = new CustomerManagement();
            string customerName = _ICustomerManagement.GetCustomerDetailsById(customerId).Customer_Name;

            string emailTo = Helper.GetEmailAddressByGroup(PageAccessCode.APPROVER);            // TO Approver    
            string emailCC = Helper.GetEmailAddressByGroup(PageAccessCode.CLIENTEXEC);          // CC Client Executives
            emailCC += Helper.GetEmailAddressByGroup(PageAccessCode.ACCNTMGR, customerId);      // CC Assigned Account Managers

            if (!string.IsNullOrEmpty(emailTo))
            {
                var templateVars = new Hashtable();
                templateVars.Add("ApproveOrderPath", ConfigurationSettings.AppSettings["ApproveOrderPath"].ToString() + CommonMethods.Encode(orderId));
                templateVars.Add("CustomerName", customerName);
                templateVars.Add("CreatedBy", currentUserName);
                var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/Order/Create.html"), templateVars);
                var emailBody = parser.Parse();
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.Send(emailTo, emailCC, string.Empty, "Redux - New Order Created", emailBody);
                TempData["OrderCreated"] = true;
            }
            else
            {
                // TODO: Pending. Implement is users not available.
            }
        }
        /// <summary>
        /// Get Customer POs using customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetCustomerPOs(int customerId)
        {
            var pos = _IGeneralService.GetCustomerDocs(customerId);
            var objDoclst = new List<DtoList>();
            foreach (DtoList obj in pos)
            {

                string dirPath = System.Web.HttpContext.Current.Server.MapPath("~") + @"TempUpload\" + customerId.ToString();
                string CurrentFileName = dirPath + @"\" + obj.otherStrVal;
                if (System.IO.File.Exists(CurrentFileName))
                {
                    objDoclst.Add(obj);
                }


            }
            return Json(objDoclst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Customer Locations using customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCustomerLocations(int customerId, int subsidiaryId, int subagentId)
        {
            var loc = _IOrderManagementService.GetCustomerLocations(customerId, subsidiaryId, subagentId);
            return Json(loc, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View Order Locations
        /// <summary>
        /// View Locations
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult ViewLocations(String id)
        {
            if (Request.UrlReferrer != null)
                ViewBag.ReturnUrl = Request.UrlReferrer;
            else
                ViewBag.ReturnUrl = "#";

            var model = new OrderModel();
            if (!string.IsNullOrEmpty(id))
            {
                string decryptedId = CommonMethods.Decode(id);
                OrderModel item = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));

                if (item != null)
                {
                    model.JobOrderHeaderId = item.JobOrderHeaderId;
                    model.JobOrderHeaderIdEncoded = item.JobOrderHeaderIdEncoded;
                    model.CustomerId = item.CustomerId;
                    model.CustomerName = item.CustomerName;

                    model.CustomerPONumber = item.CustomerPONumber;
                    model.CustomerDocName = item.CustomerDocName;


                    model.ApprovedRejectedDate = item.ApprovedRejectedDate;

                    model.NoOfMachines = item.NoOfMachines;
                    model.ExpectedOrderDate = Convert.ToDateTime(item.ExpectedDate).ToString("MM/dd/yyyy");
                    model.MachineSpecsId = item.MachineSpecsId;
                    model.MachineSpecs = item.MachineSpecs;
                    model.StatusName = item.StatusName;
                }
            }
            return View("ViewLocations", model);
        }

        /// <summary>
        /// Get Order Locations By OrderId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOrderLocationsByOrderId(int? id, int? Pagemode)
        {

            int _id = (int)id;
            int _pageMode = (int)Pagemode;

            var loc = _IOrderManagementService.GetOrderLocationsByOrderId(_id);
           

            if (_pageMode == 1)  // For View 
            {

                loc = loc.Where(x => x.NumberOfMachines > 0).ToList();
               
            }
            return Json(loc, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Order Locations
        /// <summary>
        /// Update Locations using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult UpdateLocations(String id)
        {
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];
            var model = new OrderModel();
            if (!string.IsNullOrEmpty(id))
            {
                string decryptedId = CommonMethods.Decode(id);
                OrderModel item = _IOrderManagementService.GetOrderById(Convert.ToInt32(decryptedId));
                if (item != null)
                {
                    model.JobOrderHeaderId = item.JobOrderHeaderId;
                    model.JobOrderHeaderIdEncoded = item.JobOrderHeaderIdEncoded;
                    model.CustomerId = item.CustomerId;
                    model.CustomerName = item.CustomerName;

                    model.CustomerPONumber = item.CustomerPONumber;
                    model.CustomerDocName = item.CustomerDocName;


                    model.ApprovedRejectedDate = item.ApprovedRejectedDate;

                    model.NoOfMachines = item.NoOfMachines;
                    model.ExpectedOrderDate = Convert.ToDateTime(item.ExpectedDate).ToString("MM/dd/yyyy");
                    model.MachineSpecsId = item.MachineSpecsId;
                    model.MachineSpecs = item.MachineSpecs;
                    model.StatusName = item.StatusName;
                    model.PageAccessCode = currentUser.PageAccessCode;
                    model.ClientExecComments = item.ClientExecComments;
                }
            }
            return View("UpdateLocations", model);
        }

        /// <summary>
        /// Update Order Locations
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateOrderLocations(OrderModel orderModel)
        {
            // TODO: Pending. implement created by, created date, modified by, modiied date.
            // while updating the data
            List<OrderLocationsModel> orderLocations = new List<OrderLocationsModel>();
            if (!string.IsNullOrEmpty(orderModel.JsonPostbackVal))
            {
                List<OrderLocationsModel> list = JsonConvert.DeserializeObject<List<OrderLocationsModel>>(orderModel.JsonPostbackVal);
                orderLocations = list.FindAll(d => d.NumberOfMachines != null);
            }
            _IOrderManagementService.UpdateOrderLocations(orderModel, orderLocations);
            TempData["OrderLocationsUpdated"] = true;
            return RedirectToAction("Manage", "Order");
        }
        #endregion

        /// <summary>
        /// Get Subsidiary using customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetSubsidiaryByCustomer(int CustomerId)
        {
            try
            {
                var ListSubsidary = _IGeneralService.GetSubsidiaryByCustomerId(CustomerId);
                return Json(ListSubsidary, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Agent using SubsidiaryId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetAgentBySubsidiary(int SubsidiaryId)
        {
            try
            {
                var ListAgent = _IGeneralService.GetAgentsBySubsidiaryId(SubsidiaryId);
                return Json(ListAgent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSiteUrl(Uri uri)
        {
            var builder = new UriBuilder(uri.Scheme, uri.Host, uri.Port);
            return Convert.ToString(builder);
        }
    }
}