using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class SoftwareVersionModel
    {
        public string softwareVersion { get; set; }
        public string softwareVersionPath { get; set; }
        public string softwareDetails { get; set; }
        public string softwareNote { get; set; }
        public string FileCheckSumKey { get; set; }
    }
}
