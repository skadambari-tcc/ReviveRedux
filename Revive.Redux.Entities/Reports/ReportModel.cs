using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ReportModel : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public string custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

        //Machine Activity Excel
        public List<ReportModel> CheckInDetails { get; set; }
        public List<ReportModel> MachineActivity { get; set; }
        public List<ReportModel> CheckOutDetails { get; set; }
        public List<ReportModel> ActivityType { get; set; }

        //public string customFieldName1 { get; set; }
        //public string customFieldName2 { get; set; }
        //public string customFieldName3 { get; set; }
        //public string customFieldName4 { get; set; }
        //public string customFieldName5 { get; set; }
        //public string customFieldName6 { get; set; }
        //public string customFieldName7 { get; set; }
        //public string customFieldName8 { get; set; }
        //public string customFieldName9 { get; set; }
        //public string customFieldName10 { get; set; }
    }

    public class getreportofmembershipexpired : PageBasic
    {
        public double custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public string custom4 { get; set; }
        public DateTime custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }
    public class MembershipAggregate : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public int custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public DateTime custom9 { get; set; }
        public DateTime custom10 { get; set; }

    }
    public class MembershipExpiring : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public string custom4 { get; set; }
        public DateTime custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }
    public class MachineStartStop : PageBasic
    {
        public DateTime custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public int custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }

    public class LeastUsageStore : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public int custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }

    public class KillMachines : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public DateTime custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }
    public class MachineFault : PageBasic
    {
        public DateTime custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public string custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }
    public class SuccessRateAggregate : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public int custom3 { get; set; }
        public double custom4 { get; set; }
        public double custom5 { get; set; }
        public double custom6 { get; set; }
        public double custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }
    public class SuccessRateMember : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public int custom3 { get; set; }
        public double custom4 { get; set; }
        public double custom5 { get; set; }
        public double custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }
    public class SuccessRateManufacturer : PageBasic
    {
        public string custom1 { get; set; }
        public int custom2 { get; set; }
        public double custom3 { get; set; }
        public double custom4 { get; set; }
        public double custom5 { get; set; }
        public double custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }
    public class Reconciliation : PageBasic
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public DateTime custom4 { get; set; }
        public DateTime custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }

    }


    #region Machine Activity Excel Model

    public class MachineActivity_Details
    {
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public string custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }

        //Activity type
        public string ActivityType { get; set; }

        //Checkin Details
        public string MembershipType { get; set; }
        public string MembershipId { get; set; }
        public string Member { get; set; }
        public string EmailId { get; set; }
        public string DeviceManufacturer { get; set; }
        public string HowLongAgo { get; set; }
        public string pluggedIn { get; set; }
        public string PhoneNumber { get; set; }

        //Machine Activity
        public string AccumulatedRHValue { get; set; }
        public string TotalCycles { get; set; }
        public string RemainingCycles { get; set; }
        public string KPIMaximumVacuum { get; set; }
        public string KPItimetoMaximumVacuum { get; set; }
        public string KPIInitialInjectionROR { get; set; }
        public string KPIPlatenROR { get; set; }
        public string KPIMinimumChamberHumidity { get; set; }
        public string KPIMaximumChamberHumidity { get; set; }
        public string KPIMaximumCharge { get; set; }
        public string KPINumberofCycles { get; set; }
        public string KPIDuration { get; set; }
        public string KPIProcessID { get; set; }
        public string KPIMaximumAmbientTemperatur { get; set; }
        public string ProcessState { get; set; }

        //Checkout Details
        public string CheckoutDetails { get; set; }

    }

    public class MachineActivityRawData
    {
        public DateTime? ActivityDate { get; set; }
        public string ProcessId { get; set; }
        public int MachineActivityId { get; set; }
        public string MachineId { get; set; }
        public string UserName { get; set; }
        public string MemberName { get; set; }
        public string Membership { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    
        public string InvoiceNo { get; set; }
        public string MfrType { get; set; }
        public string PluginYes { get; set; }
        public string ReviveSuccessStatus { get; set; }
      
        public string ModeName { get; set; }
        public string TimeSincePeril { get; set; }
        public string CheckOutResults { get; set; }
        public string ActivityResults { get; set; }

    }

    public class Filterable
    {
    }

    public class Attributes
    {
        public string style { get; set; }
    }

    public class FooterAttributes
    {
        public string style { get; set; }
    }

    public class HeaderAttributes
    {
        public string style { get; set; }
    }

    public class RootObject
    {
        public bool encoded { get; set; }
        public string title { get; set; }
        public bool hidden { get; set; }
        public string template { get; set; }
        public string field { get; set; }
        public Filterable filterable { get; set; }
        public Attributes attributes { get; set; }
        public FooterAttributes footerAttributes { get; set; }
        public HeaderAttributes headerAttributes { get; set; }
    }

    public class ActivityReport
    {
        public DateTime? ActivityDate { get; set; }
        public string ProcessId { get; set; }
        public int MachineActivityId { get; set; }
        public string MachineId { get; set; }
        public string LocationName { get; set; }
        public Guid CreatedBy { get; set; }
        public string UserName { get; set; }
        public string MemberName { get; set; }
        public string Membership { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IsTmp { get; set; }
        public string InvoiceNo { get; set; }
        public string MfrType { get; set; }
        public string PluginYes { get; set; }
        public string ReviveSuccessStatus { get; set; }
        public string DryTime { get; set; }
        public decimal WaterRemoved { get; set; }
        public string ModeName { get; set; }
        public string TimeSincePeril { get; set; }
        public string  CheckOutResults { get; set; }
        public string ActivityResults { get; set; }
    }




    #endregion

}
