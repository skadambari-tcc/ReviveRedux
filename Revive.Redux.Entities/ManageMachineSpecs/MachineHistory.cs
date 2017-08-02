using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class EditMachineHistory
    {
        public int MachineId { get; set; }

        [Display(Name = "Machine ID")]
        public string MachineId_Val { get; set; }

        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Last Reported Software Version")]
        public string LastReportedSoftwareVersion { get; set; }

        [Required(ErrorMessage = "Required")]
        public int ReasonType { get; set; }

        public string Notes { get; set; }
        [Display(Name = "Add Attachment")]
        public string Attachment_Path { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Status_MachineHistory { get; set; }

        public List<EditMachineHistoryLst> MachineGridList { get; set; }
        private IEnumerable<DtoList> _ReasonTypeList;
        public IEnumerable<DtoList> ReasonTypeList
        {
            get { return _ReasonTypeList; }
            set { _ReasonTypeList = value; }

        }
        private IEnumerable<DtoList> _StatusTypeList;
        public IEnumerable<DtoList> StatusTypeList
        {
            get { return _StatusTypeList; }
            set { _StatusTypeList = value; }

        }

        public IEnumerable<MachineDocumentsModel> MachineDocs { get; set; }
        public int TransactionTypeId { get; set; }
        public int CustomerId { get; set; }
        public int SubsidiaryId { get; set; }
        public int SubAgentId { get; set; }
        public int LocationId { get; set; }
        public string  ShippinglabelData { get; set; }
    }

    public class MachineHistory
    {
        public int CustomerId { get; set; }
        public int SubsidiaryID { get; set; }
        public int SubAgentID { get; set; }
        public int LocationId { get; set; }
        private IEnumerable<DtoList> _CustomerNameList;
        public IEnumerable<DtoList> CustomerNameList { get { return _CustomerNameList; } set { _CustomerNameList = value; } }
        private IEnumerable<DtoList> _LocationList;
        public IEnumerable<DtoList> LocationList { get { return _LocationList; } set { _LocationList = value; } }
        private IEnumerable<DtoList> _SubsidiaryList;
        public IEnumerable<DtoList> SubsidiaryList { get { return _SubsidiaryList; } set { _SubsidiaryList = value; } }
        private IEnumerable<DtoList> _SubAgentList;
        public IEnumerable<DtoList> SubAgentList { get { return _SubAgentList; } set { _SubAgentList = value; } }
        public List<MachineHistoryLst> MachineHistoryList { get; set; }
    }
    public class EditMachineHistoryLst
    {
        public DateTime EventDate { get; set; }
        public string Customer { get; set; }
        public string Subsidiary { get; set; }
        public string SubAgent { get; set; }
        public string Location { get; set; }
        public string Transaction_Type { get; set; }
        public string Notes_Lst { get; set; }
        public string Attachment_Path_Lst { get; set; }
        public int MachineHistoryId { get; set; }
        public string ShippingLabelData { get; set; }
    }
    public class MachineHistoryLst
    {
        public string MachineId_Val { get; set; }
        public int MachineId { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public bool IsHistoryView { get; set; }
        public int CustomerId { get; set; }
        public int SubsidiaryID { get; set; }
        public int SubAgentID { get; set; }
        public int LocationId { get; set; }
    }

    public class MachineHistoryParameters
    {
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public int SubsidiaryID { get; set; }
        public int SubAgentID { get; set; }
        public int MachineId { get; set; }
        public int MachineHistoryId { get; set; }
    }

    public class MachineDocumentsModel
    {
        public int Machine_Doc_ID { get; set; }
        public int Machine_ID { get; set; }
        public string Machine_Doc_Name { get; set; }
        public string Machine_Doc_type { get; set; }
        public string Doc_Path { get; set; }
        public string Comments { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool FileStatus { get; set; }

    }

}
