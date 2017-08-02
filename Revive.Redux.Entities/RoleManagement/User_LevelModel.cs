using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Revive.Redux.Entities
{

    public class User_LevelModel
   {
       

       public int User_Level_ID { get; set; }
       public string Email { get; set; }

       //[Display(Name = "Role Name")]
       [Required(ErrorMessage = "Required")]
       [StringLength(50, MinimumLength = 2, ErrorMessage = "Role Name should be between 2 to 50 characters")]
       [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Role Name")]
       [Remote("CheckDuplicateRoleName", "UserRole", AdditionalFields = "User_Level_Name, User_Level_ID", HttpMethod = "POST", ErrorMessage = "This Role Name already exists")]
       public string User_Level_Name { get; set; }

       public string User_Level_Description { get; set; }
       [Required(ErrorMessage = "Required")]
       public Nullable<int> UserTypeId { get; set; }
       public Nullable<System.Guid> Created_by { get; set; }
       public Nullable<System.DateTime> Created_Date { get; set; }
       public Nullable<System.Guid> Modified_by { get; set; }
       public Nullable<System.DateTime> Modified_Date { get; set; }
       public string UserType { get; set; }
       public IEnumerable<DtoList> lstUserType { get; set; }
       public bool ErrorMessage { get; set; }
       public string _ErrorMsg { get; set; }
       public string _SuccessMsg { get; set; }
       public string page_access_code { get; set; }
       private IEnumerable<DtoList> _UserTypeList;
       public IEnumerable<DtoList> UserTypeList
       {
           get { return _UserTypeList; }
           set { _UserTypeList = value; }
       
       }
   }
    public class MenuMappingLocalModel
    {
        public string RoleId { get; set; }
        public List<MenuModel> AllMenus { get; set; }
        public string Menus { get; set; }
        public int User_Level_ID { get; set; }
        public string RoleName { get; set; }
    }

}
