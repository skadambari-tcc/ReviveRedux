using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;
using System.Net.Mail;


namespace Revive.Redux.Services
{
    public interface IGeneralService
    {
        IEnumerable<DtoList> GetEmptyDDL();
        IEnumerable<DtoList> GetEmptyDDLWithoutSelect();
        IEnumerable<DtoList> GetCity(int Id);
        IEnumerable<DtoList> GetState();
        IEnumerable<DtoList> GetCustomerService(bool bActive = false);
        IEnumerable<DtoList> GetUserRoleService();
        IEnumerable<DtoList> GetCustomerLocationService(int customerId, bool bActive = false);
        IEnumerable<DtoList> GetUserType();
        IEnumerable<DtoList> GetStoreUserLevelType();
        IEnumerable<DtoList> GetMachineSpecs();
        IEnumerable<DtoList> GetCustomerDocs(int custId);
        IEnumerable<DtoList> GetUserTypeByUserLevelId(int UserLevelId);
        IEnumerable<DtoList> GetTemplateServices();
        IEnumerable<DtoList> GetAccountManagerDetail();
        IEnumerable<DtoList> GetServiceType();
        IEnumerable<DtoList> GetUserLevelType();
        IEnumerable<DtoList> GetUserLevelType(string userType);
        IEnumerable<DtoList> GetFirmware();
        IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode);
        IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode, int id);
        IEnumerable<NotificationModel> GetNotificationDashboard(string email_Id);
        IEnumerable<Debug_Templates_DefinitionModel> GetParameters();
        IEnumerable<Debug_Templates_DefinitionModel> GetParameters(int templateId);
        bool CompareDate(DateTime? StartDate, DateTime? EndDate);
        DtoList GetUserRoleByAccessCode(string Accesscode);
        bool StoreNotification(NotificationModel mailrecord);
        NotificationModel GetNotificationDetiailById(int id);
        int GetUserLevelId(string pageAccessCode);

        IEnumerable<DtoList> GetPhoneManufacturer();
        string DeleteNotification(int notificationId, string email_Id);
        IEnumerable<DtoList> GetCustomerStoreUser(Guid userId, string _PageAccessCode);
        bool DeActivateByCustomerId(int customerId, Guid modifiedBy);
        bool ActivateByCustomerId(int customerId, Guid modifiedBy);
        bool DeActivateByLocationId(int locationId, Guid modifiedBy);
        string ActivateByLocationId(int locationId, Guid modifiedBy);
        IEnumerable<OrderModel> GetHomePagePendingTasks(Guid acctManagerId, int userLevelId, string pageAccessCode, int manufac_ID);
        string checkValidLocationToActivate(int locationId);
        IEnumerable<DtoList> GetManufacturers();
        List<ModeConfigurationModel> GetAllConfigModes(ModeConfigurationModel objModeConfigDetails);
        string AddNewModeConfig(ModeConfigurationModel newModeConfig);
        ModeConfigurationModel GetModeConfigById(int id);
        bool UpdateModeConfig(ModeConfigurationModel existingModeConfig);
        bool ValidateData(int loc, DateTime from, DateTime to, int modeId);
        void DeleteModeConfig(int modeID);
        IEnumerable<DtoList> GetModes(Guid user_id);
        string AddMachine(WebApiMachineModel objMachine);
        string DeleteLocationByLocId(int locationId);
        IEnumerable<DtoList> GetLocations();
        string GetUserByPageAccessCodeByUserLevelId(int UserLevelId);
        IEnumerable<DtoList> GetSubsidiaryByCustomerId(int customerId);
        IEnumerable<DtoList> GetSubsidiaryByCustomerId(int customerId, int SubsidiaryId);
        IEnumerable<DtoList> GetAgentsBySubsidiaryId(int subsidiaryId);
        IEnumerable<DtoList> GetAgentsBySubsidiaryId(int subsidiaryId, int SubAgentId);
        IEnumerable<DtoList> GetMasterValuesByType(string configValue);
        IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, bool bActive = false);
        IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId, bool bActive = false);
        IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId);
        int IncValue();
        int GetValue();
        int ResetValue();

        bool DeActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy);
        string ActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy);
        bool DeActivateBySubAgentId(int subagentId, Guid modifiedBy);
        string ActivateBySubAgentId(int subagentId, Guid modifiedBy);

        string checkValidSubdirectoryToActivate(int subdirectoryId);
        string checkValidSubAgentToActivate(int subagentId);
        IEnumerable<DtoList> GetLoggingTypes();
        string AddMultipleModeConfig(ModeConfigurationModel newModeConfig, IEnumerable<ModeConfigurationModel> data);
        IEnumerable<DtoList> GetPriority();
        IEnumerable<DtoList> GetGroups();
        string MachineTestResult(MachineTestingModel objMachine);
        string GroupStatusByLocationId(int _LocationId);

    }
}
