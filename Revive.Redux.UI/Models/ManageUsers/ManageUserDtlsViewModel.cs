using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Revive.Web.Common;
using Revive.Redux.Entities;

namespace Revive.Redux.UI
{
    public class ManageUserDtlsViewModel
    {
        [StringLength(20, ErrorMessage = "Maximum 20 characters are allowed.")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid User Name.")]
        [Required(ErrorMessage = "User Name is Required.")]
        public string UserName { get; set; }

        public string Role { get; set; }

        public int? User_Key { get; set; }

        [Required(ErrorMessage = "Email Id is Required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Maximum 100 characters are allowed.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Phone Number is Required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only numerals allowed")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Phone number should be of 10 digit.")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Customer is Required.")]
        [Display(Name = "Customer")]
        public int Customer { get; set; }
        public int ID { get; set; }

        [Required(ErrorMessage = "Location is Required.")]
        [Display(Name = "Location")]
        public int Location { get; set; }

        [Required(ErrorMessage = "UserType is Required.")]
        [Display(Name = "UserType")]
        public int UserType { get; set; }

        public IEnumerable<DtoList> lstUserType { get; set; }
        public IEnumerable<DtoList> lstCustomer { get; set; }
        public IEnumerable<DtoList> lstLocation { get; set; }
    }
}