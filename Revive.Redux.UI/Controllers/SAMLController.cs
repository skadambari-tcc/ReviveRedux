using ComponentSpace.SAML2;
using Revive.Redux.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
//using ComponentSpace.SAML2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Revive.Redux.UI.Controllers
{
    public class SAMLController : Controller
    {
        public const string AttributesSessionKey = "";
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                SSOLog.WriteLog(returnUrl, "1");
                return Redirect(returnUrl);

            }
            else
            {
                return RedirectToAction("Index", "SAML");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult SLOService()
        {
            // Receive the single logout request or response.
            // If a request is received then single logout is being initiated by the identity provider.
            // If a response is received then this is in response to single logout having been initiated by the service provider.
            bool isRequest = false;
            string logoutReason = null;
            string partnerIdP = null;
            string relayState = null;


            SAMLServiceProvider.ReceiveSLO(Request, out isRequest, out logoutReason, out partnerIdP, out relayState);
            //  SSOLog.WriteLog(partnerIdP.ToString() + isRequest.ToString() + logoutReason.ToString() + relayState.ToString(), "3");
            SSOLog.WriteLog(isRequest.ToString(), "Action:SLOService ");
            if (isRequest)
            {
                
                // Logout locally.
                SSOLog.WriteLog(isRequest.ToString() + "PartnerIdp:" + partnerIdP == null ? "" : partnerIdP + "RelayState:" + relayState == null ? "" : relayState, "Action:SLOService isRequest=true ");
                FormsAuthentication.SignOut();

                // Respond to the IdP-initiated SLO request indicating successful logout.
                SAMLServiceProvider.SendSLO(Response, null);
                //  RedirectToAction("SingleSignOn", "Account");
            }
            else
            {
                // SP-initiated SLO has completed.

                //RedirectToAction("SingleSignOn", "Account");

                // FormsAuthentication.RedirectToLoginPage();
                SSOLog.WriteLog(isRequest.ToString(), "Action:SLOService isRequest=false ");
                Response.Redirect("/Account/SingleSignOn");

            }

            return new EmptyResult();

        }
        public ActionResult AssertionConsumerService()
        {

            bool isInResponseTo = true;
            string partnerIdP = null;
            string userName = null;
            IDictionary<string, string> attributes = null;
            string targetUrl = null;

            try
            {
                // Receive and process the SAML assertion contained in the SAML response.
                // The SAML response is received either as part of IdP-initiated or SP-initiated SSO.
                // SSOLog.WriteLog(userName.ToString(), "Action:AssertionConsumerService");


                SAMLServiceProvider.ReceiveSSO(Request, out isInResponseTo, out partnerIdP, out userName, out attributes, out targetUrl);
                SSOLog.WriteLog("userName " + userName.ToString() + "," + "isInResponseTo " + isInResponseTo.ToString() + "," + "partnerIdP " + partnerIdP.ToString(), "Action:AssertionConsumerService");

                string AttributeKeystr = "";
                string comma = "";



                if (attributes.Count > 0)
                {
                    foreach (var item in attributes)
                    {
                        AttributeKeystr = AttributeKeystr + comma + item.Key.ToString() + ":" + item.Value.ToString();
                        comma = ",";


                    }

                    SSOLog.WriteLog(AttributeKeystr, "Action:AssertionConsumerService");



                }




                string _customerId = ConfigurationManager.AppSettings["IdpCustomerId"].ToString(); //attributes.Where(x => x.Key == "Customer_Id").Select(x => x.Value).FirstOrDefault();
                string _subsidiaryId = ConfigurationManager.AppSettings["IdpSubsidiaryId"].ToString(); //attributes.Where(x => x.Key == "Subsidiary_Id").Select(x => x.Value).FirstOrDefault();
                string _subAgentId = ConfigurationManager.AppSettings["IdpSubAgentId"].ToString();//attributes.Where(x => x.Key == "SubAgent_Id").Select(x => x.Value).FirstOrDefault();
                string _locationId = ConfigurationManager.AppSettings["IdpLocationId"].ToString();//attributes.Where(x => x.Key == "Location_Id").Select(x => x.Value).FirstOrDefault();
                string _firstName = attributes.Where(x => x.Key == "FNAME").Select(x => x.Value).FirstOrDefault();
                string _lastName = attributes.Where(x => x.Key == "LNAME").Select(x => x.Value).FirstOrDefault();

                // If no target URL is provided, provide a default.


                LoginService loginser = new LoginService();

                var flag = loginser.ValidateIDPUser(userName);

                if (flag == false)
                {


                    var isSave = SaveIDPStoreUser(userName, _firstName, _lastName, _customerId, _subsidiaryId, _subAgentId, _locationId);

                    if (isSave == true)
                    {
                        if (targetUrl == null)
                        {
                            targetUrl = "~/SAML/Index?username=" + userName + "&isSuccess=true" + "&isReduxValidated=" + "true";
                            SSOLog.WriteLog("User Created" + userName.ToString(), "Action:AssertionConsumerService");
                        }
                    }
                    else
                    {
                        targetUrl = "~/SAML/Index?username=" + userName + "&isSuccess=true" + "&isReduxValidated=" + "false";
                        SSOLog.WriteLog("User not created due to some error" + userName.ToString(), "Action:AssertionConsumerService");
                    }
                }
                else
                {
                    targetUrl = "~/SAML/Index?username=" + userName + "&isSuccess=true" + "&isReduxValidated=" + "true";
                    SSOLog.WriteLog("User Already exist in database" + userName.ToString(), "Action:AssertionConsumerService");

                }

                //

                // Login automatically using the asserted identity.
                // This example uses forms authentication. Your application can use any authentication method you choose.
                // There are no restrictions on the method of authentication.
                FormsAuthentication.SetAuthCookie(userName, false);

                // Save the attributes.
                Session[AttributesSessionKey] = attributes;

                // Redirect to the target URL.

            }
            catch (Exception ex)
            {

                SSOLog.WriteLog(ex.InnerException.Message.ToString(), "Action:AssertionConsumerService ERROR");
            }

            SSOLog.WriteLog(userName.ToString(), "Action:AssertionConsumerService ERROR");

            return RedirectToLocal(targetUrl);
        }

        public ActionResult clickData()
        {

            return new EmptyResult();
        }

        public ActionResult sessionAlive()
        {

            return new EmptyResult();
        }

        public ActionResult Logout()
        {
            // Logout locally.
            try
            {

                FormsAuthentication.SignOut();

                // Request logout at the identity provider.
                string partnerIdP = ConfigurationManager.AppSettings["PartnerIdP"].ToString();
                var hasSession = SAMLServiceProvider.IsSSO(partnerIdP);
                SSOLog.WriteLog(hasSession.ToString(), "Action:Logout ");

                if (hasSession)
                {
                    SAMLServiceProvider.InitiateSLO(Response, null, null, partnerIdP);
                    SSOLog.WriteLog(partnerIdP.ToString(), "Action:Logout hasSession=true ");
                    return new EmptyResult();
                 
                }
                else
                {
                    SSOLog.WriteLog("SP has not session information to logout from IDP ", "Action:Logout ");
                    Response.Redirect("/Account/SingleSignOn");

                }
            }
            catch (Exception ex)
            {
                SSOLog.WriteLog(ex.Message.ToString(), "Action:Logout catch block");
                Response.Redirect("/Account/SingleSignOn");
            }
            return new EmptyResult();
        }



        #region
        private bool SaveIDPStoreUser(string EmailId, string FirstName, string LastName, string CustomerId, string SubsidiaryId, string SubAgentId, string LocationId)
        {
            UserModels model = new UserModels();
            IGeneralService _generalService = new GeneralService();
            IUserManagmentService _ManageUserService = new UserManagementService();
            model.User_Level_Id = _generalService.GetUserLevelId(PageAccessCode.STOREUSER);
            var result = false;
            SSOLog.WriteLog(EmailId, CustomerId.ToString());
            if (model.User_Level_Id != 0)
            {


                ModelState.Remove("Manufacturer_Id");
                ModelState.Remove("emailEdit");
                ModelState.Remove("UserId");
                ModelState.Remove("Status");
                try
                {
                    model.Status = true;
                    model.Created_by = null;
                    model.FirstName = FirstName;
                    model.LastName = LastName;
                    model.Email = EmailId;
                    model.Customer_Id = Convert.ToInt32(CustomerId);
                    model.SubsidiaryId = Convert.ToInt32(SubsidiaryId);
                    model.SubAgentId = Convert.ToInt32(SubAgentId);
                    model.Location_Id = Convert.ToInt32(LocationId);


                    result = _ManageUserService.AddEditUser(model);
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }


            return true;

        }
        #endregion


    }
}