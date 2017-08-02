using System;
using Revive.Redux.Repositories;
using Revive.Redux.Entities;

namespace Revive.Redux.Repositories.Login
{
    public interface ILoginRepository
    {
        Boolean ValidateUser(String userName, String password);

        DtoUser GetUserDetails(String userName);

        Boolean ChangePassword(String email, String newPassword, string oldPassword);

        Boolean CheckEmailAddressExists(String emailAddress);

        Boolean CheckOldPassword(String email, String newPassword, String oldPassword);

        Boolean ValidateUser(String userId);

        Boolean LockUser(String emailAddress);

        Boolean ValidateLockUser(String emailAddress);

        Boolean CheckDuplicateEmailByUserId(String emailAddress,Guid userId);
        int GetUserLevelIdForStoreUser();
        Boolean UpdatePasswordByUserID(UserModels ObjUserModel);
        bool ValidateIDPUser(String userName);

    }
}
