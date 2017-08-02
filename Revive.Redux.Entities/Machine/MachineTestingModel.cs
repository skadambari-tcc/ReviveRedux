using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MachineTestingModel
    {
        public int Machine_Testing_id { get; set; }
        public int Machine_Id { get; set; }
        public string Machine_Id_val { get; set; }
        public bool? Dielectric_test { get; set; }
        public int? Dielectric_Voltage { get; set; }
        public decimal? Dielectric_Current { get; set; }

        public int? Dielectric_Time { get; set; }
        public bool? Ground_Test { get; set; }
        public decimal? Ground_Resistance { get; set; }
        public bool? Platen_time { get; set; }
        public int? Platen_seconds { get; set; }

        public bool? Platen_sensor { get; set; }
        public decimal? Platen_delta { get; set; }
        public bool? Injection_time { get; set; }
        public int? Injection_seconds { get; set; }
        public bool? Injection_sensor { get; set; }
        public decimal? Injection_delta { get; set; }
        public bool? Vaccum_time { get; set; }
        public int? Vaccum_seconds { get; set; }
        public bool? Vaccum_sensor { get; set; }
        public decimal? Vaccum_delta { get; set; }
        public bool? USB_Internal_sensor { get; set; }
        public decimal? USB_Internal_delta { get; set; }
        public bool? USB_External_sensor { get; set; }
        public decimal? USB_External_delta { get; set; }
        public bool? Relative_Sensor { get; set; }
        public decimal? Relative_Delta { get; set; }
        public bool? Drying_Process { get; set; }
        public int? Drying_Cycles { get; set; }

        public string Test1_Name { get; set; }
        public bool? Test1_Verified { get; set; }
        public string Test2_Name { get; set; }
        public bool? Test2_Verified { get; set; }
        public string Test3_Name { get; set; }
        public bool? Test3_Verified { get; set; }
        public bool? Final_Assembly { get; set; }
        public string Final_Process_Id { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
    }
}
