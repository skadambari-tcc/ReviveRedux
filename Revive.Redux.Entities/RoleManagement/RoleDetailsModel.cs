using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class RoleDetailsModel
    {
       
            public int Role_id { get; set; }
            public string Role_Name { get; set; }
            public string UserType { get; set; }
            public int Role_Type_Id { get; set; }
            public string Access_Type_Name { get; set; }
            public string page_access_code { get; set; }
      
    }
}
