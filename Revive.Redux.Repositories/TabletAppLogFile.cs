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
    
    public partial class TabletAppLogFile
    {
        public int FIle_ID { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public string File_Path { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Customer_ID { get; set; }
        public Nullable<int> Subsidiary_ID { get; set; }
        public Nullable<int> SubAgent_ID { get; set; }
    }
}