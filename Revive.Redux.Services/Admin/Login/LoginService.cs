using Revive.Redux.Repositories.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;
using Revive.Redux.Common;
namespace Revive.Redux.Services
{
    public class LoginService : ILoginService
    {
        private ILoginRepository loginRepo = null;
        public LoginService()
        {
            loginRepo = new LoginRepository();
            //SendRelianceEmail sendReliance = new SendRelianceEmail();
            // sendReliance.SendEmail("smangla@svam.com",string.Empty,"THis is Test Email","this is Test  body");
        }

        public bool ValidateUser(string userName, string password)
        {
            return loginRepo.ValidateUser(userName, password);
        }

        public bool CheckEmailAddressExists(String emailAddress)
        {
            return loginRepo.CheckEmailAddressExists(emailAddress);
        }

        public DtoUser GetUserDetails(String userName)
        {
            return loginRepo.GetUserDetails(userName);
        }
        public Boolean ChangePassword(String email, String newPassword, string oldPassword)
        {
            return loginRepo.ChangePassword(email, newPassword, oldPassword);
        }

        public bool CheckOldPassword(String email, String newPassword, String oldPassword)
        {
            return loginRepo.CheckOldPassword(email, newPassword, oldPassword);
        }


        public bool ValidateUser(string userId)
        {
            return loginRepo.ValidateUser(userId);
        }
        public Boolean LockUser(String emailAddress)
        {
            return loginRepo.LockUser(emailAddress);
        }
        public Boolean ValidateLockUser(String emailAddress)
        {
            return loginRepo.ValidateLockUser(emailAddress);
        }
        public Boolean CheckDuplicateEmailByUserId(String emailAddress, Guid userId)
        {
            return loginRepo.CheckDuplicateEmailByUserId(emailAddress, userId);
        }
        public int GetUserLevelIdForStoreUser()
        {
            return loginRepo.GetUserLevelIdForStoreUser();
        }
        public bool ValidateStoreUserByUserName(string userName)
        {
            bool result = false;

            var userDetails = loginRepo.GetUserDetails(userName);
            if (userDetails.PageAccessCode == PageAccessCode.STOREUSER)
            {
                result = true;
            }
            return result;

        }
        public Boolean UpdatePasswordByUserID(UserModels ObjUserModel)
        {

            return loginRepo.UpdatePasswordByUserID(ObjUserModel);
        }

        public bool ValidateIDPUser(String userName)
        {

            return loginRepo.ValidateIDPUser(userName);
        }
    }
}
