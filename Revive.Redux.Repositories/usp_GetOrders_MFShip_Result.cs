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
    
    public partial class usp_GetOrders_MFShip_Result
    {
        public Nullable<int> JobOrder_Header_ID { get; set; }
        public Nullable<int> Customer_ID { get; set; }
        public Nullable<int> JobOrder_Status_Id { get; set; }
        public string CustomerPoNumber { get; set; }
        public Nullable<int> Noof_Machines { get; set; }
        public Nullable<System.DateTime> Expected_Delivery_Date { get; set; }
        public Nullable<int> MachineSpec_Id { get; set; }
        public string Client_Exec_Notes_Instructions { get; set; }
        public string Approver_notes { get; set; }
        public string Download_Software_URL { get; set; }
        public Nullable<bool> Archive_flag { get; set; }
        public Nullable<System.DateTime> Approved_rejected_Date { get; set; }
        public Nullable<System.Guid> Approved_Rejected_by_Userid { get; set; }
        public string ManufacturerComments { get; set; }
        public Nullable<int> Manufacturer_Id { get; set; }
        public string Manufacturer_Name { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string Status_Name { get; set; }
        public string Customer_Name { get; set; }
    }
}
