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
    
    public partial class usp_GetMachineDetailByMachMapId_Result
    {
        public int MachineMappingId { get; set; }
        public Nullable<int> JobOrder_Header_ID { get; set; }
        public Nullable<int> Noof_Machines { get; set; }
        public Nullable<int> JobOrder_Location_id { get; set; }
        public string LocationName { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string MachineId_Val { get; set; }
        public Nullable<int> MidLineStageId { get; set; }
        public string MidLineStageName { get; set; }
        public Nullable<bool> MidLineTestStatus { get; set; }
        public string MidLineTest { get; set; }
        public Nullable<int> ULStageId { get; set; }
        public string ULLineStageName { get; set; }
        public Nullable<bool> ULTestStatus { get; set; }
        public string ULTest { get; set; }
        public Nullable<int> FinalStageId { get; set; }
        public string FinalStageName { get; set; }
        public Nullable<bool> FinalTestStatus { get; set; }
        public string FinalTest { get; set; }
        public int Customer_ID { get; set; }
        public string CustomerName { get; set; }
        public string Shipping_ID { get; set; }
        public int MachineStatusId { get; set; }
        public string MachineStatusName { get; set; }
    }
}
