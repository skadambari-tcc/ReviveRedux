using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class modelUser
    {
        public modelUser()
        {
            this.User_Level_Menu_Mapping = new HashSet<modelUser_Level_Menu_Mapping>();
           this.User_Level_Users = new HashSet<modelUser_Level_Users>();
        }

        public int User_Level_Id { get; set; }
        public string User_Level_Name { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public virtual ICollection<modelUser_Level_Menu_Mapping> User_Level_Menu_Mapping { get; set; }
        public virtual ICollection<modelUser_Level_Users> User_Level_Users { get; set; }
    }
}
