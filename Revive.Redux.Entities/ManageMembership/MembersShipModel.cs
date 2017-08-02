using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MembersShipModel
    {
        public string membership_Id { get; set; }
        public int? Customer_Id { get; set; }
        public string MemberName { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public string Email_Id { get; set; }
        public string Phone_Number { get; set; }
        public DateTime? DOB { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public Guid user_Id { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string reviveDevice_Id { get; set; }
        public int machine_Id { get; set; }
        public string invoice_No { get; set; }
        public string machine_Id_Val { get; set; }
        public int fmSlno { get; set; }
        public int toSlno { get; set; }
        public string DOBValue { get; set; }
        public bool IsLegacy { get; set; }
        public string CustomerRefCode { get; set; }
        public Guid APIKey { get; set; }
        public bool IsMultiDevice { get; set; }
        public int  MachineHistoryDaysLength { get; set; }
        public string ProcessId { get; set; }
    }
    public class IsDOBValid
    {
        public bool IsDOBMatch { get; set; }
    }
    public class ManageMember
    {
        private IEnumerable<DtoList> _CustomerNameList;

        public IEnumerable<DtoList> CustomerNameList
        {
            get { return _CustomerNameList; }
            set { _CustomerNameList = value; }
        }

        private IEnumerable<DtoList> _LocationList;

        public IEnumerable<DtoList> LocationList
        {
            get { return _LocationList; }
            set { _LocationList = value; }
        }

        private IEnumerable<DtoList> _SubsidiaryList;

        public IEnumerable<DtoList> SubsidiaryList
        {
            get { return _SubsidiaryList; }
            set { _SubsidiaryList = value; }
        }

        private IEnumerable<DtoList> _SubAgentList;

        public IEnumerable<DtoList> SubAgentList
        {
            get { return _SubAgentList; }
            set { _SubAgentList = value; }
        }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        //Added For editmember Page
        [Display(Name = "MemberShip Start Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime MembershipStartDate { get; set; }

        

        public int SubsidiaryID { get; set; }
        public int SubAgentID { get; set; }


        public int Membership_Det_ID { get; set; }
        public string CustomerName { get; set; }
        public string MembershipId { get; set; }
        public string MemberName { get; set; }
        public int RevivesUsed { get; set; }
        public int PendingRevives { get; set; }

        //[Display(Name = "DOB")]
        //[Required(ErrorMessage = "Required")]
        //public DateTime? DOB { get; set; }

        [Display(Name = "Invoice Number")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Invoice No should be between 2 to 50 characters")]
        public string InvoiceNumber { get; set; }


        public DateTime MembershipDate { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 8)]
        [RegularExpression("^[a-zA-Z0-9_\\+-]+(\\.[a-zA-Z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format")]
        public string EmailId { get; set; }
        public string LastReviveUsedDate { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Device2")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string Device2 { get; set; }

        [Display(Name = "Device3")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string Device3 { get; set; }

        [Display(Name = "Device4")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string Device4 { get; set; }

        [Display(Name = "Device5")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string Device5 { get; set; }



        public List<MemberShipDetails> MembershipDetail { get; set; }
        public bool IsVoid { get; set; }

        public int voidDays { get; set; }
        public string VoidExpire { get; set; }
        public int DryAttemptCount { get; set; }


        //Membership Modal Start
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email_ID { get; set; }
        public string Membership_ID { get; set; }
        //public string MemberName { get; set; }
        public string Phone_Number { get; set; }
        //public string DOB { get; set; }
        public string LocationName { get; set; }
        public Guid user_Id { get; set; }
        public List<MembershipResultModel> LocationResult { get; set; }
        public string ErrorMsgs { get; set; }
        public string FileName { get; set; }  // file Name
        public List<MembershipResultModel> UserResult { get; set; }
        public bool IsLegacy { get; set; }
        //public string InvoiceNumber { get; set; }
        public int reviveDeviceId { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string strEndDate { get; set; }
        public string strStartDate { get; set; }
        public int no_of_FreeDries { get; set; }
        public int membershipDuration { get; set; }
       
        //Membership Modal End

        //Added For MemberUpload
        public string StoreNumber { get; set; }
        public bool IsMultiDevice { get; set; }
        public int custMaxNoOfDevices { get; set; }
    }


    public class MembershipDataSourceResult
    {
        public int Total { get; set; }
        public List<MemberShipDetails> objMembershipDetails { get; set; }
    }
    public class MemberShipDetails
    {
        public int Membership_Det_ID { get; set; }
        public string CustomerName { get; set; }
        public string MembershipId { get; set; }
        public string MemberName { get; set; }
        public string InvoiceNumber { get; set; }
        public int RevivesUsed { get; set; }
        public int PendingRevives { get; set; }
        public bool IsVoid { get; set; }
        public string IsMultiDevice { get; set; }
    }
    public class MemberShipParameters
    {
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public int SubsidiaryID { get; set; }
        public int SubAgentID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Membership_Details_ID { get; set; }
        public bool IsVoidUpdate { get; set; }
        public string MembershipId { get; set; }
        public string WarningMessage { get; set; }
        public int VoidDays { get; set; }
        public int DryAttemptCount { get; set; }
        public string VoidExpire { get; set; }
    }


}
