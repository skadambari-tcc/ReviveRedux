using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux;
using Revive.Redux.Entities;
using System.Net.Mail;
using Revive.Redux.Common;
using System.Data.Entity.Core.Objects;

namespace Revive.Redux.Repositories
{
    public class GeneralRepository : IGeneralRepository
    {
        public IEnumerable<DtoList> GetEmptyDDL()
        {
            try
            {
                var collection = new List<DtoList>();
                collection.Add(new DtoList { Text = "--Select--", Id = 0 });
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetEmptyDDLWithoutSelect()
        {
            try
            {
                var collection = new List<DtoList>();
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetCustomer(bool bActive = false)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {

                    var lstcollection = (from d in dbContext.Customers
                                         where d.Status == true
                                         select new DtoList
                                         {
                                             Id = d.Customer_ID,
                                             Text = d.Customer_Name
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetCustomerStoreUser(Guid userId, string _PageAccessCode)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    if (_PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                    {
                        collection = (from d in dbContext.Customers
                                      join usr in dbContext.Users on d.Customer_ID equals usr.Customer_ID
                                      where d.Status == true && usr.User_ID == userId

                                      select new DtoList
                                      {
                                          Id = d.Customer_ID,
                                          Text = d.Customer_Name,
                                          otherIntVal=d.TemplateId
                                      }).OrderBy(z => z.Text).ToList();
                    }
                    else if (_PageAccessCode == PageAccessCode.ACCNTMGR)
                    {
                        collection = (from d in dbContext.Customers
                                      join usr in dbContext.Users on d.AccountmanagerUserID equals usr.User_ID
                                      where d.Status == true && usr.User_ID == userId
                                      select new DtoList
                                      {
                                          Id = d.Customer_ID,
                                          Text = d.Customer_Name,
                                          otherIntVal = d.TemplateId
                                      }).OrderBy(z => z.Text).ToList();
                    }
                    else if (_PageAccessCode == PageAccessCode.SUBSIDIARYADMIN || _PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                    {
                        collection = (from d in dbContext.Customers
                                      join s in dbContext.Subsidiaries on d.Customer_ID equals s.Customer_ID
                                      join usr in dbContext.Users on s.Subsidiary_ID equals usr.Subsidiary_ID
                                      where d.Status == true && usr.User_ID == userId
                                      select new DtoList
                                      {
                                          Id = d.Customer_ID,
                                          Text = d.Customer_Name,
                                          otherIntVal = d.TemplateId
                                      }).OrderBy(z => z.Text).ToList();
                    }
                    else if (_PageAccessCode == PageAccessCode.ADMIN || _PageAccessCode == PageAccessCode.SUPERADMIN || _PageAccessCode == PageAccessCode.CLIENTEXEC)
                    {
                        collection = (from d in dbContext.Customers
                                      where d.Status == true
                                      select new DtoList
                                      {
                                          Id = d.Customer_ID,
                                          Text = d.Customer_Name,
                                          otherIntVal = d.TemplateId
                                      }).OrderBy(z => z.Text).ToList();
                    }



                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetTemplate()
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                    // collection.Add(selectListItem);
                    var tempcoll = (from d in dbContext.Debug_Templates
                                    select new DtoList
                                    {
                                        Id = d.Template_ID,
                                        Text = d.Template_name,
                                        otherStrVal = d.VersionNumber


                                    }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(tempcoll);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetUserRole()
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                    // collection.Add(selectListItem);

                    var lstcollection = (from d in dbContext.User_Level
                                         where d.Page_Access_Code != PageAccessCode.SUPERADMIN
                                         select new DtoList
                                         {
                                             Id = d.User_Level_ID,
                                             Text = d.User_Level_Name,
                                             otherIntVal = 0
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DtoList GetUserRoleByAccessCode(string Accesscode)
        {
            try
            {
                var record = new DtoList();
                using (var dbContext = new ReviveDBEntities())
                {
                    // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                    // collection.Add(selectListItem);
                    if (Accesscode != null)
                    {
                        record = (from d in dbContext.User_Level
                                  where d.Page_Access_Code == Accesscode
                                  select new DtoList
                                  {
                                      Id = d.User_Level_ID,
                                      Text = d.User_Level_Name,
                                      otherIntVal = 0
                                  }).FirstOrDefault();
                    }
                }
                return record;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IEnumerable<DtoList> GetCustomerLocation(int customerId, bool bActive = false)
        {
            try
            {
                var collection = new List<DtoList>();
                //List<DtoList> selectListItem = new List<DtoList>();
                //selectListItem.Add(new DtoList { Text = "--Select--", Id = 0 });
                using (var dbContext = new ReviveDBEntities())
                {

                    var lstLocation = (from d in dbContext.Customers_Locations
                                       where d.Customer_ID == customerId && d.Status == true
                                       select new DtoList
                                       {
                                           Id = d.Location_ID,
                                           Text = d.Location_Name
                                       }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstLocation);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, bool bActive = false)
        {
            try
            {
                var collection = new List<DtoList>();
                //List<DtoList> selectListItem = new List<DtoList>();
                //selectListItem.Add(new DtoList { Text = "--Select--", Id = 0 });
                using (var dbContext = new ReviveDBEntities())
                {

                    var lstLocation = (from d in dbContext.Customers_Locations
                                       where d.Customer_ID == customerId && d.Status == true && d.Subsidiary_ID == subsidiaryId
                                       select new DtoList
                                       {
                                           Id = d.Location_ID,
                                           Text = d.Location_Name
                                       }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstLocation);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId, bool bActive = false)
        {
            try
            {
                var collection = new List<DtoList>();
                //List<DtoList> selectListItem = new List<DtoList>();
                //selectListItem.Add(new DtoList { Text = "--Select--", Id = 0 });
                using (var dbContext = new ReviveDBEntities())
                {

                    var lstLocation = (from d in dbContext.Customers_Locations
                                       where d.Customer_ID == customerId && d.Status == true && d.Subsidiary_ID == subsidiaryId && d.SubAgent_ID == agentId
                                       select new DtoList
                                       {
                                           Id = d.Location_ID,
                                           Text = d.Location_Name
                                       }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstLocation);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// GetCustomerLocation
        /// </summary>
        /// <param name="customerId,subsidiaryId,agentId"></param>
        /// <returns>all Location(Active and Inactive Both)</returns>
        public IEnumerable<DtoList> GetCustomerLocation(int customerId, int subsidiaryId, int agentId)
        {
            try
            {
                var collection = new List<DtoList>();
                //List<DtoList> selectListItem = new List<DtoList>();
                //selectListItem.Add(new DtoList { Text = "--Select--", Id = 0 });
                using (var dbContext = new ReviveDBEntities())
                {

                    var lstLocation = (from d in dbContext.Customers_Locations
                                       where d.Customer_ID == customerId && d.Subsidiary_ID == subsidiaryId && d.SubAgent_ID == agentId
                                       select new DtoList
                                       {
                                           Id = d.Location_ID,
                                           Text = d.Location_Name
                                       }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstLocation);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<DtoList> GetUserType()
        {
            try
            {
                var collection = new List<DtoList>();

                // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstUserTypes = (from d in dbContext.UserTypes
                                        select new DtoList
                                        {
                                            Id = d.Id,
                                            Text = d.UserType1

                                        }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstUserTypes);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// GetUserLevelType will return except SuperAdmin
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DtoList> GetUserLevelType()
        {
            try
            {
                var collection = new List<DtoList>();

                // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstUserTypes = (from d in dbContext.User_Level
                                        where d.Page_Access_Code != PageAccessCode.SUPERADMIN
                                        select new DtoList
                                        {
                                            Id = d.User_Level_ID,
                                            Text = d.User_Level_Name

                                        }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstUserTypes);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// All User type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DtoList> GetUserLevelType(string userType)
        {
            try
            {
                var collection = new List<DtoList>();

                if (userType == "ALL")
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstUserTypes = (from d in dbContext.User_Level
                                            //    where d.Page_Access_Code == PageAccessCode.SUPERADMIN
                                            select new DtoList
                                            {
                                                Id = d.User_Level_ID,
                                                Text = d.User_Level_Name

                                            }).OrderBy(z => z.Text).ToList();
                        collection.AddRange(lstUserTypes);

                    }
                }
                else
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var lstUserTypes = (from d in dbContext.User_Level
                                            // Changes done for showing PendingApproval tasks to Admin & SuperAdmin also
                                            //where d.Page_Access_Code != null && (d.Page_Access_Code == PageAccessCode.MFSHIP || d.Page_Access_Code == PageAccessCode.CLIENTEXEC || d.Page_Access_Code == PageAccessCode.APPROVER || d.Page_Access_Code == PageAccessCode.MFPC || d.Page_Access_Code == PageAccessCode.MFASSEMBLY || d.Page_Access_Code == PageAccessCode.ACCNTMGR)
                                            where d.Page_Access_Code != null && (d.Page_Access_Code == PageAccessCode.MFSHIP || d.Page_Access_Code == PageAccessCode.CLIENTEXEC || d.Page_Access_Code == PageAccessCode.APPROVER || d.Page_Access_Code == PageAccessCode.MFPC || d.Page_Access_Code == PageAccessCode.MFASSEMBLY || d.Page_Access_Code == PageAccessCode.ACCNTMGR || d.Page_Access_Code == PageAccessCode.SUPERADMIN || d.Page_Access_Code == PageAccessCode.ADMIN)
                                            select new DtoList
                                            {
                                                Id = d.User_Level_ID,
                                                Text = d.User_Level_Name

                                            }).OrderBy(z => z.Text).ToList();
                        collection.AddRange(lstUserTypes);

                    }
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetStoreUserLevelType()
        {
            try
            {
                var collection = new List<DtoList>();

                // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstUserTypes = (from d in dbContext.User_Level
                                        join usertyp in dbContext.UserTypes on d.UserTypeId equals usertyp.Id
                                        where usertyp.UserType1 == "Customer"
                                        select new DtoList
                                        {
                                            Id = d.User_Level_ID,
                                            Text = d.User_Level_Name

                                        }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstUserTypes);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<DtoList> GetMachineSpecs()
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    //var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                    //collection.Add(selectListItem);
                    var lstcollection = (from d in dbContext.MachineSpecs
                                         select new DtoList
                                         {
                                             Id = d.MachineSpec_ID,
                                             Text = d.Generation
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetCustomerDocs(int custId)
        {
            try
            {
                var collection = new List<DtoList>();
                //var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                //collection.Add(selectListItem);
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Customer_Documents
                                         where d.Customer_ID == custId
                                         select new DtoList
                                         {
                                             Id = d.Customer_Doc_ID,
                                             Text = d.Customer_Doc_Name,
                                             otherStrVal = d.Doc_Path,
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetCity(int Id)
        {
            try
            {
                var collection = new List<DtoList>();
                //var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                //collection.Add(selectListItem);
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Cities
                                         where d.StateId == Id
                                         select new DtoList
                                       {
                                           Id = d.CityId,
                                           Text = d.city1,
                                           otherStrVal = d.state_code
                                       }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetState()
        {
            try
            {
                var collection = new List<DtoList>();
                //var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                //collection.Add(selectListItem);
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.States
                                         select new DtoList
                                         {
                                             Id = d.StateId,
                                             Text = d.state_code, // Updatint state code to display in Dropdown text As per Client Mail and discussion made with Vishal on dated 23-Feb-2016
                                             otherStrVal = d.state_code
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetAccountManagerDetail()
        {
            try
            {
                var collection = new List<DtoList>();

                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Users
                                         join ul in dbContext.User_Level on d.User_Level_ID equals ul.User_Level_ID
                                         where (ul.Page_Access_Code == PageAccessCode.ACCNTMGR || ul.Page_Access_Code == PageAccessCode.CLIENTEXEC)
                                         && d.status == true
                                         select new DtoList
                                         {
                                             CompleteId = d.User_ID,
                                             Text = d.FirstName + " " + d.LastName,
                                             otherStrVal = d.LastName
                                         }).OrderBy(z => z.Text).ToList();

                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<DtoList> GetServiceType()
        {
            try
            {
                var collection = new List<DtoList>();

                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from param in dbContext.MasterData_Config_definitions
                                         where param.MasterData_Type == "ServiceType"

                                         select new DtoList
                                         {

                                             Id = param.MasterData_Type_ID,
                                             Text = param.MasterData_Value,

                                         }).ToList();


                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Debug_Templates_DefinitionModel> GetParameters()
        {

            var result = new List<Debug_Templates_DefinitionModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from param in dbContext.MasterData_Config_definitions
                              where param.MasterData_Type == "Parameter"

                              select new Debug_Templates_DefinitionModel
                              {

                                  DebugParameter_Id = param.MasterData_Type_ID,
                                  DebugParameterName = param.MasterData_Value




                              }).OrderBy(z => z.DebugParameterName).ToList();
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        public IEnumerable<Debug_Templates_DefinitionModel> GetParameters(int templateId)
        {

            var result = new List<Debug_Templates_DefinitionModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = dbContext.usp_GetParameterNotMappedByTemplateID(templateId).Select(d => new Debug_Templates_DefinitionModel()
                    {

                        DebugParameter_Id = d.MasterData_Type_ID,
                        DebugParameterName = d.MasterData_Value
                    }).OrderBy(z => z.DebugParameterName).ToList();

                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        public IEnumerable<DtoList> GetUserTypeByUserLevelId(int UserLevelId)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from ut in dbContext.UserTypes
                                         join ul in dbContext.User_Level on ut.Id equals ul.UserTypeId
                                         where ul.User_Level_ID == UserLevelId
                                         select new DtoList
                                         {
                                             Id = ut.Id,
                                             Text = ut.UserType1,
                                             otherStrVal = ul.Page_Access_Code
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IEnumerable<DtoList> GetFirmware()
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                    // collection.Add(selectListItem);
                    var lstcollection = (from d in dbContext.MasterData_Config_definitions
                                         where d.MasterData_Type == "Software" && d.status == true
                                         select new DtoList
                                         {
                                             Id = d.MasterData_Type_ID,
                                             Text = d.MasterData_Value
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode, int id)
        {
            try
            {
                IEnumerable<UserModels> users = new List<UserModels>();
                int userLevelId = 0;

                Guid acctGuid = new Guid();
                using (var dbContext = new ReviveDBEntities())
                {
                    userLevelId = GetUserLevelId(pageAccessCode);

                    if (pageAccessCode == PageAccessCode.ACCNTMGR && id > 0)
                    {
                        var objCustomerRecord = dbContext.Customers.FirstOrDefault(a => a.Customer_ID == id);
                        acctGuid = (Guid)objCustomerRecord.AccountmanagerUserID;

                        users = dbContext.Users.Where(d => d.User_ID == acctGuid && d.status == true && d.IsLockedOut == false).Select(d => new UserModels()
                        {
                            User_Level_Name = d.UserName,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Email = d.Email_ID
                        }).ToList();

                    }
                    else if ((pageAccessCode == PageAccessCode.MFPC || pageAccessCode == PageAccessCode.MFASSEMBLY || pageAccessCode == PageAccessCode.MFSHIP) && id > 0)
                    {
                        users = dbContext.Users.Where(d => d.Manufacturer_Id == id && d.status == true && d.IsLockedOut == false && d.User_Level_ID == userLevelId).Select(d => new UserModels()
                        {
                            User_Level_Name = d.UserName,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Email = d.Email_ID
                        }).ToList();
                    }
                    else if ((pageAccessCode == PageAccessCode.SUBSIDIARYADMIN) && id > 0)
                    {
                        users = dbContext.Users.Where(d => d.Subsidiary_ID == id && d.status == true && d.IsLockedOut == false && d.User_Level_ID == userLevelId).Select(d => new UserModels()
                        {
                            User_Level_Name = d.UserName,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Email = d.Email_ID
                        }).ToList();
                    }
                    else if ((pageAccessCode == PageAccessCode.SUBAGENTADMIN) && id > 0)
                    {
                        users = dbContext.Users.Where(d => d.SubAgent_ID == id && d.status == true && d.IsLockedOut == false && d.User_Level_ID == userLevelId).Select(d => new UserModels()
                        {
                            User_Level_Name = d.UserName,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Email = d.Email_ID
                        }).ToList();
                    }
                    else
                    {
                        if (userLevelId > 0)
                        {

                            users = dbContext.Users.Where(d => (d.User_Level_ID == userLevelId && d.status == true && d.IsLockedOut == false)).Select(d => new UserModels()
                            {
                                User_Level_Name = d.UserName,
                                FirstName = d.FirstName,
                                LastName = d.LastName,
                                Email = d.Email_ID
                            }).ToList();
                        }
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<UserModels> GetUserByPageAccessCode(string pageAccessCode)
        {
            try
            {
                IEnumerable<UserModels> users = new List<UserModels>();
                int userLevelId = 0;


                using (var dbContext = new ReviveDBEntities())
                {

                    userLevelId = GetUserLevelId(pageAccessCode);
                    if (userLevelId > 0)
                    {


                        users = dbContext.Users.Where(d => (d.User_Level_ID == userLevelId && d.status == true && d.IsLockedOut == false)).Select(d => new UserModels()
                        {
                            User_Level_Name = d.UserName,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Email = d.Email_ID
                        }).ToList();
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int GetUserLevelId(string pageAccessCode)
        {
            int level = 0;
            using (var dbContext = new ReviveDBEntities())
            {
                level = (from User_Level userLevel in dbContext.User_Level
                         where userLevel.Page_Access_Code == pageAccessCode
                         select userLevel.User_Level_ID
                                          ).FirstOrDefault();


            }
            return level;
        }
        #region MyTask Report
        public IEnumerable<NotificationModel> GetNotificationDashboard(string email_Id)
        {
            try
            {
                var collection = new List<NotificationModel>();
                using (var dbContext = new ReviveDBEntities())
                {

                    var lstLocation = dbContext.usp_GetNotification(email_Id).Select(d => new NotificationModel()
                     {

                         NotificationMessages = d.NotificationMessages,
                         Notification_Id = d.Notification_Id,
                         Notification_Date = d.Notification_Date,
                         Mail_Ids = d.SentMail_Ids,
                         body_mail = d.Mail_Body,
                         readFlag = d.MailRead

                     }).ToList();

                    collection.AddRange(lstLocation);

                }



                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<OrderModel> GetHomePagePendingTasks(Guid acctManagerId, int userLevelId, string pageAccessCode, int manufac_ID)
        {
            try
            {
                var myTasklst = new List<OrderModel>();
                string OrderStatusName = string.Empty;
                Guid _acctManagerId = new Guid();

                switch (pageAccessCode)
                {
                    //case PageAccessCode.CLIENTEXEC:
                    //    OrderStatusName = string.Empty;
                    //    break;
                    case PageAccessCode.ACCNTMGR:
                        //OrderStatusName = string.Empty;
                        _acctManagerId = acctManagerId;
                        break;
                    case PageAccessCode.APPROVER:
                        OrderStatusName = OrderStatusType.PendingApproval;
                        break;
                    // Changes done for showing PendingApproval tasks to Admin & SuperAdmin also
                    case PageAccessCode.SUPERADMIN:
                        OrderStatusName = OrderStatusType.PendingApproval;
                        break;
                    case PageAccessCode.ADMIN:
                        OrderStatusName = OrderStatusType.PendingApproval;
                        break;
                    case PageAccessCode.MFPC:
                        OrderStatusName = OrderStatusType.PendingPC;
                        break;
                    case PageAccessCode.MFASSEMBLY:
                        OrderStatusName = OrderStatusType.InProgress;
                        break;
                    case PageAccessCode.MFSHIP:
                        OrderStatusName = OrderStatusType.InProgress; //Pedning
                        break;
                    default:
                        OrderStatusName = string.Empty;
                        break;
                }



                using (var dbContext = new ReviveDBEntities())
                {
                    if (pageAccessCode == PageAccessCode.ACCNTMGR || pageAccessCode == PageAccessCode.CLIENTEXEC)
                    {
                        myTasklst = dbContext.usp_GetMyTaskByUserRole(_acctManagerId, userLevelId, OrderStatusName, manufac_ID).Where(x => x.Status_Name != OrderStatusType.Shipped && x.SubOrderStatus != OrderStatusType.InShipping).Select(spResult => new OrderModel()
                        {

                            CustomerName = spResult.Customer_Name,
                            CustomerId = spResult.Customer_ID,
                            StatusName = spResult.Status_Name,
                            JobOrderHeaderId = spResult.JobOrder_Header_ID,
                            JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(spResult.JobOrder_Header_ID)),
                            NoOfMachines = spResult.Noof_Machines,
                            ManufacturerName = spResult.ManufacturerName,
                            AccountManagerName = spResult.AccountManager


                        }).ToList();
                    }
                    else if (pageAccessCode == PageAccessCode.MFPC || pageAccessCode == PageAccessCode.MFASSEMBLY)
                    {
                        myTasklst = dbContext.usp_GetMyTaskByUserRole(_acctManagerId, userLevelId, OrderStatusName, manufac_ID).Where(x => x.Status_Name != OrderStatusType.Shipped && x.SubOrderStatus != OrderStatusType.InShipping).Select(spResult => new OrderModel()
                        {

                            CustomerName = spResult.Customer_Name,
                            CustomerId = spResult.Customer_ID,
                            StatusName = spResult.Status_Name,
                            JobOrderHeaderId = spResult.JobOrder_Header_ID,
                            JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(spResult.JobOrder_Header_ID)),
                            NoOfMachines = spResult.Noof_Machines,
                            ManufacturerName = spResult.ManufacturerName,
                            AccountManagerName = spResult.AccountManager


                        }).ToList();
                    }
                    else if (pageAccessCode == PageAccessCode.MFSHIP)
                    {
                        myTasklst = dbContext.usp_GetMyTaskByUserRole(_acctManagerId, userLevelId, OrderStatusName, manufac_ID).Where(x => x.SubOrderStatus == OrderStatusType.InShipping).Select(spResult => new OrderModel()
                        {

                            CustomerName = spResult.Customer_Name,
                            CustomerId = spResult.Customer_ID,
                            StatusName = spResult.Status_Name,
                            JobOrderHeaderId = spResult.JobOrder_Header_ID,
                            JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(spResult.JobOrder_Header_ID)),
                            NoOfMachines = spResult.Noof_Machines,
                            ManufacturerName = spResult.ManufacturerName,
                            AccountManagerName = spResult.AccountManager


                        }).ToList();
                    }
                    else
                    {
                        myTasklst = dbContext.usp_GetMyTaskByUserRole(_acctManagerId, userLevelId, OrderStatusName, manufac_ID).Where(x => x.Status_Name != OrderStatusType.Shipped).Select(spResult => new OrderModel()
                        {

                            CustomerName = spResult.Customer_Name,
                            CustomerId = spResult.Customer_ID,
                            StatusName = spResult.Status_Name,
                            JobOrderHeaderId = spResult.JobOrder_Header_ID,
                            JobOrderHeaderIdEncoded = CommonMethods.Encode(Convert.ToString(spResult.JobOrder_Header_ID)),
                            NoOfMachines = spResult.Noof_Machines,
                            ManufacturerName = spResult.ManufacturerName,
                            AccountManagerName = spResult.AccountManager


                        }).ToList();
                    }


                }



                return myTasklst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public NotificationModel GetNotificationDetiailById(int id)
        {
            var result = new NotificationModel();
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {

                    var details = (from Notification rl in dbContext.Notifications.ToList()
                                   where rl.Notification_Id == id
                                   select new NotificationModel
                                   {
                                       NotificationMessages = rl.NotificationMessages,
                                       body_mail = rl.Mail_Body,
                                       readFlag = rl.MailRead,
                                       Notification_Id = rl.Notification_Id
                                   }).FirstOrDefault();

                    result = details;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        #endregion
        public bool CompareDate(DateTime? StartDate, DateTime? EndDate)
        {
            if (StartDate > EndDate)
            {
                return false;
            }
            else if (StartDate == EndDate)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        public bool StoreNotification(NotificationModel mailrecord)
        {

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (mailrecord.Notification_Id == 0)
                    {
                        var record = new Notification();
                        record.Mail_Body = mailrecord.body_mail;
                        record.MailRead = false;
                        record.Created_Date = DateTime.Now;
                        record.SentMail_Ids = mailrecord.Mail_Ids;
                        record.Created_by = mailrecord.Created_by;
                        record.NotificationMessages = mailrecord.NotificationMessages;
                        record.Notification_Date = mailrecord.Notification_Date;
                        dbContext.Notifications.Add(record);
                    }
                    else
                    {
                        var recordupdate = dbContext.Notifications.FirstOrDefault(cond => cond.Notification_Id == mailrecord.Notification_Id);
                        if (recordupdate != null)
                        {
                            recordupdate.MailRead = true;

                        }
                    }
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // API Lookup method Added by KD on dated 07-05-2015
        public IEnumerable<DtoList> GetDevivemanufacturer()
        {
            var result = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from param in dbContext.MasterData_Config_definitions
                              where param.MasterData_Type == "Devivemanufacturer"
                              orderby param.Custom_Field1
                              select new DtoList
                              {
                                  Id = param.MasterData_Type_ID,
                                  Text = param.MasterData_Value
                                  //otherStrVal=param.Custom_Field1
                              }).ToList();
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        public IEnumerable<DtoList> GetHowlongago()
        {
            var result = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from param in dbContext.MasterData_Config_definitions
                              where param.MasterData_Type == "Howlongago"
                              select new DtoList
                              {
                                  Id = param.MasterData_Type_ID,
                                  Text = param.MasterData_Value
                              }).OrderBy(z => z.Id).ToList();
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        public IEnumerable<DtoList> GetPhoneManufacturer()
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.MasterData_Config_definitions
                                         where d.MasterData_Type == "Devivemanufacturer"
                                         select new DtoList
                                         {
                                             Id = d.MasterData_Type_ID,
                                             Text = d.MasterData_Value,
                                             otherStrVal = d.MasterData_Value
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);

                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<DtoList> GetModes(Guid user_id)
        {
            try
            {
                var collection = new List<DtoList>();

                using (var dbContext = new ReviveDBEntities())
                {

                    // Get User type Id by User Id , i.e. Redux,Customer & Manufacturer

                    var userTypeId = (from users in dbContext.Users
                                      join selRole in dbContext.User_Level on users.User_Level_ID equals selRole.User_Level_ID
                                      where users.User_ID == user_id
                                      select selRole.UserTypeId).FirstOrDefault();

                    // end
                    if (userTypeId != null)
                    {
                        var lstcollection = (from d in dbContext.Modes
                                             join c in dbContext.MapMode_UserType on d.ModeId equals c.ModeId
                                             join usrtype in dbContext.UserTypes on c.UserTypeId equals usrtype.Id
                                             where usrtype.Id == userTypeId
                                             orderby d.ModeId

                                             select new DtoList
                                             {
                                                 Id = d.ModeId,
                                                 Text = d.ModeName,
                                                 otherStrVal = d.ModeName
                                             }).OrderBy(z => z.Text).ToList();
                        collection.AddRange(lstcollection);
                    }
                    else
                    {

                        var lstcollection = (from d in dbContext.Modes
                                             join c in dbContext.MapMode_UserType on d.ModeId equals c.ModeId
                                             join usrtype in dbContext.UserTypes on c.UserTypeId equals usrtype.Id
                                             where usrtype.Id == 1
                                             orderby d.ModeId
                                             select new DtoList
                                             {
                                                 Id = d.ModeId,
                                                 Text = d.ModeName,
                                                 otherStrVal = d.ModeName
                                             }).OrderBy(z => z.Text).ToList();
                        collection.AddRange(lstcollection);
                    }
                }



                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string DeleteNotification(int notificationId, string email_Id)
        {
            //var result = false;
            string ReturnVal = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var record = dbContext.Notifications.FirstOrDefault(cond => cond.Notification_Id == notificationId);
                    string[] mailIdSplited = record.SentMail_Ids.Split(';');
                    if (record != null)
                    {
                        if (mailIdSplited.Length > 1)
                        {
                            string NewMailid = "";
                            string separator = "";
                            foreach (string mailId in mailIdSplited)
                            {
                                if (email_Id != mailId && mailId != "")
                                {
                                    NewMailid = NewMailid + separator + mailId;
                                    separator = ";";
                                }

                            }
                            record.SentMail_Ids = NewMailid;

                        }
                        else
                        {

                            dbContext.Notifications.Remove(record);

                        }

                    }

                    ReturnVal = "";
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
        #region Deactivate Customer / Location
        /// <summary>
        /// Deactivate / Activate by Customer Id & Location Id
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        /// 
        public bool DeActivateByCustomerId(int customerId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (customerId != 0)
                    {
                        var recordDeactivate = dbContext.Customers.FirstOrDefault(cond => cond.Customer_ID == customerId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = false;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;

                        }

                        UpdateCustomerAdminStatusByCustomerId(dbContext, customerId, false, modifiedBy);
                        UpdateLocationStatusByCustomerId(dbContext, customerId, false, modifiedBy);
                        UpdateUserStatusByCustomerId(dbContext, customerId, false, modifiedBy);
                        UpdateSubSidiaryStatusByCustomerId(dbContext, customerId, false, modifiedBy);
                        UpdateSubAgentStatusByCustomerId(dbContext, customerId, false, modifiedBy);

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
        public bool ActivateByCustomerId(int customerId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (customerId != 0)
                    {
                        var recordDeactivate = dbContext.Customers.FirstOrDefault(cond => cond.Customer_ID == customerId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = true;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;
                        }

                        UpdateCustomerAdminStatusByCustomerId(dbContext, customerId, true, modifiedBy);
                        UpdateLocationStatusByCustomerId(dbContext, customerId, true, modifiedBy);
                        UpdateUserStatusByCustomerId(dbContext, customerId, true, modifiedBy);
                        UpdateSubSidiaryStatusByCustomerId(dbContext, customerId, true, modifiedBy);
                        UpdateSubAgentStatusByCustomerId(dbContext, customerId, true, modifiedBy);
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
        public bool DeActivateByLocationId(int locationId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (locationId != 0)
                    {
                        var recordDeactivate = dbContext.Customers_Locations.FirstOrDefault(cond => cond.Location_ID == locationId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = false;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;
                        }

                        UpdateUserStatusByLocationId(dbContext, locationId, false, modifiedBy);

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



        public bool ActivateByLocationId(int locationId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (locationId != 0)
                    {
                        var recordDeactivate = dbContext.Customers_Locations.FirstOrDefault(cond => cond.Location_ID == locationId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = true;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;
                        }

                        UpdateUserStatusByLocationId(dbContext, locationId, true, modifiedBy);

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

        /// <summary>
        /// Check Customer deactivate status when location is activating...
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string checkValidLocationToActivate(int locationId)
        {
            string result = "";
            int subdirectoryId = 0;
            int subagentId = 0;
            int customerId = 0;
            var validUser = true;

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var locationDetails = dbContext.Customers_Locations.FirstOrDefault(loc => loc.Location_ID == locationId);
                    if (locationDetails != null)
                    {
                        customerId = locationDetails.Customer_ID != null ? (int)locationDetails.Customer_ID : 0;
                        subdirectoryId = locationDetails.Subsidiary_ID != null ? (int)locationDetails.Subsidiary_ID : 0;
                        subagentId = locationDetails.SubAgent_ID != null ? (int)locationDetails.SubAgent_ID : 0;
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


            //try
            //{
            //    using (var dbContext = new ReviveDBEntities())
            //    {
            //        var customersdetail = (from cust in dbContext.Customers
            //                               join loc in dbContext.Customers_Locations on cust.Customer_ID equals loc.Customer_ID
            //                               where loc.Location_ID == locationId
            //                               select new ManageCustomersModel
            //                               {
            //                                   Customer_ID = cust.Customer_ID,
            //                                   Status = cust.Status
            //                               }).FirstOrDefault();


            //        if (customersdetail != null)
            //        {
            //            if (customersdetail.Status == false)
            //            {
            //                result = "Customer:" + customersdetail.Customer_Name + " has been deactivated.You can not activate this location. ";
            //            }
            //        }

            //    }
            //    return result;

            //}
            //catch
            //{
            //    throw;
            //}
        }





        #region Deactivate  or Activate Sub agents



        #endregion
        private void UpdateCustomerAdminStatusByCustomerId(ReviveDBEntities dbContext, int customerId, bool status, Guid modifiedBy)
        {

            var CustomerAdminusers = (from d in dbContext.Users
                                      join usrlevel in dbContext.User_Level on d.User_Level_ID equals usrlevel.User_Level_ID
                                      where usrlevel.Page_Access_Code == PageAccessCode.CUSTOMERADMIN
                                      && d.Customer_ID == customerId
                                      select d).ToList();
            foreach (User objuser in CustomerAdminusers)
            {
                objuser.status = status;
                objuser.Modified_by = modifiedBy;
                objuser.Modified_Date = System.DateTime.Now;

            }


        }
        private void UpdateLocationStatusByCustomerId(ReviveDBEntities dbContext, int customerId, bool status, Guid modifiedBy)
        {
            var locationList = dbContext.Customers_Locations.Where(cond => cond.Customer_ID == customerId).ToList();

            foreach (Customers_Locations objCustLoc in locationList)
            {
                objCustLoc.Status = status;
                objCustLoc.Modified_by = modifiedBy;
                objCustLoc.Modified_Date = System.DateTime.Now;
            }
        }
        private void UpdateUserStatusByCustomerId(ReviveDBEntities dbContext, int customerId, bool status, Guid modifiedBy)
        {
            var storeUserLst = (from d in dbContext.Users
                                join usrlevel in dbContext.User_Level on d.User_Level_ID equals usrlevel.User_Level_ID
                                //where usrlevel.Page_Access_Code == PageAccessCode.STOREUSER
                                where d.Customer_ID == customerId
                                select d);

            foreach (User objuser in storeUserLst)
            {
                objuser.status = status;
                objuser.Modified_by = modifiedBy;
                objuser.Modified_Date = System.DateTime.Now;
            }
        }
        private void UpdateUserStatusByLocationId(ReviveDBEntities dbContext, int locationId, bool status, Guid modifiedBy)
        {
            var storeUserLst = (from d in dbContext.Users
                                join usrlevel in dbContext.User_Level on d.User_Level_ID equals usrlevel.User_Level_ID
                                where usrlevel.Page_Access_Code == PageAccessCode.STOREUSER
                                && d.Location_ID == locationId
                                select d);
            foreach (User objuser in storeUserLst)
            {
                objuser.status = status;
                objuser.Modified_by = modifiedBy;
                objuser.Modified_Date = System.DateTime.Now;
            }
        }

        private void UpdateSubSidiaryStatusByCustomerId(ReviveDBEntities dbContext, int customerId, bool status, Guid modifiedBy)
        {
            var SubsidiaryLst = (from d in dbContext.Subsidiaries
                                 join cust in dbContext.Customers on d.Customer_ID equals cust.Customer_ID
                                 where cust.Customer_ID == customerId
                                 select d);
            foreach (Subsidiary objSubsidiary in SubsidiaryLst)
            {
                objSubsidiary.Status = status;
                objSubsidiary.Modified_by = modifiedBy;
                objSubsidiary.Modified_Date = System.DateTime.Now;
            }
        }

        private void UpdateSubAgentStatusByCustomerId(ReviveDBEntities dbContext, int customerId, bool status, Guid modifiedBy)
        {
            var SubAgentLst = (from d in dbContext.SubAgents
                               join cust in dbContext.Customers on d.Customer_ID equals cust.Customer_ID
                               where cust.Customer_ID == customerId
                               select d);
            foreach (SubAgent objSubagent in SubAgentLst)
            {
                objSubagent.Status = status;
                objSubagent.Modified_by = modifiedBy;
                objSubagent.Modified_Date = System.DateTime.Now;
            }
        }





        #endregion
        public IEnumerable<DtoList> GetManufacturers()
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Manufacturers.Where(c => c.Status == true)
                                         select new DtoList
                                         {
                                             Id = d.Manufacturer_Id,
                                             Text = d.Manufacturer_Name
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Mode Configuration
        public List<ModeConfigurationModel> GetAllConfigModes(ModeConfigurationModel objModeConfigDetails)
        {
            var allModeConfig = new List<ModeConfigurationModel>();

            using (var dbContext = new ReviveDBEntities())
            {
                //allModeConfig = (from mo in dbContext.Modes_configuration
                //                 where mo.Status!=false
                //                 select new ModeConfigurationModel
                //{
                //    Config_Id = mo.Config_Id,
                //    customerID = mo.Customer_Id,
                //    locationID = mo.Location_id,
                //    From_Date = mo.From_Date,
                //    To_Date = mo.To_Date,
                //    No_of_Activities = mo.No_of_Activities,
                //    //No_of_Consumed=dbContext.Tablet_App_Machine_Activity_Details.Where(a=>a.ModeId==mo.ModeId && a.Machine_Id==(dbContext..Where(a => a.Customer_ID == mo.Customer_Id).Select(a => a.Customer_Name).FirstOrDefault()))
                //    CustomerName = dbContext.Customers.Where(a => a.Customer_ID == mo.Customer_Id).Select(a => a.Customer_Name).FirstOrDefault(),
                //    locationName = dbContext.Customers_Locations.Where(a => a.Location_ID == mo.Location_id).Select(a => a.Location_Name).FirstOrDefault()
                //}).ToList();
                //allModeConfig = allModeConfig.OrderByDescending(m => m.Config_Id).ToList();
                allModeConfig = dbContext.usp_GetAllModesByModeId(Modes.Demo, objModeConfigDetails.customerID, objModeConfigDetails.SubsidiaryId, objModeConfigDetails.SubAgentId, objModeConfigDetails.locationID).Select(mo => new ModeConfigurationModel()
                {
                    Config_Id = (int)mo.Config_Id,
                    customerID = mo.Customer_Id,
                    locationID = mo.Location_id,
                    From_Date = mo.From_Date,
                    To_Date = mo.To_Date,
                    No_of_Activities = mo.No_of_Activities,
                    CustomerName = mo.CustomerName,
                    locationName = mo.locationName,
                    No_of_Consumed = (int)mo.No_of_Consumed,
                    SubAgentId = (int)mo.SubAgent_ID,
                    SubsidiaryId = (int)mo.Subsidiary_ID,
                    SubAgentName = mo.SubAgentName,
                    StoreNumber = mo.StoreNumber
                }).ToList();
                allModeConfig = allModeConfig.OrderByDescending(m => m.Config_Id).ToList();





            }
            return allModeConfig;
        }
        public string AddNewModeConfig(ModeConfigurationModel newModeConfig)
        {
            string ReturnVal = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (newModeConfig != null)
                    {
                        var obj = new Modes_configuration
                        {
                            ModeId = newModeConfig.ModeId == 0 ? 4 : newModeConfig.ModeId,
                            Customer_Id = newModeConfig.customerID,
                            Location_id = newModeConfig.locationID,
                            No_of_Activities = newModeConfig.No_of_Activities,
                            From_Date = newModeConfig.From_Date,
                            To_Date = newModeConfig.To_Date,
                            Created_by = newModeConfig.Created_by,
                            Created_Date = newModeConfig.Created_Date,
                            Subsidiary_ID = newModeConfig.SubsidiaryId == null ? 0 : (int)newModeConfig.SubsidiaryId,
                            SubAgent_ID = newModeConfig.SubAgentId == null ? 0 : (int)newModeConfig.SubAgentId
                        };
                        dbContext.Modes_configuration.Add(obj);
                        dbContext.SaveChanges();
                        ReturnVal = "Added successfully";
                    }
                }
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ModeConfigurationModel GetModeConfigById(int id)
        {

            try
            {
                var modeConfig = new ModeConfigurationModel();
                using (var dbContext = new ReviveDBEntities())
                {
                    var result = (from mode in dbContext.Modes_configuration
                                  where mode.Config_Id == id
                                  select new ModeConfigurationModel
                                  {
                                      customerID = mode.Customer_Id,
                                      locationID = mode.Location_id,
                                      No_of_Activities = mode.No_of_Activities,
                                      From_Date = mode.From_Date,
                                      To_Date = mode.To_Date,
                                      Config_Id = mode.Config_Id,
                                      SubsidiaryId = (int)mode.Subsidiary_ID,
                                      SubAgentId = (int)mode.SubAgent_ID
                                  }).FirstOrDefault();
                    modeConfig = result;
                }
                return modeConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateModeConfig(ModeConfigurationModel existingModeConfig)
        {
            var flag = false;
            try
            {
                if (existingModeConfig != null)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var modeConf = (from m in dbContext.Modes_configuration where m.Config_Id == existingModeConfig.Config_Id select m).FirstOrDefault();
                        if (modeConf != null)
                        {
                            modeConf.Customer_Id = existingModeConfig.customerID;
                            modeConf.Location_id = existingModeConfig.locationID;
                            modeConf.No_of_Activities = existingModeConfig.No_of_Activities;
                            modeConf.From_Date = existingModeConfig.From_Date;
                            modeConf.To_Date = existingModeConfig.To_Date;
                            modeConf.Modified_by = existingModeConfig.Modified_by;
                            modeConf.Modified_Date = DateTime.Now;
                        }
                        dbContext.SaveChanges();
                        flag = true;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ValidateData(int loc, DateTime from, DateTime to, int modeId)
        {
            var flag = false;
            var fromDate = from;
            var toDate = to;
            var exists = new List<Modes_configuration>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (modeId != 0)
                    {
                        exists = (from m in dbContext.Modes_configuration
                                  where m.Location_id == loc && m.Config_Id != modeId &&
                                  ((EntityFunctions.TruncateTime(m.From_Date) <= EntityFunctions.TruncateTime(fromDate) && EntityFunctions.TruncateTime(m.To_Date) >= EntityFunctions.TruncateTime(fromDate)) ||
                                  EntityFunctions.TruncateTime(m.From_Date) <= EntityFunctions.TruncateTime(toDate) && EntityFunctions.TruncateTime(m.To_Date) >= EntityFunctions.TruncateTime(toDate))
                                  select m).ToList();
                    }
                    else
                    {
                        exists = (from m in dbContext.Modes_configuration
                                  where m.Location_id == loc &&
                                  ((EntityFunctions.TruncateTime(m.From_Date) <= EntityFunctions.TruncateTime(fromDate) && EntityFunctions.TruncateTime(m.To_Date) >= EntityFunctions.TruncateTime(fromDate)) ||
                                  EntityFunctions.TruncateTime(m.From_Date) <= EntityFunctions.TruncateTime(toDate) && EntityFunctions.TruncateTime(m.To_Date) >= EntityFunctions.TruncateTime(toDate))
                                  select m).ToList();
                    }
                    if (exists.Any())
                    {
                        flag = true;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteModeConfig(int modeID)
        {
            try
            {
                if (modeID != null && modeID != 0)
                {
                    using (var dbContext = new ReviveDBEntities())
                    {
                        var deleteModeConfiguration = (from m in dbContext.Modes_configuration where m.Config_Id == modeID select m).FirstOrDefault();
                        if (deleteModeConfiguration != null)
                        {
                            dbContext.Modes_configuration.Remove(deleteModeConfiguration);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add Machine to JobOrder_Machine_Id Table

        public string AddMachine(WebApiMachineModel objMachine)
        {
            string result = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (objMachine != null)
                    {
                        // Check Duplicate Machine Id 
                        var machineExist = dbContext.Machines.FirstOrDefault(c => c.Machine_Id_Val == objMachine.MachineId);
                        if (machineExist != null)
                        {
                            result = "Machine Id" + objMachine.MachineId.ToString() + " Already exist.";
                        }
                        else // New Machine added
                        {
                            var machine = new Machine();
                            machine.Machine_Id_Val = objMachine.MachineId;
                            machine.IsAssigned = false;
                            machine.Created_Date = System.DateTime.Now;
                            machine.StatusId = dbContext.MasterData_Config_definitions.FirstOrDefault(c => (c.MasterData_Type == "MachineStatus" && c.MasterData_Value == "Inventory")).MasterData_Type_ID;
                            machine.IsTested = true;
                            machine.IsShipped = false;
                            dbContext.Machines.Add(machine);
                            result = "";
                            dbContext.SaveChanges();
                            #region Saving data into Machine Histrory table
                            int _machineId = dbContext.Machines.Where(x => x.Machine_Id_Val == objMachine.MachineId).Select(x => x.Machine_Id).FirstOrDefault();
                            MachineHistory MachineNewHistory = new MachineHistory();
                            MachineNewHistory.Machine_ID = _machineId;
                            MachineNewHistory.ReasonTypeId = 0;
                            MachineNewHistory.StatusTypeId = 0;
                            MachineNewHistory.Notes = "Added to ES";
                            MachineNewHistory.EventDate = DateTime.Now;
                            MachineNewHistory.TransactoionTypeId = 0;
                            dbContext.MachineHistories.Add(MachineNewHistory);
                            dbContext.SaveChanges();
                            #endregion
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                throw;

            }
            return result;

        }


        #endregion


        #region Testing Machine In

        public string MachineTestDataExist(int machineId, string Machine_Id_val)
        {
            string result = "";
            using (var dbContext = new ReviveDBEntities())
            {
                var TestData = dbContext.Machine_TestResults.Where(c => c.Machine_Id == machineId).FirstOrDefault();

                if (TestData != null)
                {
                    result = "Test data already exists with machine Id :" + Machine_Id_val;

                }

            }
            return result;

        }
        public string MachineTestResult(MachineTestingModel objMachine)
        {
            string result = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    Machine_TestResults objmodel = new Machine_TestResults();

                    objmodel.Vaccum_time = objMachine.Vaccum_time;
                    objmodel.Vaccum_sensor = objMachine.Vaccum_sensor;
                    objmodel.Vaccum_seconds = objMachine.Vaccum_seconds;
                    objmodel.Vaccum_delta = objMachine.Vaccum_delta;
                    objmodel.USB_Internal_sensor = objMachine.USB_Internal_sensor;
                    objmodel.USB_Internal_delta = objMachine.USB_Internal_delta;
                    objmodel.USB_External_sensor = objMachine.USB_External_sensor;
                    objmodel.USB_External_delta = objMachine.USB_External_delta;
                    objmodel.Test3_Verified = objMachine.Test3_Verified;
                    objmodel.Test3_Name = objMachine.Test3_Name;
                    objmodel.Test2_Verified = objMachine.Test2_Verified;
                    objmodel.Test2_Name = objMachine.Test2_Name;
                    objmodel.Test1_Verified = objMachine.Test1_Verified;
                    objmodel.Test1_Name = objMachine.Test1_Name;
                    objmodel.Relative_Sensor = objMachine.Relative_Sensor;
                    objmodel.Relative_Delta = objMachine.Relative_Delta;
                    objmodel.Platen_time = objMachine.Platen_time;
                    objmodel.Platen_sensor = objMachine.Platen_sensor;
                    objmodel.Platen_seconds = objMachine.Platen_seconds;
                    objmodel.Platen_delta = objMachine.Platen_delta;
                    objmodel.Modified_Date = objMachine.Modified_Date;
                    objmodel.Modified_by = objMachine.Modified_by;
                    //objmodel.Machine_Testing_id = objMachine.Machine_Testing_id;
                    objmodel.Machine_Id = objMachine.Machine_Id;
                    objmodel.Injection_time = objMachine.Injection_time;
                    objmodel.Injection_sensor = objMachine.Injection_sensor;
                    objmodel.Injection_seconds = objMachine.Injection_seconds;
                    objmodel.Injection_delta = objMachine.Injection_delta;
                    objmodel.Ground_Test = objMachine.Ground_Test;
                    objmodel.Ground_Resistance = objMachine.Ground_Resistance;
                    objmodel.Final_Process_Id = objMachine.Final_Process_Id;
                    objmodel.Final_Assembly = objMachine.Final_Assembly;
                    objmodel.Drying_Process = objMachine.Drying_Process;
                    objmodel.Drying_Cycles = objMachine.Drying_Cycles;
                    objmodel.Dielectric_Voltage = objMachine.Dielectric_Voltage;
                    objmodel.Dielectric_Time = objMachine.Dielectric_Time;
                    objmodel.Dielectric_test = objMachine.Dielectric_test;
                    objmodel.Dielectric_Current = objMachine.Dielectric_Current;
                    objmodel.Modified_Date = DateTime.Now;
                    objmodel.Created_by = objMachine.Created_by;


                    dbContext.Machine_TestResults.Add(objmodel);

                    var machineRecord = dbContext.Machines.Where(c => c.Machine_Id == objmodel.Machine_Id).FirstOrDefault();
                    machineRecord.IsTested = true;

                    MachineHistory historyrecord = new MachineHistory();
                    historyrecord.Machine_ID = objmodel.Machine_Id;
                    historyrecord.Notes = "Machine Tested Successfully";
                    historyrecord.ReasonTypeId = 0;
                    historyrecord.EventDate = System.DateTime.Now;

                    dbContext.MachineHistories.Add(historyrecord);

                    dbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                throw;

            }
            return result;

        }


        #endregion

        public string DeleteLocationByLocId(int locationId)
        {
            string retMsg = string.Empty;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    int userCount = dbContext.Users.Where(c => c.Location_ID == locationId).ToList().Count;

                    // var machineCount = dbContext.JobOrder_Locations.Where(c => c.Location_id == locationId).ToList().Count;

                    var machineCount = (from MachineLoc in dbContext.LocationMapMachines
                                        join machineMap in dbContext.Machines on MachineLoc.Machine_Id equals machineMap.Machine_Id
                                        where MachineLoc.Location_id == locationId
                                        && machineMap.Machine_Id != null
                                        select new LocationModel
                                        {
                                            LocationId = (int)MachineLoc.Location_id

                                        }).ToList().Count;

                    var _groupStatus = GetGroupStatusByLocationId(locationId);



                    if (userCount > 0 && machineCount > 0)
                    {
                        retMsg = "You can not delete this location. \nStore User(s) and Machine(s) have been assigned to this Location!";
                    }
                    else if (userCount > 0)
                    {
                        retMsg = "You can not delete this location. \nStore User(s) have been assigned to this Location!";
                    }
                    else if (machineCount > 0)
                    {
                        retMsg = "You can not delete this location. \nMachine(s) have been assigned to this Location!";
                    }
                    else if (!string.IsNullOrEmpty(_groupStatus))
                    {
                        retMsg = "You can not delete this location. \nGroup has been Mapped to this Location!";
                    }

                    else
                    {
                        // Delete
                        var recToDel = dbContext.Customers_Locations.FirstOrDefault(c => c.Location_ID == locationId);
                        if (recToDel != null)
                        {
                            dbContext.Customers_Locations.Remove(recToDel);
                            dbContext.SaveChanges();
                            retMsg = "Success";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retMsg = "Fail";
                throw;
            }
            return retMsg;
        }
        // API Lookup method Added by KD on dated 07-05-2015
        /// <summary>
        /// Location dropdown for Unused machine 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DtoList> GetLocations()
        {
            var result = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from loc in dbContext.Customers_Locations
                              where loc.Status == true


                              select new DtoList
                              {
                                  Id = loc.Location_ID,
                                  Text = loc.Location_Name
                              }).OrderBy(z => z.Text).ToList();
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        /// <summary>
        /// GetPageAccesscode by UserLevelId
        /// </summary>
        /// <param name="UserLevelId"></param>
        /// <returns></returns>
        public string GetUserByPageAccessCodeByUserLevelId(int UserLevelId)
        {
            string PageAccessCode = "";
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {
                    var objUserRoleDetails = dbContext.User_Level.FirstOrDefault(para => para.User_Level_ID == UserLevelId);
                    if (objUserRoleDetails != null)
                    {
                        PageAccessCode = objUserRoleDetails.Page_Access_Code;
                    }


                }

            }
            catch (Exception ex)
            {
                throw;

            }
            return PageAccessCode;
        }

        #region Subsidiary Changes
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public IEnumerable<DtoList> GetSubsidiaryByCustomerId(int CustomerId)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from subsidiary in dbContext.Subsidiaries
                                         where subsidiary.Customer_ID == CustomerId && subsidiary.Status == true
                                         select new DtoList
                                         {
                                             Id = subsidiary.Subsidiary_ID,
                                             Text = subsidiary.SubsidiaryName,
                                             otherIntVal = dbContext.Customers.Where(x => x.Customer_ID == CustomerId).Select(x=>x.TemplateId).FirstOrDefault()
                                         }).OrderBy(z => z.Text).ToList();
                    if (lstcollection.Count > 0)
                    {
                        collection.AddRange(lstcollection);
                    }
                    else
                    {
                        collection = new List<DtoList>();
                    }

                }
                return collection;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        //raise a new exception inserting the current one as the InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public IEnumerable<DtoList> GetSubsidiaryByCustomerId(int CustomerId, int SubsidiaryId)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from subsidiary in dbContext.Subsidiaries
                                         where subsidiary.Customer_ID == CustomerId && subsidiary.Subsidiary_ID == SubsidiaryId && subsidiary.Status == true
                                         select new DtoList
                                         {
                                             Id = subsidiary.Subsidiary_ID,
                                             Text = subsidiary.SubsidiaryName
                                         }).OrderBy(z => z.Text).ToList();
                    if (lstcollection.Count > 0)
                    {
                        collection.AddRange(lstcollection);
                    }
                    else
                    {
                        collection = new List<DtoList>();
                    }

                }
                return collection;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        //raise a new exception inserting the current one as the InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }
        public IEnumerable<DtoList> GetAgentsBySubsidiaryId(int SubsidiaryId)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from subagent in dbContext.SubAgents
                                         where subagent.Subsidiary_ID == SubsidiaryId
                                         && subagent.Status == true
                                         select new DtoList
                                         {
                                             Id = subagent.SubAgent_ID,
                                             Text = subagent.SubAgentName
                                         }).OrderBy(z => z.Text).ToList();
                    if (lstcollection.Count > 0)
                    {
                        collection.AddRange(lstcollection);
                    }
                    else
                    {
                        collection = new List<DtoList>();
                    }


                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public IEnumerable<DtoList> GetAgentsBySubsidiaryId(int SubsidiaryId, int SubAgentId)
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from subagent in dbContext.SubAgents
                                         where subagent.Subsidiary_ID == SubsidiaryId && subagent.SubAgent_ID == SubAgentId
                                         && subagent.Status == true
                                         select new DtoList
                                         {
                                             Id = subagent.SubAgent_ID,
                                             Text = subagent.SubAgentName
                                         }).OrderBy(z => z.Text).ToList();
                    if (lstcollection.Count > 0)
                    {
                        collection.AddRange(lstcollection);
                    }
                    else
                    {
                        collection = new List<DtoList>();
                    }


                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion


        public IEnumerable<DtoList> GetMasterValuesByType(string configValue)
        {
            var result = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from param in dbContext.MasterData_Config_definitions
                              where param.MasterData_Type == configValue
                              select new DtoList
                              {
                                  Id = param.MasterData_Type_ID,
                                  Text = param.MasterData_Value,
                                  otherStrVal=param.Custom_Field1
                              }).OrderBy(z => z.Text).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public int IncValue()
        {
            int resultVal = 0;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var getProcCall = dbContext.usp_GetMachineIdCounterVal(System.DateTime.Now, ConstantEntities.MachineIncValue).FirstOrDefault();
                    resultVal = (int)getProcCall.MachineIdCounterVal;
                }
            }
            catch (Exception ex)
            {
                resultVal = 0;
                throw;
            }
            return resultVal;

        }
        public int GetValue()
        {
            int resultVal = 0;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var getProcCall = dbContext.usp_GetMachineIdCounterVal(System.DateTime.Now, ConstantEntities.MachineGetValue).FirstOrDefault();
                    resultVal = (int)getProcCall.MachineIdCounterVal;
                }
            }
            catch (Exception ex)
            {
                resultVal = 0;
                throw;
            }
            return resultVal;

        }
        public int ResetValue()
        {
            int resultVal = 0;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var getProcCall = dbContext.usp_GetMachineIdCounterVal(System.DateTime.Now, ConstantEntities.MachineResetValue).FirstOrDefault();
                    resultVal = (int)getProcCall.MachineIdCounterVal;
                }
            }
            catch (Exception ex)
            {
                resultVal = 0;
                throw;
            }
            return resultVal;

        }

        public string checkValidSubdirectoryToActivate(int subdirectoryId)
        {
            string result = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var customersdetail = (from cust in dbContext.Customers
                                           join loc in dbContext.Subsidiaries on cust.Customer_ID equals loc.Customer_ID
                                           where loc.Subsidiary_ID == subdirectoryId
                                           select new ManageCustomersModel
                                           {
                                               Customer_ID = cust.Customer_ID,
                                               Status = cust.Status
                                           }).FirstOrDefault();

                    if (customersdetail != null)
                    {
                        if (customersdetail.Status == false)
                        {
                            result = "Customer:" + customersdetail.Customer_Name + " has been deactivated.You can not activate this Subdirectory. ";
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

        public string checkValidSubAgentToActivate(int subagentId)
        {
            string result = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var customersdetail = (from cust in dbContext.Subsidiaries
                                           join loc in dbContext.SubAgents on cust.Subsidiary_ID equals loc.Subsidiary_ID
                                           where loc.SubAgent_ID == subagentId
                                           select new ManageSubsidiaryModel
                                           {
                                               Subsidiary_Name = cust.SubsidiaryName,
                                               Status = cust.Status
                                           }).FirstOrDefault();

                    if (customersdetail != null)
                    {
                        if (customersdetail.Status == false)
                        {
                            result = "Subdirectory:" + customersdetail.Subsidiary_Name + " has been deactivated.You can not activate this Sub Agent. ";
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

        public bool DeActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (subdirectoryId != 0)
                    {
                        var recordDeactivate = dbContext.Subsidiaries.FirstOrDefault(cond => cond.Subsidiary_ID == subdirectoryId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = false;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;
                        }

                        UpdateLocationStatusBySubsidiaryId(dbContext, subdirectoryId, recordDeactivate.Status, modifiedBy);
                        DeactivateAgentBySubsidiaryId(dbContext, subdirectoryId, recordDeactivate.Status, modifiedBy);
                        UpdateSubsidiaryAdminStatusBySubsidiaryId(dbContext, subdirectoryId, recordDeactivate.Status, modifiedBy);

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

        public bool ActivateBySubdirectoryId(int subdirectoryId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (subdirectoryId != 0)
                    {
                        var recordDeactivate = dbContext.Subsidiaries.FirstOrDefault(cond => cond.Subsidiary_ID == subdirectoryId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = true;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;
                        }

                        UpdateLocationStatusBySubsidiaryId(dbContext, subdirectoryId, true, modifiedBy);
                        DeactivateAgentBySubsidiaryId(dbContext, subdirectoryId, true, modifiedBy);
                        UpdateSubsidiaryAdminStatusBySubsidiaryId(dbContext, subdirectoryId, true, modifiedBy);

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

        public bool DeActivateBySubAgentId(int subagentId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (subagentId != 0)
                    {
                        var recordDeactivate = dbContext.SubAgents.FirstOrDefault(cond => cond.SubAgent_ID == subagentId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = false;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;
                        }

                        UpdateLocationStatusBySubAgentId(dbContext, subagentId, recordDeactivate.Status, modifiedBy);
                        UpdateSubAgentAdminStatusBySubAgentId(dbContext, subagentId, recordDeactivate.Status, modifiedBy);

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


        private void UpdateLocationStatusBySubAgentId(ReviveDBEntities dbContext, int subAgentId, bool status, Guid modifiedBy)
        {
            var LocationList = dbContext.Customers_Locations.Where(cond => cond.SubAgent_ID == subAgentId).ToList();

            foreach (Customers_Locations objLocation in LocationList)
            {
                objLocation.Status = status;
                objLocation.Modified_by = modifiedBy;
                objLocation.Modified_Date = System.DateTime.Now;
            }
        }

        private void UpdateSubAgentAdminStatusBySubAgentId(ReviveDBEntities dbContext, int subAgentId, bool status, Guid modifiedBy)
        {
            var UserAdminList = dbContext.Users.Where(cond => cond.SubAgent_ID == subAgentId).ToList();

            foreach (User objUserAdmin in UserAdminList)
            {
                objUserAdmin.status = status;
                objUserAdmin.Modified_by = modifiedBy;
                objUserAdmin.Modified_Date = System.DateTime.Now;
            }
        }
        public bool ActivateBySubAgentId(int subagentId, Guid modifiedBy)
        {
            var result = false;
            List<DtoUser> objUserDetails = new List<DtoUser>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (subagentId != 0)
                    {
                        var recordDeactivate = dbContext.SubAgents.FirstOrDefault(cond => cond.SubAgent_ID == subagentId);
                        if (recordDeactivate != null)
                        {
                            recordDeactivate.Status = true;
                            recordDeactivate.Modified_by = modifiedBy;
                            recordDeactivate.Modified_Date = System.DateTime.Now;
                        }

                        UpdateLocationStatusBySubAgentId(dbContext, subagentId, recordDeactivate.Status, modifiedBy);
                        UpdateSubAgentAdminStatusBySubAgentId(dbContext, subagentId, recordDeactivate.Status, modifiedBy);

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

        public IEnumerable<DtoList> GetLoggingTypes()
        {
            try
            {
                var collection = new List<DtoList>();
                using (var dbContext = new ReviveDBEntities())
                {
                    // var selectListItem = new DtoList { Text = "--Select--", Id = 0 };
                    // collection.Add(selectListItem);
                    var lstcollection = (from d in dbContext.MasterData_Config_definitions
                                         where d.MasterData_Type == "TableAppLoggingType" && d.status == true
                                         select new DtoList
                                         {
                                             Id = d.MasterData_Type_ID,
                                             Text = d.MasterData_Value
                                         }).OrderBy(z => z.Text).ToList();
                    collection.AddRange(lstcollection);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private void UpdateLocationStatusBySubsidiaryId(ReviveDBEntities dbContext, int SubsidiaryId, bool status, Guid modifiedBy)
        {
            var LocationList = dbContext.Customers_Locations.Where(cond => cond.Subsidiary_ID == SubsidiaryId).ToList();

            foreach (Customers_Locations objLocation in LocationList)
            {
                objLocation.Status = status;
                objLocation.Modified_by = modifiedBy;
                objLocation.Modified_Date = System.DateTime.Now;
            }
        }

        private void DeactivateAgentBySubsidiaryId(ReviveDBEntities dbContext, int SubsidiaryId, bool status, Guid modifiedBy)
        {
            var AgentList = dbContext.SubAgents.Where(cond => cond.Subsidiary_ID == SubsidiaryId).ToList();
            foreach (SubAgent objAgent in AgentList)
            {
                objAgent.Status = status;
                objAgent.Modified_by = modifiedBy;
                objAgent.Modified_Date = System.DateTime.Now;
            }
        }

        private void UpdateSubsidiaryAdminStatusBySubsidiaryId(ReviveDBEntities dbContext, int SubsidiaryId, bool status, Guid modifiedBy)
        {
            var UserAdminList = dbContext.Users.Where(cond => cond.Subsidiary_ID == SubsidiaryId).ToList();

            foreach (User objUserAdmin in UserAdminList)
            {
                objUserAdmin.status = status;
                objUserAdmin.Modified_by = modifiedBy;
                objUserAdmin.Modified_Date = System.DateTime.Now;
            }
        }
        public string AddMultipleModeConfig(ModeConfigurationModel newModeConfig, IEnumerable<ModeConfigurationModel> data)
        {
            string ReturnVal = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (newModeConfig != null && data != null)
                    {
                        var ModetoUpdate = data.Where(cond => cond.isSelected == true).ToList();
                        foreach (var DemosToAdd in ModetoUpdate)
                        {
                            if (DemosToAdd.Config_Id != null && DemosToAdd.Config_Id != 0)
                            {
                                var deleteModeConfiguration = (from m in dbContext.Modes_configuration where m.Config_Id == DemosToAdd.Config_Id select m).FirstOrDefault();
                                if (deleteModeConfiguration != null)
                                {
                                    dbContext.Modes_configuration.Remove(deleteModeConfiguration);
                                    //dbContext.SaveChanges();
                                }
                            }

                            var obj = new Modes_configuration
                            {
                                ModeId = DemosToAdd.ModeId == 0 ? 4 : DemosToAdd.ModeId,
                                Customer_Id = DemosToAdd.customerID,
                                Location_id = DemosToAdd.locationID,
                                No_of_Activities =newModeConfig.No_of_Activities,//, Commented on dated 7th April 2017 as per cliemt mail 4th April 2017
                                From_Date = newModeConfig.From_Date,
                                To_Date = newModeConfig.To_Date,
                                Created_by = DemosToAdd.Created_by,
                                Created_Date = DemosToAdd.Created_Date,
                                Subsidiary_ID = DemosToAdd.SubsidiaryId == null ? 0 : (int)DemosToAdd.SubsidiaryId,
                                SubAgent_ID = DemosToAdd.SubAgentId == null ? 0 : (int)DemosToAdd.SubAgentId
                            };
                            dbContext.Modes_configuration.Add(obj);
                        }
                        dbContext.SaveChanges();
                        ReturnVal = "Added successfully";
                    }
                }
                return ReturnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetGroupStatusByLocationId(int _Location_Id)
        {
            ManageShippingRepository ManageShippingRepositoryObj = new ManageShippingRepository();
            string groupStatus = "";
            using (var dbContext = new ReviveDBEntities())
            {
                var GroupId = dbContext.Customers_Locations.Where(x => x.Location_ID == _Location_Id).Select(x => x.GroupId).FirstOrDefault();
                if (GroupId != null)
                {
                    var GroupStatus = ManageShippingRepositoryObj.GetShippingQueueDetailsByGroupId((int)GroupId);
                    groupStatus = GroupStatus.Status;
                }
            }


            return groupStatus;

        }

        public string GroupStatusByLocationId(int _LocationId)
        {
            string retMsg = string.Empty;
            var _groupStatus = GetGroupStatusByLocationId(_LocationId);
            if (_groupStatus != null)
            {
                retMsg = "You can not deactivate this location. \nGroup has been Mapped to this Location!";
            }
            return retMsg;
        }

        #region Group implementation for location

        public IEnumerable<DtoList> GetPriority()
        {
            var result = new List<DtoList>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from param in dbContext.GroupsPriorities
                              select new DtoList
                              {
                                  Id = param.PriorityId,
                                  Text = param.PriorityName
                              }).OrderBy(z => z.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public IEnumerable<DtoList> GetGroups()
        {
            var result = new List<DtoList>();

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from param in dbContext.LocationGroups
                              where param.Status == true
                              select new DtoList
                              {
                                  Id = param.GroupId,
                                  Text = param.GroupName,
                                  otherIntVal = param.ShippingStatus == null ? 0 : param.ShippingStatus
                              }).OrderBy(z => z.Text).ToList();
                }
            }

            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion Group implementation for location



    }
}
