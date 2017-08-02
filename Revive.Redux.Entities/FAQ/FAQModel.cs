using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Revive.Redux.Entities
{
    public class FAQModel
    {
        public Nullable<int> FAQRowID { get; set; }
        public Nullable<int> UserGrpId { get; set; }
        public string UserGrpName { get; set; }
        [AllowHtml]
        public string Question { get; set; }
        [AllowHtml]
        public string Answer { get; set; }
        public string Comments { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}
