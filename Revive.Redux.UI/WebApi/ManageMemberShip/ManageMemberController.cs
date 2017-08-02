using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Revive.Redux.Services;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Revive.Redux.Controllers.Common;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web;
using System.Web.Http.Cors;
using Revive.Redux.Entities;
using Revive.Redux.Entities.Common;
using System.Configuration;
using System.Collections;
using System.Web.Hosting;
using Revive.Redux.Common;
using System.Web.Http.Controllers;
using Revive.Redux.Commn;


namespace Revive.Redux.UI.WebApi.ManageMemberShip
{

    [ReviveApiAuth]
    public class ManageMemberController : ApiController
    {
        IMembershipService _membershipService = null;
        ReturnStatusCode objStatus = null;
        APIResponseData objApiResponseData = null;
        RepositoryResult objRepositoryResult = null;
        WriteLog logwrite = null;

        #region Constructor
        public ManageMemberController()
        {
            _membershipService = new MembershipService();
            objStatus = new ReturnStatusCode();
            objApiResponseData = new APIResponseData();
            objRepositoryResult = new RepositoryResult();
        }
        #endregion

        #region Method

        /// <summary>
        /// Add New member / Search member / Update Member / Renew Member
        /// </summary>
        /// <param name="objMembershipModel"></param>
        /// <returns></returns>


        [ActionName("AddNewMember")]
        [HttpPost]
        public APIResponseData AddNewMember(MembersShipModel objMembershipModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMembershipModel.user_Id = _userId;
                objMembershipModel.Created_by = _userId;
                //Boolean EmailExist = _membershipService.CheckEmailAddressExists(objMembershipModel.Email_Id, "", _userId);
                Boolean EmailExist = false;
                if (EmailExist == false)
                {
                    objRepositoryResult = _membershipService.AddEditMembership(objMembershipModel);
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;

                    if (objRepositoryResult.resultId == 1)
                    {
                        objMembershipModel.membership_Id = objRepositoryResult.resultIdval;
                        var MembershipData = _membershipService.GetMembmershipDetails(objMembershipModel);
                        objApiResponseData.responseData = MembershipData;
                        objStatus.StatusId = APIStatusValues.SuccessId;
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                        DateTime dtStart = (DateTime)MembershipData.FirstOrDefault().Membership_Start_date;
                        DateTime dtEnd = (DateTime)MembershipData.FirstOrDefault().Membership_End_date;
                        DateTime dtActivation = (DateTime)MembershipData.FirstOrDefault().Membership_Activation_date;
                        sendmailNotification(MembershipData.FirstOrDefault().MemberName, MembershipData.FirstOrDefault().Email_ID, "Registered", dtStart.ToString("MM/dd/yyyy"), dtEnd.ToString("MM/dd/yyyy"), dtActivation.ToString("MM/dd/yyyy"), objMembershipModel.membership_Id);

                    }
                    else
                    {
                        objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                        objStatus.StatusMessages = APIStatusValues.StatusFailure;
                        objApiResponseData.responseData = "Membership Details not Created.";
                    }
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                    objStatus.StatusMessages = "Email Id:" + objMembershipModel.Email_Id + " Already Registered.";
                    objApiResponseData.responseData = "Email Id:" + objMembershipModel.Email_Id + " Already Registered.";
                }


            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = ex.Message.ToString();
                objApiResponseData.responseData = "Error in Add New Member";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "AddNewMember Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" user_Id:", objMembershipModel.user_Id.ToString()),
                new Tuple<string,string>(" Email_Id:", objMembershipModel.Email_Id),
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData UpdateMembership(MembersShipModel objMembershipModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMembershipModel.user_Id = _userId;
                objMembershipModel.Created_by = _userId;

                // Boolean EmailExist = _membershipService.CheckEmailAddressExists(objMembershipModel.Email_Id, objMembershipModel.membership_Id, _userId);
                Boolean EmailExist = false;
                if (EmailExist == false)
                {
                    var result = _membershipService.updateMembership(objMembershipModel);
                    if (result == true)
                    {
                        objApiResponseData.responseData = "Membership details have been updated successfully.";
                        objStatus.StatusId = APIStatusValues.SuccessId;
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                        sendmailNotification(objMembershipModel.MemberName, objMembershipModel.Email_Id, "Updated", "", "", "", "");
                    }
                    else
                    {
                        objApiResponseData.responseData = "Membership details not Updated.";
                        objStatus.StatusId = APIStatusValues.FailureId;
                        objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    }
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                    objStatus.StatusMessages = "Email Id:" + objMembershipModel.Email_Id + " Already Registered.";
                    objApiResponseData.responseData = "Email Id:" + objMembershipModel.Email_Id + " Already Registered.";
                }


            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = ex.Message.ToString();
                objApiResponseData.responseData = "Error in Update Membership";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "UpdateMembership Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" Email_Id:", objMembershipModel.Email_Id),
                new Tuple<string,string>(" membership_Id:", objMembershipModel.membership_Id),
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        [ActionName("SearchMemberResult")]
        [HttpPost]
        public APIResponseData SearchMemberResult(MembersShipModel objMembershipModel)
        {

            Guid _userId = GetUserIdByKey();
            // End 
            objMembershipModel.user_Id = _userId;

            // Log Writing



            logwrite = new WriteLog();
            logwrite.funcName = "SearchMemberResult Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Email_Id:", objMembershipModel.membership_Id),
                new Tuple<string,string>(" MemberName:", objMembershipModel.MemberName),
                new Tuple<string,string>(" membership_Id:", objMembershipModel.membership_Id)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            IEnumerable<MemberShipDetailsModel> result = _membershipService.GetMembmershipDetails(objMembershipModel);
            if (result.Count() > 0)
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = result;//_membershipService.GetMembmershipDetails(objMembershipModel);
            }
            else
            {
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = APIStatusValues.NoDataFound;
            }
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData RenewMember(MembersShipModel objMembershipModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMembershipModel.user_Id = _userId;
                objMembershipModel.Created_by = _userId;

                RepositoryResult objresult = _membershipService.renewMembership(objMembershipModel);
                if (objresult.resultId == 1)
                {
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    if (objresult.resultId == 0)
                    {
                        objApiResponseData.responseData = objresult.resultVal;
                    }
                    else
                    {
                        objApiResponseData.responseData = "Membership renew created successfully.";
                        // Mail sent after renew member

                        MembersShipModel obj = new MembersShipModel();
                        obj.membership_Id = objMembershipModel.membership_Id;
                        obj.user_Id = objMembershipModel.user_Id;
                        var MembershipData = _membershipService.GetMembmershipDetails(obj);
                        string _dtStart = "";
                        string _dtEnd = "";
                        string _dtActivation = "";

                        if (MembershipData.FirstOrDefault().Renew_Start_date != null)
                        {
                            DateTime dtStart = (DateTime)MembershipData.FirstOrDefault().Renew_Start_date;
                            _dtStart = dtStart.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            DateTime dtStart = (DateTime)MembershipData.FirstOrDefault().Membership_Start_date;
                            _dtStart = dtStart.ToString("MM/dd/yyyy");
                        }
                        if (MembershipData.FirstOrDefault().Renew_End_date != null)
                        {
                            DateTime dtEnd = (DateTime)MembershipData.FirstOrDefault().Renew_End_date;
                            _dtEnd = dtEnd.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            DateTime dtEnd = (DateTime)MembershipData.FirstOrDefault().Membership_End_date;
                            _dtEnd = dtEnd.ToString("MM/dd/yyyy");
                        }
                        if (MembershipData.FirstOrDefault().Renew_Activation_date != null)
                        {
                            DateTime dtActivation = (DateTime)MembershipData.FirstOrDefault().Renew_Activation_date;
                            _dtActivation = dtActivation.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            DateTime dtActivation = (DateTime)MembershipData.FirstOrDefault().Membership_Activation_date;
                            _dtActivation = dtActivation.ToString("MM/dd/yyyy");
                        }



                        sendmailNotification(MembershipData.FirstOrDefault().MemberName, MembershipData.FirstOrDefault().Email_ID, "renewed", _dtStart, _dtEnd, _dtActivation, objMembershipModel.membership_Id);
                    }
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    objApiResponseData.responseData = objresult.resultVal;
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = ex.Message.ToString();
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "RenewMember Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" user_Id:", objMembershipModel.user_Id.ToString()),
                new Tuple<string,string>(" membership_Id:", objMembershipModel.membership_Id)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        [HttpPost]
        public APIResponseData SearchMemberByEmailId(MembersShipModel objMembershipModel)
        {

            Guid _userId = GetUserIdByKey();
            // End 
            objMembershipModel.user_Id = _userId;


            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "SearchMemberByEmailId Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Email_Id:", objMembershipModel.Email_Id)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            Boolean checkMembershipExists = _membershipService.CheckEmailAddressExists(objMembershipModel.Email_Id, "", _userId);
            if (checkMembershipExists == true)
            {


                IEnumerable<MemberShipDetailsModel> result = _membershipService.GetMembmershipDetails(objMembershipModel);
                if (result.Count() > 0)
                {
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    objApiResponseData.responseData = result;
                }
            }
            else
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = "Membership not registered with Email Id " + objMembershipModel.Email_Id;
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// GetDevivemanufacturerList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIResponseData GetDevivemanufacturerList()
        {
            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "GetDevivemanufacturerList Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            IEnumerable<DtoList> result = _membershipService.GetDevivemanufacturer();
            if (result.Count() > 0)
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = result;
            }
            else
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = "Devivemanufacturer List not configured in MasterConfigure table.";
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// GetHowlongagoList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIResponseData GetHowlongagoList()
        {
            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "GetHowlongagoList Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            IEnumerable<DtoList> result = _membershipService.GetHowlongago();
            if (result.Count() > 0)
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = result;
            }
            else
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = "How long ago List not configured in MasterConfigure table.";
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// GetModes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIResponseData GetModes()
        {
            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "GetModes Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            // Get User Id from Key
            Guid _userId = GetUserIdByKey();
            IEnumerable<DtoList> result = _membershipService.GetModes(_userId);
            if (result.Count() > 0)
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = result;
            }
            else
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = "Mode List not configured .";
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>


        [HttpPost]
        public APIResponseData ReviveCheckIn(MemberActivityModel objMemberActivity)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivity.created_by = _userId;


                objRepositoryResult = _membershipService.InitiateCheckinProcess(objMemberActivity);
                objStatus.StatusId = objRepositoryResult.resultId;
                objStatus.StatusMessages = objRepositoryResult.resultVal;
                //// API Response Data

                objApiResponseData.StatusMessages = objStatus;
                objApiResponseData.responseData = objRepositoryResult.resultVal;

                // Machie Debug customized or not? If it is customized send  debug parameter List




            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in Start Revive API";
                objApiResponseData.StatusMessages = objStatus;

            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "ReviveCheckin Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" isMember:", objMemberActivity.isMember.ToString()),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivity.machine_Id_Val),
                new Tuple<string,string>(" modeId:", Convert.ToString(objMemberActivity.modeId)),
                 new Tuple<string,string>(" invoice_No:", Convert.ToString(objMemberActivity.invoice_No))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData ReviveCheckOut(MemberActivityModel objMemberActivity)
        {

            string FilePath = "";
            string CompletePath = "";
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivity.created_by = _userId;


                objRepositoryResult = _membershipService.ReviveCheckOut(objMemberActivity);
                objStatus.StatusId = objRepositoryResult.resultId;
                objStatus.StatusMessages = objRepositoryResult.resultVal;
                //// API Response Data
                objApiResponseData.StatusMessages = objStatus;

                bool chksoftwareVersion = false;
                if (objRepositoryResult.resultId == 1 && chksoftwareVersion == false)
                {
                    // check Software version

                    var SoftwareVersionDetails = _membershipService.GetSoftwareVersionByMachineId((int)objMemberActivity.machine_Id);

                    string CurrentURI = Request.Headers.Host;
                    if (SoftwareVersionDetails != null)
                    {
                        FilePath = System.Web.HttpContext.Current.Server.MapPath("~") + SoftwareVersionDetails.softwareVersionPath;

                        CompletePath = "http://" + CurrentURI + "/" + SoftwareVersionDetails.softwareVersionPath;
                    }
                    bool SoftwarePath = false;
                    if (System.IO.File.Exists(FilePath))
                    {
                        SoftwarePath = true;
                        SoftwareVersionDetails.softwareVersionPath = CompletePath;
                    }

                    if (SoftwareVersionDetails != null && SoftwareVersionDetails.softwareVersion != objMemberActivity.software_version && SoftwarePath == true)
                    {
                        objApiResponseData.responseData = SoftwareVersionDetails;
                        objStatus.MachineCommand = "UpdateSoftware";
                        chksoftwareVersion = true;
                    }
                    else
                    {
                        objApiResponseData.responseData = SoftwareVersionDetails;
                        objStatus.MachineCommand = "Physical path not found";
                        chksoftwareVersion = false;
                    }
                }
                if (objRepositoryResult.resultId == 1 && chksoftwareVersion == false)
                {
                    // Check Debgu template exist or not
                    int template_Id = _membershipService.GetCustomizedTemplateIdByMachineId(objMemberActivity.machine_Id_Val);
                    if (template_Id > 0)
                    {
                        objApiResponseData.responseData = _membershipService.GetCustomizedDebugParameter(template_Id);
                        objStatus.MachineCommand = "SetDebug";
                    }

                }
            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = ex.Message.ToString();
                objApiResponseData.responseData = "Error in Stop Revive API";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "ReviveCheckOut Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages.ToString()),
                new Tuple<string,string>(" isMember:", objMemberActivity.isMember.ToString()),
                new Tuple<string,string>(" FilePath:", FilePath),
                new Tuple<string,string>(" CompletePathForTab:", CompletePath),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivity.machine_Id_Val)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        [HttpPost]
        public APIResponseData UpdateReviveStartResult(MemberActivityModel objMemberActivity)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivity.created_by = _userId;

                objRepositoryResult = _membershipService.UpdateReviveStartResult(objMemberActivity);
                objStatus.StatusId = objRepositoryResult.resultId;
                objStatus.StatusMessages = objRepositoryResult.resultVal;
                //// API Response Data

                objApiResponseData.StatusMessages = objStatus;
                objApiResponseData.responseData = objRepositoryResult.resultVal;



            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in  Update Revive Start results.";
                objApiResponseData.StatusMessages = objStatus;
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "UpdateReviveStartResult Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" isMember:", objMemberActivity.isMember.ToString()),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivity.machine_Id_Val),
                 new Tuple<string,string>(" modeId:", Convert.ToString(objMemberActivity.modeId))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);



            return objApiResponseData;
        }
        // called when revive end

        [HttpPost]
        public APIResponseData UpdateReviveEndResult(MemberActivityModel objMemberActivity)
        {
            try
            {




                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivity.created_by = _userId;

                objRepositoryResult = _membershipService.UpdateReviveEndResult(objMemberActivity);
                objStatus.StatusId = objRepositoryResult.resultId;
                objStatus.StatusMessages = objRepositoryResult.resultVal;
                //// API Response Data

                objApiResponseData.StatusMessages = objStatus;
                objApiResponseData.responseData = objRepositoryResult.resultVal;



            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in  Revive End results.";
                objApiResponseData.StatusMessages = objStatus;
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "UpdateReviveEndResult Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" isMember:", objMemberActivity.isMember.ToString()),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivity.machine_Id_Val),
                new Tuple<string,string>(" ActivityResult:", objMemberActivity.activity_Results),
                 new Tuple<string,string>(" modeId:", Convert.ToString(objMemberActivity.modeId))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData SendDebugInformation(MemberActivityModel objMemberActivity)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivity.created_by = _userId;

                objRepositoryResult = _membershipService.SendDebugInformation(objMemberActivity);
                objStatus.StatusId = objRepositoryResult.resultId;
                objStatus.StatusMessages = objRepositoryResult.resultVal;
                //// API Response Data

                objApiResponseData.StatusMessages = objStatus;
                objApiResponseData.responseData = objRepositoryResult.resultVal;



            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in  Debug Information Sending.";
                objApiResponseData.StatusMessages = objStatus;
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "SendDebugInformation Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" isMember:", objMemberActivity.isMember.ToString()),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivity.machine_Id_Val),
                new Tuple<string,string>(" ActivityResult:", objMemberActivity.activity_Results),
                 new Tuple<string,string>(" modeId:", Convert.ToString(objMemberActivity.modeId))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            return objApiResponseData;
        }
        [HttpPost]
        public APIResponseData MachineFault(MemberActivityModel objMemberActivityModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivityModel.created_by = _userId;

                if (objMemberActivityModel != null)
                {

                    objRepositoryResult = _membershipService.MachineFault(objMemberActivityModel);
                    objStatus.StatusId = objRepositoryResult.resultId;
                    objStatus.StatusMessages = objRepositoryResult.resultVal;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = objRepositoryResult.resultVal;
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = "Data parameter values are not correct.";
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Data parameter values are not correct.";
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in  saving machine fault information";
                objApiResponseData.StatusMessages = objStatus;

            }
            logwrite = new WriteLog();
            logwrite.funcName = "MachineFault Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" isMember:", objMemberActivityModel.isMember.ToString()),
                new Tuple<string,string>(" Process Id:", objMemberActivityModel.process_Id),
                new Tuple<string,string>(" Equipment:", objMemberActivityModel.machineFault.equipmentFaults),
                new Tuple<string,string>(" BoardFault:", objMemberActivityModel.machineFault.boardFaults),
                new Tuple<string,string>(" ProcessFault:", objMemberActivityModel.machineFault.processFaults),
                 new Tuple<string,string>(" ModeId:", objMemberActivityModel.modeId.ToString()),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivityModel.machine_Id_Val)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            return objApiResponseData;
        }
        [HttpPost]
        public APIResponseData UpdateSoftwareVersion(MemberActivityModel objMemberActivityModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivityModel.created_by = _userId;

                if (objMemberActivityModel != null)
                {

                    objRepositoryResult = _membershipService.updateSoftwareVersion(objMemberActivityModel);
                    objStatus.StatusId = objRepositoryResult.resultId;
                    objStatus.StatusMessages = objRepositoryResult.resultVal;
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = objRepositoryResult.resultVal;
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = "Data parameter values are not correct.";
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Data parameter values are not correct.";
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in Updating software version";
                objApiResponseData.StatusMessages = objStatus;
            }

            logwrite = new WriteLog();
            logwrite.funcName = "UpdateSoftwareVersion Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" isMember:", objMemberActivityModel.isMember.ToString()),
                new Tuple<string,string>(" FirmwareId:", objMemberActivityModel.firmwareID.ToString()),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivityModel.machine_Id_Val)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData GetModeValidation(MemberActivityModel objMemberActivityModel)
        {

            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivityModel.created_by = _userId;

                if (objMemberActivityModel != null)
                {

                    var result = _membershipService.GetModeValidation(objMemberActivityModel);

                    objApiResponseData.responseData = result;
                    if (result.isModeValidate == false)
                    {
                        objStatus.StatusMessages = result.machineStatus;
                        objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                    }
                    else
                    {
                        objStatus.StatusId = APIStatusValues.SuccessId;
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    }
                    //// Added by Vaibhav - on 15-09-2015
                    //if (result.machineStatus.ToLower().Contains("does not exist") || result.machineStatus.ToLower().Contains("has been killed") ||
                    //    (result.isModeValidate == false && result.machineStatus != string.Empty))
                    //{
                    //    objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                    //    if (result.machineStatus != string.Empty)
                    //        objStatus.StatusMessages = result.machineStatus;
                    //    else
                    //        objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    //}
                    //else
                    //{
                    //    objStatus.StatusId = APIStatusValues.SuccessId;
                    //    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    //}

                    objApiResponseData.StatusMessages = objStatus;


                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = "Data parameter values are not correct.";
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Data parameter values are not correct.";
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in Validating Modes from ES";
                objApiResponseData.StatusMessages = objStatus;
            }

            logwrite = new WriteLog();
            logwrite.funcName = "GetModeValidation Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" machine_Id_Val:", objMemberActivityModel.machine_Id_Val),
                new Tuple<string,string>(" modeId:", Convert.ToString(objMemberActivityModel.modeId))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            return objApiResponseData;
        }
        /// <summary>
        /// Deleting API by Membership ID
        /// </summary>
        /// <param name="objMembershipModel"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponseData DeleteMemberById(MembersShipModel objMembershipModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMembershipModel.user_Id = _userId;
                objMembershipModel.Created_by = _userId;
                var result = _membershipService.DeleteMemberShipById(objMembershipModel);
                if (result.resultId == 1)
                {
                    objApiResponseData.responseData = result.resultIdval;
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = result.resultIdval;

                }
                else
                {
                    objApiResponseData.responseData = result.resultIdval;
                    objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                    objStatus.StatusMessages = result.resultIdval;
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = ex.Message.ToString();
                objApiResponseData.responseData = "Error in Update Membership";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "DeleteMemberById Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" membership_Id:", objMembershipModel.membership_Id),
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData CheckValidMemberByEmail(MembersShipModel objMembershipModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMembershipModel.user_Id = _userId;
                objMembershipModel.Created_by = _userId;
                var result = _membershipService.CheckValidMemberByEmail(objMembershipModel);
                objApiResponseData.responseData = result.resultIdval;
                objStatus.StatusId = result.resultId;
                objStatus.StatusMessages = result.resultIdval;

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = ex.Message.ToString();
                objApiResponseData.responseData = "Error in Update Membership";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "CheckValidMemberByEmail Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Email_Id:", objMembershipModel.Email_Id),
                new Tuple<string,string>(" StatusMessages:", objStatus.StatusMessages),
                
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        [HttpPost]
        public APIResponseData IsValidMemberByDOB(MembersShipModel objMembershipModel)
        {

            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMembershipModel.user_Id = _userId;


                if (objMembershipModel != null)
                {

                    var result = _membershipService.IsValidMemberByDOB(objMembershipModel);

                    objApiResponseData.responseData = result;
                    if (result.IsDOBMatch == false)
                    {
                        objStatus.StatusMessages = "DOB Value Not Matched.";
                        objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                    }
                    else
                    {
                        objStatus.StatusId = APIStatusValues.SuccessId;
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    }
                    objApiResponseData.StatusMessages = objStatus;


                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = "Data parameter values are not correct.";
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Data parameter values are not correct.";
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in Validating DOB Values from ES";
                objApiResponseData.StatusMessages = objStatus;
            }

            logwrite = new WriteLog();
            logwrite.funcName = "IsValidMemberByDOB Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" Email:", objMembershipModel.Email_Id),
                new Tuple<string,string>(" DOB:", Convert.ToString(objMembershipModel.DOBValue))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            return objApiResponseData;
        }

        /// <summary>
        /// API called by Table App after Crash the App
        /// </summary>
        /// <param name="APIParameterObj"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponseData GetReviveDetails(APIParameter APIParameterObj)
        {
            try
            {
                if (APIParameterObj != null)
                {
                    if (APIParameterObj.ProcessId == null && APIParameterObj.Machine_Id_Val == null)
                    {
                        objStatus.StatusMessages = "Can not perform the operation.";
                        objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                    }
                    else
                    {
                        var result = _membershipService.GetReviveDetails(APIParameterObj);
                        objApiResponseData.responseData = result;
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                        objStatus.StatusId = APIStatusValues.SuccessId;
                    }

                    objApiResponseData.StatusMessages = objStatus;
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = "Data parameter values are not correct.";
                    objApiResponseData.StatusMessages = objStatus;
                    objApiResponseData.responseData = "Data parameter values are not correct.";
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = "Error in Validating Modes from ES";
                objApiResponseData.StatusMessages = objStatus;
            }

            logwrite = new WriteLog();
            logwrite.funcName = "GetReviveDetails Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" MachineId:", APIParameterObj.Machine_Id_Val),
                new Tuple<string,string>(" Process Id:", Convert.ToString(APIParameterObj.ProcessId))
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            return objApiResponseData;
        }

        /// <summary>
        /// CR16
        /// </summary>
        /// <param name="objMemberActivity"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponseData CheckFirmWareUpdate(MemberActivityModel objMemberActivity)
        {

            string FilePath = "";
            string CompletePath = "";

            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMemberActivity.created_by = _userId;

                var SoftwareVersionDetails = _membershipService.CheckFirmWareUpdate(objMemberActivity.machine_Id_Val);

                if (SoftwareVersionDetails != null)
                {
                    FilePath = System.Web.HttpContext.Current.Server.MapPath("~") + SoftwareVersionDetails.softwareVersionPath;
                    string CurrentURI = Request.Headers.Host;
                    CompletePath = "http://" + CurrentURI + "/" + SoftwareVersionDetails.softwareVersionPath;

                    bool SoftwarePath = false;
                    if (System.IO.File.Exists(FilePath))
                    {
                        SoftwarePath = true;
                        SoftwareVersionDetails.softwareVersionPath = CompletePath;
                    }

                    if (SoftwareVersionDetails != null && SoftwareVersionDetails.softwareVersion != objMemberActivity.software_version && SoftwarePath == true)
                    {
                        objApiResponseData.responseData = SoftwareVersionDetails;
                        objStatus.MachineCommand = "UpdateSoftware";
                        objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                        objStatus.StatusId = APIStatusValues.SuccessId;

                    }


                }
                else
                {
                    objApiResponseData.responseData = "Machine firmware is up to date";
                    objStatus.MachineCommand = "Physical path not found";
                    objStatus.StatusMessages = APIStatusValues.StatusFailure;
                    objStatus.StatusId = APIStatusValues.FailureId;
                }


            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = ex.Message.ToString();
                objApiResponseData.responseData = "Error in calling API CheckFirmWareUpdate";
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "CheckFirmWareUpdate Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", Convert.ToString(objStatus.StatusMessages)),
                new Tuple<string,string>(" isMember:", objMemberActivity.isMember.ToString()),
                new Tuple<string,string>(" FilePath:", FilePath),
                new Tuple<string,string>(" CompletePathForTab:", CompletePath),
                new Tuple<string,string>(" machineId:", objMemberActivity.machine_Id.ToString())
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objMembershipModel"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponseData UpdateMemberAsVoid(MembersShipModel objMembershipModel)
        {
            try
            {
                // Get UserId from Key
                Guid _userId = GetUserIdByKey();
                // End 
                objMembershipModel.user_Id = _userId;
                objMembershipModel.Created_by = _userId;

                RepositoryResult objresult = _membershipService.VoidMembership(objMembershipModel);
                if (objresult.resultId == 1)
                {
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    objApiResponseData.responseData = objresult.resultVal;

                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = objresult.resultVal;
                    objApiResponseData.responseData = objresult.resultVal;
                }

            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = ex.Message.ToString();
            }

            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "UpdateMemberAsVoid Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" user_Id:", objMembershipModel.user_Id.ToString()),
                new Tuple<string,string>(" membership_Id:", objMembershipModel.membership_Id)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);


            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData GetMachineHistoryByMachineId(MembersShipModel objMembershipModel)
        {

            Guid _userId = GetUserIdByKey();
            // End 
            objMembershipModel.user_Id = _userId;


            // Log Writing
            logwrite = new WriteLog();
            string daysLength = objMembershipModel.MachineHistoryDaysLength == null ? "" : objMembershipModel.MachineHistoryDaysLength.ToString();
            logwrite.funcName = "GetMachineHistoryByMachineId Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Machine Id:", objMembershipModel.machine_Id_Val),
                 new Tuple<string,string>(" MachineHistoryReportDays", daysLength)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            IEnumerable<AppMachineHistoryModel> result = _membershipService.GetAppMachineHistoryByMachineId(objMembershipModel);


            if (result.Count() > 0)
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = result;
            }

            else
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.NoDataFound;
                objApiResponseData.responseData = "Machine history data not exist with Machine Id " + objMembershipModel.machine_Id_Val;
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData GetMachineHistoryByProcessId(MembersShipModel objMembershipModel)
        {

            Guid _userId = GetUserIdByKey();
            // End 
            objMembershipModel.user_Id = _userId;


            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "GetMachineHistoryByProcessId Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Process Id:", objMembershipModel.ProcessId)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            AppMachineHistoryModel result = _membershipService.GetAppMachineHistoryByProcessId(objMembershipModel);


            if (result != null)
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = result;
            }

            else
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.NoDataFound;
                objApiResponseData.responseData = "Machine history data not exist with Machine Id " + objMembershipModel.ProcessId;
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData SendMailCheckOutResults(MembersShipModel objMembershipModel)
        {

            Guid _userId = GetUserIdByKey();
            // End 
            objMembershipModel.user_Id = _userId;


            // Log Writing
            logwrite = new WriteLog();
            logwrite.funcName = "SendMailCheckOutResults Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Process Id:", objMembershipModel.ProcessId)
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);

            AppMachineHistoryModel result = _membershipService.GetAppMachineHistoryByProcessId(objMembershipModel);


            if (result != null)
            {
                if (sendmailNotificationForCheckoutResults(result))
                {
                    objStatus.StatusId = APIStatusValues.SuccessId;
                    objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                    objApiResponseData.responseData = "Email sent to " + result.CustomerName + "  successfully.";
                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = APIStatusValues.NoDataFound;
                    objApiResponseData.responseData = "Error in sending mail to customer for  " + objMembershipModel.ProcessId;

                }
            }

            else
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.NoDataFound;
                objApiResponseData.responseData = "Machine history data not exist with Machine Id " + objMembershipModel.ProcessId;
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }



        private void sendmailNotification(string MembershipName, string MemberMailId, string ActionType, string StartDate, string EndDate, string ActivationDate, string MembershipId)
        {
            var emailLink = ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString();
            var templateVars = new Hashtable();
            templateVars.Add("UserFullName", MembershipName);
            templateVars.Add("ActionType", ActionType);


            if (ActionType == "Registered" || ActionType == "renewed")
            {
                string addOrRenew = string.Format("<p class=MsoNormal>Membership Id:{0}</p><p class=MsoNormal>Start Date:{1}</p><p class=MsoNormal>End Date:{2}</p><p class=MsoNormal>Activation Date:{3}</p>", MembershipId, StartDate, EndDate, ActivationDate);
                templateVars.Add("AddOrRenew", addOrRenew);
            }
            var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/TabletMembershipRegistration.html"), templateVars);
            var emailBody = parser.Parse(); // string.Format(ConstantEntities.NewCustomerEmailBody, emailLink, customer.Email, pasword);
            EmailHelper emailHelper = new EmailHelper();
            emailHelper.Send(MemberMailId, string.Empty, string.Empty, "Redux - New Membership Registration", emailBody);

        }

        private bool sendmailNotificationForCheckoutResults(AppMachineHistoryModel CheckoutResults)
        {
            var result = false;
            try
            {

                var templateVars = new Hashtable();
                string ResultsDesc = GetFailureReason(CheckoutResults.ReviveAnswer);
                templateVars.Add("ActivityDate", CheckoutResults.ActivityDate);
                templateVars.Add("Location", CheckoutResults.MemberLocation);
                templateVars.Add("CustomerName", CheckoutResults.CustomerName);
                templateVars.Add("MobileNumber", CheckoutResults.MemberPhone);
                templateVars.Add("DryTime", CheckoutResults.LengthOfDryDesc);
                templateVars.Add("WaterRemoved", CheckoutResults.WaterRemoved);

                if (CheckoutResults.Results == "Success")
                {
                    ResultsDesc = "<br/>"+GetSuccessReason(CheckoutResults.ReviveAnswer);

                }

                if (!String.IsNullOrEmpty(ResultsDesc) )
                {
                    templateVars.Add("Results", CheckoutResults.Results + ResultsDesc);
                }
                else
                {
                    templateVars.Add("Results", CheckoutResults.Results);
                }
                var resultsBccEmailid = ConfigurationSettings.AppSettings["CheckoutResults"].ToString();
                var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/MemberCheckoutResult.html"), templateVars);
                var emailBody = parser.Parse(); // string.Format(ConstantEntities.NewCustomerEmailBody, emailLink, customer.Email, pasword);
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.SendCheckOutResults(CheckoutResults.MemberEmailId, string.Empty, resultsBccEmailid, "Verizon Wireless/Redux Drying Service - Results Summary", emailBody);
                result = true;

            }
            catch (Exception ex)
            {

                result = false;

                // Log Writing
                logwrite = new WriteLog();
                logwrite.funcName = "sendmailNotificationForCheckoutResults Called-----";
                var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Mail Exception:", ex.Message.ToString())
                };
                logwrite.keyValPair = list1;
                CommonMethods.WriteLog(logwrite);


            }
            return result;

        }

        private string GetSuccessReason(string reviveAnswer)
        {
            if (String.IsNullOrEmpty(reviveAnswer))
            {
                return "";
            }
            var str_array = reviveAnswer.Split(',');
            var finalResult = "";

            for (int i = 0; i < str_array.Length; i++)
            {
                var strAns = str_array[i];
                var str_arrayOne = strAns.Split(':');

                var que = "";
                var ans = "";
                if (str_arrayOne.Length > 1)
                {
                    que = str_arrayOne[0];
                    ans = str_arrayOne[1];
                }

              

                if (ans == "Yes")
                {
                    finalResult += que;
                    finalResult += "<br/>";
                }


            }
            return finalResult;
        }
        private string GetFailureReason(string reviveAnswer)
        {
            if (String.IsNullOrEmpty(reviveAnswer))
            {
                return "";
            }
            var str_array = reviveAnswer.Split(',');
            var finalResult = "";
            var willAddMultiple = false;
            for (int i = 0; i < str_array.Length; i++)
            {
                var strAns = str_array[i];
                var str_arrayOne = strAns.Split(':');

                var que = "";
                var ans = "";
                if (str_arrayOne.Length > 1)
                {
                    que = str_arrayOne[0];
                    ans = str_arrayOne[1];
                }
                if (i == 0 && ans == "No")
                {
                    finalResult = " - Device did not power on";
                }
                else if (i == 0 && ans == "Yes")
                {
                    willAddMultiple = true;
                    finalResult = "";
                }
                else if (willAddMultiple)
                {
                    if (ans == "No")
                    {
                        if (finalResult == "")
                        {
                            finalResult += " - ";
                        }
                        finalResult += que;
                        finalResult += "<br/>";
                    }
                }

            }
            return finalResult;
        }

        private Guid GetUserIdByKey()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            object keyVal = headerValues.FirstOrDefault();
            string sKey = ConfigurationSettings.AppSettings["Encrption"].ToString();
            string _key = keyVal.ToString().Split(' ')[1].ToString();
            Guid userId = new Guid();
            if (_key != null)
            {
                string parameter = Cryptography.DecryptText(_key, sKey);
                string[] _params = parameter.Split('@');
                userId = new Guid(_params[0]);
            }
            return userId;

        }




        #endregion

    }


}

