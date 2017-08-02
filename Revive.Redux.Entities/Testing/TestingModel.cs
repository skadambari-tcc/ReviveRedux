using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class TestingModel
    {
        //public Nullable<int> MachineTestingId { get; set; }
        public Nullable<int> MidLineTestingRowId { get; set; }
        public Nullable<int> ULTestingRowId { get; set; }
        public Nullable<int> FinalTestingRowId { get; set; }

        public Nullable<int> MachineMappingId { get; set; }
        public string MachineMappingIdEncoded { get; set; }
        public List<DtoList> TestingStages { get; set; }

        public Nullable<int> MidLineStageId { get; set; }
        public Nullable<int> ULStageId { get; set; }
        public Nullable<int> FinalStageId { get; set; }

        public Nullable<int> MidLineTestStatus { get; set; }
        public Nullable<int> ULTestStatus { get; set; }
        public Nullable<int> FinalTestStatus { get; set; }

        #region Mid-Line Testing

        public Nullable<bool> PlatenTime { get; set; }
        public Nullable<int> PlatenSeconds { get; set; }
        public Nullable<bool> PlatenSensor { get; set; }
        public Nullable<decimal> PlatenDelta { get; set; }

        public Nullable<bool> InjectionTime { get; set; }
        public Nullable<int> InjectionSeconds { get; set; }
        public Nullable<bool> InjectionSensor { get; set; }
        public Nullable<decimal> InjectionDelta { get; set; }

        public Nullable<bool> VaccumTime { get; set; }
        public Nullable<int> VaccumSeconds { get; set; }
        public Nullable<bool> VaccumSensor { get; set; }
        public Nullable<decimal> VaccumDelta { get; set; }

        public Nullable<bool> USBInternalSensor { get; set; }
        public Nullable<decimal> USBInternalDelta { get; set; }
        public Nullable<bool> USBExternalSensor { get; set; }
        public Nullable<decimal> USBExternalDelta { get; set; }

        public Nullable<bool> RelativeSensor { get; set; }
        public Nullable<decimal> RelativeDelta { get; set; }
        public Nullable<bool> DryingProcess { get; set; }
        public Nullable<int> DryingCycles { get; set; }

        public string Test1Name { get; set; }
        public Nullable<bool> Test1Verified { get; set; }
        public string Test2Name { get; set; }
        public Nullable<bool> Test2Verified { get; set; }
        public string Test3Name { get; set; }
        public Nullable<bool> Test3Verified { get; set; }

        #endregion

        #region UL Requirements Testing

        public Nullable<bool> DielectricTest { get; set; }
        public Nullable<int> DielectricVoltage { get; set; }
        public Nullable<decimal> DielectricCurrent { get; set; }
        public Nullable<int> DielectricTime { get; set; }
        public Nullable<bool> GroundTest { get; set; }
        public Nullable<decimal> GroundResistance { get; set; }

        #endregion

        #region Final Testing

        public Nullable<bool> FinalAssembly { get; set; }
        public string FinalProcessId { get; set; }

        #endregion

        // Can be -> Pending / Mid-Line Testing Passed / UL Requirements Testing Passed / Final Assembly Testing Passed
        public string MachineTestingStatus { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
