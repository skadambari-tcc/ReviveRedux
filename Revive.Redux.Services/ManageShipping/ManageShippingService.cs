using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public class ManageShippingService : IManageShippingService
    {
        IManageShippingRepository manageShippingRepository = null;
        public ManageShippingService()
        {
            manageShippingRepository = new ManageShippingRepository();
        }
        public ShipMachineModel GetLocationToMapByOrderId()
        {
            return manageShippingRepository.GetLocationToMapByOrderId();
        }
        public List<ShipMachineModel> GetShippingQueueDetails()
        {
            return manageShippingRepository.GetShippingQueueDetails();
        }
        public ShippingConfigModel AddShippingConfiguration(ShippingConfigModel ViewRecord)
        {
            return manageShippingRepository.AddShippingConfiguration(ViewRecord);
        }
        public List<ShippingConfigModel> GetShippingConfigurations()
        {
            return manageShippingRepository.GetShippingConfigurations();
        }

        public ShippingConfigModel GetShippingConfigurationById(int Id)
        {
            return manageShippingRepository.GetShippingConfigurationById(Id);
        }

        public ShippingConfigModel UpdateShippingConfiguration(ShippingConfigModel ViewRecord)
        {
            return manageShippingRepository.UpdateShippingConfiguration(ViewRecord);
        }
        public UPSSettings GetUPSSettings()
        {
            return manageShippingRepository.GetUPSSettings();
        }
        public UPSSettings UpdateUPSSettings(UPSSettings ViewRecord)
        {
            return manageShippingRepository.UpdateUPSSettings(ViewRecord);
        }
        public bool UpdShippingStatus(List<LocationList> LocationList, int LocationStatusId, int GroupId, Guid CreatedBy)
        {
            return manageShippingRepository.UpdShippingStatus(LocationList, LocationStatusId, GroupId, CreatedBy);
        }
        public List<GroupedLocationForShip> GetGroupdLocationForMapByGroupId(int GroupId)
        {
            return manageShippingRepository.GetGroupdLocationForMapByGroupId(GroupId);
        }
        public bool UpdateNoOfMachinesByGroupLocation(List<LocationList> LocationList, int _GroupId, Guid CreatedBy)
        {
            return manageShippingRepository.UpdateNoOfMachinesByGroupLocation(LocationList, _GroupId, CreatedBy);
        }
        public List<ShipMachineModel> GetShippableCountByCustomerGroup()
        {
            return manageShippingRepository.GetShippableCountByCustomerGroup();
        }
        
    } 
}
