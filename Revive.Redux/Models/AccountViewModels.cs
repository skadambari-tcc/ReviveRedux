﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Revive.Redux.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("CheckEmailAddress", "Account", AdditionalFields = "Email", HttpMethod = "POST", ErrorMessage = "Username not exists.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public bool ErrorMessage { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("CheckEmailAddress", "Account", AdditionalFields = "Email", HttpMethod = "POST", ErrorMessage = "Email Address do not match.")]
        public string Email { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [Remote("CheckOldPassword", "Account", AdditionalFields = "OldPassword", HttpMethod = "POST", ErrorMessage = "Current Password is incorrect.")]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Password should have atleast 8 alphanumeric characters and It should include only these @$!%*#?& special characters")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
