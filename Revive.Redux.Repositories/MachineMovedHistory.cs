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
    using System.Collections.Generic;
    
    public partial class MachineMovedHistory
    {
        public int Id { get; set; }
        public Nullable<int> JobOrderHeaderId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public Nullable<int> JobOrderMachineMappingId { get; set; }
        public Nullable<int> CustomerIdFrom { get; set; }
        public Nullable<int> SubsidiaryIdFrom { get; set; }
        public Nullable<int> AgentIdFrom { get; set; }
        public Nullable<int> LocationIdFrom { get; set; }
        public Nullable<int> CustomerIdTo { get; set; }
        public Nullable<int> SubsidiaryIdTo { get; set; }
        public Nullable<int> AgentIdTo { get; set; }
        public Nullable<int> LocationIdTo { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> transactionTypeId { get; set; }
    }
}
