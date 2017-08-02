using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
   public class modelUser_Level_Users
    {
        public int Id { get; set; }
        public System.Guid UserId { get; set; }
        public int User_Level_Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public virtual modelUser UserRole { get; set; }
    }
}
