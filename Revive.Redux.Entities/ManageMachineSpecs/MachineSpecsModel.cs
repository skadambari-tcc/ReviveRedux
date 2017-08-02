using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Revive.Redux.Entities
{//ui model
   public class MachineSpecsModel
    {
        public int MachineSpec_ID { get; set; }


        [Display(Name = "Generation")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[a-zA-Z0-9-]+$", ErrorMessage = "Invalid Generation")]
        [StringLength(100, ErrorMessage = "Machine Generation should be between 2 to 100 characters", MinimumLength = 2)]
        [AllowHtml]
        public string Generation { get; set; }

      
       //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Pump { get; set; }
        //[Display(Name = "Software Version")]
        [AllowHtml]
        public string Debug_Parameter_default_value { get; set; }

        
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Customfield1 { get; set; }
        
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Customfield2 { get; set; }
        
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Customfield3 { get; set; }
       
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Customfield4 { get; set; }
       
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Customfield5 { get; set; }
       
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string PCB_Version { get; set; }
       
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string INjection_Heater { get; set; }
        
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string PowerSupply_1 { get; set; }
        
        //[Required(ErrorMessage = "Required")]

        [AllowHtml]
        public string PowerSupply_2 { get; set; }
       
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Valve_1 { get; set; }
        
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Valve_2 { get; set; }
       
       // [Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Valve_3 { get; set; }

        [Required(ErrorMessage = "Required")] // As per discussion with Amit & Vishal sir , this filed will be mandatory..... dated : 30:07:2015 this firmware version will be use for machine current software
        [AllowHtml]
        public string Firmware_Version { get; set; }

        [AllowHtml]
        public string software_version { get; set; }
       
        //[Required(ErrorMessage = "Required")]
        [AllowHtml]
        public string Platen_heater { get; set; }
        public Nullable<System.Guid> Created_by { get; set; }
        public System.DateTime Created_Date { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public bool isSelected { get; set; }
    }
}
