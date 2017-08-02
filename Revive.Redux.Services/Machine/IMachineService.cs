using Kendo.Mvc.UI;
using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Revive.Redux.Services
{
    public interface IMachineService
    {
        IEnumerable<MachineModel> GetMachinesByOrderIdForShipping(int id);
        IEnumerable<MachineModel> GetMappedMachinesByOrderId(int id);
        void InsertMappedMachine(int jobOrdHeaderId, int jobOrdLocId, int machineId, Guid currentUserId, int softwareVersion_id);
        void UpdateMappedMachine(int machineMappingId, int jobOrdLocId, int newMachineId, int oldMachineId, Guid currentUserId, int softwareVersion_id);
        IEnumerable<DtoList> GetUnassignedMachineID(bool Status = false);
        MachineModel GetMachineDetailByMachMapId(int machineMapId);
        void UpdateShippingId(int machineMappingId, string shippingId, int jobOrderId);
        IEnumerable<DtoList> GetUnassignedMachineIDForUpdate(int MachineMapId);
        bool isValidMappingByLocation(int JobOrder_locationId, int OrderId, int newRecordCount, int machineMappingId);
        bool isValidMappingFinalSubmitByLocation(int JobOrder_locationId, int OrderId);
        bool ShippingDetailsFileUpload(MachineModel objShippingDetails, out MachineModel objShippingDetailsResult);
        List<MachineModel> GetUnAssignedMachines();
        bool InsertShippedMachine(ShipMachineModel ShipMachineModelObj, Guid CreatedBy);
        string CheckMachineShipped(string MachineId_Val);
    }
}
