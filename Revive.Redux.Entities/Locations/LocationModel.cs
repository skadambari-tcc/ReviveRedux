using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Revive.Redux.Entities
{
    public class LocationModel
    {


        public string CustomerName { get; set; }

        [Display(Name = "CustomerName")]
        [Required(ErrorMessage = "Required")]
        public int CustomerId { get; set; }



        [Display(Name = "Location")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Location should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid location name")]
        public string LocationName { get; set; }

        public int LocationId { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Required")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Address Line 1 should be between 2 to 35 characters")]  // modified on dated 19th May 2017 ,UPS API is accepting max 35 chars per address
        [AllowHtml]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        // [Required(ErrorMessage = "Address line 2 is required")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Address Line 2 should be between 2 to 35 characters")]
        [AllowHtml]
        public string AddressLine2 { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select correct State")]
        public int State { get; set; }

        [Display(Name = "City")]
        //[Required(ErrorMessage = "Required")]
        //[Range(1, int.MaxValue, ErrorMessage = "Please select correct City")]
        public int City { get; set; }

        // New Field Added by KD as per Client Feedback..

        [Display(Name = "City")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "City Name should be between 2 to 200 characters")]
        [AllowHtml]
        public string CityName { get; set; }



        [Display(Name = "Country")]
        public int Country { get; set; }

        [Display(Name = "ZipCode")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Invalid zip code format")]
        public string ZipCode { get; set; }

        [Display(Name = "Primary Phone Number")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid primary phone number")]
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneExt { get; set; }

        [Display(Name = "Store Number")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Store Number should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid store number")]
        [Remote("CheckDuplicateStoreNo", "ManageCustomers", AdditionalFields = "StoreNumber, CustomerId, LocationId", HttpMethod = "POST", ErrorMessage = "This store number already exists. Please enter a different store number")]
        public string StoreNumber { get; set; }

        [Display(Name = "Opening Time(24 Hours)")]
        [RegularExpression(@"^([01]?\d|2[0-3]):[0-5]\d$", ErrorMessage = "Invalid opening time format")]
        public string StoreOpeningTime { get; set; }

        [Display(Name = "Closing Time(24 Hours)")]
        [RegularExpression(@"^([01]?\d|2[0-3]):[0-5]\d$", ErrorMessage = "Invalid closing time format")]
        public string StoreClosingTime { get; set; }

        [AllowHtml]
        public string CustomField1 { get; set; }
        [AllowHtml]
        public string CustomField2 { get; set; }
        [AllowHtml]
        public string CustomField3 { get; set; }
        [AllowHtml]
        public string CustomField4 { get; set; }
        [AllowHtml]
        public string CustomField5 { get; set; }

        public bool CustomField1Valid { get; set; }
        public bool CustomField2Valid { get; set; }
        public bool CustomField3Valid { get; set; }
        public bool CustomField4Valid { get; set; }
        public bool CustomField5Valid { get; set; }

        public string CustomField1Text { get; set; }
        public string CustomField2Text { get; set; }
        public string CustomField3Text { get; set; }
        public string CustomField4Text { get; set; }
        public string CustomField5Text { get; set; }

        [Display(Name = "Header")]
        public string PageHeader { get; set; }

        [Display(Name = "Location Button")]
        public string PageButtonSubmit { get; set; }

        public int PageMode { get; set; } // 0 - Add, 1 - Edit , 2 - Read Only

        public bool Status { get; set; }  // Activate or Deactivate Location - Defaute is false

        public string StateName { get; set; }


        public string Message { get; set; }  // file upload status message
        public string FileName { get; set; }  // file Name

        public Guid? Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid? Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }

        public string CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public string DateModified { get; set; }

        public IEnumerable<DtoList> CityList { get; set; }
        public IEnumerable<DtoList> StateList { get; set; }
        public IEnumerable<DtoList> CustomerNameList { get; set; }

        public List<CustomerLocationModel> CustomerLocationDetails { get; set; }
        public List<GroupedLocationForShip> GroupedLocationForShipDetails { get; set; }

        


        public List<LocationResultModel> LocationResult { get; set; }

        //Subsidiary Changes
        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryId { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentId { get; set; }

        public IEnumerable<DtoList> SubsidiaryNameList { get; set; }
        public IEnumerable<DtoList> AgentNameList { get; set; }

        public IEnumerable<DtoList> GroupNameList { get; set; }

        //Group Id
        [Display(Name = "GroupId")]
        //[Required(ErrorMessage = "Required")]
        public int? GroupId { get; set; }

        public IEnumerable<DtoList> StatusList { get; set; }

        public int ShippingStatusId { get; set; }
    }

    public class LocationResultModel
    {
        public string LineNumber { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }

    public class LocationModelBulk
    {



        [Display(Name = "Location")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Location should be between 2 to 100 characters")]
        public string LocationName { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address Line 1 should be between 2 to 100 characters")]
        [AllowHtml]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        // [Required(ErrorMessage = "Address line 2 is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address Line 2 should be between 2 to 100 characters")]
        [AllowHtml]
        public string AddressLine2 { get; set; }



        // New Field Added by KD as per Client Feedback..

        [Display(Name = "City")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "City Name should be between 2 to 200 characters")]
        [AllowHtml]
        public string CityName { get; set; }




        [Display(Name = "ZipCode")]
        // [Required(ErrorMessage = "Required")]
        //[RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Invalid zip code format")]
        public string ZipCode { get; set; }



        [Display(Name = "Primary Phone Number")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid primary phone number")]
        public string PrimaryPhone { get; set; }



        [Display(Name = "Store Number")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Store Number should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid store number")]
        [Remote("CheckDuplicateStoreNo", "ManageCustomers", AdditionalFields = "StoreNumber, CustomerId, LocationId", HttpMethod = "POST", ErrorMessage = "This store number already exists. Please enter a different store number")]
        public string StoreNumber { get; set; }



        [Display(Name = "Opening Time(24 Hours)")]
        [RegularExpression(@"^([01]?\d|2[0-3]):[0-5]\d$", ErrorMessage = "Invalid opening time format")]
        public string StoreOpeningTime { get; set; }

        [Display(Name = "Closing Time(24 Hours)")]
        [RegularExpression(@"^([01]?\d|2[0-3]):[0-5]\d$", ErrorMessage = "Invalid closing time format")]
        public string StoreClosingTime { get; set; }

        [Display(Name = "State Code")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int StateId { get; set; }

       



    }



}
