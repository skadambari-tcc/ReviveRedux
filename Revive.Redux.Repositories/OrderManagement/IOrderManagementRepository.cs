using Revive.Redux.Entities;
using System;
using System.Collections.Generic;

namespace Revive.Redux.Repositories
{
    public interface IOrderManagementRepository
    {
        string CreateOrder(OrderModel order, List<OrderLocationsModel> orderLocations);
        string CreatemachineOrder(OrderModel order);
        IEnumerable<OrderModel> GetOrdersList(string pageAccessCode, Guid userGuid, int manufac_ID);
        IEnumerable<OrderModel> GetMachineOrdersList();
        OrderModel GetOrderById(int ID);
        int GetOrderStatusId(string statusName);
        string UpdateOrderStatus(OrderModel order);
        IEnumerable<OrderModel> GetArchiveOrders(string pageAccessCode);
        void SetArchiveStatus(int orderId);
        void AssignOrder(OrderModel order);
        IEnumerable<OrderLocationsModel> GetOrderLocationsByOrderId(int orderId);
        string UpdateOrderLocations(OrderModel model, List<OrderLocationsModel> orderLocations);
        IEnumerable<OrderLocationsModel> GetCustomerLocations(int custId, int subsidiaryId, int subagentId);
        string GetDownloadSoftwareUrl(int machSpecId);
        List<MappedMachsLocs> APIGetMachineIdLocsList(string OrderIDList);
        bool RemoveOrder(OrderModel Obj);
    }
}
