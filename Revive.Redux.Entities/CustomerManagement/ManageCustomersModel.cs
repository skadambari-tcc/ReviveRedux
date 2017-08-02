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
    public class ManageCustomersModel
    {

        [Display(Name = "Customer ID")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Customer ID")]
        public int Customer_ID { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Customer Name")]
        // Updated changes for bug #6156
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer Name should be between 2 to 100 characters")]
        [Remote("CheckCustomerName", "ManageCustomers", AdditionalFields = "Customer_Name,Customer_ID", HttpMethod = "POST", ErrorMessage = "Customer already exists")]
        
        public string Customer_Name { get; set; }

        [Display(Name = "Redux Account Mgr.")]
        [Required(ErrorMessage = "Required")]
        public Nullable<System.Guid> AccountmanagerUserID { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address should be between 2 to 100 characters")]
        //[RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Address")]
        [AllowHtml]
        public string Primary_Address { get; set; }

        [Display(Name = "CustomerID")]
        [RegularExpression("^[a-zA-Z0-9-]+$", ErrorMessage = "Invalid Customer ID")]
        [Remote("CheckDuplicateCustID", "ManageCustomers", AdditionalFields = "Customer_Ref_Code, Customer_ID", HttpMethod = "POST", ErrorMessage = "This Customer ID already exists. Please enter a different Customer ID")]
        //[Required(ErrorMessage = "CustomerID is required")]

        public string Customer_Ref_Code { get; set; }

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
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone format is not valid")]
        [RegularExpression(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$", ErrorMessage = "Invalid phone number")]
        public string Primary_Phone { get; set; }

        public string Primary_Phone_Ext { get; set; }

        [Display(Name = "Lease Term Start Date")]
        public DateTime? Lease_Start_Date { get; set; }

        [Display(Name = "Lease Term Start Date")]
        public DateTime? Lease_end_date { get; set; }

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



        public int? Membership_Duration { get; set; }
        public int? Membership_renew_duration { get; set; }
        public bool? NoofRevivesAllowed { get; set; }
        public bool? NoofRenewalRevivesAllowed { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool Status { get; set; }
        public IEnumerable<DtoList> AccountManagerList { get; set; }
        public IEnumerable<DtoList> CityList { get; set; }
        public IEnumerable<DtoList> StateList { get; set; }

        public string Successmsg { get; set; }
        public string Errormsg = "Select Value";

        public IEnumerable<HttpPostedFileBase> File { get; set; }

        public IEnumerable<CustomerDocumentsModel> CustomerDocs { get; set; }
        public string PageAccessCode { get; set; }
        public int TempLateId { get; set; }
        public int  ReviveAllowed { get; set; }
        public int CustomerMaxDevices { get; set; }
        public bool IsCustomerMultiDevice { get; set; }
    }

    public class CustomerDocumentsModel
    {
        public int Customer_Doc_ID { get; set; }
        public int Customer_ID { get; set; }
        public string Customer_Doc_Name { get; set; }
        public string Customer_Doc_type { get; set; }
        public string Doc_Path { get; set; }
        public string Comments { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool FileStatus { get; set; }
    }

}
