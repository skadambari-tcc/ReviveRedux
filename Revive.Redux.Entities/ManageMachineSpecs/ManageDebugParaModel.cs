using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Revive.Redux.Entities
{
   public class ManageDebugParaModel
    {
       [Display(Name = "Customer")]
       //[Required(ErrorMessage = "User email address is required")]
        public int CustomerId { get; set; }
        public string Customer_Name { get; set; }        
        public int LocationId { get; set; }
        public string Location_Name { get; set; }
        public int MachineId { get; set; }
        public string MachineId_Val { get; set; }
        public int? TempalteId { get; set; }
       
        public string Template_VersionNumber { get; set; }
        public string Comments { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool isSelected { get; set; }
       //[Required(ErrorMessage = "Required")]
        public string Template_Name { get; set; }
        public string msg { get; set; }
        public int templateId { get; set; }

        //Subsidiary Changes
        public int SubsidiaryId { get; set; }
        public int SubAgentId { get; set; }

    }
}
