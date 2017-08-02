using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public interface IManageShippingService
    {
        ShipMachineModel GetLocationToMapByOrderId();
        List<ShipMachineModel> GetShippingQueueDetails();
        ShippingConfigModel AddShippingConfiguration(ShippingConfigModel ViewRecord);
        List<ShippingConfigModel> GetShippingConfigurations();
        ShippingConfigModel GetShippingConfigurationById(int Id);
        ShippingConfigModel UpdateShippingConfiguration(ShippingConfigModel ViewRecord);
        UPSSettings GetUPSSettings();
        UPSSettings UpdateUPSSettings(UPSSettings ViewRecord);
        bool UpdShippingStatus(List<LocationList> LocationList, int LocationStatusId, int GroupId, Guid CreatedBy);
        List<GroupedLocationForShip> GetGroupdLocationForMapByGroupId(int GroupId);
        bool UpdateNoOfMachinesByGroupLocation(List<LocationList> LocationList, int _GroupId, Guid CreatedBy);
        List<ShipMachineModel> GetShippableCountByCustomerGroup();
    }
}
