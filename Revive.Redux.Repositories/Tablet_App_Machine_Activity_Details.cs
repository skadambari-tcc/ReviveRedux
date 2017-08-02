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
    
    public partial class Tablet_App_Machine_Activity_Details
    {
        public Tablet_App_Machine_Activity_Details()
        {
            this.Tablet_App_NonMemberActivity_Details = new HashSet<Tablet_App_NonMemberActivity_Details>();
            this.Tablet_App_Machine_Activity_Results = new HashSet<Tablet_App_Machine_Activity_Results>();
            this.Tablet_App_MemberActivity_Details = new HashSet<Tablet_App_MemberActivity_Details>();
        }
    
        public int Machine_Activity_ID { get; set; }
        public Nullable<System.DateTime> Activity_Date { get; set; }
        public Nullable<int> Activity_type_id { get; set; }
        public Nullable<int> Machine_Id { get; set; }
        public string Process_Id { get; set; }
        public int ModeId { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Mode Mode { get; set; }
        public virtual Mode Mode1 { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual ICollection<Tablet_App_NonMemberActivity_Details> Tablet_App_NonMemberActivity_Details { get; set; }
        public virtual ICollection<Tablet_App_Machine_Activity_Results> Tablet_App_Machine_Activity_Results { get; set; }
        public virtual ICollection<Tablet_App_MemberActivity_Details> Tablet_App_MemberActivity_Details { get; set; }
    }
}