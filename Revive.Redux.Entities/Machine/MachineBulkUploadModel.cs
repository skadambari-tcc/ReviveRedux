using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MachineBulkUploadModel
    {
        [Display(Name = "Machine ID")]
        [Required(ErrorMessage = "Required")]
        public string MachineId { get; set; }

        [Display(Name = "Location Name")]
        [Required(ErrorMessage = "Required")]
        public string Location { get; set; }

        [Display(Name = "Shipping ID")]
        [Required(ErrorMessage = "Required")]
        public string ShippingId { get; set; }
    }

    
}
