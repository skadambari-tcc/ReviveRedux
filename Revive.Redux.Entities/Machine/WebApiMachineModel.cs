using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
   public class WebApiMachineModel
    {
       public string MachineId { get; set; }  // For External API Call to insert new record in JobOrder_Machine_Id
       public string other1 { get; set; }
       public string other2 { get; set; } // For any other data to take from External 

    }
}
