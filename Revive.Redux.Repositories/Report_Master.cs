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
    
    public partial class Report_Master
    {
        public Report_Master()
        {
            this.Report_Filter_Type = new HashSet<Report_Filter_Type>();
        }
    
        public int Report_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Filters { get; set; }
        public string Fields { get; set; }
        public string Authorization { get; set; }
        public string Field_Name { get; set; }
    
        public virtual ICollection<Report_Filter_Type> Report_Filter_Type { get; set; }
    }
}
