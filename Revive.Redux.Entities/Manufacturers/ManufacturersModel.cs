using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Revive.Redux.Entities
{
    public class ManufacturersModel
    {
        public int? Manufacturer_Id { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Company Name should be between 2 to 100 characters")]
        [AllowHtml]
        public string Manufacturer_Name { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Address should be between 2 to 100 characters")]
        [AllowHtml]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone format")]
        

        public string Phone_Number { get; set; }

        [AllowHtml]
        public string Company_Website { get; set; }
        [AllowHtml]
        public string PM_Name { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\+-]+(\\.[a-zA-Z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format")]
        public string PM_Email { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Format")]
        public string PM_Office_Phone { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Format")]
        public string PM_Mobile { get; set; }

        public Nullable<bool> Status { get; set; }
        public Nullable<Guid> Created_by { get; set; }
        public Nullable<DateTime> Created_Date { get; set; }
        public Nullable<Guid> Modified_by { get; set; }
        public Nullable<DateTime> Modified_Date { get; set; }

        public string PageAccessCode { get; set; }
        public string Successmsg { get; set; }

        [Display(Name = "Manufacturer ID")]
        [Required(ErrorMessage = "Required")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Manufacturer id should be 1 character only")]
        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Invalid manufacturer id")]
        [Remote("CheckManufacturerRefCode", "Manufacturers", AdditionalFields = "Manufacturer_Ref_Code,Manufacturer_Id", HttpMethod = "POST", ErrorMessage = "Manufacturer ID already exists")]
        public string Manufacturer_Ref_Code { get; set; }
    }
}
