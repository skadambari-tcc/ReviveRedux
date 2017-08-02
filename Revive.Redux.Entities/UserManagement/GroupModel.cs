using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Revive.Redux.Entities
{
    public class GroupModel
    {
        public int GroupId { get; set; }
    
        [Display(Name = "Priority")]
        [Required(ErrorMessage = "Required")]
        [Remote("CheckDuplicatePriority", "ManageUsers", AdditionalFields = "Priority_Id,GroupId", HttpMethod = "POST", ErrorMessage = "This priority is already assigned to a group.")]       
        public int? Priority_Id { get; set; }

        [Display(Name = "Group Name")]
        //[MaxLength(99, ErrorMessage = "Maximum 100 characters are allowed")]
        [Required(ErrorMessage = "Required")]
        // Updated changes for bug #6156
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Group name")]
        [Remote("CheckDuplicateGroupName", "ManageUsers", AdditionalFields = "GroupName,GroupId", HttpMethod = "POST", ErrorMessage = "This Group Name already exists")]
        public string GroupName { get; set; }

        public string Machinestatus { get; set; }
     

        public string PriorityName { get; set; }
        public bool GroupStatus { get; set; }


        [Display(Name = "Shipping Date")]
        public DateTime? ShippingDate { get; set; }

        private IEnumerable<DtoList> _PriorityList;
        public IEnumerable<DtoList> PriorityList
        {
            get { return _PriorityList; }
            set { _PriorityList = value; }
        }

        public string Successmsg { get; set; }
        public string ErrorMsgs { get; set; }
        public string Redirectpath { get; set; }
        public string Result { get; set; }
        public string ImageData { get; set; }
     }
}
