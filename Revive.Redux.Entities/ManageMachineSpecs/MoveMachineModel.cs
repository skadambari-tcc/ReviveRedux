using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MoveMachineModel
    {
        #region Move Machine From
        [Required(ErrorMessage = "Required")]
        public int? CustomerIdFrom { get; set; }
        public IEnumerable<DtoList> CustomerListFrom { get; set; }

        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryIdFrom { get; set; }
        public IEnumerable<DtoList> SubsidiaryNameListFrom { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentIdFrom { get; set; }
        public IEnumerable<DtoList> SubAgentNameListFrom { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? LocationIdFrom { get; set; }
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public IEnumerable<DtoList> LocationListFrom { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? ShippedMachId { get; set; }
        public string ShippedMachIdVal { get; set; }
        public IEnumerable<DtoList> ShippedMachines { get; set; }
        #endregion

        #region Move Machine To
        [Required(ErrorMessage = "Required")]
        public int? CustomerIdTo { get; set; }
        public IEnumerable<DtoList> CustomerListTo { get; set; }

        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryIdTo { get; set; }
        public IEnumerable<DtoList> SubsidiaryNameListTo { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentIdTo { get; set; }
        public IEnumerable<DtoList> SubAgentNameListTo { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? LocationIdTo { get; set; }
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public IEnumerable<DtoList> LocationListTo { get; set; }


        [Required(ErrorMessage = "Required")]
        public int? TransactionType { get; set; }
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public IEnumerable<DtoList> TransactionTypeLst { get; set; }


        #endregion
    }
}
