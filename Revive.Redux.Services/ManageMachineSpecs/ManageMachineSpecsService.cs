using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;

namespace Revive.Redux.Services
{
    public class ManageMachineSpecsService : IManageMachineSpecsService
    {


        IManageMachineSpecsRepository machineRepo = null;

        public ManageMachineSpecsService()
        {
            machineRepo = new ManageMachineSpecsRepository();
        }

        public IEnumerable<MachineSpecsModel> GetMachineSpecs()
        {
            return machineRepo.GetMachineSpecs();
        }

        public IEnumerable<ManageDebugParaModel> GetManageDebugPara(int Id, int locId, int SubsidiaryId, int SubAgentId)
        {
            return machineRepo.GetManageDebugPara(Id, locId, SubsidiaryId, SubAgentId);
        }

        public bool UpdtTempl(IEnumerable<ManageDebugParaModel> da, int templ)
        {
            return machineRepo.UpdtTempl(da, templ);
        }

        public bool UpdSoftware(IEnumerable<MachineModel> data, int softversn)
        {
            return machineRepo.UpdSoftware(data, softversn);
        }

        public bool AddEditMachineSpec(MachineSpecsModel mechspec)
        {
            return machineRepo.AddEditMachineSpec(mechspec);
        }

        public bool AddEditSoftware(ManageSoftwareVersionModel softmodel)
        {

            return machineRepo.AddEditSoftware(softmodel);
        }

        public MachineSpecsModel GetMachineDetailsById(int mechId)
        {
            return machineRepo.GetMachineDetailsById(mechId);
        }

        public ManageSoftwareVersionModel GetSoftwareDetailsById(int sfId)
        {
            return machineRepo.GetSoftwareDetailsById(sfId);
        }

        public IEnumerable<ManageSoftwareVersionModel> GetSoftwareDetail()
        {
            return machineRepo.GetSoftwareDetails();
        }

        public IEnumerable<Debug_Templates_DefinitionModel> GetTemplatesPara()
        {
            return machineRepo.GetTemplatesPara();
        }

        public List<Debug_Templates_DefinitionModel> GetTemplatesParaById(int Id)
        {
            return machineRepo.GetTemplatesParaById(Id);
        }
        public IEnumerable<MachineModel> GetMappedMachinesByCustomer(int customer_Id, int location_Id, int SubsidiaryId, int SubAgentId)
        {
            return machineRepo.GetMappedMachinesByCustomer(customer_Id, location_Id, SubsidiaryId, SubAgentId);
        }

        public List<MachineModel> GetKillMachineByLocationAndCustomer(int nLocationId, int nCustomerId, int SubsidiaryId, int SubAgentId)
        {
            return machineRepo.GetKillMachineByLocationAndCustomer(nLocationId, nCustomerId, SubsidiaryId, SubAgentId);

        }

        public MachineModel KillMachine(MachineModel objMachineModel)
        {
            return machineRepo.KillMachine(objMachineModel);
        }
        public bool AddTemplate(List<Debug_Templates_DefinitionModel> debugTemplatesDefinitions)
        {
            return machineRepo.AddTemplate(debugTemplatesDefinitions);
        }


        public string GetActivityName(int nActivityTypeId)
        {
            return machineRepo.GetActivityName(nActivityTypeId);
        }

        public void UpdateArchiveStatus(int MasterData_Type_ID)
        {
            machineRepo.UpdateArchiveStatus(MasterData_Type_ID);
        }

        public IEnumerable<DtoList> GetShippedMachIds(int customer_Id, int location_Id)
        {
            return machineRepo.GetShippedMachIds(customer_Id, location_Id);
        }

        public bool ReplaceMachine(int shippedMachId, int unassignedMachId, Guid currentUser, int OldMachineLocationId)
        {
            return machineRepo.ReplaceMachine(shippedMachId, unassignedMachId, currentUser, OldMachineLocationId);
        }
        public bool DeleteMachine(IEnumerable<MachineModel> DeletedMachineData)
        {
            return machineRepo.DeleteMachine(DeletedMachineData);
        }

        public bool MoveMachine(MoveMachineModel model, Guid currentUser)
        {
            return machineRepo.MoveMachine(model, currentUser);
        }
        public bool CheckSoftwareVersion(String Name, int MasterData_Type_ID)
        {
            return machineRepo.CheckSoftwareVersion(Name, MasterData_Type_ID);
        }
        public List<ConfigureMachineHistoryModel> GetConfigureMachineHistory()
        {
            return machineRepo.GetConfigureMachineHistory();
        }
        public ConfigureMachineHistoryModel GetConfigureMachineHistoryById(int Id)
        {
            return machineRepo.GetConfigureMachineHistoryById(Id);
        }
        public bool AddEditConfigureMachineHistory(ConfigureMachineHistoryModel ConfigMachineHisModel, Guid CurrentUser)
        {
            return machineRepo.AddEditConfigureMachineHistory(ConfigMachineHisModel, CurrentUser);
        }
        public IEnumerable<MachineHistoryLst> GetMachineHistoryLst(int customer_Id, int location_Id, int SubsidiaryId, int SubAgentId)
        {
            return machineRepo.GetMachineHistoryLst(customer_Id, location_Id, SubsidiaryId, SubAgentId);
        }
        public EditMachineHistory GetMachineHistoryByMachineId(int MachineId)
        {
            return machineRepo.GetMachineHistoryByMachineId(MachineId);
        }
        public bool CheckDuplicateConfigureHistoryName(string ConfiguredValue, int ConfigureTypeId, int ConfigureHistoryId)
        {
            return machineRepo.CheckDuplicateConfigureHistoryName(ConfiguredValue, ConfigureTypeId, ConfigureHistoryId);
        }
        public IEnumerable<DtoList> GetMachineHistoryConfigTypeByValue(string ConfigType, string ConfigHistoryType)
        {
            return machineRepo.GetMachineHistoryConfigTypeByValue(ConfigType, ConfigHistoryType);
        }
        public IEnumerable<EditMachineHistoryLst> GetEditMachineHistoryDetailsByMachineId(int machineId)
        {
            return machineRepo.GetEditMachineHistoryDetailsByMachineId(machineId);
        }
        public bool AddMachineHistory(EditMachineHistory MachineHistModel)
        {
            return machineRepo.AddMachineHistory(MachineHistModel);
        }
        public IEnumerable<MachineDocumentsModel> GetMachineDocsList(int MachineId, int MachineHistoryId)
        {
            return machineRepo.GetMachineDocsList(MachineId, MachineHistoryId);
        }
        public MachineDocumentsModel GetMachineDocumentDetailById(MachineDocumentsModel Obj)
        {
            return machineRepo.GetMachineDocumentDetailById(Obj);
        }

        public string DeleteMachineHistoryById(int MachineHistoryId)
        {
            return machineRepo.DeleteMachineHistoryById(MachineHistoryId);
        }

        public string DeleteConfigureMachineHistoryById(int ConfigureMachineHistoryID)
        {
            return machineRepo.DeleteConfigureMachineHistoryById(ConfigureMachineHistoryID);
        }
    }
}
