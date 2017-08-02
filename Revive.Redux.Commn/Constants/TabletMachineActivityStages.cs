using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Commn
{
    public  class TabletMachineActivityStages
    {
        public const string ReviveInitiateCheckin = "ReviveInitiateCheckin";
        public const string ReviveStartResult = "ReviveStartResult";
        public const string ReviveCheckOut = "ReviveCheckOut";
        public const string ReviveMachineFault = "ReviveMachineFault";
        public const string ReviveProcessfaults = "ReviveProcessfaults";
        public const string ReviveEquipmentfaults = "ReviveEquipmentfaults";
        public const string ReviveBoardfaults = "ReviveBoardfaults";
        public const string ReviveEndResult = "ReviveEndResult";
        public const string ReviveDebugParams = "ReviveDebugParams";
        public const string ReviveDefaultParams = "ReviveDefaultParams";
        public const string ReviveSuccessToStart = "ReviveSuccessToStart";
        public const string ReviveFailedToStart = "ReviveFailedToStart";
        public const string ReviveStopedWithFault = "ReviveStopedWithFault";


    }

   
}
