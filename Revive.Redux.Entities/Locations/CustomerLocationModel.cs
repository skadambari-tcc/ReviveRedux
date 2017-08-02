using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class CustomerLocationModel
    {
        public int Location_ID { get; set; }
        public int Customer_ID { get; set; }
        public string CustomerName { get; set; }
        public string StoreNumber { get; set; }
        public string Location_Name { get; set; }
        public string Address { get; set; }
        public string Additional_Address_info { get; set; }
        public Nullable<int> City { get; set; }
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
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string Successmsg { get; set; }
        public string ErrorMsgs { get; set; }
        public string strCreated_Date { get; set; }
        public string SubAgentName { get; set; }
        public int SubsidiaryId { get; set; }
        public int SubAgentId { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public int ShippingStatusId { get; set; }
        public string ShippingStatusName { get; set; }
        public bool IsSelected { get; set; }        
    }
    public class LocationList
    {
        public int LocationId { get; set; }
        public int NoofMachines { get; set; }
        public int ShippingStatusId { get; set; }
    }
}
