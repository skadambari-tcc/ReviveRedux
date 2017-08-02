using Kendo.Mvc.UI;
using Revive.Redux.Commn;
using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public class MachineService : IMachineService
    {
        private IMachineRepository _IMachineRepository = null;
        private IMembershipRepository _IMembershipRepository = null;

        public MachineService()
        {
            _IMachineRepository = new MachineRepository();
            _IMembershipRepository = new MembershipRepository();
        }

        public IEnumerable<MachineModel> GetMachinesByOrderIdForShipping(int ID)
        {
            return _IMachineRepository.GetMachinesByOrderIdForShipping(ID);
        }

        public IEnumerable<MachineModel> GetMappedMachinesByOrderId(int ID)
        {
            return _IMachineRepository.GetMappedMachinesByOrderId(ID);
        }
        public void InsertMappedMachine(int jobOrdHeaderId, int jobOrdLocId, int machineId, Guid currentUserId, int softwareVersion_id)
        {
            _IMachineRepository.InsertMappedMachine(jobOrdHeaderId, jobOrdLocId, machineId, currentUserId, softwareVersion_id);
        }
        public void UpdateMappedMachine(int machineMappingId, int jobOrdLocId, int newMachineId, int oldMachineId, Guid currentUserId, int softwareVersion_id)
        {
            _IMachineRepository.UpdateMappedMachine(machineMappingId, jobOrdLocId, newMachineId, oldMachineId, currentUserId, softwareVersion_id);
        }
        public IEnumerable<DtoList> GetUnassignedMachineID(bool Status = false)
        {
            return _IMachineRepository.GetUnassignedMachineID(Status);
        }
        public IEnumerable<DtoList> GetUnassignedMachineIDForUpdate(int MachineMapId)
        {
            return _IMachineRepository.GetUnassignedMachineIDForUpdate(MachineMapId);
        }
        public MachineModel GetMachineDetailByMachMapId(int machineMapId)
        {
            return _IMachineRepository.GetMachineDetailByMachMapId(machineMapId);
        }
        public void UpdateShippingId(int machineMappingId, string shippingId, int jobOrderId)
        {
            _IMachineRepository.UpdateShippingId(machineMappingId, shippingId, jobOrderId);
        }
        public bool isValidMappingByLocation(int JobOrder_locationId, int OrderId, int newRecordCount, int machineMappingId)
        {
            return _IMachineRepository.isValidMappingByLocation(JobOrder_locationId, OrderId, newRecordCount, machineMappingId);
        }
        public bool isValidMappingFinalSubmitByLocation(int JobOrder_locationId, int OrderId)
        {
            return _IMachineRepository.isValidMappingFinalSubmitByLocation(JobOrder_locationId, OrderId);
        }


        public bool ShippingDetailsFileUpload(MachineModel objMachine, out MachineModel objMachineResult)
        {
            bool bResult = false;
            List<MachinResultModel> lstMachineResult = new List<MachinResultModel>();

            objMachineResult = new MachineModel();
            int nLineNumber = 1;
            string sMessage = string.Empty;
            string errorMesg = string.Empty;

            List<MachineModel> lstMachineModel = FileHandler.ValidateExcelBulkShippingDetails(objMachine.FileName, out errorMesg);
            var LstMemberShip = new List<MachineModel>();
            LocationManagement locationSer = new LocationManagement();

            foreach (var _machine in lstMachineModel)
            {
                var validationResults = new List<ValidationResult>();
                bResult = false;

                if (ValidateShippingDetailFile(_machine, out validationResults))
                {
                    _IMembershipRepository = new MembershipRepository();
                    var _macId = _IMembershipRepository.GetMachineIdByMachineIdVal(_machine.MachineId_Val);
                    var _locId = _IMachineRepository.GetOrderDetailByMachineId(_macId, _machine.Location);
                    if (_locId != null && _macId > 0)
                    {
                        var OrderDetail = new MachineModel();
                        OrderDetail = _IMachineRepository.GetOrderDetailByMachineId(_macId, _machine.Location);

                        RepositoryResult objresult = new RepositoryResult();
                        _IMachineRepository.UpdateShippingId((int)OrderDetail.MachineMappingId, _machine.ShippingId, OrderDetail.JobOrder_Id);
                        sMessage = "Successfully Inserted.";
                        //if (objresult.resultVal == "")
                        //{
                        //    sMessage = "Successfully Inserted.";
                        //}
                        //else
                        //{
                        //    sMessage = "Failed to Inserted.";
                        //}
                    }
                    else
                    {
                        sMessage = "Invalid Location and Shipping ID.";
                    }
                }
                else
                {
                    sMessage = "Failed to Inserted. ";
                    foreach (var val in validationResults)
                    {
                        var errorKeyLst = val.MemberNames;
                        string keystr = "";

                        foreach (var errorstr in errorKeyLst)
                        {
                            switch (errorstr)
                            {
                                case "MachineId":
                                    {
                                        keystr += "Machine ID";
                                        break;
                                    }
                                case "ShippingId":
                                    {
                                        keystr += "Shipping ID";
                                        break;
                                    }
                                case "Location":
                                    {
                                        keystr += "Location Name";
                                        break;
                                    }

                                default:
                                    {
                                        keystr += errorstr;
                                        break;
                                    }
                            }
                        }
                        sMessage += "; " + keystr + " not valid";
                    }
                }

                var MachineResult = new MachinResultModel()
                {
                    LineNumber = "Line Number " + nLineNumber,
                    Message = sMessage,
                    Status = bResult

                };

                lstMachineResult.Add(MachineResult);
                nLineNumber++;
            }
            if (lstMachineModel.Count == 0)
            {
                var MachineResult = new MachinResultModel()
                {
                    LineNumber = "Line Number 1",
                    //Message = "Invalid file format.",
                    Message = errorMesg,
                    Status = bResult

                };
                lstMachineResult.Add(MachineResult);
            }
            objMachineResult.MachineResult = lstMachineResult;
            return bResult;
        }


        private bool ValidateShippingDetailFile(MachineModel objMachineModel, out List<ValidationResult> validationResults)
        {
            MachineBulkUploadModel objShippingDetailModelBulkModel = new MachineBulkUploadModel();
            objShippingDetailModelBulkModel.MachineId = objMachineModel.MachineId_Val;
            objShippingDetailModelBulkModel.ShippingId = objMachineModel.ShippingId;
            objShippingDetailModelBulkModel.Location = objMachineModel.Location;
            bool bResult = false;

            validationResults = new List<ValidationResult>();

            var context = new ValidationContext(objShippingDetailModelBulkModel, serviceProvider: null, items: null);

            bResult = Validator.TryValidateObject(objShippingDetailModelBulkModel, context, validationResults, true);

            return bResult;
        }
        public List<MachineModel> GetUnAssignedMachines()
        {
            return _IMachineRepository.GetUnAssignedMachines();
        }
        public bool InsertShippedMachine(ShipMachineModel ShipMachineModelObj, Guid CreatedBy)
        {
            int _machineId = _IMembershipRepository.GetMachineIdByMachineIdVal(ShipMachineModelObj.MachineId_Val);
            ShipMachineModelObj.MachineId = _machineId;
            return _IMachineRepository.InsertShippedMachine(ShipMachineModelObj, CreatedBy);
        }
        public string CheckMachineShipped(string MachineId_Val)
        {
            int MachineId = _IMembershipRepository.GetMachineIdByMachineIdVal(MachineId_Val);
            return _IMachineRepository.CheckMachineShipped(MachineId);
        }

    }
}
