using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Revive.Redux.Entities
{
    public class UserModels
    {
        public System.Guid UserId { get; set; }

        [Display(Name = "User Email Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 8)]
        //[RegularExpression("^[a-zA-Z0-9_\\+-]+(\\.[a-zA-Z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format")]
        [Remote("CheckEmailAddressRegistation", "Account", AdditionalFields = "Email", HttpMethod = "POST", ErrorMessage = "Email already exists")]
        public string Email { get; set; }
        //CheckEmailAddressRegistation

        [Display(Name = "User Email Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 8)]
        //[RegularExpression("^[a-zA-Z0-9_\\+-]+(\\.[a-zA-Z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format")]
        [Remote("CheckEmailAddressEdit", "Account", AdditionalFields = "emailEdit,UserId", HttpMethod = "POST", ErrorMessage = "Email already exists")]
        public string emailEdit { get; set; }




        public string Pasword { get; set; }

        [Display(Name = "First Name")]
        //[MaxLength(99, ErrorMessage = "Maximum 100 characters are allowed")]
        [Required(ErrorMessage = "Required")]
        // Updated changes for bug #6156
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid first name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        //[Required(ErrorMessage = "Last name is required")]
        //[MaxLength(99, ErrorMessage = "Maximum 100 characters are allowed")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name should be between 2 to 100 characters")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid last name")]
        public string LastName { get; set; }


        [Display(Name = "User Mobile")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone format")]
        public string UserMobile { get; set; }

        [Display(Name = "City")]
        //[StringLength(150, ErrorMessage = "City should be of 150 characters")]
        //[Required(ErrorMessage = "City is Required")]
        public String City { get; set; }

        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Required")]
        public int? Customer_Id { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int? Location_Id { get; set; }

        [Display(Name = "Type of User")]
        [Required(ErrorMessage = "Required")]
        public int User_Level_Id { get; set; }

        // [Display(Name = "Status")]
        //  [Required(ErrorMessage = "Status is Required")]

        [Display(Name = "Manufacturer")]
        [Required(ErrorMessage = "Required")]
        public int? Manufacturer_Id { get; set; }

        public Nullable<System.Guid> Created_by { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }


        public Nullable<System.DateTime> Created_Date { get; set; }

        public Nullable<System.DateTime> Modified_Date { get; set; }

        public string Successmsg { get; set; }

        public string LocationName { get; set; }
        public string User_Level_Name { get; set; }
        public string UserType { get; set; }
        public string PageAccessCode { get; set; }
        public bool Status { get; set; }
        public bool IsStoreUserLogging { get; set; }
        public string ErrorMsgs { get; set; }
        public string FileName { get; set; }  // file Name
        public List<LocationResultModel> UserResult { get; set; }
        public List<ValidMailIdForBulkUpload> ValidUserNameToSendMail { get; set; }

        public filedata FileDetails { get; set; }

        //Subsidiary Changes
        [Display(Name = "Subsidiary")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubsidiaryId { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Required")]
        public int SubAgentId { get; set; }

        public int LoggTypeId { get; set; }
        private IEnumerable<DtoList> _CustomerNameList;

        public IEnumerable<DtoList> CustomerNameList
        {
            get { return _CustomerNameList; }
            set { _CustomerNameList = value; }
        }

        private IEnumerable<DtoList> _LocationList;

        public IEnumerable<DtoList> LocationList
        {
            get { return _LocationList; }
            set { _LocationList = value; }
        }

        private IEnumerable<DtoList> _SubsidiaryList;

        public IEnumerable<DtoList> SubsidiaryList
        {
            get { return _SubsidiaryList; }
            set { _SubsidiaryList = value; }
        }

        private IEnumerable<DtoList> _SubAgentList;

        public IEnumerable<DtoList> SubAgentList
        {
            get { return _SubAgentList; }
            set { _SubAgentList = value; }
        }

        //For manage Password
        public string NewPassword { get; set; }
        public bool IsEmailNotification { get; set; }

        //For Import Store User
        public string StoreNumber { get; set; }
    }

   public class ValidMailIdForBulkUpload
   {
       public string Email_Id { get; set; }
   }

   public class UserModelsBulkUpload
   {
       
       [Display(Name = "User Email Address")]
       [Required(ErrorMessage = "Required")]
       [StringLength(200, MinimumLength = 8)]
       [RegularExpression("^[a-zA-Z0-9_!#$%&'*+-/=?^_`{|}~\\+-]+(\\.[a-zA-Z0-9_ !#$%&'*+-/=?^_`{|}~\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format")]  //Commented by KD as per Client request mail date 22th April 2016
       [Remote("CheckEmailAddressRegistation", "Account", AdditionalFields = "Email", HttpMethod = "POST", ErrorMessage = "Email already exists")]
       public string Email { get; set; }
       
       
       [Display(Name = "First Name")]
       [Required(ErrorMessage = "Required")]
       [StringLength(100, MinimumLength = 2, ErrorMessage = "First name should be between 2 to 100 characters")]
       [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid first name")]
       public string FirstName { get; set; }


       [Display(Name = "Last Name")]
       [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name should be between 2 to 100 characters")]
       [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid last name")]
       public string LastName { get; set; }


       [Display(Name = "User Mobile")]
       [Required(ErrorMessage = "Required")]
       [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone format")]
       public string UserMobile { get; set; }

       [Display(Name = "Location")]
       [Required(ErrorMessage = "Required")]
       public int? Location_Id { get; set; }

     

     

   }
   
   //public class filedata
   //{
   //    public IList<string> data { get; set; }
   //    public IList<string> Exceptiondata { get; set; }
   //    public IList<string> Errordata { get; set; }
   //    public IList<string> Infodata { get; set; }
   //    public string FileName { get; set; }
       
   //    public filedata ()
   //    {
   //        data = new List<string>();
   //        Infodata = new List<string>();
   //        Exceptiondata = new List<string>();
   //        Errordata = new List<string>();
   //    }       
   //}
  
}
