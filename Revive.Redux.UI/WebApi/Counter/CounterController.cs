using Revive.Redux.Common;
using Revive.Redux.Entities;
using Revive.Redux.Entities.Common;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Revive.Redux.UI.WebApi.Counter
{
    public class CounterController : ApiController
    {
        IGeneralService generalService = null;
        ReturnStatusCode objStatus = null;
        APIResponseData objApiResponseData = null;
        RepositoryResult objRepositoryResult = null;
        WriteLog logwrite = null;
        public CounterController()
        {
            generalService = new GeneralService();
            objStatus = new ReturnStatusCode();
            objApiResponseData = new APIResponseData();
            objRepositoryResult = new RepositoryResult();
            logwrite = new WriteLog();
        }
        /// <summary>
        /// CR011 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIResponseData IncValue()
        {

            try
            {
                objApiResponseData.responseData = generalService.IncValue();
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;


            }
            catch (Exception ex)
            {
                objStatus.StatusMessages = ex.Message.ToString();
            }

            logwrite.funcName = "IncValue Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" ResultVal:",Convert.ToString(objApiResponseData.responseData))
               
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// CR011
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIResponseData GetValue()
        {

            try
            {
                objApiResponseData.responseData = generalService.GetValue();
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;


            }
            catch (Exception ex)
            {
                objStatus.StatusMessages = ex.Message.ToString();
            }

            logwrite.funcName = "GetVal Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" ResultVal:", Convert.ToString(objApiResponseData.responseData))
               
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// CR011
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIResponseData ResetValue()
        {

            try
            {
                objApiResponseData.responseData = generalService.ResetValue();
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
            }
            catch (Exception ex)
            {
                objStatus.StatusMessages = ex.Message.ToString();
            }

            logwrite.funcName = "ResetValue Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                new Tuple<string,string>(" Status Messages:", objStatus.StatusMessages),
                new Tuple<string,string>(" ResultVal:", Convert.ToString(objApiResponseData.responseData))
               
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public APIResponseData TestApiPara(TestAPI objTestAPI)
        {

            try
            {
                objApiResponseData.responseData = generalService.ResetValue();
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
            }
            catch (Exception ex)
            {
                objStatus.StatusMessages = ex.InnerException.Message.ToString();
            }

            logwrite.funcName = "TestApiNoPara Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                 new Tuple<string,string>(" New log data:", objTestAPI.TestData==null?"":objTestAPI.TestData+System.DateTime.Now.ToShortDateString()),
                 new Tuple<string,string>(" Status messages:", objStatus.StatusMessages),
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

        [HttpPost]
        public APIResponseData TestApiNoPara()
        {

            try
            {
                objApiResponseData.responseData = generalService.ResetValue();
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
            }
            catch (Exception ex)
            {
                objStatus.StatusMessages = ex.InnerException.Message.ToString();
            }

            logwrite.funcName = "TestApiNoPara Called-----";
            var list1 = new List<Tuple<string, string>>{
                new Tuple<string,string>(" Url:", Convert.ToString(Request.Headers.Host)),
                 new Tuple<string,string>(" New log data:", "Test Data --------"+System.DateTime.Now.ToShortDateString()),
                 new Tuple<string,string>(" Status messages:", objStatus.StatusMessages),
                };
            logwrite.keyValPair = list1;
            CommonMethods.WriteLog(logwrite);
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }

      


    }


}

public class TestAPI
{
    public string TestData { get; set; }

}
