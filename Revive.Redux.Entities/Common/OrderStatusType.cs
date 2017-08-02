using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class OrderStatusType
    {
        public const string PendingApproval = "Pending Approval";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string PendingPC = "Pending PC";
        public const string InProgress = "In-Progress";
        public const string PendingTesting = "Pending Testing";
        public const string InTesting = "In-Testing";
        public const string InShipping = "In-Shipping";
        public const string Shipped = "Shipped";
        public const string Closed = "Closed";
    }
}
