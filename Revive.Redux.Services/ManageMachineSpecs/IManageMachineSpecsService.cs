using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Entities;
using Revive.Redux.Repositories;


namespace Revive.Redux.Services
{
    public interface IManageMachineSpecsService
    {
        IEnumerable<MachineSpecsModel> GetMachineSpecs();

        IEnumerable<Debug_Templates_DefinitionModel> GetTemplatesPara();

        List<Debug_Templates_DefinitionModel> GetTemplatesParaById(int Id);

        Boolean AddEditMachineSpec(MachineSpecsModel mechspec);

        MachineSpecsModel GetMachineDetailsById(int mechId);

        Boolean UpdtTempl(IEnumerable<ManageDebugParaModel> da, int templ);

        Boolean UpdSoftware(IEnumerable<MachineModel> data, int softversn);

        Boolean AddTemplate(List<Debug_Templates_DefinitionModel> debugTemplatesDefinitions);

        IEnumerable<ManageDebugParaModel> GetManageDebugPara(int Id, int locId, int SubsidiaryId, int SubAgentId);

        IEnumerable<ManageSoftwareVersionModel> GetSoftwareDetail();

        ManageSoftwareVersionModel GetSoftwareDetailsById(int sfid);

        Boolean AddEditSoftware(ManageSoftwareVersionModel softmodel);

        IEnumerable<MachineModel> GetMappedMachinesByCustomer(int customer_Id, int location_Id, int SubsidiaryId, int SubAgentId);

        List<MachineModel> GetKillMachineByLocationAndCustomer(int nLocationId, int nCustomerId, int SubsidiaryId, int SubAgentId);

        MachineModel KillMachine(MachineModel objMachineModel);

        string GetActivityName(int nActivityTypeId);

        void UpdateArchiveStatus(int MasterData_Type_ID);

        IEnumerable<DtoList> GetShippedMachIds(int customer_Id, int location_Id);

        bool ReplaceMachine(int shippedMachId, int unassignedMachId, Guid currentUser, int OldMachineLocationId);
        bool DeleteMachine(IEnumerable<MachineModel> DeletedMachineData);
        bool MoveMachine(MoveMachineModel model, Guid currentUser);
        bool CheckSoftwareVersion(String Name, int MasterData_Type_ID);
        List<ConfigureMachineHistoryModel> GetConfigureMachineHistory();
        ConfigureMachineHistoryModel GetConfigureMachineHistoryById(int Id);
        bool AddEditConfigureMachineHistory(ConfigureMachineHistoryModel ConfigMachineHisModel, Guid CurrentUser);
        IEnumerable<MachineHistoryLst> GetMachineHistoryLst(int customer_Id, int location_Id, int SubsidiaryId, int SubAgentId);
        EditMachineHistory GetMachineHistoryByMachineId(int MachineId);
        bool CheckDuplicateConfigureHistoryName(string ConfiguredValue, int ConfigureTypeId, int ConfigureHistoryId);
        IEnumerable<DtoList> GetMachineHistoryConfigTypeByValue(string ConfigType, string ConfigHistoryType);
        IEnumerable<EditMachineHistoryLst> GetEditMachineHistoryDetailsByMachineId(int machineId);
        bool AddMachineHistory(EditMachineHistory MachineHistModel);
        IEnumerable<MachineDocumentsModel> GetMachineDocsList(int MachineId, int MachineHistoryId);
        MachineDocumentsModel GetMachineDocumentDetailById(MachineDocumentsModel Obj);
        string DeleteMachineHistoryById(int MachineHistoryId);
        string DeleteConfigureMachineHistoryById(int ConfigureMachineHistoryID);
    }
}
