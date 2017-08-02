using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class GroupedLocationForShip
    {
        public int Location_ID { get; set; }
        public int Customer_ID { get; set; }
        public string CustomerName { get; set; }
        public string StoreNumber { get; set; }
        public string Location_Name { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public int ShippingStatusId { get; set; }
        public string ShippingStatusName { get; set; }
        public bool IsSelected { get; set; }
        public string  SubsidiaryName { get; set; }
        public string SubAgentName { get; set; }
        public bool IsShipped { get; set; }
        public DateTime ShippingDate { get; set; }
        

        [DefaultValue(1)]
        public int NoofMachines { get; set; }
    }
}
