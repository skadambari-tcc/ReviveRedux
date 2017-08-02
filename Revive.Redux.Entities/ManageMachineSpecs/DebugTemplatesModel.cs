using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
  public class DebugTemplatesModel
    {
        
        public IEnumerable<Debug_Templates_DefinitionModel> debugTemplatesDefinitions { get; set; }
        
        public  List<string> Parameters { get; set; }
    }
}
