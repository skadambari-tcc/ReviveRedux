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
    
    public partial class Customers_Locations
    {
        public Customers_Locations()
        {
            this.JobOrder_Locations = new HashSet<JobOrder_Locations>();
            this.MapLocationByGroups = new HashSet<MapLocationByGroup>();
            this.LocationMapMachines = new HashSet<LocationMapMachine>();
        }
    
        public int Location_ID { get; set; }
        public int Customer_ID { get; set; }
        public string StoreNumber { get; set; }
        public string Location_Name { get; set; }
        public string Address { get; set; }
        public string Additional_Address_info { get; set; }
        public Nullable<int> City { get; set; }
        public string City_Name { get; set; }
        public string Area { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<int> Country { get; set; }
        public string ZipCode { get; set; }
        public string Email_ID { get; set; }
        public string Phone { get; set; }
        public string Phone_Ext { get; set; }
        public string SToreOpentime { get; set; }
        public string SToreClosetime { get; set; }
        public string Customfield1 { get; set; }
        public string Customfield2 { get; set; }
        public string Customfield3 { get; set; }
        public string Customfield4 { get; set; }
        public string Customfield5 { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public bool Status { get; set; }
        public Nullable<int> Subsidiary_ID { get; set; }
        public Nullable<int> SubAgent_ID { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> ShippingStatusId { get; set; }
    
        public virtual Subsidiary Subsidiary { get; set; }
        public virtual ICollection<JobOrder_Locations> JobOrder_Locations { get; set; }
        public virtual SubAgent SubAgent { get; set; }
        public virtual ICollection<MapLocationByGroup> MapLocationByGroups { get; set; }
        public virtual LocationGroup LocationGroup { get; set; }
        public virtual ICollection<LocationMapMachine> LocationMapMachines { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
