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
    
    public partial class FAQ_Management
    {
        public int FAQ_ID { get; set; }
        public Nullable<int> Usergrp_Id { get; set; }
        public Nullable<int> Position { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Comments { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual UserType UserType { get; set; }
    }
}