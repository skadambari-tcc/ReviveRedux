using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ShippingConfigModel
    {
       // [Display(Name = "Group")]

        public int Id { get; set; }

        [Display(Name = "Service Type")]
        [Required(ErrorMessage = "Required")]
        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "Required")]
       // [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public decimal? InsuranceAmount { get; set; }
        [Display(Name = "Package Weight")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PkgWt { get; set; }
        [Display(Name = "Package Length")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PkgLength { get; set; }
        [Display(Name = "Package Width")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PkgWidth { get; set; }
        [Display(Name = "Package Height")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PkgHeight { get; set; }
       
        [Display(Name = "Machine Identifier")]
        [Required(ErrorMessage = "Required")]
        public string MachineIdentifier { get; set; }
        public bool? IsActive { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public Guid CurrentUserId { get; set; }
        public bool IsEdit { get; set; }
      

    }
}
