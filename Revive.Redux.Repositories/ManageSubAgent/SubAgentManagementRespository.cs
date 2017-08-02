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
    public class SubAgentManagementRespository : ISubAgentManagementRespository
    {
        /// <summary>
        /// Get All Subsidiary Records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManageSubAgentModel> GetSubAgentList(Guid userId, string _PageAccessCode)
        {
            try
            {
                IEnumerable<ManageSubAgentModel> ObjRoleDetails;
                using (var dbContext = new ReviveDBEntities())
                {
                    if (_PageAccessCode == PageAccessCode.ACCNTMGR)
                    {
                        ObjRoleDetails = dbContext.SubAgents.Where(x => x.AccountmanagerUserID == userId).Select(d => new ManageSubAgentModel()
                        {
                            Primary_Address = d.Address,
                            Primary_City_Name = d.CityName,
                            CustomerId = d.Customer_ID,
                            SubsidiaryId = d.Subsidiary_ID,
                            CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                            SubsidiaryName = dbContext.Subsidiaries.Where(a => a.Subsidiary_ID == d.Subsidiary_ID).Select(a => a.SubsidiaryName).FirstOrDefault(),
                            Email_ID = d.ContactEmail,
                            Status = d.Status,
                            Primary_Phone = d.Phone,
                            Primary_State = d.Primary_State,
                            SubAgent_Ref_Code=d.SubAgent_Ref_Code
                        }).ToList();

                    }
                    else if (_PageAccessCode == PageAccessCode.CUSTOMERADMIN)
                    {
                        ObjRoleDetails = dbContext.SubAgents.Join(dbContext.Users, cust => cust.Customer_ID, subs => subs.Customer_ID, (subs, cust)
                            => new { subs, cust }).Where(x => x.cust.User_ID == userId).Select(d => new ManageSubAgentModel()
                            {
                                Primary_Address = d.subs.Address,
                                SubAgent_ID = d.subs.SubAgent_ID,
                                SubAgent_Name = d.subs.SubAgentName,
                                Primary_City_Name = d.subs.CityName,
                                CustomerId = d.subs.Customer_ID,
                                SubsidiaryId = d.subs.Subsidiary_ID,
                                CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.subs.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                SubsidiaryName = dbContext.Subsidiaries.Where(a => a.Subsidiary_ID == d.subs.Subsidiary_ID).Select(a => a.SubsidiaryName).FirstOrDefault(),
                                Email_ID = d.subs.ContactEmail,
                                Status = d.subs.Status,
                                Primary_Phone = d.subs.Phone,
                                Primary_State = d.subs.ContactState,
                                SubAgent_Ref_Code = d.subs.SubAgent_Ref_Code
                            }).ToList();
                    }
                    else if (_PageAccessCode == PageAccessCode.SUBSIDIARYADMIN)
                    {
                        ObjRoleDetails = dbContext.SubAgents.Join(dbContext.Users, cust => cust.Subsidiary_ID, subs => subs.Subsidiary_ID, (subs, cust)
                            => new { subs, cust }).Where(x => x.cust.User_ID == userId).Select(d => new ManageSubAgentModel()
                            {
                                Primary_Address = d.subs.Address,
                                SubAgent_ID = d.subs.SubAgent_ID,
                                SubAgent_Name = d.subs.SubAgentName,
                                Primary_City_Name = d.subs.CityName,
                                CustomerId = d.subs.Customer_ID,
                                SubsidiaryId = d.subs.Subsidiary_ID,
                                CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.subs.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                SubsidiaryName = dbContext.Subsidiaries.Where(a => a.Subsidiary_ID == d.subs.Subsidiary_ID).Select(a => a.SubsidiaryName).FirstOrDefault(),
                                Email_ID = d.subs.ContactEmail,
                                Status = d.subs.Status,
                                Primary_Phone = d.subs.Phone,
                                Primary_State = d.subs.ContactState,
                                SubAgent_Ref_Code = d.subs.SubAgent_Ref_Code
                            }).ToList();
                    }
                    else if (_PageAccessCode == PageAccessCode.SUBAGENTADMIN)
                    {
                        ObjRoleDetails = dbContext.SubAgents.Join(dbContext.Users, cust => cust.SubAgent_ID, subs => subs.SubAgent_ID, (subs, cust)
                            => new { subs, cust }).Where(x => x.cust.User_ID == userId).Select(d => new ManageSubAgentModel()
                            {
                                Primary_Address = d.subs.Address,
                                SubAgent_ID = d.subs.SubAgent_ID,
                                SubAgent_Name = d.subs.SubAgentName,
                                Primary_City_Name = d.subs.CityName,
                                CustomerId = d.subs.Customer_ID,
                                SubsidiaryId = d.subs.Subsidiary_ID,
                                CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.subs.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                                SubsidiaryName = dbContext.Subsidiaries.Where(a => a.Subsidiary_ID == d.subs.Subsidiary_ID).Select(a => a.SubsidiaryName).FirstOrDefault(),
                                Email_ID = d.subs.ContactEmail,
                                Status = d.subs.Status,
                                Primary_Phone = d.subs.Phone,
                                Primary_State = d.subs.ContactState,
                                SubAgent_Ref_Code = d.subs.SubAgent_Ref_Code
                            }).ToList();
                    }
                    else
                    {
                        ObjRoleDetails = dbContext.SubAgents.Select(d => new ManageSubAgentModel()
                        {
                            Primary_Address = d.Address,
                            SubAgent_ID = d.SubAgent_ID,
                            SubAgent_Name = d.SubAgentName,
                            Primary_City_Name = d.CityName,
                            CustomerId = d.Customer_ID,
                            SubsidiaryId = d.Subsidiary_ID,
                            CustomerName = dbContext.Customers.Where(a => a.Customer_ID == d.Customer_ID).Select(a => a.Customer_Name).FirstOrDefault(),
                            SubsidiaryName = dbContext.Subsidiaries.Where(a => a.Subsidiary_ID == d.Subsidiary_ID).Select(a => a.SubsidiaryName).FirstOrDefault(),
                            Email_ID = d.ContactEmail,
                            Status = d.Status,
                            Primary_Phone = d.Phone,
                            Primary_State = d.ContactState,
                            SubAgent_Ref_Code = d.SubAgent_Ref_Code
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

        public ManageSubAgentModel CreateSubAgent(ManageSubAgentModel AgentRecord)
        {
            ManageSubAgentModel ReturnRecords = new ManageSubAgentModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (AgentRecord.SubAgent_ID == 0)
                    {
                        var Record = new SubAgent();
                        Record.Customer_ID = AgentRecord.CustomerId;
                        Record.Subsidiary_ID = AgentRecord.SubsidiaryId;
                        Record.ContactEmail = AgentRecord.Email_ID;
                        Record.SubAgentName = AgentRecord.SubAgent_Name;
                        Record.ShipToAttention = AgentRecord.ShiptoAttention;
                        Record.Address = AgentRecord.Primary_Address;
                        Record.Phone = AgentRecord.Primary_Phone;
                        Record.CityName = AgentRecord.Primary_City_Name;
                        Record.Primary_State = AgentRecord.Primary_State;
                        Record.ZipCode = AgentRecord.Primary_ZipCode;
                        Record.ContactName = AgentRecord.Acct_manager_Name;
                        Record.ContactAddress = AgentRecord.Acct_manager_Primary_Address;
                        Record.ContactCityName = AgentRecord.Acct_manager_Primary_City_Name;
                        Record.ContactState = AgentRecord.Acct_manager_Primary_State;
                        Record.ContactZipCode = AgentRecord.Acct_manager_Primary_Zip;
                        Record.ContactPhone = AgentRecord.Acct_manager_Primary_Phone;
                        Record.SubAgentTypeId = AgentRecord.UserTypeID;
                        Record.AccountmanagerUserID = AgentRecord.AccountmanagerUserID;
                        Record.Lease_Start_Date = AgentRecord.Lease_Start_Date;
                        Record.Lease_end_date = AgentRecord.Lease_end_date;
                        Record.Status = true;
                        Record.Created_Date = System.DateTime.Now;
                        Record.SubAgent_Ref_Code = AgentRecord.SubAgent_Ref_Code;
                        dbContext.SubAgents.Add(Record);
                        dbContext.SaveChanges();
                        ReturnRecords.SubAgent_ID = Record.SubAgent_ID;
                        ReturnRecords.Successmsg = "Sub Agent Created Successfully";
                    }
                    else
                    {
                        var recordUpdate = dbContext.SubAgents.FirstOrDefault(cond => cond.SubAgent_ID == AgentRecord.SubAgent_ID);
                        if (recordUpdate != null)
                        {
                            recordUpdate.Customer_ID = AgentRecord.CustomerId;
                            recordUpdate.ContactEmail = AgentRecord.Email_ID;
                            recordUpdate.Address = AgentRecord.Primary_Address;
                            recordUpdate.Phone = AgentRecord.Primary_Phone;
                            recordUpdate.SubAgentName = AgentRecord.SubAgent_Name;
                            recordUpdate.Subsidiary_ID = AgentRecord.SubsidiaryId;
                            recordUpdate.SubAgentTypeId = AgentRecord.UserTypeID;
                            recordUpdate.AccountmanagerUserID = AgentRecord.AccountmanagerUserID;
                            recordUpdate.Lease_Start_Date = AgentRecord.Lease_Start_Date;
                            recordUpdate.Lease_end_date = AgentRecord.Lease_end_date;
                            recordUpdate.ShipToAttention = AgentRecord.ShiptoAttention;
                            recordUpdate.CityName = AgentRecord.Primary_City_Name;
                            recordUpdate.Primary_State = AgentRecord.Primary_State;
                            recordUpdate.ZipCode = AgentRecord.Primary_ZipCode;
                            recordUpdate.ContactName = AgentRecord.Acct_manager_Name;
                            recordUpdate.ContactAddress = AgentRecord.Acct_manager_Primary_Address;
                            recordUpdate.ContactCityName = AgentRecord.Acct_manager_Primary_City_Name;
                            recordUpdate.ContactState = AgentRecord.Acct_manager_Primary_State;
                            recordUpdate.ContactZipCode = AgentRecord.Acct_manager_Primary_Zip;
                            recordUpdate.ContactPhone = AgentRecord.Acct_manager_Primary_Phone;
                            recordUpdate.SubAgent_Ref_Code = AgentRecord.SubAgent_Ref_Code;
                            dbContext.SaveChanges();
                        }
                        ReturnRecords.SubAgent_ID = recordUpdate.SubAgent_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ReturnRecords;
        }

        public Boolean CheckSubAgentExists(String Name, int AgentID)
        {
            var Duplicateflag = true;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (AgentID != 0)
                    {
                        var record = dbContext.SubAgents.FirstOrDefault(cond => cond.SubAgentName.ToLower() == Name.ToLower() && cond.SubAgent_ID != AgentID);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }
                    else
                    {
                        var record = dbContext.SubAgents.FirstOrDefault(cond => cond.SubAgentName.ToLower() == Name.ToLower());
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

        public ManageSubAgentModel GetSubAgentDetailsById(int SubstId)
        {

            var result = new ManageSubAgentModel();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var records = (from SubAgent x in dbContext.SubAgents.ToList()
                                   where x.SubAgent_ID == Convert.ToInt16(SubstId)
                                   select new ManageSubAgentModel
                                   {
                                       SubAgent_ID = x.SubAgent_ID,
                                       CustomerId = x.Customer_ID,
                                       SubsidiaryId = x.Subsidiary_ID,
                                       Email_ID = x.ContactEmail,
                                       SubAgent_Name = x.SubAgentName,
                                       Primary_Address = x.Address,
                                       Primary_Phone = x.Phone,
                                       Primary_City_Name = x.CityName,
                                       Primary_State = x.Primary_State,
                                       Primary_ZipCode = x.ZipCode,
                                       ShiptoAttention=x.ShipToAttention,
                                       Acct_manager_Name = x.ContactName,
                                       Acct_manager_Primary_Address = x.ContactAddress,
                                       Acct_manager_Primary_City_Name = x.ContactCityName,
                                       Acct_manager_Primary_State = x.ContactState,
                                       Acct_manager_Primary_Zip = x.ContactZipCode,
                                       Acct_manager_Primary_Phone = x.ContactPhone,
                                       UserTypeID = x.SubAgentTypeId,
                                       AccountmanagerUserID = x.AccountmanagerUserID,
                                       Lease_Start_Date = x.Lease_Start_Date,
                                       Lease_end_date = x.Lease_end_date,
                                       SubAgent_Ref_Code = x.SubAgent_Ref_Code,
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

       

        

        public Boolean CheckSubAgentRefExists(String SubAgent_Ref_Code, int AgentID, int CustomerId)
        {
            var Duplicateflag = true;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (AgentID != 0)
                    {
                        var record = dbContext.SubAgents.FirstOrDefault(cond => cond.SubAgent_Ref_Code.ToLower() == SubAgent_Ref_Code.ToLower() && cond.SubAgent_ID != AgentID && cond.Customer_ID == CustomerId);
                        if (record != null)
                        {
                            Duplicateflag = false;
                        }
                    }
                    else
                    {
                        var record = dbContext.SubAgents.FirstOrDefault(cond => cond.SubAgent_Ref_Code.ToLower() == SubAgent_Ref_Code.ToLower() && cond.Customer_ID == CustomerId);
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
