using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Revive.Redux.Entities
{
    public class ShipMachineModel
    {

        public int MachineId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string MachineId_Val { get; set; }

        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public int MachineCount { get; set; }
        public string Status { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public DateTime? BeginShippingDate { get; set; }
        public int LastMachineId { get; set; }
        public string LastMachineId_Val { get; set; }
        public string ShipmentId { get; set; }
        public string ResultMsg { get; set; }
        public string ImageData { get; set; }
        public int Location_Id { get; set; }
        public string LocationName { get; set; }
        public int NoOfMachinesOrdered { get; set; }
        public int TotalShipped { get; set; }
        public int TotalReadyToShip { get; set; }
        public int JobOrderLocationId { get; set; }
        public int JobOrder_Header_Id { get; set; }
        public string SubsidiaryName { get; set; }
        public string SubAgentName  { get; set; }
        public string LocationAddress { get; set; }
        public string City_Name { get; set; }
        public string  ZipCode { get; set; }
        public string StateCode { get; set; }
        public string LocationPhone { get; set; }
        public int TotalMachineMapped  { get; set; }
        public int? Grp_No_of_Machines { get; set; }
        public int? Grp_TotReadyToShip { get; set; }
        public int? Grp_TotalShipped { get; set; }
        public string PriorityName { get; set; }
        public string LocationAddress2 { get; set; }
        public string ServiceCode { get; set; }
     

    }
    
}
