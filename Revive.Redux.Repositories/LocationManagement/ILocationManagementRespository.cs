using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux;
using Revive.Redux.Entities;

namespace Revive.Redux.Repositories
{
    public interface ILocationManagementRespository
    {
        bool AddEditLocation(LocationModel objLocation);
        List<CustomerLocationModel> GetCustomerLocationsById(int CustomerId);
        List<CustomerLocationModel> GetCustomerLocationsById(int CustomerId, Guid AccountManagerId);
        List<CustomerLocationModel> GetCustomerLocationsBySubsidiaryAdminUser(Guid SubsidiaryAdminUserId);
        List<CustomerLocationModel> GetCustomerLocationsBySubAgentAdminUser(Guid SubAgentAdminUserId);
        CustomerLocationModel GetCustomerLocation(int nLocationId);
        int GetCityIdByCityName(string sCityName, int nStateId);
        int GetStateIdByStateName(string sStateName);
        bool CheckDuplicateStoreNo(string storeNo, int custID, int locID);
        bool CheckDuplicateCustID(string custRefCode, String custID);
        int GetLocationIdByLocationName(string _locationName,int CustomerId);
        int GetLocationIdByStoreNumber(string _StoreNumber, int CustomerId);
        List<CustomerLocationModel> GetCustomerLocations(CustomerLocationModel objCustomerLocation, Guid UserId, string pageAccessCode);
       
    }
}
