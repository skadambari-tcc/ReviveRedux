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
    
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            this.JobOrder_header = new HashSet<JobOrder_header>();
            this.MachineOrders = new HashSet<MachineOrder>();
        }
    
        public int Manufacturer_Id { get; set; }
        public string Manufacturer_Name { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public string Company_Website { get; set; }
        public string Manufacturer_Ref_Code { get; set; }
        public string PM_Name { get; set; }
        public string PM_Email { get; set; }
        public string PM_Office_Phone { get; set; }
        public string PM_Mobile { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual ICollection<JobOrder_header> JobOrder_header { get; set; }
        public virtual ICollection<MachineOrder> MachineOrders { get; set; }
    }
}
