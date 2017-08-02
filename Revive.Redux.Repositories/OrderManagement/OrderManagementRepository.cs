using System;
using System.Collections.Generic;
using System.Linq;
using Revive.Redux.Entities;
using Revive.Redux.Common;
using System.Web;

namespace Revive.Redux.Repositories
{
    public class OrderManagementRepository : IOrderManagementRepository
    {
        private ILogService logService = null;

        public OrderManagementRepository()
        {
            logService = new LogService();
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderLocations"></param>
        /// <returns></returns>
        public string CreateOrder(OrderModel order, List<OrderLocationsModel> orderLocations)
        {
            string orderId = string.Empty;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    JobOrder_header obj = new JobOrder_header();
                    obj.Customer_ID = (int)order.CustomerId;
                    obj.JobOrder_Status_Id = (int)order.JobOrderStatusId;
                    obj.CustomerPoNumber = order.CustomerPONumber;
                    //obj.CustomerDocId = order.CustomerDocId;
                    obj.Noof_Machines = order.NoOfMachines;
                    obj.Expected_Delivery_Date = order.ExpectedDate;
                    obj.MachineSpec_Id = order.MachineSpecsId;
                    obj.Download_Software_URL = string.IsNullOrEmpty(order.DownloadSWUrl) ? string.Empty : order.DownloadSWUrl;
                    obj.Client_Exec_Notes_Instructions = order.ClientExecComments;
                    obj.Manufacturer_Id = (int)order.ManufacturerId;
                    obj.Archive_flag = order.ArchiveFlag;
                    obj.Created_by = order.CreatedByUserId;
                    obj.Created_Date = order.CreatedDate;
                    obj.Subsidiary_ID = order.SubsidiaryId == null ? 0 : (int)order.SubsidiaryId;
                    obj.SubAgent_ID = order.SubAgentId == null ? 0 : (int)order.SubAgentId;
                    dbContext.JobOrder_header.Add(obj);
                    // TODO: Pending. implement save at one place..throwing exception
                    //dbContext.SaveChanges();
                    foreach (OrderLocationsModel model in orderLocations)
                    {
                        if (model.NumberOfMachines != null)
                        {
                            JobOrder_Locations locations = new JobOrder_Locations();
                            locations.JobOrder_Id = obj.JobOrder_Header_ID;
                            locations.Priority = model.Priority;
                            locations.Location_id = model.LocationId;
                            locations.No_of_Machines = model.NumberOfMachines;
                            locations.Created_by = order.CreatedByUserId;
                            dbContext.JobOrder_Locations.Add(locations);
                            //dbContext.SaveChanges();
                        }
                    }
                    dbContext.SaveChanges();
                    orderId = Convert.ToString(obj.JobOrder_Header_ID);
                }
                return orderId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Create new machine order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderLocations"></param>
        /// <returns></returns>
        public string CreatemachineOrder(OrderModel order)
        {
            string orderId = string.Empty;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    MachineOrder obj = new MachineOrder();                 
                    obj.Noof_Machines = order.NoOfMachines;
                   
                    obj.MachineSpec_Id = order.MachineSpecsId;
                
                    obj.Client_Exec_Notes_Instructions = order.ClientExecComments;
                    obj.Manufacturer_Id = (int)order.ManufacturerId;
                    obj.Order_Date = order.CreatedDate;
                    obj.Created_by = order.CreatedByUserId;
                    obj.Created_Date = order.CreatedDate;
            
                    dbContext.MachineOrders.Add(obj);
                 
                    dbContext.SaveChanges();
                    orderId = Convert.ToString(obj.Order_ID);
                }
                return orderId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Order Locations
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderLocations"></param>
        /// <returns></returns>
        public string UpdateOrderLocations(OrderModel order, List<OrderLocationsModel> orderLocations)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    // Update no_of_location all to 0

                    var allLocations = dbContext.JobOrder_Locations.Where(d => d.JobOrder_Id == order.JobOrderHeaderId).ToList();
                    if (allLocations != null)
                    {
                        foreach (var location in allLocations)
                        {
                            var locToUpdate = dbContext.JobOrder_Locations.Where(d => d.Location_id == location.Location_id && d.JobOrder_Id == order.JobOrderHeaderId).SingleOrDefault();
                            locToUpdate.No_of_Machines = 0;
                        }
                    }

                    // Updating AM comments  in Order Table As per Client Item No 100

                    var OrderHeader = dbContext.JobOrder_header.Where(d => d.JobOrder_Header_ID == order.JobOrderHeaderId).SingleOrDefault();
                    if (OrderHeader != null)
                    {
                        OrderHeader.Client_Exec_Notes_Instructions = order.ClientExecComments;
                    }

                    // Updating AM comments  in Order Table

                    // Update no_of_location all to 0

                    var InsertedLocation = orderLocations.Where(x => x.IsChecked == true && x.NumberOfMachines > 0).ToList();
                    if (InsertedLocation.Count > 0)
                    {
                        foreach (var locationToAdd in InsertedLocation)
                        {
                            if (locationToAdd.NumberOfMachines != null)
                            {
                                if (locationToAdd.LocationId != null)
                                {
                                    var orderLocsToUpdate = dbContext.JobOrder_Locations.Where(d => d.Location_id == locationToAdd.LocationId && d.JobOrder_Id == order.JobOrderHeaderId).SingleOrDefault();
                                    if (orderLocsToUpdate != null) // For Update Order 
                                    {
                                        //dbContext.JobOrder_Locations.Remove(orderLocsToDelete);
                                        orderLocsToUpdate.No_of_Machines = locationToAdd.NumberOfMachines;

                                    }
                                    else
                                    {
                                        // New location added 

                                        var loc = new JobOrder_Locations();
                                        loc.JobOrder_Id = (int)order.JobOrderHeaderId;
                                        loc.Location_id = locationToAdd.LocationId;
                                        loc.No_of_Machines = locationToAdd.NumberOfMachines;
                                        loc.Priority = locationToAdd.Priority;
                                        // TODO: Pending. implement created by, created date, modified by, modiied date.
                                        loc.Created_by = locationToAdd.CreatedBy;
                                        //loc.Created_Date = (DateTime)locationToAdd.CreatedDate;
                                        loc.Modified_by = locationToAdd.ModifiedBy;
                                        //loc.Modified_Date = (DateTime)locationToAdd.ModifiedDate;
                                        dbContext.JobOrder_Locations.Add(loc);

                                    }
                                }



                            }
                        }

                    }
                    dbContext.SaveChanges();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in order management  repository UpdateOrderLocations := ", ex);
                throw;
            }
        }

         public IEnumerable<OrderModel> GetMachineOrdersList()
        {
         
            try
            {
                IEnumerable<OrderModel> objOrderDetails = new List<OrderModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    objOrderDetails = dbContext.MachineOrders.Select(d => new OrderModel
                    {

                        OrderId = d.Order_ID,
                        NoOfMachines = d.Noof_Machines,
                        MachineSpecsId = d.MachineSpec_Id,
                        ManufacturerId = d.Manufacturer_Id,
                        ManufacturerName=d.Manufacturer.Manufacturer_Name,
                        CreatedByUserId = d.Created_by,
                        CreatedDate = d.Created_Date,                     

                    }).ToList();
                }
                return objOrderDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Manage Orders

        /// <summary>
        /// Get Orders List which are not rejected,archived
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderModel> GetOrdersList(string pageAccessCode, Guid userGuid, int manufac_ID)
        {
            IEnumerable<OrderModel> objOrderDetails = new List<OrderModel>();
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    int statusRejected = GetOrderStatusId(OrderStatusType.Rejected);
                    if (pageAccessCode == PageAccessCode.CLIENTEXEC)
                    {
                        objOrderDetails = GetOrders_ClientExec(dbContext, pageAccessCode, statusRejected);
                    }
                    else if (pageAccessCode == PageAccessCode.ACCNTMGR)
                    {
                        objOrderDetails = GetOrders_AccntMgr(dbContext, pageAccessCode, statusRejected, userGuid);
                    }
                    else if (pageAccessCode == PageAccessCode.APPROVER)
                    {
                        objOrderDetails = GetOrders_Approver(dbContext, pageAccessCode, statusRejected);
                    }
                    else if (pageAccessCode == PageAccessCode.MFPC)
                    {
                        objOrderDetails = GetOrders_MFPC(dbContext, pageAccessCode, statusRejected, manufac_ID);
                    }
                    else if (pageAccessCode == PageAccessCode.MFASSEMBLY)
                    {
                        objOrderDetails = GetOrders_MFAssembly(dbContext, pageAccessCode, statusRejected, manufac_ID);
                    }
                    // If Shipping user show only orders with status In-Progress or Shipped and machine status with In-Shipping or Shipped
                    else if (pageAccessCode == PageAccessCode.MFSHIP)
                    {
                        objOrderDetails = GetOrders_MFShip(dbContext, pageAccessCode, statusRejected, manufac_ID);
                    }
                    else if (pageAccessCode == PageAccessCode.ADMIN || pageAccessCode == PageAccessCode.SUPERADMIN)
                    {
                        objOrderDetails = GetOrders_AdminSupAdmin(dbContext, pageAccessCode, statusRejected);
                    }
                    else if (pageAccessCode == PageAccessCode.ADMIN || pageAccessCode == PageAccessCode.CUSTOMERADMIN)
                    {
                        objOrderDetails = GetOrders_AdminSupAdmin(dbContext, pageAccessCode, statusRejected);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objOrderDetails;
        }


        /// <summary>
        /// Delete Customer Document Record
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool RemoveOrder(OrderModel Obj)
        {
            OrderModel returnRecord = new OrderModel();

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if ((Obj.OrderId) != 0)
                    {

                        var record = dbContext.MachineOrders.FirstOrDefault(cond => cond.Order_ID == Obj.OrderId);
                        if (record != null)
                        {


                            dbContext.MachineOrders.Remove(record);
                          
                        }

                    }
                    // save the changes
                    dbContext.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;

        }

        /// <summary>
        /// If User is Client Exec get all orders
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageAccessCode"></param>
        /// <param name="statusRejected"></param>
        /// <returns></returns>
        private List<OrderModel> GetOrders_ClientExec(ReviveDBEntities dbContext, string pageAccessCode, int statusRejected)
        {
            List<OrderModel> orders = new List<OrderModel>();
            orders = dbContext.usp_GetOrders().Where(d => (d.Archive_flag == false && d.JobOrder_Status_Id != statusRejected))
                .Select(d => BindOrderModel(pageAccessCode, d)).ToList();
            if (orders.Count > 0)
            {
                orders = orders.OrderByDescending(ord => ord.StatusName == OrderStatusType.InProgress)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.PendingApproval)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.PendingPC)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.Shipped).ToList();
            }
            return orders;
        }
        /// <summary>
        /// If User is Account Manager get orders created by him
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageAccessCode"></param>
        /// <param name="statusRejected"></param>
        /// <returns></returns>
        private List<OrderModel> GetOrders_AccntMgr(ReviveDBEntities dbContext, string pageAccessCode, int statusRejected, Guid userGuid)
        {
            // Display orders of all customers under logged in Accnt Manager (any customer active or inactive)
            var custUnderAM = from x in dbContext.Customers
                              where x.AccountmanagerUserID == userGuid
                              select x.Customer_ID;

            List<OrderModel> orders = new List<OrderModel>();
            orders = dbContext.usp_GetOrders().Where(d => (d.Archive_flag == false && d.JobOrder_Status_Id != statusRejected && custUnderAM.Contains(d.Customer_ID)))
                .Select(d => BindOrderModel(pageAccessCode, d)).ToList();
            if (orders.Count > 0)
            {
                orders = orders.OrderByDescending(ord => ord.StatusName == OrderStatusType.InProgress)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.PendingApproval)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.PendingPC)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.Shipped).ToList();
            }
            return orders;
        }
        /// <summary>
        /// If user is Approver, get all orders
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageAccessCode"></param>
        /// <param name="statusRejected"></param>
        /// <returns></returns>
        private List<OrderModel> GetOrders_Approver(ReviveDBEntities dbContext, string pageAccessCode, int statusRejected)
        {
            List<OrderModel> orders = new List<OrderModel>();
            orders = dbContext.usp_GetOrders().Where(d => (d.Archive_flag == false && d.JobOrder_Status_Id != statusRejected))
                .Select(d => BindOrderModel(pageAccessCode, d)).ToList();
            if (orders.Count > 0)
            {
                orders = orders.OrderByDescending(ord => ord.StatusName == OrderStatusType.PendingApproval)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.InProgress)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.PendingPC)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.Shipped).ToList();
            }
            return orders;
        }
        /// <summary>
        /// If user is MFPC, get orders with status Pending PC/In-Progress/Shipped
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageAccessCode"></param>
        /// <param name="statusRejected"></param>
        /// <returns></returns>
        private List<OrderModel> GetOrders_MFPC(ReviveDBEntities dbContext, string pageAccessCode, int statusRejected, int manufac_ID)
        {
            List<OrderModel> orders = null;
            int pendingPC = GetOrderStatusId(OrderStatusType.PendingPC);
            int inProgress = GetOrderStatusId(OrderStatusType.InProgress);
            int shipped = GetOrderStatusId(OrderStatusType.Shipped);

            orders = dbContext.usp_GetOrders().Where(d => (d.Archive_flag == false && d.JobOrder_Status_Id != statusRejected &&
                 d.Manufacturer_Id == manufac_ID &&
                (d.JobOrder_Status_Id == pendingPC || d.JobOrder_Status_Id == inProgress || d.JobOrder_Status_Id == shipped)
                )).Select(d => BindOrderModel(pageAccessCode, d)).ToList();

            if (orders.Count > 0)
            {
                orders = orders.OrderByDescending(ord => ord.StatusName == OrderStatusType.PendingPC)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.InProgress)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.Shipped).ToList();
            }
            return orders;
        }
        /// <summary>
        /// MFAssembly user, with order status => In-Progress, Shipped, Closed
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageAccessCode"></param>
        /// <param name="statusRejected"></param>
        /// <returns></returns>
        private List<OrderModel> GetOrders_MFAssembly(ReviveDBEntities dbContext, string pageAccessCode, int statusRejected, int manufac_ID)
        {
            List<OrderModel> orders = new List<OrderModel>();
            // used as of SR35.4
            int inProgress = GetOrderStatusId(OrderStatusType.InProgress);
            int shipped = GetOrderStatusId(OrderStatusType.InProgress);
            //int closed = GetOrderStatusId(OrderStatusType.InProgress);
            orders = dbContext.usp_GetOrders().Where(d => (d.Archive_flag == false && d.JobOrder_Status_Id != statusRejected &&
                        d.Manufacturer_Id == manufac_ID &&
                        (d.JobOrder_Status_Id == inProgress || d.JobOrder_Status_Id == shipped)))
                        .Select(d => BindOrderModel(pageAccessCode, d)).ToList();
            if (orders.Count > 0)
            {
                orders = orders.OrderByDescending(ord => ord.StatusName == OrderStatusType.InProgress)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.Shipped).ToList();
            }
            return orders;
        }
        /// <summary>
        /// MF-Shipping user, get only orders with status In-Progress or Shipped and machine status is In-Shipping or Shipped
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private List<OrderModel> GetOrders_MFShip(ReviveDBEntities dbContext, string pageAccessCode, int statusRejected, int manufac_ID)
        {
            List<OrderModel> orders;
            string orderStatus = OrderStatusType.InProgress + "," + OrderStatusType.Shipped;
            string machineStatus = OrderStatusType.InShipping + "," + OrderStatusType.Shipped;
            orders = dbContext.usp_GetOrders_MFShip(machineStatus, orderStatus).Where(d => (d.Archive_flag == false && d.JobOrder_Status_Id != statusRejected && d.Manufacturer_Id == manufac_ID))
                .Select(d => new OrderModel()
            {
                CustomerId = d.Customer_ID,
                CustomerName = d.Customer_Name,
                JobOrderHeaderId = d.JobOrder_Header_ID,
                JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(d.JobOrder_Header_ID)),
                JobOrderStatusId = d.JobOrder_Status_Id,
                StatusName = d.Status_Name,
                CustomerPONumber = d.CustomerPoNumber,
                //MachineSpecs = d.MachineSpecs,
                MachineSpecsId = d.MachineSpec_Id,
                ApproverComments = d.Approver_notes,
                ClientExecComments = d.Client_Exec_Notes_Instructions,
                ManufacturerComments = d.ManufacturerComments,
                ManufacturerId = d.Manufacturer_Id,
                ManufacturerName = d.Manufacturer_Name,
                NoOfMachines = d.Noof_Machines,
                ExpectedDate = d.Expected_Delivery_Date,
                CreatedDate = d.Created_Date,
                ArchiveFlag = (bool)d.Archive_flag,
                DownloadSWUrl = d.Download_Software_URL,
                PageAccessCode = pageAccessCode
            }).ToList();
            if (orders.Count > 0)
            {
                orders = orders.OrderByDescending(ord => ord.StatusName == OrderStatusType.InProgress)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.Shipped).ToList();
            }
            return orders;
        }
        /// <summary>
        /// If User is Admin or Super Admin, get all orders
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageAccessCode"></param>
        /// <param name="statusRejected"></param>
        /// <returns></returns>
        private List<OrderModel> GetOrders_AdminSupAdmin(ReviveDBEntities dbContext, string pageAccessCode, int statusRejected)
        {
            List<OrderModel> orders = new List<OrderModel>();
            orders = dbContext.usp_GetOrders().Where(d => (d.Archive_flag == false && d.JobOrder_Status_Id != statusRejected))
                .Select(d => BindOrderModel(pageAccessCode, d)).ToList();
            if (orders.Count > 0)
            {
                orders = orders.OrderByDescending(ord => ord.StatusName == OrderStatusType.Closed)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.Shipped)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.InProgress)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.PendingPC)
                    .ThenByDescending(ord => ord.StatusName == OrderStatusType.PendingApproval).ToList();
            }
            return orders;
        }
        /// <summary>
        /// Bind order model with result set got from usp_GetOrders Stored procedure
        /// </summary>
        /// <param name="pageAccessCode"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static OrderModel BindOrderModel(string pageAccessCode, usp_GetOrders_Result d)
        {
            return new OrderModel()
            {
                CustomerId = d.Customer_ID,
                CustomerName = d.Customer_Name,
                JobOrderHeaderId = d.JobOrder_Header_ID,
                JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(d.JobOrder_Header_ID)),
                JobOrderStatusId = d.JobOrder_Status_Id,
                StatusName = d.Status_Name,
                CustomerPONumber = d.CustomerPoNumber,
                //MachineSpecs = d.MachineSpecs,
                MachineSpecsId = d.MachineSpec_Id,
                ApproverComments = d.Approver_notes,
                ClientExecComments = d.Client_Exec_Notes_Instructions,
                ManufacturerComments = HttpUtility.HtmlDecode(d.ManufacturerComments),
                ManufacturerId = d.Manufacturer_Id,
                ManufacturerName = d.Manufacturer_Name,
                NoOfMachines = d.Noof_Machines,
                ExpectedDate = d.Expected_Delivery_Date,
                CreatedDate = d.Created_Date,
                ArchiveFlag = (bool)d.Archive_flag,
                DownloadSWUrl = d.Download_Software_URL,
                PageAccessCode = pageAccessCode
            };
        }

        #endregion

        /// <summary>
        /// Get Order data By order id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderModel GetOrderById(int id)
        {
            var result = new OrderModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var record = (from usp_GetOrderByID_Result spResult in dbContext.usp_GetOrderByID(id)
                                  where spResult.JobOrder_Header_ID == id
                                  select new OrderModel
                                {
                                    CustomerName = spResult.Customer_Name,
                                    CustomerId = spResult.Customer_ID,
                                    CustomerPONumber = spResult.CustomerPoNumber,
                                    CustomerDocName = spResult.CustomerDocName,
                                    NoOfMachines = spResult.Noof_Machines,
                                    ExpectedDate = spResult.Expected_Delivery_Date,
                                    ClientExecComments = spResult.Client_Exec_Notes_Instructions,
                                    ApproverComments = spResult.Approver_notes,
                                    ManufacturerComments = HttpUtility.HtmlDecode(spResult.ManufacturerComments),
                                    ManufacturerId = spResult.Manufacturer_Id,
                                    ManufacturerName = spResult.Manufacturer_Name,
                                    MachineSpecsId = spResult.MachineSpec_Id,
                                    MachineSpecs = spResult.MachineSpec,
                                    DownloadSWUrl = spResult.Download_Software_URL,
                                    MachineSpecGeneration = spResult.MachineSpec,
                                    JobOrderHeaderId = spResult.JobOrder_Header_ID,
                                    JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(spResult.JobOrder_Header_ID)),
                                    ApprovedRejectedUserName = GetUserName(spResult.Approved_Rejected_by_Userid),
                                    StatusName = spResult.Status_Name,
                                    ApprovedRejectedDate = Convert.ToDateTime(spResult.Approved_rejected_Date).ToString("MM/dd/yyyy"),
                                    NoOfMachinesDelivered = spResult.NoOfMachinesDelivered != null ? (int)spResult.NoOfMachinesDelivered : 0,
                                    CreatedByUserId = spResult.Created_by,
                                    softwareVersionId = (int)spResult.SoftwareVersionId,
                                    AccountManagerName = spResult.AccountManagerName,
                                    SubsidiaryId = spResult.Subsidiary_ID == null ? 0 : (int)spResult.Subsidiary_ID,
                                    SubAgentId = spResult.SubAgent_ID == null ? 0 : (int)spResult.SubAgent_ID,
                                    NoOfMachinesMapped =  (int)spResult.RemainingMachinesToBeMapped,
                                }).FirstOrDefault();


                    result = record;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        //raise a new exception inserting the current one as the InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return result;
        }

        /// <summary>
        /// Get Order Status Id using statusName
        /// </summary>
        /// <param name="statusName"></param>
        /// <returns></returns>
        public int GetOrderStatusId(string statusName)
        {
            int StatusId = 0;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    StatusId = (from JobOrder_Status order in dbContext.JobOrder_Status
                                where order.Status_Name == statusName
                                select order.JobOrder_Status_Id).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return StatusId;
        }

        /// <summary>
        /// Update Order Status approve/reject
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string UpdateOrderStatus(OrderModel order)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recordToUpdate = dbContext.JobOrder_header.FirstOrDefault(c => c.JobOrder_Header_ID == order.JobOrderHeaderId);
                    if (recordToUpdate != null)
                    {
                        recordToUpdate.Approver_notes = order.ApproverComments;
                        recordToUpdate.JobOrder_Status_Id = (int)order.JobOrderStatusId;
                        recordToUpdate.Approved_rejected_Date = DateTime.Now;
                        recordToUpdate.Approved_Rejected_by_Userid = order.ApprovedRejectedUserId;
                        recordToUpdate.Modified_by = order.ModifiedByUserId;
                        recordToUpdate.Modified_Date = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Archive Orders list which can be archived
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderModel> GetArchiveOrders(string pageAccessCode)
        {
            try
            {
                IEnumerable<OrderModel> objArchiveOrders;
                using (var dbContext = new ReviveDBEntities())
                {
                    int statusPendingApproval = GetOrderStatusId(OrderStatusType.PendingApproval);
                    int statusRejected = GetOrderStatusId(OrderStatusType.Rejected);
                    objArchiveOrders = dbContext.usp_GetOrders().Where(d => (d.JobOrder_Status_Id == statusPendingApproval || d.JobOrder_Status_Id == statusRejected)).Select(d => new OrderModel()
                    {
                        CustomerId = d.Customer_ID,
                        CustomerName = d.Customer_Name,
                        JobOrderHeaderId = d.JobOrder_Header_ID,
                        JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(d.JobOrder_Header_ID)),
                        JobOrderStatusId = d.JobOrder_Status_Id,
                        StatusName = d.Status_Name,
                        CustomerPONumber = d.CustomerPoNumber,
                        //MachineSpecs = d.MachineSpecs,
                        MachineSpecsId = d.MachineSpec_Id,
                        ApproverComments = d.Approver_notes,
                        ClientExecComments = d.Client_Exec_Notes_Instructions,
                        ManufacturerComments = HttpUtility.HtmlDecode(d.ManufacturerComments),
                        ManufacturerId = d.Manufacturer_Id,
                        ManufacturerName = d.Manufacturer_Name,
                        NoOfMachines = d.Noof_Machines,
                        ExpectedDate = d.Expected_Delivery_Date,
                        CreatedDate = d.Created_Date,
                        ArchiveFlag = (bool)d.Archive_flag,
                        DownloadSWUrl = d.Download_Software_URL,
                        PageAccessCode = pageAccessCode
                    }).OrderBy(o => o.ArchiveFlag == true).ToList();// Descending(o => o.StatusName.Equals(OrderStatusType.PendingApproval)).ToList();
                }
                return objArchiveOrders;
            }
            catch (Exception ex)
            {
                //logService.LogError("Error in order management  repository GetArchiveOrders := ", ex);
                throw;
            }
        }
        public void SetArchiveStatus(int orderId)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recordToUpdate = dbContext.JobOrder_header.FirstOrDefault(c => c.JobOrder_Header_ID == orderId);
                    if (recordToUpdate != null)
                    {
                        recordToUpdate.Archive_flag = true;
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in order management  repository SetArchiveStatus := ", ex);
                throw;
            }
        }

        public void AssignOrder(OrderModel order)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recordToUpdate = dbContext.JobOrder_header.FirstOrDefault(c => c.JobOrder_Header_ID == order.JobOrderHeaderId);
                    if (recordToUpdate != null)
                    {
                        recordToUpdate.Expected_Delivery_Date = order.ExpectedDate;
                        recordToUpdate.JobOrder_Status_Id = GetOrderStatusId(OrderStatusType.InProgress);
                        recordToUpdate.ManufacturerComments = HttpUtility.HtmlEncode(order.ManufacturerComments);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.LogError("Error in order management  repository AssignOrder := ", ex);
                throw;
            }
        }

        private string GetUserName(Nullable<Guid> uId)
        {
            string userName = string.Empty;
            if (uId.HasValue)
            {
                Guid userId = uId.Value;
                UserManagementRepository userDetails = new UserManagementRepository();
                UserModels user = userDetails.GetUserById(userId);
                if (!string.IsNullOrEmpty(user.FirstName))
                    userName += user.FirstName + " ";
                if (!string.IsNullOrEmpty(user.LastName))
                    userName += user.LastName + " ";
                userName = userName.Trim();
            }
            return userName;
        }


        public IEnumerable<OrderLocationsModel> GetOrderLocationsByOrderId(int orderId)
        {
            try
            {
                IEnumerable<OrderLocationsModel> locations;
                using (var dbContext = new ReviveDBEntities())
                {
                    locations = dbContext.usp_GetOrderLocations(orderId).Select(d => new OrderLocationsModel()
                                {
                                    JobOrderLocationId = d.JobOrder_Location_Id,
                                    JobOrderId = (int)d.JobOrder_Header_ID,
                                    CustomerId = d.Customer_ID,
                                    LocationId = d.Location_ID,
                                    LocationName = d.Location_Name,
                                    Priority = d.Priority,
                                    NumberOfMachines = d.No_of_Machines,
                                    CreatedBy = d.Created_By,
                                    CreatedDate = d.Created_Date,
                                    IsChecked = d.No_of_Machines == 0 ? false : true,
                                    IsMapped =  false,
                                    TotalShipped_Inshpping = d.TotalShipped_Inshpping,
                                    NumberOfMachinesMapped = d.No_of_machine_mapped

                                }).ToList();
                }
                return locations;
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in order management  repository GetOrderLocationsByOrderId := ", ex);
                throw;
            }
        }
        public IEnumerable<OrderLocationsModel> GetCustomerLocations(int custId, int subsidiaryId, int subagentId)
        {
            try
            {
                var locations = new List<OrderLocationsModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Customers_Locations
                                         where d.Customer_ID == custId && d.Status == true && d.Subsidiary_ID == subsidiaryId && d.SubAgent_ID == subagentId// d.Status == true "Condition added by Sandeep to show active location only"
                                         select new OrderLocationsModel
                                         {
                                             CustomerId = d.Customer_ID,
                                             LocationId = d.Location_ID,
                                             LocationName = d.Location_Name,
                                             SubsidiaryID = d.Subsidiary_ID == null ? 0 : (int)d.Subsidiary_ID,
                                             SubsidiaryName = dbContext.Subsidiaries.FirstOrDefault(c => (c.Subsidiary_ID == d.Subsidiary_ID)).SubsidiaryName,
                                             SubAgentID = d.SubAgent_ID == null ? 0 : (int)d.Subsidiary_ID,
                                             SubAgentName = dbContext.SubAgents.FirstOrDefault(c => (c.SubAgent_ID == d.SubAgent_ID)).SubAgentName,
                                             Priority = 1,      // Setting default value for create order screen
                                             IsChecked = false  // Setting default value for create order screen
                                         }).ToList();
                    locations.AddRange(lstcollection);
                }
                return locations;
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in order management  repository GetCustomerLocations := ", ex);
                throw;
            }
        }

        public string GetDownloadSoftwareUrl(int machSpecId)
        {
            string url = string.Empty;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    int? firmwareVer = dbContext.MachineSpecs.FirstOrDefault(c => c.MachineSpec_ID == machSpecId).Firmware_Version;
                    if (firmwareVer != null && firmwareVer != 0)
                    {
                        url = dbContext.MasterData_Config_definitions.FirstOrDefault(c => c.MasterData_Type_ID == firmwareVer).Custom_Field1;
                    }
                    else
                        url = "#";
                }
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in order management  repository GetCustomerLocations := ", ex);
                throw;
            }
            return url;
        }

        /// <summary>
        /// Point 3 of CR implemented.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<MappedMachsLocs> APIGetMachineIdLocsList(string OrderIDList)
        {
            try
            {
                List<MappedMachsLocs> mappedMachsLocs;
                using (var dbContext = new ReviveDBEntities())
                {
                    mappedMachsLocs = dbContext.usp_GetMachineIdLocsListByOrderId(OrderIDList).Select(d => new MappedMachsLocs()
                    {
                        MachineID = d.MachineId,
                        Location = d.Location,
                        OrderID = (int)d.OrderID,
                        Address1 = d.Address1,
                        Address2 = d.Address2,
                        Zip = d.Zip,
                        City = d.City,
                        State = d.State,
                        Phone = d.Phone,
                        Email = d.Email_Id,
                        AttentionName = d.AttentionName
                    }).ToList();
                }
                return mappedMachsLocs;
            }
            catch (Exception ex)
            {
                // logService.LogError("Error in order management  repository APIGetMachineIdLocsList := ", ex);
                throw;
            }
        }
    }
}
