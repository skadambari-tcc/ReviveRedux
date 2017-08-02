using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class AppMachineHistoryModel
    {
        public string CustomerName { get; set; }
        public string IsTempValue { get; set; }
        public string LengthOfDry { get; set; }  //Seconds
        public string WaterRemoved { get; set; }
        public string Results { get; set; }
        public string ActivityDate { get; set; }
        public string ActivityTime { get; set; }
        public string InvoiceNo { get; set; }
        public string ProcessId { get; set; }
        public string MemberEmailId { get; set; }
        public string MemberLocation  { get; set; }
        public string MemberPhone { get; set; }
        public string LengthOfDryLong { get; set; } //20:30
        public string LengthOfDryDesc { get; set; } // 20 minutes,10 seconds
        public string ReviveAnswer { get; set; }
    }
}
