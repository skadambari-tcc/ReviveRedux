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
    
    public partial class usp_GetAllModesByModeId_Result
    {
        public int Config_Id { get; set; }
        public Nullable<int> ModeId { get; set; }
        public int Customer_Id { get; set; }
        public int Location_id { get; set; }
        public Nullable<int> Subsidiary_ID { get; set; }
        public Nullable<int> SubAgent_ID { get; set; }
        public string StoreNumber { get; set; }
        public Nullable<int> No_of_Activities { get; set; }
        public Nullable<System.DateTime> From_Date { get; set; }
        public Nullable<System.DateTime> To_Date { get; set; }
        public Nullable<bool> Status { get; set; }
        public string CustomerName { get; set; }
        public string locationName { get; set; }
        public string SubAgentName { get; set; }
        public Nullable<int> No_of_Consumed { get; set; }
    }
}
