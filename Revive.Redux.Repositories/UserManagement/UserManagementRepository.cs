using System;
using System.Collections.Generic;
using System.Linq;
using Revive.Redux.Repositories;
using Revive.Redux;
using Revive.Redux.Entities;
using Revive.Redux.Common;


namespace Revive.Redux.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        public bool AddEditUser(UserModels objUser)
        {
            try
            {
                bool isNewUser = false;
                var user = new User();

                using (var dbContext = new ReviveDBEntities())
                {
                    if (objUser.UserId == new Guid())
                    {
                        isNewUser = true;
                    }

                    if (isNewUser == true)
                    {
                        objUser.UserId = Guid.NewGuid();
                        objUser.Status = true;
                        objUser.Created_Date = System.DateTime.Now;
                        user = MaptoUserModelToUser(objUser, user);
                    }
                    else
                    {
                        user = dbContext.Users.FirstOrDefault(us => us.User_ID == objUser.UserId);
                        user.Modified_Date = System.DateTime.Now;
                        user.Modified_by = objUser.Modified_by;
                        user.status = objUser.Status;
                        user = MaptoUserModelToUser(objUser, user);
                        user.UserName = objUser.emailEdit;
                        user.Email_ID = objUser.emailEdit;
                    }



                    if (isNewUser) // in case of new user
                    {

                        var objuser = dbContext.Users.FirstOrDefault(cond => cond.UserName.Trim().ToLower() == objUser.Email.Trim().ToLower());
                        if (objuser == null)
                            dbContext.Users.Add(user);
                    }



                    dbContext.SaveChanges();

                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private User MaptoUserModelToUser(UserModels userModel, User user)
        {
            int _custId = 0;
            int _subsidiaryId = 0;
            int _agentId = 0;
            int _locId = 0;
            int _manufId = 0;
            using (var dbContext = new ReviveDBEntities())
            {
                var objUserLevel = dbContext.User_Level.FirstOrDefault(ul => ul.User_Level_ID == userModel.User_Level_Id);

                if (objUserLevel.Page_Access_Code == PageAccessCode.CUSTOMERADMIN)
                {
                    _custId = (int)userModel.Customer_Id;
                }
                else if (objUserLevel.Page_Access_Code == PageAccessCode.STOREUSER)
                {
                    _custId = (int)userModel.Customer_Id;
                    _subsidiaryId = (int)userModel.SubsidiaryId;
                    _agentId = (int)userModel.SubAgentId;
                    _locId = (int)userModel.Location_Id;
                }
                else if (objUserLevel.Page_Access_Code == PageAccessCode.SUBSIDIARYADMIN)
                {
                    _custId = (int)userModel.Customer_Id;
                    _subsidiaryId = (int)userModel.SubsidiaryId;
                }
                else if (objUserLevel.Page_Access_Code == PageAccessCode.SUBAGENTADMIN)
                {
                    _custId = (int)userModel.Customer_Id;
                    _subsidiaryId = (int)userModel.SubsidiaryId;
                    _agentId = (int)userModel.SubAgentId;
                }
                else if (objUserLevel.Page_Access_Code == PageAccessCode.MFPC || objUserLevel.Page_Access_Code == PageAccessCode.MFASSEMBLY ||
                    objUserLevel.Page_Access_Code == PageAccessCode.MFSHIP)
                {
                    _manufId = (int)userModel.Manufacturer_Id;
                }
                else   // Other Role  i.e. Custom Role
                {
                    var objUserType = dbContext.UserTypes.FirstOrDefault(a => a.Id == objUserLevel.UserTypeId);
                    if (objUserType.UserType1.ToUpper() == "CUSTOMER")
                    {
                        _custId = (int)userModel.Customer_Id;
                        _locId = (int)userModel.Location_Id;
                    }
                    else if (objUserType.UserType1.ToUpper() == "MANUFACTURER")
                    {
                        _manufId = (int)userModel.Manufacturer_Id;
                    }

                }


            }

            user.Cell_Phone = userModel.UserMobile;
            user.User_Level_ID = userModel.User_Level_Id;
            user.Customer_ID = _custId;
            user.Location_ID = _locId;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.status = userModel.Status;
            user.User_ID = userModel.UserId;
            user.Manufacturer_Id = _manufId;
            user.Email_ID = userModel.Email != null ? userModel.Email : user.Email_ID;// for edit mode;
            user.UserName = userModel.Email != null ? userModel.Email : user.Email_ID;// for edit mode;
            user.Created_Date = userModel.Created_Date != null ? userModel.Created_Date : DateTime.Now;// for edit mode
            user.Created_by = userModel.Created_by != null ? userModel.Created_by : user.Created_by;// for edit mode
            user.Password = userModel.Pasword != null ? userModel.Pasword : user.Password;// for edit mode
            user.UserName = userModel.Email != null ? userModel.Email : user.Email_ID;// for edit mode

            user.Subsidiary_ID = _subsidiaryId;
            user.SubAgent_ID = _agentId;
            return user;
        }
        public IEnumerable<DtoUser> GetUserDetails(int User_Level_Id)
        {
            try
            {
                IEnumerable<DtoUser> objUserDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    objUserDetails = dbContext.usp_GetUserDetails(User_Level_Id, 0, "NORMALUSER", 0, 0, 0).Select(d => new DtoUser()
                    {

                        UserName = d.UserName,
                        Email_ID = d.Email,
                        Cell_Phone = d.MobileCode,
                        Customer_ID = d.CustomerId,
                        User_Level_Name = d.UserLevel,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        User_ID = d.User_ID,
                        Location_ID = d.Location_ID,
                        status = d.status,
                        Deactivate_Flag = d.Deactivate_Flag,
                        UserType = d.UserType,
                        Created_Date = d.Created_Date,
                        IsMultiDeviceSupported=d.IsMultiDevice
                    }).ToList();
                }



                return objUserDetails;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public DtoUser GetUserDetailsByUserId(Guid UserId)
        {
            try
            {


                DtoUser userDetails = new DtoUser();
                using (var dbContext = new ReviveDBEntities())
                {
                    var result = dbContext.Users.Where(s => s.User_ID == UserId).FirstOrDefault();
                    userDetails.IsUserLogging = Convert.ToBoolean(result.IsUserLogging);
                    userDetails.User_ID = result.User_ID;
                }



                return userDetails;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public IEnumerable<DtoUser> GetUserDetails(int User_Level_Id, int Customer_Id)
        {
            try
            {


                IEnumerable<DtoUser> objUserDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    objUserDetails = dbContext.usp_GetUserDetails(User_Level_Id, Customer_Id, "NORMALUSER", 0, 0, 0).Select(d => new DtoUser()
                    {

                        UserName = d.UserName,
                        Email_ID = d.Email,
                        Cell_Phone = d.MobileCode,
                        Customer_ID = d.CustomerId,
                        User_Level_Name = d.UserLevel,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        User_ID = d.User_ID,
                        Location_ID = d.Location_ID,
                        Location_Name = d.LocationName,
                        status = d.status,
                        Deactivate_Flag = d.Deactivate_Flag,
                        UserType = d.UserType

                    }).ToList();
                }



                return objUserDetails;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public IEnumerable<DtoUser> GetStoreUserDetails(int User_Level_Id, int Customer_Id)
        {
            try
            {


                IEnumerable<DtoUser> objUserDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    objUserDetails = dbContext.usp_GetUserDetails(User_Level_Id, Customer_Id, "STOREUSER", 0, 0, 0).Select(d => new DtoUser()
                    {

                        UserName = d.UserName,
                        Email_ID = d.Email,
                        Cell_Phone = d.MobileCode,
                        Customer_ID = d.CustomerId,
                        User_Level_Name = d.UserLevel,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        User_ID = d.User_ID,
                        Location_ID = d.Location_ID,
                        Location_Name = d.LocationName,
                        status = d.status,
                        Deactivate_Flag = d.Deactivate_Flag,
                        UserType = d.UserType,
                        IsUserLogging = d.IsUserLogging,
                        LogTypeId = d.LoggingTypeId

                    }).ToList();
                }



                return objUserDetails;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public IEnumerable<DtoUser> GetStoreUserDetails(int User_Level_Id, UserModels UserDetails)
        {
            try
            {


                IEnumerable<DtoUser> objUserDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    objUserDetails = dbContext.usp_GetUserDetails(User_Level_Id, UserDetails.Customer_Id, "STOREUSER", UserDetails.SubsidiaryId, UserDetails.SubAgentId, UserDetails.Location_Id).Select(d => new DtoUser()
                    {

                        UserName = d.UserName,
                        Email_ID = d.Email,
                        Cell_Phone = d.MobileCode,
                        Customer_ID = d.CustomerId,
                        User_Level_Name = d.UserLevel,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        User_ID = d.User_ID,
                        Location_ID = d.Location_ID,
                        Location_Name = d.LocationName,
                        status = d.status,
                        Deactivate_Flag = d.Deactivate_Flag,
                        UserType = d.UserType,
                        IsUserLogging = d.IsUserLogging,
                        LogTypeId = d.LoggingTypeId,
                        CustomerName = d.CustomerName,
                        SubAgentName = d.SubAgentName,
                        FullName = d.FirstName + ' ' + d.LastName != null ? d.FirstName + ' ' + d.LastName : ""

                    }).ToList();
                }



                return objUserDetails;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public UserModels GetUserById(Nullable<System.Guid> uId)
        {
            var result = new UserModels();
            try
            {
                if (uId != null)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var user = dbContext.Users.FirstOrDefault(d => d.User_ID == uId);

                        if (user != null)
                        {
                            result = new UserModels
                            {
                                Email = user.UserName,
                                FirstName = user.FirstName,
                                LastName = !string.IsNullOrEmpty(user.LastName) ? user.LastName : string.Empty,
                                UserMobile = user.Cell_Phone,
                                Customer_Id = user.Customer_ID,
                                Location_Id = user.Location_ID,
                                User_Level_Id = user.User_Level_ID,
                                Status = user.status,
                                PageAccessCode = dbContext.User_Level.FirstOrDefault(c => (c.User_Level_ID == user.User_Level_ID)).Page_Access_Code,
                                Manufacturer_Id = Convert.ToInt32(user.Manufacturer_Id),
                                emailEdit = user.UserName,
                                SubsidiaryId = user.Subsidiary_ID == null ? 0 : (int)(user.Subsidiary_ID),
                                SubAgentId = user.SubAgent_ID == null ? 0 : (int)(user.SubAgent_ID)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool ActivateDeactivateUserByGuid(UserModels objUsermodel)
        {

            var result = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (objUsermodel.UserId != new Guid())
                    {
                        var UserRecord = dbContext.Users.FirstOrDefault(usr => usr.User_ID == objUsermodel.UserId);
                        if (UserRecord != null)
                        {
                            UserRecord.status = objUsermodel.Status;
                            if (UserRecord.status == true)
                            {
                                UserRecord.status = false;
                            }
                            else
                            {
                                UserRecord.status = true;
                            }
                        }
                        dbContext.SaveChanges();
                        result = true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        public void ChangeUserStatus(ReviveDBEntities dbcontext, int CustomerId)
        {
            var objUserList = dbcontext.Users.Where(cond => cond.Customer_ID == CustomerId);
            if (objUserList != null)
            {
                foreach (User objuser in objUserList)
                {

                    objuser.status = false;
                }
            }
            dbcontext.SaveChanges();
        }
        private int GetLocationId(Guid UserId)
        {
            using (var dbContext = new ReviveDBEntities())
            {
                var locationresult = dbContext.Users.Where(s => s.User_ID == UserId).FirstOrDefault();
                int locationId = 0;
                if (locationresult != null)
                {
                    locationId = Convert.ToInt32(locationresult.Location_ID);
                }
                return locationId;
            }
        }
        public int GetStoreUserCountbyLocation(Guid UserId)
        {
            int locationId = GetLocationId(UserId);
            using (var dbContext = new ReviveDBEntities())
            {
                var result = dbContext.Users.Where(s => s.Location_ID == locationId && (s.IsUserLogging == false || s.IsUserLogging == null)).Count();
                int count = 0;
                if (result != null)
                {
                    count = result;
                }
                return count;
            }
        }
        public bool LogStoreUserbyLocation(Guid UserId, int count, bool Status, int LogType)
        {
            bool returnstatus = true;
            using (var dbContext = new ReviveDBEntities())
            {
                if (count > 0)
                {
                    int locationId = GetLocationId(UserId);
                    var result = dbContext.Users.Where(s => s.Location_ID == locationId && (s.IsUserLogging == false || s.IsUserLogging == null)).ToList();
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.IsUserLogging = true;
                            item.Modified_Date = DateTime.Now;
                            item.LoggingTypeID = LogType;
                        }
                    }
                    returnstatus = true;
                }
                else
                {
                    var result = dbContext.Users.Where(s => s.User_ID == UserId).FirstOrDefault();
                    if (result != null)
                    {
                        if (Status == true)
                        {
                            result.IsUserLogging = true;
                            result.Modified_Date = DateTime.Now;
                            returnstatus = true;
                            result.LoggingTypeID = LogType;
                        }
                        else
                        {
                            result.IsUserLogging = false;
                            result.Modified_Date = DateTime.Now;
                            returnstatus = false;
                            result.LoggingTypeID = LogType;
                        }

                    }
                }

                dbContext.SaveChanges();
            }
            return returnstatus;
        }
        /// <summary>
        /// Check Customer & Location Activate Status in case of Deactivating Store User ,Customer Admin
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public string checkValidUserToActivate(Guid userId)
        {
            var validUser = true;
            int customerId = 0;
            int locationId = 0;
            int manufacturerId = 0;
            int subdirectoryId = 0;
            int subagentId = 0;
            string result = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var usrdetails = dbContext.Users.FirstOrDefault(usr => usr.User_ID == userId);
                    if (usrdetails != null)
                    {

                        customerId = usrdetails.Customer_ID != null ? (int)usrdetails.Customer_ID : 0;
                        locationId = usrdetails.Location_ID != null ? (int)usrdetails.Location_ID : 0;
                        manufacturerId = usrdetails.Manufacturer_Id != null ? (int)usrdetails.Manufacturer_Id : 0;
                        subdirectoryId = usrdetails.Subsidiary_ID != null ? (int)usrdetails.Subsidiary_ID : 0;
                        subagentId = usrdetails.SubAgent_ID != null ? (int)usrdetails.SubAgent_ID : 0;
                    }


                    if (customerId > 0 && validUser == true)
                    {

                        var customerRecord = dbContext.Customers.FirstOrDefault(cust => cust.Customer_ID == customerId);
                        if (customerRecord.Status == false)
                        {
                            validUser = false;
                            result = "Customer: " + customerRecord.Customer_Name + " has been deactivated. You can not activate this user.";
                        }


                    }
                    if (locationId > 0 && validUser == true)
                    {

                        var locationRecord = dbContext.Customers_Locations.FirstOrDefault(loc => loc.Location_ID == locationId);
                        if (locationRecord.Status == false)
                        {
                            validUser = false;
                            result = "Location: " + locationRecord.Location_Name + " has been deactivated. You can not activate this user.";
                        }


                    }
                    if (manufacturerId > 0 && validUser == true)
                    {
                        var MF_Record = dbContext.Manufacturers.FirstOrDefault(mf => mf.Manufacturer_Id == manufacturerId);
                        if (MF_Record.Status == false)
                        {
                            validUser = false;
                            result = "Manufacturer: " + MF_Record.Manufacturer_Name + " has been deactivated. You can not activate this user.";
                        }
                    }
                    if (subdirectoryId > 0 && validUser == true)
                    {
                        var SubDirectory_Record = dbContext.Subsidiaries.FirstOrDefault(sd => sd.Subsidiary_ID == subdirectoryId);
                        if (SubDirectory_Record.Status == false)
                        {
                            validUser = false;
                            result = "Subdirectory: " + SubDirectory_Record.SubsidiaryName + " has been deactivated. You can not activate this user.";
                        }
                    }
                    if (subagentId > 0 && validUser == true)
                    {
                        var Subagent_Record = dbContext.SubAgents.FirstOrDefault(se => se.SubAgent_ID == subagentId);
                        if (Subagent_Record.Status == false)
                        {
                            validUser = false;
                            result = "Sub Agent: " + Subagent_Record.SubAgentName + " has been deactivated. You can not activate this user.";
                        }
                    }

                }
                return result;

            }
            catch
            {
                throw;
            }
        }

        #region Location group

        public List<GroupModel> GetGroupList()
        {
            try
            {
                List<GroupModel> GrouplstObj;

                using (var dbContext = new ReviveDBEntities())
                {
                    GrouplstObj = dbContext.LocationGroups.Where(m => m.Status == true).Select(d => new GroupModel()
                    {
                        GroupName = d.GroupName,
                        GroupId = d.GroupId,
                        Priority_Id = d.Priority_Id,
                        PriorityName = d.GroupsPriority.PriorityName,
                        GroupStatus = (bool)d.Status,
                        ShippingDate = (DateTime)d.ShippingDate
                    }).ToList();

                }
                return GrouplstObj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public GroupModel GetGroupById(int Id)
        {
            var result = new GroupModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (Id > 0)
                    {
                        var ConfigMacHist = (from objLocGroupModel in dbContext.LocationGroups
                                             where objLocGroupModel.GroupId == Id && objLocGroupModel.Status == true
                                             select new GroupModel
                                             {
                                                 GroupId = objLocGroupModel.GroupId,
                                                 GroupName = objLocGroupModel.GroupName,
                                                 Priority_Id = (int)objLocGroupModel.Priority_Id,
                                                 ShippingDate = (DateTime)objLocGroupModel.ShippingDate
                                             }).FirstOrDefault();

                        result = ConfigMacHist;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool AddEditGroup(GroupModel ObjGroupModel, Guid UserId)
        {
            bool flag = false;
            var objLocationGroup = new LocationGroup();
            var isNew = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (ObjGroupModel.GroupId == 0)
                    {
                        isNew = true;
                    }
                    else // in case of edit Group
                    {
                        objLocationGroup = dbContext.LocationGroups.FirstOrDefault(g => g.GroupId == ObjGroupModel.GroupId && g.Status == true);
                    }

                    objLocationGroup = MapGroup(ObjGroupModel, objLocationGroup);

                    if (isNew) // in case of new Group
                    {
                        // Restrict adding duplicate Group
                        var objConfigureHistory =
                            dbContext.LocationGroups.FirstOrDefault(cond => cond.GroupId == ObjGroupModel.GroupId);
                        if (objConfigureHistory == null)
                            dbContext.LocationGroups.Add(objLocationGroup);
                        objLocationGroup.Created_Date = DateTime.Now;
                        objLocationGroup.Created_by = UserId;
                    }
                    else
                    {
                        objLocationGroup.Modified_Date = DateTime.Now;
                        objLocationGroup.Modified_by = UserId;

                        var MapLocationByGroup =
                           dbContext.MapLocationByGroups.Where(x => x.GroupId == ObjGroupModel.GroupId);
                        foreach (var item in MapLocationByGroup)
                        {
                            if (ObjGroupModel.ShippingDate != null)
                            {
                                item.ShippingDate = (DateTime)ObjGroupModel.ShippingDate;
                            }
                        }
                    }

                    dbContext.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }

        private LocationGroup MapGroup(GroupModel objGroupModel, LocationGroup objLocationGroup)
        {
            objLocationGroup.GroupName = objGroupModel.GroupName;
            objLocationGroup.Priority_Id = Convert.ToInt32(objGroupModel.Priority_Id);
            objLocationGroup.Status = true;
            objLocationGroup.ShippingDate = objGroupModel.ShippingDate;

            return objLocationGroup;
        }

        public bool CheckDuplicateGroupName(string GroupName, int GroupId)
        {
            try
            {
                bool isExists = false;
                using (var dbContext = new ReviveDBEntities())
                {
                    var allGroupName = new List<string>();
                    if (GroupId != 0)
                    {
                        allGroupName = (from lc in dbContext.LocationGroups where lc.GroupName.ToLower() == GroupName.ToLower() && lc.GroupId != GroupId && lc.Status == true select (string)lc.GroupName).ToList();
                    }
                    else
                        allGroupName = (from lc in dbContext.LocationGroups where lc.GroupName.ToLower() == GroupName.ToLower() && lc.Status == true select (string)lc.GroupName).ToList();
                    if (allGroupName.Any())
                        isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckDuplicatePriority(int PriorityId, int GroupId)
        {
            try
            {
                bool isExists = false;
                using (var dbContext = new ReviveDBEntities())
                {
                    var allGroupName = new List<int>();
                    if (GroupId != 0)
                    {
                        allGroupName = (from lc in dbContext.LocationGroups where lc.Priority_Id == PriorityId && lc.GroupId != GroupId && lc.Status == true select (int)lc.Priority_Id).ToList();
                    }
                    else
                        allGroupName = (from lc in dbContext.LocationGroups where lc.Priority_Id == PriorityId && lc.Status == true select (int)lc.Priority_Id).ToList();
                    if (allGroupName.Any())
                        isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ChangeStatusByGroupId(int GroupId, Guid modifiedBy, bool Status)
        {
            var result = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (GroupId != 0)
                    {
                        if (Status == true)
                        {
                            UpdateStatusByGroupId(dbContext, GroupId, Status, modifiedBy);
                            result = true;
                        }
                        else if (Status == false)
                        {
                            var IsLocationMappedWithGroup = dbContext.Customers_Locations.Where(m => m.GroupId == GroupId).Count() > 0;
                            if (!IsLocationMappedWithGroup)
                            {
                                UpdateStatusByGroupId(dbContext, GroupId, Status, modifiedBy);
                                result = true;
                            }
                        }
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        private void UpdateStatusByGroupId(ReviveDBEntities dbContext, int GroupId, bool Status, Guid modifiedBy)
        {
            var recordDeactivate = dbContext.LocationGroups.FirstOrDefault(m => m.GroupId == GroupId);
            if (recordDeactivate != null)
            {
                recordDeactivate.Status = Status;
                recordDeactivate.Modified_by = modifiedBy;
                recordDeactivate.Modified_Date = System.DateTime.Now;
            }
        }

        #endregion




    }
}
