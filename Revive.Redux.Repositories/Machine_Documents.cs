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
    
    public partial class Machine_Documents
    {
        public int Machine_Doc_ID { get; set; }
        public Nullable<int> MachineHistory_ID { get; set; }
        public int Machine_ID { get; set; }
        public string Machine_Doc_Name { get; set; }
        public string Machine_Doc_type { get; set; }
        public string Doc_Path { get; set; }
        public string Comments { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual MachineHistory MachineHistory { get; set; }
    }
}
