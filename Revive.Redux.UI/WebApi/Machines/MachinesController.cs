using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Revive.Redux.Services;
using Revive.Redux.Entities;
using Revive.Redux.Entities.Common;
using Revive.Redux.Common;

namespace Revive.Redux.UI.WebApi.Machines
{
    public class MachinesController : ApiController
    {
        IGeneralService generalService = null;
        ReturnStatusCode objStatus = null;
        APIResponseData objApiResponseData = null;
        RepositoryResult objRepositoryResult = null;
        IOrderManagementService objApiOrderService = null;
        WriteLog logwrite = null;

        public MachinesController()
        {
            generalService = new GeneralService();
            objStatus = new ReturnStatusCode();
            objApiResponseData = new APIResponseData();
            objRepositoryResult = new RepositoryResult();
            objApiOrderService = new OrderManagementService();
            logwrite = new WriteLog();
        }
        /// <summary>
        /// addMachine
        /// Parameter MachineId
        /// </summary>
        /// <param name="objMachine"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponseData addMachine(WebApiMachineModel objMachine)
        {
            if (objMachine.MachineId == null || objMachine.MachineId == "")
            {
                objStatus.StatusId = 0;
                objStatus.StatusMessages = "Machine Id can not be null or blank.";

            }
            else
            {
                objStatus.StatusMessages = generalService.AddMachine(objMachine);
                if (objStatus.StatusMessages == "")
                {
                    objStatus.StatusId = 1;
                    objApiResponseData.responseData = "MachineId Added Successfully.";

                }
                else
                {
                    objStatus.StatusId = 0;
                    objApiResponseData.responseData = objStatus.StatusMessages;
                }
            }

            objApiResponseData.StatusMessages = objStatus;

            logwrite.funcName = "addMachine Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" Machine Id:", objMachine.MachineId.ToString())
               
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        /// <summary>
        /// addMachine
        /// Parameter MachineId
        /// </summary>
        /// <param name="objMachine"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponseData AddTestData(MachineTestingModel objMachine)
        {

            if (string.IsNullOrEmpty(objMachine.Machine_Id_val))
            {
                objStatus.StatusId = 0;
                objStatus.StatusMessages = "Machine Id can not be null or Zero.";

            }
            else
            {
                objStatus.StatusMessages = generalService.MachineTestResult(objMachine);
                if (objStatus.StatusMessages == "")
                {
                    objStatus.StatusId = 1;
                    objApiResponseData.responseData = "Machine Test Data Submitted Successfully.";

                }
                else
                {
                    objStatus.StatusId = 0;
                    objApiResponseData.responseData = objStatus.StatusMessages;
                }
            }

            objApiResponseData.StatusMessages = objStatus;

            logwrite.funcName = "TestMachine Called-----";
            //var list1 = new List<Tuple<string, string>>{
            //    new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
            //    new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
            //    new Tuple<string,string>(" Machine Id:", objMachine.MachineId.ToString())

            //    };
            //    logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }


        //[HttpPost]
        //public APIResponseData getMachineIdLocsList(MappedMachsLocs objList)
        //{
        //    if (objList != null)
        //    {
        //        if (objList.OrderID <= 0)
        //        {
        //            objStatus.StatusId = 0;
        //            objStatus.StatusMessages = "Order Id can not be null or blank.";

        //        }
        //        else
        //        {
        //            var result = objApiOrderService.APIGetMachineIdLocsList(objList.OrderID);
        //            if (result != null)
        //            {
        //                objStatus.StatusId = 1;
        //                objApiResponseData.responseData = result;
        //                objStatus.StatusMessages = "Data found and sent";

        //            }
        //            else
        //            {
        //                objStatus.StatusId = 0;
        //                objApiResponseData.responseData = "No data found";
        //                objStatus.StatusMessages = "No data found";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        objStatus.StatusId = 0;
        //        objStatus.StatusMessages = "Null object received.";
        //    }
        //    objApiResponseData.StatusMessages = objStatus;

        //    logwrite.funcName = "getMachineIdLocsList Called-----";
        //    string orderId = (objList != null && objList.OrderID > 0) ? objList.OrderID.ToString() : "Order ID is null";
        //    var list1 = new List<Tuple<string, string>>{
        //        new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
        //        new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
        //        new Tuple<string,string>(" Order Id:", orderId)

        //        };
        //    logwrite.keyValPair = list1;
        //    CommonMethods.WriteLog(logwrite);
        //    objApiResponseData.StatusMessages = objStatus;
        //    return objApiResponseData;
        //}

        [HttpPost]
        public List<MappedMachsLocs> getMachineIdLocsList(MappedMachsLocs objList)
        {
            List<MappedMachsLocs> objMappedMachines = new List<MappedMachsLocs>();
            if (objList != null)
            {
                if (objList.OrderIDList == "")
                {
                    objMappedMachines = null;
                }
                else
                {
                    objMappedMachines = objApiOrderService.APIGetMachineIdLocsList(objList.OrderIDList);
                }
            }


            logwrite.funcName = "getMachineIdLocsList Called-----";
            string orderId = (objList != null && objList.OrderID > 0) ? objList.OrderID.ToString() : "Order ID is null";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" Order Id:", orderId)

                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objMappedMachines;
        }
        [HttpPost]
        public APIResponseData CheckFirmWareUpdateNew(MemberActivityModel objMemberActivity)
        {
            string FilePath = "";
            string CompletePath = "";
            try
            {

                IMembershipService _membershipService = null;
                _membershipService = new MembershipService();


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
        // NON token base API for Mark
        [HttpPost]
        public APIResponseData UpdateReviveEndResult(MemberActivityModel objMemberActivity)
        {
            try
            {

                IMembershipService _membershipService =  new MembershipService();
                // Get UserId from Key
                Guid _userId = Guid.Empty;

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


    }
}
