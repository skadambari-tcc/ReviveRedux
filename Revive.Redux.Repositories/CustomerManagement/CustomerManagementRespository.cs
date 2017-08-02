using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public class CustomerManagementRespository : ICustomerManagementRespository
    {
        /// <summary>
        /// Get All Customer Records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManageCustomersModel> GetCustomersList()
        {
            try
            {
                IEnumerable<ManageCustomersModel> ObjRoleDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    ObjRoleDetails = dbContext.Customers.Select(d => new ManageCustomersModel()
                    {
                        AccountmanagerUserID = d.AccountmanagerUserID,
                        Acct_manager_Name = (dbContext.Users.Where(a => a.User_ID == d.AccountmanagerUserID).Select(a => a.FirstName + " " + (a.LastName != null ? a.LastName : "")).FirstOrDefault()),
                        Primary_Address = d.Primary_Address,
                        //Additional_Address_info = d.Additional_Address_info,
                        // Primary_City = d.Primary_City,
                        Primary_City_Name = d.Primary_City_Name,
                        Customer_ID = d.Customer_ID,
                        Email_ID = d.Email_ID,
                        Status = d.Status,
                        Customer_Name = d.Customer_Name,
                        Customer_Ref_Code = d.Customer_Ref_Code,
                        Primary_Phone = d.Primary_Phone,
                        Lease_Start_Date = d.Lease_Start_Date,
                        Lease_end_date = d.Lease_end_date,
                        //  CustomerPO = Convert.ToString(d.Customer_Documents),
                        Primary_State = d.Primary_State,
                        Primary_ZipCode = d.Primary_ZipCode

                    }).ToList();

                }

                return ObjRoleDetails;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IEnumerable<ManageCustomersModel> GetCustomersList(Guid AccountManagerId)
        {
            try
            {
                IEnumerable<ManageCustomersModel> ObjRoleDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    ObjRoleDetails = dbContext.Customers.Where(x => x.AccountmanagerUserID == AccountManagerId).Select(d => new ManageCustomersModel()
                    {
                        AccountmanagerUserID = d.AccountmanagerUserID,
                        Acct_manager_Name = (dbContext.Users.Where(a => a.User_ID == d.AccountmanagerUserID).Select(a => a.FirstName + " " + (a.LastName != null ? a.LastName : "")).FirstOrDefault()),
                        Primary_Address = d.Primary_Address,
                        //Additional_Address_info = d.Additional_Address_info,
                        Primary_City = d.Primary_City,
                        Customer_ID = d.Customer_ID,
                        Email_ID = d.Email_ID,
                        Status = d.Status,
                        Customer_Name = d.Customer_Name,
                        Customer_Ref_Code = d.Customer_Ref_Code,
                        Primary_Phone = d.Primary_Phone,
                        Lease_Start_Date = d.Lease_Start_Date,
                        Lease_end_date = d.Lease_end_date,
                        //  CustomerPO = Convert.ToString(d.Customer_Documents),
                        Primary_State = d.Primary_State,
                        Primary_ZipCode = d.Primary_ZipCode

                    }).ToList();

                }

                return ObjRoleDetails;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IEnumerable<ManageCustomersModel> GetCustomersList(int customerId)
        {
            try
            {
                IEnumerable<ManageCustomersModel> ObjRoleDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    ObjRoleDetails = dbContext.Customers.Where(x => x.Customer_ID == customerId).Select(d => new ManageCustomersModel()
                    {
                        AccountmanagerUserID = d.AccountmanagerUserID,
                        Acct_manager_Name = (dbContext.Users.Where(a => a.User_ID == d.AccountmanagerUserID).Select(a => a.FirstName + " " + (a.LastName != null ? a.LastName : "")).FirstOrDefault()),
                        Primary_Address = d.Primary_Address,
                        //Additional_Address_info = d.Additional_Address_info,
                        Primary_City = d.Primary_City,
                        Customer_ID = d.Customer_ID,
                        Email_ID = d.Email_ID,
                        Status = d.Status,
                        Customer_Name = d.Customer_Name,
                        Customer_Ref_Code = d.Customer_Ref_Code,
                        Primary_Phone = d.Primary_Phone,
                        Lease_Start_Date = d.Lease_Start_Date,
                        Lease_end_date = d.Lease_end_date,
                        //  CustomerPO = Convert.ToString(d.Customer_Documents),
                        Primary_State = d.Primary_State,
                        Primary_ZipCode = d.Primary_ZipCode,
                        TempLateId=(int)d.TemplateId,
                        Membership_Duration=d.Membership_Duration,
                        ReviveAllowed = (int)d.NoofRevivesAllowed,
                        CustomerMaxDevices=(int)d.MaxNoOfDevices,
                        IsCustomerMultiDevice=(bool)d.IsMultiDevice
                        
                    }).ToList();

                }

                return ObjRoleDetails;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        /// <summary>
        /// Add and update Customer Record
        /// </summary>
        /// <param name="CustomerRecord"></param>
        /// <returns></returns>
        public ManageCustomersModel CreateCustomer(ManageCustomersModel CustomerRecord)
        {
            //var result = false;
            ManageCustomersModel ReturnRecords = new ManageCustomersModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (CustomerRecord.Customer_ID == 0)
                    {
                        var Record = new Customer();
                        Record.Customer_Name = CustomerRecord.Customer_Name;
                        Record.Customer_Ref_Code = CustomerRecord.Customer_Ref_Code;
                        Record.Acct_manager_Name = CustomerRecord.Acct_manager_Name;
                        // Record.Customer_ID = CustomerRecord.Customer_ID;
                        Record.Email_ID = CustomerRecord.Email_ID;
                        Record.Lease_end_date = CustomerRecord.Lease_end_date;
                        Record.Lease_Start_Date = CustomerRecord.Lease_Start_Date;
                        Record.Primary_Address = CustomerRecord.Primary_Address;
                        //Record.Additional_Address_info = CustomerRecord.Additional_Address_info;
                        Record.Primary_Phone = CustomerRecord.Primary_Phone;
                        Record.AccountmanagerUserID = CustomerRecord.AccountmanagerUserID;
                        // Record.Primary_City = CustomerRecord.Primary_City;
                        Record.Primary_City_Name = CustomerRecord.Primary_City_Name;
                        Record.Primary_State = CustomerRecord.Primary_State;
                        Record.Primary_ZipCode = CustomerRecord.Primary_ZipCode;
                        Record.Acct_manager_Name = CustomerRecord.Acct_manager_Name;
                        //Record.Acct_manager_Additional_Address_info = CustomerRecord.Acct_manager_Additional_Address_info;
                        Record.Acct_manager_Primary_Address = CustomerRecord.Acct_manager_Primary_Address;
                        //Record.Acct_manager_Primary_City = CustomerRecord.Acct_manager_Primary_City;
                        Record.Acct_manager_Primary_City_Name = CustomerRecord.Acct_manager_Primary_City_Name;
                        Record.Acct_manager_Primary_State = CustomerRecord.Acct_manager_Primary_State;
                        Record.Acct_manager_Primary_Zip = CustomerRecord.Acct_manager_Primary_Zip;
                        Record.Acct_manager_Primary_Phone = CustomerRecord.Acct_manager_Primary_Phone;
                        Record.Status = true;
                        Record.Created_Date = System.DateTime.Now;
                        Record.NoofRenewalRevivesAllowed = Common.MembershipConfig.TotalRenewedReviveAllowed;
                        Record.NoofRevivesAllowed = Common.MembershipConfig.TotalReviveAllowed;
                        Record.Membership_Duration = Common.MembershipConfig.MembershipDuration;
                        Record.Membership_renew_duration = Common.MembershipConfig.MembershipRenewDuration;
                        dbContext.Customers.Add(Record);

                        var CutomerDocs = new Customer_Documents();
                        foreach (var item in CustomerRecord.CustomerDocs)
                        {
                            CutomerDocs.Customer_Doc_Name = item.Customer_Doc_Name;
                            CutomerDocs.Customer_Doc_type = item.Customer_Doc_type;
                            CutomerDocs.Doc_Path = item.Doc_Path;
                            CutomerDocs.Created_Date = item.Created_Date;
                            dbContext.Customer_Documents.Add(CutomerDocs);
                        }
                        dbContext.SaveChanges();
                        ReturnRecords.Customer_ID = Record.Customer_ID;
                        ReturnRecords.Successmsg = "Customer Created Successfully";

                    }
                    else
                    {
                        var recordUpdate = dbContext.Customers.FirstOrDefault(cond => cond.Customer_ID == CustomerRecord.Customer_ID);
                        var CustomerDocs = dbContext.Customer_Documents.Where(cond => cond.Customer_ID == CustomerRecord.Customer_ID).ToList();

                        if (recordUpdate != null)
                        {
                            recordUpdate.Customer_Name = CustomerRecord.Customer_Name;

                            recordUpdate.Email_ID = CustomerRecord.Email_ID;
                            recordUpdate.Lease_end_date = CustomerRecord.Lease_end_date;
                            recordUpdate.Lease_Start_Date = CustomerRecord.Lease_Start_Date;
                            recordUpdate.Primary_Address = CustomerRecord.Primary_Address;
                            recordUpdate.Customer_Ref_Code = CustomerRecord.Customer_Ref_Code;
                            //recordUpdate.Additional_Address_info = CustomerRecord.Additional_Address_info;
                            recordUpdate.Primary_Phone = CustomerRecord.Primary_Phone;
                            recordUpdate.AccountmanagerUserID = CustomerRecord.AccountmanagerUserID;
                            //recordUpdate.Primary_City = CustomerRecord.Primary_City;
                            recordUpdate.Primary_City_Name = CustomerRecord.Primary_City_Name;
                            recordUpdate.Primary_State = CustomerRecord.Primary_State;
                            recordUpdate.Primary_ZipCode = CustomerRecord.Primary_ZipCode;
                            recordUpdate.Acct_manager_Name = CustomerRecord.Acct_manager_Name;
                            // recordUpdate.Acct_manager_Additional_Address_info = CustomerRecord.Acct_manager_Additional_Address_info;
                            recordUpdate.Acct_manager_Primary_Address = CustomerRecord.Acct_manager_Primary_Address;
                            //recordUpdate.Acct_manager_Primary_City = CustomerRecord.Acct_manager_Primary_City;
                            recordUpdate.Acct_manager_Primary_City_Name = CustomerRecord.Acct_manager_Primary_City_Name;
                            recordUpdate.Acct_manager_Primary_State = CustomerRecord.Acct_manager_Primary_State;
                            recordUpdate.Acct_manager_Primary_Zip = CustomerRecord.Acct_manager_Primary_Zip;
                            recordUpdate.Acct_manager_Primary_Phone = CustomerRecord.Acct_manager_Primary_Phone;

                            foreach (var item in CustomerDocs)
                            {
                                foreach (var Coll in CustomerRecord.CustomerDocs)
                                {
                                    if (item.Customer_Doc_Name == Coll.Customer_Doc_Name)
                                    {
                                        dbContext.Customer_Documents.Remove(item);

                                    }
                                }

                            }

                            Customer_Documents CutomerDocs = null;
                            foreach (var item in CustomerRecord.CustomerDocs)
                            {
                                CutomerDocs = new Customer_Documents();
                                CutomerDocs.Customer_Doc_Name = item.Customer_Doc_Name;
                                CutomerDocs.Customer_Doc_type = item.Customer_Doc_type;
                                CutomerDocs.Doc_Path = item.Doc_Path;
                                CutomerDocs.Created_Date = item.Created_Date;
                                CutomerDocs.Customer_ID = item.Customer_ID;
                                dbContext.Customer_Documents.Add(CutomerDocs);


                            }
                            dbContext.SaveChanges();
                        }

                        ReturnRecords.Customer_ID = recordUpdate.Customer_ID;

                    }
                    // dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ReturnRecords;

        }

        /// <summary>
        /// Get Customer Details By CustomerID
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public ManageCustomersModel GetCustomerDetailsById(int CustID)
        {

            var result = new ManageCustomersModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var records = (from Customer x in dbContext.Customers.ToList()
                                   where x.Customer_ID == Convert.ToInt16(CustID)
                                   select new ManageCustomersModel
                                 {
                                     Customer_ID = x.Customer_ID,
                                     Customer_Name = x.Customer_Name,
                                     Email_ID = x.Email_ID,
                                     Primary_Phone = x.Primary_Phone,
                                     Primary_State = x.Primary_State,
                                     //Primary_City = x.Primary_City,
                                     Primary_City_Name = x.Primary_City_Name,
                                     Primary_Address = x.Primary_Address,
                                     //Additional_Address_info = x.Additional_Address_info,
                                     Customer_Ref_Code = x.Customer_Ref_Code,
                                     Primary_ZipCode = x.Primary_ZipCode,
                                     Lease_Start_Date = x.Lease_Start_Date,
                                     Lease_end_date = x.Lease_end_date,
                                     Acct_manager_Name = x.Acct_manager_Name,
                                     AccountmanagerUserID = x.AccountmanagerUserID,
                                     // Acct_manager_Additional_Address_info = x.Acct_manager_Additional_Address_info,
                                     Acct_manager_Primary_Address = x.Acct_manager_Primary_Address,
                                     //Acct_manager_Primary_City = x.Acct_manager_Primary_City,
                                     Acct_manager_Primary_City_Name = x.Acct_manager_Primary_City_Name,
                                     Acct_manager_Primary_State = x.Acct_manager_Primary_State,
                                     Acct_manager_Primary_Zip = x.Acct_manager_Primary_Zip,
                                     Acct_manager_Primary_Phone = x.Acct_manager_Primary_Phone,
                                     Status = x.Status


                                 }).FirstOrDefault();

                    result = records;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Activate or Deactivate Customer Status
        /// </summary>
        /// <param name="CustomerRecord"></param>
        /// <returns></returns>
        /// 
        // Commented by KD  on dated 29-May-2015, this method will available in General repository
        //public bool ActivateDeactivateCustomerById(ManageCustomersModel CustomerRecord)
        //{

        //    var result = false;
        //    List<DtoUser> objUserDetails = new List<DtoUser>();
        //    try
        //    {
        //        using (var dbContext = new ReviveDBEntities())
        //        {
        //            if (CustomerRecord.Customer_ID != 0)
        //            {
        //                var recordDeactivate = dbContext.Customers.FirstOrDefault(cond => cond.Customer_ID == CustomerRecord.Customer_ID);
        //                if (recordDeactivate != null)
        //                {
        //                    if (CustomerRecord.Status == true)
        //                    {
        //                        recordDeactivate.Status = false;
        //                    }
        //                    else
        //                    {
        //                        recordDeactivate.Status = true;
        //                    }
        //                }
        //                UserManagementRepository obj = new UserManagementRepository();
        //                obj.ChangeUserStatus(dbContext, CustomerRecord.Customer_ID);
        //                //var objUserList = dbContext.Users.Where(cond => cond.Customer_ID == CustomerRecord.Customer_ID);

        //                //if (objUserList != null)
        //                //{
        //                //    foreach (User objuser in objUserList)
        //                //    {
        //                //        if (objuser.status == true)
        //                //        {
        //                //            objuser.status = false;
        //                //        }
        //                //        else
        //                //        {
        //                //            objuser.status = true;
        //                //        }                               
        //                //    }
        //                //}
        //                dbContext.SaveChanges();
        //                result = true;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return result;
        //}


        /// <summary>
        /// Get Customer Documents Records On the Basis of customerId
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public IEnumerable<CustomerDocumentsModel> GetCustomerDocsList(int CustID)
        {
            try
            {
                IEnumerable<CustomerDocumentsModel> ObjCustomerDocs;
                using (var dbContext = new ReviveDBEntities())
                {
                    ObjCustomerDocs = (from Customer_Documents d in dbContext.Customer_Documents.ToList()
                                       where d.Customer_ID == Convert.ToInt16(CustID)
                                       select new CustomerDocumentsModel
                        {
                            Customer_Doc_Name = d.Customer_Doc_Name,
                            Customer_ID = d.Customer_ID,
                            Customer_Doc_type = d.Customer_Doc_type,
                            Created_Date = d.Created_Date

                        }).ToList();

                }

                return ObjCustomerDocs;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Delete Customer Document Record
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public CustomerDocumentsModel DeleteCustomerDocumentById(CustomerDocumentsModel Obj)
        {
            CustomerDocumentsModel returnRecord = new CustomerDocumentsModel();

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if ((Obj.Customer_ID) != 0)
                    {

                        var record = dbContext.Customer_Documents.FirstOrDefault(cond => cond.Customer_ID == Obj.Customer_ID && cond.Customer_Doc_Name == Obj.Customer_Doc_Name);
                        if (record != null)
                        {

                            returnRecord.Customer_ID = record.Customer_ID;
                            returnRecord.Customer_Doc_Name = record.Customer_Doc_Name;
                            returnRecord.Customer_Doc_type = record.Customer_Doc_type;
                            dbContext.Customer_Documents.Remove(record);
                        }

                    }
                    // save the changes
                    dbContext.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return returnRecord;

        }
        public bool CheckCustomerDocExists(int id)
        {
            bool existFlag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (id != 0)
                    {

                        var record = dbContext.Customer_Documents.FirstOrDefault(cond => cond.Customer_ID == id);
                        if (record != null)
                        {

                            existFlag = true;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return existFlag;
        }

        /// <summary>
        /// Get Details of Customer Documents
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public CustomerDocumentsModel GetCustomerDocumentDetailById(CustomerDocumentsModel Obj)
        {
            CustomerDocumentsModel returnRecord = new CustomerDocumentsModel();

            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if ((Obj.Customer_ID) != 0)
                    {

                        var record = dbContext.Customer_Documents.FirstOrDefault(cond => cond.Customer_ID == Obj.Customer_ID && cond.Customer_Doc_Name == Obj.Customer_Doc_Name);
                        if (record != null)
                        {

                            returnRecord.Customer_ID = record.Customer_ID;
                            returnRecord.Customer_Doc_Name = record.Customer_Doc_Name;
                            returnRecord.Customer_Doc_type = record.Customer_Doc_type;
                        }

                    }
                    // save the changes
                    dbContext.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return returnRecord;

        }
        public Boolean CheckCheckCustomerNameExists(String Name, int CustId)
        {
            var Duplicateflag = true;


            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (CustId != 0)
                    {
                        var record = dbContext.Customers.FirstOrDefault(cond => cond.Customer_Name.ToLower() == Name.ToLower() && cond.Customer_ID != CustId);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }
                    else
                    {
                        var record = dbContext.Customers.FirstOrDefault(cond => cond.Customer_Name.ToLower() == Name.ToLower());
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Duplicateflag;
        }
        /// <summary>
        /// Add Customer Documents Records
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        public string FilePathUpload(List<CustomerDocumentsModel> Object)
        {
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var FileData = new Customer_Documents();
                    foreach (var item in Object)
                    {
                        FileData.Customer_Doc_Name = item.Customer_Doc_Name;
                        FileData.Doc_Path = item.Doc_Path;
                        FileData.Customer_Doc_type = item.Customer_Doc_type;
                        dbContext.Customer_Documents.Add(FileData);

                    }


                    dbContext.SaveChanges();

                }
                return "";
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public IEnumerable<MembershipConfigModel> GetCustomerMembership(int nCustomerId)
        {
            var objMembershipConfig = new List<MembershipConfigModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (nCustomerId == 0)
                    {
                        var objMembershipConfigRecord = (from Customer x in dbContext.Customers.ToList()
                                                         where x.Customer_ID > nCustomerId
                                                         select new MembershipConfigModel
                                                         {
                                                             CustomerId = x.Customer_ID,
                                                             CustomerName = dbContext.Customers.Where(a => a.Customer_ID == x.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                                             MembershipDuration = (x.Membership_Duration != null) ? (int)x.Membership_Duration : 0,
                                                             MembershipRenewDuration = (x.Membership_renew_duration != null) ? (int)x.Membership_renew_duration : 0,
                                                             TotalRenewedReviveAllowed = (x.NoofRenewalRevivesAllowed != null) ? (int)x.NoofRenewalRevivesAllowed : 0,
                                                             TotalReviveAllowed = (x.NoofRevivesAllowed != null) ? (int)x.NoofRevivesAllowed : 0,
                                                             eligibiltyWaitPeriod = (x.EligibiltyWaitPeriod != null) ? (int)x.EligibiltyWaitPeriod : 0,
                                                             VoidMembershipDays = (x.VoidDays != null) ? (int)x.VoidDays : 0,
                                                             IsMultiDevice = (x.IsMultiDevice != null) && Convert.ToBoolean(x.IsMultiDevice),
                                                             NoOfDevices = (x.MaxNoOfDevices != null) ? (int)x.MaxNoOfDevices :1

                                                         }).ToList();
                        objMembershipConfig.AddRange(objMembershipConfigRecord);
                    }
                    else
                    {
                        var objMembershipConfigRecord = (from Customer x in dbContext.Customers.ToList()
                                                         where x.Customer_ID == nCustomerId
                                                         select new MembershipConfigModel
                                                         {
                                                             CustomerId = x.Customer_ID,
                                                             CustomerName = dbContext.Customers.Where(a => a.Customer_ID == x.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                                             MembershipDuration = (x.Membership_Duration != null) ? (int)x.Membership_Duration : 0,
                                                             MembershipRenewDuration = (x.Membership_renew_duration != null) ? (int)x.Membership_renew_duration : 0,
                                                             TotalRenewedReviveAllowed = (x.NoofRenewalRevivesAllowed != null) ? (int)x.NoofRenewalRevivesAllowed : 0,
                                                             TotalReviveAllowed = (x.NoofRevivesAllowed != null) ? (int)x.NoofRevivesAllowed : 0,
                                                             eligibiltyWaitPeriod = (x.EligibiltyWaitPeriod != null) ? (int)x.EligibiltyWaitPeriod : 0,
                                                             VoidMembershipDays = (x.VoidDays != null) ? (int)x.VoidDays : 0,
                                                             IsMultiDevice = (x.IsMultiDevice != null) && Convert.ToBoolean(x.IsMultiDevice),
                                                             NoOfDevices = (x.MaxNoOfDevices != null) ? (int)x.MaxNoOfDevices : 1
                                                         }).ToList();
                        objMembershipConfig.AddRange(objMembershipConfigRecord);
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objMembershipConfig;
        }

        public bool UpdateCustomerMembership(MembershipConfigModel objCustomerMembership)
        {
            var bResult = false;
            var objMembershipConfig = new List<MembershipConfigModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var objMembershipConfigRecord = dbContext.Customers.FirstOrDefault(cond => cond.Customer_ID == objCustomerMembership.CustomerId);

                    if (objMembershipConfigRecord != null)
                    {

                        objMembershipConfigRecord.Membership_Duration = objCustomerMembership.MembershipDuration;
                        objMembershipConfigRecord.Membership_renew_duration = objCustomerMembership.MembershipRenewDuration;
                        objMembershipConfigRecord.NoofRevivesAllowed = objCustomerMembership.TotalReviveAllowed;
                        objMembershipConfigRecord.NoofRenewalRevivesAllowed = objCustomerMembership.TotalRenewedReviveAllowed;
                        objMembershipConfigRecord.EligibiltyWaitPeriod = objCustomerMembership.eligibiltyWaitPeriod;
                        objMembershipConfigRecord.VoidDays = objCustomerMembership.VoidMembershipDays;
                        objMembershipConfigRecord.IsMultiDevice = objCustomerMembership.IsMultiDevice;
                        objMembershipConfigRecord.MaxNoOfDevices = objCustomerMembership.NoOfDevices;
                    }

                    dbContext.SaveChanges();
                    bResult = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return bResult;
        }
    }
}
