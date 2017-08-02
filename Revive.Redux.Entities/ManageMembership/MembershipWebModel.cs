using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Revive.Redux.Entities
{
    
   public class MembershipWebModel
    {   
        public int CustomerId { get; set; }
        public string Membership_ID { get; set; }
       [Required(ErrorMessage = "Required")]
        public string MemberName { get; set; }

        [Display(Name = "Phone Number")]
        //[Required(ErrorMessage = "Required")]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid phone number")]
       // [RegularExpression(@"^([0-9]+)$", ErrorMessage = "Invalid phone number")]
       // [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone format")]
        public string Phone_Number { get; set; }

       // [RegularExpression("^[a-zA-Z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format")]
        public string Email_ID { get; set; }
        public string DOB { get; set; }
        public int Machine_ID { get; set; }

        [Display(Name = "Location")]
        //[Required(ErrorMessage = "Required")]
        // [StringLength(100, MinimumLength = 2, ErrorMessage = "Location should be between 2 to 100 characters")]
        //[RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid location name")]
        public string LocationName { get; set; }


        
        public Guid user_Id { get; set; }

        //Subsidiary Changes
        
        public int SubsidiaryId { get; set; }        
        public int SubAgentId { get; set; }

        public IEnumerable<DtoList> CustomerNameList { get; set; }
        public List<MembershipResultModel> LocationResult { get; set; }
        public IEnumerable<DtoList> SubsidiaryNameList { get; set; }
        public IEnumerable<DtoList> AgentNameList { get; set; }


        public string ErrorMsgs { get; set; }
        public string FileName { get; set; }  // file Name
        public List<MembershipResultModel> UserResult { get; set; }

       [Required(ErrorMessage = "Required")]
       [RegularExpression(@"^([1-9]+)$", ErrorMessage = "Invalid Revives Remaining Value")]
        public int no_of_FreeDries { get; set; }

       //[Required(ErrorMessage = "Required")]
       public DateTime StartDate { get; set; }

       //[Required(ErrorMessage = "Required")]
       public DateTime EndDate { get; set; }       

       [Required(ErrorMessage = "Required")]
       public string InvoiceNumber { get; set; }

       public bool IsLegacy { get; set; }
       public int reviveDeviceId { get; set; }
       [Required(ErrorMessage = "Required")]
       [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
       public int LocationId { get; set; }

       [Required(ErrorMessage = "Required")]
       public string strStartDate { get; set; }
       [Required(ErrorMessage = "Required")]
       public string strEndDate { get; set; }

       [Display(Name = "Store Number")]
       //[Required(ErrorMessage = "Required")]
       //[StringLength(100, MinimumLength = 2, ErrorMessage = "Store Number should be between 2 to 100 characters")]
       //[RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid store number")]       
       public string StoreNumber { get; set; }
       public bool IsMultiDevice { get; set; }
    }

    

   //public class MembershipResultModel
   //{
   //    public string LineNumber { get; set; }
   //    public string Message { get; set; }
   //    public bool Status { get; set; }
   //}
}
