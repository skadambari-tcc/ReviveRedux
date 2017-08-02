using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class NotificationModel
    {
        public int Notification_Id { get; set; }
        public string NotificationMessages { get; set; }
        public DateTime Notification_Date { get; set; }
        public bool? Status { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string Mail_Ids { get; set; }
        public string body_mail { get; set; }
        public bool? readFlag { get; set; }
    }
}
