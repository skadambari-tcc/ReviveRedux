using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ReportMasterModel : PageBasic
    {
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Filters { get; set; }
        public string Fields { get; set; }
        public string Authorization { get; set; }
        public string FieldName { get; set; }
    }
}
