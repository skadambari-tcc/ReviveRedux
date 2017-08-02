using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
   public  class Role
    {
        public int MapId { get; set; }
        public int ControllerId { get; set; }
        public string ControllerName { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public int UserLevelID { get; set; }
    }
}
