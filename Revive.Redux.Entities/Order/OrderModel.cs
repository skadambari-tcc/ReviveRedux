using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Revive.Redux.Entities
{
    public class OrderModel
    {
        public int? JobOrderHeaderId { get; set; }

        public int? OrderId { get; set; }
        public string JobOrderHeaderIdEncoded { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer")]
        public int? CustomerId { set; get; }
        public IEnumerable<DtoList> CustomerList { set; get; }

        public string CustomerName { get; set; }
        public string CustomerPONumber { get; set; }


        public int? JobOrderStatusId { get; set; }
        public string StatusName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer PO")]
        public int? CustomerDocId { get; set; }

        public string CustomerDocName { get; set; }
        public IEnumerable<DtoList> CustomerDocList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Only 4 digits numeric allowed")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only numeric value allowed")]
        public int? NoOfMachines { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000, ErrorMessage = "Max 1000 characters allowed")]
        public string ClientExecComments { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000, ErrorMessage = "Max 1000 characters allowed")]
        public string ApproverComments { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000, ErrorMessage = "Max 1000 characters allowed")]
        //[Required(ErrorMessage = "Required")]
        public string ManufacturerComments { get; set; }

        public string DownloadSWUrl { get; set; }
        public bool ArchiveFlag { get; set; }
        public DateTime? ApproveRejectDate { get; set; }
        public Nullable<Guid> ApprovedRejectedUserId { get; set; }
        public string ApprovedRejectedUserName { get; set; }

        public Nullable<Guid> CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<Guid> ModifiedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? OrderDate { get; set; }

        public string MachineSpecs { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Machine Specs")]
        public int? MachineSpecsId { get; set; }
        public IEnumerable<DtoList> MachineSpecsList { get; set; }
        public string MachineSpecGeneration { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Expected Delivery Date")]
        public DateTime? ExpectedDate { get; set; }
        public string ExpectedOrderDate { get; set; }

        public IEnumerable<DtoList> LocationsList { get; set; }
        public int? NoOfMachinesDelivered { get; set; }
        public string ApprovedRejectedDate { get; set; }

        public List<MachineModel> MachineDetails { get; set; }

        public string JsonPostbackVal { get; set; }
        public string PageAccessCode { get; set; }
        public IEnumerable<DtoList> MachineIds { get; set; }

        public IEnumerable<DtoList> ManufacturerList { set; get; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Manufacturer Name")]
        public int? ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string AccountManagerName { get; set; }
        public int softwareVersionId { get; set; }

        //Subsidiary Changes
        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryId { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentId { get; set; }

        [Display(Name = "No. of Machines Remaining To Be Mapped")]
        public int NoOfMachinesMapped { get; set; }

        public string SuccessMsg { get; set; }

        public bool result { get; set; } 

    }

    public class MappedMachsLocs
    {
        public string MachineID { get; set; }
        public string Location { get; set; }
        public int OrderID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AttentionName { get; set; }
        public string OrderIDList { get; set; }



    }
}
