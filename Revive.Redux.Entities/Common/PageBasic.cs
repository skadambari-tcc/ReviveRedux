using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
   public  class PageBasic
    {
        [Display(Name = "Header")]
        public string PageHeader { get; set; }

        [Display(Name = "Location Button")]
        public string PageButtonSubmit { get; set; }

        public int PageMode { get; set; } // 0 - Add, 1 - Edit , 2 - Read Only

        [Display(Name = "Message")]
        public string Message { get; set; } // used for either sucess or failure message

        public Nullable<Guid> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
