using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ReplaceMachineModel
    {
        [Required(ErrorMessage = "Required")]
        public int? CustomerId { get; set; }
        public IEnumerable<DtoList> CustomerList { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? LocationId { get; set; }
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public IEnumerable<DtoList> LocationList { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? ShippedMachId { get; set; }
        public string ShippedMachId_Val { get; set; }
        public IEnumerable<DtoList> ShippedMachines { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? UnassignedMachId { get; set; }
        public string UnassignedMachId_Val { get; set; }
        public IEnumerable<DtoList> UnassignedMachines { get; set; }

        public int? Machine_LocationId { get; set; }
        public IEnumerable<DtoList> Machine_LocationList { get; set; }

        //Subsidiary Changes
        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryId { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentId { get; set; }

        public IEnumerable<DtoList> SubsidiaryNameList { get; set; }
        public IEnumerable<DtoList> AgentNameList { get; set; }
    }
}
