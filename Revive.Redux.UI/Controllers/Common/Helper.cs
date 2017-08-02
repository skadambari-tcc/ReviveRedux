using Revive.Redux.Common;
using Revive.Redux.Entities;
using Revive.Redux.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revive.Redux.UI
{
    public class Helper
    {

        #region Private Method

        //private IGeneralService _IGeneralService = null;

        #endregion

        #region constructor
        public Helper()
        {
            
        }

        #endregion


        /// <summary>
        /// Get Group Email Address using group name
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetEmailAddressByGroup(string group)
        {
            string emailAddr = string.Empty;
            IGeneralService _IGeneralService = new GeneralService();
            IEnumerable<UserModels> users = _IGeneralService.GetUserByPageAccessCode(group);
            if (users != null)
            {
                foreach (UserModels user in users)
                    emailAddr += user.Email + ConstantEntities.EmailSeparator;
            }
            return emailAddr;
        }
        public static string GetEmailAddressByGroup(string group,int id)
        {
            string emailAddr = string.Empty;
            IGeneralService _IGeneralService = new GeneralService();
            IEnumerable<UserModels> users = _IGeneralService.GetUserByPageAccessCode(group, id);
            if (users != null)
            {
                foreach (UserModels user in users)
                    emailAddr += user.Email + ConstantEntities.EmailSeparator;
            }
            return emailAddr;
        }
        public static UserModels GetAMbyCustId(string group, int id)
        {
            IGeneralService _IGeneralService = new GeneralService();
            UserModels user = _IGeneralService.GetUserByPageAccessCode(group, id).FirstOrDefault();
            return user;
        }
    }
}