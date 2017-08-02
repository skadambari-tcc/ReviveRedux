using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ManageGroupShippingQueueModel
    {
        [Display(Name = "Group")]
        [Required(ErrorMessage = "Required")]
        public int GroupId { get; set; }

        [Display(Name = "Begin Shipping Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime BeginShippingDate { get; set; }

        private IEnumerable<DtoList> _GroupList;
        public IEnumerable<DtoList> GroupList
        {
            get { return _GroupList; }
            set { _GroupList = value; }
        }

        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public int MachineCount { get; set; }
        public string Status { get; set; }
        public string GroupName { get; set; }

        public int MachineId { get; set; }
    }
}
