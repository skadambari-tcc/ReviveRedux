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
    
    public partial class Debug_Templates
    {
        public Debug_Templates()
        {
            this.Debug_Templates_Definition = new HashSet<Debug_Templates_Definition>();
        }
    
        public int Template_ID { get; set; }
        public string Template_name { get; set; }
        public string VersionNumber { get; set; }
        public Nullable<int> Current_Active_Flag { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual ICollection<Debug_Templates_Definition> Debug_Templates_Definition { get; set; }
    }
}
