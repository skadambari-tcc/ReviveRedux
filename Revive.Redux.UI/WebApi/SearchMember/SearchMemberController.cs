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

namespace Revive.Redux.UI.WebApi.SearchMember
{
    public class SearchMemberController : ApiController
    {
        IMembershipService _membershipService = null;
        ReturnStatusCode objStatus = null;
        APIResponseData objApiResponseData = null;
        RepositoryResult objRepositoryResult = null;
        WriteLog logwrite = null;



        public SearchMemberController()
        {
            _membershipService = new MembershipService();
            objStatus = new ReturnStatusCode();
            objApiResponseData = new APIResponseData();
            objRepositoryResult = new RepositoryResult();
        }

        #region Client Specific API

        [ActionName("SearchMemberByCustomerId")]
        [HttpPost]
        public APIResponseData SearchMemberByCustomerId(MembersShipModel objMembershipModel)
        {
            var ClientAuthKey = GetCustomerRefCodeByKey();
            var validateKey = _membershipService.IsAPIAccessByClient(ClientAuthKey.CustRefCode, ClientAuthKey.Key);

            objMembershipModel.CustomerRefCode = ClientAuthKey.CustRefCode;
            objMembershipModel.APIKey = ClientAuthKey.Key;
            IEnumerable<MemberShipDetailsModel> result = null;
            if (validateKey == true)
            {
                result = _membershipService.SearchMemberByCustomerId(objMembershipModel);

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

            }
            else
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.NoAuthorized;
                objApiResponseData.responseData = APIStatusValues.NoAuthorized;
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }


        #endregion



        /// <summary>
        /// GetReviveData Return values Customer Name ,Process Date,Device type & Status as passing phone parameter CR 20.
        /// </summary>
        /// <param name="objMembershipModel"></param>
        /// <returns></returns>
        public APIResponseData GetReviveData(MembersShipModel objMembershipModel)
        {
            var ClientAuthKey = GetCustomerRefCodeByKey();
            var validateKey = _membershipService.IsAPIAccessByClient(ClientAuthKey.CustRefCode, ClientAuthKey.Key);

            objMembershipModel.CustomerRefCode = ClientAuthKey.CustRefCode;
            objMembershipModel.APIKey = ClientAuthKey.Key;


            try
            {
                IEnumerable<ReviveDataByPhoneParam> result = null;
                if (validateKey == true)
                {
                    result = _membershipService.GetReviveData(objMembershipModel);
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
                        objApiResponseData.responseData = "No Data Found";
                    }

                }
                else
                {
                    objStatus.StatusId = APIStatusValues.FailureId;
                    objStatus.StatusMessages = APIStatusValues.NoAuthorized;
                    objApiResponseData.responseData = APIStatusValues.NotAuthorized;
                }


                objApiResponseData.StatusMessages = objStatus;
            }
            catch (Exception ex)
            {
                objStatus.StatusId = APIStatusValues.FailureId;
                objStatus.StatusMessages = APIStatusValues.StatusFailure;
                objApiResponseData.responseData = ex.Message.ToString();
            }
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }


        [ActionName("EncryptData")] // one type before deployment of encryption and decryption
        [HttpPost]
        public APIResponseData EncryptData(EncryptKey objAuthKey)
        {


            var result = _membershipService.UpdateTabletNonMemberFieldAsEncryption(objAuthKey.authKeyName);


            if (result=="")
            {
                objStatus.StatusId = APIStatusValues.SuccessId;
                objStatus.StatusMessages = APIStatusValues.StatusSuccess;
                objApiResponseData.responseData = result;//_membershipService.GetMembmershipDetails(objMembershipModel);
            }
            else
            {
                objStatus.StatusId = APIStatusValues.FailureIdmsgs;
                objStatus.StatusMessages = result;
                objApiResponseData.responseData = APIStatusValues.NoDataFound;
            }


         
           
            objApiResponseData.StatusMessages = objStatus;
            return objApiResponseData;
        }




        private CustomerAPIKey GetCustomerRefCodeByKey()
        {
            CustomerAPIKey ObjKey = new CustomerAPIKey();
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            object keyVal = headerValues.FirstOrDefault();
            string _key = keyVal.ToString();//.Split(' ')[1].ToString();

            if (_key != null)
            {
                string[] _params = _key.Split('#');
                ObjKey.CustRefCode = _params[0];
                ObjKey.Key = new Guid(_params[1].ToString());
            }
            return ObjKey;

        }
        private class CustomerAPIKey
        {
            public Guid Key { get; set; }
            public string CustRefCode { get; set; }
        }

        public class EncryptKey
        {
            public string authKeyName { get; set; }
          
        }




    }
}
