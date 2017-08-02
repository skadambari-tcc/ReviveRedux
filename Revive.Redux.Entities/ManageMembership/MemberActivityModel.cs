using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MemberActivityModel
    {

        public string membership_Id { get; set; }
        public string process_Id { get; set; }
        public int? machine_Id { get; set; }
        public string machine_Id_Val { get; set; }
        public int? devicemanufacturer_Id { get; set; }
        public int? howlongago_Id { get; set; }
        public bool? pluggedinYes { get; set; }
        public string device_id { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public bool isMember { get; set; }
        public string member_name { get; set; }
        public string email_Id { get; set; }
        public string phonenumber { get; set; }
        // Revive Result

        public int result_category_Id { get; set; }
        public string resultCategoryCode { get; set; }

        public string revive_questions { get; set; }
        public string revive_answers { get; set; }
        public string software_version { get; set; }
        public string activity_Results { get; set; }

        // Error Fault category code  processfaults,equipmentfaults & boardfaults
        public string binaryErrorValues { get; set; }
        // Default debugparameter values;
        public string defaultdebugParametervalues { get; set; }
        public int machineActivity_id { get; set; }
        public MachineFaultModel machineFault { get; set; }
        public int modeId { get; set; }
        public string invoice_No { get; set; }
        public string firmwareID { get; set; }
        public bool? IsTMP { get; set; }
        public double Dry_time { get; set; }
        public double Water_removed { get; set; }
        public int LocationId { get; set; }
    }
}
