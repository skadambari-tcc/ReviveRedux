using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;
namespace Revive.Redux.Services
{
    public interface ILoginService
    {
        Boolean ValidateUser(String userName, String password);
        Boolean CheckEmailAddressExists(String emailAddress);
        DtoUser GetUserDetails(String userName);
        Boolean ChangePassword(String email, String newPassword, string oldPassword);
        Boolean CheckOldPassword(String email, String newPassword, String oldPassword);
        bool ValidateUser(String userId);
        Boolean LockUser(String emailAddress);
        Boolean ValidateLockUser(String emailAddress);
        Boolean CheckDuplicateEmailByUserId(String emailAddress,Guid userId);
        int GetUserLevelIdForStoreUser();
        bool ValidateStoreUserByUserName(string userName);
        Boolean UpdatePasswordByUserID(UserModels ObjUserModel);
        bool ValidateIDPUser(String userName);

    }

}
