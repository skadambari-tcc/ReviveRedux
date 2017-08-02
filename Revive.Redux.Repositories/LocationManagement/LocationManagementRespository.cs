using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Common;
using Revive.Redux;
using Revive.Redux.Entities;

namespace Revive.Redux.Repositories
{
    public class LocationManagementRespository : ILocationManagementRespository
    {

        public List<CustomerLocationModel> GetCustomerLocationsById(int CustomerId)
        {
            try
            {
                var locations = new List<CustomerLocationModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    if (CustomerId == 0)
                    {
                        var lstcollection = (from d in dbContext.Customers_Locations
                                             where d.Customer_ID > CustomerId
                                             select new CustomerLocationModel
                                             {
                                                 Location_ID = d.Location_ID,
                                                 Customer_ID = d.Customer_ID,
                                                 StoreNumber = d.StoreNumber,
                                                 Location_Name = d.Location_Name,
                                                 Address = d.Address,
                                                 Additional_Address_info = d.Additional_Address_info,
                                                 City = d.City,
                                                 Area = d.Area,
                                                 State = d.State,
                                                 Country = d.Country,
                                                 ZipCode = d.ZipCode,
                                                 Email_ID = d.Email_ID,
                                                 Phone = d.Phone,
                                                 Phone_Ext = d.Phone,
                                                 SToreOpentime = d.SToreOpentime,
                                                 SToreClosetime = d.SToreClosetime,
                                                 Customfield1 = d.Customfield1,
                                                 Customfield2 = d.Customfield2,
                                                 Customfield3 = d.Customfield3,
                                                 Customfield4 = d.Customfield4,
                                                 Customfield5 = d.Customfield5,
                                                 Created_by = d.Created_by,
                                                 Created_Date = d.Created_Date,
                                                 Modified_by = d.Modified_by,
                                                 Modified_Date = d.Modified_Date,
                                                 Status = d.Status,
                                                 SubAgentName =d.SubAgent.SubAgentName, //dbContext.SubAgents.Where(a => a.SubAgent_ID == d.SubAgent_ID).Select(a => a.SubAgentName).FirstOrDefault(),
                                                 CityName = d.City_Name, //dbContext.Cities.Where(a => a.CityId == d.City).Select(a => a.city1).FirstOrDefault(),
                                                 StateName = dbContext.States.Where(a => a.StateId == d.State).Select(a => a.state_code).FirstOrDefault(),
                                                 CustomerName = d.Customer.Customer_Name,//dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                                 GroupName = d.LocationGroup.GroupName
                                             }).ToList();
                        locations.AddRange(lstcollection);
                    }
                    else
                    {
                        var lstcollection = (from d in dbContext.Customers_Locations
                                             where d.Customer_ID == CustomerId
                                             select new CustomerLocationModel
                                             {
                                                 Location_ID = d.Location_ID,
                                                 Customer_ID = d.Customer_ID,
                                                 StoreNumber = d.StoreNumber,
                                                 Location_Name = d.Location_Name,
                                                 Address = d.Address,
                                                 Additional_Address_info = d.Additional_Address_info,
                                                 City = d.City,
                                                 Area = d.Area,
                                                 State = d.State,
                                                 Country = d.Country,
                                                 ZipCode = d.ZipCode,
                                                 Email_ID = d.Email_ID,
                                                 Phone = d.Phone,
                                                 Phone_Ext = d.Phone,
                                                 SToreOpentime = d.SToreOpentime,
                                                 SToreClosetime = d.SToreClosetime,
                                                 Customfield1 = d.Customfield1,
                                                 Customfield2 = d.Customfield2,
                                                 Customfield3 = d.Customfield3,
                                                 Customfield4 = d.Customfield4,
                                                 Customfield5 = d.Customfield5,
                                                 Created_by = d.Created_by,
                                                 Created_Date = d.Created_Date,
                                                 Modified_by = d.Modified_by,
                                                 Modified_Date = d.Modified_Date,
                                                 Status = d.Status,
                                                 SubAgentName = dbContext.SubAgents.Where(a => a.SubAgent_ID == d.SubAgent_ID).Select(a => a.SubAgentName).FirstOrDefault(),
                                                 CityName = d.City_Name, //dbContext.Cities.Where(a => a.CityId == d.City).Select(a => a.city1).FirstOrDefault() ,
                                                 StateName = dbContext.States.Where(a => a.StateId == d.State).Select(a => a.state_code).FirstOrDefault(),
                                                 CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                                 GroupName = d.LocationGroup.GroupName
                                             }).ToList();
                        locations.AddRange(lstcollection);
                    }

                }
                return locations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<CustomerLocationModel> GetCustomerLocationsById(int CustomerId, Guid AccountManagerId)
        {
            try
            {
                var locations = new List<CustomerLocationModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    if (CustomerId == 0)
                    {
                        var lstcollection = (from d in dbContext.Customers_Locations
                                             join cust in dbContext.Customers on d.Customer_ID equals cust.Customer_ID

                                             where cust.AccountmanagerUserID == AccountManagerId
                                             select new CustomerLocationModel
                                             {
                                                 Location_ID = d.Location_ID,
                                                 Customer_ID = d.Customer_ID,
                                                 StoreNumber = d.StoreNumber,
                                                 Location_Name = d.Location_Name,
                                                 Address = d.Address,
                                                 Additional_Address_info = d.Additional_Address_info,
                                                 City = d.City,
                                                 Area = d.Area,
                                                 State = d.State,
                                                 Country = d.Country,
                                                 ZipCode = d.ZipCode,
                                                 Email_ID = d.Email_ID,
                                                 Phone = d.Phone,
                                                 Phone_Ext = d.Phone,
                                                 SToreOpentime = d.SToreOpentime,
                                                 SToreClosetime = d.SToreClosetime,
                                                 Customfield1 = d.Customfield1,
                                                 Customfield2 = d.Customfield2,
                                                 Customfield3 = d.Customfield3,
                                                 Customfield4 = d.Customfield4,
                                                 Customfield5 = d.Customfield5,
                                                 Created_by = d.Created_by,
                                                 Created_Date = d.Created_Date,
                                                 //strCreated_Date = Convert.ToDateTime(d.Created_Date).ToString("MM/dd/yyyy"),
                                                 Modified_by = d.Modified_by,
                                                 Modified_Date = d.Modified_Date,
                                                 Status = d.Status,
                                                 SubAgentName = dbContext.SubAgents.Where(a => a.SubAgent_ID == d.SubAgent_ID).Select(a => a.SubAgentName).FirstOrDefault(),
                                                 CityName = dbContext.Cities.Where(a => a.CityId == d.City).Select(a => a.city1).FirstOrDefault(),
                                                 StateName = dbContext.States.Where(a => a.StateId == d.State).Select(a => a.state_code).FirstOrDefault(),
                                                 CustomerName = cust.Customer_Name,
                                                 GroupName = d.LocationGroup.GroupName// dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault()
                                             }).ToList();
                        locations.AddRange(lstcollection);
                    }
                    else
                    {
                        var lstcollection = (from d in dbContext.Customers_Locations
                                             join cust in dbContext.Customers on d.Customer_ID equals cust.Customer_ID
                                             where cust.AccountmanagerUserID == AccountManagerId && d.Customer_ID == CustomerId
                                             select new CustomerLocationModel
                                             {
                                                 Location_ID = d.Location_ID,
                                                 Customer_ID = d.Customer_ID,
                                                 StoreNumber = d.StoreNumber,
                                                 Location_Name = d.Location_Name,
                                                 Address = d.Address,
                                                 Additional_Address_info = d.Additional_Address_info,
                                                 City = d.City,
                                                 Area = d.Area,
                                                 State = d.State,
                                                 Country = d.Country,
                                                 ZipCode = d.ZipCode,
                                                 Email_ID = d.Email_ID,
                                                 Phone = d.Phone,
                                                 Phone_Ext = d.Phone,
                                                 SToreOpentime = d.SToreOpentime,
                                                 SToreClosetime = d.SToreClosetime,
                                                 Customfield1 = d.Customfield1,
                                                 Customfield2 = d.Customfield2,
                                                 Customfield3 = d.Customfield3,
                                                 Customfield4 = d.Customfield4,
                                                 Customfield5 = d.Customfield5,
                                                 Created_by = d.Created_by,
                                                 Created_Date = d.Created_Date,
                                                 // strCreated_Date=Convert.ToDateTime(d.Created_Date).ToString("MM/dd/yyyy"),
                                                 Modified_by = d.Modified_by,
                                                 Modified_Date = d.Modified_Date,
                                                 Status = d.Status,
                                                 SubAgentName = dbContext.SubAgents.Where(a => a.SubAgent_ID == d.SubAgent_ID).Select(a => a.SubAgentName).FirstOrDefault(),
                                                 CityName = dbContext.Cities.Where(a => a.CityId == d.City).Select(a => a.city1).FirstOrDefault(),
                                                 StateName = dbContext.States.Where(a => a.StateId == d.State).Select(a => a.state_code).FirstOrDefault(),
                                                 CustomerName = cust.Customer_Name, //dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                                 GroupName = d.LocationGroup.GroupName
                                             }).ToList();
                        locations.AddRange(lstcollection);
                    }

                }
                return locations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<CustomerLocationModel> GetCustomerLocationsBySubsidiaryAdminUser(Guid SubsidiaryAdminUserId)
        {
            try
            {
                var locations = new List<CustomerLocationModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Customers_Locations
                                         join u in dbContext.Users on d.Subsidiary_ID equals u.Subsidiary_ID
                                         where u.User_ID == SubsidiaryAdminUserId
                                         select new CustomerLocationModel
                                         {
                                             Location_ID = d.Location_ID,
                                             Customer_ID = d.Customer_ID,
                                             StoreNumber = d.StoreNumber,
                                             Location_Name = d.Location_Name,
                                             Address = d.Address,
                                             Additional_Address_info = d.Additional_Address_info,
                                             City = d.City,
                                             Area = d.Area,
                                             State = d.State,
                                             Country = d.Country,
                                             ZipCode = d.ZipCode,
                                             Email_ID = d.Email_ID,
                                             Phone = d.Phone,
                                             Phone_Ext = d.Phone,
                                             SToreOpentime = d.SToreOpentime,
                                             SToreClosetime = d.SToreClosetime,
                                             Customfield1 = d.Customfield1,
                                             Customfield2 = d.Customfield2,
                                             Customfield3 = d.Customfield3,
                                             Customfield4 = d.Customfield4,
                                             Customfield5 = d.Customfield5,
                                             Created_by = d.Created_by,
                                             Created_Date = d.Created_Date,
                                             //strCreated_Date = Convert.ToDateTime(d.Created_Date).ToString("MM/dd/yyyy"),
                                             Modified_by = d.Modified_by,
                                             Modified_Date = d.Modified_Date,
                                             Status = d.Status,
                                             SubAgentName = dbContext.SubAgents.Where(a => a.SubAgent_ID == d.SubAgent_ID).Select(a => a.SubAgentName).FirstOrDefault(),
                                             CityName = dbContext.Cities.Where(a => a.CityId == d.City).Select(a => a.city1).FirstOrDefault(),
                                             StateName = dbContext.States.Where(a => a.StateId == d.State).Select(a => a.state_code).FirstOrDefault(),
                                             CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault()//cust.Customer_Name// dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault()
                                         }).ToList();
                    locations.AddRange(lstcollection);

                }
                return locations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<CustomerLocationModel> GetCustomerLocationsBySubAgentAdminUser(Guid SubAgentAdminUserId)
        {
            try
            {
                var locations = new List<CustomerLocationModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Customers_Locations
                                         join u in dbContext.Users on d.SubAgent_ID equals u.SubAgent_ID
                                         where u.User_ID == SubAgentAdminUserId
                                         select new CustomerLocationModel
                                         {
                                             Location_ID = d.Location_ID,
                                             Customer_ID = d.Customer_ID,
                                             StoreNumber = d.StoreNumber,
                                             Location_Name = d.Location_Name,
                                             Address = d.Address,
                                             Additional_Address_info = d.Additional_Address_info,
                                             City = d.City,
                                             Area = d.Area,
                                             State = d.State,
                                             Country = d.Country,
                                             ZipCode = d.ZipCode,
                                             Email_ID = d.Email_ID,
                                             Phone = d.Phone,
                                             Phone_Ext = d.Phone,
                                             SToreOpentime = d.SToreOpentime,
                                             SToreClosetime = d.SToreClosetime,
                                             Customfield1 = d.Customfield1,
                                             Customfield2 = d.Customfield2,
                                             Customfield3 = d.Customfield3,
                                             Customfield4 = d.Customfield4,
                                             Customfield5 = d.Customfield5,
                                             Created_by = d.Created_by,
                                             Created_Date = d.Created_Date,
                                             //strCreated_Date = Convert.ToDateTime(d.Created_Date).ToString("MM/dd/yyyy"),
                                             Modified_by = d.Modified_by,
                                             Modified_Date = d.Modified_Date,
                                             Status = d.Status,
                                             SubAgentName = dbContext.SubAgents.Where(a => a.SubAgent_ID == d.SubAgent_ID).Select(a => a.SubAgentName).FirstOrDefault(),
                                             CityName = dbContext.Cities.Where(a => a.CityId == d.City).Select(a => a.city1).FirstOrDefault(),
                                             StateName = dbContext.States.Where(a => a.StateId == d.State).Select(a => a.state_code).FirstOrDefault(),
                                             CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault()//cust.Customer_Name// dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault()
                                         }).ToList();
                    locations.AddRange(lstcollection);

                }
                return locations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // This method commented by KD on dated 28-05-2015 , it is available in general service

        //public bool ActivateDeactivateLocation(CustomerLocationModel objLocation)
        //{

        //    var bResult = false;
        //    try
        //    {
        //        using (var dbContext = new ReviveDBEntities())
        //        {
        //            if (objLocation.Location_ID > 0 && objLocation.Customer_ID > 0)
        //            {
        //                var objCustomerLocationModel = dbContext.Customers_Locations.FirstOrDefault(cond => cond.Location_ID == objLocation.Location_ID);

        //                if (objCustomerLocationModel != null)
        //                {
        //                    objCustomerLocationModel.Status = !objLocation.Status;                            

        //                    objCustomerLocationModel.Modified_Date = objLocation.Modified_Date;
        //                    objCustomerLocationModel.Modified_by = objLocation.Modified_by;                            
        //                }
        //               // deactivate the store user name and password 
        //                var objUserList = dbContext.Users.Where(cond => cond.Customer_ID == objLocation.Customer_ID);

        //                if (objUserList != null)
        //                {
        //                    foreach (User objuser in objUserList)
        //                    {
        //                        objuser.status = !objLocation.Status;
        //                        objuser.Modified_Date = objLocation.Modified_Date;
        //                        objuser.Modified_by = objLocation.Modified_by;
        //                    }
        //                }

        //                dbContext.SaveChanges();
        //                bResult = true;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return bResult;
        //}

        public bool AddEditLocation(LocationModel objLocation)
        {
            try
            {
                var objCustomerLocationModel = new Customers_Locations();
                using (var dbContext = new ReviveDBEntities())
                {

                    objCustomerLocationModel = MapDataModels(objLocation, objCustomerLocationModel);

                    if (objLocation.PageMode == 0)
                    {
                        dbContext.Customers_Locations.Add(objCustomerLocationModel);
                    }
                    else if (objLocation.PageMode == 1)
                    {
                        objCustomerLocationModel = dbContext.Customers_Locations.FirstOrDefault(cond => cond.Location_ID == objCustomerLocationModel.Location_ID);

                        if (objCustomerLocationModel != null)
                        {
                            objCustomerLocationModel = MapDataModels(objLocation, objCustomerLocationModel);
                        }
                    }

                    dbContext.SaveChanges();

                }
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                throw;

            }
            return false;
        }


        private Customers_Locations MapDataModels(LocationModel objLocation, Customers_Locations objCustomerLocationModel)
        {

            objCustomerLocationModel.Location_ID = objLocation.LocationId;
            objCustomerLocationModel.Customer_ID = objLocation.CustomerId;
            objCustomerLocationModel.StoreNumber = objLocation.StoreNumber;
            objCustomerLocationModel.Location_Name = objLocation.LocationName;
            objCustomerLocationModel.Address = objLocation.AddressLine1;
            objCustomerLocationModel.Additional_Address_info = objLocation.AddressLine2;
            //objCustomerLocationModel.City = objLocation.City; Commneted by KD on Dated 27-Aug-2018  By Refering bug no 7140
            objCustomerLocationModel.City_Name = objLocation.CityName; // Added new 
            objCustomerLocationModel.Area = null;
            objCustomerLocationModel.State = objLocation.State;
            objCustomerLocationModel.Country = objLocation.Country;
            objCustomerLocationModel.ZipCode = objLocation.ZipCode;
            objCustomerLocationModel.Email_ID = null;
            objCustomerLocationModel.Phone = objLocation.PrimaryPhone;
            objCustomerLocationModel.Phone_Ext = objLocation.PrimaryPhoneExt;
            objCustomerLocationModel.SToreOpentime = Convert.ToString(objLocation.StoreOpeningTime);
            objCustomerLocationModel.SToreClosetime = Convert.ToString(objLocation.StoreClosingTime);
            objCustomerLocationModel.Customfield1 = objLocation.CustomField1;
            objCustomerLocationModel.Customfield2 = objLocation.CustomField2;
            objCustomerLocationModel.Customfield3 = objLocation.CustomField3;
            objCustomerLocationModel.Customfield4 = objLocation.CustomField4;
            objCustomerLocationModel.Customfield5 = objLocation.CustomField5;
            objCustomerLocationModel.Modified_by = objLocation.Modified_by;
            objCustomerLocationModel.Modified_Date = objLocation.Modified_Date;
            objCustomerLocationModel.Status = objLocation.Status;
            objCustomerLocationModel.GroupId = objLocation.GroupId;


            objCustomerLocationModel.Subsidiary_ID = objLocation.SubsidiaryId;
            objCustomerLocationModel.SubAgent_ID = objLocation.SubAgentId;

            if (objLocation.PageMode == 0)
            {
                objCustomerLocationModel.Created_by = objLocation.Created_by;
                objCustomerLocationModel.Created_Date = objLocation.Created_Date;
                objCustomerLocationModel.Status = true;
            }




            return objCustomerLocationModel;
        }


        public CustomerLocationModel GetCustomerLocation(int nLocationId)
        {
            try
            {
                var locations = new CustomerLocationModel();
                using (var dbContext = new ReviveDBEntities())
                {
                    var lstcollection = (from d in dbContext.Customers_Locations
                                         where d.Location_ID == nLocationId
                                         select new CustomerLocationModel
                                         {
                                             Location_ID = d.Location_ID,
                                             Customer_ID = d.Customer_ID,
                                             StoreNumber = d.StoreNumber,
                                             Location_Name = d.Location_Name,
                                             Address = d.Address,
                                             Additional_Address_info = d.Additional_Address_info,
                                             City = d.City,
                                             CityName = d.City_Name,
                                             Area = d.Area,
                                             State = d.State,
                                             Country = d.Country,
                                             ZipCode = d.ZipCode,
                                             Email_ID = d.Email_ID,
                                             Phone = d.Phone,
                                             Phone_Ext = d.Phone,
                                             SToreOpentime = d.SToreOpentime,
                                             SToreClosetime = d.SToreClosetime,
                                             Customfield1 = d.Customfield1,
                                             Customfield2 = d.Customfield2,
                                             Customfield3 = d.Customfield3,
                                             Customfield4 = d.Customfield4,
                                             Customfield5 = d.Customfield5,
                                             Created_by = d.Created_by,
                                             Created_Date = d.Created_Date,
                                             Modified_by = d.Modified_by,
                                             Modified_Date = d.Modified_Date,
                                             SubsidiaryId = (int)d.Subsidiary_ID == null ? 0 : (int)d.Subsidiary_ID,
                                             SubAgentId = (int)d.SubAgent_ID == null ? 0 : (int)d.SubAgent_ID,
                                             Status = d.Status,
                                             GroupName = d.LocationGroup.GroupName,
                                             GroupId = d.LocationGroup.GroupId == null ? 0 : d.LocationGroup.GroupId,
                                             ShippingStatusId = dbContext.LocationGroups.Where(c => c.GroupId == d.LocationGroup.GroupId).Select(c => c.ShippingStatus).FirstOrDefault() != null ? (int)dbContext.LocationGroups.Where(c => c.GroupId == d.LocationGroup.GroupId).Select(c => c.ShippingStatus).FirstOrDefault() : 0
                                         }).FirstOrDefault();
                    locations = lstcollection;
                }
                return locations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int GetCityIdByCityName(string sCityName, int nStateId)
        {
            try
            {
                int nCityId = 0;
                using (var dbContext = new ReviveDBEntities())
                {
                    nCityId = dbContext.Cities.Where(a => (a.city1.ToLower() == sCityName.ToLower() && a.StateId == nStateId)).Select(a => a.CityId).FirstOrDefault();
                }
                return nCityId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GetStateIdByStateName(string sStateName)
        {
            try
            {
                int nStateId = 0;
                using (var dbContext = new ReviveDBEntities())
                {
                    nStateId = dbContext.States.Where(a => a.state_code.ToLower() == sStateName.ToLower()).Select(a => a.StateId).FirstOrDefault();
                }
                return nStateId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        /// <summary>
        /// Checks duplicacy of Store Number for a given Customer while adding/editing Customer Location.
        /// </summary>
        /// <param name="storeNo">New Store Number entered by the User</param>
        /// <param name="custID">Customer ID selected by the User</param>
        /// <param name="locID">Location ID (Primary Key) used only while editing Customer Location</param>
        /// <returns></returns>
        public bool CheckDuplicateStoreNo(string storeNo, int custID, int locID)
        {
            try
            {
                bool isExists = false;

                using (var dbContext = new ReviveDBEntities())
                {
                    var allStoreNoCustWise = new List<string>();
                    if (locID != 0)
                    {
                        allStoreNoCustWise = (from c in dbContext.Customers_Locations where c.Customer_ID == custID && c.Location_ID != locID && c.StoreNumber.ToLower() == storeNo.ToLower() select c.StoreNumber).ToList();
                    }
                    else
                        allStoreNoCustWise = (from c in dbContext.Customers_Locations where c.Customer_ID == custID && c.StoreNumber.ToLower() == storeNo.ToLower() select c.StoreNumber).ToList();
                    if (allStoreNoCustWise.Any())
                        isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Checks duplicacy of Customer Reference Code while adding/editing Customer.
        /// </summary>
        /// <param name="custRefCode">New Customer Reference entered by the User</param>
        /// <param name="custID">Customer ID (Primary Key) used only while editing Customer</param>
        /// <returns></returns>
        public bool CheckDuplicateCustID(string custRefCode, String custID)
        {
            try
            {
                bool isExists = false;
                using (var dbContext = new ReviveDBEntities())
                {
                    var allCustRefCode = new List<string>();
                    if (!string.IsNullOrEmpty(custID) && custID != "0")
                    {
                        var intCustID = Convert.ToInt32(custID);
                        allCustRefCode = (from cust in dbContext.Customers where cust.Customer_Ref_Code.ToLower() == custRefCode.ToLower() && cust.Customer_ID != intCustID select cust.Customer_Ref_Code).ToList();
                    }
                    else
                        allCustRefCode = (from cust in dbContext.Customers where cust.Customer_Ref_Code.ToLower() == custRefCode.ToLower() select cust.Customer_Ref_Code).ToList();
                    if (allCustRefCode.Any())
                        isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetLocationIdByLocationName(string _locationName, int CustomerId)
        {
            int _LocationId = 0;
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {

                    if (_locationName != null)
                    {
                        _LocationId = dbContext.Customers_Locations.Where(c => c.Customer_ID == CustomerId && c.Location_Name.ToUpper() == _locationName.ToUpper().Trim()).Select(a => a.Location_ID).FirstOrDefault();
                    }

                }
                return _LocationId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetLocationIdByStoreNumber(string _StoreNumber, int CustomerId)
        {
            int _LocationId = 0;
            try
            {

                using (var dbContext = new ReviveDBEntities())
                {

                    if (_StoreNumber != null)
                    {
                        _LocationId = dbContext.Customers_Locations.Where(c => c.Customer_ID == CustomerId && c.StoreNumber.ToUpper() == _StoreNumber.ToUpper().Trim()).Select(a => a.Location_ID).FirstOrDefault();
                    }

                }
                return _LocationId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CustomerLocationModel> GetCustomerLocations(CustomerLocationModel objCustomerLocation, Guid UserId, string pageAccessCode)
        {
            try
            {
                var locations = new List<CustomerLocationModel>();
                using (var dbContext = new ReviveDBEntities())
                {
                    if (UserId != null)
                    {
                        var AdminUserId = dbContext.Users.Where(m => m.User_ID == UserId).FirstOrDefault();

                        if (pageAccessCode == PageAccessCode.ACCNTMGR)
                        {
                            objCustomerLocation.Customer_ID = (int)AdminUserId.Customer_ID;
                        }
                        else if (pageAccessCode == PageAccessCode.CUSTOMERADMIN)
                        {
                            objCustomerLocation.Customer_ID = (int)AdminUserId.Customer_ID;
                        }
                        else if (pageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                        {
                            objCustomerLocation.SubsidiaryId = (int)AdminUserId.Subsidiary_ID;
                        }
                        else if (pageAccessCode == PageAccessCode.SUBAGENTADMIN)
                        {
                            objCustomerLocation.SubAgentId = (int)AdminUserId.SubAgent_ID;
                        }
                    }

                    var lstcollection = (from d in dbContext.Customers_Locations
                                         select new CustomerLocationModel
                                         {
                                             Location_ID = d.Location_ID,
                                             Customer_ID = d.Customer_ID,
                                             StoreNumber = d.StoreNumber,
                                             Location_Name = d.Location_Name,
                                             Address = d.Address,
                                             Additional_Address_info = d.Additional_Address_info,
                                             City = d.City,
                                             Area = d.Area,
                                             State = d.State,
                                             Country = d.Country,
                                             ZipCode = d.ZipCode,
                                             Email_ID = d.Email_ID,
                                             Phone = d.Phone,
                                             Phone_Ext = d.Phone,
                                             SToreOpentime = d.SToreOpentime,
                                             SToreClosetime = d.SToreClosetime,
                                             Customfield1 = d.Customfield1,
                                             Customfield2 = d.Customfield2,
                                             Customfield3 = d.Customfield3,
                                             Customfield4 = d.Customfield4,
                                             Customfield5 = d.Customfield5,
                                             Created_by = d.Created_by,
                                             Created_Date = d.Created_Date,
                                             Modified_by = d.Modified_by,
                                             Modified_Date = d.Modified_Date,
                                             Status = d.Status,
                                             SubAgentName = dbContext.SubAgents.Where(a => a.SubAgent_ID == d.SubAgent_ID).Select(a => a.SubAgentName).FirstOrDefault(),
                                             CityName = d.City_Name, //dbContext.Cities.Where(a => a.CityId == d.City).Select(a => a.city1).FirstOrDefault(),
                                             StateName = dbContext.States.Where(a => a.StateId == d.State).Select(a => a.state_code).FirstOrDefault(),
                                             CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                             SubsidiaryId = (int)d.Subsidiary_ID,
                                             SubAgentId = (int)d.SubAgent_ID,
                                             GroupName = d.LocationGroup.GroupName,
                                             GroupId = (int)d.GroupId != null ? (int)d.GroupId : 0,
                                             ShippingStatusId = (int)d.ShippingStatusId != null ? (int)d.ShippingStatusId : 0,
                                             ShippingStatusName = dbContext.MasterData_Config_definitions.Where(m => m.MasterData_Type == ConstantEntities.ShippingStatusName && m.MasterData_Type_ID == d.ShippingStatusId).Select(m => m.MasterData_Value).FirstOrDefault()
                                         }).ToList();

                    if (objCustomerLocation.SubAgentId > 0)
                    {
                        if (objCustomerLocation.Customer_ID > 0 && objCustomerLocation.SubsidiaryId > 0)
                        {

                            lstcollection = lstcollection.Where(m => m.Customer_ID == objCustomerLocation.Customer_ID && m.SubsidiaryId == objCustomerLocation.SubsidiaryId && m.SubAgentId == objCustomerLocation.SubAgentId).ToList();
                        }
                        else
                        {
                            lstcollection = lstcollection.Where(m => m.SubAgentId == objCustomerLocation.SubAgentId).ToList();
                        }
                    }
                    else if (objCustomerLocation.SubsidiaryId > 0)
                    {
                        if (objCustomerLocation.Customer_ID > 0)
                        {
                            lstcollection = lstcollection.Where(m => m.Customer_ID == objCustomerLocation.Customer_ID && m.SubsidiaryId == objCustomerLocation.SubsidiaryId).ToList();
                        }
                        else
                        {
                            lstcollection = lstcollection.Where(m => m.SubsidiaryId == objCustomerLocation.SubsidiaryId).ToList();
                        }
                    }
                    else if (objCustomerLocation.Customer_ID > 0)
                    {
                        lstcollection = lstcollection.Where(m => m.Customer_ID == objCustomerLocation.Customer_ID).ToList();
                    }
                    else if (objCustomerLocation.GroupId > 0)
                    {
                        lstcollection = lstcollection.Where(m => m.GroupId == objCustomerLocation.GroupId).ToList();
                    }

                    locations.AddRange(lstcollection);

                }
                return locations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




    }
}
