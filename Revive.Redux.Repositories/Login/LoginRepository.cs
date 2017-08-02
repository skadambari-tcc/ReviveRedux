using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Dapper;
using Revive.Redux.Repositories;
using System.Data;
using Revive.Redux.Entities;

namespace Revive.Redux.Repositories.Login
{
    public class LoginRepository : ILoginRepository
    {

        public bool ValidateUser(String userName, String password)
        {
            var flag = false;
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName && cond.Password == password);

                    if (user != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return flag;
        }
        public DtoUser GetUserDetails(String userName)
        {
            DtoUser objuser = null;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    objuser = (from d in dbContext.Users
                               join c in dbContext.User_Level on d.User_Level_ID equals c.User_Level_ID
                               where d.UserName == userName

                               select new DtoUser
                                    {
                                        UserName = d.UserName,
                                        Email_ID = d.Email_ID,
                                        Cell_Phone = d.Cell_Phone,
                                        Customer_ID = d.Customer_ID,
                                        User_ID = d.User_ID,
                                        User_Level_ID = d.User_Level_ID,
                                        Password = d.Password,
                                        FirstName = d.FirstName,
                                        MiddleName = d.MiddleName,
                                        LastName = d.LastName,
                                        Office_Phone = d.Office_Phone,
                                        Office_phone_etx = d.Office_phone_etx,
                                        Location_ID = d.Location_ID,
                                        Deactivate_Flag = d.Deactivate_Flag,
                                        Created_by = d.Created_by,
                                        Created_Date = d.Created_Date,
                                        Modified_by = d.Modified_by,
                                        Modified_Date = d.Modified_Date,
                                        status = d.status,
                                        IsLockedOut = d.IsLockedOut,
                                        LastPasswordChangeDate = d.LastPasswordChangeDate,
                                        PageAccessCode = dbContext.User_Level.Where(a => a.User_Level_ID == d.User_Level_ID).Select(a => a.Page_Access_Code).FirstOrDefault(),
                                        User_Level_Name = dbContext.User_Level.Where(a => a.User_Level_ID == d.User_Level_ID).Select(a => a.User_Level_Name).FirstOrDefault(),
                                        LastLoginActivity = d.LastLoginActivity,
                                        Manufacturer_Id = (int)d.Manufacturer_Id,
                                        userTypeId = (int)c.UserTypeId,
                                        UserType = dbContext.UserTypes.Where(a => a.Id == c.UserTypeId).Select(a => a.UserType1).FirstOrDefault(),
                                        SubsidiaryId = d.Subsidiary_ID,
                                        SubAgentId = d.SubAgent_ID,
                                        Manufacturer_Ref_Id = dbContext.Manufacturers.Where(a => a.Manufacturer_Id == d.Manufacturer_Id).Select(a => a.Manufacturer_Ref_Code).FirstOrDefault(),
                                        IsStoreUserLog = d.IsUserLogging == null ? false : (bool)d.IsUserLogging,
                                        LoggingTypeCode = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type_ID == d.LoggingTypeID).Select(a => a.MasterData_Value).FirstOrDefault(),
                                        IsMultiDeviceSupported =d.Customer_ID!=null? (dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.IsMultiDevice).FirstOrDefault()!=null?(bool)dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.IsMultiDevice).FirstOrDefault():false):false,
                                        CustomerMaxDevices = d.Customer_ID != null ? (dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.MaxNoOfDevices).FirstOrDefault() != null ? (int)dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.MaxNoOfDevices).FirstOrDefault() : 1) : 1  // 1 for default value of no of devices


                                    }).FirstOrDefault();

                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName);

                    if (user != null)
                    {
                        user.LastLoginActivity = Common.CommonMethods.GetCurrentDate();
                        dbContext.SaveChanges();
                    }

                    return objuser;



                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public Boolean ChangePassword(String email, String newPassword, string oldPassword)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == email);
                    if (user != null)
                    {
                        if (oldPassword == "SKIP")
                        {
                            user.LastPasswordChangeDate = DateTime.Now;
                        }
                        else
                        {
                            user.Password = newPassword;
                            user.IsLockedOut = false;
                            if (oldPassword != "")
                            {
                                user.LastPasswordChangeDate = DateTime.Now;

                            }
                            else
                            {
                                user.LastPasswordChangeDate = null;

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
        public Boolean UpdatePasswordByUserID(UserModels ObjUserModel)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.User_ID == ObjUserModel.UserId);
                    if (user != null)
                    {
                        user.Password = ObjUserModel.NewPassword;
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


        public Boolean CheckEmailAddressExists(String emailAddress)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName.ToLower() == emailAddress.ToLower());
                    if (user != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public Boolean ValidateLockUser(String emailAddress)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName.ToLower() == emailAddress.ToLower() && cond.IsLockedOut == true);
                    if (user != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }


        public Boolean LockUser(String emailAddress)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == emailAddress);
                    if (user != null)
                    {
                        user.IsLockedOut = true;
                        dbContext.SaveChanges();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }


        public bool CheckOldPassword(string email, string newPassword, string oldPassword)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == email);
                    if (user != null)
                    {
                        if (string.Compare(user.Password, oldPassword) == 0)
                        {
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }

        public bool ValidateUser(String userId)
        {
            Guid gUserId = Guid.Parse(userId);
            var flag = false;
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.User_ID == gUserId && cond.status == true);

                    if (user != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return flag;
        }

        public Boolean CheckDuplicateEmailByUserId(String emailAddress, Guid userId)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName.ToLower() == emailAddress.ToLower() && cond.User_ID != userId);
                    if (user != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return flag;
        }

        public int GetUserLevelIdForStoreUser()
        {
            var UserLevelId = 0;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    UserLevelId = dbContext.User_Level.Where(c => c.Page_Access_Code == "STOREUSER").Select(a => a.User_Level_ID).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return UserLevelId;
        }

        public bool ValidateIDPUser(String userName)
        {
            var flag = false;
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName);

                    if (user != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return flag;
        }



    }
}
