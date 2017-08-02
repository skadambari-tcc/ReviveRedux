using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace Revive.Redux.Entities
{
    public class ManageSubsidiaryModel
    {

        [Display(Name = "Subsidiary ID")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Subsidiary ID")]
        public int Subsidiary_ID { get; set; }

        [Display(Name = "Subsidiary Name")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Subsidiary Name")]
        // Updated changes for bug #6156
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Subsidiary Name should be between 2 to 100 characters")]
        [Remote("CheckSubsidiaryName", "ManageSubsidiary", AdditionalFields = "Subsidiary_Name,Subsidiary_ID", HttpMethod = "POST", ErrorMessage = "Subsidiary already exists")]
        [AllowHtml]
        public string Subsidiary_Name { get; set; }

        
        public string CustomerName { get; set; }

        [Display(Name = "Customer Affiliation.")]
        [Required(ErrorMessage = "Required")]
        public int CustomerId { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address should be between 2 to 100 characters")]     
        [AllowHtml]
        public string Primary_Address { get; set; }
        [Display(Name = "City")]        
        //[Required(ErrorMessage = "Required")]
        public int? Primary_City { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "City Name should be between 2 to 200 characters")]
        [AllowHtml]
        public string Primary_City_Name { get; set; }


        public string Primary_Add_Area { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Required")]
        public int? Primary_State { get; set; }

        [Display(Name = "Primary_Country")]
        public int? Primary_Country { get; set; }

        [Display(Name = "Primary_ZipCode")]
        // [Required(ErrorMessage = "Zip Code is required")]
        [RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Invalid zip code format")]
        public string Primary_ZipCode { get; set; }

        [Display(Name = "Address 2")]
        [Required(ErrorMessage = "Address 2 is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address2 should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Address 2")]
        [AllowHtml]
        public string Additional_Address_info { get; set; }

        [Display(Name = "Email ")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 8, ErrorMessage = "Email should be greater than 8 characters")]
        [RegularExpression("^[a-zA-Z0-9_\\+-]+(\\.[a-zA-Z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format")]

        public string Email_ID { get; set; }

        [Display(Name = "Phone")]      
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string Primary_Phone { get; set; }

        public string Primary_Phone_Ext { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Required")]
        // Updated changes for bug #6156
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Name")]
        public string Acct_manager_Name { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address should be between 2 to 100 characters")]
        //[RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Address")]
        [AllowHtml]
        public string Acct_manager_Primary_Address { get; set; }

        [Display(Name = "Address 2")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address2 should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Address 2")]
        [AllowHtml]
        public string Acct_manager_Additional_Address_info { get; set; }

        [Display(Name = "Account Manager City")]
        //[Required(ErrorMessage = "Required")]
        public int? Acct_manager_Primary_City { get; set; }

        [Display(Name = "Account Manager City")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "City Name should be between 2 to 200 characters")]
        [AllowHtml]
        public string Acct_manager_Primary_City_Name { get; set; }



        public string Acct_manager_Primary_Add_Area { get; set; }

        [Display(Name = "Account Manager State")]
        [Required(ErrorMessage = "Required")]
        public int? Acct_manager_Primary_State { get; set; }

        public int? Acct_manager_Primary_Country { get; set; }

        //[Required(ErrorMessage = "Zip code is required")]
        [RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Invalid zip code format")]
        public string Acct_manager_Primary_Zip { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string Acct_manager_Primary_Phone { get; set; }

        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool Status { get; set; }
        public IEnumerable<DtoList> CustomerList { get; set; }
        public IEnumerable<DtoList> CityList { get; set; }
        public IEnumerable<DtoList> StateList { get; set; }

        public string Successmsg { get; set; }
        public string Errormsg = "Select Value";

        public string PageAccessCode { get; set; }

        
        [Display(Name = "Subsidiary ID")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Invalid Subsidiary ID")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Subsidiary ID should be between 2 to 50 characters")]
        [Remote("CheckSubsidiaryRefCode", "ManageSubsidiary", AdditionalFields = "Subsidiary_Ref_Code,Subsidiary_ID,CustomerId", HttpMethod = "POST", ErrorMessage = "Subsidiary ID already exists")]
        public string Subsidiary_Ref_Code { get; set; }

        public string ErrorMsgs { get; set; }
    }
}
