using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public class ManageShippingRepository : IManageShippingRepository
    {
        public ShipMachineModel GetLocationToMapByOrderId()
        {
            try
            {
                var LocationToMap = new ShipMachineModel();
                using (var dbContext = new ReviveDBEntities())
                {
                    LocationToMap = dbContext.usp_GetGroupLocationToBeShipByPriority().Select(d => new ShipMachineModel
                    {
                        GroupId = d.GroupId,
                        GroupName = d.GroupName,
                        MachineCount = (int)d.No_of_Machines > 0 ? (int)d.No_of_Machines : 0,
                        TotalMachineMapped = d.TotalMachineMapped,
                        Location_Id = d.Location_ID,
                        LocationName = d.Location_Name,
                        LocationAddress = d.LocationAddress,
                        City_Name = d.LocationCityName,
                        ZipCode = d.CityZipCode,
                        StateCode = d.StateCode,
                        LocationPhone = d.Phone,
                        SubAgentName=d.SubAgentName,
                        LocationAddress2=d.LocationAddress2

                    }).FirstOrDefault();
                }

                return LocationToMap;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<ShipMachineModel> GetShippingQueueDetails()
        {
            try
            {
                var ShippingQueueDetails = new List<ShipMachineModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    ShippingQueueDetails = dbContext.usp_GetShippedDetails().Select(d => new ShipMachineModel
                    {

                        GroupName = d.GroupName,
                        CustomerName = d.Customer_Name,
                        NoOfMachinesOrdered = (int)d.No_of_Machines,
                        TotalShipped = (int)d.TotalShipped,
                        TotalReadyToShip = (int)d.TotalReadyToShip,
                        Status = d.status,
                        BeginShippingDate = d.Created_Date != null ? d.Created_Date : null,
                        SubsidiaryName = d.subsidiaryName.ToString(),
                        SubAgentName = d.SubAgentName,
                        GroupId = (int)d.GroupId,
                        Grp_No_of_Machines = d.Grp_No_of_Machines,
                        Grp_TotalShipped = d.Grp_TotalShipped,
                        Grp_TotReadyToShip = d.Grp_TotReadyToShip,
                        PriorityName=d.PriorityName

                    }).ToList();
                }
                return ShippingQueueDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ShipMachineModel> GetShippableCountByCustomerGroup()
        {
            try
            {
                var ShippingQueueDetails = new List<ShipMachineModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    ShippingQueueDetails = dbContext.usp_GetShippableDetailsByCustomerGroups().Select(d => new ShipMachineModel
                    {

                        GroupName = d.GroupName,
                        CustomerName = d.Customer_Name,
                        NoOfMachinesOrdered = (int)d.No_of_Machines,
                        TotalShipped = (int)d.TotalShipped,
                        TotalReadyToShip = (int)d.TotalReadyToShip,
                        Status = d.status,
                        BeginShippingDate = d.Created_Date != null ? d.Created_Date : null,
                        GroupId = (int)d.GroupId,
                        PriorityName = d.PriorityName

                    }).ToList();
                }
                return ShippingQueueDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ShipMachineModel GetShippingQueueDetailsByGroupId(int GroupId)
        {
            try
            {
                var ShippingQueueDetails = GetShippingQueueDetails(); ;
                var GroupStatus = ShippingQueueDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();

                return GroupStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<ShippingConfigModel> GetShippingConfigurations()
        {
            try
            {
                var ShippingConfigurations = new List<ShippingConfigModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    ShippingConfigurations = dbContext.ShippingConfigurations.Select(d => new ShippingConfigModel
                    {

                        Id = d.Id,
                        ServiceTypeId = d.ServiceTypeId,
                        PkgHeight = (int)d.PkgHeight,
                        PkgWidth = (int)d.PkgWidth,
                        PkgLength = (int)d.PkgLength,
                        MachineIdentifier = d.MachineIdentifier,
                        Created_Date = d.Created_Date,
                        InsuranceAmount = d.InsuranceAmount,
                        PkgWt = (int)d.PkgWt,

                    }).ToList();
                }
                return ShippingConfigurations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public UPSSettings GetUPSSettings()
        {
            try
            {
                var UPSSetting = new UPSSettings();
                using (var dbContext = new ReviveDBEntities())
                {
                    UPSSetting = dbContext.UPSSettings.Select(d => new UPSSettings
                    {

                        Id = d.Id,
                        UserName = d.UserName,
                        Psw = d.Psw,
                        ShipmentDescription = d.ShipmentDescription,
                        ShipperNumber = d.ShipperNumber,
                        AccountNumber = d.AccountNumber,
                        ShipperAddressLine = d.ShipperAddressLine,
                        ShipperCity = d.ShipperCity,
                        ShipperPostalCode = (int)d.ShipperPostalCode,
                        ShipperStateProvinceCode = d.ShipperStateProvinceCode,
                        ShipperCountryCode = d.ShipperCountryCode,
                        ShipperName = d.ShipperName,
                        ShipperAttentionName = d.ShipperAttentionName,
                        ShipperPhone = d.ShipperPhone,
                        ShipFromCity = d.ShipFromCity,
                        ShipFromAddressLine = d.ShipFromAddressLine,
                        ShipFromPostalCode = (int)d.ShipFromPostalCode,
                        ShipFromStateProvinceCode = d.ShipFromStateProvinceCode,
                        ShipFromCountryCode = d.ShipFromCountryCode,
                        ShipFromAttentionName = d.ShipFromAttentionName,
                        ShipFromName = d.ShipFromName,
                        ShipmentChargeType = d.ShipmentChargeType,
                        ServiceCode = d.ServiceCode
                    }).FirstOrDefault();
                }
                return UPSSetting;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public UPSSettings UpdateUPSSettings(UPSSettings ViewRecord)
        {

            UPSSettings ReturnRecords = new UPSSettings();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (ViewRecord.Id != 0)
                    {
                        var Record = dbContext.UPSSettings.FirstOrDefault(cond => cond.Id == ViewRecord.Id);

                        if (Record != null)
                        {

                            Record = MapUPSModel(ViewRecord, Record);
                            Record.Modified_Date = DateTime.Now;
                        }

                        dbContext.SaveChanges();
                        ReturnRecords.Id = Record.Id;
                    }
                    else
                    {
                        var Record = new UPSSetting();
                        Record = MapUPSModel(ViewRecord, Record);
                        Record.Created_Date = DateTime.Now;
                        dbContext.UPSSettings.Add(Record);
                        dbContext.SaveChanges();
                        ReturnRecords.Id = Record.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return ReturnRecords;

        }

        private UPSSetting MapUPSModel(UPSSettings ViewRecord, UPSSetting Record)
        {

            Record.UserName = ViewRecord.UserName;
            Record.Psw = ViewRecord.Psw;
            Record.ShipmentDescription = ViewRecord.ShipmentDescription;
            Record.ShipperNumber = ViewRecord.ShipperNumber;
            Record.AccountNumber = ViewRecord.AccountNumber;
            Record.ShipperAddressLine = ViewRecord.ShipperAddressLine;
            Record.ShipperCity = ViewRecord.ShipperCity;
            Record.ShipperPostalCode = ViewRecord.ShipperPostalCode;
            Record.ShipperStateProvinceCode = ViewRecord.ShipperStateProvinceCode;
            Record.ShipperCountryCode = ViewRecord.ShipperCountryCode;
            Record.ShipperName = ViewRecord.ShipperName;
            Record.ShipperAttentionName = ViewRecord.ShipperAttentionName;
            Record.ShipperPhone = ViewRecord.ShipperPhone;
            Record.ShipFromAddressLine = ViewRecord.ShipFromAddressLine;
            Record.ShipFromCity = ViewRecord.ShipFromCity;
            Record.ShipFromPostalCode = ViewRecord.ShipFromPostalCode;
            Record.ShipFromStateProvinceCode = ViewRecord.ShipFromStateProvinceCode;
            Record.ShipFromCountryCode = ViewRecord.ShipFromCountryCode;
            Record.ShipFromAttentionName = ViewRecord.ShipFromAttentionName;
            Record.ShipFromName = ViewRecord.ShipFromName;
            Record.Modified_by = ViewRecord.CurrentUserId;
            Record.ShipmentChargeType = ViewRecord.ShipmentChargeType;
            Record.ServiceCode = ViewRecord.ServiceCode;
            return Record;
        }
        public ShippingConfigModel AddShippingConfiguration(ShippingConfigModel ViewRecord)
        {
            //var result = false;
            ShippingConfigModel ReturnRecords = new ShippingConfigModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var Record = new ShippingConfiguration();
                    Record.ServiceTypeId = ViewRecord.ServiceTypeId;
                    Record.PkgHeight = ViewRecord.PkgHeight;
                    Record.PkgLength = ViewRecord.PkgLength;
                    Record.PkgWidth = ViewRecord.PkgWidth;
                    Record.PkgWt = ViewRecord.PkgWt;
                    Record.MachineIdentifier = ViewRecord.MachineIdentifier;
                    Record.InsuranceAmount = ViewRecord.InsuranceAmount;
                    Record.IsActive = true;
                    Record.Created_Date = DateTime.Now;
                    Record.Created_by = ViewRecord.CurrentUserId;
                    dbContext.ShippingConfigurations.Add(Record);
                    dbContext.SaveChanges();
                    ReturnRecords.Id = Record.Id;

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return ReturnRecords;

        }
        public ShippingConfigModel UpdateShippingConfiguration(ShippingConfigModel ViewRecord)
        {
            ShippingConfigModel ReturnRecords = new ShippingConfigModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var Record = dbContext.ShippingConfigurations.FirstOrDefault(cond => cond.Id == ViewRecord.Id);

                    if (Record != null)
                    {

                        Record.ServiceTypeId = ViewRecord.ServiceTypeId;
                        Record.PkgHeight = ViewRecord.PkgHeight;
                        Record.PkgLength = ViewRecord.PkgLength;
                        Record.PkgWidth = ViewRecord.PkgWidth;
                        Record.PkgWt = ViewRecord.PkgWt;
                        Record.MachineIdentifier = ViewRecord.MachineIdentifier;
                        Record.InsuranceAmount = ViewRecord.InsuranceAmount;
                        Record.IsActive = true;
                        Record.Modified_Date = DateTime.Now;
                        Record.Modified_by = ViewRecord.CurrentUserId;
                    }

                    dbContext.SaveChanges();
                    ReturnRecords.Id = Record.Id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ReturnRecords;
        }

        public ShippingConfigModel GetShippingConfigurationById(int Id)
        {

            var result = new ShippingConfigModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var records = (from ShippingConfiguration x in dbContext.ShippingConfigurations.ToList()
                                   where x.Id == Convert.ToInt16(Id)
                                   select new ShippingConfigModel
                                   {
                                       Id = x.Id,
                                       ServiceTypeId = x.ServiceTypeId,
                                       PkgHeight = (int)x.PkgHeight,
                                       PkgLength = (int)x.PkgLength,
                                       PkgWidth = (int)x.PkgWidth,
                                       PkgWt = (int)x.PkgWt,
                                       MachineIdentifier = x.MachineIdentifier,
                                       InsuranceAmount = x.InsuranceAmount,
                                       IsActive = x.IsActive

                                   }).FirstOrDefault();

                    result = records;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool UpdShippingStatus(List<LocationList> LocationList, int LocationStatusId, int _GroupId, Guid CreatedBy)
        {
            var flag = false;
            DateTime ShippingDate;

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (LocationStatusId != 0 && _GroupId != 0 && LocationList.Count > 0)
                    {
                        ShippingDate = (DateTime)dbContext.LocationGroups.Where(x => x.GroupId == _GroupId).FirstOrDefault().ShippingDate;
                        for (int i = 0; i < LocationList.Count(); i++)
                        {
                            int _LocationId = Convert.ToInt32(LocationList[i].LocationId);
                            var recordExists = dbContext.MapLocationByGroups.Where(x => x.GroupId == _GroupId && x.Location_Id == _LocationId).FirstOrDefault();
                            if (recordExists == null)
                            {
                                MapLocationByGroup MapLocationByGroupObj = new MapLocationByGroup();
                                MapLocationByGroupObj.Location_Id = _LocationId;
                                MapLocationByGroupObj.GroupId = _GroupId;
                                MapLocationByGroupObj.DeploymentStatusId = LocationStatusId;
                                MapLocationByGroupObj.Created_Date = System.DateTime.Now;
                                MapLocationByGroupObj.ShippingDate = ShippingDate;
                                MapLocationByGroupObj.No_Of_Machines = LocationList[i].NoofMachines;
                                dbContext.MapLocationByGroups.Add(MapLocationByGroupObj);

                            }
                            else
                            {
                                recordExists.DeploymentStatusId = LocationStatusId;
                                recordExists.No_Of_Machines = LocationList[i].NoofMachines;
                            }

                        }
                        dbContext.SaveChanges();
                    }
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }

        public bool UpdateNoOfMachinesByGroupLocation(List<LocationList> LocationList, int _GroupId, Guid CreatedBy)
        {
            var flag = false;
            DateTime ShippingDate;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (_GroupId != 0 && LocationList.Count > 0)
                    {
                        ShippingDate = (DateTime)dbContext.LocationGroups.Where(x => x.GroupId == _GroupId).FirstOrDefault().ShippingDate;
                        for (int i = 0; i < LocationList.Count(); i++)
                        {
                            if (LocationList[i].ShippingStatusId == 0)
                            {
                                int _LocationId = Convert.ToInt32(LocationList[i].LocationId);
                                var recordExists = dbContext.MapLocationByGroups.Where(x => x.GroupId == _GroupId && x.Location_Id == _LocationId && x.ShippingDate == ShippingDate).FirstOrDefault();
                                int LocationStatusId = dbContext.MasterData_Config_definitions.FirstOrDefault(c => (c.MasterData_Type == "ShippingStatus" && c.MasterData_Value == "Ready To Deploy")).MasterData_Type_ID;
                                if (recordExists == null)
                                {
                                    MapLocationByGroup MapLocationByGroupObj = new MapLocationByGroup();
                                    MapLocationByGroupObj.Location_Id = _LocationId;
                                    MapLocationByGroupObj.GroupId = _GroupId;
                                    MapLocationByGroupObj.Created_Date = System.DateTime.Now;
                                    MapLocationByGroupObj.Created_by = CreatedBy;
                                    MapLocationByGroupObj.No_Of_Machines = LocationList[i].NoofMachines > 0 ? (int)LocationList[i].NoofMachines : 1;
                                    MapLocationByGroupObj.ShippingDate = ShippingDate;
                                    MapLocationByGroupObj.DeploymentStatusId = LocationStatusId;
                                    dbContext.MapLocationByGroups.Add(MapLocationByGroupObj);
                                }
                                else
                                {
                                    recordExists.Modified_Date = System.DateTime.Now;
                                    recordExists.Modified_by = CreatedBy;
                                    recordExists.No_Of_Machines = LocationList[i].NoofMachines > 0 ? (int)LocationList[i].NoofMachines : 1;
                                }
                            }
                        }
                        dbContext.SaveChanges();
                    }
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }

        public List<GroupedLocationForShip> GetGroupdLocationForMapByGroupId(int GroupId)
        {
            try
            {
                var GroupedLocationForShip = new List<GroupedLocationForShip>();
                using (var dbContext = new ReviveDBEntities())
                {
                    GroupedLocationForShip = dbContext.usp_GetGroupedLocationForShipByGroupId(GroupId).Select(d => new GroupedLocationForShip
                    {

                        GroupName = d.GroupName,
                        CustomerName = d.Customer_Name,
                        SubsidiaryName = d.subsidiaryName,
                        SubAgentName = d.SubAgentName,
                        NoofMachines = d.No_Of_Machines > 0 ? (int)d.No_Of_Machines : 1,
                        Location_Name = d.Location_Name,
                        StoreNumber = d.StoreNumber,
                        ShippingStatusName = d.ShippingStatusName,
                        Location_ID = d.Location_ID,
                        IsShipped = d.isShipped == 1 ? true : false,
                        ShippingDate = (DateTime)d.ShippingDate,
                        ShippingStatusId = d.shipping_status

                    }).ToList();
                }
                return GroupedLocationForShip;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
