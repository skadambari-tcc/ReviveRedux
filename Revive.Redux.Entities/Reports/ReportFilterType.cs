using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ReportFilterType
    {
        public int FilterTypeId { get; set; }
        public int ReportId { get; set; }
        public string FilterType { get; set; }
        public string FitlerText { get; set; }
        public string FilterDataType { get; set; }
        public int Priority { get; set; }

        public bool IsCustomerAvailable { get; set; }
        public bool IsLocationAvailable { get; set; }
        public bool IsDateFromAvailable { get; set; }
        public bool IsDateToAvailable { get; set; }
        public bool IsDateExpireAvailable { get; set; }
        public bool IsMembershipStatusAvailable { get; set; }
        public bool IsPhoneManufacturerAvailable { get; set; }
        public bool IsOrderAvailable { get; set; }
        public bool IsMachineAvalable { get; set; }
        public bool IsModeAvailable { get; set; }
        public bool IsManufacturerAvailable { get; set; }
        public bool IsOrderMachineUnassigned { get; set; }
        public bool IsSubsidiaryAvailable { get; set; }
        public bool IsSubAgentAvailable { get; set; }
        public bool IsSubAgentMultiAvailable { get; set; }
        public bool IsLocationGroupAvailable { get; set; }
        public bool IsMultiDeviceAvailable { get; set; }
        public bool IsReportStatusAvailable { get; set; }

    }
}
