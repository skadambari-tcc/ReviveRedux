using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class OrderLocationsModel
    {
        public Nullable<int> JobOrderLocationId { get; set; }
        public int JobOrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int LocationId { get; set; }         // ID of location
        public string LocationName { get; set; }
        public int? Priority { get; set; }
        public int? NumberOfMachines { get; set; }
        public int? NumberOfMachinesMapped { get; set; }
        public int? TotalShipped_Inshpping { get; set; }

        public bool IsChecked { get; set; }
        public bool IsMapped { get; set; }
        //public string CallFrom { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        //Subsidiary Changes
        public string SubsidiaryName { get; set; }
        public int SubsidiaryID { get; set; }
        public string SubAgentName { get; set; }
        public int SubAgentID { get; set; }

    }
}
