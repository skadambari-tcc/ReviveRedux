using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Revive.Redux.Common
{
    public class ConstantEntities
    {
        public const int SpecialAdminRoleId = 1;
        public const int CustomerRoleId = 2;
        public const string ForgotPasswordEmailSubject = "Redux LLC Website New Password";
        public const char EmailSeparator = ';';       // Used for separating multiple email addresses.
        public const string AgentTypeName = "SubAgent";
        public const int MachineIncValue = 1;
        public const int MachineGetValue = 2;
        public const int MachineResetValue = 3;
        public const string MachineConfigureHistoryTypeName = "MachineConfigureHistoryType";
        public const string MachineHistoryReason = "Reason";
        public const string MachineHistoryStatus = "Status";
        public const string MachineHistoryTransaction = "Transaction";
        public const string ShippingStatusName = "ShippingStatus";

        public const string StatuesKilled = "Killed";
        public const string StatuesMoved = "Moved";
        public const string StatuesStock = "Inventory";
        public const string StatuesRecovered = "Recovered";
        public const string StatuesReplaced = "Replaced";
        public const string MachineStatus = "MachineStatus";
        public const string GroupStatusStarted = "GroupStatusStarted";
        public const string GroupStatusCompleted = "GroupStatusCompleted";

        public const string InitialDeployment = "InitialDeploymentNotToDelete";
        public const string NonMemberEncryptionKey = "updateTabletNonMemberFieldEncryption";
        public const int  StandardTemplate=1;   // Normal template for all customer
        public const int CustomerTemplateAW=2;  // Specific customer for Wireless
            



        




        #region MemoryCache
        //public const string UserCartCount = "UserCartCount";
        //public const string CacheGetAdminConfiguration = "GetAdminConfiguration";
        //public const string CacheAdvertisementImagePath = "GetAdvertisementImagePath";
        //public const string CacheProductList = "GetProductList";
        //public const string ExcelPrefix = "ProductInfo";
        //public const string CacheBrandList = "GetBrandList";
        //public const string CacheCategoryList = "GetCategoryList";
        //public const string CacheDeviceList = "GetDeviceList";
        #endregion

    }

    public static class MembershipConfig
    {
        public static int MembershipDuration = 24;
        public static int MembershipRenewDuration = 24;
        public static int TotalReviveAllowed = 2;
        public static int TotalRenewedReviveAllowed = 2;

    }

    public static class Modes
    {
        public static int Debug = 1;
        public static int Live = 2;
        public static int Manufacturer = 3;
        public static int Demo = 4;
        public static int Training = 5;

    }
    public static class ReportNames
    {
        public static int MachineAggregateRate = 5;
        public static int MachineMembersRate = 6;
        public static int MachinePhoneManufacRate = 7;
        public static int MemberShipAggregateSummary = 23;
    }
   

}
