using System.Collections.Generic;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;
using System;
using Revive.Redux.Commn;
using System.ComponentModel.DataAnnotations;

namespace Revive.Redux.Services
{
    public class UserManagementService : IUserManagmentService
    {
        private IUserManagementRepository _IUserManagementRepository = null;
        private ILocationManagement _ILocationManagement = null;
        public UserManagementService()
        {
            _IUserManagementRepository = new UserManagementRepository();
            _ILocationManagement = new LocationManagement();
        }
        public bool AddEditUser(UserModels objUser)
        {

            return _IUserManagementRepository.AddEditUser(objUser);
        }
        //public string UpdateUser(DtoUser objUser)
        //{
        //    return _IUserManagementRepository.UpdateUser(objUser);
        //}
        //public string DeactivateUser(DtoUser objUser)
        //{
        //    return _IUserManagementRepository.DeactivateUser(objUser);
        //}
        public int GetStoreUserCountbyLocation(Guid UserId)
        {
            int locId = _IUserManagementRepository.GetStoreUserCountbyLocation(UserId);
            return locId;
        }
        public bool LogStoreUserbyLocation(Guid UserId, int count, bool Status, int LogType)
        {
            return _IUserManagementRepository.LogStoreUserbyLocation(UserId, count, Status, LogType);
        }
        public DtoUser GetUserDetailsByUserId(Guid UserId)
        {
            return _IUserManagementRepository.GetUserDetailsByUserId(UserId);
        }
        public IEnumerable<DtoUser> GetUserDetails(int User_Level_Id)
        {
            return _IUserManagementRepository.GetUserDetails(User_Level_Id);
        }
        public IEnumerable<DtoUser> GetUserDetails(int User_Level_Id, int Customer_Id)
        {
            return _IUserManagementRepository.GetUserDetails(User_Level_Id, Customer_Id);
        }
        public UserModels GetUserById(Nullable<System.Guid> uId)
        {
            return _IUserManagementRepository.GetUserById(uId);
        }
        public string ActivateDeactivateUserByGuid(UserModels objUsermodel)
        {
            string result = "";
            result = checkValidUserToActivate(objUsermodel.UserId);
            if (result == "")
            {
                bool activated = _IUserManagementRepository.ActivateDeactivateUserByGuid(objUsermodel);
                if (activated == true)
                {
                    result = "";
                }
                else
                {
                    result = "Error in Activate & Deactivate the user. ";
                }

            }
            return result;
        }

        public IEnumerable<DtoUser> GetStoreUserDetails(int User_Level_Id, int Customer_Id)
        {
            return _IUserManagementRepository.GetStoreUserDetails(User_Level_Id, Customer_Id);
        }

        public IEnumerable<DtoUser> GetStoreUserDetails(int User_Level_Id, UserModels UserDetails)
        {
            return _IUserManagementRepository.GetStoreUserDetails(User_Level_Id, UserDetails);
        }
        public string checkValidUserToActivate(Guid userId)
        {
            return _IUserManagementRepository.checkValidUserToActivate(userId);
        }
        public List<GroupModel> GetGroupList()
        {
            return _IUserManagementRepository.GetGroupList();


        }
        public bool UserFileUpload(UserModels objUser, out UserModels objUserResult)
        {
            bool bResult = false;
            List<LocationResultModel> lstUserResult = new List<LocationResultModel>();
            List<ValidMailIdForBulkUpload> _ValidUserNameToSendMail = new List<ValidMailIdForBulkUpload>();

            objUserResult = new UserModels();
            int nLineNumber = 1;
            string sMessage = string.Empty;
            string errorMesg = string.Empty;
            List<UserModels> lstUsernModel = FileHandler.ValidateExcelBulkUser(objUser.FileName, out errorMesg);
            LocationManagement locationSer = new LocationManagement();


            foreach (var _user in lstUsernModel)
            {
                _user.Customer_Id = objUser.Customer_Id;
                _user.Created_by = objUser.Created_by;
                _user.Created_Date = Common.CommonMethods.GetCurrentDate();
                _user.emailEdit = _user.Email;
                _user.Manufacturer_Id = 0;
                _user.Pasword = objUser.Pasword.ToMD5HashForPassword();
                _user.SubsidiaryId = objUser.SubsidiaryId;
                _user.SubAgentId = objUser.SubAgentId;

                var _locId = locationSer.GetLocationIdByLocationName(_user.LocationName, (int)_user.Customer_Id);

                if (_locId > 0)
                {
                    _user.Location_Id = locationSer.GetLocationIdByLocationName(_user.LocationName, (int)_user.Customer_Id);

                }

                var validationResults = new List<ValidationResult>();
                bResult = false;
                if (ValidateUserFile(_user, out validationResults))
                {

                    LoginService loginSer = new LoginService();

                    var storeDuplicate = loginSer.CheckEmailAddressExists(_user.Email);// CheckDuplicateStoreNo(objLocation.StoreNumber, objLocation.CustomerId, 0); // Added by KD for Store Duplicate check
                    if (!storeDuplicate)
                    {
                        _user.User_Level_Id = loginSer.GetUserLevelIdForStoreUser();
                        bResult = _IUserManagementRepository.AddEditUser(_user);

                        if (bResult)
                        {
                            ValidMailIdForBulkUpload objMailId = new ValidMailIdForBulkUpload();
                            objMailId.Email_Id = _user.Email;
                            _ValidUserNameToSendMail.Add(objMailId);

                            sMessage = "Successfully Inserted.";
                        }
                    }
                    else
                    {

                        sMessage = "Failed to Inserted. ";
                        sMessage += "; " + "User Email :" + _user.Email.ToString() + " Already Exists.";
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
                                case "Location_Id":
                                    {
                                        keystr += "Location Name";// "Location Name";
                                        break;
                                    }

                                case "FirstName":
                                    {
                                        keystr += "First Name";
                                        break;
                                    }
                                case "LastName":
                                    {
                                        keystr += "Last Name";
                                        break;
                                    }
                                case "UserMobile":
                                    {
                                        keystr += "Mobile";
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

                var UserResult = new LocationResultModel()
                {
                    LineNumber = "Line Number " + nLineNumber,
                    Message = sMessage,
                    Status = bResult

                };

                lstUserResult.Add(UserResult);
                nLineNumber++;
            }
            if (lstUsernModel.Count == 0)
            {
                var UserResult = new LocationResultModel()
                {
                    LineNumber = "Line Number 1",
                    //Message = "Invalid file format.",
                    Message = errorMesg,
                    Status = bResult

                };
                lstUserResult.Add(UserResult);
            }
            objUserResult.UserResult = lstUserResult;
            objUserResult.ValidUserNameToSendMail = _ValidUserNameToSendMail;
            return bResult;
        }
        private bool ValidateUserFile(UserModels objUserModel, out List<ValidationResult> validationResults)
        {

            if (objUserModel.FirstName == "")
            {
                objUserModel.FirstName = null;
            }
            if (objUserModel.LastName == "")
            {
                objUserModel.LastName = null;
            }

            UserModelsBulkUpload objUserModelBulkModel = new UserModelsBulkUpload();
            objUserModelBulkModel.FirstName = objUserModel.FirstName;
            objUserModelBulkModel.LastName = objUserModel.LastName;
            objUserModelBulkModel.UserMobile = objUserModel.UserMobile;
            objUserModelBulkModel.Email = objUserModel.Email;
            objUserModelBulkModel.Location_Id = objUserModel.Location_Id;

            bool bResult = false;


            validationResults = new List<ValidationResult>();

            var context = new ValidationContext(objUserModelBulkModel, serviceProvider: null, items: null);

            bResult = Validator.TryValidateObject(objUserModelBulkModel, context, validationResults, true);

            return bResult;
        }

        public GroupModel GetGroupById(int Id)
        {
            return _IUserManagementRepository.GetGroupById(Id);
        }
        public bool AddEditGroup(GroupModel ObjGroupModel, Guid UserId)
        {
            return _IUserManagementRepository.AddEditGroup(ObjGroupModel, UserId);
        }

        public bool CheckDuplicateGroupName(string GroupName, int GroupId)
        {
            return _IUserManagementRepository.CheckDuplicateGroupName(GroupName, GroupId);
        }

        public bool CheckDuplicatePriority(int PriorityId, int GroupId)
        {
            return _IUserManagementRepository.CheckDuplicatePriority(PriorityId, GroupId);
        }

        public bool ChangeStatusByGroupId(int GroupId, Guid modifiedBy, bool Status)
        {
            return _IUserManagementRepository.ChangeStatusByGroupId(GroupId, modifiedBy, Status);
        }

    }
}
