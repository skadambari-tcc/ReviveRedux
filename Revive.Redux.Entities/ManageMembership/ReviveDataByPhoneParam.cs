using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ReviveDataByPhoneParam
    {
        public string CustomerName { get; set; }
        public DateTime ProcessDate { get; set; }
        public string  DeviceTypeName { get; set; }
        public string Status { get; set; }
        public string  ProcessId { get; set; }
        public string MobileNumber { get; set; }
        public string IsMember { get; set; }
        public string FreeAttempt { get; set; }

    }
}
