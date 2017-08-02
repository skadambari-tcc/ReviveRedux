using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Revive.Redux.Entities
{
    public class Debug_Templates_DefinitionModel :PageBasic
    {
        public int Template_ID { get; set; }
        public string Template_Name { get; set; }
        public string VersionNumber { get; set; }
       public int? DebugParameter_Id { get; set; }
        //public string DebugParameter_Id { get; set; }
        public string DebugParameterName { get; set; }
        public string DebugParameter_default_value { get; set; }
        public string  TemplateCreatedBy { get; set; }
       
    }
}
