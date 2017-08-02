using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities.Common
{
    public class WriteLog
    {
        public string funcName { get; set; }
        public List<Tuple<string, string>> keyValPair { get; set; }
    }
}
