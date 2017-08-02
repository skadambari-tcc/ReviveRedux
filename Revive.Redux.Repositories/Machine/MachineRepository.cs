using Kendo.Mvc.UI;
using Revive.Redux.Common;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
namespace Revive.Redux.Repositories
{
    public class MachineRepository : IMachineRepository
    {
        public MachineRepository()
        {

        }

        /// <summary>
        /// Get machines which has machine status- In-Shipping or Shipped.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        public IEnumerable<MachineModel> GetMachinesByOrderIdForShipping(int id)
        {
            try
            {
                IEnumerable<MachineModel> machineModel = new List<MachineModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    // Get machines which has machine status- In-Shipping or Shipped.
                    int inShippingStatusId = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.InShipping).JobOrder_Status_Id;
                    int shippedStatusId = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.Shipped).JobOrder_Status_Id;
                    machineModel = dbContext.usp_GetMappedMachinesByOrderId(id).Where(c => (c.MachineStatusId == inShippingStatusId || c.MachineStatusId == shippedStatusId)).Select(d => new MachineModel()
                    {
                        JobOrderHeaderID = d.JobOrder_Header_ID,
                        JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(d.JobOrder_Header_ID)),
                        MachineMappingId = d.MachineMappingId,
                        MachineId = d.MachineId,
                        MachineId_Val = d.machineId_Val,
                        LocationId = d.LocationId == null ? 0 : d.LocationId,
                        Location = string.IsNullOrEmpty(d.Location) ? string.Empty : d.Location,
                        MidLineTest = d.MidLineTest,
                        ULTest = d.ULTest,
                        FinalTest = d.FinalTest,
                        ShippingId = d.ShippingId,
                        MachineStatusName = d.MachineStatusName,
                        JobOrderLocationId = d.JobOrder_Location_Id


                    }).ToList();
                }
                return machineModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Mapped Machines using OrderId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<MachineModel> GetMappedMachinesByOrderId(int id)
        {
            try
            {
                IEnumerable<MachineModel> machineModel;
                using (var dbContext = new ReviveDBEntities())
                {
                    machineModel = dbContext.usp_GetMappedMachinesByOrderId(id).Select(d => new MachineModel()
                    {
                        JobOrderHeaderID = d.JobOrder_Header_ID,
                        JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(d.JobOrder_Header_ID)),
                        MachineMappingId = d.MachineMappingId,
                        MachineMappingIdEncoded = CommonMethods.Encode(Convert.ToString(d.MachineMappingId)),
                        MachineId = d.MachineId,
                        MachineId_Val = d.machineId_Val,
                        LocationId = d.LocationId == null ? 0 : d.LocationId,
                        Location = string.IsNullOrEmpty(d.Location) ? string.Empty : d.Location,
                        MidLineTest = d.MidLineTest,
                        ULTest = d.ULTest,
                        FinalTest = d.FinalTest,
                        ShippingId = d.ShippingId,
                        MachineStatusName = d.MachineStatusName,
                        JobOrderLocationId = d.JobOrder_Location_Id
                    }).OrderByDescending(o => o.MachineStatusName.Equals(OrderStatusType.InTesting)).ToList();
                }
                return machineModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert Mapped Machine
        /// </summary>
        /// <param name="jobOrdHeaderId"></param>
        /// <param name="jobOrdLocId"></param>
        /// <param name="machineId"></param>
        /// <param name="currentUserId"></param>
        public void InsertMappedMachine(int jobOrdHeaderId, int jobOrdLocId, int machineId, Guid currentUserId, int softwareVersion_id)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    JobOrder_Machine_Mapping machineMap = new JobOrder_Machine_Mapping();

                    machineMap.JobOrder_Header_ID = jobOrdHeaderId;
                    // If user hasn't selected any location, pass null.
                    if (jobOrdLocId != 0)
                        machineMap.JobOrder_Location_id = jobOrdLocId;
                    machineMap.MachineId = machineId;

                    machineMap.MidlineTesting_Stage_Id = dbContext.MasterData_Config_definitions.FirstOrDefault(c => (c.MasterData_Type == "TestingStages" && c.MasterData_Value == "Mid-Line Testing")).MasterData_Type_ID;
                    // At Insertion, status would be null.
                    machineMap.MidlineTesting_Status = null;
                    machineMap.ULLineTesting_Stage_Id = dbContext.MasterData_Config_definitions.FirstOrDefault(c => (c.MasterData_Type == "TestingStages" && c.MasterData_Value == "UL Requirements Testing")).MasterData_Type_ID;
                    machineMap.ULLineTesting_Status = null;
                    machineMap.FinalTesting_Stage_Id = dbContext.MasterData_Config_definitions.FirstOrDefault(c => (c.MasterData_Type == "TestingStages" && c.MasterData_Value == "Final Assembly Testing")).MasterData_Type_ID;
                    machineMap.FinalTesting_Status = null;
                    // Set Pending-Testing as status of machine.
                    machineMap.Machine_Status_Id = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.PendingTesting).JobOrder_Status_Id;
                    machineMap.Created_by = currentUserId;
                    machineMap.Created_Date = DateTime.Now;
                    machineMap.Softwareversion_Id = softwareVersion_id;
                    dbContext.JobOrder_Machine_Mapping.Add(machineMap);

                    dbContext.SaveChanges();
                    // Update machine assigned status.
                    SetMachineIdAsAssigned(machineId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Mapped Machine
        /// </summary>
        /// <param name="machineMappingId"></param>
        /// <param name="jobOrdLocId"></param>
        /// <param name="newMachineId"></param>
        /// <param name="oldMachineId"></param>
        /// <param name="currentUserId"></param>
        public void UpdateMappedMachine(int machineMappingId, int jobOrdLocId, int newMachineId, int oldMachineId, Guid currentUserId, int softwareVersion_id)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recordToUpdate = dbContext.JobOrder_Machine_Mapping.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                    if (recordToUpdate != null)
                    {
                        // If user hasn't selected any location, pass null.
                        if (jobOrdLocId == 0)
                            recordToUpdate.JobOrder_Location_id = null;
                        else
                        {
                            recordToUpdate.JobOrder_Location_id = jobOrdLocId;
                            string machStatus = dbContext.JobOrder_Status.FirstOrDefault(c => c.JobOrder_Status_Id == recordToUpdate.Machine_Status_Id).Status_Name;
                            if (recordToUpdate.MidlineTesting_Status != null && recordToUpdate.ULLineTesting_Status != null && recordToUpdate.FinalTesting_Status != null)
                            {
                                if (machStatus == OrderStatusType.InTesting && (bool)recordToUpdate.MidlineTesting_Status && (bool)recordToUpdate.ULLineTesting_Status && (bool)recordToUpdate.FinalTesting_Status)
                                {
                                    recordToUpdate.Machine_Status_Id = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name.ToLower() == OrderStatusType.InShipping.ToLower()).JobOrder_Status_Id;
                                }
                            }
                        }
                        recordToUpdate.MachineId = newMachineId;
                        recordToUpdate.Modified_by = currentUserId;
                        recordToUpdate.Modified_Date = DateTime.Now;
                        recordToUpdate.Softwareversion_Id = softwareVersion_id;

                        dbContext.SaveChanges();
                        // Set new machine as assigned
                        SetMachineIdAsAssigned(newMachineId);
                        // Set old machine as un-assigned
                        SetMachineIdAsUnAssigned(oldMachineId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Unassigned Machine IDs
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DtoList> GetUnassignedMachineID(bool Status = false)
        {
            IEnumerable<DtoList> machineIDs = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    machineIDs = (from d in dbContext.Machines
                                  where d.IsAssigned == Status
                                  select new DtoList
                                  {
                                      Id = d.Machine_Id,
                                      //Text = SqlFunctions.StringConvert((double)d.Machine_Id).Trim()
                                      Text = d.Machine_Id_Val.Trim()
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return machineIDs;
        }
        public IEnumerable<DtoList> GetUnassignedMachineIDForUpdate(int MachineMapId)
        {
            IEnumerable<DtoList> machineIDs = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    machineIDs = (from d in dbContext.JobOrder_Machine_Id
                                  where d.IsAssigned == false
                                  select new DtoList
                                  {
                                      Id = d.Machine_Id,
                                      Text = d.Machine_Id_Val.Trim()
                                  })
                                  .Union(
                                  from d in dbContext.JobOrder_Machine_Id
                                  join mach in dbContext.JobOrder_Machine_Mapping on d.Machine_Id equals mach.MachineId
                                  where mach.JobOrder_Machine_mapping_ID == MachineMapId
                                  select new DtoList
                                  {
                                      Id = d.Machine_Id,
                                      Text = d.Machine_Id_Val.Trim()
                                  })
                                  .ToList();

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return machineIDs;
        }


        /// <summary>
        /// Set Machine Id As Assigned using machine id
        /// </summary>
        /// <param name="machId"></param>
        private void SetMachineIdAsAssigned(int machId)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recToUpdate = dbContext.JobOrder_Machine_Id.FirstOrDefault(c => c.Machine_Id == machId);
                    if (recToUpdate != null)
                    {
                        recToUpdate.IsAssigned = true;
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Set Machine Id As UnAssigned using machine id
        /// </summary>
        /// <param name="machId"></param>
        private void SetMachineIdAsUnAssigned(int machId)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var recToUpdate = dbContext.JobOrder_Machine_Id.FirstOrDefault(c => c.Machine_Id == machId);
                    if (recToUpdate != null)
                    {
                        recToUpdate.IsAssigned = false;
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetMachineIdAsAssigned(int machId, ReviveDBEntities dbContext, DateTime ShippingDate, DateTime ShippedDate)
        {
            try
            {

                var recToUpdate = dbContext.Machines.FirstOrDefault(c => c.Machine_Id == machId);
                if (recToUpdate != null)
                {
                    recToUpdate.IsAssigned = true;
                    recToUpdate.IsShipped = true;
                    recToUpdate.ShippedDate = ShippedDate;
                    recToUpdate.ShippingDate = ShippingDate;

                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public bool isValidMappingByLocation(int JobOrder_locationId, int OrderId, int newRecordCount, int machineMappingId)
        {
            bool result = true;
            using (var dbContext = new ReviveDBEntities())
            {
                var NoOfMachineCount_OrderLocation = dbContext.JobOrder_Locations.Where(c => c.JobOrder_Location_Id == JobOrder_locationId && c.JobOrder_Id == OrderId).Select(x => x.No_of_Machines).First();
                var MachineMapCount_OrderLocation = dbContext.JobOrder_Machine_Mapping.Where(p => p.JobOrder_Location_id == JobOrder_locationId && p.JobOrder_Header_ID == OrderId).ToList().Count();
                var mappingRecord = dbContext.JobOrder_Machine_Mapping.Where(p => p.JobOrder_Machine_mapping_ID == machineMappingId && p.JobOrder_Location_id == null).ToList().Count();

                int _countLocation = 0;
                int _countMachinMapped = 0 + newRecordCount;



                if (NoOfMachineCount_OrderLocation != null)
                {
                    _countLocation = (int)NoOfMachineCount_OrderLocation;
                }
                _countMachinMapped = MachineMapCount_OrderLocation + newRecordCount + mappingRecord;

                if (_countMachinMapped > _countLocation)
                {
                    result = false;
                }

                return result;


            }
        }
        public bool isValidMappingFinalSubmitByLocation(int JobOrder_locationId, int OrderId)
        {
            bool result = true;
            using (var dbContext = new ReviveDBEntities())
            {
                var NoOfMachineCount_OrderLocation = dbContext.JobOrder_Locations.Where(c => c.JobOrder_Location_Id == JobOrder_locationId && c.JobOrder_Id == OrderId).Select(x => x.No_of_Machines).First();
                var Inshiping_shipped_MapCount = dbContext.JobOrder_Machine_Mapping.Where(p => p.JobOrder_Location_id == JobOrder_locationId && p.JobOrder_Header_ID == OrderId && p.FinalTesting_Status == true).ToList().Count();



                int _countLocation = 0;
                int _countMachinMapped = 0;


                if (JobOrder_locationId != 0 && JobOrder_locationId != null)
                {

                    if (NoOfMachineCount_OrderLocation != null)
                    {
                        _countLocation = (int)NoOfMachineCount_OrderLocation; // Order Location Count
                    }
                    _countMachinMapped = Inshiping_shipped_MapCount; ;

                    if (_countMachinMapped >= _countLocation)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = true;
                }

                return result;


            }
        }

        /// <summary>
        /// Get Machine Detail By Mach MapId using machineMapId
        /// </summary>
        /// <param name="machineMapId"></param>
        /// <returns></returns>
        public MachineModel GetMachineDetailByMachMapId(int machineMapId)
        {
            var result = new MachineModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var record = (from usp_GetMachineDetailByMachMapId_Result spResult in dbContext.usp_GetMachineDetailByMachMapId(machineMapId)
                                  select new MachineModel
                                  {
                                      MachineMappingId = spResult.MachineMappingId,
                                      JobOrderHeaderID = spResult.JobOrder_Header_ID,
                                      JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(spResult.JobOrder_Header_ID)),
                                      NoOfMachines = spResult.Noof_Machines,
                                      LocationId = spResult.JobOrder_Location_id,
                                      Location = spResult.LocationName,
                                      MachineId = spResult.MachineId,
                                      MachineId_Val = spResult.MachineId_Val,

                                      MidLineStageId = spResult.MidLineStageId,
                                      MidLineStageName = spResult.MidLineStageName,
                                      MidLineTest = spResult.MidLineTest,
                                      MidLineTestStatus = spResult.MidLineTestStatus,

                                      ULStageId = spResult.ULStageId,
                                      ULStageName = spResult.ULLineStageName,
                                      ULTest = spResult.ULTest,
                                      ULTestStatus = spResult.ULTestStatus,

                                      FinalStageId = spResult.FinalStageId,
                                      FinalStageName = spResult.FinalStageName,
                                      FinalTest = spResult.FinalTest,
                                      FinalTestStatus = spResult.FinalTestStatus,

                                      //Customer_Id = spResult.Customer_ID,
                                      CustomerName = spResult.CustomerName,
                                      ShippingId = spResult.Shipping_ID,

                                      // Get overall status of machine -> Pending-Testing / In-Testing / In-Shipping / Shipped
                                      MachineStatusName = spResult.MachineStatusName,
                                      MachineStatusId = spResult.MachineStatusId
                                  }).FirstOrDefault();
                    result = record;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
            //usp_GetMachineDetailByMachMapId_Result
        }

        /// <summary>
        /// Update Shipping Id using machineMappingId and shippingId
        /// </summary>
        /// <param name="machineMappingId"></param>
        /// <param name="shippingId"></param>
        public void UpdateShippingId(int machineMappingId, string shippingId, int jobOrderId)
        {
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    var recToUpdate = dbContext.JobOrder_Machine_Mapping.FirstOrDefault(c => c.JobOrder_Machine_mapping_ID == machineMappingId);
                    if (recToUpdate != null)
                    {
                        int shippedID = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.Shipped).JobOrder_Status_Id;
                        if (recToUpdate.Machine_Status_Id != shippedID)
                            recToUpdate.Machine_Status_Id = shippedID;
                        recToUpdate.Shipping_ID = shippingId;

                        // Calling methord to update value  in table JobOrder_Machine_Id  for column Mapped_location_Id & Mapped_Location_Date
                        var locationId = dbContext.JobOrder_Locations.Where(c => c.JobOrder_Location_Id == recToUpdate.JobOrder_Location_id).Select(x => x.Location_id).FirstOrDefault();

                        UpdateMachineMapDetails((int)recToUpdate.MachineId, (int)locationId, dbContext);
                        // End 


                        dbContext.SaveChanges();


                        // Check if all machines are shipped, change Order status to Shipped.
                        UpdateOrderIfMachinesShipped(jobOrderId);

                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Pending.
                throw;
            }
        }

        private void UpdateMachineMapDetails(int machineId, int locationId, ReviveDBEntities dbContext)
        {

            try
            {
                var RecordmachineId = dbContext.JobOrder_Machine_Id.Where(c => c.Machine_Id == machineId).FirstOrDefault();

                if (RecordmachineId != null)
                {
                    RecordmachineId.Mapped_Location_Date = System.DateTime.Now;
                    RecordmachineId.Mapped_Location_Id = locationId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        private static void UpdateOrderIfMachinesShipped(int jobOrderId)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var NoOfMachines = dbContext.JobOrder_header.FirstOrDefault(c => c.JobOrder_Header_ID == jobOrderId).Noof_Machines;
                    if (NoOfMachines != null)
                    {
                        int shippedID = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.Shipped).JobOrder_Status_Id;
                        var machinesShipped = dbContext.JobOrder_Machine_Mapping.Count(c => c.JobOrder_Header_ID == jobOrderId && c.Machine_Status_Id == shippedID);
                        if (NoOfMachines == machinesShipped)
                        {
                            var recToUpdate = dbContext.JobOrder_header.FirstOrDefault(c => c.JobOrder_Header_ID == jobOrderId);
                            if (recToUpdate != null)
                            {
                                recToUpdate.JobOrder_Status_Id = shippedID;
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Pending.
                throw;
            }
        }

        private static void UpdateOrderIfMachinesShipped(int jobOrderId, ReviveDBEntities dbContext)
        {
            try
            {
                var NoOfMachines = dbContext.JobOrder_header.FirstOrDefault(c => c.JobOrder_Header_ID == jobOrderId).Noof_Machines;
                if (NoOfMachines != null)
                {
                    int shippedID = dbContext.JobOrder_Status.FirstOrDefault(c => c.Status_Name == OrderStatusType.Shipped).JobOrder_Status_Id;
                    var machinesShipped = dbContext.JobOrder_Machine_Mapping.Count(c => c.JobOrder_Header_ID == jobOrderId && c.Machine_Status_Id == shippedID);
                    if (NoOfMachines == machinesShipped)
                    {
                        var recToUpdate = dbContext.JobOrder_header.FirstOrDefault(c => c.JobOrder_Header_ID == jobOrderId);
                        if (recToUpdate != null)
                        {
                            recToUpdate.JobOrder_Status_Id = shippedID;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // TODO: Pending.
                throw;
            }
        }


        #region Get Location ID By Machine ID and Location Name
        public MachineModel GetOrderDetailByMachineId(int MachineId, string LocationName)
        {
            try
            {
                var result = new MachineModel();
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from job_mac_map in dbContext.JobOrder_Machine_Mapping
                              join job_order_loc in dbContext.JobOrder_Locations
                              on job_mac_map.JobOrder_Location_id equals job_order_loc.JobOrder_Location_Id
                              join cus_loc in dbContext.Customers_Locations
                              on job_order_loc.Location_id equals cus_loc.Location_ID
                              where cus_loc.Location_Name == LocationName && job_mac_map.MachineId == MachineId
                              select new MachineModel
                              {
                                  Location_Id = cus_loc.Location_ID,
                                  JobOrder_Status = job_mac_map.Machine_Status_Id,
                                  MachineMappingId = job_mac_map.JobOrder_Machine_mapping_ID,
                                  JobOrder_Id = job_order_loc.JobOrder_Id

                              }).FirstOrDefault();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<MachineModel> GetUnAssignedMachines()
        {
            try
            {
                var lstReport = new List<MachineModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstReportDetails = dbContext.usp_GetUnassignedMachineToDelete().Select(d => new MachineModel
                    {
                        MachineId = d.Machine_Id,
                        Status = d.Status,
                        Location = d.Location,
                        MachineId_Val = d.Machine_Id_Val,
                        MachineMappingIdEncoded = string.Empty
                    }).ToList();

                    lstReport.AddRange(lstReportDetails);
                }
                return lstReport;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updating shipment No  & Auto map of Machine to Location  UPS integration
        /// </summary>
        /// <param name="ShipMachineModelObj"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        public bool InsertShippedMachine(ShipMachineModel ShipMachineModelObj, Guid CreatedBy)
        {
            bool result = false;
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {   // Update machine assigned status.

                    var GroupShippingDate = dbContext.LocationGroups.Where(x => x.GroupId == ShipMachineModelObj.GroupId).FirstOrDefault();

                    LocationMapMachine LocationMapMachineObj = new LocationMapMachine();
                    LocationMapMachineObj.Machine_Id = ShipMachineModelObj.MachineId;
                    LocationMapMachineObj.Location_id = ShipMachineModelObj.Location_Id;
                    LocationMapMachineObj.TrackingNumber = ShipMachineModelObj.ShipmentId;
                    LocationMapMachineObj.ShippingLabelData = ShipMachineModelObj.ImageData;
                    dbContext.LocationMapMachines.Add(LocationMapMachineObj);
                    SetMachineIdAsAssigned(ShipMachineModelObj.MachineId, dbContext, (DateTime)GroupShippingDate.ShippingDate, DateTime.Now);
                    // UpdateOrderIfMachinesShipped(ShipMachineModelObj.JobOrder_Header_Id, dbContext);
                    dbContext.SaveChanges();
                    SaveShippedMachineToMachineHistory(ShipMachineModelObj, dbContext); // Saving data into Machine History table
                    UpdateGroupStatusStarted(ShipMachineModelObj.GroupId, dbContext);
                    UpdateGroupStatusCompleted(ShipMachineModelObj.GroupId, dbContext);

                    dbContext.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        private void UpdateGroupStatusCompleted(int _GroupId, ReviveDBEntities dbContext)
        {
            ManageShippingRepository ManageShippingRepositoryObj = new ManageShippingRepository();
            var GroupStatusDetails = ManageShippingRepositoryObj.GetShippingQueueDetailsByGroupId(_GroupId);
            int statusId = dbContext.MasterData_Config_definitions.Where(x => x.status == true && x.MasterData_Type == ConstantEntities.GroupStatusCompleted).Select(x => x.MasterData_Type_ID).FirstOrDefault();

            if ((GroupStatusDetails.Grp_No_of_Machines - GroupStatusDetails.Grp_TotalShipped) == 0)
            {
                var GroupLocMaplst = dbContext.MapLocationByGroups.Where(x => x.GroupId == _GroupId).ToList();
                var GroupTable = dbContext.LocationGroups.Where(x => x.GroupId == _GroupId).FirstOrDefault();
                var LocationLst = dbContext.Customers_Locations.Where(x => x.GroupId == _GroupId).ToList();
                LocationLst.ForEach(x => x.GroupId = null);
                GroupTable.ShippingDate = null;
                GroupTable.ShippingStatus = null;
                GroupLocMaplst.ForEach(x => x.DeploymentStatusId = statusId);
            }


        }
        private void UpdateGroupStatusStarted(int _GroupId, ReviveDBEntities dbContext)
        {
            ManageShippingRepository ManageShippingRepositoryObj = new ManageShippingRepository();
            //  int statusId = dbContext.MasterData_Config_definitions.Where(x => x.status == true && x.MasterData_Type == ConstantEntities.GroupStatusStarted).Select(x => x.MasterData_Type_ID).FirstOrDefault();
            int statusId = dbContext.MasterData_Config_definitions.Where(x => x.status == true && x.MasterData_Type == ConstantEntities.GroupStatusStarted).Select(x => x.MasterData_Type_ID).FirstOrDefault();

            var GroupTable = dbContext.LocationGroups.Where(x => x.GroupId == _GroupId).FirstOrDefault();
            if (GroupTable.ShippingStatus == null)
            {
                GroupTable.ShippingStatus = statusId;
            }



        }
        private void SaveShippedMachineToMachineHistory(ShipMachineModel ShipMachineModelObj, ReviveDBEntities dbContext)
        {
            EditMachineHistory MachineHistoryObj = new EditMachineHistory();

            var objCustomerDetails = dbContext.usp_GetMachineHistoryCustomerDetailsByMachineId(ShipMachineModelObj.MachineId).FirstOrDefault();
            if (objCustomerDetails != null)
            {
                var objmachineconfiguration = dbContext.MachineHistory_Configuration.Where(m => m.ConfiguredValue == ConstantEntities.InitialDeployment).FirstOrDefault();
                objCustomerDetails.TransactionTypeId = objmachineconfiguration != null ? objmachineconfiguration.Id : 0;
                MachineHistoryObj.CustomerId = Convert.ToInt32(objCustomerDetails.CustomerId);
                MachineHistoryObj.SubsidiaryId = Convert.ToInt32(objCustomerDetails.Subsidiary_ID);
                MachineHistoryObj.SubAgentId = Convert.ToInt32(objCustomerDetails.SubAgent_ID);
                MachineHistoryObj.LocationId = Convert.ToInt32(objCustomerDetails.LocationId);
                MachineHistoryObj.TransactionTypeId = Convert.ToInt32(objCustomerDetails.TransactionTypeId);

            }
            MachineHistoryObj.MachineId = ShipMachineModelObj.MachineId;
            MachineHistoryObj.Notes = "Shipped : " + ShipMachineModelObj.ShipmentId.ToString();
            MachineHistoryObj.ShippinglabelData = ShipMachineModelObj.ImageData;

            ManageMachineSpecsRepository objMachineSpecsRepo = new ManageMachineSpecsRepository();
            objMachineSpecsRepo.SaveMachineHistoryData(dbContext, MachineHistoryObj);


        }

        /// <summary>
        /// Check Machine active
        /// </summary>
        /// <param name="MachineId"></param>
        /// <returns></returns>
        public string CheckMachineShipped(int MachineId)
        {
            string returnval = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {



                    var machineIdQuery = dbContext.Machines.FirstOrDefault(mach => mach.Machine_Id == MachineId && mach.IsTested == true && (mach.IsShipped == false || mach.IsShipped == null));


                    if (machineIdQuery != null)
                    {
                        returnval = "";

                    }
                    else
                    {
                        returnval = "Machine Id is not valid with Enterprise Server.";
                    }
                }

            }
            catch (Exception ex)
            {
                returnval = "Error in validating machine Id" + ex.Message.ToString();
            }
            return returnval;

        }

        #endregion
    }
}
