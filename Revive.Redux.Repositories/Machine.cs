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
    
    public partial class Machine
    {
        public Machine()
        {
            this.Machine_activity_detail = new HashSet<Machine_activity_detail>();
            this.Tablet_App_Machine_Activity_Details = new HashSet<Tablet_App_Machine_Activity_Details>();
            this.LocationMapMachines = new HashSet<LocationMapMachine>();
        }
    
        public int Machine_Id { get; set; }
        public Nullable<bool> IsAssigned { get; set; }
        public string Machine_Id_Val { get; set; }
        public Nullable<int> Mode_id { get; set; }
        public Nullable<bool> IsTested { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> Softwareversion_Id { get; set; }
        public Nullable<int> Proposedversion_id { get; set; }
        public Nullable<bool> IsShipped { get; set; }
        public Nullable<bool> IsMove { get; set; }
        public Nullable<System.Guid> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<System.DateTime> ShippedDate { get; set; }
        public Nullable<System.DateTime> ShippingDate { get; set; }
    
        public virtual ICollection<Machine_activity_detail> Machine_activity_detail { get; set; }
        public virtual MasterData_Config_definitions MasterData_Config_definitions { get; set; }
        public virtual ICollection<Tablet_App_Machine_Activity_Details> Tablet_App_Machine_Activity_Details { get; set; }
        public virtual ICollection<LocationMapMachine> LocationMapMachines { get; set; }
    }
}
