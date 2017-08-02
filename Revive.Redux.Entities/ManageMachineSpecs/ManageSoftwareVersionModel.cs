using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Revive.Redux.Entities
{
    public class ManageSoftwareVersionModel
    {
        public int MasterData_Type_ID { get; set; }
        public string MasterData_Type { get; set; }
        [Display(Name = "Software Version")]
        [Required(ErrorMessage = ("Required"))]
        //[RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Invalid Software version Name")]
        [Remote("CheckSoftwareVersion", "ManageMachineSpecs", AdditionalFields = "MasterData_Value,MasterData_Type_ID", HttpMethod = "POST", ErrorMessage = "Software Version already exists")]
        [AllowHtml]
        public string MasterData_Value { get; set; }

        // [Required(ErrorMessage = ("Required"))]
        [Display(Name = "Package Path")]
        //[RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Invalid Path")]
        [AllowHtml]
        public string Custom_Field1 { get; set; }


        [Required(ErrorMessage = ("Required"))]
        [Display(Name = "Details")]
        //[RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Invalid Details")]
        [AllowHtml]
        public string CustomField2 { get; set; }


        [Display(Name = "Notes")]
        //[RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Invalid Notes")]
        [AllowHtml]
        public string CustomField3 { get; set; }

        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        //Used for Archiving
        public bool status { get; set; }
        public string CheckSumKey { get; set; }
    }
}

