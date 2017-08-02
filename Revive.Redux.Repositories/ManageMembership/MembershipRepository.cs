using System;
using System.Collections.Generic;
using System.Linq;
using Revive.Redux.Repositories;
using Revive.Redux;
using Revive.Redux.Entities;
using System.Transactions;
using Revive.Redux.Entities.Common;
using Revive.Redux.Commn;
using Revive.Redux.Common;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


namespace Revive.Redux.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        public RepositoryResult AddEditMembership(MembersShipModel objmembership)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();
            try
            {
                bool isNewUser = false;
                var membership = new Membership();
                using (var dbContext = new ReviveDBEntities())
                {
                    if (objmembership.membership_Id == null)
                    {
                        isNewUser = true;
                    }
                    if (isNewUser == true)
                    {
                        objmembership.Created_Date = System.DateTime.Now;
                    }
                    else
                    {
                        membership = dbContext.Memberships.FirstOrDefault(memb => memb.Membership_ID == objmembership.membership_Id);
                        membership.Modified_Date = System.DateTime.Now;
                        membership.Modified_by = objmembership.Modified_by;
                        dbContext.SaveChanges();
                    }


                    membership = Map_MembershipModel_To_Membership(objmembership, membership);

                    if (isNewUser) // in case of new user
                    {
                        var getProcCall = dbContext.usp_RegisterMembership(objmembership.MemberName, objmembership.Email_Id, objmembership.Phone_Number, objmembership.DOB, objmembership.user_Id, objmembership.reviveDevice_Id, objmembership.machine_Id, objmembership.invoice_No, objmembership.IsMultiDevice).FirstOrDefault();
                        //objReturnStatus.MemberShipId = getProcCall.Select(d => d.MemberShipId).FirstOrDefault();
                        if (getProcCall.StatusId == 1)
                        {
                            objresult.resultIdval = getProcCall.MemberShipId;
                            objresult.resultVal = "";
                            objresult.resultId = APIStatusValues.SuccessId;

                        }
                        else
                        {
                            objresult.resultId = APIStatusValues.FailureIdmsgs;
                            objresult.resultVal = "Error in Inserting Membership";
                        }

                    }

                }

                // objReturnStatus.StatusId = 1;
                // objReturnStatus.StatusMessages = "Success";
                //objReturnStatus.MemberShipId = membership.Membership_ID;



            }
            catch (Exception ex)
            {
                // objresult.resultVal = ex.Message.ToString();
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = ex.Message.ToString();

            }

            return objresult;

        }
        public RepositoryResult UploadMember(MembershipWebModel objmembership)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();
            try
            {
                bool isNewUser = false;
                var membership = new Membership();
                using (var dbContext = new ReviveDBEntities())
                {
                    isNewUser = true;

                    if (isNewUser) // in case of new user
                    {
                        var getProcCall = dbContext.usp_LegacyMemberInsert(objmembership.MemberName, objmembership.Email_ID, objmembership.Phone_Number, objmembership.user_Id, "", 0, objmembership.InvoiceNumber, true, objmembership.StartDate, objmembership.EndDate, objmembership.no_of_FreeDries, objmembership.CustomerId, objmembership.SubsidiaryId, objmembership.SubAgentId, objmembership.LocationId, objmembership.IsMultiDevice).FirstOrDefault();

                        if (getProcCall.StatusId == 1)
                        {
                            objresult.resultIdval = getProcCall.MemberShipId;
                            objresult.resultVal = "";
                            objresult.resultId = APIStatusValues.SuccessId;

                        }
                        else
                        {
                            objresult.resultId = APIStatusValues.FailureIdmsgs;
                            objresult.resultVal = "Error in Inserting Membership";
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                // objresult.resultVal = ex.Message.ToString();
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = ex.Message.ToString();

            }

            return objresult;

        }
        private Membership Map_MembershipModel_To_Membership(MembersShipModel membershipModel, Membership objMember)
        {

            objMember.MemberName = membershipModel.MemberName;
            objMember.Email_ID = membershipModel.Email_Id;
            objMember.Phone_Number = membershipModel.Phone_Number;
            objMember.DOB = membershipModel.DOB;
            objMember.IsMultiDevice = membershipModel.IsMultiDevice;

            return objMember;

        }
        public Boolean CheckEmailAddressExists(String emailAddress, string membershipId, Guid user_Id)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var objuser = dbContext.Users.FirstOrDefault(cond => cond.User_ID == user_Id);

                    if ((membershipId == "" || membershipId == null) && objuser != null)
                    {
                        var user = dbContext.Memberships.FirstOrDefault(cond => cond.Email_ID == emailAddress && cond.CustomerId == objuser.Customer_ID);
                        if (user != null)
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        var user = dbContext.Memberships.FirstOrDefault(cond => cond.Email_ID == emailAddress && cond.Membership_ID != membershipId && cond.CustomerId == objuser.Customer_ID);
                        if (user != null)
                        {
                            flag = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public Boolean CheckDuplicateMailByCustomerID(string emailAddress, int CustomerId)
        {
            var flag = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var member = dbContext.Memberships.FirstOrDefault(cond => cond.Email_ID == emailAddress && cond.CustomerId == CustomerId);
                    if (member != null)
                    {
                        flag = true;
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }
        /// <summary>
        /// Search Member API for tablet APP 
        /// </summary>
        /// <param name="objMembership"></param>
        /// <returns></returns>
        public IEnumerable<MemberShipDetailsModel> GetMembmershipDetails(MembersShipModel objMembership)
        {
            IEnumerable<MemberShipDetailsModel> objMembshipDetails;

            using (var dbContext = new ReviveDBEntities())
            {

                var objuser = dbContext.Users.FirstOrDefault(cond => cond.User_ID == objMembership.user_Id);
                objMembshipDetails = dbContext.usp_GetMembershipDetails(objMembership.membership_Id, objMembership.MemberName, objMembership.Email_Id, objMembership.Phone_Number, objuser.Customer_ID, objMembership.invoice_No, objMembership.fmSlno, objMembership.toSlno).Select(d => new MemberShipDetailsModel()
                {
                    MemberName = d.MemberName,
                    Phone_Number = d.Phone_Number,
                    Email_ID = d.Email_ID,
                    Membership_ID = d.Membership_ID,
                    Membership_Start_date = d.Membership_Start_date,
                    Membership_End_date = d.Membership_End_date,
                    Membership_Activation_date = d.Membership_Activation_date,
                    No_of_Free_dries = d.No_of_Free_dries,
                    StoreCreated = d.StoreCreated,
                    Store_ID = d.Store_ID,
                    MembershipStatus = d.MembershipStatus,
                    CommonText = d.CommonText,
                    Renew_Start_date = d.Renew_Start_date,
                    Renew_End_date = d.Renew_End_date,
                    Renew_Activation_date = d.Renew_Activation_date,
                    Renew_No_of_Free_dries = d.Renew_No_of_Free_dries,
                    createdBy = d.Created_by,
                    member_LastReviveDate = d.Member_LastReviveDate,
                    member_LastLocation = d.Member_LastLocation,
                    member_LastStore = d.Member_LastStore,
                    member_LastUserName = d.Member_LastUserName,
                    IsLegacy = (bool)d.isLegacy,
                    DOB = d.DOB,
                    IsActive = d.ActiveStatus == "Active" ? true : false
                }).ToList();



            }


            return objMembshipDetails;

        }
        public bool updateMembership(MembersShipModel objMembership)
        {


            var result = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (objMembership.membership_Id != null && objMembership.membership_Id != "")
                    {
                        var membership = dbContext.Memberships.FirstOrDefault(member => member.Membership_ID == objMembership.membership_Id);
                        if (membership != null)
                        {
                            membership.MemberName = objMembership.MemberName;
                            membership.Phone_Number = objMembership.Phone_Number;
                            membership.Email_ID = objMembership.Email_Id;// As per discussion Tab APP team , it should editable  dated : 29-05-2015

                            membership.Modified_Date = System.DateTime.Now;

                            if (objMembership.IsLegacy)
                            {
                                membership.DOB = objMembership.DOB;
                            }
                            membership.IsLegacy = false;
                        }
                        dbContext.SaveChanges();
                        result = true;
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public RepositoryResult renewMembership(MembersShipModel objMembership)
        {
            RepositoryResult objretResult = new RepositoryResult();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    var result1 = (from p in dbContext.usp_UpdateMembershipRenew(objMembership.membership_Id, objMembership.user_Id, objMembership.invoice_No)
                                   select new
                                   {
                                       StatusId = p.StatusId,
                                       StatusMessages = p.StatusMessages
                                   }).ToList();

                    foreach (var item in result1)
                    {
                        objretResult.resultId = item.StatusId;
                        objretResult.resultVal = item.StatusMessages;
                    }





                }
            }
            catch (Exception ex)
            {
                objretResult.resultId = APIStatusValues.FailureIdmsgs;
                objretResult.resultVal = ex.Message.ToString();

                // result = false;
            }
            return objretResult;
        }
        /// <summary>
        /// Start Revive Revive Checkin for memmbership
        /// </summary>
        /// <param name="objMembershipActivity"></param>
        /// <returns></returns>
        public RepositoryResult InitiateCheckinProcess(MemberActivityModel objMembershipActivity)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();

            var membership = new Membership();
            using (var dbContext = new ReviveDBEntities())
            {
                Tablet_App_Machine_Activity_Details objMachineNewActivity = new Tablet_App_Machine_Activity_Details();
                Tablet_App_MemberActivity_Details objMembernewActivity = new Tablet_App_MemberActivity_Details();
                Tablet_App_NonMemberActivity_Details objNonMemberNewActivity = new Tablet_App_NonMemberActivity_Details();
                int MachineLocationId = 0;
                try
                {
                    // Initiate CheckIn  
                    // Map to Tablet_App_Machine_Activity_Details
                    MachineLocationId = dbContext.LocationMapMachines.Where(x => x.Machine_Id == objMembershipActivity.machine_Id).Select(x => x.Location_id).FirstOrDefault();
                    objMembershipActivity.LocationId = MachineLocationId;
                    objMembershipActivity.machineActivity_id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveInitiateCheckin).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objMachineNewActivity = MapFieldsToMachineActivity(objMachineNewActivity, objMembershipActivity);
                    dbContext.Tablet_App_Machine_Activity_Details.Add(objMachineNewActivity);

                    // Map toTablet_App_MemberActivity_Details
                    if (objMembershipActivity.isMember == true)
                    {
                        objMembernewActivity = MapFieldsToMemberActivity(objMembernewActivity, objMembershipActivity);
                        objMembernewActivity.Machine_Activity_ID = objMachineNewActivity.Machine_Activity_ID;
                        dbContext.Tablet_App_MemberActivity_Details.Add(objMembernewActivity);
                    }
                    else
                    {
                        objNonMemberNewActivity = MapFieldsToNonMemberActivity(objNonMemberNewActivity, objMembershipActivity);
                        objNonMemberNewActivity.Machine_Activity_ID = objMachineNewActivity.Machine_Activity_ID;
                        dbContext.Tablet_App_NonMemberActivity_Details.Add(objNonMemberNewActivity);
                    }

                    dbContext.SaveChanges();
                    objresult.resultId = APIStatusValues.SuccessId;
                    objresult.resultVal = "Revive Started.";
                }
                catch (Exception ex)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    //  objresult.resultVal = "Error in Check in process data insertion." + ex.Message.ToString();
                    objresult.resultVal = "System is not able to update check in data. Please try again";
                }

            }
            return objresult;
        }
        /// <summary>
        /// This API will be called by Tablet team when machine will not able to Start
        /// </summary>
        /// <param name="objMembershipActivity"></param>
        /// <returns></returns>

        /// <summary>
        /// UpdateReviveStartResult
        /// </summary>
        /// <param name="objMembershipActivity"></param>
        /// <returns></returns>

        public RepositoryResult UpdateReviveStartResult(MemberActivityModel objMembershipActivity)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();

            var membership = new Membership();
            using (var dbContext = new ReviveDBEntities())
            {
                Tablet_App_Machine_Activity_Results objNewReviveResults = new Tablet_App_Machine_Activity_Results();
                Tablet_App_Machine_Activity_Details objMachineNewActivity = new Tablet_App_Machine_Activity_Details();
                try
                {
                    var MachineLastActivity = dbContext.Tablet_App_Machine_Activity_Details.OrderByDescending(x => x.Machine_Activity_ID).Where(z => z.Machine_Id == objMembershipActivity.machine_Id && z.Process_Id != null).FirstOrDefault();
                    // Insert into MachineActivity table
                    objMembershipActivity.machineActivity_id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveStartResult).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objMachineNewActivity = MapFieldsToMachineActivity(objMachineNewActivity, objMembershipActivity);
                    if (MachineLastActivity != null)
                    {
                        objMachineNewActivity.Process_Id = MachineLastActivity.Process_Id;
                    }
                    else
                    {
                        objMachineNewActivity.Process_Id = objMembershipActivity.process_Id;
                    }



                    dbContext.Tablet_App_Machine_Activity_Details.Add(objMachineNewActivity);


                    // Revive Results
                    objNewReviveResults = MapFieldsToMachineActivityResults(objNewReviveResults, objMembershipActivity);


                    if (objMembershipActivity.activity_Results == TabletMachineActivityStages.ReviveSuccessToStart)
                    {
                        objNewReviveResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveSuccessToStart).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                        objNewReviveResults.Activity_Results = "Revive started successfully";
                    }
                    else
                    {
                        objNewReviveResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveFailedToStart).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                        objNewReviveResults.Activity_Results = "Revive unable to start";
                    }

                    objNewReviveResults.Machine_Activity_ID = objMachineNewActivity.Machine_Activity_ID;
                    dbContext.Tablet_App_Machine_Activity_Results.Add(objNewReviveResults);
                    dbContext.SaveChanges();
                    objresult.resultId = APIStatusValues.SuccessId;
                    objresult.resultVal = "Revive Start Result saved successfully.";
                }
                catch (Exception ex)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Error in Saving Revive Start Result." + ex.Message.ToString();
                }

            }
            return objresult;
        }

        public RepositoryResult UpdateReviveEndResult(MemberActivityModel objMembershipActivity)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();

            var membership = new Membership();
            using (var dbContext = new ReviveDBEntities())
            {
                Tablet_App_Machine_Activity_Results objNewReviveResults = new Tablet_App_Machine_Activity_Results();
                Tablet_App_Machine_Activity_Details objMachineNewActivity = new Tablet_App_Machine_Activity_Details();
                try
                {
                    var MachineLastActivity = dbContext.Tablet_App_Machine_Activity_Details.OrderByDescending(x => x.Machine_Activity_ID).Where(z => z.Machine_Id == objMembershipActivity.machine_Id && z.Process_Id != null).FirstOrDefault();
                    // Insert into MachineActivity table
                    objMembershipActivity.machineActivity_id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveEndResult).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objMachineNewActivity = MapFieldsToMachineActivity(objMachineNewActivity, objMembershipActivity);
                    if (MachineLastActivity != null)
                    {
                        objMachineNewActivity.Process_Id = MachineLastActivity.Process_Id;
                        objMachineNewActivity.ModeId = MachineLastActivity.ModeId;
                    }
                    else
                    {
                        objMachineNewActivity.Process_Id = objMembershipActivity.process_Id;
                        objMachineNewActivity.ModeId = objMembershipActivity.modeId;
                    }
                    dbContext.Tablet_App_Machine_Activity_Details.Add(objMachineNewActivity);


                    // Revive Results
                    objNewReviveResults = MapFieldsToMachineActivityResults(objNewReviveResults, objMembershipActivity);
                    objNewReviveResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == objMembershipActivity.resultCategoryCode).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objNewReviveResults.Machine_Activity_ID = objMachineNewActivity.Machine_Activity_ID;
                    dbContext.Tablet_App_Machine_Activity_Results.Add(objNewReviveResults);
                    dbContext.SaveChanges();
                    objresult.resultId = APIStatusValues.SuccessId;
                    objresult.resultVal = "Revive End Result saved successfully.";
                }
                catch (Exception ex)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Error in Saving Revive End Result." + ex.Message.ToString();
                }

            }
            return objresult;
        }
        public RepositoryResult SendDebugInformation(MemberActivityModel objMembershipActivity)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();

            var membership = new Membership();
            using (var dbContext = new ReviveDBEntities())
            {
                Tablet_App_Machine_Activity_Results objNewReviveResults = new Tablet_App_Machine_Activity_Results();
                Tablet_App_Machine_Activity_Details objMachineNewActivity = new Tablet_App_Machine_Activity_Details();
                try
                {
                    var MachineLastActivity = dbContext.Tablet_App_Machine_Activity_Details.OrderByDescending(x => x.Machine_Activity_ID).Where(z => z.Machine_Id == objMembershipActivity.machine_Id && z.Process_Id != null).FirstOrDefault();
                    // Insert into MachineActivity table
                    objMembershipActivity.machineActivity_id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveDebugParams).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objMachineNewActivity = MapFieldsToMachineActivity(objMachineNewActivity, objMembershipActivity);
                    if (MachineLastActivity != null)
                    {
                        objMachineNewActivity.Process_Id = MachineLastActivity.Process_Id;
                    }
                    else
                    {
                        objMachineNewActivity.Process_Id = objMembershipActivity.process_Id;
                    }
                    dbContext.Tablet_App_Machine_Activity_Details.Add(objMachineNewActivity);
                    // Revive Results
                    objNewReviveResults = MapFieldsToMachineActivityResults(objNewReviveResults, objMembershipActivity);
                    objNewReviveResults.Category_Id = objMembershipActivity.machineActivity_id;
                    objNewReviveResults.Machine_Activity_ID = objMachineNewActivity.Machine_Activity_ID;

                    if (objMembershipActivity.modeId == Modes.Debug)
                    {
                        objNewReviveResults.Activity_Results = MapDebugValues((int)objMembershipActivity.machine_Id, objMembershipActivity.activity_Results);
                    }
                    else
                    {
                        objNewReviveResults.Activity_Results = objMembershipActivity.activity_Results;  // Default calling 
                    }


                    //objNewReviveResults.Revive_Answer = objMembershipActivity.activity_Results;
                    dbContext.Tablet_App_Machine_Activity_Results.Add(objNewReviveResults);
                    dbContext.SaveChanges();
                    objresult.resultId = APIStatusValues.SuccessId;
                    objresult.resultVal = "Debug Information saved successfully.";
                }
                catch (Exception ex)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Error in Debug information." + ex.Message.ToString();
                }

            }
            return objresult;
        }

        #region Validate Revive Stoped or Not
        public bool IsReviveStopped(MemberActivityModel objMembershipActivity, string ActivityTypeCode)
        {
            bool result = false;

            using (var dbContext = new ReviveDBEntities())
            {
                var ActivityId = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == ActivityTypeCode).Select(a => a.MasterData_Type_ID).FirstOrDefault();

                var MachineActivity_RecordCount = dbContext.Tablet_App_Machine_Activity_Details.Where(z => z.Machine_Id == objMembershipActivity.machine_Id && z.Process_Id == objMembershipActivity.process_Id && z.Activity_type_id == ActivityId).ToList().Count();

                if (MachineActivity_RecordCount > 0)
                {
                    result = true;
                }
            }
            return result;


        }
        #endregion


        public RepositoryResult ReviveCheckOut(MemberActivityModel objMembershipActivity)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();

            var membership = new Membership();
            using (var dbContext = new ReviveDBEntities())
            {

                Tablet_App_Machine_Activity_Details objMachineNewActivity = new Tablet_App_Machine_Activity_Details();
                Tablet_App_Machine_Activity_Results objNewReviveResults = new Tablet_App_Machine_Activity_Results();

                try
                {

                    var MachineLastActivity = dbContext.Tablet_App_Machine_Activity_Details.OrderByDescending(x => x.Machine_Activity_ID).Where(z => z.Machine_Id == objMembershipActivity.machine_Id && z.Process_Id != null).FirstOrDefault();
                    // Insert into MachineActivity table
                    objMembershipActivity.machineActivity_id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveCheckOut).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objMachineNewActivity = MapFieldsToMachineActivity(objMachineNewActivity, objMembershipActivity);
                    if (MachineLastActivity != null)
                    {
                        objMachineNewActivity.Process_Id = MachineLastActivity.Process_Id;
                        objMachineNewActivity.ModeId = MachineLastActivity.ModeId;
                    }
                    else
                    {
                        objMachineNewActivity.Process_Id = objMembershipActivity.process_Id;
                        objMachineNewActivity.ModeId = objMembershipActivity.modeId;
                    }
                    dbContext.Tablet_App_Machine_Activity_Details.Add(objMachineNewActivity);


                    // Revive Results
                    objNewReviveResults = MapFieldsToMachineActivityResults(objNewReviveResults, objMembershipActivity);
                    objNewReviveResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == objMembershipActivity.resultCategoryCode).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objNewReviveResults.Machine_Activity_ID = objMachineNewActivity.Machine_Activity_ID;
                    dbContext.Tablet_App_Machine_Activity_Results.Add(objNewReviveResults);

                    // Updating no_of_free_dries in Membership Details table
                    var membershipDetails = new Memebership_Details();
                    int valid_membershipDetail_Id = dbContext.Memebership_Details.Where(a => a.Membership_Id == objMembershipActivity.membership_Id && a.Membership_Start_date <= System.DateTime.Now).OrderByDescending(b => b.Membership_Det_ID).Select(a => a.Membership_Det_ID).FirstOrDefault();
                    if (valid_membershipDetail_Id > 0)
                    {
                        membershipDetails = dbContext.Memebership_Details.FirstOrDefault(c => c.Membership_Det_ID == valid_membershipDetail_Id);
                        if (membershipDetails != null && membershipDetails.No_of_Free_dries > 0)
                        {
                            membershipDetails.No_of_Free_dries = membershipDetails.No_of_Free_dries - 1; // Revive Stop  , substracting value 1 from no_of_free_dries
                        }

                    }
                    // Updating no_of_free_dries in Membership Details table
                    dbContext.SaveChanges();
                    objresult.resultId = APIStatusValues.SuccessId;
                    objresult.resultVal = "Revive Stoped.";
                }
                catch (Exception ex)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Error in Check Out process data insertion." + ex.Message.ToString();
                }

            }
            return objresult;
        }
        public RepositoryResult MachineFault(MemberActivityModel objMemberActivityModel)
        {
            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();

            var membership = new Membership();
            using (var dbContext = new ReviveDBEntities())
            {
                Tablet_App_Machine_Activity_Details objNewMachineActivity = new Tablet_App_Machine_Activity_Details();
                Tablet_App_Machine_Activity_Results objNewMachineResults;


                try
                {
                    var MachineLastActivity = dbContext.Tablet_App_Machine_Activity_Details.OrderByDescending(x => x.Machine_Activity_ID).Where(z => z.Machine_Id == objMemberActivityModel.machine_Id && z.Process_Id != null).FirstOrDefault(); // Last processId By machine Id
                    // Taking last process Id by Machine Id  , Will taken only in case of Revive Process else process Id will be update in DB received from table App.

                    objMemberActivityModel.machineActivity_id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveMachineFault).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objNewMachineActivity = MapFieldsToMachineActivity(objNewMachineActivity, objMemberActivityModel);
                    objNewMachineActivity.Process_Id = MachineLastActivity.Process_Id;
                    if (objMemberActivityModel.process_Id != null && objMemberActivityModel.modeId == Modes.Debug)
                    {
                        objNewMachineActivity.Process_Id = objMemberActivityModel.process_Id;
                    }
                    dbContext.Tablet_App_Machine_Activity_Details.Add(objNewMachineActivity);


                    // Process Faults
                    if (objMemberActivityModel.machineFault.processFaults != null)
                    {
                        objNewMachineResults = new Tablet_App_Machine_Activity_Results();
                        objNewMachineResults.Revive_Question = "Machine Fault";
                        objNewMachineResults = MapFieldsToMachineActivityResults(objNewMachineResults, objMemberActivityModel);
                        objNewMachineResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveProcessfaults).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                        objNewMachineResults.Machine_Activity_ID = objNewMachineActivity.Machine_Activity_ID;
                        objNewMachineResults.Activity_Results = GetValueByfaultCategoryId((int)objNewMachineResults.Category_Id, objMemberActivityModel.machineFault.processFaults);


                        dbContext.Tablet_App_Machine_Activity_Results.Add(objNewMachineResults);

                    }
                    // Equipment Faults
                    if (objMemberActivityModel.machineFault.equipmentFaults != null)
                    {
                        objNewMachineResults = new Tablet_App_Machine_Activity_Results();
                        objNewMachineResults.Revive_Question = "Machine Fault";

                        objNewMachineResults = MapFieldsToMachineActivityResults(objNewMachineResults, objMemberActivityModel);
                        objNewMachineResults.Machine_Activity_ID = objNewMachineActivity.Machine_Activity_ID;
                        objNewMachineResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveEquipmentfaults).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                        objNewMachineResults.Activity_Results = GetValueByfaultCategoryId((int)objNewMachineResults.Category_Id, objMemberActivityModel.machineFault.equipmentFaults);
                        dbContext.Tablet_App_Machine_Activity_Results.Add(objNewMachineResults);


                    }
                    // Board Faults

                    if (objMemberActivityModel.machineFault.boardFaults != null)
                    {
                        objNewMachineResults = new Tablet_App_Machine_Activity_Results();
                        objNewMachineResults.Revive_Question = "Machine Fault";
                        objNewMachineResults = MapFieldsToMachineActivityResults(objNewMachineResults, objMemberActivityModel);
                        objNewMachineResults.Machine_Activity_ID = objNewMachineActivity.Machine_Activity_ID;
                        objNewMachineResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveBoardfaults).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                        objNewMachineResults.Activity_Results = GetValueByfaultCategoryId((int)objNewMachineResults.Category_Id, objMemberActivityModel.machineFault.boardFaults);
                        dbContext.Tablet_App_Machine_Activity_Results.Add(objNewMachineResults);

                        // dbContext.SaveChanges();
                    }



                    dbContext.SaveChanges();
                    objresult.resultId = APIStatusValues.SuccessId;
                    objresult.resultVal = "Machine fault error saved successfully.";
                }
                catch (Exception ex)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Error in Machine fault Data saving." + ex.Message.ToString();
                }

            }
            return objresult;
        }
        public RepositoryResult updateSoftwareVersion(MemberActivityModel objMemberActivityModel)
        {

            RepositoryResult objresult = new RepositoryResult();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (objMemberActivityModel.machine_Id != null && objMemberActivityModel.machine_Id > 0)
                    {
                        var machineQry = dbContext.Machines.FirstOrDefault(c => c.Machine_Id == objMemberActivityModel.machine_Id);
                        var OldversionId = dbContext.MasterData_Config_definitions.FirstOrDefault(c => c.MasterData_Type_ID == machineQry.Proposedversion_id);



                        if (machineQry != null && machineQry.Proposedversion_id != null && (Convert.ToString(OldversionId.MasterData_Value).Trim().ToUpper() == Convert.ToString(objMemberActivityModel.firmwareID).Trim().ToUpper()))
                        {
                            machineQry.Softwareversion_Id = machineQry.Proposedversion_id;
                            machineQry.Modified_Date = DateTime.Now;
                            machineQry.Proposedversion_id = null;
                            SaveMoveHistoryData(dbContext, objMemberActivityModel); // Firmware update details inserting into move history.
                            dbContext.SaveChanges();
                            objresult.resultId = APIStatusValues.SuccessId;
                            objresult.resultVal = "Software version updated successfully.";
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = "Error in Software version updating." + ex.Message.ToString();
            }
            return objresult;
        }
        private void SaveMoveHistoryData(ReviveDBEntities dbContext, MemberActivityModel model)
        {

            var objMachine = dbContext.usp_GetMachineHistoryCustomerDetailsByMachineId(model.machine_Id).FirstOrDefault();


            MachineHistory MachineNewHistory = new MachineHistory();
            MachineNewHistory.Machine_ID = model.machine_Id;
            MachineNewHistory.ReasonTypeId = 0;
            MachineNewHistory.StatusTypeId = 0;
            MachineNewHistory.Notes = "Firmware updated on Dated " + DateTime.Now.ToLongDateString();
            MachineNewHistory.EventDate = DateTime.Now;
            MachineNewHistory.TransactoionTypeId = 0;
            MachineNewHistory.Customer_Id = objMachine.CustomerId;
            MachineNewHistory.Subsidiary_Id = objMachine.Subsidiary_ID;
            MachineNewHistory.SubAgent_Id = objMachine.SubAgent_ID;
            MachineNewHistory.Location_Id = objMachine.LocationId;
            dbContext.MachineHistories.Add(MachineNewHistory);


        }

        #region Map fields for App Machine Activity & Member activity
        private Tablet_App_Machine_Activity_Details MapFieldsToMachineActivity(Tablet_App_Machine_Activity_Details objMachineActivity, MemberActivityModel objModelMemberActivity)
        {

            objMachineActivity.Activity_Date = System.DateTime.Now;
            objMachineActivity.Activity_type_id = objModelMemberActivity.machineActivity_id;
            objMachineActivity.Machine_Id = objModelMemberActivity.machine_Id;
            objMachineActivity.Process_Id = objModelMemberActivity.process_Id;
            objMachineActivity.ModeId = objModelMemberActivity.modeId;
            objMachineActivity.Created_by = objModelMemberActivity.created_by;
            objMachineActivity.Created_Date = System.DateTime.Now;


            return objMachineActivity;

        }
        private string GetInvoiceNoByMembershipId(string memberShipId)
        {
            string InvoiceNo = "";
            using (var dbContext = new ReviveDBEntities())
            {
                var queryInvoice = dbContext.Memebership_Details.Where(a => a.Membership_Id == memberShipId && a.Membership_Start_date <= System.DateTime.Now).OrderByDescending(u => u.Membership_Det_ID).FirstOrDefault();
                if (queryInvoice != null)
                {
                    InvoiceNo = queryInvoice.Invoice_No;
                }
            }
            return InvoiceNo;
        }
        private Tablet_App_MemberActivity_Details MapFieldsToMemberActivity(Tablet_App_MemberActivity_Details objMemberActivity, MemberActivityModel objModelMemberActivity)
        {
            objMemberActivity.Membership_Id = objModelMemberActivity.membership_Id;
            objMemberActivity.Devicemanufacturer_Id = objModelMemberActivity.devicemanufacturer_Id;
            objMemberActivity.Howlongago_Id = objModelMemberActivity.howlongago_Id;
            objMemberActivity.pluggedinYes = objModelMemberActivity.pluggedinYes;
            objMemberActivity.Device_id = objModelMemberActivity.device_id;
            objMemberActivity.Created_by = objModelMemberActivity.created_by;
            objMemberActivity.Created_Date = System.DateTime.Now;
            objMemberActivity.Invoice_No = GetInvoiceNoByMembershipId(objModelMemberActivity.membership_Id); // invoice No Updating
            objMemberActivity.Location_id = objModelMemberActivity.LocationId;
            objMemberActivity.phonenumber = objModelMemberActivity.phonenumber;
            return objMemberActivity;
        }
        private Tablet_App_NonMemberActivity_Details MapFieldsToNonMemberActivity(Tablet_App_NonMemberActivity_Details objMemberActivity, MemberActivityModel objModelMemberActivity)
        {

            objMemberActivity.Devicemanufacturer_Id = objModelMemberActivity.devicemanufacturer_Id;
            objMemberActivity.Howlongago_Id = objModelMemberActivity.howlongago_Id;
            objMemberActivity.pluggedinYes = objModelMemberActivity.pluggedinYes;
            objMemberActivity.Device_id = objModelMemberActivity.device_id;
            objMemberActivity.Created_by = objModelMemberActivity.created_by;
            objMemberActivity.Created_Date = System.DateTime.Now;
            objMemberActivity.Member_name = objModelMemberActivity.member_name;
            objMemberActivity.Email_Id = objModelMemberActivity.email_Id;
            objMemberActivity.phonenumber = objModelMemberActivity.phonenumber;
            objMemberActivity.Invoice_No = objModelMemberActivity.invoice_No;
            objMemberActivity.IsTMP = objModelMemberActivity.IsTMP;
            objMemberActivity.Location_id = objModelMemberActivity.LocationId;
            return objMemberActivity;
        }
        private Tablet_App_Machine_Activity_Results MapFieldsToMachineActivityResults(Tablet_App_Machine_Activity_Results objMachineActivityResults, MemberActivityModel objModelMemberActivity)
        {

            objMachineActivityResults.Category_Id = objModelMemberActivity.result_category_Id;
            objMachineActivityResults.Revive_Question = objModelMemberActivity.revive_questions;
            objMachineActivityResults.Revive_Answer = objModelMemberActivity.revive_answers;
            objMachineActivityResults.Activity_Results = objModelMemberActivity.activity_Results;

            objMachineActivityResults.Created_by = objModelMemberActivity.created_by;
            objMachineActivityResults.Created_Date = System.DateTime.Now;
            objMachineActivityResults.isMember = objModelMemberActivity.isMember;
            objMachineActivityResults.Water_removed = (decimal)objModelMemberActivity.Water_removed;
            objMachineActivityResults.Dry_time = (decimal)objModelMemberActivity.Dry_time;

            return objMachineActivityResults;
        }
        #endregion

        private string MapDebugValues(int machine_Id, string DebugValues)
        {
            string ActivityResult = "";
            if (DebugValues.Length > 0)
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var MachineParamlst = (from machTemp in dbContext.MachineTemplateMappings
                                           join debgTemp in dbContext.Debug_Templates on machTemp.TempalteId equals debgTemp.Template_ID
                                           join debgdef in dbContext.Debug_Templates_Definition on debgTemp.Template_ID equals debgdef.Template_ID
                                           join mstConfig in dbContext.MasterData_Config_definitions on debgdef.DebugParameter_Id equals mstConfig.MasterData_Type_ID
                                           where mstConfig.MasterData_Type == "Parameter"
                                           && machTemp.MachineId == machine_Id
                                           select new CustomizedParameterModel
                                           {
                                               template_ID = debgTemp.Template_ID,
                                               debugParameter_Id = debgdef.DebugParameter_Id,
                                               debugParameterName = mstConfig.Custom_Field1
                                           }).ToList();



                    if (DebugValues.Split(',').Length > 0)
                    {
                        string[] debugValuesArray = DebugValues.Split(',');
                        string comma = "";
                        foreach (string debugVal in debugValuesArray)
                        {
                            string[] debugvalSingleArray = debugVal.Split(':');
                            var findData = MachineParamlst.FirstOrDefault(c => c.debugParameterName == debugvalSingleArray[0]);
                            if (findData != null)
                            {
                                ActivityResult = ActivityResult + comma + debugVal;
                                comma = ",";
                            }
                        }


                    }
                    else
                    {
                        ActivityResult = DebugValues;
                    }

                }


            }



            return ActivityResult;
        }
        private string GetValueByfaultCategoryId(int faultcategory_id, string binaryfaultvalues)
        {

            string ErrorFaultvalues = "";
            using (var dbContext = new ReviveDBEntities())
            {
                var errorParameterValues = dbContext.MachinefaultParameterValues.Where(c => c.Faultcategory_id == faultcategory_id).OrderBy(x => x.SeqNo).ToList();
                string commaseparator = "";
                if (errorParameterValues != null)
                {
                    if (binaryfaultvalues.Length > 0 && binaryfaultvalues.Length <= 16)
                    {
                        for (int i = 0; i < binaryfaultvalues.Length; i++)
                        {
                            string charval = binaryfaultvalues.Substring(i, 1);
                            if (charval == "1")
                            {
                                ErrorFaultvalues = ErrorFaultvalues + commaseparator + errorParameterValues[i].ParameterName.ToString();
                                commaseparator = ",";
                            }


                        }

                    }
                }

            }





            return ErrorFaultvalues;


        }


        public IEnumerable<AppMachineHistoryModel> GetAppMachineHistoryByMachineId(MembersShipModel objMembership)
        {
            IEnumerable<AppMachineHistoryModel> objMembshipDetails;
            string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();
            using (var dbContext = new ReviveDBEntities())
            {


                objMembshipDetails = dbContext.usp_AppMachineHistoryByMachineId(objMembership.machine_Id_Val, objMembership.MachineHistoryDaysLength).Select(d => new AppMachineHistoryModel()
                {
                    CustomerName = d.Customer_Name != null && d.Customer_Name.Trim() != "" && d.Customer_Name.Trim() != string.Empty ? (d.Customer_Name.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.Customer_Name.Substring(0, d.Customer_Name.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.Customer_Name) : d.Customer_Name,
                    IsTempValue = d.IsTMP,
                    LengthOfDry = d.LengthOfDry,
                    WaterRemoved = d.WaterRemoved,
                    Results = d.ReviveSuccessStatus,
                    ActivityDate = d.Activity_Date,
                    ProcessId = d.Process_Id,
                    InvoiceNo = d.Invoice_no,
                    ActivityTime = d.ActivityTime,
                    LengthOfDryLong = d.LengthOfDryLong,
                    LengthOfDryDesc = d.LengthOfDryDesc

                }).ToList();



            }


            return objMembshipDetails;

        }

        public AppMachineHistoryModel GetAppMachineHistoryByProcessId(MembersShipModel objMembership)
        {
            AppMachineHistoryModel objMembshipDetails;
            string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();
            using (var dbContext = new ReviveDBEntities())
            {
                objMembshipDetails = dbContext.usp_AppMachineHistoryByProcessId(objMembership.ProcessId).Select(d => new AppMachineHistoryModel()
                {
                    CustomerName = d.Customer_Name != null && d.Customer_Name.Trim() != "" && d.Customer_Name.Trim() != string.Empty ? (d.Customer_Name.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.Customer_Name.Substring(0, d.Customer_Name.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.Customer_Name) : d.Customer_Name,
                    MemberEmailId = d.Email != null && d.Email.Trim() != "" && d.Email.Trim() != string.Empty ? (d.Email.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.Email.Substring(0, d.Email.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.Email) : d.Email,
                    LengthOfDry = d.LengthOfDry,
                    LengthOfDryLong = d.LengthOfDryLong,
                    LengthOfDryDesc = d.LengthOfDryDesc,
                    WaterRemoved = d.WaterRemoved,
                    Results = d.ReviveSuccessStatus,
                    ActivityDate = d.Activity_Date,
                    ProcessId = d.Process_Id,
                    InvoiceNo = d.Invoice_no,
                    ActivityTime = d.ActivityTime,
                    MemberLocation = d.Location,
                    //MemberPhone=d.PhoneNumber                   
                    MemberPhone = d.PhoneNumber != null && d.PhoneNumber != "" && d.PhoneNumber != string.Empty ? (d.PhoneNumber.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.PhoneNumber.Substring(0, d.PhoneNumber.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.PhoneNumber) : d.PhoneNumber,
                    ReviveAnswer = d.Revive_Answer

                }).FirstOrDefault();

            }
            return objMembshipDetails;

        }


        /// <summary>
        /// Check Machine Kill or active
        /// </summary>
        /// <param name="machine_id"></param>
        /// <returns></returns>
        public string CheckMachineActive(int machine_id)
        {
            string returnval = "";
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var machineIdQuery = dbContext.Machines.FirstOrDefault(mach => mach.Machine_Id == machine_id);

                    if (machineIdQuery != null)
                    {
                        //var killedMachine = from u in dbContext.Machines
                        //                    where u.Machine_Id == machineIdQuery.Machine_Id
                        //                    let ces = from ce in dbContext.Machine_activity_detail
                        //                              where ce.Activity_result == "Kill"
                        //                              select ce.Machine_Id
                        //                    where ces.Contains(u.Machine_Id)
                        //                    select u;
                        int statusId = GetStatuesID(ConstantEntities.StatuesKilled, dbContext);
                        if (machineIdQuery != null)
                        {
                            if (statusId == machineIdQuery.StatusId)
                            {

                                returnval = "Machine Id " + machine_id.ToString() + " Has been Killed.";
                            }
                        }

                    }
                    else
                    {
                        returnval = "Machine Id is not valid with Enterprise Server.";
                    }

                }

            }
            catch (Exception ex)
            {
                returnval = "Error in validating machine Id" + ex.Message.ToString();
            }
            return returnval;

        }
        private int GetStatuesID(string status, ReviveDBEntities dbContext)
        {
            try
            {
                var RecordmachineId = dbContext.MasterData_Config_definitions.Where(c => c.MasterData_Type == ConstantEntities.MachineStatus && c.MasterData_Value == status).FirstOrDefault();
                if (RecordmachineId != null)
                {
                    return RecordmachineId.MasterData_Type_ID;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return 0;

        }
        public SoftwareVersionModel GetSoftwareVersionByMachineId(int machine_Id)
        {
            using (var dbContext = new ReviveDBEntities())
            {
                var result1 = (from p in dbContext.Machines
                               join mst in dbContext.MasterData_Config_definitions on p.Proposedversion_id equals mst.MasterData_Type_ID
                               where p.Machine_Id == machine_Id
                               select new SoftwareVersionModel
                               {
                                   softwareVersion = mst.MasterData_Value,
                                   softwareVersionPath = mst.Custom_Field1,
                                   softwareDetails = mst.CustomField2,
                                   softwareNote = mst.CustomField3,
                                   FileCheckSumKey = mst.CustomField4
                               }).FirstOrDefault();
                return result1;

            }

        }

        public int GetCustomizedTemplateIdByMachineId(int machine_id)
        {
            int _Template_Id = 0;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {


                    var MachineCustomizeParameter = from mchne in dbContext.MachineTemplateMappings
                                                    where mchne.MachineId == machine_id && mchne.TempalteId != null
                                                    select mchne;
                    if (MachineCustomizeParameter.ToList().Count > 0)
                    {
                        _Template_Id = (int)MachineCustomizeParameter.Select(a => a.TempalteId).FirstOrDefault();
                    }

                }

            }
            catch (Exception ex)
            {
                _Template_Id = 0;
            }
            return _Template_Id;

        }
        /// <summary>
        /// GetCustomizedDebugParameter
        /// </summary>
        /// <param name="Template_Id"></param>
        /// <returns></returns>
        public IEnumerable<CustomizedParameterModel> GetCustomizedDebugParameter(int Template_Id)
        {
            var result = new List<CustomizedParameterModel>();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    result = (from dbgtemp in dbContext.Debug_Templates_Definition
                              join mstconfig in dbContext.MasterData_Config_definitions on dbgtemp.DebugParameter_Id equals mstconfig.MasterData_Type_ID
                              where dbgtemp.Template_ID == Template_Id && mstconfig.MasterData_Type == "Parameter"
                              select new CustomizedParameterModel
                              {
                                  template_ID = dbgtemp.Template_ID,
                                  debugParameter_Id = dbgtemp.DebugParameter_Id,
                                  debugParameterName = mstconfig.MasterData_Value
                              }).ToList();


                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;

        }
        public int GetMachineIdByMachineIdVal(string machine_Id_Val)
        {
            int _machine_id = 0;
            using (var dbContext = new ReviveDBEntities())
            {
                var machineIdQuery = dbContext.Machines.FirstOrDefault(mach => mach.Machine_Id_Val == machine_Id_Val);
                if (machineIdQuery != null)
                {
                    _machine_id = machineIdQuery.Machine_Id;
                }

                return _machine_id;
            }
        }

        /// <summary>
        /// New method add for checking machine has mapped to particular location 
        /// </summary>
        /// <param name="machine_Id"></param>
        /// <returns></returns>
        public int GetMachineIdValidateForReviveByMachineId(int machine_Id)
        {
            int _machine_id = 0;
            using (var dbContext = new ReviveDBEntities())
            {
                var machineIdQuery = (from mc in dbContext.Machines
                                      join mcloc in dbContext.LocationMapMachines
                                      on mc.Machine_Id equals mcloc.Machine_Id
                                      where mc.IsTested == true && mc.IsAssigned == true && mc.IsShipped == true && mc.Machine_Id == machine_Id
                                      select mc).FirstOrDefault();

                                     

                if (machineIdQuery != null)
                {
                    _machine_id = machineIdQuery.Machine_Id;
                }

                return _machine_id;
            }
        }

        public ModeConfigurationAPIModel GetModeValidation(MemberActivityModel objMembership)
        {
            ModeConfigurationAPIModel objModeConfig = new ModeConfigurationAPIModel();
            objModeConfig.isModeValidate = true;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var getProcCall = dbContext.usp_GetModeValidate(objMembership.modeId, objMembership.machine_Id);
                    if (getProcCall.FirstOrDefault().StatusId == 0)
                    {
                        objModeConfig.isModeValidate = false;
                    }

                }
            }
            catch (Exception ex)
            {
                objModeConfig.isModeValidate = false;

                // result = false;
            }
            return objModeConfig;
        }
        public bool CheckMembershipActive(string membership_Id, Guid userId)
        {
            var _activeMember = true;
            using (var dbContext = new ReviveDBEntities())
            {
                var objMembshipDetails = new List<MemberShipDetailsModel>();
                var objuser = dbContext.Users.FirstOrDefault(cond => cond.User_ID == userId);
                objMembshipDetails = dbContext.usp_GetMembershipDetails(membership_Id, null, null, null, objuser.Customer_ID, null, null, null).Where(x => x.ActiveStatus == "Active").Select(d => new MemberShipDetailsModel()
                {
                    MemberName = d.MemberName,
                    Phone_Number = d.Phone_Number,
                    Email_ID = d.Email_ID,
                    Membership_ID = d.Membership_ID,
                    Membership_Start_date = d.Membership_Start_date,
                    Membership_End_date = d.Membership_End_date,
                    No_of_Free_dries = d.No_of_Free_dries,
                    StoreCreated = d.StoreCreated,
                    Store_ID = d.Store_ID,
                    MembershipStatus = d.MembershipStatus,
                    CommonText = d.CommonText,
                    Renew_Start_date = d.Renew_Start_date,
                    Renew_End_date = d.Renew_End_date,
                    Renew_Activation_date = d.Renew_Activation_date,
                    Renew_No_of_Free_dries = d.Renew_No_of_Free_dries
                }).ToList();

                if (objMembshipDetails.Count == 0)
                {
                    _activeMember = false;
                }


            }
            return _activeMember;
        }

        public int GetLastModeByMachineId(int machineId, int mode_id)
        {
            int modeId = 0;
            using (var dbContext = new ReviveDBEntities())
            {
                var ModeObj = dbContext.Machines.Where(x => x.Machine_Id == machineId).Select(x => x.Mode_id).FirstOrDefault();
                if (ModeObj != null)
                {
                    modeId = (int)ModeObj;
                }


                var MachineRecordFmMaster = dbContext.Machines.FirstOrDefault(x => x.Machine_Id == machineId);
                if (MachineRecordFmMaster != null)
                {
                    MachineRecordFmMaster.Mode_id = mode_id;
                    dbContext.SaveChanges();
                }


            }

            return modeId;


        }
        public bool DeleteTemplateByMachineId(int machineId)
        {
            using (var dbContext = new ReviveDBEntities())
            {

                var machineTemplate = dbContext.MachineTemplateMappings.FirstOrDefault(x => x.MachineId == machineId);
                if (machineTemplate != null)
                {
                    machineTemplate.TempalteId = 0;
                    dbContext.SaveChanges();
                }
            }

            return true;
        }
        /// <summary>
        /// Deleting Membership by Id
        /// </summary>
        /// <param name="objmembership"></param>
        /// <returns></returns>
        public RepositoryResult DeleteMemberShipById(MembersShipModel objmembership)
        {
            RepositoryResult objresult = new RepositoryResult();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var getProcCall = dbContext.usp_DeleteMembershipById(objmembership.membership_Id).FirstOrDefault();
                    if (getProcCall.StatusId == 1)
                    {
                        objresult.resultIdval = getProcCall.StatusMessages;
                        objresult.resultId = APIStatusValues.SuccessId;
                    }
                    else
                    {
                        objresult.resultId = APIStatusValues.FailureIdmsgs;
                        objresult.resultIdval = getProcCall.StatusMessages;
                    }



                }
            }
            catch (Exception ex)
            {
                // objresult.resultVal = ex.Message.ToString();
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = ex.Message.ToString();

            }

            return objresult;

        }
        /// <summary>
        /// Email_Id
        /// </summary>
        /// <param name="objmembership"></param>
        /// <returns></returns>
        public RepositoryResult CheckValidMemberByEmail(MembersShipModel objmembership)
        {
            RepositoryResult objresult = new RepositoryResult();
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var getProcCall = dbContext.usp_GetValidMemberById(objmembership.Email_Id).FirstOrDefault();

                    if (getProcCall != null)
                    {
                        if (getProcCall.MembershipStatus == "Invalid")
                        {
                            objresult.resultIdval = "Membership has been expired.";
                            objresult.resultId = APIStatusValues.FailureIdmsgs;
                        }
                        else if (getProcCall.MembershipStatus == "Valid")
                        {
                            objresult.resultIdval = "Valid Member";
                            objresult.resultId = APIStatusValues.SuccessId;
                        }
                    }
                    else
                    {
                        objresult.resultIdval = "Membership not found in ES.";
                        objresult.resultId = APIStatusValues.FailureId;
                    }

                }
            }
            catch (Exception ex)
            {
                // objresult.resultVal = ex.Message.ToString();
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultIdval = ex.Message.ToString();

            }

            return objresult;

        }
        /// <summary>
        /// IsValidMemberByDOB
        /// </summary>
        /// <param name="objmembership"></param>
        /// <returns></returns>
        public IsDOBValid IsValidMemberByDOB(MembersShipModel objmembership)
        {
            IsDOBValid objresult = new IsDOBValid();
            objresult.IsDOBMatch = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    var MemberInfo = dbContext.Memberships.FirstOrDefault(c => c.Email_ID == objmembership.Email_Id);


                    if (MemberInfo != null)
                    {
                        string DOBVal = Convert.ToDateTime(MemberInfo.DOB).ToString("MMddyyyy");
                        if (DOBVal == objmembership.DOBValue)
                        {
                            objresult.IsDOBMatch = true;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                objresult.IsDOBMatch = false;

            }

            return objresult;

        }
        /// <summary>
        /// Updating fault during Revive
        /// </summary>
        /// <param name="objMembershipActivity"></param>
        /// <returns></returns>
        public RepositoryResult UpdateReviveFaultResult(MemberActivityModel objMembershipActivity)
        {
            RepositoryResult objresult = new RepositoryResult();
            var membership = new Membership();
            using (var dbContext = new ReviveDBEntities())
            {
                Tablet_App_Machine_Activity_Results objNewReviveResults = new Tablet_App_Machine_Activity_Results();
                Tablet_App_Machine_Activity_Details objMachineNewActivity = new Tablet_App_Machine_Activity_Details();
                try
                {
                    var MachineLastActivity = dbContext.Tablet_App_Machine_Activity_Details.OrderByDescending(x => x.Machine_Activity_ID).Where(z => z.Machine_Id == objMembershipActivity.machine_Id && z.Process_Id != null).FirstOrDefault();
                    // Insert into MachineActivity table
                    objMembershipActivity.machineActivity_id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == TabletMachineActivityStages.ReviveStopedWithFault).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objMachineNewActivity = MapFieldsToMachineActivity(objMachineNewActivity, objMembershipActivity);
                    if (MachineLastActivity != null)
                    {
                        objMachineNewActivity.Process_Id = MachineLastActivity.Process_Id;
                    }
                    else
                    {
                        objMachineNewActivity.Process_Id = objMembershipActivity.process_Id;
                    }
                    dbContext.Tablet_App_Machine_Activity_Details.Add(objMachineNewActivity);


                    // Revive Results
                    objNewReviveResults = MapFieldsToMachineActivityResults(objNewReviveResults, objMembershipActivity);
                    objNewReviveResults.Category_Id = dbContext.MasterData_Config_definitions.Where(a => a.MasterData_Type == objMembershipActivity.resultCategoryCode).Select(a => a.MasterData_Type_ID).FirstOrDefault();
                    objNewReviveResults.Machine_Activity_ID = objMachineNewActivity.Machine_Activity_ID;
                    objNewReviveResults.Revive_Answer = null;
                    objNewReviveResults.Activity_Results = "Fault in machine during Revive.";
                    dbContext.Tablet_App_Machine_Activity_Results.Add(objNewReviveResults);
                    dbContext.SaveChanges();
                    objresult.resultId = APIStatusValues.SuccessId;
                    objresult.resultVal = "Fault data saved successfully.";
                }
                catch (Exception ex)
                {
                    objresult.resultId = APIStatusValues.FailureIdmsgs;
                    objresult.resultVal = "Error in Saving Fault Data." + ex.Message.ToString();
                }

            }
            return objresult;
        }
        /// <summary>
        /// Methord created to call by table App after crash app.
        /// </summary>
        /// <param name="APIParameterObj"></param>
        /// <returns></returns>
        public ReviveDetails GetReviveDetails(APIParameter APIParameterObj)
        {
            ReviveDetails MemberDetailsObj;
            string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();
            using (var dbContext = new ReviveDBEntities())
            {

                MemberDetailsObj = dbContext.usp_GetReviveDetails(APIParameterObj.ProcessId, APIParameterObj.MachineId).Select(d => new ReviveDetails()
                {
                    MemberName = d.MemberName != null && d.MemberName.Trim() != "" && d.MemberName.Trim() != string.Empty ? (d.MemberName.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.MemberName.Substring(0, d.MemberName.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.MemberName) : d.MemberName,
                    EmailId = d.EmailId != null && d.EmailId.Trim() != "" && d.EmailId.Trim() != string.Empty ? (d.EmailId.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.EmailId.Substring(0, d.EmailId.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.EmailId) : d.EmailId,
                    Phonenumber = d.Phonenumber != null && d.Phonenumber.Trim() != "" && d.Phonenumber.Trim() != string.Empty ? (d.Phonenumber.LastIndexOf("Non-Member", StringComparison.Ordinal) > 0 ? Cryptography.SplitText(Cryptography.DecryptText(d.Phonenumber.Substring(0, d.Phonenumber.LastIndexOf("Non-Member", StringComparison.Ordinal)), sKey), "@") : d.Phonenumber) : d.Phonenumber,
                    Membership_Id = d.Membership_Id,
                    // Phonenumber = d.Phonenumber,
                    IsMember = d.IsMember,
                    // EmailId = d.EmailId,
                    PhoneModel = d.PhoneModel
                }).FirstOrDefault();



            }


            return MemberDetailsObj;

        }
        /// <summary>
        /// Created this fuction for CR 15 Void membershio
        /// </summary>
        /// <param name="objmembership"></param>
        /// <returns></returns>
        public RepositoryResult VoidMembership(MembersShipModel objmembership)
        {

            // ReturnStatusCode objReturnStatus =new ReturnStatusCode();
            RepositoryResult objresult = new RepositoryResult();
            try
            {


                using (var dbContext = new ReviveDBEntities())
                {


                    var result = dbContext.usp_UpdateMemberAsVoid(objmembership.membership_Id, objmembership.user_Id).FirstOrDefault();

                    if (result.StatusId == 1)
                    {
                        objresult.resultIdval = result.MemberShipId;
                        objresult.resultVal = result.StatusMessages;
                        objresult.resultId = APIStatusValues.SuccessId;

                    }
                    else
                    {
                        objresult.resultId = APIStatusValues.FailureIdmsgs;
                        objresult.resultVal = result.StatusMessages;
                    }

                }

            }
            catch (Exception ex)
            {
                // objresult.resultVal = ex.Message.ToString();
                objresult.resultId = APIStatusValues.FailureIdmsgs;
                objresult.resultVal = ex.Message.ToString();

            }

            return objresult;

        }

        public string UpdateTabletNonMemberFieldAsEncryption(string key)
        {
            string result = "";
            using (var dbContext = new ReviveDBEntities())
            {
                if (key == ConstantEntities.NonMemberEncryptionKey)
                {
                    var NonMemberLst = dbContext.Tablet_App_NonMemberActivity_Details.ToList();

                    string sKey = System.Configuration.ConfigurationSettings.AppSettings["Encrption"].ToString();

                    foreach (var item in NonMemberLst)
                    {

                        item.Member_name = (item.Member_name != null && item.Member_name != string.Empty) ? Cryptography.EncryptText(item.Member_name + "@", sKey) : string.Empty;
                        item.Email_Id = (item.Email_Id != null && item.Email_Id != string.Empty) ? Cryptography.EncryptText(item.Email_Id + "@", sKey) : string.Empty;
                        item.phonenumber = (item.phonenumber != null && item.phonenumber != string.Empty) ? Cryptography.EncryptText(item.phonenumber + "@", sKey) : string.Empty;

                    }
                    dbContext.SaveChanges();

                }
                else
                {

                    result = "Encryption is not allowed";
                }



            }

            return result;
        }



        #region Manage Membership For EnterPrise Server
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ObjMemberShipParameters"></param>
        /// <returns></returns>
        public DataSourceResult GetMembershipDetailsES(DataSourceRequest req, MemberShipParameters ObjMemberShipParameters)
        {
            LoggingModel LoggingModelObj = new LoggingModel();
            WriteLog logwrite = new WriteLog();
            DataSourceResult obj = new DataSourceResult();

            List<MemberShipDetails> lstMembership = new List<MemberShipDetails>();




            try
            {
                using (var dbContext = new ReviveDBEntities())
                {

                    string sqlQuery = string.Format("select cast(a.Membership_ID as varchar) as MembershipId," +
                                                    "cast(a.MemberName as varchar) as MemberName, " +
                                                    "cast(isnull(dbo.[fn_GetReviveCountByMembershipId](a.Membership_ID),0) as int)  as RevivesUsed," +
                                                    "cast(a.Location_Name as varchar)  as Location_Name," +
                                                    "convert(varchar, a.Membership_Start_date ,110) as StartDate," +
                                                    "convert(varchar, a.Membership_End_date ,110) as EndDate," +
                                                    "cast(a.invoice_No as varchar) as InvoiceNumber," +
                                                    "cast(a.Phone_Number as varchar) as Phone_Number," +
                                                    "a.CustomerName as CustomerName," +
                                                    "isnull(a.No_of_Free_dries,0) As PendingRevives," +
                                                    "a.Membership_Det_ID as Membership_Det_ID," +
                                                    "a.IsVoid as IsVoid," +
                                                    "case when a.isMultiDevice=0 then 'No' else 'Yes' end as IsMultiDevice" +
                                                    " from view_MembershipDetails a  " +
                                                    "where 1 =1 ");

                    if (ObjMemberShipParameters.CustomerId > 0)
                    {
                        sqlQuery = string.Format(" {0} and a.CustomerId = isnull({1},a.CustomerId) ", sqlQuery, ObjMemberShipParameters.CustomerId);
                    }

                    if (ObjMemberShipParameters.SubsidiaryID > 0)
                    {
                        sqlQuery = string.Format(" {0} and isnull(a.Subsidiary_Id,0) = isnull(isnull({1},a.Subsidiary_Id),0) ", sqlQuery, ObjMemberShipParameters.SubsidiaryID);
                    }

                    if (ObjMemberShipParameters.SubAgentID > 0)
                    {
                        sqlQuery = string.Format(" {0} and  isnull(a.SubAgent_Id,0) = isnull(isnull({1},a.SubAgent_Id),0) ", sqlQuery, ObjMemberShipParameters.SubAgentID);
                    }

                    if (ObjMemberShipParameters.LocationId > 0)
                    {
                        sqlQuery = string.Format(" {0} and  isnull(a.locationId,0) = isnull(isnull({1},a.locationId),0) ", sqlQuery, ObjMemberShipParameters.LocationId);
                    }

                    if (ObjMemberShipParameters.DateFrom != null && ObjMemberShipParameters.DateFrom != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                    {
                        string fromDate = ObjMemberShipParameters.DateFrom.ToString("MM/dd/yyyy HH:mm:ss");
                        sqlQuery = string.Format(" {0} and  a.Membership_Start_date >= '{1}' ", sqlQuery, fromDate);
                    }

                    if (ObjMemberShipParameters.DateTo != null && ObjMemberShipParameters.DateTo != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                    {
                        string ToDate = ObjMemberShipParameters.DateTo.ToString("MM/dd/yyyy") + " 23:59:00";
                        sqlQuery = string.Format(" {0} and  a.Membership_Start_date <= '{1}' ", sqlQuery, ToDate);
                    }

                    var dataResult = dbContext.Database.SqlQuery<MemberShipDetails>(sqlQuery);
                    obj = dataResult.ToDataSourceResult(req);

                    logwrite.funcName = "GetMembershipDetailsES Called-----1";
                    var list2 = new List<Tuple<string, string>>{
                        new Tuple<string,string>(" Url:", "GetMembershipDetailsES"),
                        new Tuple<string,string>(" Count:",obj.Total.ToString())
                        };
                    logwrite.keyValPair = list2;
                    CommonMethods.WriteLog(logwrite);



                }
            }
            catch (Exception ex)
            {
                logwrite.funcName = "GetMembershipDetailsES Called-----";
                var list1 = new List<Tuple<string, string>>{
                        new Tuple<string,string>(" Url:", "GetMembershipDetailsES"),
                        new Tuple<string,string>(" Error:",ex.Message.ToString())
                        };
                logwrite.keyValPair = list1;
                CommonMethods.WriteLog(logwrite);



            }
            return obj;

        }


        public ManageMember GetMemberDetailsByMemberDtlsID(MemberShipParameters ObjMemberShipParameters)
        {
            using (var dbContext = new ReviveDBEntities())
            {
                string sqlQuery = string.Format("select cast(a.Membership_ID as varchar) as MembershipId," +
                                                "cast(a.MemberName as varchar) as MemberName, " +
                                                "cast(isnull(dbo.[fn_GetReviveCountByMembershipId](a.Membership_ID),0) as int)  as RevivesUsed," +
                                                "cast(a.Location_Name as varchar)  as Location_Name," +
                                                " a.Membership_Start_date  as MembershipStartDate," +
                                                " a.Membership_End_date  as EndDate," +
                                                "cast(a.invoice_No as varchar) as InvoiceNumber," +
                                                "cast(a.Phone_Number as varchar(200)) as MobileNumber," +
                                                "a.CustomerName as CustomerName," +
                                                "isnull(a.No_of_Free_dries,0) as PendingRevives," +
                                                "a.Membership_Det_ID as Membership_Det_ID," +
                                                "isnull((dbo.[fn_GetLastReviveDateByMembershipId](a.Membership_ID)),'') as LastReviveUsedDate," +
                    //"a.DOB as DOB," +
                                                "a.IsVoid as IsVoid," +
                                                "cast(a.Email_ID as varchar) as EmailId," +
                                                "(select isnull(voidDays,0) from Customers  where Customer_ID=a.CustomerID) as voidDays," +
                                                "case when DATEDIFF(day,a.Membership_Start_date,GETDATE())>((select isnull(voidDays,0) from Customers  where Customer_ID=a.CustomerID)) then 'Yes' else 'No' end VoidExpire," +
                                                "isnull([dbo].[fn_GetReviveCountByMembershipId](a.Membership_ID),0)  as DryAttemptCount," +
                                                "a.MaxNoOfDevices as custMaxNoOfDevices," +
                                                "a.IsMultiDevice"+
                                                " from view_MembershipDetails a" +
                                                " where a.Membership_Det_ID = isnull({0},a.Membership_Det_ID)"
                                                , ObjMemberShipParameters.Membership_Details_ID);

                var dataResult = dbContext.Database.SqlQuery<ManageMember>(sqlQuery);
                return dataResult.FirstOrDefault();
            }
        }

        public DateTime GetReviveLastDateByMembershipId(string MembershipID)
        {
            using (var dbContext = new ReviveDBEntities())
            {


                string sqlQuery = string.Format("select " +
                                              " Convert(varchar,case when isnull((dbo.[fn_GetFirstReviveDateByMembershipId](a.Membership_ID)),'')<>''" +
                                               " then DATEADD(day, (select isnull(EligibiltyWaitPeriod,0)*-1 from Customers where Customer_ID=a.CustomerId), dbo.[fn_GetFirstReviveDateByMembershipId](a.Membership_ID))" +
                                               " else isnull((dbo.[fn_GetFirstReviveDateByMembershipId](a.Membership_ID)),'') end,101) as LastReviveUsedDate" +
                                                " from view_MembershipDetails a" +
                                                " where a.Membership_ID = isnull({0},a.Membership_ID)"
                                                , MembershipID);

                var dataResult = dbContext.Database.SqlQuery<ManageMember>(sqlQuery);
                var ReviveLastDate = dataResult.Select(x => x.LastReviveUsedDate).FirstOrDefault();
                if (ReviveLastDate == null || ReviveLastDate == "")
                {
                    ReviveLastDate = "01/01/1900";
                }
                return Convert.ToDateTime(ReviveLastDate);
            }
        }



        public bool updateMemberDetailsByMemberDtlsID(ManageMember objMembershipDetails)
        {
            var result = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (objMembershipDetails.Membership_Det_ID != null && objMembershipDetails.Membership_Det_ID > 0)
                    {

                        var membershipDetails = dbContext.Memebership_Details.FirstOrDefault(member => member.Membership_Det_ID == objMembershipDetails.Membership_Det_ID);
                        DateTime memberShipEndDate = DateTime.Now;
                        DateTime memberShipStartDate = DateTime.Now;



                        if (objMembershipDetails.MembershipStartDate != null)
                        {
                            TimeSpan TimeMemberDate;
                            if (membershipDetails.Membership_Start_date > objMembershipDetails.MembershipStartDate)
                            {
                                TimeMemberDate = (TimeSpan)(membershipDetails.Membership_Start_date - objMembershipDetails.MembershipStartDate);
                            }
                            else
                            {
                                TimeMemberDate = (TimeSpan)(objMembershipDetails.MembershipStartDate - membershipDetails.Membership_Start_date);
                            }

                            double MemberSDateInDays = TimeMemberDate.TotalDays;
                            DateTime memberShipEDate = ((Convert.ToDateTime(membershipDetails.Membership_End_date)).AddDays(MemberSDateInDays));

                            DateTime dtE = DateTime.Parse(membershipDetails.Membership_End_date.ToString());
                            string EtimeString = dtE.ToString("HH:mm:ss");//TimeString StartDate

                            DateTime dtS = DateTime.Parse(membershipDetails.Membership_Start_date.ToString());
                            string StimeString = dtS.ToString("HH:mm:ss");//TimeString EndDate

                            memberShipStartDate = objMembershipDetails.MembershipStartDate.Add(TimeSpan.Parse(StimeString));//MemberShipStartDate
                            memberShipEndDate = memberShipEDate.Date.Add(TimeSpan.Parse(EtimeString));//MemberShipEndDate
                        }


                        if (membershipDetails != null)
                        {
                            membershipDetails.Invoice_No = objMembershipDetails.InvoiceNumber;
                            membershipDetails.Modified_Date = System.DateTime.Now;
                            membershipDetails.Membership_Start_date = memberShipStartDate;
                            membershipDetails.Membership_End_date = memberShipEndDate;
                        }
                    }

                    if (objMembershipDetails.MembershipId != null && objMembershipDetails.MembershipId != "")
                    {
                        var membership = dbContext.Memberships.FirstOrDefault(member => member.Membership_ID == objMembershipDetails.MembershipId);
                        if (membership != null)
                        {
                            //membership.DOB = objMembershipDetails.DOB;
                            membership.Phone_Number = objMembershipDetails.MobileNumber;
                            membership.Email_ID = objMembershipDetails.EmailId;
                            membership.Modified_Date = System.DateTime.Now;

                            if (membership.IsLegacy == true && objMembershipDetails.EmailId != null && objMembershipDetails.MobileNumber != null)
                            {
                                membership.IsLegacy = false;
                            }

                        }
                    }
                    dbContext.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }



        public bool voidMemberShipByMembershipId(MemberShipParameters MembershipPara)
        {
            var result = false;
            try
            {
                using (var dbContext = new ReviveDBEntities())
                {
                    if (MembershipPara.MembershipId != null && MembershipPara.MembershipId != "")
                    {
                        var membership = dbContext.Memberships.FirstOrDefault(member => member.Membership_ID == MembershipPara.MembershipId);
                        if (membership != null)
                        {
                            membership.IsVoid = true;

                            membership.Modified_Date = System.DateTime.Now;
                        }
                        dbContext.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region CR 20 21-06-2016
        /// <summary>
        /// GetMembmershipDetails which will return last revive details as per Phone Number.
        /// </summary>
        /// <param name="objMembership"></param>
        /// <returns></returns>
        public IEnumerable<ReviveDataByPhoneParam> GetReviveData(MembersShipModel objMembership)
        {
            IEnumerable<ReviveDataByPhoneParam> objMembshipDetails;

            using (var dbContext = new ReviveDBEntities())
            {
                objMembshipDetails = dbContext.usp_GetReviveData(objMembership.Phone_Number).Select(d => new ReviveDataByPhoneParam()
                {
                    CustomerName = d.Customer_Name,
                    ProcessDate = (DateTime)d.Activity_Date,
                    DeviceTypeName = d.DeviceType,
                    Status = d.Status,
                    FreeAttempt = d.FreeAttempt,
                    MobileNumber = d.MobileNumber,
                    IsMember = d.IsMember,
                    ProcessId = d.Process_Id
                }).ToList();
            }
            return objMembshipDetails;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objMembershipActivity"></param>
        /// <param name="ActivityTypeCode"></param>
        /// <returns></returns>
        public bool IsAPIAccessByClient(string CustRefCode, Guid APIKey)
        {
            bool result = false;

            using (var dbContext = new ReviveDBEntities())
            {
                var Customer_RecordCount = dbContext.Customers.Where(z => z.Customer_Ref_Code == CustRefCode && z.APIKey == APIKey).ToList().Count();

                if (Customer_RecordCount > 0)
                {
                    result = true;
                }
            }
            return result;


        }

        public IEnumerable<MemberShipDetailsModel> SearchMemberByCustomerId(MembersShipModel objMembership)
        {
            IEnumerable<MemberShipDetailsModel> objMembshipDetails;

            using (var dbContext = new ReviveDBEntities())
            {
                var objCustomer = dbContext.Customers.FirstOrDefault(cond => cond.APIKey == objMembership.APIKey);
                objMembshipDetails = dbContext.usp_GetMembershipDetails(objMembership.membership_Id, objMembership.MemberName, objMembership.Email_Id, objMembership.Phone_Number, objCustomer.Customer_ID, objMembership.invoice_No, objMembership.fmSlno, objMembership.toSlno).Select(d => new MemberShipDetailsModel()
                {
                    MemberName = d.MemberName,
                    Phone_Number = d.Phone_Number,
                    Email_ID = d.Email_ID,
                    Membership_ID = d.Membership_ID,
                    Membership_Start_date = d.Membership_Start_date,
                    Membership_End_date = d.Membership_End_date,
                    Membership_Activation_date = d.Membership_Activation_date,
                    No_of_Free_dries = d.No_of_Free_dries,
                    StoreCreated = d.StoreCreated,
                    Store_ID = d.Store_ID,
                    MembershipStatus = d.MembershipStatus,
                    CommonText = d.CommonText,
                    Renew_Start_date = d.Renew_Start_date,
                    Renew_End_date = d.Renew_End_date,
                    Renew_Activation_date = d.Renew_Activation_date,
                    Renew_No_of_Free_dries = d.Renew_No_of_Free_dries,
                    createdBy = d.Created_by,
                    member_LastReviveDate = d.Member_LastReviveDate,
                    member_LastLocation = d.Member_LastLocation,
                    member_LastStore = d.Member_LastStore,
                    member_LastUserName = d.Member_LastUserName,
                    IsLegacy = (bool)d.isLegacy,
                    DOB = d.DOB,
                    IsActive = d.ActiveStatus == "Active" ? true : false
                }).ToList();



            }
            return objMembshipDetails;

        }


        #endregion





    }
}
