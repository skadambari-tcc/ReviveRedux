using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Revive.Redux.Services;
using System.Collections.Specialized;
using Revive.Redux.Entities;


namespace Revive.Redux.Controllers.Common
{
    public class CustomMemberShip : MembershipProvider
    {
        ILoginService loginValidation = null;

        private string _ApplicationName;
        private bool _EnablePasswordReset;
        private bool _EnablePasswordRetrieval = false;
        private bool _RequiresQuestionAndAnswer = false;
        private bool _RequiresUniqueEmail = true;
        private int _MaxInvalidPasswordAttempts;
        private int _PasswordAttemptWindow;
        private int _MinRequiredPasswordLength;
        private int _MinRequiredNonalphanumericCharacters;
        private string _PasswordStrengthRegularExpression;
        private MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Membership Provider");
            }

            base.Initialize(name, config);

            _ApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _MaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _PasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            _MinRequiredNonalphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _MinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _EnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _PasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));

        }


        public CustomMemberShip()
        {
            loginValidation = new LoginService();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return loginValidation.ChangePassword(username, newPassword, oldPassword);
        }



        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _MaxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            return loginValidation.ValidateUser(username, password);
        }

        
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public DtoUser GetUserDetails(String email)
        {
            return loginValidation.GetUserDetails(email);
        }

        public Boolean CheckEmailAddressExists(String emailAddress)
        {
            return loginValidation.CheckEmailAddressExists(emailAddress);
        }
        public Boolean CheckDuplicateEmailByUserId(String emailAddress, Guid userId)
        {
            return loginValidation.CheckDuplicateEmailByUserId(emailAddress, userId);
        }
        public Boolean ValidateLockUser(String emailAddress)
        {
            return loginValidation.ValidateLockUser(emailAddress);
        }
        public Boolean LockUser(String emailAddress)
        {
            return loginValidation.LockUser(emailAddress);
        }

        public bool ValidateStoreUserByUserName(string userName)
        {
            return loginValidation.ValidateStoreUserByUserName(userName);
        }
    }
    public class CurrentUserDetail
    {
        public Guid User_Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email_Id { get; set; }
        public String FullName { get; set; }
        public String Department { get; set; }
        public String CompanyName { get; set; }
        public Boolean IsCustomer { get; set; }
        public String SalesPersonEmail { get; set; }
        public String OrderContactPersonEmail { get; set; }
        public int TierId { get; set; }
        public int UserLevelId { get; set; }
        public int? Customer_Id { get; set; }
        public int? Location_Id { get; set; }
        public string PageAccessCode { get; set; }
        public bool isChangePassword { get; set; }
        public string Key { get; set; }
        public DateTime? LastLoginActivity { get; set; }
        public string userLevelName { get; set; }
        public int? Manufacturer_Id { get; set; }
        public string userType { get; set; }
        public int? SubsidiaryId { get; set; }
        public int? SubAgentId { get; set; }
        public string Manufacturer_Ref_Id { get; set; }
        public bool IsLoggingAllowed { get; set; }
        public string LoggingTypeCode { get; set; }
        public bool IsMultiDeviceSupported { get; set; }
        public int CustomerNoOfDevices { get; set; }

    }
}