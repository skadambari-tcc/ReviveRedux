using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public  class APIResponseData
    {
        public ReturnStatusCode StatusMessages { get; set; }
        public dynamic responseData { get; set; }
       

    }
    public class APIParameter
    {
        public string ProcessId { get; set; }
        public string Machine_Id_Val { get; set; }
        public int MachineId { get; set; }
    }

    public class ReviveDetails
    {
        public Nullable<int> Machine_Id { get; set; }
        public string Process_Id { get; set; }
        public string Membership_Id { get; set; }
        public string MemberName { get; set; }
        public string Phonenumber { get; set; }
        public string EmailId { get; set; }
        public string PhoneModel { get; set; }
        public Nullable<bool> IsMember { get; set; }
    }

   
}
