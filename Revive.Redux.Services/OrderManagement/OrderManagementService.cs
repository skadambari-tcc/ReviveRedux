using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public class OrderManagementService : IOrderManagementService
    {
        private IOrderManagementRepository _IOrderManagementRepository = null;

        public OrderManagementService()
        {
            _IOrderManagementRepository = new OrderManagementRepository();
        }

        public string CreateOrder(OrderModel order, List<OrderLocationsModel> orderLocations)
        {
            return _IOrderManagementRepository.CreateOrder(order, orderLocations);
        }

        public IEnumerable<OrderModel> GetOrdersList(string pageAccessCode, Guid userGuid, int manufac_ID)
        {
            return _IOrderManagementRepository.GetOrdersList(pageAccessCode, userGuid, manufac_ID);
        }

        public OrderModel GetOrderById(int ID)
        {
            return _IOrderManagementRepository.GetOrderById(ID);
        }

        public int GetOrderStatusId(string statusName)
        {
            return _IOrderManagementRepository.GetOrderStatusId(statusName);
        }
        public string UpdateOrderStatus(OrderModel objOrder)
        {
            return _IOrderManagementRepository.UpdateOrderStatus(objOrder);
        }
        public IEnumerable<OrderModel> GetArchiveOrders(string pageAccessCode)
        {
            return _IOrderManagementRepository.GetArchiveOrders(pageAccessCode);
        }
        public void SetArchiveStatus(int orderId)
        {
            _IOrderManagementRepository.SetArchiveStatus(orderId);
        }
        public void AssignOrder(OrderModel order)
        {
            _IOrderManagementRepository.AssignOrder(order);
        }

        public IEnumerable<OrderLocationsModel> GetOrderLocationsByOrderId(int orderId)
        {
            return _IOrderManagementRepository.GetOrderLocationsByOrderId(orderId);
        }
        public string UpdateOrderLocations(OrderModel model, List<OrderLocationsModel> orderLocations)
        {
            return _IOrderManagementRepository.UpdateOrderLocations(model, orderLocations);
        }
        public IEnumerable<OrderLocationsModel> GetCustomerLocations(int custId, int subsidiaryId, int subagentId)
        {
            return _IOrderManagementRepository.GetCustomerLocations(custId, subsidiaryId, subagentId);
        }

        public string GetDownloadSoftwareUrl(int machSpecId)
        {
            return _IOrderManagementRepository.GetDownloadSoftwareUrl(machSpecId);
        }
        public List<MappedMachsLocs> APIGetMachineIdLocsList(string OrderIDList)
        {
            return _IOrderManagementRepository.APIGetMachineIdLocsList(OrderIDList);
        }

        public string CreatemachineOrder(OrderModel order)
        {
            return _IOrderManagementRepository.CreatemachineOrder(order);
        }
        public IEnumerable<OrderModel> GetMachineOrdersList()
        {
            return _IOrderManagementRepository.GetMachineOrdersList();
        }
        public bool RemoveOrder(OrderModel Obj)
        {
            return _IOrderManagementRepository.RemoveOrder(Obj);
        }
        
    }
}
