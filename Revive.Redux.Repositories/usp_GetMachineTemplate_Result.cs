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
    
    public partial class usp_GetMachineTemplate_Result
    {
        public int CustomerId { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string Location_Name { get; set; }
        public Nullable<int> MachineId { get; set; }
        public Nullable<int> TempalteId { get; set; }
        public string Template_name { get; set; }
        public string Template_VersionNumber { get; set; }
        public string Comments { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string machineId_Val { get; set; }
    }
}
