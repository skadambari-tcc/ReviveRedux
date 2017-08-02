using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class DtoUser
    {
        public string UserName { get; set; }
        public System.Guid User_ID { get; set; }
        public int User_Level_ID { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email_ID { get; set; }
        public string Office_Phone { get; set; }
        public string Office_phone_etx { get; set; }
        public string Cell_Phone { get; set; }
        public string Fax { get; set; }
        public string Location_Name { get; set; }
        public Nullable<int> Customer_ID { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public Nullable<int> Deactivate_Flag { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<bool> status { get; set; }
        public bool IsLockedOut { get; set; }
        public Nullable<System.DateTime> LastPasswordChangeDate { get; set; }
        public string User_Level_Name { get; set; }
        public IEnumerable<DtoList> ListUserLevel { get; set; }
        public string UserType { get; set; }
        public int userTypeId { get; set; }
        public string PageAccessCode { get; set; }
        public DateTime? LastLoginActivity { get; set; }
        public int? Manufacturer_Id { get; set; }
        public int? SubsidiaryId { get; set; }
        public int? SubAgentId { get; set; }
        public string Manufacturer_Ref_Id { get; set; }
        public bool IsUserLogging { get; set; }
        public string UserLogging { get; set; }
        public int Count { get; set; }
        public bool IsStoreUserLog { get; set; }
        public string LoggingTypeCode { get; set; }
        public string CustomerName { get; set; }
        public string SubAgentName { get; set; }
        public int? LogTypeId { get; set; }
        public string SubsidiaryName { get; set; }
        public bool IsMultiDeviceSupported { get; set; }
        public int  CustomerMaxDevices { get; set; }
    }
}
