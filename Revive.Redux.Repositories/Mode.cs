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
    
    public partial class Mode
    {
        public Mode()
        {
            this.MapMode_UserType = new HashSet<MapMode_UserType>();
            this.Modes_configuration = new HashSet<Modes_configuration>();
            this.Tablet_App_Machine_Activity_Details = new HashSet<Tablet_App_Machine_Activity_Details>();
            this.Tablet_App_Machine_Activity_Details1 = new HashSet<Tablet_App_Machine_Activity_Details>();
        }
    
        public int ModeId { get; set; }
        public string ModeName { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
    
        public virtual ICollection<MapMode_UserType> MapMode_UserType { get; set; }
        public virtual ICollection<Modes_configuration> Modes_configuration { get; set; }
        public virtual ICollection<Tablet_App_Machine_Activity_Details> Tablet_App_Machine_Activity_Details { get; set; }
        public virtual ICollection<Tablet_App_Machine_Activity_Details> Tablet_App_Machine_Activity_Details1 { get; set; }
    }
}