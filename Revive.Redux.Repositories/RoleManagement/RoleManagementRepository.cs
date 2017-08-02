using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux;
using Revive.Redux.Entities;

namespace Revive.Redux.Repositories
{
    public class RoleManagementRepository : IRoleManagementRepository
    {
        /// <summary>
        /// Add New User Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public string AddRole(User_LevelModel role)
        {
            string ReturnVal = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var newRole = new User_Level();
                    newRole.User_Level_Name = role.User_Level_Name;
                    newRole.Created_Date = System.DateTime.Now;
                    newRole.UserTypeId = role.UserTypeId;
                    //newRole.Aud_User = "Admin";
                    //newRole.Role_Type_Id = 1;
                    dbContext.User_Level.Add(newRole);
                    dbContext.SaveChanges();
                    ReturnVal = newRole.User_Level_Name + " successFully added";
                }
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        /// <summary>
        /// Delete User Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public string DeleteRole(User_LevelModel role)
        {
            //var result = false;
            string ReturnVal = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    // var user=dbContext.Users.FirstOrDefault(cond => cond.User_Level_ID == role.User_Level_ID).

                    var UserCount = dbContext.Users.Where(i => i.User_Level_ID == role.User_Level_ID).Select(x => x.User_Level_ID).Count();
                    var record = dbContext.User_Level.FirstOrDefault(cond => cond.User_Level_ID == role.User_Level_ID);
                    var menumapCount = dbContext.User_Level_Menu_Mapping.Where(i => i.User_Level_Id == role.User_Level_ID).Select(x => x.User_Level_Id).Count();
                    var controllerActionMapCount = dbContext.UserMap_Controller_Action.Where(i => i.User_Level_ID == role.User_Level_ID).Select(x => x.User_Level_ID).Count();
                    int RecordExistCount = UserCount + menumapCount + controllerActionMapCount;

                    if (RecordExistCount == 0)
                    {
                        if ((role.User_Level_ID) != 0)
                        {
                            if (record != null)
                            {
                                dbContext.User_Level.Remove(record);
                                ReturnVal = "";
                            }


                        }
                    }
                    else
                    {
                        ReturnVal = "Role: '" + record.User_Level_Name + "' can't be deleted, as it is assigned with user or Menus.";
                    }
                    // save the changes
                    dbContext.SaveChanges();

                }


            }
            catch (Exception ex)
            {
                // ReturnVal = ex.Message.ToString();
                throw;
            }

            return ReturnVal;
        }

        /// <summary>
        /// Update User Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Boolean UpdateRole(User_LevelModel role)
        {
            var flag = false;
            try
            {
                if (role != null)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var roleupdate = dbContext.User_Level.FirstOrDefault(cond => cond.User_Level_ID == role.User_Level_ID);
                        if (roleupdate != null)
                        {
                            roleupdate.User_Level_Name = role.User_Level_Name;
                            roleupdate.UserTypeId = role.UserTypeId;
                            roleupdate.Modified_by = role.Modified_by;
                            roleupdate.Modified_Date = DateTime.Now;
                        }
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

        /// <summary>
        /// Get User Role By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User_LevelModel GetRoleById(int id)
        {
            var result = new User_LevelModel();
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {

                    var role = (from User_Level rl in dbContext.User_Level.ToList()
                                where rl.User_Level_ID == id
                                select new User_LevelModel
                                {
                                    User_Level_Name = rl.User_Level_Name,
                                    UserTypeId = rl.UserTypeId.Value,
                                    User_Level_ID = rl.User_Level_ID
                                }).FirstOrDefault();

                    result = role;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        public int GetRoleIDForUser(string username)
        {
            int roleID = 0;
            if (string.IsNullOrEmpty(username))
            {
                return 0;
            }
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    User user = null;

                    var role = (from users in dbContext.Users
                                join selRole in dbContext.User_Level on users.User_Level_ID equals selRole.User_Level_ID
                                where users.UserName == username
                                select selRole.User_Level_ID).FirstOrDefault();

                    roleID = role;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return roleID;
        }
        public User_LevelModel GetRoleForUser(string userName)
        {
            return GetRoleById(GetRoleIDForUser(userName));
        }

        /// <summary>
        /// Get All User Roles i.e All Records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDetailsModel> GetRoleList()
        {
            try
            {
                IEnumerable<RoleDetailsModel> ObjRoleDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    ObjRoleDetails = dbContext.usp_GetRoleDetails().Select(d => new RoleDetailsModel()
                    {
                        Role_id = d.Role_id,
                        Role_Name = d.Role_Name,
                        UserType = d.UserType,
                        Access_Type_Name = d.Access_Type_Name,
                        Role_Type_Id = d.Role_Type_Id,
                        page_access_code = d.Page_Access_code
                    }).ToList();

                }

                return ObjRoleDetails;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public IEnumerable<Role> GetAuthUserRole(int UserLevelId)
        {
            try
            {
                var collection = new List<Role>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstRole = dbContext.usp_GetControllerActionByUserLevelID(UserLevelId)
                                      .Select(d => new Role()
                                      {
                                          MapId = d.Map_Id,
                                          ControllerId = d.Controller_Id,
                                          ControllerName = d.ControllerName,
                                          ActionId = d.Action_Id,
                                          ActionName = d.Action_Name,
                                          UserLevelID = d.User_Level_ID
                                      });

                    collection.AddRange(lstRole);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public bool CheckRoleExists(User_LevelModel Object)
        {
            var flag = false;
            var Record = new List<int>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (Object.User_Level_ID != 0)
                    {
                        Record = (from ul in dbContext.User_Level
                                  where ul.User_Level_Name.ToLower() == Object.User_Level_Name.ToLower()
                                      && ul.User_Level_ID != Object.User_Level_ID
                                  select ul.User_Level_ID).ToList();
                    }
                    else
                        Record = (from ul in dbContext.User_Level where ul.User_Level_Name.ToLower() == Object.User_Level_Name.ToLower() select ul.User_Level_ID).ToList();
                    //Record = dbContext.User_Level.FirstOrDefault(cond => cond.User_Level_Name.ToLower() == Object.User_Level_Name.ToLower());
                    if (Record.Any())
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

        public string AddNewRole(User_LevelModel role)
        {
            string ReturnVal = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (role != null)
                    {
                        var objRole = new User_Level
                        {
                            User_Level_Name = role.User_Level_Name,
                            UserTypeId = role.UserTypeId,
                            Created_by = role.Created_by,
                            Created_Date = DateTime.Now
                        };
                        dbContext.User_Level.Add(objRole);
                        dbContext.SaveChanges();
                        ReturnVal = objRole.User_Level_Name + " successfully added";
                    }
                }
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CheckDuplicateRoleName(string User_Level_Name, int User_Level_ID)
        {
            try
            {
                bool isExists = false;
                using (var dbContext = new ReviveDBEntities())
                {
                    var allCustName = new List<int>();
                    if (User_Level_ID != 0)
                    {
                        allCustName = (from us in dbContext.User_Level where us.User_Level_Name.ToLower() == User_Level_Name.ToLower() && us.User_Level_ID != User_Level_ID select us.User_Level_ID).ToList();
                    }
                    else
                        allCustName = (from us in dbContext.User_Level where us.User_Level_Name.ToLower() == User_Level_Name.ToLower() select us.User_Level_ID).ToList();
                    if (allCustName.Any())
                        isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


