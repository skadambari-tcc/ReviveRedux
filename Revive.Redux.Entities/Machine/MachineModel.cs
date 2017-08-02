using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MachineModel : PageBasic
    {
        public Nullable<int> MachineMappingId { get; set; }
        public string MachineMappingIdEncoded { get; set; }
        public Nullable<int> JobOrderHeaderID { get; set; }
        public string JobOrderHeaderIdEncoded { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string MachineId_Val { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string Location { get; set; }

        public Nullable<int> MidLineStageId { get; set; }
        public string MidLineStageName { get; set; }
        public string MidLineTest { get; set; }
        public Nullable<bool> MidLineTestStatus { get; set; }

        public Nullable<int> ULStageId { get; set; }
        public string ULStageName { get; set; }
        public string ULTest { get; set; }
        public Nullable<bool> ULTestStatus { get; set; }

        public Nullable<int> FinalStageId { get; set; }
        public string FinalStageName { get; set; }
        public string FinalTest { get; set; }
        public Nullable<bool> FinalTestStatus { get; set; }

        [RegularExpression("[A-Za-z0-9-]+", ErrorMessage = "Alphanumeric only")]
        [StringLength(50, ErrorMessage = "Alphanumeric only(Max length 50)", MinimumLength = 1)]
        [Required(ErrorMessage = "Required")]
        public string ShippingId { get; set; }
        // Added by KD on dated 13-04-2015 Adding Customer Id and Customer Name
        public int Customer_Id { get; set; }
        public int Location_Id { get; set; }
        public string CustomerName { get; set; }
        public string SoftwareVersion { get; set; }
        public int CurrentVersion_Id { get; set; }
        public bool isSelected { get; set; }
       
        public Nullable<int> NoOfMachines { get; set; }

        // Used for Machine Status Name - Pending-Testing / In-Testing / In-Shipping / Shipped
        public string MachineStatusName { get; set; }
        public Nullable<int> MachineStatusId { get; set; }

        public bool isChecked { get; set; }

        private string _sActivityType;

        public string ActivityType
        {
            get { return _sActivityType; }
            set { _sActivityType = value; }
        }

        private int _nActivityTypeId;

        public int ActivityTypeId
        {
            get { return _nActivityTypeId; }
            set { _nActivityTypeId = value; }
        }

        private string _sStatus;

        public string Status
        {
            get { return _sStatus; }
            set { _sStatus = value; }
        }

        private int _nJobOrderMachineMappingID;

        public int JobOrderMachineMappingID
        {
            get { return _nJobOrderMachineMappingID; }
            set { _nJobOrderMachineMappingID = value; }
        }

        private bool _bResult;

        public bool Result
        {
            get { return _bResult; }
            set { _bResult = value; }
        }
        public int Proposedversion_id { get; set; }
        public string ProposedVersion { get; set; }
        public int? JobOrderLocationId { get; set; }


        //Subsidiary Changes
        public int SubsidiaryId { get; set; }
        public int SubAgentId { get; set; }

        public List<MachinResultModel> MachineResult { get; set; }

        //Upload Shipping Details
        public string FileName { get; set; }  // file Name
        public int JobOrder_Id { get; set; }  // Job Order Id
        public int JobOrder_Status { get; set; }  // Job Order Status
        public int Machine_Mapping_Id { get; set; }  // Machine Mapping Id

        public string strJobOrderHeaderID { get; set; }
    }

    public class MachinResultModel
    {
        public string LineNumber { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
}
