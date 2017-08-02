using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Commn;
using Revive.Redux.Entities;
using System.Net.Mail;
using Revive.Redux.Common;

namespace Revive.Redux.Services
{
    public class GeneralService : IGeneralService
    {
        private IGeneralRepository _IGeneralRepository;
        private IEnumerable<DtoList> lstData = null;

        public GeneralService()
        {
            _IGeneralRepository = new GeneralRepository();
        }

        public IEnumerable<DtoList> GetEmptyDDL()
        {
            return _IGeneralRepository.GetEmptyDDL();
        }
        public IEnumerable<DtoList> GetEmptyDDLWithoutSelect()
        {
            return _IGeneralRepository.GetEmptyDDLWithoutSelect();
        }


        // IList<Customer> GetCustomer();
        //IList<User_Role> GetUserRole();
        //IList<CustomerLocation> GetCustomerLocation(int customerId);
        public IEnumerable<DtoList> GetCustomerService(bool bActive = false)
        {
            return _IGeneralRepository.GetCustomer(bActive);
        }

        public IEnumerable<DtoList> GetTemplateServices()
        {
            return _IGeneralRepository.GetTemplate();
        }

        public IEnumerable<DtoList> GetUserRoleService()
        {
            return _IGeneralRepository.GetUserRole();
        }
        public IEnumerable<DtoList> GetCustomerLocationService(int CustomerId, bool bActive = false)
        {
            return _IGeneralRepository.GetCustomerLocation(CustomerId);

        }



        public IEnumerable<DtoList> GetUserType()
        {
            return _IGeneralRepository.GetUserType();
        }


        public IEnumerable<DtoList> GetStoreUserLevelType()
        {
            return _IGeneralRepository.GetStoreUserLevelType();
        }

        public IEnumerable<DtoList> GetMachineSpecs()
        {
            return _IGeneralRepository.GetMachineSpecs();
        }
        public IEnumerable<DtoList> GetCustomerDocs(int custId)
        {
            return _IGeneralRepository.GetCustomerDocs(custId);
        }
        public IEnumerable<DtoList> GetCity(int Id)
        {
            //fetch the City from the cache:
            lstData = ReviveCaching.Instance.GetItem("GetCity_" + Id) as IEnumerable<DtoList>;

            if (lstData == null)
            {
                lstData = _IGeneralRepository.GetCity(Id);

                //Store the City in the cache:
                ReviveCaching.Instance.AddItem("GetCity_" + Id, lstData);
            }
            return lstData;
        }
        public IEnumerable<DtoList> GetState()
        {
            //fetch the State from the cache:
            lstData = ReviveCaching.Instance.GetItem("GetState") as IEnumerable<DtoList>;

            if (lstData == null)
            {
                lstData = _IGeneralRepository.GetState();

                //Store the State in the cache:
                ReviveCaching.Instance.AddItem("GetState", lstData);
            }
            return lstData;
        }
        public IEnumerable<DtoList> GetAccountManagerDetail()
        {
            return _IGeneralRepository.GetAccountManagerDetail();
        }
        public IEnumerable<DtoList> GetServiceType()
        {
            return _IGeneralRepository.GetServiceType();
        }
        public IEnumerable<DtoList> GetUserTypeByUserLevelId(int UserLevelId)
        {
            return _IGeneralRepository.GetUserTypeByUserLevelId(UserLevelId);

        }

        public IEnumerable<DtoList> GetFirmware()
        {
            return _IGeneralRepository.GetFirmware();
        }
        public IEnumerable<DtoList> GetUserLevelType()
        {
            return _IGeneralRepository.GetUserLevelType();
        }
        public IEnumerable<DtoList> GetUserLevelType(string userType)
        {
            return _IGeneralRepository.GetUserLevelType(userType);
        }
        public IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode)
        {
            return _IGeneralRepository.GetUserByPageAccessCode(pageAccessCode);
        }
        public IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode, int id)
        {
            return _IGeneralRepository.GetUserByPageAccessCode(pageAccessCode, id);
        }


        public IEnumerable<NotificationModel> GetNotificationDashboard(string email_Id)
        {
            return _IGeneralRepository.GetNotificationDashboard(email_Id);
        }

        public IEnumerable<Debug_Templates_DefinitionModel> GetParameters()
        {
            return _IGeneralRepository.GetParameters();
        }
        public IEnumerable<Debug_Templates_DefinitionModel> GetParameters(int templateId)
        {
            return _IGeneralRepository.GetParameters(templateId);
        }

        public bool CompareDate(DateTime? StartDate, DateTime? EndDate)
        {
            return _IGeneralRepository.CompareDate(StartDate, EndDate);
        }
        public DtoList GetUserRoleByAccessCode(string Accesscode)
        {
            return _IGeneralRepository.GetUserRoleByAccessCode(Accesscode);
        }
        public bool StoreNotification(NotificationModel mailrecord)
        {
            return _IGeneralRepository.StoreNotification(mailrecord);
        }
        public NotificationModel GetNotificationDetiailById(int id)
        {
            return _IGeneralRepository.GetNotificationDetiailById(id);
        }
        public int GetUserLevelId(string pageAccessCode)
        {
            return _IGeneralRepository.GetUserLevelId(pageAccessCode);
        }


        public IEnumerable<DtoList> GetPhoneManufacturer()
        {
            return _IGeneralRepository.GetPhoneManufacturer();
        }
        public string DeleteNotification(int notificationId, string email_Id)
        {
            return _IGeneralRepository.DeleteNotification(notificationId, email_Id);
        }
        public IEnumerable<DtoList> GetCustomerStoreUser(Guid userId, string _PageAccessCode)
        {
            return _IGeneralRepository.GetCustomerStoreUser(userId, _PageAccessCode);
        }

        public bool DeActivateByCustomerId(int customerId, Guid modifiedBy)
        {
            bool result;

            if (customerId > 0)
            {
                result = _IGeneralRepository.DeActivateByCustomerId(customerId, modifiedBy);
            }
            else
            {
                result = false;
            }
            return result;

        }
        public bool ActivateByCustomerId(int customerId, Guid modifiedBy)
        {
            bool result;

            if (customerId > 0)
            {
                result = _IGeneralRepository.ActivateByCustomerId(customerId, modifiedBy);
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool DeActivateByLocationId(int locationId, Guid modifiedBy)
        {
            bool result;
            if (locationId > 0)
            {
                result = _IGeneralRepository.DeActivateByLocationId(locationId, modifiedBy);
            }
            else
            {
                result = false;
            }
            return result;
        }
        public string ActivateByLocationId(int locationId, Guid modifiedBy)
        {

            string result = "";
            result = checkValidLocationToActivate(locationId);
            if (result == "")
            {
                bool activated = _IGeneralRepository.ActivateByLocationId(locationId, modifiedBy);
                if (activated == true)
                {
                    result = "";
                }
                else
                {
                    result = "Error in Activate & Deactivate the Location. ";
                }

            }
            return result;


        }
        public IEnumerable<OrderModel> GetHomePagePendingTasks(Guid acctManagerId, int userLevelId, string pageAccessCode, int manufac_ID)
        {
            return _IGeneralRepository.GetHomePagePendingTasks(acctManagerId, userLevelId, pageAccessCode, manufac_ID);
        }
        public string checkValidLocationToActivate(int locationId)
        {
            return _IGeneralRepository.checkValidLocationToActivate(locationId);
        }
        public IEnumerable<DtoList> GetManufacturers()
        {
            return _IGeneralRepository.GetManufacturers();
        }
        public List<ModeConfigurationModel> GetAllConfigModes(ModeConfigurationModel objModeConfigDetails)
        {
            return _IGeneralRepository.GetAllConfigModes(objModeConfigDetails);
        }
        public string AddNewModeConfig(ModeConfigurationModel newModeConfig)
        {
            return _IGeneralRepository.AddNewModeConfig(newModeConfig);
        }
        public ModeConfigurationModel GetModeConfigById(int id)
        {
            return _IGeneralRepository.GetModeConfigById(id);
        }
        public bool UpdateModeConfig(ModeConfigurationModel existingModeConfig)
        {
            return _IGeneralRepository.UpdateModeConfig(existingModeConfig);
        }
        public bool ValidateData(int loc, DateTime from, DateTime to, int modeId)
        {
            return _IGeneralRepository.ValidateData(loc, from, to, modeId);
        }
        public void DeleteModeConfig(int modeID)
        {
            _IGeneralRepository.DeleteModeConfig(modeID);
        }
        public IEnumerable<DtoList> GetModes(Guid user_id)
        {
            return _IGeneralRepository.GetModes(user_id);
        }
        public string AddMachine(WebApiMachineModel objMachine)
        {
            return _IGeneralRepository.AddMachine(objMachine);
        }
        public string DeleteLocationByLocId(int locationId)
        {
            return _IGeneralRepository.DeleteLocationByLocId(locationId);
        }
        public IEnumerable<DtoList> GetLocations()
        {
            return _IGeneralRepository.GetLocations();
        }

        public string GetUserByPageAccessCodeByUserLevelId(int UserLevelId)
        {
            return _IGeneralRepository.GetUserByPageAccessCodeByUserLevelId(UserLevelId);
        }

        public IEnumerable<DtoList> GetSubsidiaryByCustomerId(int customerId)
        {
            return _IGeneralRepository.GetSubsidiaryByCustomerId(customerId);
        }

        public IEnumerable<DtoList> GetSubsidiaryByCustomerId(int customerId, int subsidiaryId)
        {
            return _IGeneralRepository.GetSubsidiaryByCustomerId(customerId, subsidiaryId);
        }

        public IEnumerable<DtoList> GetAgentsBySubsidiaryId(int subsidiaryId)
        {
            return _IGeneralRepository.GetAgentsBySubsidiaryId(subsidiaryId);
        }
        public IEnumerable<DtoList> GetAgentsBySubsidiaryId(int subsidiaryId, int SubAgentId)
        {
            return _IGeneralRepository.GetAgentsBySubsidiaryId(subsidiaryId, SubAgentId);
        }

        public IEnumerable<DtoList> GetMasterValuesByType(string configValue)
        {
            return _IGeneralRepository.GetMasterValuesByType(configValue);
        }


        public IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, bool bActive = false)
        {
            return _IGeneralRepository.GetCustomerLocation(customerId, subsidiaryId, bActive);
        }
        public IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId, bool bActive = false)
        {
            return _IGeneralRepository.GetCustomerLocation(customerId, subsidiaryId, agentId, bActive);
        }
        public IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId)
        {
            return _IGeneralRepository.GetCustomerLocation(customerId, subsidiaryId, agentId);
        }
        public int IncValue()
        {
            return _IGeneralRepository.IncValue();
        }
        public int GetValue()
        {
            return _IGeneralRepository.GetValue();
        }
        public int ResetValue()
        {
            return _IGeneralRepository.ResetValue();

        }

        public string checkValidSubdirectoryToActivate(int subdirectoryId)
        {
            return _IGeneralRepository.checkValidSubdirectoryToActivate(subdirectoryId);
        }

        public string checkValidSubAgentToActivate(int subagentId)
        {
            return _IGeneralRepository.checkValidSubAgentToActivate(subagentId);
        }

        public bool DeActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy)
        {
            return _IGeneralRepository.DeActivateBySubdirectoryId(subdirectoryId, modifiedBy);
        }


        public string ActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy)
        {

            string result = "";
            result = checkValidSubdirectoryToActivate(subdirectoryId);
            if (result == "")
            {
                bool activated = _IGeneralRepository.ActivateBySubdirectoryId(subdirectoryId, modifiedBy);
                if (activated == true)
                {
                    result = "";
                }
                else
                {
                    result = "Error in Activate & Deactivate the Location. ";
                }

            }
            return result;
        }

        public string ActivateBySubAgentId(int subagentId, Guid modifiedBy)
        {

            string result = "";
            result = checkValidSubAgentToActivate(subagentId);
            if (result == "")
            {
                bool activated = _IGeneralRepository.ActivateBySubAgentId(subagentId, modifiedBy);
                if (activated == true)
                {
                    result = "";
                }
                else
                {
                    result = "Error in Activate & Deactivate the Location. ";
                }

            }
            return result;


        }
        public bool DeActivateBySubAgentId(int subagentId, Guid modifiedBy)
        {
            return _IGeneralRepository.DeActivateBySubAgentId(subagentId, modifiedBy);
        }
        public IEnumerable<DtoList> GetLoggingTypes()
        {
            return _IGeneralRepository.GetLoggingTypes();
        }



        public string AddMultipleModeConfig(ModeConfigurationModel newModeConfig, IEnumerable<ModeConfigurationModel> data)
        {
            return _IGeneralRepository.AddMultipleModeConfig(newModeConfig, data);
        }
        public IEnumerable<DtoList> GetPriority()
        {
            return _IGeneralRepository.GetPriority();
        }

        public IEnumerable<DtoList> GetGroups()
        {
            return _IGeneralRepository.GetGroups();
        }
        public string MachineTestResult(MachineTestingModel objMachine)
        {
            MembershipRepository membershipRep = new MembershipRepository();

            string result = "";
            int _machineId = membershipRep.GetMachineIdByMachineIdVal(objMachine.Machine_Id_val);
            if (_machineId == 0)
            {

                result = "Error";
            }
            else
            {
                objMachine.Machine_Id = _machineId;
                result = _IGeneralRepository.MachineTestDataExist(_machineId,objMachine.Machine_Id_val);
                if (result == "")
                {
                    result = _IGeneralRepository.MachineTestResult(objMachine);

                }
            }


            return result;
        }

        public string GroupStatusByLocationId(int _LocationId)
        {
            return _IGeneralRepository.GroupStatusByLocationId(_LocationId);

        }
    }
}
