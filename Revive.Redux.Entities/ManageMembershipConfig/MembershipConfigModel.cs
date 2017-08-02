using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MembershipConfigModel : PageBasic
    {
        private int _nCustomerId;

        public int CustomerId
        {
            get { return _nCustomerId; }
            set { _nCustomerId = value; }
        }

        private string _sCustomerName;

        public string CustomerName
        {
            get { return _sCustomerName; }
            set { _sCustomerName = value; }
        }

        private int _nMembershipDuration;

        [Display(Name = "Membership Duration")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Membership duration cannot be less than 1")]
        [RegularExpression(@"^(\d+)?$", ErrorMessage = "Invalid membership duration")]
        public int MembershipDuration
        {
            get { return _nMembershipDuration; }
            set { _nMembershipDuration = value; }
        }

        private int _nMembershipRenewDuration;

        [Display(Name = "Renew Membership Duration")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Renew membership duration cannot be less than 1")]
        [RegularExpression(@"^(\d+)?$", ErrorMessage = "Invalid renew membership duration")]
        public int MembershipRenewDuration
        {
            get { return _nMembershipRenewDuration; }
            set { _nMembershipRenewDuration = value; }
        }

        private int _nTotalReviveAllowed;

        [Display(Name = "Revive Allowed")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Revive allowed cannot be less than 1")]
        [RegularExpression(@"^(\d+)?$", ErrorMessage = "Invalid revive allowed")]
        public int TotalReviveAllowed
        {
            get { return _nTotalReviveAllowed; }
            set { _nTotalReviveAllowed = value; }
        }

        private int _nTotalRenewedReviveAllowed;

        [Display(Name = "Renew Revive Allowed")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Revives allowed in case of renewal cannot be less than 1")]
        [RegularExpression(@"^(\d+)?$", ErrorMessage = "Invalid revives allowed in case of renewal")]
        public int TotalRenewedReviveAllowed
        {
            get { return _nTotalRenewedReviveAllowed; }
            set { _nTotalRenewedReviveAllowed = value; }
        }

        [Range(typeof(Int32), "0", "90",ErrorMessage="Eligibility Wait Period must be between 0 and 90")]
        [Required(ErrorMessage = "Required")]
        public int eligibiltyWaitPeriod
        { get; set; }

         [Range(typeof(Int32), "0", "366", ErrorMessage = "Void days  must be between 0 and 366")]
        public int VoidMembershipDays { get; set; }
         public bool IsMultiDevice { get; set; }


        //[Range(typeof(Int32), "1", "5", ErrorMessage = "No of Devices must be between 1 and 5")]
        [Required(ErrorMessage = "No of devices field is required")]
         public int NoOfDevices { get; set; }



    }
}
