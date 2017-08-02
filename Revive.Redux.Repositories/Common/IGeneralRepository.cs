using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux;
using Revive.Redux.Entities;
using System.Net.Mail;


namespace Revive.Redux.Repositories
{
    public interface IGeneralRepository
    {
        IEnumerable<DtoList> GetEmptyDDL();
        IEnumerable<DtoList> GetEmptyDDLWithoutSelect();
        IEnumerable<DtoList> GetCity(int Id);
        IEnumerable<DtoList> GetState();
        IEnumerable<DtoList> GetCustomer(bool bActive = false);
        IEnumerable<DtoList> GetUserRole();
        IEnumerable<DtoList> GetCustomerLocation(int customerId, bool bActive = false);
        IEnumerable<DtoList> GetUserType();
        IEnumerable<DtoList> GetStoreUserLevelType();
        IEnumerable<DtoList> GetMachineSpecs();
        IEnumerable<DtoList> GetCustomerDocs(int custId);
        IEnumerable<DtoList> GetUserTypeByUserLevelId(int UserLevelId);
        IEnumerable<DtoList> GetTemplate();
        IEnumerable<DtoList> GetAccountManagerDetail();
        IEnumerable<DtoList> GetServiceType();
        IEnumerable<DtoList> GetFirmware();
        IEnumerable<DtoList> GetUserLevelType();
        IEnumerable<DtoList> GetUserLevelType(string userType);
        IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode);
        IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode, int id);
        IEnumerable<Debug_Templates_DefinitionModel> GetParameters();
        IEnumerable<Debug_Templates_DefinitionModel> GetParameters(int templateId);
        IEnumerable<NotificationModel> GetNotificationDashboard(string email_Id);
        DtoList GetUserRoleByAccessCode(string Accesscode);
        bool CompareDate(DateTime? StartDate, DateTime? EndDate);
        bool StoreNotification(NotificationModel mailrecord);
        NotificationModel GetNotificationDetiailById(int id);
        int GetUserLevelId(string pageAccessCode);
        // API Call method
        IEnumerable<DtoList> GetDevivemanufacturer();
        IEnumerable<DtoList> GetHowlongago();
        //
        IEnumerable<DtoList> GetPhoneManufacturer();
        string DeleteNotification(int notificationId, string email_Id);
        IEnumerable<DtoList> GetCustomerStoreUser(Guid userId, string _PageAccessCode);
        bool DeActivateByCustomerId(int customerId, Guid modifiedBy);
        bool ActivateByCustomerId(int customerId, Guid modifiedBy);
        bool DeActivateByLocationId(int locationId, Guid modifiedBy);
        bool ActivateByLocationId(int locationId, Guid modifiedBy);
        IEnumerable<OrderModel> GetHomePagePendingTasks(Guid acctManagerId, int userLevelId, string pageAccessCode, int manufac_ID);
        string checkValidLocationToActivate(int locationId);
        IEnumerable<DtoList> GetManufacturers();
        IEnumerable<DtoList> GetModes(Guid user_id);
        string AddNewModeConfig(ModeConfigurationModel newModeConfig);
        List<ModeConfigurationModel> GetAllConfigModes(ModeConfigurationModel objModeConfigDetails);
        ModeConfigurationModel GetModeConfigById(int id);
        bool UpdateModeConfig(ModeConfigurationModel existingModeConfig);
        bool ValidateData(int loc, DateTime from, DateTime to, int modeId);
        void DeleteModeConfig(int modeID);
        string AddMachine(WebApiMachineModel objMachine);
        string DeleteLocationByLocId(int locationId);
        IEnumerable<DtoList> GetLocations();
        string GetUserByPageAccessCodeByUserLevelId(int UserLevelId);
        IEnumerable<DtoList> GetSubsidiaryByCustomerId(int customerId);
        IEnumerable<DtoList> GetSubsidiaryByCustomerId(int customerId, int subsidiaryId);
        IEnumerable<DtoList> GetAgentsBySubsidiaryId(int subsidiaryId);
        IEnumerable<DtoList> GetAgentsBySubsidiaryId(int subsidiaryId, int SubAgentId);
        IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, bool bActive = false);
        IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId, bool bActive = false);
        IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId);
        IEnumerable<DtoList> GetMasterValuesByType(string configValue);
        int IncValue();
        int GetValue();
        int ResetValue();
        string checkValidSubdirectoryToActivate(int subdirectoryId);
        string checkValidSubAgentToActivate(int subagentId);
        bool DeActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy);
        bool ActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy);
        bool DeActivateBySubAgentId(int subagentId, Guid modifiedBy);
        bool ActivateBySubAgentId(int subagentId, Guid modifiedBy);
        IEnumerable<DtoList> GetLoggingTypes();
        string AddMultipleModeConfig(ModeConfigurationModel newModeConfig, IEnumerable<ModeConfigurationModel> data);
        IEnumerable<DtoList> GetPriority();
        IEnumerable<DtoList> GetGroups();
        string MachineTestResult(MachineTestingModel objMachine);
        string GroupStatusByLocationId(int _LocationId);
        string MachineTestDataExist(int machineId, string Machine_Id_val);
    }
}
