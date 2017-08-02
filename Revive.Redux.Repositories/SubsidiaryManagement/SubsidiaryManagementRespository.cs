using Revive.Redux.Common;
using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Repositories
{
    public class SubsidiaryManagementRespository : ISubsidiaryManagementRespository
    {
        /// <summary>
        /// Get All Subsidiary Records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManageSubsidiaryModel> GetSubsidiaryList(Guid userId, string _PageAccessCode)
        {
            try
            {
                IEnumerable<ManageSubsidiaryModel> ObjRoleDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    if (_PageAccessCode == PageAccessCode.ACCNTMGR)
                    {
                        ObjRoleDetails = dbContext.Subsidiaries.Join(dbContext.Customers, cust => cust.Customer_ID, subs => subs.Customer_ID, (subs, cust)
                            => new { subs, cust }).Where(x => x.cust.AccountmanagerUserID == userId).Select(d => new ManageSubsidiaryModel()
                        {
                            Primary_Address = d.subs.Address,
                            Primary_City_Name = d.subs.CityName,
                            CustomerId = d.subs.Customer_ID,
                            Subsidiary_ID = d.subs.Subsidiary_ID,
                            CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.subs.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                            Email_ID = d.subs.Email_ID,
                            Status = d.subs.Status,
                            Subsidiary_Name = d.subs.SubsidiaryName,
                            Primary_Phone = d.subs.Phone,
                            Primary_State = d.subs.State,
                            Subsidiary_Ref_Code=d.subs.Subsidiary_Ref_Code
                        }).ToList();
                    }
                    else if (_PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                    {
                        ObjRoleDetails = dbContext.Subsidiaries.Join(dbContext.Users, cust => cust.Customer_ID, subs => subs.Customer_ID, (subs, cust)
                            => new { subs, cust }).Where(x => x.cust.User_ID == userId).Select(d => new ManageSubsidiaryModel()
                            {
                                Primary_Address = d.subs.Address,
                                Primary_City_Name = d.subs.CityName,
                                CustomerId = d.subs.Customer_ID,
                                Subsidiary_ID = d.subs.Subsidiary_ID,
                                CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.subs.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                Email_ID = d.subs.Email_ID,
                                Status = d.subs.Status,
                                Subsidiary_Name = d.subs.SubsidiaryName,
                                Primary_Phone = d.subs.Phone,
                                Primary_State = d.subs.State,
                                Subsidiary_Ref_Code = d.subs.Subsidiary_Ref_Code
                            }).ToList();
                    }
                    else if (_PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                    {
                        ObjRoleDetails = dbContext.Subsidiaries.Join(dbContext.Users, cust => cust.Subsidiary_ID, subs => subs.Subsidiary_ID, (subs, cust)
                            => new { subs, cust }).Where(x => x.cust.User_ID == userId).Select(d => new ManageSubsidiaryModel()
                            {
                                Primary_Address = d.subs.Address,
                                Primary_City_Name = d.subs.CityName,
                                CustomerId = d.subs.Customer_ID,
                                Subsidiary_ID = d.subs.Subsidiary_ID,
                                CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.subs.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                Email_ID = d.subs.Email_ID,
                                Status = d.subs.Status,
                                Subsidiary_Name = d.subs.SubsidiaryName,
                                Primary_Phone = d.subs.Phone,
                                Primary_State = d.subs.State,
                                Subsidiary_Ref_Code = d.subs.Subsidiary_Ref_Code
                            }).ToList();
                    }
                    else
                    {
                        ObjRoleDetails = dbContext.Subsidiaries.Select(d => new ManageSubsidiaryModel()
                        {
                            Primary_Address = d.Address,
                            Primary_City_Name = d.CityName,
                            CustomerId = d.Customer_ID,
                            Subsidiary_ID = d.Subsidiary_ID,
                            CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                            Email_ID = d.Email_ID,
                            Status = d.Status,
                            Subsidiary_Name = d.SubsidiaryName,
                            Primary_Phone = d.Phone,
                            Primary_State = d.State,
                            Subsidiary_Ref_Code = d.Subsidiary_Ref_Code
                        }).ToList();
                    }

                }
                return ObjRoleDetails;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ManageSubsidiaryModel CreateSubsidiary(ManageSubsidiaryModel SubsidiaryRecord)
        {
            ManageSubsidiaryModel ReturnRecords = new ManageSubsidiaryModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (SubsidiaryRecord.Subsidiary_ID == 0)
                    {
                        var Record = new Subsidiary();
                        Record.Customer_ID = SubsidiaryRecord.CustomerId;
                        Record.SubsidiaryName = SubsidiaryRecord.Subsidiary_Name;
                        Record.Email_ID = SubsidiaryRecord.Email_ID;
                        Record.Address = SubsidiaryRecord.Primary_Address;
                        Record.Phone = SubsidiaryRecord.Primary_Phone;
                        Record.CityName = SubsidiaryRecord.Primary_City_Name;
                        Record.State = SubsidiaryRecord.Primary_State;
                        Record.ZipCode = SubsidiaryRecord.Primary_ZipCode;
                        Record.ContactName = SubsidiaryRecord.Acct_manager_Name;
                        Record.ContactAddress = SubsidiaryRecord.Acct_manager_Primary_Address;
                        Record.ContactCityName = SubsidiaryRecord.Acct_manager_Primary_City_Name;
                        Record.ContactState = SubsidiaryRecord.Acct_manager_Primary_State;
                        Record.ContactZipCode = SubsidiaryRecord.Acct_manager_Primary_Zip;
                        Record.ContactPhone = SubsidiaryRecord.Acct_manager_Primary_Phone;
                        Record.Subsidiary_Ref_Code = SubsidiaryRecord.Subsidiary_Ref_Code;
                        Record.Status = true;
                        Record.Created_Date = System.DateTime.Now;
                        dbContext.Subsidiaries.Add(Record);
                        dbContext.SaveChanges();
                        ReturnRecords.Subsidiary_ID = Record.Subsidiary_ID;
                        ReturnRecords.Successmsg = "Subsidiary Created Successfully";
                    }
                    else
                    {
                        var recordUpdate = dbContext.Subsidiaries.FirstOrDefault(cond => cond.Subsidiary_ID == SubsidiaryRecord.Subsidiary_ID);
                        if (recordUpdate != null)
                        {
                            recordUpdate.Customer_ID = SubsidiaryRecord.CustomerId;
                            recordUpdate.Email_ID = SubsidiaryRecord.Email_ID;
                            recordUpdate.Address = SubsidiaryRecord.Primary_Address;
                            recordUpdate.Phone = SubsidiaryRecord.Primary_Phone;
                            recordUpdate.CityName = SubsidiaryRecord.Primary_City_Name;
                            recordUpdate.State = SubsidiaryRecord.Primary_State;
                            recordUpdate.ZipCode = SubsidiaryRecord.Primary_ZipCode;
                            recordUpdate.ContactName = SubsidiaryRecord.Acct_manager_Name;
                            recordUpdate.ContactAddress = SubsidiaryRecord.Acct_manager_Primary_Address;
                            recordUpdate.ContactCityName = SubsidiaryRecord.Acct_manager_Primary_City_Name;
                            recordUpdate.ContactState = SubsidiaryRecord.Acct_manager_Primary_State;
                            recordUpdate.ContactZipCode = SubsidiaryRecord.Acct_manager_Primary_Zip;
                            recordUpdate.ContactPhone = SubsidiaryRecord.Acct_manager_Primary_Phone;
                            recordUpdate.Subsidiary_Ref_Code = SubsidiaryRecord.Subsidiary_Ref_Code;
                            dbContext.SaveChanges();
                        }
                        ReturnRecords.Subsidiary_ID = recordUpdate.Subsidiary_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ReturnRecords;
        }

        public Boolean CheckSubsidiaryExists(String Name, int SubstId)
        {
            var Duplicateflag = true;


            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (SubstId != 0)
                    {
                        var record = dbContext.Subsidiaries.FirstOrDefault(cond => cond.SubsidiaryName.ToLower() == Name.ToLower() && cond.Subsidiary_ID != SubstId);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }
                    else
                    {
                        var record = dbContext.Subsidiaries.FirstOrDefault(cond => cond.SubsidiaryName.ToLower() == Name.ToLower());
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

        public ManageSubsidiaryModel GetSubsidiaryDetailsById(int SubstId)
        {

            var result = new ManageSubsidiaryModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var records = (from Subsidiary x in dbContext.Subsidiaries.ToList()
                                   where x.Subsidiary_ID == Convert.ToInt16(SubstId)
                                   select new ManageSubsidiaryModel
                                   {
                                       Subsidiary_ID = x.Subsidiary_ID,
                                       CustomerId = x.Customer_ID,
                                       Subsidiary_Name = x.SubsidiaryName,
                                       Email_ID = x.Email_ID,
                                       Primary_Phone = x.Phone,
                                       Primary_State = x.State,
                                       Primary_City_Name = x.CityName,
                                       Primary_Address = x.Address,
                                       Primary_ZipCode = x.ZipCode,
                                       Acct_manager_Name = x.ContactName,
                                       Acct_manager_Primary_Address = x.ContactAddress,
                                       Acct_manager_Primary_City_Name = x.ContactCityName,
                                       Acct_manager_Primary_State = x.ContactState,
                                       Acct_manager_Primary_Zip = x.ContactZipCode,
                                       Acct_manager_Primary_Phone = x.ContactPhone,
                                       Subsidiary_Ref_Code = x.Subsidiary_Ref_Code,
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






        public Boolean CheckSubsidiaryRefExists(String Subsidiary_Ref_Code, int Subsidiary_ID, int CustomerId)
        {
            var Duplicateflag = true;


            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (Subsidiary_ID != 0)
                    {
                        var record = dbContext.Subsidiaries.FirstOrDefault(cond => cond.Subsidiary_Ref_Code.ToLower() == Subsidiary_Ref_Code.ToLower() && cond.Subsidiary_ID != Subsidiary_ID && cond.Customer_ID == CustomerId);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }
                    else
                    {
                        var record = dbContext.Subsidiaries.FirstOrDefault(cond => cond.Subsidiary_Ref_Code.ToLower() == Subsidiary_Ref_Code.ToLower() && cond.Customer_ID == CustomerId);
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
    }
}
