using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MemberShipDetailsModel
    {
        public string MemberName { get; set; }
        public string Phone_Number { get; set; }
        public string Email_ID { get; set; }
        public string Membership_ID { get; set; }
        public Nullable<System.DateTime> Membership_Start_date { get; set; }
        public Nullable<System.DateTime> Membership_End_date { get; set; }
        public Nullable<int> No_of_Free_dries { get; set; }
        public string StoreCreated { get; set; }
        public string Store_ID { get; set; }
        public Nullable<System.DateTime> Membership_Activation_date { get; set; }
        public string createdBy { get; set; }
        public string member_LastReviveDate { get; set; }
        public string member_LastLocation { get; set; }
        public string member_LastUserName { get; set; }
        public string member_LastStore { get; set; }

        public string MembershipStatus { get; set; }
        public string CommonText { get; set; }
        public string ActiveStatus { get; set; }
        public Nullable<System.DateTime> Renew_Start_date { get; set; }
        public Nullable<System.DateTime> Renew_End_date { get; set; }
        public Nullable<System.DateTime> Renew_Activation_date { get; set; }
        public Nullable<int> Renew_No_of_Free_dries { get; set; }
        public bool IsLegacy { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public bool IsActive { get; set; }


    }
}
