using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Revive.Redux.Entities
{
    public class ConfigureMachineHistoryModel
    {
        public int ConfigureMachineHistoryId { get; set; }
        [Display(Name = "Custom Type")]
        public int ConfigureTypeId { get; set; }

        public string ConfigureTypeValue { get; set; }

        [Display(Name = "Custom Field")]
        [Required(ErrorMessage = "The Custom Field Name is required.")]
        [Remote("CheckDuplicateCustomeFieldName", "ManageMachineSpecs", AdditionalFields = "ConfiguredValue, ConfigureTypeId,ConfigureMachineHistoryId", HttpMethod = "POST", ErrorMessage = "This Custom Field Name already exists")]
        public string ConfiguredValue { get; set; }

        public bool Status { get; set; }
        private IEnumerable<DtoList> _ConfigureTypeList;
        public IEnumerable<DtoList> ConfigureTypeList
        {
            get { return _ConfigureTypeList; }
            set { _ConfigureTypeList = value; }

        }

        //public int Config_ID { get; set; }
        //// Reason
        //[Display(Name="Custom Field 1")]
        //public string Reason_Custom1 { get; set; }
        //[Display(Name = "Custom Field 2")]
        //public string Reason_Custom2 { get; set; }
        //[Display(Name = "Custom Field 3")]
        //public string Reason_Custom3 { get; set; }
        //[Display(Name = "Custom Field 4")]
        //public string Reason_Custom4 { get; set; }
        //[Display(Name = "Custom Field 5")]
        //public string Reason_Custom5 { get; set; }
        //[Display(Name = "Custom Field 6")]
        //public string Reason_Custom6 { get; set; }
        //[Display(Name = "Custom Field 7")]
        //public string Reason_Custom7 { get; set; }
        //[Display(Name = "Custom Field 8")]
        //public string Reason_Custom8 { get; set; }
        //[Display(Name = "Custom Field 9")]
        //public string Reason_Custom9 { get; set; }

        //// Status
        //[Display(Name = "Custom Field 1")]
        //public string Status_Custom1 { get; set; }
        //[Display(Name = "Custom Field 2")]
        //public string Status_Custom3 { get; set; }
        //[Display(Name = "Custom Field 3")]
        //public string Status_Custom2 { get; set; }
        //[Display(Name = "Custom Field 4")]
        //public string Status_Custom4 { get; set; }
        //[Display(Name = "Custom Field 5")]
        //public string Status_Custom5 { get; set; }
        //[Display(Name = "Custom Field 6")]
        //public string Status_Custom6 { get; set; }
        //[Display(Name = "Custom Field 7")]
        //public string Status_Custom7 { get; set; }
        //[Display(Name = "Custom Field 8")]
        //public string Status_Custom8 { get; set; }
        //[Display(Name = "Custom Field 9")]
        //public string Status_Custom9 { get; set; }

        ////TransactionType
        //[Display(Name = "Custom Field 1")]
        //public string TransType_Custom1 { get; set; }
        //[Display(Name = "Custom Field 2")]
        //public string TransType_Custom3 { get; set; }
        //[Display(Name = "Custom Field 3")]
        //public string TransType_Custom2 { get; set; }
        //[Display(Name = "Custom Field 4")]
        //public string TransType_Custom4 { get; set; }
        //[Display(Name = "Custom Field 5")]
        //public string TransType_Custom5 { get; set; }
        //[Display(Name = "Custom Field 6")]
        //public string TransType_Custom6 { get; set; }
        //[Display(Name = "Custom Field 7")]
        //public string TransType_Custom7 { get; set; }
        //[Display(Name = "Custom Field 8")]
        //public string TransType_Custom8 { get; set; }
        //[Display(Name = "Custom Field 9")]
        //public string TransType_Custom9 { get; set; }



    }


}
