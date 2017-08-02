using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public interface IOrderManagementService
    {
        string CreateOrder(OrderModel order, List<OrderLocationsModel> orderLocations);
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
        string CreatemachineOrder(OrderModel order);
        bool RemoveOrder(OrderModel Obj);
    }
}
