//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Revive.Redux.Repositories
{
    using System;
    
    public partial class usp_GetUserDetails_Result
    {
        public string UserName { get; set; }
        public string MobileCode { get; set; }
        public string Email { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        public int User_level_id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserLevel { get; set; }
        public System.Guid User_ID { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public bool status { get; set; }
        public Nullable<int> Deactivate_Flag { get; set; }
        public string UserType { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Manufacturer_Id { get; set; }
        public string Manufacturer_Ref_Code { get; set; }
        public bool IsUserLogging { get; set; }
        public int LoggingTypeId { get; set; }
        public string SubAgentName { get; set; }
        public bool IsMultiDevice { get; set; }
    }
}
